using Giddh_Cross_Portable.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(ExtendedSearchBarRenderer))]

namespace Giddh_Cross_Portable.iOS.Renderers
{
    class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var control = this.Control;
            //if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = true;
                
            }
        }
    }
}
