using System;
using Avalonia.Controls;
using Mafia.ViewModels.Pages;
using ReactiveUI;

namespace Mafia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        
        this.WhenAnyValue(window => window.CurrentPageControl.Content, vm => vm is not StarterViewModel)
            .Subscribe(x => HeaderControl.Opacity = x ? 1 : 0);
    }
}