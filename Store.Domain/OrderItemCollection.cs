using Store.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {
        private OrderDto _orderDto;
        private List<OrderItem> _items;

        public OrderItemCollection(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto));
            }
            _orderDto = orderDto;
            _items = orderDto.Items
                .Select(OrderItem.Mapper.Map)
                .ToList();
        }

        public int Count => _items.Count;

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_items as IEnumerable).GetEnumerator();
        }

        public OrderItem Get(int bookId)
        {
            if (TryGet(bookId, out OrderItem orderItem))
            {
                return orderItem;
            }
            throw new ArgumentException(nameof(bookId));
        }

        public bool TryGet(int bookId, out OrderItem orderItem)
        {
            var index = _items.FindIndex(i => i.BookId == bookId);
            if (index == -1)
            {
                orderItem = null;
                return false;
            }

            orderItem = _items[index];
            return true;
        }

        public void Add(int bookId, decimal price, int count)
        {

            if (TryGet(bookId, out _))
            {
                throw new ArgumentException("Book already exists");
            }

            var orderItemDto = OrderItem.Factory.Create(bookId, price, count, _orderDto);
            _orderDto.Items.Add(orderItemDto);

            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            _items.Add(orderItem);
        }

        public void RemoveItem(int bookId)
        {
            var index = _items.FindIndex(item => item.BookId == bookId);

            if (index == -1)
            {
                ThrowBookException("Order doesn't contain specified item", bookId);
            }
            _orderDto.Items.RemoveAt(index);
            _items.RemoveAt(index);
        }

        public void RemoveAll()
        {
            _orderDto.Items.Clear();
            _items.Clear();
        }

        private static void ThrowBookException(string message, int bookId)
        {
            var exception = new ArgumentException(message);
            exception.Data["bookId"] = bookId;
            throw exception;
        }
    }
}
