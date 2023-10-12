using System;
using System.Collections.Generic;
using System.Linq;

namespace Store;

public class Order
{
	public int Id { get; set; }
	public decimal TotalPrice => _items.Sum(i => i.Price * i.Count);

	public int TotalCount => _items.Sum(i => i.Count);

	private List<OrderItem> _items;

	public IReadOnlyCollection<OrderItem> Items => _items;

	public Order(int id, IEnumerable<OrderItem> items)
	{
		if (items == null)
		{
			throw new ArgumentNullException(nameof(items));
		}
		Id = id;
		_items = new List<OrderItem>(items);
	}

	public void AddItem(Book book, int count)
	{
		if (book == null)
		{
			throw new ArgumentNullException(nameof(book));
		}

		var item = _items.FirstOrDefault(i => i.Id == book.Id);
		if (item == null)
		{
			_items.Add(new OrderItem(book.Id, book.Price, count));
		}
		else
		{
			item.Count++;
		}
	}

	public void DeleteItem(int id)
	{
		var item = _items.First(i => i.Id == id);
		if (item.Count > 1)
		{
			item.Count--;
		}
		else
		{
			_items.Remove(item);
		}
	}

	public void DeleteAll()
	{
		_items.Clear();
	}


}