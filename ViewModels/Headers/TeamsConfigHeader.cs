using Mafia.ViewModels.Pages;

namespace Mafia.ViewModels.Headers;

public class TeamsConfigHeader(TeamsConfigViewModel parent) : HeaderVm<TeamsConfigViewModel>
{
    public override TeamsConfigViewModel Parent { get; } = parent;
}