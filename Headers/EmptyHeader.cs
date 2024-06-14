using Avalonia.Controls;
using Mafia.ViewModels;

namespace Mafia.Headers;

public class EmptyHeader : HeaderTemplate<ViewModelBase>
{
    protected override ViewModelBase Parent { get; } = new();
    public override Control? Build(object? param) => new();

    public override bool Match(object? data) => false;
}