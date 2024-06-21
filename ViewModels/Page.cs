using System;

namespace Mafia.ViewModels
{
    public abstract class Page : ViewModelBase
    {
        public abstract HeaderTemplateBase Header { get; init; }

        public abstract IObservable<bool> CanMoveForward { get; }
        
        public abstract IObservable<bool> CanMoveBack { get; }
    }
}
