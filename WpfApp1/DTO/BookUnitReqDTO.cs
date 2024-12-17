using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DTO
{
    public class BookUnitReqDTO
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public string UnitSize { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public string UserNote { get; set; } = string.Empty;
    }
}
