using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable.Model
{
    public class groupTrialBalance
    {
        public ObservableCollection<groupWise> groupList { get; set; }
        public double openingTotal { get; set; }
        public double closingTotal { get; set; }
        public double percentage { get; set; }
        public string oTotal { get; set; }
        public string cTotal { get; set; }
        public string name { get; set; }
    }
}
