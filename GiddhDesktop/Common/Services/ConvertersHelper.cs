using GiddhDesktop.Common.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GiddhDesktop.Common.Services
{
    public class convertStringToDate : IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, string language)
        {
            string[] dates = value.ToString().Split('-');
            int[] dateInt = { System.Convert.ToInt32(dates[2]), System.Convert.ToInt32(dates[1]), System.Convert.ToInt32(dates[0]) };            
            DateTime dt = new DateTime(dateInt[0], dateInt[1], dateInt[2]);
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto;
        }

        public object ConvertBack(object value, Type targetType,object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class showNameWithUniqueName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            accountDetail ac = value as accountDetail;
            string str = ac.name + " (" + ac.uniqueName + ")";
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class sharedEntityToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            if (value is company)
            {
                var str = (company)value;
                if (string.IsNullOrEmpty(str.role.uniqueName) || str.role.uniqueName.ToLower().Equals("view_only"))
                {
                    return Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(str.sharedEntity))
                {
                    return Visibility.Visible;
                }
                else
                {
                    if (str.role.uniqueName.ToLower().Equals("view_only"))
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
            }
            else
            {
                var str = (accountDetail)value;
                if (string.IsNullOrEmpty(str.role.uniqueName) || str.role.uniqueName.ToLower().Equals("view_only"))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

            
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class boolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool str = (bool)value;
            if (str)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class dateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((DateTimeOffset)value).Date.ToString("dd-MM-yyyy");
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class intToContent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString().Equals("0") && parameter == null)
            {
                return "Save";
            }            
            else if (parameter != null && parameter.ToString().Equals("total"))
                return "Total " + value.ToString();
            else
                return "Save all entries";
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class forwardedBalanceManipulation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            balance bal = value as balance;
            if (string.IsNullOrEmpty(bal.description))
            {
                bal.description = "C/F Balance";
            }
            else
            {
                if (bal.description.ToLower().Contains("bf_"))
                {
                    bal.description = "B/F Balance";
                }
                else if (bal.description.ToLower().Contains("cf_"))
                {
                    bal.description = "C/F Balance";
                }
                else if (bal.description.ToLower().Contains("open"))
                {
                    bal.description = "Opening Balance";
                }
            }
            if (parameter.ToString().Equals("debit") && bal.type.ToLower().Equals("debit"))
            {
                return bal.description + " " + bal.amount;
            }
            else if (parameter.ToString().Equals("credit") && bal.type.ToLower().Equals("credit"))
            {
                
                return bal.description + " " + bal.amount;
            }
            else
                return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
