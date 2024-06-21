using System;
using System.Windows.Input;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region + Private Fields +

        private readonly Page[] _pages;
        private Page _currentPage;
        private int _pageIndex;

        #endregion

        #region + Properties +

        public Page CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private int PageIndex
        {
            get => _pageIndex;
            set => this.RaiseAndSetIfChanged(ref _pageIndex, value);
        }
        
        public IObservable<bool> CanMoveForward { get; }
        
        public IObservable<bool> CanMoveBack { get; }
        
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

            CanMoveForward = this.WhenAnyValue(
                x => x.PageIndex, x => x._pages.Length,
                (cur, len) => cur < len - 1);
            
            CanMoveBack = this.WhenAnyValue(x => x.PageIndex,
                index => index > 0);
            
            MoveNextCommand = ReactiveCommand.Create(GetNextPage, CanMoveForward);
            MoveBackCommand = ReactiveCommand.Create(GetPreviousPage, CanMoveBack);
            
            _currentPage = _pages[_pageIndex];
        }

        #region + Commands +

        public ICommand MoveNextCommand { get; }
        public ICommand MoveBackCommand { get; }

        #endregion

        #region + Command Methods +

        private void GetNextPage() => CurrentPage = _pages[++PageIndex];

        private void GetPreviousPage() => CurrentPage = _pages[--PageIndex];

        #endregion
    }
}
