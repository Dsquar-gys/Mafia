using Mafia.ViewModels;

namespace Mafia.Headers;

public class TeamsConfigHeader(TeamsConfigViewModel parent) : HeaderTemplate<TeamsConfigViewModel>
{
    public override TeamsConfigViewModel Parent { get; } = parent;
}