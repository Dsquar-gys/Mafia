using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Mafia.Converters;

/// <summary>
/// Converter for coloring alive and kicked players
/// </summary>
public class AliveColorBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!targetType.IsAssignableTo(typeof(IBrush)) || value is not bool isAlive)
            return new BindingNotification(new InvalidCastException("Wrong target or value..."), BindingErrorType.Error,
                new SolidColorBrush(Color.FromRgb(150,80,80)));

        return isAlive switch
        {
            true => new SolidColorBrush(Color.FromRgb(255,200,200)),
            false => new SolidColorBrush(Colors.White)
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error,
            new SolidColorBrush(Color.FromRgb(150,80,80)));
    }
}