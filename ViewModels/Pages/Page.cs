using System;
using Mafia.ViewModels.Headers;

namespace Mafia.ViewModels.Pages
{
    /// <summary>
    /// Base view model for UI page
    /// </summary>
    public abstract class Page : ViewModelBase
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
    }
}
