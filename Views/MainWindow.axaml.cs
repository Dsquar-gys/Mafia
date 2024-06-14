using System;
using Avalonia.Controls;
using Mafia.ViewModels;
using ReactiveUI;

namespace Mafia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WhenAnyValue(window => window.CurrentPageControl.Content, vm => vm is not StarterViewModel)
                .Subscribe(x => HeaderControl.IsVisible = x);
        }
    }
}