using Mafia.Models;

namespace Mafia.ViewModels
{
    public abstract class Page : ViewModelBase
    {
        public abstract HeaderTemplateBase Header { get; init; }
    }
}
