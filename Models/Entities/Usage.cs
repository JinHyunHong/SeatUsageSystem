using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.Entities
{
    public class Usage
    {
        public int UsageId { get; set; }

        public int MemberId { get; set; }

        public int SeatId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Member Member { get; set; } = null!;

        public Seat Seat { get; set; } = null!;
    }
}