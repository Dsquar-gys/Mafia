using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Mafia.Models;
using Mafia.ViewModels.Pages;

namespace Mafia.Views.Pages;

public partial class TeamsConfigView : UserControl
{
    public TeamsConfigView()
    {
        InitializeComponent();
        
        AddHandler(DragDrop.DragOverEvent, DragOver);
        AddHandler(DragDrop.DropEvent, Drop);
    }
    
    private void View_OnLoaded(object? sender, RoutedEventArgs e)
    {
        GhostItem.IsVisible = false;
    }
    
    private async void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Console.WriteLine("DoDrag start");

        if (sender is not Border item) return;
        if (item.DataContext is not Player player) return;
        
        var mousePos = e.GetPosition(DragContainer);
        var ghostPos = GhostItem.Bounds.Position;
        var offsetX = mousePos.X - ghostPos.X;
        var offsetY = mousePos.Y - ghostPos.Y;
        GhostItem.RenderTransform = new TranslateTransform(offsetX + 15, offsetY - 45);
        
        if (DataContext is not TeamsConfigViewModel vm) return;
        vm.StartDrag(player);

        GhostItem.IsVisible = true;

        var dragData = new DataObject();
        dragData.Set(TeamsConfigViewModel.CustomFormat, player);
        var result = await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
        
        Console.WriteLine("DragAndDrop result: {0}", result);
        GhostItem.IsVisible = false;
    }

    private void DragOver(object? sender, DragEventArgs e)
    {
        var currentPos = e.GetPosition(DragContainer);
        var ghostPos = GhostItem.Bounds.Position;
        var offsetX = currentPos.X - ghostPos.X;
        var offsetY = currentPos.Y - ghostPos.Y;
        
        GhostItem.RenderTransform = new TranslateTransform(offsetX + 15, offsetY - 45);
        
        // set drag cursor icon
        e.DragEffects = DragDropEffects.Move;
        if (DataContext is not TeamsConfigViewModel vm) return;
        var data = e.Data.Get(TeamsConfigViewModel.CustomFormat);
        if (data is not Player) return;
        if (!vm.IsDestinationValid((e.Source as Control)?.Name))
            e.DragEffects = DragDropEffects.None;
    }

    private void Drop(object? sender, DragEventArgs e)
    {
        Console.WriteLine("Drop");
        
        var data = e.Data.Get(TeamsConfigViewModel.CustomFormat);

        if (data is not Player player)
        {
            Console.WriteLine("No any player");
            return;
        }

        if (DataContext is not TeamsConfigViewModel vm) return;
        vm.Drop(player, (e.Source as Control)?.Name);
    }
}