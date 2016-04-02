using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.CustomControl
{
    public class filterListView : ListView
    {
        public void FilterLocations(string filter)
        {
            this.BeginRefresh();

            if (string.IsNullOrWhiteSpace(filter))
            {
                this.ItemsSource = Constants.accountList.OrderBy(x => x.name).ToList();
            }
            else {
                this.ItemsSource = Constants.accountList
                    .Where(x => x.name.ToLower()
                   .Contains(filter.ToLower()) || x.uniqueName.ToLower().Contains(filter.ToLower())).OrderBy(x => x.name).ToList();
            }

            this.EndRefresh();
        }
    }
}
