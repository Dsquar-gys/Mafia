using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Mafia.Models.Enums;

namespace Mafia.Converters;

/// <summary>
/// Convert <see cref="GameRole"/> to <see cref="IBrush"/>
/// </summary>
public class RoleColorBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!targetType.IsAssignableTo(typeof(IBrush)) || value is not GameRole role)
            return new BindingNotification(new InvalidCastException("Wrong target or value..."), BindingErrorType.Error,
                new SolidColorBrush(Color.FromRgb(250,50,50)));

        return role switch
        {
            GameRole.Peasant => new SolidColorBrush(Color.FromArgb(255, 139, 0, 0)),
            GameRole.Detective => new SolidColorBrush(Color.FromArgb(255, 59, 70, 91)),
            GameRole.Mafia => new SolidColorBrush(Color.FromArgb(255, 105, 105, 105)),
            GameRole.Don => new SolidColorBrush(Color.FromArgb(255, 55, 55, 55)),
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