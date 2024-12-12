using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DTO
{
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string Token { get; set; }
    }
}
