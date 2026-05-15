using SeatUsageSystem.Common.Enums;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.DTOs;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.Services;
using SeatUsageSystem.UI.Layouts;
using SeatUsageSystem.UI.Models;
using SeatUsageSystem.Views.Pages;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace SeatUsageSystem.ViewModels.Pages
{
    public partial class UsageViewModel : ObservableObject, INavigationAware
    {
        #region FIELDS

        private bool _isInitialized = false;

        private readonly IAuthService _authService;

        private readonly IAuthGuardService _authGuardService;

        private readonly IDialogService _dialogService;

        private readonly IDatabase<Seat> _seatRepository;

        private readonly IUsageRepository _usageRepository;

        private readonly IUsageService _usageService;

        #endregion

        #region PROPERTIES

        [ObservableProperty]
        private ObservableCollection<SeatItem> _seats = new();

        [ObservableProperty]
        private SeatItem? _selectedSeat;

        [ObservableProperty]
        private Usage? _currentUsage;

        public string? CurrentSeatLabel => Seats.FirstOrDefault(x => x.Id == CurrentUsage?.SeatId)?.Label;

        public bool IsUsing => CurrentUsage != null;

        #endregion

        #region CONSTRUCTOR

        public UsageViewModel(IAuthService authService, IAuthGuardService authGuardService, IDialogService dialogService,
                              IDatabase<Seat> seatRepository, IUsageRepository usageRepository, IUsageService usageService)
        {
            _authService = authService;
            _authGuardService = authGuardService;
            _dialogService = dialogService;
            _seatRepository = seatRepository;
            _usageRepository = usageRepository;
            _usageService = usageService;
        }

        #endregion

        #region COMMANDS

        /// <summary>
        /// 좌석 선택
        /// </summary>
        /// <param name="seat"></param>
        [RelayCommand]
        private void SelectSeat(SeatItem seat)
        {
            if (seat is null)
            {
                return;
            }

            // 이용 가능만 선택 가능
            if (seat.Status != SeatStatus.Available)
            {
                return;
            }

            // 기존 선택 해제
            foreach (var s in Seats)
            {
                s.IsSelected = false;
            }

            // 선택
            seat.IsSelected = true;

            // 선택된 좌석 저장
            SelectedSeat = seat;
        }

        /// <summary>
        /// 좌석 이용 시작
        /// </summary>
        [RelayCommand]
        private async Task StartUsage()
        {
            if (!await _authGuardService.CheckAndRedirectLogin())
            {
                return;
            }

            var seat = SelectedSeat;

            if (seat is null)
            {
                _dialogService.ShowMessage("이용할 좌석이 선택되지 않았습니다.");
                return;
            }

            if (_dialogService.ShowConfirm($"{seat.Label} 좌석을 이용하시겠습니까?") != true)
            {
                return;
            }

            var user = _authService.CurrentUserRequired;

            var result = await _usageService.StartUsageAsync(user.MemberId, seat.Id);

            switch (result.Result)
            {
                case StartUsageResult.AlreadyUsing:
                    _dialogService.ShowMessage("이미 사용중인 좌석이 있습니다.");
                    return;

                case StartUsageResult.SeatNotFound:
                    _dialogService.ShowMessage("좌석 정보를 찾을 수 없습니다.");
                    return;

                case StartUsageResult.SeatUnavailable:
                    _dialogService.ShowMessage("해당 좌석을 사용할 수 없습니다.");
                    return;

                case StartUsageResult.Fail:
                    _dialogService.ShowMessage("좌석 이용에 실패하였습니다.");
                    return;

                case StartUsageResult.Success:
                    break;
            }

            _dialogService.ShowMessage($"{seat.Label} 좌석 이용이 시작되었습니다.");

            await RefreshViewModelAsync();
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

        private async Task InitializeViewModelAsync()
        {
            await InitSeatsAsync();

            CurrentUsage = await _usageRepository.GetActiveUsageAsync(_authService.CurrentUserRequired.MemberId);
            _isInitialized = true;
        }

        partial void OnCurrentUsageChanged(Usage? value)
        {
            OnPropertyChanged(nameof(IsUsing));
            OnPropertyChanged(nameof(CurrentSeatLabel));
        }

        /// <summary>
        /// 실제 DB 값과 매핑하여 Seat 객체 생성
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="dbSeat"></param>
        /// <returns></returns>
        private SeatItem MapToSeatItem(string displayName, Seat? dbSeat)
        {
            var pos = SeatLayoutMap.Get(displayName);

            return new SeatItem
            {
                Id = dbSeat?.SeatId ?? 0,
                Label = displayName,

                X = pos.X,
                Y = pos.Y,

                Status = SeatStatusMapper.ToStatus(dbSeat?.UsageStateCd ?? string.Empty)
            };
        }

        private async Task InitSeatsAsync()
        {
            var seatsFromDb = await _seatRepository.GetAllAsync();

            var dbMap = seatsFromDb.ToDictionary(x => x.DisplayName);

            Seats = new ObservableCollection<SeatItem>(
                SeatLayoutMap.Keys.Select(displayName =>
                    MapToSeatItem(displayName, dbMap.GetValueOrDefault(displayName))
                )
            );
        }

        public async Task OnNavigatedToAsync()
        {
            bool isLogin = await _authGuardService.CheckAndRedirectLogin();
            if (!isLogin)
            {
                return;
            }

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

        /// <summary>
        /// 좌석 상태 다시 조회
        /// </summary>
        /// <returns></returns>
        private async Task SetSeatStatusAsync()
        {
            var seatsFromDb = await _seatRepository.GetAllAsync();

            var dbMap = seatsFromDb
                .ToDictionary(x => x.DisplayName);

            foreach (var seat in Seats)
            {
                if (dbMap.TryGetValue(seat.Label, out var dbSeat))
                {
                    seat.Status = SeatStatusMapper.ToStatus(dbSeat.UsageStateCd);
                }
                else
                {
                    seat.Status = SeatStatus.Unavailable;
                }

                seat.IsSelected = false;
            }
        }

        /// <summary>
        /// 현재 상태를 다시 조회하여 화면 데이터를 최신 상태로 갱신
        /// </summary>
        /// <returns></returns>
        private async Task RefreshViewModelAsync()
        {
            await SetSeatStatusAsync();

            SelectedSeat = null;

            CurrentUsage = await _usageRepository.GetActiveUsageAsync(_authService.CurrentUserRequired.MemberId);
        }

        #endregion
    }
}
