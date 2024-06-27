using System;
using System.Reactive.Subjects;
using System.Windows.Input;
using Mafia.ViewModels.Pages;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region + Private Fields +

        private readonly Page[] _pages;
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
        
        #endregion
        
        public MainWindowViewModel()
        {
            _pages =
            [
                new StarterViewModel(),
                new LobbyConfigViewModel(),
                new TeamsConfigViewModel(),
                new RoundViewModel()
            ];

            CanMoveForwardCore = new Subject<bool>();
            CanMoveBackCore = new Subject<bool>();

            _currentPage = _pages[_pageIndex];
            
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

        public ICommand MoveNextCommand { get; }
        public ICommand MoveBackCommand { get; }

        #endregion

        #region + Command Methods +

        private void GetNextPage() => CurrentPage = _pages[++_pageIndex];
        private void GetPreviousPage() => CurrentPage = _pages[--_pageIndex];

        #endregion
    }
}
