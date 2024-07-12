using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Mafia.ViewModels;
using Mafia.ViewModels.Pages;
using Mafia.Views;
using Mafia.Views.Pages;

namespace Mafia
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();

                desktop.MainWindow.TemplateApplied += (sender, args) =>
                {
                    var notifications = new WindowNotificationManager(TopLevel.GetTopLevel(desktop.MainWindow));

                    desktop.MainWindow.DataContext = new MainWindowViewModel(notifications);
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView();

                var notifications = new WindowNotificationManager(TopLevel.GetTopLevel(singleViewPlatform.MainView));

                singleViewPlatform.MainView.DataContext = new MainViewModel()
                {
                    NotificationManager = notifications
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}