using SeatUsageSystem.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SeatUsageSystem.Views.Pages
{
    public partial class UsagePage : INavigableView<UsageViewModel>
    {
        public UsageViewModel ViewModel { get; }

        public UsagePage(UsageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
