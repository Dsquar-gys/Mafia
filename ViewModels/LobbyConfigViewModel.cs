using System;
using Mafia.Models;
using System.Linq;
using System.Reactive;
using DynamicData;
using DynamicData.Binding;
using Mafia.Headers;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public sealed class LobbyConfigViewModel : Page
    {
        #region Private fields
        
        private int _indexer;

        #endregion

        public LobbyConfigViewModel()
        {
            Header = new LobbyConfigHeader(this);

            Statistic.Players.Connect()
                .Bind(Players)
                .Subscribe();
        }
        
        #region Properties

        public override SessionStage Stage => SessionStage.PlayerLineUp;
        public IObservableCollection<Player> Players { get; } = new ObservableCollectionExtended<Player>();
        private string GetRandomName
        {
            get
            {
                Random random = new();

                var existing = Players.Select(x => x.Nickname).ToArray();
                var total = Enum.GetNames(typeof(DefaultName));

                var rest = total.Except(existing).ToArray();

                if (rest.Length != 0) return rest[random.Next(rest.Length)];
                
                Console.WriteLine("Out of default names...");
                return "NickName";
            }
        }
        
        #endregion
        
        #region Commands

        public ReactiveCommand<Unit, Unit> AddPlayerCommand => ReactiveCommand.Create(() =>
        {
            Statistic.Players.Add(new Player(++_indexer, GetRandomName));
        });

        public ReactiveCommand<Player, Unit> RemovePlayerCommand => ReactiveCommand.Create<Player>(player =>
        {
            Statistic.Players.Remove(player);
            _indexer--;
            var tempIndexer = 0;
            foreach (var exPlayer in Players)
                exPlayer.UpdatePosition(++tempIndexer);
        });

        #endregion

        public override HeaderTemplateBase Header { get; init; }
    }
}
