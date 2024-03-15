using Mafia.Models;
using System.Collections.ObjectModel;
using System.Reactive;
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
        
        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddPlayerCommand => ReactiveCommand.Create(() =>
        {
            Players.Add(new Player(++_indexer));
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
            Statistic.Players.Edit(innerList =>
            {
                innerList.Clear();
                innerList.AddRange(Players);
            });
        });

        #endregion
    }
}
