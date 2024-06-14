using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;

namespace Mafia.ViewModels;

public abstract class HeaderTemplate<TVM> : HeaderTemplateBase
    where TVM : ViewModelBase
{
    protected abstract TVM Parent { get; }
}