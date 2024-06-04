using System;
using Mafia.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Mafia.Templated_Controls;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class LobbyConfigViewModel : Page
    {
        #region Private fields
        
        private int _indexer;
        
        #endregion
        
        #region Properties
        
        public ObservableCollection<Player> Players { get; } = new();
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
            Players.Add(new Player(++_indexer, GetRandomName));
        });

        public ReactiveCommand<Player, Unit> RemovePlayerCommand => ReactiveCommand.Create<Player>(player =>
        {
            Players.Remove(player);
            _indexer--;
            var tempIndexer = 0;
            foreach (var exPlayer in Players)
                exPlayer.UpdatePosition(++tempIndexer);
        });

        public ReactiveCommand<Unit, Unit> CommitPlayersCommand => ReactiveCommand.Create(() =>
        {
            Statistic.Players.Edit(innerCollection =>
            {
                innerCollection.Clear();
                innerCollection.AddRange(Players);
            });
        });

        #endregion
    }
}
