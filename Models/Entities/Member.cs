using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.Models.Entities
{
    public class Member
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}