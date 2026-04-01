using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Typist.Desktop.Services;

namespace Typist.Desktop.Helpers;

public class CharStateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CharState state)
        {
            return state switch
            {
                CharState.Correct   => new SolidColorBrush(Color.FromRgb(0, 210, 106)),
                CharState.Incorrect => new SolidColorBrush(Color.FromRgb(255, 82, 82)),
                CharState.Current   => new SolidColorBrush(Colors.White),
                CharState.Pending   => new SolidColorBrush(Color.FromRgb(100, 100, 120)),
                _                   => new SolidColorBrush(Color.FromRgb(100, 100, 120))
            };
        }
        return new SolidColorBrush(Color.FromRgb(100, 100, 120));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>Returns a cursor underline brush when IsCurrent == true, else Transparent.</summary>
public class BoolToCursorBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isCurrent = value is bool b && b;
        return isCurrent
            ? new SolidColorBrush(Color.FromRgb(0, 210, 214))
            : new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool invert = parameter?.ToString() == "Invert";
        bool boolValue = value is bool b && b;
        if (invert) boolValue = !boolValue;
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>Shows element when value is NOT null; ConverterParameter=Invert shows when null.</summary>
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isNull = value == null;
        bool invert = parameter?.ToString() == "Invert";
        bool show = invert ? isNull : !isNull;
        return show ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>Collapses element when string is null or whitespace.</summary>
public class NullOrEmptyToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool hasContent = value is string s && !string.IsNullOrWhiteSpace(s);
        return hasContent ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class GradeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double wpm)
        {
            return wpm switch
            {
                >= 100 => "S",
                >= 80  => "A",
                >= 60  => "B",
                >= 40  => "C",
                >= 20  => "D",
                _      => "F"
            };
        }
        return "?";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class GradeToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double wpm)
        {
            return wpm switch
            {
                >= 100 => new SolidColorBrush(Color.FromRgb(255, 215, 0)),
                >= 80  => new SolidColorBrush(Color.FromRgb(0, 210, 255)),
                >= 60  => new SolidColorBrush(Color.FromRgb(0, 210, 106)),
                >= 40  => new SolidColorBrush(Color.FromRgb(255, 180, 0)),
                >= 20  => new SolidColorBrush(Color.FromRgb(255, 130, 67)),
                _      => new SolidColorBrush(Color.FromRgb(255, 82, 82))
            };
        }
        return new SolidColorBrush(Colors.White);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class PercentToAngleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double percent)
            return percent / 100.0 * 360.0;
        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class TimeFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            return ts.ToString(@"m\:ss");
        }
        return "0:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
