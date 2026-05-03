using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.Entities
{
    public class Seat
    {
        public int SeatId { get; set; }

        public int AreaId { get; set; }

        public string UsageStateCd { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }

        public Area Area { get; set; } = null!;
    }
}