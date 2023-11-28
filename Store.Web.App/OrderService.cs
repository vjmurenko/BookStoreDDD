using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Mesages;

namespace Store.Web.App;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notificationService;

    public OrderService(
        IOrderRepository orderRepository,
        IBookRepository bookRepository,
        IHttpContextAccessor httpContextAccessor,
        INotificationService notificationService
    )
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
        _httpContextAccessor = httpContextAccessor;
        _notificationService = notificationService;
    }

    private ISession Session => _httpContextAccessor.HttpContext.Session;
    private readonly PhoneNumberUtil _phoneNumberUtil = PhoneNumberUtil.GetInstance();

    public async Task<(bool, OrderModel)> TryGetModelAsync()
    {
        var (success, order) = await TryGetOrder();
        if (success)
        {
            var orderModel = await MapOrderAsync(order);
            return (true, orderModel);
        }

        return (false, null);
    }

    public async Task<Order> GetOrder()
    {
        var (success, order) = await TryGetOrder();
        if (success)
        {
            return order;
        }

        throw new InvalidOperationException("Empty session");
    }

    private async Task<(bool, Order)> TryGetOrder()
    {
        if (Session.TryGetCart(out Cart cart))
        {
            var order = await _orderRepository.GetByIdAsync(cart.OrderId);
            return (true, order);
        }

        return (false, null);
    }

    public async Task<OrderModel> AddBook(int bookId, int count)
    {
        var (success, order) = await TryGetOrder();
        if (!success)
        {
            order = await _orderRepository.CreateAsync();
        }

        await AddOrUpdateBook(order, bookId, count);
        UpdateSession(order);

        return await MapOrderAsync(order);
    }

    private async Task AddOrUpdateBook(Order order, int bookId, int count)
    {
        var book = await _bookRepository.GetBookByIdAsync(bookId);
        if (order.Items.TryGet(bookId, out OrderItem orderItem))
        {
            orderItem.Count += count;
        }
        else
        {
            order.Items.Add(bookId, book.Price, count);
        }

        await _orderRepository.UpdateAsync(order);
    }

    private void UpdateSession(Order order)
    {
        Session.Set(new Cart(order.Id, order.TotalCount, order.TotalPrice));
    }

    public async Task DeleteBook(int bookId)
    {
        var order = await GetOrder();
        order.Items.RemoveItem(bookId);

        await _orderRepository.UpdateAsync(order);
        UpdateSession(order);
    }

    public async Task DeleteAllBooks()
    {
        var order = await GetOrder();
        order.Items.RemoveAll();
        await _orderRepository.UpdateAsync(order);
        Session.RemoveCart();
    }

    public async Task<OrderModel> SendConfirmationCodeAsync(string phoneNumber)
    {
        var order = await GetOrder();
        var orderModel = await MapOrderAsync(order);

        if (TryFormatPhone(phoneNumber, out string formattedPhone))
        {
            orderModel.PhoneNumber = formattedPhone;
            // var code = new Random().Next(1000, 10000);
            var code = 1111;
            await _notificationService.SendNotificationCodeAsync(code, formattedPhone);
            Session.SetInt32(formattedPhone, code);
        }
        else
        {
            orderModel.Errors["phoneNumber"] = "Номер телефона не соответствует формату +71234567891";
        }

        return orderModel;
    }

    public async Task<OrderModel> ConfirmPhoneAsync(string phoneNumber, int code)
    {
        var orderModel = new OrderModel();
        var codeFromSession = Session.GetInt32(phoneNumber);

        if (codeFromSession == null)
        {
            orderModel.Errors["code"] = "Пустой код, повторите отправку";
            return orderModel;
        }

        if (codeFromSession != code)
        {
            orderModel.Errors["code"] = "Введенный код не соответствует отправленному";
            return orderModel;
        }

        var order = await GetOrder();
        order.PhoneNumber = phoneNumber;

        await _orderRepository.UpdateAsync(order);
        Session.Remove(phoneNumber);

        return await MapOrderAsync(order);
    }

    public async Task SetDelivery(OrderDelivery orderDelivery)
    {
        var order = await GetOrder();
        order.Delivery = orderDelivery;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task<OrderModel> SetPaymentAsync(OrderPayment orderPayment)
    {
        var order = await GetOrder();
        order.Payment = orderPayment;
        await _orderRepository.UpdateAsync(order);
        Session.RemoveCart();

        await _notificationService.StartProcessAsync(order);
        return await MapOrderAsync(order);
    }

    private async Task<OrderModel> MapOrderAsync(Order order)
    {
        var books = await _bookRepository.GetBooksByIdsAsync(order.Items.Select(s => s.BookId));
        var orderItemModels = (from book in books
            join orderItem in order.Items on book.Id equals orderItem.BookId
            select new OrderItemModel()
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
                Count = orderItem.Count,
                Author = book.Author,
                Description = book.Description,
                Isbn = book.Isbn
            }).ToArray();

        var orderModel = new OrderModel
        {
            Id = order.Id,
            TotalCount = order.TotalCount,
            TotalPrice = order.TotalPrice,
            OrderItems = orderItemModels,
            PhoneNumber = order.PhoneNumber,
            DeliveryDescription = order.Delivery?.Description,
            PaymentDescription = order.Payment?.Description,
        };
        return orderModel;
    }

    private bool TryFormatPhone(string phoneNumber, out string formattedPhone)
    {
        try
        {
            var parsedPhone = _phoneNumberUtil.Parse(phoneNumber, "ru");
            formattedPhone = _phoneNumberUtil.Format(parsedPhone, PhoneNumberFormat.INTERNATIONAL);
            return true;
        }
        catch (NumberParseException e)
        {
            formattedPhone = null;
            return false;
        }
    }
}