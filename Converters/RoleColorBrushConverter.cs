using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Mafia.Models;

namespace Mafia.Converters;

public class RoleColorBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!targetType.IsAssignableTo(typeof(IBrush)) || value is not GameRole role)
            return new BindingNotification(new InvalidCastException("Wrong target or value..."), BindingErrorType.Error,
                new SolidColorBrush(Color.FromRgb(250,50,50)));

        return role switch
        {
            GameRole.Peasant => new SolidColorBrush(Color.FromRgb(200, 180, 180)),
            GameRole.Detective => new SolidColorBrush(Color.FromRgb(180, 180, 200)),
            GameRole.Mafia => new SolidColorBrush(Color.FromRgb(200, 200, 180)),
            GameRole.Don => new SolidColorBrush(Color.FromRgb(200, 200, 160)),
            _ => new BindingNotification(new InvalidCastException("Invalid Game Role..."), BindingErrorType.Error,
                new SolidColorBrush(Color.FromRgb(255, 255, 255)))
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error,
            new SolidColorBrush(Color.FromRgb(250,50,50)));
    }
}