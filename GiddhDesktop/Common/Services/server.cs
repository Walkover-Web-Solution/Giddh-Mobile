using GiddhDesktop.Common.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiddhDesktop.Common.Services
{
    class server
    {
        const string _loginWithGoogle = "login-with-google";
        const string _loginWithTwitter = "login-with-twitter";
        //string _companies = "users/" + Constants.userObj.user.uniqueName + "/companies";

        public static async Task<Response> loginWithGoogle(string token)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {

            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Access-Token", token);
            //addUserIDType(values);
            //addCountryArray(values);
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestGETResponse(_loginWithGoogle, values, header);
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = "Exception";
                return response;
            }
            if (response.status.ToLower().Equals("success"))
                Constants.userObj = JsonConvert.DeserializeObject<userObject>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return response;
        }

        public static async Task<Response> loginWithTwitter(string token,string tokenSecret)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {

            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Access-Token", token);
            KeyValuePair<string, string> header1 = new KeyValuePair<string, string>("Access-Secret", tokenSecret);
            List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
            headerList.Add(header);
            headerList.Add(header1);
            //addUserIDType(values);
            //addCountryArray(values);
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestGETResponse(_loginWithTwitter, values, headerList);
            }
            catch (Exception ex)
            {
                response.message = "No user found";
                response.status = "failure";
                return response;
                //throw ex;
            }
            if (response.status.ToLower().Equals("success"))
                Constants.userObj = JsonConvert.DeserializeObject<userObject>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return response;
        }

        public static async Task<ResponseA> getRoles()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {

            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            //addUserIDType(values);
            //addCountryArray(values);
            ResponseA response = new ResponseA();
            try
            {
                response = await postSubmitter.SendRequestGETResponseA("roles", values, header);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Constants.roles = new List<Roles>();
            Constants.roles = JsonConvert.DeserializeObject<List<Roles>>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return response;
        }

        public static async Task<ResponseA> companies()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {

            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            //addUserIDType(values);
            //addCountryArray(values);
            ResponseA response = new ResponseA();
            try
            {
                response = await postSubmitter.SendRequestGETResponseA("users/" + Constants.userObj.user.uniqueName + "/companies", values, header);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List<company> cList = JsonConvert.DeserializeObject<List<company>>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Constants.companies = new List<company>();
            foreach (company compny in cList)
            {
                //if (compny.sharedEntity == null)
                { Constants.companies.Add(compny); }
            }
            return response;
        }

        public static async Task<ResponseA> getAccountsWithDetails()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {

            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            //addUserIDType(values);
            //addCountryArray(values);
            ResponseA response = new ResponseA();
            try
            {
                response = await postSubmitter.SendRequestGETResponseA("company/" + Constants.selectedCompany.uniqueName + "/detailed-groups-with-accounts", values, header);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (response.status.ToLower().Equals("success"))
            {
                Constants.accountList = new ObservableCollection<accountDetail>();
                var gdetail = JsonConvert.DeserializeObject<List<groupDetail>>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                filterAccounts(gdetail);
                Constants.accountList = new ObservableCollection<accountDetail>(Constants.accountList.OrderBy(x => x.parentGroupName).ToList());
            }
            return response;

        }

        public static void filterAccounts(List<groupDetail> groupList)
        {
            foreach (groupDetail group in groupList)
            {
                try
                {
                    if (group.accounts != null && group.accounts.Count > 0)
                    {
                        foreach (accountDetail account in group.accounts)
                        {
                            account.parentGroupUniqueName = group.uniqueName;
                            account.parentGroupName = group.name;
                            account.name = account.name.TrimEnd();
                            Constants.accountList.Add(account);
                        }
                    }
                    if (group.groups != null && group.groups.Count > 0)
                    {
                        filterAccounts(group.groups);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }


        public static async Task<accountLedger> getAccountLedgers(accountDetail acntDetail,string fromDate = "",string toDate = "")
        {
            if (string.IsNullOrEmpty(fromDate))
            {
                fromDate = DateTime.Now.AddMonths(-1).ToString("dd-MM-yyyy");
                toDate = DateTime.Now.ToString("dd-MM-yyyy");
            }
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("from",fromDate),
                new KeyValuePair<string, string>("to",toDate)
            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            //addUserIDType(values);
            //addCountryArray(values);
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestGETResponse("company/" + Constants.selectedCompany.uniqueName + "/groups/" + acntDetail.parentGroupUniqueName + "/accounts/" + acntDetail.uniqueName + "/ledgers", values, header);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (response.status.ToLower().Equals("success"))
            {
                var aLedger = JsonConvert.DeserializeObject<accountLedger>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                aLedger.creditTransactions = new ObservableCollection<transaction>();
                aLedger.debitTransactions = new ObservableCollection<transaction>();
                aLedger.acDetail = acntDetail;
                foreach (var ledger in aLedger.ledgers)
                {
                    ledger.creditTransactions = new ObservableCollection<transaction>();
                    ledger.debitTransactions = new ObservableCollection<transaction>();
                    foreach (var transaction in ledger.transactions)
                    {
                        transaction.entryDate = ledger.entryDate;
                        transaction.ledgerUniqueName = ledger.uniqueName;
                        transaction.ledgerDiscription = ledger.description;
                        transaction.ledgerTag = ledger.tag;
                        transaction.ledgerVoucher = ledger.voucher;
                        transaction.ledgerVoucherNo = ledger.voucherNo;
                        if (transaction.type.ToLower().Equals("credit"))
                        {
                            ledger.creditTransactions.Add(transaction);
                            aLedger.creditTransactions.Add(transaction);
                        }
                        else if (transaction.type.ToLower().Equals("debit"))
                        {
                            ledger.debitTransactions.Add(transaction);
                            aLedger.debitTransactions.Add(transaction);
                        }
                    }
                }
                return aLedger;
            }
            else
                throw new ArgumentException(response.message);
        }


        #region trial balance section

        public static void filterAccountsFromTB(List<gDetails> groupList)
        {
            foreach (gDetails group in groupList)
            {
                try
                {
                    if (group.accounts != null && group.accounts.Count > 0)
                    {
                        foreach (aDetails account in group.accounts)
                        {
                            accountDetail newAccount = new accountDetail();
                            newAccount.parentGroupUniqueName = group.uniqueName;
                            newAccount.name = account.name.TrimEnd();
                            newAccount.openingBalance = account.openingBalance.amount;
                            newAccount.openingBalanceType = account.openingBalance.type;
                            newAccount.uniqueName = account.uniqueName;
                            Constants.accountList.Add(newAccount);
                        }
                    }
                    if (group.childGroups != null && group.childGroups.Count > 0)
                    {
                        filterAccountsFromTB(group.childGroups);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        public static async Task<Response> getTrialBalance(DateTime from = new DateTime(), DateTime to = new DateTime())
        {
            string fromDate = from.ToString("dd-MM-yyyy");
            string toDate = to.ToString("dd-MM-yyyy");
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("from",from.ToString("dd-MM-yyyy")),
                new KeyValuePair<string, string>("to",to.ToString("dd-MM-yyyy"))
            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestGETResponse("company/" + Constants.selectedCompany.uniqueName + "/trial-balance", values, header);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Constants.trialBalance = JsonConvert.DeserializeObject<trialBalance>(response.body.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Constants.accountList = new ObservableCollection<accountDetail>();
            filterAccountsFromTB(Constants.trialBalance.groupDetails);
            var groups = Constants.trialBalance.groupDetails.GroupBy(x => x.category).Select(g => new
            {
                groupName = g.Key,
                openingBalance = ((g.Where(x => x.category.Equals(g.Key) && x.forwardedBalance.type.ToLower().Contains("debit")).Sum(x => x.forwardedBalance.amount)) - (g.Where(x => x.category.Equals(g.Key) && x.forwardedBalance.type.ToLower().Contains("credit")).Sum(x => x.forwardedBalance.amount))),
                closingBalance = ((g.Where(x => x.category.Equals(g.Key) && x.closingBalance.type.ToLower().Contains("debit")).Sum(x => x.closingBalance.amount)) - (g.Where(x => x.category.Equals(g.Key) && x.closingBalance.type.ToLower().Contains("credit")).Sum(x => x.closingBalance.amount))),
                creditTotal = (g.Where(x => x.category.Equals(g.Key)).Sum(x => x.creditTotal)),
                debitTotal = (g.Where(x => x.category.Equals(g.Key)).Sum(x => x.debitTotal)),
                group = g
            });
            if (Constants.GWTrialBalance != null)
                Constants.GWTrialBalance.Clear();
            else
                Constants.GWTrialBalance = new ObservableCollection<groupWise>();
            foreach (var group in groups)
            {
                groupWise gw = new groupWise();
                gw.name = group.groupName;
                gw.name = gw.name.ToCharArray()[0].ToString().ToUpper() + gw.name.Remove(0, 1);
                gw.openingBalance = Math.Round(group.openingBalance);
                gw.closingBalance = Math.Round(group.closingBalance);
                if (gw.name.ToLower().Contains("assets"))
                {
                    gw.creditTotal = "d" + (Math.Round(group.creditTotal)).ToString("N").Replace(".00", "");
                    gw.debitTotal = "u" + (Math.Round(group.debitTotal)).ToString("N").Replace(".00", "");
                }
                else if (gw.name.ToLower().Contains("liabilities"))
                {
                    gw.creditTotal = "d" + (Math.Round(group.creditTotal)).ToString("N").Replace(".00", "");
                    gw.debitTotal = "u" + (Math.Round(group.debitTotal)).ToString("N").Replace(".00", "");
                }
                else if (gw.name.ToLower().Contains("income"))
                {
                    gw.creditTotal = "d" + (Math.Round(group.creditTotal)).ToString("N").Replace(".00", "");
                    gw.debitTotal = "u" + (Math.Round(group.debitTotal)).ToString("N").Replace(".00", "");
                }
                else if (gw.name.ToLower().Contains("expense"))
                {
                    gw.creditTotal = "d" + (Math.Round(group.creditTotal)).ToString("N").Replace(".00", "");
                    gw.debitTotal = "u" + (Math.Round(group.debitTotal)).ToString("N").Replace(".00", "");
                }
                if (gw.openingBalance >= 0)
                { gw.oBalance = gw.openingBalance.ToString("N").Replace(".00", "") + " Dr."; }
                else
                { gw.oBalance = (gw.openingBalance * -1).ToString("N").Replace(".00", "") + " Cr."; }
                if (gw.closingBalance >= 0)
                { gw.cBalance = "Dr. " + gw.closingBalance.ToString("N").Replace(".00", ""); }
                else
                { gw.cBalance = "Cr. " + (gw.closingBalance * -1).ToString("N").Replace(".00", ""); }
                Constants.GWTrialBalance.Add(gw);
            }
            Constants.inExpGroup = new groupTrialBalance();
            Constants.inExpGroup.groupList = new ObservableCollection<groupWise>();
            Constants.assLiGroup = new groupTrialBalance();
            Constants.assLiGroup.groupList = new ObservableCollection<groupWise>();
            for (int i = 0; i < Constants.GWTrialBalance.Count(); i++)
            {
                if (Constants.GWTrialBalance[i].name.ToLower().Contains("assets") || Constants.GWTrialBalance[i].name.ToLower().Contains("liabilities"))
                {
                    Constants.assLiGroup.name = "Net Worth";
                    Constants.assLiGroup.groupList.Add(Constants.GWTrialBalance[i]);
                    Constants.assLiGroup.openingTotal += Constants.GWTrialBalance[i].openingBalance;
                    Constants.assLiGroup.closingTotal += Constants.GWTrialBalance[i].closingBalance;
                }
                else if (Constants.GWTrialBalance[i].name.ToLower().Contains("income") || Constants.GWTrialBalance[i].name.ToLower().Contains("expense"))
                {
                    Constants.inExpGroup.name = "Net Profit";
                    Constants.inExpGroup.groupList.Add(Constants.GWTrialBalance[i]);
                    Constants.inExpGroup.openingTotal += Constants.GWTrialBalance[i].openingBalance;
                    Constants.inExpGroup.closingTotal += Constants.GWTrialBalance[i].closingBalance;
                }
            }
            calculateNetWorth();
            setValues(Constants.assLiGroup);
            setValues(Constants.inExpGroup);
            calculateGrossProfit();
            Constants.reportsList = new ObservableCollection<groupTrialBalance>();
            Constants.reportsList.Add(Constants.assLiGroup);
            Constants.reportsList.Add(Constants.inExpGroup);
            Constants.reportsList.Add(Constants.grossProfit);

            return response;
        }



        public static void setValues(groupTrialBalance gt)
        {
            if (gt.openingTotal != 0)
            {
                gt.percentage = Math.Round(((gt.closingTotal - gt.openingTotal) * 100) / gt.openingTotal, 2);
            }
            gt.openingTotal = Math.Round(gt.openingTotal);
            gt.closingTotal = Math.Round(gt.closingTotal);
            if (gt.openingTotal < 0)
            {
                gt.oTotal = (gt.openingTotal * -1).ToString("N").Replace(".00", "");
            }
            else
                gt.oTotal = gt.openingTotal.ToString("N").Replace(".00", "");
            if (gt.closingTotal < 0)
            {
                gt.cTotal = (gt.closingTotal * -1).ToString("N").Replace(".00", "");
            }
            else
                gt.cTotal = gt.closingTotal.ToString("N").Replace(".00", "");
        }

        public static void calculateNetWorth()
        {
            try
            {
                var letsC = Constants.trialBalance.groupDetails.Where(x => x.groupName.ToLower().Contains("capital")).ToList()[0];
                if (letsC.forwardedBalance.type.ToLower().Equals("credit"))
                {
                    letsC.forwardedBalance.amount = letsC.forwardedBalance.amount * -1;
                }
                if (letsC.closingBalance.type.ToLower().Equals("credit"))
                {
                    letsC.closingBalance.amount = letsC.closingBalance.amount * -1;
                }

                Constants.assLiGroup.openingTotal = Constants.assLiGroup.openingTotal - letsC.forwardedBalance.amount;
                Constants.assLiGroup.closingTotal = Constants.assLiGroup.closingTotal - letsC.closingBalance.amount;
            }
            catch (Exception ex)
            { }
        }

        public static void calculateGrossProfit()
        {
            Constants.grossProfit = new groupTrialBalance();
            var revenue = Constants.trialBalance.groupDetails.Where(x => x.groupName.ToLower().Contains("revenue")).ToList()[0];
            var operating = Constants.trialBalance.groupDetails.Where(x => x.groupName.ToLower().Contains("operating")).ToList()[0];
            if (revenue.forwardedBalance.type.ToLower().Equals("debit"))
            {
                revenue.forwardedBalance.amount = revenue.forwardedBalance.amount * -1;
            }
            if (revenue.closingBalance.type.ToLower().Equals("debit"))
            {
                revenue.closingBalance.amount = revenue.closingBalance.amount * -1;
            }
            if (operating.forwardedBalance.type.ToLower().Equals("credit"))
            {
                operating.forwardedBalance.amount = operating.forwardedBalance.amount * -1;
            }
            if (operating.closingBalance.type.ToLower().Equals("credit"))
            {
                operating.closingBalance.amount = operating.closingBalance.amount * -1;
            }
            Constants.grossProfit.openingTotal = revenue.forwardedBalance.amount - operating.forwardedBalance.amount;
            Constants.grossProfit.closingTotal = revenue.closingBalance.amount - operating.closingBalance.amount;
            Constants.grossProfit.name = "Gross Profit";
            setValues(Constants.grossProfit);
        }

        public static void calculateBankAccounts()
        {
            Constants.bankDetail = new groupTrialBalance();
            Constants.bankDetail.name = "Bank";
            setValues(Constants.bankDetail);
        }
        #endregion

        #region entries
        public static async Task<Response> createNewEntry(ledgerToSend ledger)
        {
            string str = JsonConvert.SerializeObject(ledger);
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("",JsonConvert.SerializeObject(ledger))
            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestPOSTResponse("company/" + Constants.selectedCompany.uniqueName + "/accounts/" + Constants.selectedAccount.uniqueName + "/ledgers", values, header);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public static async Task<Response> mailLedger(string[] mailRecipient,string fromDate,string toDate)
        {
            mailRecipients mr = new mailRecipients() { recipients = mailRecipient };
            string str = JsonConvert.SerializeObject(mr);
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("",JsonConvert.SerializeObject(mr))
            };
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestPOSTResponse("company/" + Constants.selectedCompany.uniqueName + "/accounts/" + Constants.selectedAccount.uniqueName + "/ledgers/mail?from=" + fromDate + "&to=" + toDate, values, header);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public static async Task<Response> exportLedger(string fromDate, string toDate)
        {
            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Auth-Key", Constants.userObj.authKey);
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("from",fromDate),
                new KeyValuePair<string, string>("to",toDate)
            };
            Response response = new Response();
            try
            {
                response = await postSubmitter.SendRequestGETResponse("company/" + Constants.selectedCompany.uniqueName + "/accounts/" + Constants.selectedAccount.uniqueName + "/export-ledger", values, header);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        #endregion
    }
}
