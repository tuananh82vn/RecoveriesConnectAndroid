using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HScrollDemo.ResourceExtensions
{
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;

            // Do your translation lookup here, using whatever method you require
            //var imageSource = ImageSource.FromResource(Source);
            //string imageSource = "HScrollDemo.Resources." + Source;
            string imageSource = Source;
            var image = ImageSource.FromFile(imageSource);

            return image;
        }
    }
}
