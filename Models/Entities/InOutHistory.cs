using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.Entities
{
    public class InOutHistory
    {
        public string InOutYmd { get; set; } = string.Empty;

        public int InOutSeq { get; set; }

        public int UsageId { get; set; }

        public string InOutCd { get; set; } = string.Empty;

        public string InOutTime { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }

        public Usage Usage { get; set; } = null!;
    }
}