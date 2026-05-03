using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.UI.Layouts
{
    public enum InOutStatus
    {
        None = 0,

        /// <summary>
        /// 입실
        /// </summary>
        In = 1,

        /// <summary>
        /// 퇴실
        /// </summary>
        Out = 2
    }

    public static class InOutStatusMapper
    {
        private static readonly Dictionary<string, InOutStatus> _map = new()
        {
            ["1"] = InOutStatus.In,
            ["2"] = InOutStatus.Out,
        };

        private static readonly Dictionary<InOutStatus, string> _reverseMap =
        _map.ToDictionary(x => x.Value, x => x.Key);

        public static InOutStatus ToStatus(string code)
        {
            return code != null && _map.TryGetValue(code, out var status)
                ? status
                : InOutStatus.None;
        }

        public static string ToCode(InOutStatus status)
        {
            return _reverseMap.TryGetValue(status, out var code)
                ? code
                : throw new InvalidOperationException($"Invalid SeatStatus {status}");
        }
    }

    public static class InOutStatusExtensions
    {
        /// <summary>
        /// 화면 표시용
        /// </summary>
        public static string ToText(this InOutStatus status)
        {
            return status switch
            {
                InOutStatus.In => "입실",
                InOutStatus.Out => "퇴실",
                _ => "-"
            };
        }
    }
}
