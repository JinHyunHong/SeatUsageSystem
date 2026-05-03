using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.UI.Layouts
{
    // 좌석 위치 C#에서 관리, DB에서는 상태만 관리
    public static class SeatLayoutMap
    {
        public static IEnumerable<string> Keys => _map.Keys;

        private static readonly Dictionary<string, SeatPosition> _map = new()
        {
            // 상단 벽면
            ["A1"] = new SeatPosition(55, 50),
            ["A2"] = new SeatPosition(105, 50),
            ["A3"] = new SeatPosition(155, 50),
            ["A4"] = new SeatPosition(205, 50),

            // 중앙 좌측
            ["B1"] = new SeatPosition(276, 210),
            ["B3"] = new SeatPosition(276, 263),

            // 중앙 우측
            ["B2"] = new SeatPosition(328, 210),
            ["B4"] = new SeatPosition(328, 263),

            // 우측 벽면
            ["C1"] = new SeatPosition(420, 175),
            ["C2"] = new SeatPosition(420, 230),
            ["C3"] = new SeatPosition(420, 285),
            ["C4"] = new SeatPosition(420, 340),
            ["C5"] = new SeatPosition(420, 395),

            // 좌측 벽면
            ["D1"] = new SeatPosition(37, 190),
            ["D2"] = new SeatPosition(37, 250),
            ["D3"] = new SeatPosition(37, 310),
        };

        public static SeatPosition Get(string displayName)
        {
            if (_map.TryGetValue(displayName, out var pos))
            {
                return pos;
            }

            return new SeatPosition(0, 0);
        }
    }
}
