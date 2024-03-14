using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using Mafia.Models;
using Mafia.Templated_Controls;
using ReactiveUI;

namespace Mafia.ViewModels;

public class TeamsConfigViewModel : Page
{
    public ObservableCollection<PlayerCard> TransparentPlayers => new();
    public ObservableCollection<PlayerCard> BlackPlayers => new();
    public ObservableCollection<PlayerCard> RedPlayers => new();

    public TeamsConfigViewModel()
    { // Remember about player NICKNAME CHANGE
        Statistic.Players.WhenAnyValue(x => x.Count)
            .Subscribe(CountChanged);
    }

    private void CountChanged(int x)
    {
        TransparentPlayers.Clear();
        TransparentPlayers.AddRange(Statistic.Players.Items);
        BlackPlayers.Clear();
        RedPlayers.Clear();
    }
}