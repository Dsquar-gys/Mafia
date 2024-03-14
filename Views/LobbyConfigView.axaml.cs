using Avalonia.Controls;
using Mafia.Models;

namespace Mafia.Views;

public partial class LobbyConfigView : UserControl
{
    public LobbyConfigView()
    {
        InitializeComponent();

        MasterNameDisplay.Bind(TextBlock.TextProperty, Statistic.MasterNameSubject);
    }
}