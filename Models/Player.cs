using System;
using System.Reactive;
using Mafia.Models.Enums;
using ReactiveUI;

namespace Mafia.Models;

public class Player : ReactiveObject
{
    #region + Private fields +
    
    private int _position;
    private string _nickname;
    private GameRole _role;
    private int _fouls;

    private bool _isMuted;
    private bool _isNominated;
    private bool _isKickedOut;
    
    #endregion

    #region + Properties +
    
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

    public bool IsActiveRole
    {
        get => _role is GameRole.Detective or GameRole.Don;
        set
        {
            if (value) Role = Role is GameRole.Mafia ? GameRole.Don : GameRole.Detective;
            else Role = Role is GameRole.Don ? GameRole.Mafia : GameRole.Peasant;
            
            this.RaisePropertyChanged();
        }
    }

    public bool IsKickedOut
    {
        get => _isKickedOut;
        private set => this.RaiseAndSetIfChanged(ref _isKickedOut, value);
    }

    public bool IsMuted
    {
        get => _isMuted;
        private set => this.RaiseAndSetIfChanged(ref _isMuted, value);
    }

    public bool IsNominated
    {
        get => _isNominated;
        private set => this.RaiseAndSetIfChanged(ref _isNominated, value);
    }
    
    #endregion

    public Player(int position, string name)
    {
        _position = position;
        _nickname = name;
        _role = GameRole.None;

        this.WhenAnyValue(pl => pl.Fouls, fouls => fouls >= 3)
            .Subscribe(x => IsMuted = x);
    }

    #region + Commands +
    
    public ReactiveCommand<Unit, Unit> SetFoulCommand => ReactiveCommand.Create(() => { Fouls++; });
    public ReactiveCommand<Unit, Unit> NominateCommand => ReactiveCommand.Create(() => { IsNominated = true; });
    public ReactiveCommand<Unit, Unit> KickCommand => ReactiveCommand.Create(() => { IsKickedOut = true; });
    
    #endregion
    
    #region + Methods +
    
    public void UpdatePosition(int newPos) => Position = newPos;
    public void UpdateRole(GameRole newRole) => Role = newRole;
    
    #endregion
}