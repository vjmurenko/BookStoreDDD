using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Mesages;
using System.Text.RegularExpressions;

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

    public bool TryGetModel(out OrderModel orderModel)
    {
        if (TryGetOrder(out var order))
        {
            orderModel = MapOrder(order);
            return true;
        }

        orderModel = null;
        return false;
    }

    public Order GetOrder()
    {
        if (TryGetOrder(out Order order))
        {
            return order;
        }

        throw new InvalidOperationException("Empty session");
    }

    private bool TryGetOrder(out Order order)
    {
        if (Session.TryGetCart(out Cart cart))
        {
            order = _orderRepository.GetById(cart.OrderId);
            return true;
        }

        order = null;
        return false;
    }

    public void AddBook(int bookId, int count)
    {
        if (!TryGetOrder(out Order order))
        {
            order = _orderRepository.Create();
        }

        UpdateSession(order);
        AddOrUpdateBook(order, bookId, count);

        _orderRepository.Update(order);
    }

    private void AddOrUpdateBook(Order order, int bookId, int count)
    {
        var book = _bookRepository.GetBookById(bookId);
        if (order.Items.TryGet(bookId, out OrderItem orderItem))
        {
            orderItem.Count++;
        }
        else
        {
            order.Items.Add(bookId, book.Price, count);
        }
    }

    private void UpdateSession(Order order)
    {
        Session.Set(new Cart(order.Id, order.TotalCount, order.TotalPrice));
    }

    public void DeleteBook(int bookId)
    {
        var order = GetOrder();
        order.Items.RemoveItem(bookId);
        UpdateSession(order);
    }

    public void DeleteAllBooks()
    {
        var order = GetOrder();
        order.Items.RemoveAll();
        Session.RemoveCart();
    }

    public OrderModel SendConfirmationCode(string phoneNumber)
    {
        var order = GetOrder();
        var orderModel = MapOrder(order);

        if (TryFormatPhone(phoneNumber, out string formattedPhone))
        {
            orderModel.PhoneNumber = formattedPhone;
            // var code = new Random().Next(1000, 10000);
            var code = 1111;
            _notificationService.SendNotificationCode(code, formattedPhone);
            Session.SetInt32(formattedPhone, code);
        }
        else
        {
            orderModel.Errors["phoneNumber"] = "Номер телефона не соответствует формату +71234567891";
        }

        return orderModel;
    }

    public OrderModel ConfirmPhone(string phoneNumber, int code)
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

        var order = GetOrder();
        order.PhoneNumber = phoneNumber;

        _orderRepository.Update(order);
        Session.Remove(phoneNumber);

        return MapOrder(order);
    }

    public void SetDelivery(OrderDelivery orderDelivery)
    {
        var order = GetOrder();
        order.Delivery = orderDelivery;
        _orderRepository.Update(order);
    }

    public void SetPayment(OrderPayment orderPayment)
    {
        var order = GetOrder();
        order.Payment = orderPayment;
        _orderRepository.Update(order);
        Session.RemoveCart();
    }

    private static bool IsValidPhone(string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return false;
        }

        phone = phone.Replace(" ", "").Replace("-", "");

        return Regex.IsMatch(phone, @"\+?\d{11}$");
    }

    private OrderModel MapOrder(Order order)
    {
        var books = _bookRepository.GetBooksByIds(order.Items.Select(s => s.BookId));
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
            PhoneNumber = order.PhoneNumber
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