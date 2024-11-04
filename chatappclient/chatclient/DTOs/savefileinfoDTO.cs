using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLUSER.DTOs
{
    internal class savefileinfoDTO
    {
        public string Username { get; set; }
        public string Groupname { get; set; }
        public string Channelname { get; set; }
        public string Message { get; set; }
        public string[] filename { get; set; }
    }
}
