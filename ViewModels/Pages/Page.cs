using System;
using Mafia.Models;
using Mafia.ViewModels.Headers;

namespace Mafia.ViewModels.Pages
{
    /// <summary>
    /// Base view model for UI page
    /// </summary>
    public abstract class Page(ILogicalParent parent) : ViewModelBase
    {
        /// <summary>
        /// Header of the page
        /// </summary>
        public abstract HeaderVMBase Header { get; init; }

        /// <summary>
        /// Observable option to go to the next page
        /// </summary>
        public abstract IObservable<bool> CanMoveForward { get; }
        
        /// <summary>
        /// Observable option to go to the previous page
        /// </summary>
        public abstract IObservable<bool> CanMoveBack { get; }

        protected ILogicalParent Parent { get; init; } = parent;

        public abstract void OnActivate();
        public abstract void OnDeactivate();
        public abstract void OnReset();

        //protected abstract void Initialize();
    }
}
