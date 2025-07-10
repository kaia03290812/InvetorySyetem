﻿using InventorySyetem.Utils;

namespace InventorySyetem.Services;

public class NotificationService
{
 private readonly INotifier _notifier;

 public NotificationService(INotifier notifier)
 {
  _notifier = notifier;
 }

 public void NotifyUser(string recipient, string message)
 {
  Console.WriteLine($"準備通知用戶:{recipient}");
  _notifier.SendNotification(recipient, message);
  Console.WriteLine("通知操作完成");
 }
}