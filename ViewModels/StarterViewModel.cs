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

        public string MasterName
        {
            get => _masterName;
            set
            {
                Launchable = value.Length > 0;
                this.RaiseAndSetIfChanged(ref _masterName, value);
            }
        }

        public bool Launchable
        {
            get => _launchable;
            set => this.RaiseAndSetIfChanged(ref _launchable, value);
        }
        
        #endregion

        #region + Commands +

        public ReactiveCommand<Unit, Unit> ChangeMasterName => ReactiveCommand.Create(() =>
        {
            Statistic.DefineMaster(MasterName);
        });

        #endregion
    }
}
