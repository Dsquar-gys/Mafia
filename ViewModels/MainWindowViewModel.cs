using Mafia.ViewModels.Commands;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly Page[] _pages = new Page[] { new StarterViewModel(), new LobbyConfigViewModel() };
        private Page _currentPage;
        private int _pageIdex = 0;

        #endregion

        #region Properties

        public static MainWindowViewModel Instance { get; private set; }
        public Page CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        #endregion 

        public MainWindowViewModel()
        {
            Instance = Instance ?? this;

            MoveNextCommand = new DelegateCommand(GetNextPage, OnCanMoveForward);
            MovePreviousCommand = new DelegateCommand(GetPreviousPage, OnCanMoveBackward);

            CurrentPage = _pages[_pageIdex];
        }

        #region Commands

        public DelegateCommand MoveNextCommand { get; }
        public DelegateCommand MovePreviousCommand { get; }

        #endregion

        #region Command Methods

        private void GetNextPage(object parameter) => CurrentPage = _pages[++_pageIdex];

        private void GetPreviousPage(object parameter) => CurrentPage = _pages[--_pageIdex];

        private bool OnCanMoveForward(object parameter) => _pageIdex < _pages.Length - 1;

        private bool OnCanMoveBackward(object parameter) => _pageIdex > 0;

        #endregion
    }
}
