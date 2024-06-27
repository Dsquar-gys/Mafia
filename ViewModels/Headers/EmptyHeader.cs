using Mafia.ViewModels.Pages;

namespace Mafia.ViewModels.Headers;

public class EmptyHeader : HeaderVm<Page>
{
    public override Page Parent => null!;
}