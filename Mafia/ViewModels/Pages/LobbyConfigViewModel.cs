using System;
using System.Linq;
using System.Reactive;
using DynamicData;
using DynamicData.Binding;
using Mafia.Models;
using Mafia.Models.Enums;
using Mafia.ViewModels.Headers;
using ReactiveUI;

namespace Mafia.ViewModels.Pages
{
    public sealed class LobbyConfigViewModel : Page
    {
        #region + Private fields +
        
        private int _playerIndexer;
        private readonly Random _random = new();
        private IDisposable _connectionSubscription;

        #endregion
        
        #region + Properties +

        public override HeaderVMBase Header { get; init; }

        public override IObservable<bool> CanMoveForward { get; }
        
        public override IObservable<bool> CanMoveBack { get; }
        
        public IObservableCollection<Player> Players { get; } = new ObservableCollectionExtended<Player>();
        
        private string GetRandomName
        {
            get
            {
                var existing = Players.Select(x => x.Nickname).ToArray();
                var total = Enum.GetNames(typeof(DefaultName));

                var rest = total.Except(existing).ToArray();

                if (rest.Length != 0) return rest[_random.Next(rest.Length)];
                
                Console.WriteLine("Out of default names...");
                return "NickName";
            }
        }
        
        #endregion
        
        public LobbyConfigViewModel(ILogicalParent parent) : base(parent)
        {
            Header = new LobbyConfigHeader(this);
            
            // Permanent true
            CanMoveBack = this.WhenAnyValue(property1: vm => vm.Header, selector: header => header is LobbyConfigHeader);
            CanMoveForward = Players.WhenAnyValue(x => x.Count, count => count >= 6);
        }
        
        #region + Commands +

        public ReactiveCommand<Unit, Unit> AddPlayerCommand => ReactiveCommand.Create(() =>
        {
            Statistic.Players.Add(new Player(++_playerIndexer, GetRandomName));
        });

        public ReactiveCommand<Player, Unit> RemovePlayerCommand => ReactiveCommand.Create<Player>(player =>
        {
            Statistic.Players.Remove(player);
            _playerIndexer--;
            var tempIndexer = 0;
            foreach (var exPlayer in Players)
                exPlayer.UpdatePosition(++tempIndexer);
        });

        #endregion
        
        #region + Methods +
        
        public override void OnActivate()
        {
            _connectionSubscription = Statistic.Players.Connect()
                .Bind(Players)
                .Subscribe(UpdatePlayerIndexer);
        }

        public override void OnDeactivate()
        {
            _connectionSubscription.Dispose();
            Players.Clear();
        }

        public override void OnReset()
        {
            OnDeactivate();
            _playerIndexer = 0;
        }

        private void UpdatePlayerIndexer(IChangeSet<Player> changeSet)
        {
            foreach (var change in changeSet)
            {
                _playerIndexer = change.Reason switch
                {
                    ListChangeReason.Clear => 0,
                    _ => _playerIndexer
                };
            }
        }
        
        #endregion
    }
}
