namespace InventorySyetem.Utils;

public interface INotifier
{
 void SendNotification(string recipient, string message);
}