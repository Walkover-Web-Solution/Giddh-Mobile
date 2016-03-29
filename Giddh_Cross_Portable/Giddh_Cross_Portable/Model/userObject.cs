using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable.Model
{
    public class userObject
    {
        public userDetail user { get; set; }
        public string authKey { get; set; }
    }

    public class userDetail
    {
        public string name { get; set; }
        public string email { get; set; }
        public string contactNo { get; set; }
        public string uniqueName { get; set; }
    }
}
