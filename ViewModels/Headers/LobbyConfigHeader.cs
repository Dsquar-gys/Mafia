using Mafia.ViewModels.Pages;

namespace Mafia.ViewModels.Headers;

public class LobbyConfigHeader(LobbyConfigViewModel parent) : HeaderVm<LobbyConfigViewModel>
{
    public override LobbyConfigViewModel Parent { get; } = parent;
}