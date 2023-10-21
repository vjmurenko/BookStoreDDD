using System;
using System.Collections.Generic;
using System.Linq;

namespace Store;

public class Order
{
	public int Id { get; set; }
	
	private List<OrderItem> _items;
	
	public IReadOnlyCollection<OrderItem> Items => _items;
	
	public decimal TotalPrice => _items.Sum(i => i.Price * i.Count);

	public int TotalCount => _items.Sum(i => i.Count);

	public Order(int id, IEnumerable<OrderItem> items)
	{
		if (items == null)
		{
			throw new ArgumentNullException(nameof(items));
		}
		Id = id;
		_items = new List<OrderItem>(items);
	}

	public OrderItem GetItem(int bookId)
	{
		var index = _items.FindIndex(i => i.BookId == bookId);
		if (index == -1)
		{
			ThrowBookException("Book not found", bookId);
		}

		return _items[index];
	}

	public void AddOrUpdateItem(Book book, int count)
	{
		if (book == null)
		{
			throw new ArgumentNullException(nameof(book));
		}
		
		var index = _items.FindIndex(i => i.BookId == book.Id);
		if (index == -1)
		{
			_items.Add(new OrderItem(book.Id, book.Price, count));
		}
		else
		{
			_items[index].Count += count;
		}
	}

	public void RemoveItem(int bookId)
	{
		var index = _items.FindIndex(item => item.BookId == bookId);

		if (index == -1)
		{
			ThrowBookException("Order doesn't contain specified item", bookId);
		}
		_items.RemoveAt(index);
	}

	public void DeleteAll()
	{
		_items.Clear();
	}

	private static void ThrowBookException(string message, int bookId)
	{
		var exception = new ArgumentException(message);
		exception.Data["bookId"] = bookId;
		throw exception;
	}
	
}