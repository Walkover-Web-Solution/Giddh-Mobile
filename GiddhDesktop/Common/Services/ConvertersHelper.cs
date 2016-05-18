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
            company str = (company)value;
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
}
