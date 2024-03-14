using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Mafia.Models;

namespace Mafia.Templated_Controls;

public class PlayerCard : TemplatedControl
{
    public static readonly StyledProperty<string> PlayerNameProperty =
        AvaloniaProperty.Register<PlayerCard, string>(nameof(PlayerName));

    public string PlayerName
    {
        get => this.GetValue(PlayerNameProperty);
        set => SetValue(PlayerNameProperty, value);
    }

    public static readonly StyledProperty<int> PositionProperty =
        AvaloniaProperty.Register<PlayerCard, int>(nameof(Position));

    public int Position
    {
        get => this.GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<PlayerCard, ICommand>(nameof(Command));

    public ICommand Command
    {
        get => this.GetValue(CommandProperty);
        init => SetValue(CommandProperty, value);
    }
    
    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<PlayerCard, object>(nameof(CommandParameter));

    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        init => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="position">Player position in queue</param>
    /// <param name="onRemoveCommand">Command for removal button click</param>
    public PlayerCard(int position, ICommand onRemoveCommand)
    {
        Position = position;
        Command = onRemoveCommand;
        CommandParameter = this;
        PlayerName = GetRandomName().ToString();
    }

    /// <summary>
    /// Constructor for design preview
    /// </summary>
    public PlayerCard() => PlayerName = GetRandomName().ToString();

    // Default name for player card
    private static DefaultName GetRandomName()
    {
        Random random = new();
        var amount = Enum.GetNames(typeof(DefaultName)).Length;
        var s = random.Next(amount);
        return (DefaultName)s;
    }
}