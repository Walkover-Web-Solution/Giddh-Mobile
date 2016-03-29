using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable.Model
{
    public class trialBalance
    {
        public balance forwardedBalance { get; set; }
        public double creditTotal { get; set; }
        public double debitTotal { get; set; }
        public balance closingBalance { get; set; }
        public List<gDetails> groupDetails { get; set; }

    }

    public class balance
    {
        public double amount { get; set; }
        public string type { get; set; }
        public string description { get; set; }
    }

    public class gDetails
    {
        public balance forwardedBalance { get; set; }
        public double creditTotal { get; set; }
        public double debitTotal { get; set; }
        public balance closingBalance { get; set; }
        public string groupName { get; set; }
        public string category { get; set; }
        public string uniqueName { get; set; }
        public List<gDetails> childGroups { get; set; }
        public List<aDetails> accounts { get; set; }
        public List<gDetails> groups { get; set; }

    }

    public class aDetails
    {
        public balance openingBalance { get; set; }
        public double creditTotal { get; set; }
        public double debitTotal { get; set; }
        public balance closingBalance { get; set; }
        public string name { get; set; }
        public string uniqueName { get; set; }
    }

    public class groupWise
    {
        public string name { get; set; }
        public double openingBalance { get; set; }
        public string oBalance { get; set; }
        public double closingBalance { get; set; }
        public string cBalance { get; set; }
        public string creditTotal { get; set; }
        public string debitTotal { get; set; }
        public string oArrow { get; set; }
        public string cArrow { get; set; }

        public gDetails group { get; set; }
    }

    public class groupDetail
    {
        public string name { get; set; }
        public string description { get; set; }
        public string uniqueName { get; set; }
        public Role role { get; set; }
        public List<groupDetail> groups { get; set; }
        public List<accountDetail> accounts { get; set; }
        public string synonyms { get; set; }
        public string category { get; set; }
        

    }

    public class accountDetail
    {
        public string name { get; set; }
        public string description { get; set; }
        public string uniqueName { get; set; }
        public Role role { get; set; }
        public double openingBalance { get; set; }
        public string openingBalanceType { get; set; }
        public string openingBalanceDate { get; set; }
        public string email { get; set; }
        public string mobileNo { get; set; }
        public string companyName { get; set; }
        public string address { get; set; }
        public string parentGroupUniqueName { get; set; }

        public string NameSort { get { if (string.IsNullOrWhiteSpace(name) || name.Length == 0) return "?"; return name[0].ToString().ToUpper(); } }
    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }

    public class Role
    {
        public string name { get; set; }
        public string uniqueName { get; set; }

    }

    public class accountLedger
    {
        public accountDetail acDetail { get; set; }
        public balance forwardedBalance { get; set; }
        public double creditTotal { get; set; }
        public double debitTotal { get; set; }
        public balance balance { get; set; }
        public ObservableCollection<ledger> ledgers { get; set; }
    }

    public class ledger
    {
        public ObservableCollection<transaction> transactions { get; set; }
        public string uniqueName { get; set; }
        public string entryDate { get; set; }
        public double voucherNo { get; set; }
        public Voucher voucher { get; set; }
        public string tag { get; set; }
        public string description { get; set; }
    }

    public class transaction
    {
        public Role particular { get; set; }
        public double amount { get; set; }
        public string type { get; set; }

    }

    public class Voucher
    {
        public string name { get; set; }
        public string shortCode { get; set; }
    }

    public class datewiseLedger
    {
        public datewiseLedger(string date)
        {
            Date = date;
        }

        public string Date { get; set; }
        public ObservableCollection<ledger> ledgers { get; set; }
        public ObservableCollection<showLedger> transactions { get; set; }
    }

    public class ledgerFinal : ObservableCollection<showLedger>
    {
        public ledgerFinal(string date)
        {
            Date = date;
        }
        public string Date { get; set; }
        public string shortDate { get; set; }
    }

    public class showLedger
    {
        public string name { get; set; }
        public double debitAmount { get; set; }
        public double creditAmount { get; set; }

    }
}
