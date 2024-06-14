using Avalonia.Controls;
using Mafia.ViewModels;

namespace Mafia.Headers;

public class EmptyHeader : HeaderTemplate
{
    public override Control? Build(object? param) => new();

    public override bool Match(object? data) => false;
}