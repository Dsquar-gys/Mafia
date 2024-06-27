using System;
using System.Reactive;
using Mafia.Models;
using Mafia.ViewModels.Headers;
using ReactiveUI;

namespace Mafia.ViewModels.Pages
{
    public class StarterViewModel : Page
    {
        #region + Private fields +
        
        private string _masterName = string.Empty;
        
        #endregion
        
        #region + Properties +

        public override HeaderVMBase Header { get; init; } = new EmptyHeader();
        
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

        public StarterViewModel()
        {
            CanMoveBack = this.WhenAnyValue(vm => vm.Header, header => header is not EmptyHeader);
            CanMoveForward = this.WhenAnyValue(vm => vm.MasterName.Length, length => length > 0);
        }
        
        #region + Commands +

        public ReactiveCommand<Unit, Unit> ChangeMasterName => ReactiveCommand.Create(() =>
        {
            Statistic.DefineMaster(MasterName);
        });

        #endregion
    }
}
