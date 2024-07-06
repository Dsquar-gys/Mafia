using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls.Notifications;
using DynamicData;
using Mafia.Models;
using Mafia.Models.Enums;
using Mafia.ViewModels.Pages;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ILogicalParent
    {
        #region + Private Fields +

        private readonly Page[] _pages;
        private Page? _currentPage;
        private int _pageIndex;
        private bool _canEndSession;

        private IDisposable? _subscriptionForward;
        private IDisposable? _subscriptionBackward;

        #endregion

        #region + Properties +

        public Page? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public bool CanEndSession
        {
            get => _canEndSession;
            set => this.RaiseAndSetIfChanged(ref _canEndSession, value);
        }

        private Subject<bool> CanMoveForwardCore { get; }

        private Subject<bool> CanMoveBackCore { get; }
        
        public required INotificationManager NotificationManager { get; init; }

        #endregion
        
        public MainWindowViewModel()
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
                    _subscriptionForward = pageChange.Current.CanMoveForward.Subscribe(x => CanMoveForwardCore.OnNext(x));
                    
                    _subscriptionBackward?.Dispose();
                    _subscriptionBackward = pageChange.Current.CanMoveBack.Subscribe(CanMoveBackCore.OnNext);

                    CanEndSession = pageChange.Current is RoundViewModel;
                });
            
            CurrentPage = _pages[_pageIndex];
            
            MoveNextCommand = ReactiveCommand.Create(GetNextPage, CanMoveForwardCore);
            MoveBackCommand = ReactiveCommand.Create(GetPreviousPage, CanMoveBackCore);

            EndSessionCommand = ReactiveCommand.Create(EndSession);
        }

        #region + Commands +

        public ReactiveCommand<Unit, Unit> MoveNextCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }
        public ReactiveCommand<Unit, Unit> EndSessionCommand { get; }

        #endregion

        #region + Command Methods +

        private void GetNextPage() => CurrentPage = _pages[++_pageIndex];
        private void GetPreviousPage() => CurrentPage = _pages[--_pageIndex];

        private void SetPageTo(int index)
        {
            _pageIndex = index;
            CurrentPage = _pages[index];
        }

        private void EndSession()
        {
            Statistic.CreateReport(GameOver.None);
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