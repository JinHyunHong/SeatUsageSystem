using SeatUsageSystem.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SeatUsageSystem.Views.Pages
{
    public partial class UsageListPage : INavigableView<UsageListViewModel>
    {
        public UsageListViewModel ViewModel { get; }

        public UsageListPage(UsageListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
