using Mafia.Models;
using Mafia.ViewModels.Commands;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class StarterViewModel : Page
    {
        private string _masterName = Statistic.MasterNameProperty;
        private bool _launchable = false;

        public StarterViewModel() : base()
        {
            ChangeMasterName = new DelegateCommand(parameter =>
            {
                Statistic.DefineMaster(MasterName);
            });
        }

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

        public DelegateCommand ChangeMasterName { get; }
    }
}
