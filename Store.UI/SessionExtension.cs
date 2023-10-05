using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Store.UI.Models;

namespace Store.Web;

public static class SessionExtension
{
    private const string key = "Cart";

    public static void Set(this ISession session, Cart cart)
    {
        if (cart == null)
        {
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
            using (var reader = new BinaryReader(stream,Encoding.UTF8, true ))
            {
                var orderId = reader.ReadInt32();
                cart = new Cart(orderId)
                {
                    TotalCount = reader.ReadInt32(),
                    TotalPrice = reader.ReadInt32()
                };
                return true;
            }
        }

        cart = null;
        return false;
    }
}