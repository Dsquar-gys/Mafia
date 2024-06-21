namespace Mafia.ViewModels;

public abstract class HeaderTemplate<TVM> : HeaderTemplateBase
    where TVM : ViewModelBase
{
    public abstract TVM Parent { get; }
}