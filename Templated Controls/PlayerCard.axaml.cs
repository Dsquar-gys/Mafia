using Avalonia;
using Avalonia.Controls.Primitives;
using System.Windows.Input;

namespace Mafia;

public class PlayerCard : TemplatedControl
{
    public static readonly StyledProperty<string> PlayerNameProperty =
        AvaloniaProperty.Register<PlayerCard, string>(nameof(PlayerName), "Крэк");

    public string PlayerName
    {
        get => this.GetValue(PlayerNameProperty);
        set => SetValue(PlayerNameProperty, value);
    }

    public static readonly StyledProperty<int> PositionProperty =
        AvaloniaProperty.Register<PlayerCard, int>(nameof(Position), 0);

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
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<PlayerCard, object>(nameof(CommandParameter));

    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public PlayerCard(int position, string name, ICommand command, object parameter = null)
    {
        Position = position;
        Name = name;
        Command = command;
        CommandParameter = parameter ?? this;
    }

    public PlayerCard() { }
}