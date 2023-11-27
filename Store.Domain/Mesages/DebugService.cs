﻿using System.Diagnostics;

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
}