using Mafia.ViewModels.Pages;

namespace Mafia.ViewModels.Headers;

public class RoundHeader(RoundViewModel parent) : HeaderVm<RoundViewModel>
{
    public override RoundViewModel Parent { get; } = parent;
}