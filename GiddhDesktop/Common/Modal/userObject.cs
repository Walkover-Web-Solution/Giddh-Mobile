using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiddhDesktop.Common.Modal
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

    public class voucherType
    {
        public string code { get; set; }
        public string shortCode { get; set; }
        public string getShortCode(string codee)
        {
            return shortCode;
        }
    }
}
