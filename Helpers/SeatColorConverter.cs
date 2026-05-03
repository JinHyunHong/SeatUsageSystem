using SeatUsageSystem.UI.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SeatUsageSystem.Helpers
{
    public class SeatColorConverter : IMultiValueConverter
    {
        /// <summary>
        /// Hex to Brush
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private Brush ToBrush(string hex)
        {
            var result = new BrushConverter().ConvertFromString(hex);
            return result as Brush ?? Brushes.Transparent;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool selected = values[0] is bool b && b;
            var status = (SeatStatus)values[1];

            if (selected)
            {
                return ToBrush("#1E88E5");
            }

            return status switch
            {
                SeatStatus.Available => ToBrush("#4CAF50"), // Green
                SeatStatus.InUse => ToBrush("#E53935"), // Red
                SeatStatus.Unavailable => ToBrush("#808080"), // Gray
                _ => ToBrush("#1E88E5") // Blue
            };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
    }
}
