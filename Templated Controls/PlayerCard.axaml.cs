using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Mafia.Models;

namespace Mafia.Templated_Controls;

public class PlayerCard : TemplatedControl
{
    public static readonly StyledProperty<Player> PlayerProperty =
        AvaloniaProperty.Register<PlayerCard, Player>(nameof(Player));

    public Player Player
    {
        get => this.GetValue(PlayerProperty);
        set => SetValue(PlayerProperty, value);
    }

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<PlayerCard, ICommand>(nameof(Command));

    public ICommand Command
    {
        get => this.GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<PlayerCard, object>(nameof(CommandParameter));

    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        init => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Constructor for design preview
    /// </summary>
    public PlayerCard() => Player = new Player(0);
}