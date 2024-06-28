using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Timers;
using DynamicData;
using DynamicData.Binding;
using Mafia.Models;
using Mafia.Models.Enums;
using Mafia.ViewModels.Headers;
using ReactiveUI;

namespace Mafia.ViewModels.Pages;

public sealed class RoundViewModel : Page
{
    #region + Private Fields +
    
    private bool _paused = true;
    private int _seconds;
    private string? _timeDisplay;
    private readonly Timer _secondTimer = new();
    private Player? _currentPlayer;
    private Player? _firstSpeaker;
    private GameStage _stage;
    private int _round;
    
    /// <summary>
    /// Subscription to skip current player on IsMuted changed true
    /// </summary>
    private IDisposable? _skipSubscription;

    /// <summary>
    /// Dispose all players` nomination subscriptions
    /// </summary>
    private CompositeDisposable _nominationSub = new();
    
    #endregion

    #region + Properties +
    
    public override HeaderVMBase Header { get; init; }
    
    public override IObservable<bool> CanMoveForward { get; }
    
    public override IObservable<bool> CanMoveBack { get; }
    
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
    
    public string? TimeDisplay
    {
        get => _timeDisplay;
        set => this.RaiseAndSetIfChanged(ref _timeDisplay, value);
    }

    public int Round
    {
        get => _round;
        set => this.RaiseAndSetIfChanged(ref _round, value);
    }
    
    public GameStage Stage
    {
        get => _stage;
        set
        {
            this.RaiseAndSetIfChanged(ref _stage, value);
            
            this.RaisePropertyChanged(nameof(VoteStagePermission));
            this.RaisePropertyChanged(nameof(NightStagePermission));
            this.RaisePropertyChanged(nameof(DayStagePermission));
        }
    }

    public bool VoteStagePermission => _stage is GameStage.Vote;
    public bool NightStagePermission => _stage is GameStage.Night;
    public bool DayStagePermission => _stage is GameStage.Day;

    public ObservableCollection<Player> Players { get; } = new();
    
    public ObservableCollection<Player> NominatedPlayers { get; } = new();

    public Player? CurrentPlayer
    {
        get => _currentPlayer;
        set => this.RaiseAndSetIfChanged(ref _currentPlayer, value);
    }

    #endregion

    public RoundViewModel()
    {
        _secondTimer.AutoReset = true;
        _secondTimer.Interval = 50;
        _secondTimer.Elapsed += (_, _) => Seconds++;

        Header = new RoundHeader(this);

        // Permanent false
        CanMoveBack = this.WhenAnyValue(vm => vm.Header, header => header is not RoundHeader);
        // Permanent false
        CanMoveForward = this.WhenAnyValue(vm => vm.Header, header => header is not RoundHeader);
        
        // Update players list
        Statistic.Players.CountChanged
            .Subscribe(_ =>
            {
                Players.Clear();
                Players.AddRange(Statistic.Players.Items);
                
                _nominationSub.Dispose();
                _nominationSub = new();
                
                foreach (var player in Players)
                {
                    player.WhenAnyValue(p => p.IsNominated)
                        .Subscribe(nominated =>
                        {
                            if (nominated && ! NominatedPlayers.Contains(player)) NominatedPlayers.Add(player);
                        })
                        .DisposeWith(_nominationSub);

                    player.WhenAnyValue(p => p.IsKickedOut)
                        .Subscribe(kicked =>
                        {
                            if (kicked && NominatedPlayers.Contains(player))
                                NominatedPlayers.Remove(player);
                        });
                }

                Stage = GameStage.Day;
                Round = 1;
                
                _firstSpeaker = Players.FirstOrDefault(x => x is { IsMuted: false, IsKickedOut: false });
                CurrentPlayer = _firstSpeaker;
            });
        
        // On current speaker change
        this.WhenAnyValue(vm => vm.CurrentPlayer)
            .Subscribe(nextPlayer =>
            {
                // Dispose subscription for previous player
                _skipSubscription?.Dispose();
                _skipSubscription = nextPlayer.WhenAnyValue(x => x.IsMuted).Subscribe(muted =>
                {
                    if ( ! muted ) return;
                    Paused = true;
                    Seconds = 0;
                    CurrentPlayer = GetNextPerson(CurrentPlayer!.Position);
                });
            });

        // Change round number on next day
        this.WhenAnyValue(vm => vm.Stage)
            .Subscribe(stage => Round = stage is GameStage.Day ? Round + 1 : Round);
        
        // Change first speaker on next round
        this.WhenAnyValue(vm => vm.Round)
            .Subscribe(round =>
            {
                if (Players.Count <= 0 || round <= 0) return;
                
                CurrentPlayer = GetNextPerson(_firstSpeaker?.Position ?? 0);
                _firstSpeaker = Players[(round - 1) % Players.Count];
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
                // Get next speakable player
                CurrentPlayer = GetNextPerson(CurrentPlayer!.Position);
            });

        // Change time display each second
        this.WhenAnyValue(x => x.Seconds)
            .Subscribe(x => TimeDisplay = $"{x / 60}:{x % 60}");
        
        // Subscription to switch stage on murder or kick
        Players.ToObservableChangeSet()
            .WhenPropertyChanged(x => x.IsKickedOut)
            .Subscribe(_ => SwitchStage());

        SwitchStageCommand = ReactiveCommand.Create(SwitchStage);
        EndSessionCommand = ReactiveCommand.Create<GameOver>(EndSession);
    }

    #region + Commands +

    public ReactiveCommand<Unit, Unit> SwitchTimerCommand => ReactiveCommand.Create(() =>
    {
        Paused ^= true;
        if (!Paused && Seconds >= 60) Seconds = 0;
    });

    public ReactiveCommand<Unit, Unit> SwitchStageCommand { get; }
    
    public ReactiveCommand<GameOver, Unit> EndSessionCommand { get; }
    
    #endregion
    
    #region Methods

    private Player? GetNextPerson(int startFromPosition)
    {
        // To avoid crush on all players muted
        var crushIterator = 0;
        
        int nextPersonIndex;
        do
        {
            nextPersonIndex = startFromPosition++ % Players.Count;
            
            // if we passed first speaker --> round is over
            if (Players[nextPersonIndex] == _firstSpeaker) SwitchStage();
        } while ((Players[nextPersonIndex].IsMuted || Players[nextPersonIndex].IsKickedOut) && ++crushIterator < Players.Count + 1 );
        
        return crushIterator == Players.Count + 1 ? null : Players[nextPersonIndex];
    }

    private void SwitchStage() => Stage = (GameStage)((int)(Stage + 1) % Enum.GetValues(typeof(GameStage)).Length);
    
    private GameOver CheckGameOver()
    {
        var alive = Players.Where(x => !x.IsKickedOut).ToArray();
        var mafias = alive.Where(x => x.Role is GameRole.Mafia or GameRole.Don).ToArray();
        
        if (mafias.Length == 0) return GameOver.RedWins;
        
        var peasants = alive.Except(mafias).ToArray();
        return peasants.Length <= mafias.Length ? GameOver.BlackWins : GameOver.None;
    }

    private void EndSession(GameOver gameOver)
    {
        Statistic.CreateReport(gameOver);
        
        // TODO Reset session
        
        _nominationSub.Dispose();
    }
    
    #endregion
}