using SeatUsageSystem.Common.Enums;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.DTOs;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.Services;
using SeatUsageSystem.UI.Layouts;
using SeatUsageSystem.Views.Pages;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace SeatUsageSystem.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        #region FIELDS

        private bool _isInitialized = false;

        private readonly INavigationService _navigationService;

        private readonly IAuthService _authService;

        private readonly IAuthGuardService _authGuardService;

        private readonly IDialogService _dialogService;

        private readonly IDoorService _doorService;

        private readonly IDatabase<Area> _areaRepository;

        private readonly IDatabase<Seat> _seatRepository;
        
        private readonly IUsageRepository _usageRepository;

        private readonly IUsageService _usageService;

        private const string LoginRequiredMessage = "로그인이 필요합니다.";

        #endregion

        #region PROPERTIES

        [ObservableProperty]
        private string _welcomeText = LoginRequiredMessage;

        [ObservableProperty]
        private List<AreaItem>? _areas = null;

        [ObservableProperty]
        private int _selectedAreaId = -1;

        /// <summary>
        /// 전체 좌석 수
        /// </summary>
        [ObservableProperty]
        private int? _totalSeatCount = null;

        /// <summary>
        /// 이용 가능 좌석 수
        /// </summary>
        [ObservableProperty]
        private int? _availableSeatCount = null;

        /// <summary>
        /// 이용 중 좌석 수
        /// </summary>
        [ObservableProperty]
        private int? _InUseSeatCount = null;

        /// <summary>
        /// 오늘 이용 건수
        /// </summary>
        [ObservableProperty]
        private int? _todayUsageCount = null;

        #endregion

        #region CONSTRUCTOR

        public DashboardViewModel(INavigationService navigationService, IAuthService authService, IAuthGuardService authGuardService, 
                                  IDialogService dialogService, IDoorService doorService, IDatabase<Area> areaRepository,
                                  IDatabase<Seat> seatRepository, IUsageRepository usageRepository, IUsageService usageService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _authGuardService = authGuardService;
            _dialogService = dialogService;
            _doorService = doorService;
            _areaRepository = areaRepository;
            _seatRepository = seatRepository;
            _usageRepository = usageRepository;
            _usageService = usageService;

            _authService.AuthChanged += OnAuthChanged;
        }

        #endregion

        #region COMMANDS
        
        [RelayCommand]
        private async Task GoUsage()
        {
            bool isLogin = await _authGuardService.CheckAndRedirectLogin();
            if (!isLogin)
            {
                return;
            }

            GoPage<UsagePage>();
        }

        [RelayCommand]
        private async Task OpenDoor()
        {
            try
            {
                var usage = await GetValidUsageAsync();
                if (usage is null)
                {
                    return;
                }

                await _doorService.OpenDoorAsync(usage.UsageId);

                _dialogService.ShowMessage("문이 열렸습니다.");
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }


        [RelayCommand]
        private async Task CloseDoor()
        {
            try
            {
                var usage = await GetValidUsageAsync();
                if (usage is null)
                {
                    return;
                }

                await _doorService.CloseDoorAsync(usage.UsageId);

                _dialogService.ShowMessage("문이 닫혔습니다.");
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }

        [RelayCommand]
        private async Task OnSearchSeatStatusAsync()
        {
            await SetSeatStatusAsync();
        }

        /// <summary>
        /// 이용중인 좌석 퇴실처리
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnEndUsage()
        {
            if (!await _authGuardService.CheckAndRedirectLogin())
            {
                return;
            }

            var user = _authService.CurrentUserRequired;

            if (_dialogService.ShowConfirm($"현재 이용중인 좌석을 퇴실 처리하시겠습니까?") != true)
            {
                return;
            }

            var result = await _usageService.EndUsageAsync(user.MemberId);

            switch (result.Result)
            {
                case EndUsageResult.UserNotInUse:
                    _dialogService.ShowMessage("현재 이용중인 좌석이 없습니다.");
                    return;

                case EndUsageResult.SeatNotFound:
                    _dialogService.ShowMessage("좌석 정보를 찾을 수 없습니다.");
                    return;

                case EndUsageResult.Fail:
                    _dialogService.ShowMessage("퇴실 처리에 실패하였습니다.");
                    return;

                case EndUsageResult.Success:
                    break;
            }
            _dialogService.ShowMessage("좌석 이용이 종료되었습니다.");

            await RefreshViewModelAsync();
        }

        #endregion

        #region METHODS

        private void OnAuthChanged()
        {
            if (_authService.IsLoggedIn)
            {
                var name = _authService.CurrentUser?.MemberName;

                WelcomeText = string.IsNullOrWhiteSpace(name)
                    ? "환영합니다."
                    : $"{name}님 환영합니다.";

                return;
            }

            WelcomeText = LoginRequiredMessage;
        }

        public async Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
            {
                await InitializeViewModelAsync();
                return;
            }

            await RefreshViewModelAsync();
        }

        public Task OnNavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        private async Task InitializeViewModelAsync()
        {
            var areas = await _areaRepository.GetAllAsync();

            Areas = areas.Select(x => new AreaItem
            {
                Id = x.AreaId,
                Name = x.DisplayName
            }).ToList();

            // 전체 조회 조건 추가
            Areas.Insert(0, new AreaItem
            {
                Id = 0,
                Name = "전체"
            });

            SelectedAreaId = 0;

            await SetSeatStatusAsync();

            OnAuthChanged();
            _isInitialized = true;
        }

        /// <summary>
        /// 페이지 이동
        /// </summary>
        /// <param name="pageType"></param>
        private void GoPage<T>() where T : Page
        {
            _navigationService.Navigate(typeof(T));
        }

        /// <summary>
        /// 이용중인 Usage 있는지 확인
        /// </summary>
        /// <returns></returns>
        private async Task<Usage?> GetValidUsageAsync()
        {
            bool isLogin = await _authGuardService.CheckAndRedirectLogin();
            if (!isLogin)
            {
                return null;
            }

            var user = _authService.CurrentUser;

            if (user is null)
            {
                _dialogService.ShowMessage("로그인이 필요합니다.");
                return null;
            }

            var usage = await _usageRepository.GetActiveUsageAsync(user.MemberId);

            if (usage is null)
            {
                _dialogService.ShowMessage("이용중인 좌석이 없습니다.");
                return null;
            }

            return usage;
        }

        /// <summary>
        /// 현재 상태를 다시 조회하여 화면 데이터를 최신 상태로 갱신
        /// </summary>
        /// <returns></returns>
        private async Task RefreshViewModelAsync()
        {
            await SetSeatStatusAsync();
        }

        /// <summary>
        /// 좌석 현황 데이터 설정
        /// </summary>
        private async Task SetSeatStatusAsync()
        {
            var seats = await _seatRepository.GetAllAsync();
            var filteredSeats = SelectedAreaId == 0 ? seats : seats.Where(x => x.AreaId == SelectedAreaId);

            int total = 0;
            int available = 0;
            int inUse = 0;

            foreach (var seat in filteredSeats)
            {   
                var status = SeatStatusMapper.ToStatus(seat.UsageStateCd);

                total++;

                switch (status)
                {
                    case SeatStatus.Available:
                        available++;
                        break;

                    case SeatStatus.InUse:
                        inUse++;
                        break;
                }
            }

            TotalSeatCount = total;
            AvailableSeatCount = available;
            InUseSeatCount = inUse;

            TodayUsageCount = await _usageRepository
                .GetTodayUsageCountAsync(SelectedAreaId == 0 ? null : SelectedAreaId);
        }

        #endregion
    }
}
