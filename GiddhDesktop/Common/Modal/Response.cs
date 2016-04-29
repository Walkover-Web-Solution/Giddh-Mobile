using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiddhDesktop.Common.Modal
{
    public class Response
    {
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public JObject body { get; set; }
    }

    public class ResponseA
    {
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public JArray body { get; set; }
    }
}
