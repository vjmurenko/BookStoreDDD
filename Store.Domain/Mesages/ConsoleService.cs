namespace Store.Mesages;

public class ConsoleService : INotificationService
{
    public void SendNotificationCode(int code, string phoneNumber)
    {
        System.Console.WriteLine($"Code is : {code} for number: {phoneNumber}");
    }
}
