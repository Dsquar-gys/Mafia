using System.Reactive;
using Avalonia.Controls.Notifications;
using ReactiveUI;

namespace Mafia.Models;

public interface ILogicalParent
{
    public INotificationManager NotificationManager { get; }
    
    public ReactiveCommand<Unit, Unit> MoveNextCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }
}