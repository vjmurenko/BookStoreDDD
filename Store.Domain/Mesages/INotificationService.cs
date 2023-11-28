using System.Threading.Tasks;

namespace Store.Mesages;

public interface INotificationService
{
    void SendNotificationCode(int code, string phoneNumber);

    void StartProcess(Order order);
    
    Task SendNotificationCodeAsync(int code, string phoneNumber);
    Task StartProcessAsync(Order order);
}