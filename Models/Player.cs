using System;
using System.Reactive;
using ReactiveUI;

namespace Mafia.Models;

public class Player : ReactiveObject
{
    private int _position;
    private string _nickname;
    private GameRole _role;
    private int _fouls;

    private bool _isMuted;
    private bool _isNominated;
    private bool _isKickedOut;

    public int Fouls
    {
        get => _fouls;
        set => this.RaiseAndSetIfChanged(ref _fouls, value);
    }

    public int Position
    {
        get => _position;
        private set => this.RaiseAndSetIfChanged(ref _position, value);
    }

    public string Nickname
    {
        get => _nickname;
        set => this.RaiseAndSetIfChanged(ref _nickname, value);
    }

    public GameRole Role
    {
        get => _role;
        set => this.RaiseAndSetIfChanged(ref _role, value);
    }
    
    public bool IsActiveRole => _role is GameRole.Detective or GameRole.Don;

    public bool IsKickedOut
    {
        get => _isKickedOut;
        set => this.RaiseAndSetIfChanged(ref _isKickedOut, value);
    }

    public bool IsMuted
    {
        get => _isMuted;
        private set => this.RaiseAndSetIfChanged(ref _isMuted, value);
    }

    public bool IsNominated
    {
        get => _isNominated;
        set => this.RaiseAndSetIfChanged(ref _isNominated, value);
    }

    public Player(int position, string name)
    {
        _position = position;
        _nickname = name;
        _role = GameRole.None;

        this.WhenAnyValue(pl => pl.Fouls, fouls => fouls >= 3)
            .Subscribe(x => IsMuted = x);
    }

    public ReactiveCommand<Unit, Unit> SetFoulCommand => ReactiveCommand.Create(() => { Fouls++; });
    public ReactiveCommand<Unit, Unit> NominateCommand => ReactiveCommand.Create(() => { IsNominated = true; });
    public ReactiveCommand<Unit, Unit> KickCommand => ReactiveCommand.Create(() => { IsKickedOut = true; });
    
    public void UpdatePosition(int newPos) => Position = newPos;
    public void UpdateRole(GameRole newRole) => Role = newRole;
}