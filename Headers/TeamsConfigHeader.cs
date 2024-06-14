using Avalonia.Controls;
using Mafia.Models;
using Mafia.ViewModels;

namespace Mafia.Headers;

public class TeamsConfigHeader : HeaderTemplate<TeamsConfigViewModel>
{
    protected override TeamsConfigViewModel Parent { get; }

    public TeamsConfigHeader(TeamsConfigViewModel parent)
    {
        Parent = parent;
    }
    
    public override Control? Build(object? param)
    {
        return new TeamsConfigHeaderView {DataContext = Parent};
    }

    public override bool Match(object? data) => data is SessionStage.Teaming;
}