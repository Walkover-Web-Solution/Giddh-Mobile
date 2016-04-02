using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.CustomControl
{
    class accountListCell : ViewCell
    {
        public accountListCell()
        {
            var letsc = this;
            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start
            };
            nameLabel.SetBinding(Label.TextProperty, "name");
            //nameLabel.BindingContextChanged += NameLabel_BindingContextChanged1;

            var bottomBorder = new BoxView()
            {
                HeightRequest = 2,
                BackgroundColor = Color.Gray
            };
            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { nameLabel },
                Padding = new Thickness(10, 5)
            };
            View = viewLayout;
        }
    }
}
