using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiddhDesktop.Common.Modal
{
    public class accounts
    {
        public string name { get; set; }
        public string uniqueName { get; set; }
    }


    public class ledgerToSend
    {
        public string description { get; set; }
        public string tag { get; set; }
        public string entryDate{ get; set; }
        public string voucherType { get; set; }
        public ledgerTransaction[] transactions { get; set; }
        public string unconfirmedEntry { get; set; }
    }

    public class ledgerTransaction
    {
        public string type { get; set; }
        public string amount { get; set; }
        public string particular { get; set; }
    }

    public class mailRecipients
    {
        public string[] recipients { get; set; }
    }
}
