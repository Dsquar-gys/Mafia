using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls.Notifications;
using DynamicData;
using Mafia.Models;
using Mafia.Models.Enums;
using Mafia.ViewModels.Pages;
using ReactiveUI;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace Mafia.ViewModels
{
    public class MainViewModel : ViewModelBase, ILogicalParent
    {
        #region + Private Fields +

        private readonly Page[] _pages;
        private Page? _currentPage;
        private int _pageIndex;
        private bool _canEarlyEndSession;

        private IDisposable? _subscriptionForward;
        private IDisposable? _subscriptionBackward;

        #endregion

        #region + Properties +

        public Page? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public bool CanEarlyEndSession
        {
            get => _canEarlyEndSession;
            set => this.RaiseAndSetIfChanged(ref _canEarlyEndSession, value);
        }

        private Subject<bool> CanMoveForwardCore { get; }

        private Subject<bool> CanMoveBackCore { get; }
        
        public required INotificationManager NotificationManager { get; init; }

        #endregion
        
        public MainViewModel()
        {
            _pages =
            [
                new StarterViewModel(this),
                new LobbyConfigViewModel(this),
                new TeamsConfigViewModel(this),
                new RoundViewModel(this)
            ];

            CanMoveForwardCore = new Subject<bool>();
            CanMoveBackCore = new Subject<bool>();
            
            // Update movability for current page
            this.WhenAnyValue(vm => vm.CurrentPage)
                .Buffer(2, 1)
                .Select(t => (Previous: t[0], Current: t[1]))
                .Subscribe(pageChange =>
                {
                    pageChange.Previous?.OnDeactivate();
                    pageChange.Current!.OnActivate();
                    
                    _subscriptionForward?.Dispose();
                    _subscriptionForward = pageChange.Current.CanMoveForward.Subscribe(CanMoveForwardCore.OnNext);
                    
                    _subscriptionBackward?.Dispose();
                    _subscriptionBackward = pageChange.Current.CanMoveBack.Subscribe(CanMoveBackCore.OnNext);

                    CanEarlyEndSession = pageChange.Current is RoundViewModel;
                });
            
            CurrentPage = _pages[_pageIndex];
            
            MoveNextCommand = ReactiveCommand.Create(GetNextPage, CanMoveForwardCore);
            MoveBackCommand = ReactiveCommand.Create(GetPreviousPage, CanMoveBackCore);

            EarlyEndSessionCommand = ReactiveCommand.Create(() => EndSession(GameOver.None));
        }

        #region + Commands +

        public ReactiveCommand<Unit, Unit> MoveNextCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }
        public ReactiveCommand<Unit, Unit> EarlyEndSessionCommand { get; }

        #endregion

        #region + Command Methods +

        private void GetNextPage() => CurrentPage = _pages[++_pageIndex];
        private void GetPreviousPage() => CurrentPage = _pages[--_pageIndex];

        private void SetPageTo(int index)
        {
            _pageIndex = index;
            CurrentPage = _pages[index];
        }

        public void EndSession(GameOver sessionResult)
        {
            var title = sessionResult switch
            {
                GameOver.None => "It's draw",
                GameOver.BlackWins => "Black wins",
                GameOver.RedWins => "Red wins",
                _ => throw new InvalidDataException()
            };
            
            // List of mafia names
            var mafias = Statistic.Players.Items.Where(x => x.Role is GameRole.Mafia).Select(x => x.Nickname + ", ")
                .Aggregate(string.Empty, (current, mafia) => current + mafia);

            var message =
                $"Detective: {Statistic.Players.Items.FirstOrDefault(x => x.Role is GameRole.Detective)?.Nickname}\n" +
                $"Don: {Statistic.Players.Items.FirstOrDefault(x => x.Role is GameRole.Don)?.Nickname}\n" +
                $"Mafia: {mafias}";
            
            var notification = new Notification(title, message[..^2]);
            
            // Show session result
            NotificationManager.Show(notification);
            
            // Clear statistic
            Statistic.Players.Clear();
            Statistic.DefineMaster(string.Empty);

            SetPageTo(0);
            foreach (var page in _pages)
            {
                page.OnReset();
            }
        }

        #endregion
    }
}