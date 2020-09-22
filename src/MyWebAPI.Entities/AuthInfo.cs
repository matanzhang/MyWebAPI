using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebAPI.Entities
{
    public class AuthInfo
    {
        public string UserId { get; set; }
        public DateTime Expires { get; set; }
    }
}
