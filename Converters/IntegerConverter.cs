using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Mafia.Converters
{
    public class IntegerConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int number)
            {
                if (targetType.IsAssignableTo(typeof(string)))
                    return number.ToString();

                return new BindingNotification(new InvalidCastException("Convert int to ???"), BindingErrorType.Error);
            }

            return new BindingNotification(new InvalidCastException("Value is not integer..."), BindingErrorType.Error);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}
