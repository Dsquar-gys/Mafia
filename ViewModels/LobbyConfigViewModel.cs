using Mafia.Models;
using Mafia.ViewModels.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mafia.ViewModels
{
    public class LobbyConfigViewModel : Page
    {
        private int _indexer = 0;
        public ObservableCollection<PlayerCard> Players { get; }
        public LobbyConfigViewModel() : base()
        {
            Players = new ObservableCollection<PlayerCard>();

            RemovePlayerCommand = new DelegateCommand(parameter =>
            {
                Players.Remove(parameter as PlayerCard);
                _indexer--;
                int tempIndexer = 0;
                foreach (PlayerCard playerCard in Players)
                    playerCard.Position = ++tempIndexer;
            });

            AddPlayerCommand = new DelegateCommand(parameter =>
            {
                Players.Add(new PlayerCard(++_indexer, "Player", RemovePlayerCommand));
            });

            CommitPlayersCommand = new DelegateCommand(parameter =>
            {
                Statistic.Players = new List<PlayerCard>(Players);
            });
        }

        #region Commands

        public DelegateCommand AddPlayerCommand { get; }
        public DelegateCommand RemovePlayerCommand { get; }
        public DelegateCommand CommitPlayersCommand { get; }

        #endregion
    }
}
