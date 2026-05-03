using SeatUsageSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.DTOs
{
    /// <summary>
    /// 이용 내역 조회용 DTO
    /// </summary>
    public class UsageListDto
    {
        /// <summary>
        /// 이름
        /// </summary>
        public string MemberName { get; set; } = string.Empty;  

        /// <summary>
        /// 연락처
        /// </summary>
        public string Phone { get; set; } = string.Empty;       

        /// <summary>
        /// 좌석
        /// </summary>
        public string SeatLabel { get; set; } = string.Empty;   

        /// <summary>
        /// 상태
        /// </summary>
        public string Status { get; set; } = string.Empty;      

        /// <summary>
        /// 시작일시
        /// </summary>
        public DateTime StartAt { get; set; }                   

        /// <summary>
        /// 종료일시
        /// </summary>
        public DateTime? EndAt { get; set; }                    
    }
}