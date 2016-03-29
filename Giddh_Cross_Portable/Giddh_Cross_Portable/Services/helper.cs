using Giddh_Cross_Portable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Services
{
    public class helper
    {
        // Function called from OnDestroy
        public static void saveset()
        {

            //store
            Application.Current.Resources.Add("authKey", Constants.userObj);

        }

        // Function called from OnCreate
        public static void retrieveset()
        {
            try
            {
                Constants.userObj = (userObject)Application.Current.Resources["authKey"];
            }
            catch(Exception ex)
            { }
        }

        public static string signConvention(string addComma)
        {
            string commaString = string.Empty;
            commaString = String.Format("N2", addComma);
            return commaString;
        }
    }
}
