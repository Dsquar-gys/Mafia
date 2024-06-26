using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using DynamicData;
using Mafia.Models;
using Mafia.Models.Enums;
using Mafia.ViewModels.Headers;
using ReactiveUI;

namespace Mafia.ViewModels.Pages;

public sealed class TeamsConfigViewModel : Page
{
    /// <summary>
    /// Tag for dragging player
    /// </summary>
    public const string CustomFormat = "player-format";
    
    #region + Private fields +
    
    private Player? _draggingPlayer;
    private readonly Random _random = new();
    
    #endregion
    
    #region + Properties +
    
    public override HeaderVMBase Header { get; init; }
    
    public override IObservable<bool> CanMoveForward { get; }
    
    public override IObservable<bool> CanMoveBack { get; }
    
    /// <summary>
    /// Unauthorized players
    /// </summary>
    public ObservableCollection<Player> TransparentPlayers { get; } = new();
    
    /// <summary>
    /// Mafias
    /// </summary>
    public ObservableCollection<Player> BlackPlayers { get; } = new();
    
    /// <summary>
    /// Peasants
    /// </summary>
    public ObservableCollection<Player> RedPlayers { get; } = new();

    public Player? DraggingPlayer
    {
        get => _draggingPlayer;
        private set => this.RaiseAndSetIfChanged(ref _draggingPlayer, value);
    }
    
    #endregion
    
    public TeamsConfigViewModel()
    {
        Header = new TeamsConfigHeader(this);
        
        // Permanent true
        CanMoveBack = this.WhenAnyValue(property1: vm => vm.Header, selector: h => h is TeamsConfigHeader);
        CanMoveForward = TransparentPlayers.WhenAnyValue(x => x.Count, count => count == 0);

        Statistic.Players.CountChanged
            .Subscribe(x =>
            {
                Console.WriteLine("Amount of players changed to {0}", x);
                CountChanged();
            });
    }
    
    #region + Commands +
    
    public ReactiveCommand<Unit, Unit> ShuffleCommand => ReactiveCommand.Create(() =>
    {
        var playerPool = new List<Player>(TransparentPlayers.Concat(RedPlayers).Concat(BlackPlayers));
        TransparentPlayers.Clear();
        RedPlayers.Clear();
        BlackPlayers.Clear();

        // Counting amount of mafia players
        var mafias = Math.Round(playerPool.Count / (_random.NextSingle() + 3));
        
        playerPool.Shuffle();
        
        BlackPlayers.AddRange(playerPool.Take((int)mafias).OrderBy(x => x.Position));
        RedPlayers.AddRange(playerPool.Skip((int)mafias).OrderBy(x => x.Position));
        
        // Reset roles
        foreach (var blackPlayer in BlackPlayers)
            blackPlayer.UpdateRole(GameRole.Mafia);
        foreach (var redPlayer in RedPlayers)
            redPlayer.UpdateRole(GameRole.Peasant);
        
        BlackPlayers[_random.Next(BlackPlayers.Count - 1)].UpdateRole(GameRole.Don);
        RedPlayers[_random.Next(RedPlayers.Count - 1)].UpdateRole(GameRole.Detective);
    });
    
    #endregion
    
    #region + Methods +
    
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
    
    #endregion
}