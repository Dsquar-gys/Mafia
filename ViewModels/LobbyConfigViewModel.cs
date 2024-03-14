using Mafia.Models;
using System.Collections.ObjectModel;
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
        
        public ObservableCollection<PlayerCard> Players { get; } = new();
        
        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddPlayerCommand => ReactiveCommand.Create(() =>
        {
            Players.Add(new PlayerCard(++_indexer, RemovePlayerCommand));
        });

        public ReactiveCommand<PlayerCard, Unit> RemovePlayerCommand => ReactiveCommand.Create<PlayerCard>(player =>
        {
            Players.Remove(player);
            _indexer--;
            var tempIndexer = 0;
            foreach (var playerCard in Players)
                playerCard.Position = ++tempIndexer;
        });

        public ReactiveCommand<Unit, Unit> CommitPlayersCommand => ReactiveCommand.Create(() =>
        {
            Statistic.Players.Edit(innerCollection =>
            {
                innerCollection.Load(Players);
            });
        });

        #endregion
    }
}
