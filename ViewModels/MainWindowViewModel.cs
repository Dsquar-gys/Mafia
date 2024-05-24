using System;
using System.Reactive;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly Page[] _pages;
        
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
            
            _pages = new Page[]
            {
                new StarterViewModel(),
                new LobbyConfigViewModel(),
                new TeamsConfigViewModel(),
                new RoundViewModel()
            };

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

        private void SetPage(ImportantPage page)
        {
            var pageIndex = (int)page;
            if (pageIndex >= 0 && pageIndex < _pages.Length)
            {
                _pageIndex = pageIndex;
                CurrentPage = _pages[_pageIndex];
            }
            else throw new IndexOutOfRangeException("Wrong page index...");
        }

        public void ResetSession()
        {
            foreach (var page in _pages)
                page.ResetPage();
            
            SetPage(ImportantPage.StartingPage);
        }
    }
}
