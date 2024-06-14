using Avalonia.Controls;
using Mafia.Models;
using Mafia.ViewModels;

namespace Mafia.Headers;

public class LobbyConfigHeader : HeaderTemplate
{
    public LobbyConfigViewModel Parent { get; }

    public LobbyConfigHeader(LobbyConfigViewModel parent)
    {
        Parent = parent;
    }
    
    public override Control? Build(object? param)
    {
        return new LobbyConfigHeaderView {DataContext = Parent};
    }

    public override bool Match(object? data) => data is SessionStage.PlayerLineUp;
}