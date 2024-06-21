using Mafia.ViewModels;

namespace Mafia.Headers;

public class RoundHeader(RoundViewModel parent) : HeaderTemplate<RoundViewModel>
{
    public override RoundViewModel Parent { get; } = parent;
}