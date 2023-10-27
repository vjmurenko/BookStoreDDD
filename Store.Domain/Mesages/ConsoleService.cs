using System;
using Store.Mesages;

namespace Store;

public class ConsoleService : INotificationService
{
    public void SendNotificationCode(int code, string phoneNumber)
    {
        System.Console.WriteLine($"Code is : {code} for number: {phoneNumber}");
    }
}
