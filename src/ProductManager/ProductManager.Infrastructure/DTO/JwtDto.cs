using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public long Expires { get; set; }
    }
}
