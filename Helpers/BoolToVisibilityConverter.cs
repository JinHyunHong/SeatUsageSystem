using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Appearance;

namespace SeatUsageSystem.Helpers
{
    // Boolean → Visibility Converter
    public class BoolToVisibilityConverter : IValueConverter
    {
        // UI 보여줄지 결정
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        // UI 상태를 다시 VM으로 보내는 기능
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
}