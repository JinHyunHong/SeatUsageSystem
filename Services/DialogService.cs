using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models;
using System.Linq.Expressions;

namespace SeatUsageSystem.Services
{
    /// <summary>
    /// WPF MessageBox 기반 DialogService 구현
    /// </summary>
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title = "알림")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool ShowConfirm(string message, string title = "확인")
        {
            return MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            ) == MessageBoxResult.Yes;
        }
    }
}