using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.Entities
{
    public class CommonCodeManage
    {
        public string LargeGroup { get; set; } = string.Empty;

        public string MiddleGroup { get; set; } = string.Empty;

        public string SmallGroup { get; set; } = string.Empty;

        public string? ConfigValue1 { get; set; }

        public string? ConfigValue2 { get; set; }

        public string? ConfigValue3 { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}