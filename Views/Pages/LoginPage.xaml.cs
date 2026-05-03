using SeatUsageSystem.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SeatUsageSystem.Views.Pages
{
    public partial class LoginPage : INavigableView<LoginViewModel>
    {
        public LoginViewModel ViewModel { get; }

        public LoginPage(LoginViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
