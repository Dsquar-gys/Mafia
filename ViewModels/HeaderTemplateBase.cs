using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;

namespace Mafia.ViewModels;

public abstract class HeaderTemplateBase : ReactiveObject, IDataTemplate
{
    public abstract Control? Build(object? param);

    public abstract bool Match(object? data);
}