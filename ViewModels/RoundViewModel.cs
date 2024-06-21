using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Timers;
using DynamicData;
using Mafia.Headers;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels;

public sealed class RoundViewModel : Page
{
    private bool _paused = true;
    private int _seconds;
    private string _timeDisplay;
    private readonly Timer _secondTimer = new();
    private Player? _currentPlayer;
    private int _speechIteration;
    
    public override HeaderTemplateBase Header { get; init; }
    private bool Paused
    {
        get => _paused;
        set => this.RaiseAndSetIfChanged(ref _paused, value);
    }
    
    private int Seconds
    {
        get => _seconds;
        set => this.RaiseAndSetIfChanged(ref _seconds, value);
    }
    
    public string TimeDisplay
    {
        get => _timeDisplay;
        set => this.RaiseAndSetIfChanged(ref _timeDisplay, value);
    }
    
    public ObservableCollection<Player> Players { get; } = new();
    public ObservableCollection<Player> SpeakablePlayers { get; } = new();
    public ObservableCollection<Player> NominatedPlayers { get; } = new();

    public Player? CurrentPlayer
    {
        get => _currentPlayer;
        set => this.RaiseAndSetIfChanged(ref _currentPlayer, value);
    }


    public RoundViewModel()
    {
        _secondTimer.AutoReset = true;
        _secondTimer.Interval = 100;
        _secondTimer.Elapsed += (sender, args) => Seconds++;

        Header = new RoundHeader(this);
        
        // Update players list
        Statistic.Players.CountChanged
            .Subscribe(_ =>
            {
                Players.Clear();
                SpeakablePlayers.Clear();
                
                Players.AddRange(Statistic.Players.Items);
                SpeakablePlayers.AddRange(Players);

                _speechIteration = 0;
                CurrentPlayer = Players.FirstOrDefault();
            });
        
        // Dependency for timer on Paused prop
        this.WhenAnyValue(x => x.Paused)
            .Subscribe(x =>
            {
                if (x) _secondTimer.Stop();
                else _secondTimer.Start();
            });
        
        // Paused after 1 minute
        this.WhenAnyValue(property1: vm => vm.Seconds,
                selector: seconds => seconds >= 60)
            .Subscribe(x =>
            {
                if (!x) return;
                Paused = x;
                CurrentPlayer = SpeakablePlayers[++_speechIteration % SpeakablePlayers.Count];
            });

        // Change time display each second
        this.WhenAnyValue(x => x.Seconds)
            .Subscribe(x => TimeDisplay = $"{x / 60}:{x % 60}");
    }

    public ReactiveCommand<Unit, Unit> SwitchTimer => ReactiveCommand.Create(() =>
    {
        Paused ^= true;
        if (!Paused && Seconds >= 60) Seconds = 0;
    });
    
    // public ReactiveCommand<Player, Unit> AddFoul => ReactiveCommand.Create<Player>(player =>
    // {
    //     var index = player.Fouls.IndexOf(false);
    //     player.Fouls[index] = true;
    //
    //     if (!player.Fouls.All(x => x)) return;
    //     SpeakablePlayers.Remove(player);
    //     --_speechIteration;
    //
    //     if (CurrentPlayer != player) return;
    //     Seconds = 60;
    // });
    
    public ReactiveCommand<Player, Unit> AddCandidate => ReactiveCommand.Create<Player>(player =>
    {
        if ( ! NominatedPlayers.Contains(player))
            NominatedPlayers.Add(player);
    });
    
    public ReactiveCommand<Player, Unit> KickPlayer => ReactiveCommand.Create<Player>(player =>
    {
        if (SpeakablePlayers.Remove(player))
        {
            player.IsKickedOut = true;
            --_speechIteration;
        }

        var state = CheckGameOver();
        switch (state)
        {
            case GameOver.None:
                return;
            case GameOver.RedWins:
            case GameOver.BlackWins:
            default:
                EndSessionCommand.Execute(state).Subscribe();
                return;
        }
    });
    
    public ReactiveCommand<GameOver, Unit> EndSessionCommand { get; } =
        ReactiveCommand.Create<GameOver>(gameOver =>
        {
            Statistic.CreateReport( gameOver );
            
            // TODO Reset session
        });

    private GameOver CheckGameOver()
    {
        var alive = Players.Where(x => !x.IsKickedOut).ToArray();
        var mafias = alive.Where(x => x.Role is GameRole.Mafia or GameRole.Don).ToArray();
        
        if (mafias.Length == 0) return GameOver.RedWins;
        
        var peasants = alive.Except(mafias).ToArray();
        if (peasants.Length <= mafias.Length) return GameOver.BlackWins;

        return GameOver.None;
    }
}