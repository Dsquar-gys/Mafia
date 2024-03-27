using System;
using ReactiveUI;

namespace Mafia.Models;

public class Player : ReactiveObject
{
    private int _number;
    private string _nickname;
    private GameRole _role;

    public int Number
    {
        get => _number;
        private set => this.RaiseAndSetIfChanged(ref _number, value);
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

    public Player(int position, string name)
    {
        _number = position;
        _nickname = name;
        _role = GameRole.None;
    }

    public void UpdatePosition(int newPos) => Number = newPos;
    public void UpdateRole(GameRole newRole) => Role = newRole;
}