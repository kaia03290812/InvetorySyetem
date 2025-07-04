namespace InventorySyetem.Utils;

public class EmailNotifier : INotifier
{
 public void SendNotification(string recipient, string message)
 {
  Console.WriteLine($"發送簡訊至{recipient}:{message}");
 }
}