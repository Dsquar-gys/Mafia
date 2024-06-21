using Mafia.ViewModels;

namespace Mafia.Headers;

public class EmptyHeader : HeaderTemplate<ViewModelBase>
{
    public override ViewModelBase Parent { get; } = new();
}