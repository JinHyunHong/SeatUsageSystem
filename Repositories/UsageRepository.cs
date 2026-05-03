using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Common.Enums;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.DTOs;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.UI.Layouts;
using System.Linq.Expressions;

namespace SeatUsageSystem.Repositories
{
    public class UsageRepository : IUsageRepository
    {
        private readonly AppDbContext _context;

        public UsageRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 회원이 현재 사용중인 Usage 조회
        /// (한 사람당 1개만 존재)
        /// </summary>
        public async Task<Usage?> GetActiveUsageAsync(int memberId)
        {
            return await _context.Usages
                .FirstOrDefaultAsync(x => x.MemberId == memberId && x.EndAt == null);
        }

        /// <summary>
        /// 좌석 사용 시작
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="seatId"></param>
        /// <returns>처리 결과, usageId</returns>
        public async Task<StartUsageResultDto> StartUsageAsync(int memberId, int seatId)
        {
            // 1. 이미 사용중인지 체크
            var active = await GetActiveUsageAsync(memberId);
            if (active is not null)
            {
                return new StartUsageResultDto
                {
                    Result = StartUsageResult.AlreadyUsing
                };
            }

            // 2. 좌석 조회
            var seat = await _context.Seats.FindAsync(seatId);

            if (seat is null)
            {
                return new StartUsageResultDto
                {
                    Result = StartUsageResult.SeatUnavailable
                };
            }

            if (seat is null || SeatStatusMapper.ToStatus(seat.UsageStateCd) != SeatStatus.Available)
            {
                return new StartUsageResultDto
                {
                    Result = StartUsageResult.SeatNotFound
                };
            }

            // 3. Usage 생성
            var usage = new Usage
            {
                MemberId = memberId,
                SeatId = seatId,
                StartAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Usages.Add(usage);

            // 4. 좌석 상태 변경
            seat.UsageStateCd = SeatStatusMapper.ToCode(SeatStatus.InUse);

            try
            {
                // 5. 저장
                await _context.SaveChangesAsync();

                // 6. usageId 반환
                return new StartUsageResultDto
                {
                    Result = StartUsageResult.Success,
                    UsageId = usage.UsageId
                };
            }
            catch (DbUpdateException)
            {
                // 이미 누가 선점했거나, 중복 발생
                return new StartUsageResultDto
                {
                    Result = StartUsageResult.Fail
                };
            }
        }

        /// <summary>
        /// 좌석 사용 종료
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>처리 결과, usageId</returns>
        public async Task<EndUsageResultDto> EndUsageAsync(int memberId)
        {
            // 1. 이미 사용중인지 체크
            var usage = await GetActiveUsageAsync(memberId);
            if (usage is null)
            {
                return new EndUsageResultDto()
                {
                    Result = EndUsageResult.UserNotInUse
                };
            }

            // 2. 좌석 조회(usage가 이미 active면 seat 상태는 신뢰하지 않음)
            var seat = await _context.Seats.FindAsync(usage.SeatId);

            if (seat is null)
            {
                // 운영 관점: 데이터 정합성 문제
                return new EndUsageResultDto
                {
                    Result = EndUsageResult.SeatNotFound
                };
            }

            // 3. 좌석 상태 변경
            seat.UsageStateCd = SeatStatusMapper.ToCode(SeatStatus.Available);

            // 4. 사용 완료 처리
            usage.EndAt = DateTime.Now;

            try
            {
                // 5. 저장
                await _context.SaveChangesAsync();

                // 6. usageId 반환
                return new EndUsageResultDto()
                {
                    Result = EndUsageResult.Success,
                    UsageId = usage.UsageId
                };
            }
            catch (DbUpdateException)
            {
                return new EndUsageResultDto
                {
                    Result = EndUsageResult.Fail
                };
            }
        }

        /// <summary>
        /// 금일 전체 / 구역별 사용 건수 조회
        /// </summary>
        public async Task<int> GetTodayUsageCountAsync(int? areaId = null)
        {
            var today = DateTime.Today;

            return await _context.Usages
                .Where(u =>
                    u.StartAt >= today &&
                    (areaId == null || u.Seat.AreaId == areaId))
                .CountAsync();
        }

        /// <summary>
        /// 금일 사용 목록 조회 (필요 시)
        /// </summary>
        public async Task<List<Usage>> GetTodayUsagesAsync(int? areaId = null)
        {
            var today = DateTime.Today;

            var query = _context.Usages
                .Where(u =>
                    u.StartAt >= today &&
                    (areaId == null || u.Seat.AreaId == areaId));

            return await query.ToListAsync();
        }

        /// <summary>
        /// 기간별 사용 목록 조회
        /// </summary>
        public async Task<List<Usage>> GetUsagesByDateRangeAsync(DateOnly startDate, DateOnly endDate, int? areaId = null)
        {
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.AddDays(1).ToDateTime(TimeOnly.MinValue);

            return await _context.Usages
                .Include(u => u.Member)
                .Include(u => u.Seat)
                .Where(u =>
                    u.StartAt >= start &&
                    u.StartAt < end &&
                    (areaId == null || u.Seat.AreaId == areaId))
                .ToListAsync();
        }
    }
}