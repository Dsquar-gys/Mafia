using System;
using System.Reactive;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly Page[] _pages = new Page[]{
            new StarterViewModel(),
            new LobbyConfigViewModel(),
            new TeamsConfigViewModel()};
        
        private Page _currentPage;
        private int _pageIndex;

        #endregion

        #region Properties

        public static MainWindowViewModel? Instance { get; private set; }
        public Page CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }
        
        public IObservable<bool> CanMoveForward { get; }
        
        public IObservable<bool> CanMoveBack { get; }
        
        #endregion 

        public MainWindowViewModel()
        {
            Instance ??= this;

            CanMoveForward = this.WhenAnyValue(
                x => x._pageIndex, x => x._pages.Length,
                (cur, len) => cur < len - 1);
            CanMoveBack = this.WhenAnyValue(x => x._pageIndex,
                index => index > 0);

            _currentPage = _pages[_pageIndex];
        }

        #region Commands

        public ReactiveCommand<Unit, Unit> MoveNextCommand => ReactiveCommand.Create(GetNextPage, CanMoveForward);
        public ReactiveCommand<Unit, Unit> MoveBackCommand => ReactiveCommand.Create(GetPreviousPage, CanMoveBack);

        #endregion

        #region Command Methods

        private void GetNextPage() => CurrentPage = _pages[++_pageIndex];

        private void GetPreviousPage() => CurrentPage = _pages[--_pageIndex];

        #endregion
    }
}
