using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.DTO
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public long Expires { get; set; }
    }
}
