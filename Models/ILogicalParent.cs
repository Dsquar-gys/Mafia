using Avalonia.Controls.Notifications;
using Mafia.Models.Enums;

namespace Mafia.Models;

public interface ILogicalParent
{
    public INotificationManager NotificationManager { get; }
    public void EndSession(GameOver sessionResult);
}