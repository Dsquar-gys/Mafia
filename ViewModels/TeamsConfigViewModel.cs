using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using Mafia.Models;
using ReactiveUI;
using Console = System.Console;

namespace Mafia.ViewModels;

public class TeamsConfigViewModel : Page
{
    private int _playersAmount = 0;
    private Player? _draggingPlayer;

    public const string CustomFormat = "player-format";
    
    public ObservableCollection<Player> TransparentPlayers { get; private set; } = new();
    public ObservableCollection<Player> BlackPlayers { get; private set; } = new();
    public ObservableCollection<Player> RedPlayers { get; private set; } = new();

    public Player? DraggingPlayer
    {
        get => _draggingPlayer;
        set => this.RaiseAndSetIfChanged(ref _draggingPlayer, value);
    }

    public TeamsConfigViewModel()
    { // Remember about player NICKNAME CHANGE

        Statistic.Players.CountChanged
            .Subscribe(x =>
            {
                Console.WriteLine("Amount of players changed to {0}", x);
                CountChanged();
            });
    }

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
            GameRole.Donna => BlackPlayers,
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