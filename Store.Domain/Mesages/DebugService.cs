using System.Diagnostics;
using System.Threading.Tasks;

namespace Store.Mesages;

public class DebugService : INotificationService
{
    public void SendNotificationCode(int code, string phoneNumber)
    {
        Debug.WriteLine($"Phone number: {phoneNumber}, verification code: {code}");
    }

    public void StartProcess(Order order)
    {
        throw new System.NotImplementedException();
    }

    public Task StartProcessAsync(Order order)
    {
        StartProcess(order);
        return Task.CompletedTask;
    }

    public Task SendNotificationCodeAsync(int code, string phoneNumber)
    {
        SendNotificationCode(code, phoneNumber);
        return Task.CompletedTask;
    }
}