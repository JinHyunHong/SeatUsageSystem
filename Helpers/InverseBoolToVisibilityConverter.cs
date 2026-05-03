using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Appearance;

namespace SeatUsageSystem.Helpers
{
    // Boolean → Visibility Inserse Converter
    internal class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value != Visibility.Visible;
        }
    }
}