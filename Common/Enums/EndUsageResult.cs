using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Common.Enums
{
    /// <summary>
    /// 좌석 이용 종료 요청의 처리 결과
    /// </summary>
    public enum EndUsageResult
    {    
        /// <summary>
        /// 정상적으로 좌석 이용이 종료됨
        /// </summary>
        Success,

        /// <summary>
        /// 사용자가 현재 좌석을 이용 중이 아님
        /// </summary>
        UserNotInUse,

        /// <summary>
        /// 해당 좌석이 존재하지 않음
        /// </summary>
        SeatNotFound,

        /// <summary>
        /// 데이터베이스 처리 중 오류 발생
        /// </summary>
        Fail
    }
}
