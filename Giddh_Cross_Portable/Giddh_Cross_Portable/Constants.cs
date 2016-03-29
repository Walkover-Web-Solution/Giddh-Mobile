using Giddh_Cross_Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable
{
    public class Constants
    {
        public static userObject userObj { get; set; }

        public static List<company> companies { get; set; }
        public static company selectedCompany { get; set; }
        public static trialBalance trialBalance { get; set; }
        public static ObservableCollection<groupWise> GWTrialBalance { get; set; }
        public static groupTrialBalance assLiGroup { get; set; }
        public static groupTrialBalance inExpGroup { get; set; }
        public static groupTrialBalance grossProfit { get; set; }
        public static ObservableCollection<groupTrialBalance> reportsList { get; set; }
        public static groupTrialBalance bankDetail { get; set; }

        public static ObservableCollection<accountDetail> accountList { get; set; }
    }
}
