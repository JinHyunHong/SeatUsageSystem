using SeatUsageSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.DTOs
{
    /// <summary>
    /// 좌석 이용 시작 요청의 결과를 나타내는 DTO
    /// </summary>
    public class StartUsageResultDto
    {    
        /// <summary>
         /// 좌석 이용 시작 처리 결과 상태
         /// </summary>
        public StartUsageResult Result { get; set; }


        /// <summary>
        /// 좌석 이용 시작 성공 시 생성된 Usage 식별자
        /// </summary>
        public int UsageId { get; set; }
    }
}