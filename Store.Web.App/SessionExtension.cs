using System.Text;
using Microsoft.AspNetCore.Http;

namespace Store.Web.App;

public static class SessionExtension
{
	private const string key = "Cart";

	public static void Set(this ISession session, Cart cart)
	{
		if (cart == null)
		{
			return;
		}

		if (cart.TotalCount == 0)
		{
			session.RemoveCart();
			return;
		}

		using (var stream = new MemoryStream())
		using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
		{
			writer.Write(cart.OrderId);
			writer.Write(cart.TotalCount);
			writer.Write(cart.TotalPrice);

			session.Set(key, stream.ToArray());
		}
	}

	public static bool TryGetCart(this ISession session, out Cart cart)
	{
		if (session.TryGetValue(key, out byte[] buffer))
		{
			using (var stream = new MemoryStream(buffer))
			using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				var orderId = reader.ReadInt32();
				var totalCount = reader.ReadInt32();
				var totalPrice = reader.ReadInt32();
				
				cart = new Cart(orderId, totalCount, totalPrice);
				return true;
			}
		}

		cart = null;
		return false;
	}
	public static void RemoveCart(this ISession session)
	{
		session.Remove(key);
	}
}