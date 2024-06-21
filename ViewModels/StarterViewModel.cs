using System;
using System.Reactive;
using Mafia.Headers;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class StarterViewModel : Page
    {
        #region + Private fields +
        
        private string _masterName = "";
        private bool _launchable;
        
        #endregion
        
        #region + Properties +

        public override HeaderTemplateBase Header { get; init; } = new EmptyHeader();
        
        public bool Launchable
        {
            get => _launchable;
            set => this.RaiseAndSetIfChanged(ref _launchable, value);
        }
        
        public override IObservable<bool> CanMoveForward { get; }
        
        public override IObservable<bool> CanMoveBack { get; }

        public string MasterName
        {
            get => _masterName;
            set
            {
                Launchable = value.Length > 0;
                this.RaiseAndSetIfChanged(ref _masterName, value);
            }
        }
        
        #endregion

        public StarterViewModel()
        {
            CanMoveBack = this.WhenAnyValue(vm => vm.Header, header => header is not EmptyHeader);
            CanMoveForward = this.WhenAnyValue(vm => vm.Launchable);
        }
        
        #region + Commands +

        public ReactiveCommand<Unit, Unit> ChangeMasterName => ReactiveCommand.Create(() =>
        {
            Statistic.DefineMaster(MasterName);
        });

        #endregion
    }
}
