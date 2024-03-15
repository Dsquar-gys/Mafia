using System;
using System.Collections.ObjectModel;
using DynamicData;
using Mafia.Models;
using Console = System.Console;

namespace Mafia.ViewModels;

public class TeamsConfigViewModel : Page
{
    private int _playersAmount = 0;
    
    public ObservableCollection<Player> TransparentPlayers { get; private set; } = new();
    public ObservableCollection<Player> BlackPlayers { get; private set; } = new();
    public ObservableCollection<Player> RedPlayers { get; private set; } = new();

    public TeamsConfigViewModel()
    { // Remember about player NICKNAME CHANGE

        Statistic.Players.CountChanged
            .Subscribe(x =>
            {
                Console.WriteLine("Amount of players changed to {0}", x);
                CountChanged();
            });
    }

    private void CountChanged()
    { // Check if Statistic.Players changes at all !!
        TransparentPlayers.Clear();
        TransparentPlayers.AddRange(Statistic.Players.Items);
        /*BlackPlayers.Clear();
        RedPlayers.Clear();*/
        Console.WriteLine("kek");
    }
}