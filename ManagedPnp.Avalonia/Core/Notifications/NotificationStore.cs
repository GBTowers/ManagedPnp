using System.Threading.Tasks;

#if WINDOWS
using Microsoft.Toolkit.Uwp.Notifications;
#endif
namespace ManagedPnp.Avalonia.Core.Notifications;

public class NotificationStore : INotificationStore
{
    public void ShowNotification(Notification notification)
    {
        Task.Run( () =>
        {
#if WINDOWS
            new ToastContentBuilder()
                .AddText(notification.Header)
                .AddText(notification.Message)
                .AddAttributionText(notification.Footer)
                .Show();
#endif
        });
    }
}