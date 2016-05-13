using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiddhDesktop.Common.Modal
{
    public class company
    {
        public string uniqueName { get; set; }
        public string name { get; set; }
        public compCreatedBy createdBy { get; set; }
        public string sharedEntity { get; set; }
        public compRole role { get; set; }

    }

    public class compCreatedBy
    {
        public string name { get; set; }
        public string email { get; set; }
        public string uniqueName { get; set; }
    }

    public class compRole
    {
        public string uniqueName { get; set; }
        public string name { get; set; }
    }

    public class Roles
    {
        public string name { get; set; }
        public string uniqueName { get; set; }
        public List<permissions> permissions { get; set; }
    }

    public class permissions
    {
        public string code { get; set; }
        public string description { get; set; }
    }
}
