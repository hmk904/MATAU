using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DTO
{
    public class UnitDTO 
    {

        public int Id { get; set; }
        public int HouseId { get; set; }
        // 두 값을 결합한 새로운 속성
        public string CombinedId => $"{HouseId} - {Id}";

        public string Size { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Price { get; set; }

        // 체크박스 상태 추가
        public bool IsSelected { get; set; } = false;
        
        // 체크박스 활성화 여부
        public bool IsEnabled => Status != "InUse"; // InUse면 false

    }
}
