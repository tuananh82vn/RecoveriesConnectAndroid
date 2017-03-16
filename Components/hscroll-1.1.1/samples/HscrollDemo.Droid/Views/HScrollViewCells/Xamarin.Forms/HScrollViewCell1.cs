using System;

using Xamarin.Forms;
using mcpNetwork.Components.Forms;

namespace HScrollDemo.Views.Xamarin.Forms
{

    public class HScrollViewCell1Data
    {
        public int id {get; set;}
        public string name { get; set; }
        public bool shortSelectionTime { get; set; }
    }

    public class HScrollViewCell1 : ContentView
    {

        protected HScrollViewCell1Data bindedData;
        public object BindedData
        {
            get
            {
                return this.bindedData;
            }
            set
            {
                this.bindedData = (HScrollViewCell1Data)value;
            }
        }

        public HScrollViewCell1()
        {

            this.BackgroundColor = Color.Transparent;
            this.Padding = 10;
            Label iconLabel = new Label
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
                
                FontSize = Device.OnPlatform(
                    Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                    Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                    Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                ),
                
                BackgroundColor = Color.Transparent
            };
            iconLabel.SetBinding(Label.TextProperty, "name");

            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { iconLabel },
                BackgroundColor = Color.Transparent
            };

            Content = viewLayout;

        }

    }
}
