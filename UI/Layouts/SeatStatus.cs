using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.UI.Layouts
{
    public enum SeatStatus
    {
        None = 0,

        /// <summary>
        /// 이용가능
        /// </summary>
        Available = 1,

        /// <summary>
        /// 이용중
        /// </summary>
        InUse = 2,

        /// <summary>
        /// 이용불가
        /// </summary>
        Unavailable = 3
    }

    public static class SeatStatusMapper
    {
        private static readonly Dictionary<string, SeatStatus> _map = new()
        {
            ["1"] = SeatStatus.Available,
            ["2"] = SeatStatus.InUse,
            ["3"] = SeatStatus.Unavailable
        };

        private static readonly Dictionary<SeatStatus, string> _reverseMap =
        _map.ToDictionary(x => x.Value, x => x.Key);

        public static SeatStatus ToStatus(string code)
        {
            return code != null && _map.TryGetValue(code, out var status)
                ? status
                : SeatStatus.None;
        }

        public static string ToCode(SeatStatus status)
        {
            return _reverseMap.TryGetValue(status, out var code)
                ? code
                : throw new InvalidOperationException($"Invalid SeatStatus {status}");
        }
    }

    public static class SeatStatusExtensions
    {
        /// <summary>
        /// 화면 표시용
        /// </summary>
        public static string ToText(this SeatStatus status)
        {
            return status switch
            {
                SeatStatus.Available => "이용가능",
                SeatStatus.InUse => "이용중",
                SeatStatus.Unavailable => "사용불가",
                _ => "-"
            };
        }
    }
}
