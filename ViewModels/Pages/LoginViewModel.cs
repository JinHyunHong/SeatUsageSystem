using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.Services;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace SeatUsageSystem.ViewModels.Pages
{
    public partial class LoginViewModel : ObservableObject, INavigationAware
    {
        #region FIELDS

        private bool _isInitialized = false;

        private readonly IAuthService _authService;

        private readonly IDialogService _dialogService;

        private readonly IDatabase<Member> _memberRepository;

        #endregion

        #region PROPERTIES

        [ObservableProperty]
        private string _inputName = string.Empty;

        [ObservableProperty]
        private string _inputPhoneNumber = string.Empty;

        [ObservableProperty]
        private string? currentUserName = null;

        #endregion

        #region CONSTRUCTOR

        public LoginViewModel(IAuthService authService, IDialogService dialogService, IDatabase<Member> memberRepository)
        {
            _authService = authService;
            _dialogService = dialogService;
            _memberRepository = memberRepository;

            _authService.AuthChanged += OnAuthChanged;
        }

        #endregion

        #region COMMANDS

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (_authService.IsLoggedIn) // 이미 로그인된 경우(중복 로그인 방지)
                return;

            if (string.IsNullOrWhiteSpace(InputName) ||
                string.IsNullOrWhiteSpace(InputPhoneNumber))
            {
                _dialogService.ShowMessage("입력값을 확인하세요.");
                return;
            }

            var name = InputName.Trim();
            var digits = NormalizePhoneNumber(InputPhoneNumber);

            if (!ValidatePhoneNumber(digits))
            {
                _dialogService.ShowMessage("전화번호 형식이 올바르지 않습니다.");
                return;
            }

            var member = await GetMemberAsync(name, digits);

            if (member != null)
            {
                _authService.Login(member);
                _dialogService.ShowMessage("로그인 성공");
                return;
            }

            var result = _dialogService.ShowConfirm(
                "사용자가 없습니다. 회원가입 하시겠습니까?",
                "회원가입");

            if (result)
            {
                var newMember = await RegisterAsync(name, digits);
                if (newMember != null)
                {
                    _authService.Login(newMember);
                    _dialogService.ShowMessage("회원가입 및 로그인 완료");
                }
            }
        }

        [RelayCommand]
        private void Logout()
        {
            _authService.Logout();
            InputName = string.Empty;
            InputPhoneNumber = string.Empty;
            CurrentUserName = string.Empty;
            _dialogService.ShowMessage("로그아웃 완료");
        }

        #endregion

        #region METHODS

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        public bool IsLoggedIn => _authService.IsLoggedIn;

        private void OnAuthChanged()
        {
            CurrentUserName = _authService.CurrentUser?.MemberName ?? string.Empty;

            OnPropertyChanged(nameof(IsLoggedIn)); // 변경시 UI 다시그리도록 함
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        /// <summary>
        /// 숫자만 추출(010-1234-5678 -> 01012345678)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string NormalizePhoneNumber(string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// 전화번호 형식 검증
        /// </summary>
        /// <param name="digits">숫자만(01012345678)</param>
        /// <returns></returns>
        private bool ValidatePhoneNumber(string digits)
        {
            if (digits.Length < 9 || digits.Length > 11)
            {
                return false;
            }

            // 서울 지역번호(02)로 시작하는 경우(9 ~ 10자리)
            if (digits.StartsWith("02"))
            {
                return digits.Length is 9 or 10;
            }

            // 휴대폰 번호(010)로 시작하는 경우(11자리)
            if (digits.StartsWith("010"))
            {
                return digits.Length == 11;
            }

            // 031, 051, 064 등 그 외 지역번호(10자리, 11자리)
            return digits.Length is 10 or 11;
        }

        
        private async Task<Member?> GetMemberAsync(string name, string digits)
        {
            var members = await _memberRepository.FindAsync(m =>
                m.MemberName == name &&
                m.PhoneNumber == digits);

            return members.FirstOrDefault();
        }

        private async Task<Member?> RegisterAsync(string name, string digits)
        {
            var newMember = new Member
            {
                MemberName = name,
                PhoneNumber = digits,
                UpdatedAt = DateTime.Now
            };

            await _memberRepository.AddAsync(newMember);

            return newMember;
        }

        #endregion
    }
}
