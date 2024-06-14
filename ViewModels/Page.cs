using Mafia.Models;

namespace Mafia.ViewModels
{
    public abstract class Page : ViewModelBase
    {
        public abstract SessionStage Stage { get; }
        public abstract HeaderTemplate Header { get; init; }
    }
}
