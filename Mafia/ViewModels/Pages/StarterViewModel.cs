using System;
using System.Reactive;
using Mafia.Models;
using Mafia.ViewModels.Headers;
using ReactiveUI;

namespace Mafia.ViewModels.Pages
{
    public sealed class StarterViewModel : Page
    {
        #region + Private fields +
        
        private string _masterName = string.Empty;
        
        #endregion
        
        #region + Properties +

        public override HeaderVMBase Header { get; init; }
        
        public override IObservable<bool> CanMoveForward { get; }
        
        public override IObservable<bool> CanMoveBack { get; }

        /// <summary>
        /// Name of game master
        /// </summary>
        public string MasterName
        {
            get => _masterName;
            set => this.RaiseAndSetIfChanged(ref _masterName, value);
        }
        
        #endregion

        public StarterViewModel(ILogicalParent parent) : base(parent)
        {
            Header = new EmptyHeader();
            
            // Permanent false
            CanMoveBack = this.WhenAnyValue(vm => vm.Header, header => header is not EmptyHeader);
            // On game master name length
            CanMoveForward = this.WhenAnyValue(vm => vm.MasterName, name => name.Length > 0);
        }
        
        #region + Commands +

        public ReactiveCommand<Unit, Unit> ChangeMasterName => ReactiveCommand.Create(() =>
        {
            Statistic.DefineMaster(MasterName);
        });

        #endregion
        
        #region + Methods +
        
        public override void OnActivate() { }

        public override void OnDeactivate() { }

        public override void OnReset()
        {
            MasterName = string.Empty;
        }

        #endregion
    }
}
