using System;
using Mafia.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using DynamicData.Binding;
using ReactiveUI;

namespace Mafia.ViewModels
{
    public class LobbyConfigViewModel : Page
    {
        #region Private fields
        
        private int _indexer;
        private bool _readyForward;
        
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
                
                var s = random.Next(rest.Length);
                try
                {
                    return rest[s];
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Out of default names...");
                    return "NickName";
                }
            }
        }
        
        public bool ReadyForward
        {
            get => _readyForward;
            set => this.RaiseAndSetIfChanged(ref _readyForward, value);
        }
        
        #endregion

        public LobbyConfigViewModel()
        {
            Players.WhenPropertyChanged(x => x.Count)
                .Subscribe(x => ReadyForward = x.Value >= 6);
        }
        
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
            Statistic.Players.Edit(innerList =>
            {
                innerList.Clear();
                innerList.AddRange(Players);
            });
        });

        #endregion

        internal override void ResetPage()
        {
            Players.Clear();
            _indexer = 0;
        }
    }
}
