using GiddhDesktop.Common.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GiddhDesktop.Common
{
    public class Constants
    {
        public static userObject userObj { get; set; }

        public static List<company> companies { get; set; }
        public static List<Roles> roles { get; set; }
        public static company selectedCompany { get; set; }
        public static trialBalance trialBalance { get; set; }
        public static ObservableCollection<groupWise> GWTrialBalance { get; set; }
        public static groupTrialBalance assLiGroup { get; set; }
        public static groupTrialBalance inExpGroup { get; set; }
        public static groupTrialBalance grossProfit { get; set; }
        public static ObservableCollection<groupTrialBalance> reportsList { get; set; }
        public static groupTrialBalance bankDetail { get; set; }

        public static accountDetail selectedAccount { get; set; }

        public static ObservableCollection<accountDetail> accountList { get; set; }
        public static ObservableCollection<accountWithParent> groupAccountList { get; set; }

        public static Windows.Data.Xml.Dom.XmlDocument CreateToast(string message)
        {
            var xDoc = new XDocument(
               new XElement("toast",
               new XElement("visual",
               new XElement("binding", new XAttribute("template", "ToastGeneric"),
               new XElement("text", "Giddh"),
               new XElement("text", message)
            )
            )// actions  
            //new XElement("actions",
            //new XElement("action", new XAttribute("activationType", "background"),
            //new XAttribute("content", "Yes"), new XAttribute("arguments", "yes")),
            //new XElement("action", new XAttribute("activationType", "background"),
            //new XAttribute("content", "No"), new XAttribute("arguments", "no"))
            //)
            )
            );

            var xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }
    }
}
