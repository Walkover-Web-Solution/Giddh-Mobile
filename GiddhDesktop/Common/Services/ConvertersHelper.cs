using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GiddhDesktop.Common.Services
{
    public class convertStringToDate : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            string[] dates = value.ToString().Split('-');
            int[] dateInt = { System.Convert.ToInt32(dates[2]), System.Convert.ToInt32(dates[1]), System.Convert.ToInt32(dates[0]) };
            DateTime dt = new DateTime(dateInt[0], dateInt[1], dateInt[2]);
            return dt;
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
