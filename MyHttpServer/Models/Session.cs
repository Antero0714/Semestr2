using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models
{
    public class Session
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
