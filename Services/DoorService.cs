using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.UI.Layouts;
using System.Linq.Expressions;

namespace SeatUsageSystem.Services
{
    public class DoorService : IDoorService
    {
        private readonly IInOutHistoryRepository _historyRepository;

        public DoorService(
            IInOutHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        /// <summary>
        /// 마지막 상태 조회 + 모든 예외 검증
        /// </summary>
        private async Task<InOutStatus> GetLastStatusOrThrow(int usageId)
        {
            if (usageId <= 0)
            {
                throw new ArgumentException("유효하지 않은 usageId입니다.", nameof(usageId));
            }

            var last = await _historyRepository.GetLastAsync(usageId);

            if (last == null)
            {
                throw new InvalidOperationException("이용 이력이 존재하지 않습니다.");
            }

            if (string.IsNullOrWhiteSpace(last.InOutCd))
            {
                throw new InvalidOperationException("출입 상태 코드가 없습니다.");
            }

            var status = InOutStatusMapper.ToStatus(last.InOutCd);

            if (status == InOutStatus.None)
            {
                throw new InvalidOperationException("출입 상태가 유효하지 않습니다.");
            }

            return status;
        }

        public async Task OpenDoorAsync(int usageId)
        {
            var status = await GetLastStatusOrThrow(usageId);

            // 이미 문이 열려있으면 막기
            if (status == InOutStatus.In)
            {
                throw new InvalidOperationException($"이미 입실 상태입니다.");
            }

            await SaveHistoryAsync(usageId, InOutStatus.In);
        }

        public async Task CloseDoorAsync(int usageId)
        {
            var status = await GetLastStatusOrThrow(usageId);

            // 이미 닫혀있으면 막기
            if (status == InOutStatus.Out)
            {
                throw new InvalidOperationException($"이미 퇴실 상태입니다.");
            }

            await SaveHistoryAsync(usageId, InOutStatus.Out);
        }

        private async Task SaveHistoryAsync(int usageId, InOutStatus status)
        {
            var now = DateTime.Now;

            var ymd = now.ToString("yyyyMMdd");
            var time = now.ToString("HHmmss");

            var seq = await _historyRepository.GetNextSeqAsync(ymd);

            var history = new InOutHistory
            {
                InOutYmd = ymd,
                InOutSeq = seq,
                UsageId = usageId,
                InOutCd = InOutStatusMapper.ToCode(status),
                InOutTime = time,
                UpdatedAt = now
            };

            await _historyRepository.AddAsync(history);
        }
    }
}