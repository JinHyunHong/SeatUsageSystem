using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.DTOs;
using Wpf.Ui.Abstractions.Controls;

namespace SeatUsageSystem.ViewModels.Pages
{
    public partial class UsageListViewModel : ObservableObject, INavigationAware
    {
        #region FIELDS

        private bool _isInitialized = false;

        private readonly IAuthGuardService _authGuardService;

        private readonly IUsageService _usageService;

        #endregion

        #region PROPERTIES

        [ObservableProperty]
        private DateTime _startDate;

        [ObservableProperty]
        private DateTime _endDate;

        [ObservableProperty]
        private List<UsageListDto> _usageListDtos = new();

        #endregion

        #region CONSTRUCTOR

        public UsageListViewModel(IAuthGuardService authGuardService, IUsageService usageService)
        {
            _authGuardService = authGuardService;
            _usageService = usageService;
        }

        #endregion

        #region COMMANDS
        
        [RelayCommand]
        public async Task OnSearch()
        {
            await SetUsageData();
        
        }
        #endregion

        #region METHODS

        public async Task OnNavigatedToAsync()
        {
            bool isLogin = await _authGuardService.CheckAndRedirectLogin();
            if (!isLogin)
            {
                return;
            }

            if (!_isInitialized)
            {
                InitializeViewModel();
            }

            await SetUsageData();
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private async Task SetUsageData()
        {
            DateOnly start = DateOnly.FromDateTime(StartDate);
            DateOnly end = DateOnly.FromDateTime(EndDate);

            UsageListDtos = await _usageService.GetUsagesByDateRangeAsync(start, end);
        }

        private void InitializeViewModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            _isInitialized = true;
        }

        #endregion
    }
}