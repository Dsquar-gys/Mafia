using System;
using System.Reactive;
using ReactiveUI;

namespace Mafia.Models;

public class Player : ReactiveObject
{
    private int _position;
    private string _nickname;
    private GameRole _role;
    private short _fouls;

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
    public bool IsKickedOut { get; set; } = false;
    public bool IsMuted { get; private set; }

    public Player(int position, string name)
    {
        _position = position;
        _nickname = name;
        _role = GameRole.None;
        _fouls = 0;

        this.WhenAnyValue(pl => pl._fouls >= 3).Subscribe(x => IsMuted = x);
    }

    public ReactiveCommand<Unit, Unit> SetFoul => ReactiveCommand.Create(() => { _fouls++; });
    
    public void UpdatePosition(int newPos) => Position = newPos;
    public void UpdateRole(GameRole newRole) => Role = newRole;
}