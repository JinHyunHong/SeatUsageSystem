using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.UI.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.UI.Models
{
    public partial class SeatItem : ObservableObject
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// DB에 저장된 실제 좌석 데이터 여부 (0 = 미연결 상태)
        /// </summary>
        public bool IsFromDb => Id > 0;

        public double X { get; set; }
        public double Y { get; set; }

        [ObservableProperty]
        public SeatStatus _status = SeatStatus.Available;

        [ObservableProperty]
        private bool _isSelected;
    }
}
