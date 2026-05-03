using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Common.Enums
{
    /// <summary>
    /// 좌석 이용 시작 요청의 처리 결과
    /// </summary>
    public enum StartUsageResult
    {
        /// <summary>
        /// 정상적으로 좌석 이용이 시작됨
        /// </summary>
        Success,

        /// <summary>
        /// 이미 다른 좌석을 이용 중인 사용자
        /// </summary>
        AlreadyUsing,

        /// <summary>
        /// 좌석이 존재하지 않음
        /// </summary>
        SeatNotFound,

        /// <summary>
        /// 해당 좌석이 현재 사용 가능한 상태가 아님
        /// </summary>
        SeatUnavailable,

        /// <summary>
        /// 알 수 없는 이유로 실패
        /// </summary>
        Fail
    }
}
