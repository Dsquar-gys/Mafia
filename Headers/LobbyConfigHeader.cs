using Mafia.ViewModels;

namespace Mafia.Headers;

public class LobbyConfigHeader(LobbyConfigViewModel parent) : HeaderTemplate<LobbyConfigViewModel>
{
    public override LobbyConfigViewModel Parent { get; } = parent;
}