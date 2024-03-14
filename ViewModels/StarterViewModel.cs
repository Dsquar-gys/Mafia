using System.Reactive;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class StarterViewModel : Page
    {
        private string _masterName = Statistic.MasterNameProperty;
        private bool _launchable;

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

        public ReactiveCommand<Unit, Unit> ChangeMasterName => ReactiveCommand.Create(() =>
        {
            Statistic.DefineMaster(MasterName);
        });
    }
}
