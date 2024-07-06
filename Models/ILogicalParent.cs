using Avalonia.Controls.Notifications;

namespace Mafia.Models;

public interface ILogicalParent
{
    public INotificationManager NotificationManager { get; }
}