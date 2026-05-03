using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Common.Enums;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.DTOs;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.Repositories;
using SeatUsageSystem.UI.Layouts;
using System.Linq.Expressions;

namespace SeatUsageSystem.Services
{
    /// <summary>
    /// 좌석 관련 비즈니스 로직 처리
    /// </summary>
    public class UsageService : IUsageService
    {
        private readonly IUsageRepository _usageRepository;
        private readonly IDoorService _doorService;

        public UsageService(IUsageRepository usageRepository, IDoorService doorService)
        {
            _usageRepository = usageRepository;
            _doorService = doorService;
        }


        /// <summary>
        /// 좌석 사용 시작
        /// </summary>
        public async Task<StartUsageResultDto> StartUsageAsync(int memberId, int seatId)
        {
            var result = await _usageRepository.StartUsageAsync(memberId, seatId);

            if (result.Result != StartUsageResult.Success)
            {
                return result;
            }

            await _doorService.OpenDoorAsync(result.UsageId);

            return result;
        }

        /// <summary>
        /// 좌석 사용 종료
        /// </summary>
        public async Task<EndUsageResultDto> EndUsageAsync(int memberId)
        {
            var result = await _usageRepository.EndUsageAsync(memberId);

            if (result.Result != EndUsageResult.Success)
            {
                return result;
            }

            await _doorService.CloseDoorAsync(result.UsageId);

            return result;
        }

        /// <summary>
        /// [이용 내역] 기간별 사용 목록 조회
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public async Task<List<UsageListDto>> GetUsagesByDateRangeAsync(DateOnly startDate, DateOnly endDate, int? areaId = null)
        {
            var usages = await _usageRepository.GetUsagesByDateRangeAsync(startDate, endDate, areaId);

            return usages.Select(u => new UsageListDto
            {
                MemberName = u.Member.MemberName,
                Phone = u.Member.PhoneNumber,
                SeatLabel = u.Seat.DisplayName,
                Status = SeatStatusMapper.ToStatus(u.Seat.UsageStateCd).ToText(),
                StartAt = u.StartAt,
                EndAt = u.EndAt
            }).ToList();
        }
    }
}