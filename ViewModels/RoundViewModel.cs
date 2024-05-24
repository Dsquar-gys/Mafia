using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Timers;
using DynamicData;
using DynamicData.Binding;
using Mafia.Models;
using ReactiveUI;

namespace Mafia.ViewModels;

public class RoundViewModel : Page
{
    private bool _paused = true;
    private int _seconds;
    private string _timeDisplay;
    private readonly Timer _secondTimer = new();
    private Player? _currentPlayer;
    private int _speechIteration;
    
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

        // Update players list
        Statistic.Players.CountChanged
            .Subscribe(_ =>
            {
                Players.Clear();
                Players.AddRange(Statistic.Players.Items);

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
                CurrentPlayer = Players[++_speechIteration % Players.Count];
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
    
    //TODO EndSessionCommand
    public ReactiveCommand<Unit, Unit> EndSessionCommand =>
        ReactiveCommand.Create(() =>
        {
            Statistic.CreateReport();
            
            MainWindowViewModel.Instance!.ResetSession();
        });

    internal override void ResetPage() { }
}