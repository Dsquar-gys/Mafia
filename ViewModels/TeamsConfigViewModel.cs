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

    /*public void Drop(Player player, string? destinationListName)
    {
        var sourceList = GetSourceList(player.Role);
        var item = sourceList.SingleOrDefault(p => p.Nickname == player.Nickname);
        if (item is null)
        {
            Console.WriteLine($"Player with nickname '{player.Nickname}' not found");
            return;
        }

        var destination = GetDestinationList(player.Role);

        if (destination.ListName != destinationListName)
        {
            Console.WriteLine($"Invalid drop location '{destinationListName}'. Valid location is {destination.ListName}");
            return;
        }

        sourceList.Remove(item);
        item.UpdateRole(destination.Role);
        destination.List.Add(item);
        Console.WriteLine($"Changing '{player.Nickname}''s role to '{item.Role}'");
    }*/
    
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

        if (destination is null)
        {
            Console.WriteLine($"Invalid drop location '{destinationListName}'");
            return;
        }

        sourceList.Remove(item);
        //item.UpdateRole(destination.Role);
        destination.Add(item);
        Console.WriteLine($"Changing '{player.Nickname}''s role to '{item.Role}'");
    }
    
    public bool IsDestinationValid(Player player, string? destinationName)
    {
        var destination = GetDestinationList(destinationName);
        return destination is not null;
    }
    
    private void CountChanged()
    { // Check if Statistic.Players changes at all !!
        TransparentPlayers.Clear();
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
            GameRole.Mafia => BlackPlayers,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
    
    private ObservableCollection<Player>? GetDestinationList(string? listName)
    {
        return listName switch
        {
            "WhiteCollection" => TransparentPlayers,
            "RedCollection" => RedPlayers,
            "BlackCollection" => BlackPlayers,
            _ => null
        };
    }
}