using System;
using System.Reactive;
using System.Reactive.Subjects;
using Avalonia.Controls.Notifications;
using Mafia.Models;
using Mafia.ViewModels.Pages;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ILogicalParent
    {
        #region + Private Fields +

        private readonly Lazy<Page>[] _pages;
        private Page _currentPage;
        private int _pageIndex;

        private IDisposable? _subscriptionForward;
        private IDisposable? _subscriptionBackward;

        #endregion

        #region + Properties +

        public Page CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private Subject<bool> CanMoveForwardCore { get; }

        private Subject<bool> CanMoveBackCore { get; }
        
        public required INotificationManager NotificationManager { get; init; }

        #endregion
        
        public MainWindowViewModel()
        {
            _pages =
            [
                new Lazy<Page>(() => new StarterViewModel(this)),
                new Lazy<Page>(() => new LobbyConfigViewModel(this)),
                new Lazy<Page>(() => new TeamsConfigViewModel(this)),
                new Lazy<Page>(() => new RoundViewModel(this))
            ];

            CanMoveForwardCore = new Subject<bool>();
            CanMoveBackCore = new Subject<bool>();

            _currentPage = _pages[_pageIndex].Value;
            
            // Update movability for current page
            this.WhenAnyValue(vm => vm.CurrentPage)
                .Subscribe(page =>
                {
                    _subscriptionForward?.Dispose();
                    _subscriptionForward = page.CanMoveForward.Subscribe(CanMoveForwardCore.OnNext);
                    
                    _subscriptionBackward?.Dispose();
                    _subscriptionBackward = page.CanMoveBack.Subscribe(CanMoveBackCore.OnNext);
                });
            
            MoveNextCommand = ReactiveCommand.Create(GetNextPage, CanMoveForwardCore);
            MoveBackCommand = ReactiveCommand.Create(GetPreviousPage, CanMoveBackCore);
        }

        #region + Commands +

        public ReactiveCommand<Unit, Unit> MoveNextCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }

        #endregion

        #region + Command Methods +

        private void GetNextPage() => CurrentPage = _pages[++_pageIndex].Value;
        private void GetPreviousPage() => CurrentPage = _pages[--_pageIndex].Value;

        #endregion
    }
}