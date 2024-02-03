namespace Mafia.ViewModels
{
    public abstract class Page : ViewModelBase
    {
        public MainWindowViewModel ParentVM { get; }
        public Page() => ParentVM = MainWindowViewModel.Instance;
    }
}
