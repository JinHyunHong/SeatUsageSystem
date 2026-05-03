namespace SeatUsageSystem.Interfaces
{
    /// <summary>
    /// UI 알림/확인 대화창을 추상화한 서비스
    /// ViewModel이 UI 기술(WPF MessageBox 등)에 직접 의존하지 않도록 분리
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// 단순 정보 메시지 출력
        /// </summary>
        void ShowMessage(string message, string title = "알림");

        /// <summary>
        /// 사용자 확인이 필요한 경우 (Yes/No)
        /// </summary>
        bool ShowConfirm(string message, string title = "확인");
    }
}