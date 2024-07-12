using Mafia.ViewModels.Pages;

namespace Mafia.ViewModels.Headers;

/// <summary>
/// Header base for specific page
/// </summary>
/// <typeparam name="TVM">Type of <see cref="Page"/> heir</typeparam>
public abstract class HeaderVm<TVM> : HeaderVMBase
    where TVM : Page
{
    public abstract TVM Parent { get; }
}