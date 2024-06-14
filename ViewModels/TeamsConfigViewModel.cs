using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using DynamicData;
using DynamicData.Binding;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels;

public class TeamsConfigViewModel : Page
{
    public override SessionStage Stage => SessionStage.Teaming;
    public override HeaderTemplate Header { get; init; }
    
    #region Private fields
    
    private Player? _draggingPlayer;

    private Random _random = new();

    private bool _readyForward;
    
    #endregion
    
    #region Properties
    
    public const string CustomFormat = "player-format";
    
    public ObservableCollection<Player> TransparentPlayers { get; } = new();
    public ObservableCollection<Player> BlackPlayers { get; } = new();
    public ObservableCollection<Player> RedPlayers { get; } = new();

    public Player? DraggingPlayer
    {
        get => _draggingPlayer;
        private set => this.RaiseAndSetIfChanged(ref _draggingPlayer, value);
    }

    public bool ReadyForward
    {
        get => _readyForward;
        set => this.RaiseAndSetIfChanged(ref _readyForward, value);
    }
    
    #endregion

    public TeamsConfigViewModel()
    { // Remember about player NICKNAME CHANGE

        Statistic.Players.CountChanged
            .Subscribe(x =>
            {
                Console.WriteLine("Amount of players changed to {0}", x);
                CountChanged();
            });

        TransparentPlayers.WhenPropertyChanged(x => x.Count)
            .Subscribe(x => ReadyForward = x.Value == 0);
    }

    public ReactiveCommand<Unit, Unit> ShuffleCommand => ReactiveCommand.Create(() =>
    {
        var playerPool = new List<Player>(TransparentPlayers.Concat(RedPlayers).Concat(BlackPlayers));
        TransparentPlayers.Clear();
        RedPlayers.Clear();
        BlackPlayers.Clear();

        // Counting amount of mafia players
        var mafias = Math.Round(playerPool.Count / (_random.NextSingle() + 3));
        
        playerPool.Shuffle();
        
        BlackPlayers.AddRange(playerPool.Take((int)mafias));
        RedPlayers.AddRange(playerPool.Skip((int)mafias));
        
        // Reset roles
        foreach (var blackPlayer in BlackPlayers)
            blackPlayer.UpdateRole(GameRole.Mafia);
        foreach (var redPlayer in RedPlayers)
            redPlayer.UpdateRole(GameRole.Peasant);
        
        BlackPlayers[0].UpdateRole(GameRole.Don);
        RedPlayers[0].UpdateRole(GameRole.Detective);
    });
    
    public void StartDrag(Player player) => DraggingPlayer = player;
    
    public void Drop(Player player, string? destinationListName)
    {
        var sourceList = GetSourceList(player.Role);
        var item = sourceList.SingleOrDefault(p => p.Nickname == player.Nickname);
        if (item is null)
        {
            Console.WriteLine($"Player with nickname '{player.Nickname}' not found");
            return;
        }

        var destination = GetDestinationList(destinationListName);

        if (destination.collection is null)
        {
            Console.WriteLine($"Invalid drop location '{destinationListName}'");
            return;
        }

        sourceList.Remove(item);
        destination.collection!.Add(item);
        item.UpdateRole(destination.role);
        Console.WriteLine($"Changing '{player.Nickname}''s role to '{item.Role}'");
    }
    
    public bool IsDestinationValid(string? destinationName)
    {
        var destination = GetDestinationList(destinationName);
        return destination.collection is not null;
    }
    
    private void CountChanged()
    { // Check if Statistic.Players changes at all !!
        TransparentPlayers.Clear();
        
        foreach (var player in Statistic.Players.Items)
            player.UpdateRole(GameRole.None);
        
        TransparentPlayers.AddRange(Statistic.Players.Items);
        BlackPlayers.Clear();
        RedPlayers.Clear();
    }
    
    private ObservableCollection<Player> GetSourceList(GameRole role)
    {
        return role switch
        {
            GameRole.None => TransparentPlayers,
            GameRole.Peasant => RedPlayers,
            GameRole.Detective => RedPlayers,
            GameRole.Mafia => BlackPlayers,
            GameRole.Don => BlackPlayers,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
    
    private (ObservableCollection<Player>? collection, GameRole role) GetDestinationList(string? listName)
    {
        return listName switch
        {
            "WhiteCollection" => (TransparentPlayers, GameRole.None),
            "RedCollection" => (RedPlayers, GameRole.Peasant),
            "BlackCollection" => (BlackPlayers, GameRole.Mafia),
            _ => (null, GameRole.None)
        };
    }
}