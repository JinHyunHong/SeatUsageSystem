using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.Entities
{
    public class Area
    {
        public int AreaId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}