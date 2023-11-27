using System;
using System.Net.Mail;
using System.Text;

namespace Store.Mesages;

public class ConsoleService : INotificationService
{
    public void SendNotificationCode(int code, string phoneNumber)
    {
        System.Console.WriteLine($"Code is : {code} for number: {phoneNumber}");
    }

    public void StartProcess(Order order)
    {
        // using (var client = new SmtpClient())
        // {
        //     var message = new MailMessage("from@at.my.domain", "to@at.my.domain");
        //     message.Subject = "Order №: " + order.Id;
        //     var builder = new StringBuilder();
        //     foreach (var orderItem in order.Items)
        //     {
        //         builder.Append($"{orderItem.BookId}, {orderItem.Count}");
        //         builder.AppendLine();
        //     }
        //
        //     client.Send(message);
        // }
        
        Console.WriteLine($"Order id: {order.Id}");
        Console.WriteLine($"Delivery: {order.Delivery.Description}");
        Console.WriteLine($"Payment: {order.Payment.Description}");
    }
}