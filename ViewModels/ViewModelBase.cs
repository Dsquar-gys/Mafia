using System;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class ViewModelBase : ReactiveObject, ILogicalParent
    {
        // TODO DI implementation
        public IServiceProvider Services { get; }
    }
}
