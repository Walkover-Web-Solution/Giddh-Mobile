using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class companyCell : ViewCell
    {
        public companyCell()
        {
            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 18,
                TextColor = Color.Gray                                
            };
            nameLabel.SetBinding(Label.TextProperty, "name");
            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { nameLabel },
                Padding = new Thickness(10)                
            };
            View = viewLayout;
        }
    }
}
