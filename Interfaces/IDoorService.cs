using SeatUsageSystem.Models;

namespace SeatUsageSystem.Interfaces
{
    /// <summary>
    /// 문 연관 물리 동작
    /// </summary>
    public interface IDoorService
    {
        Task OpenDoorAsync(int usageId);
        Task CloseDoorAsync(int usageId);
    }
}