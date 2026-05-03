using Microsoft.Win32;
using SeatUsageSystem.ViewModels.Pages;
using SeatUsageSystem.ViewModels.Windows;
using SeatUsageSystem.Views.Windows;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace SeatUsageSystem.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void btnAdminInfo_Click(object sender, RoutedEventArgs e)
        {
            // Info Open
            btnAdminInfo.ContextMenu.IsOpen = true;
        }
    }
}
