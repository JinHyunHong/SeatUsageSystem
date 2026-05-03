using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Appearance;

namespace SeatUsageSystem.Helpers
{
    public class PhoneNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string phone || string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            phone = phone.Replace("-", "");

            return phone.Length switch
            {
                11 => $"{phone[..3]}-{phone[3..7]}-{phone[7..]}",
                10 => phone.StartsWith("02")
                        ? $"{phone[..2]}-{phone[2..6]}-{phone[6..]}"
                        : $"{phone[..3]}-{phone[3..6]}-{phone[6..]}",
                9 => $"{phone[..2]}-{phone[2..5]}-{phone[5..]}",
                _ => phone
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}