namespace Store.Mesages;

public interface INotificationService
{
    void SendNotificationCode(int code, string phoneNumber);
}