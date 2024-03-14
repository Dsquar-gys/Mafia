namespace Mafia.ViewModels
{
    public abstract class Page : ViewModelBase
    {
        public MainWindowViewModel ParentViewModel { get; } = MainWindowViewModel.Instance!;
    }
}
