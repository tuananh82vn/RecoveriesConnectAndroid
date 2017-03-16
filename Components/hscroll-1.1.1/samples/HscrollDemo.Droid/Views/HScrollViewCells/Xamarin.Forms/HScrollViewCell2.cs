using System;

using Xamarin.Forms;
using mcpNetwork.Components.Forms;

namespace HScrollDemo.Views.Xamarin.Forms
{

    public class HScrollViewCell2Data
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool resetSelection { get; set; }
    }

    public class HScrollViewCell2 : ContentView
    {

        protected Image iconImage;

        protected HScrollViewCell2Data bindedData;
        public object BindedData
        {
            get
            {
                return this.bindedData;
            }
            set
            {
                this.bindedData = (HScrollViewCell2Data)value;
            }
        }

        public HScrollViewCell2()
        {

            this.BackgroundColor = Color.Transparent;
            this.Padding = 10;
            iconImage = new Image
            {
                HeightRequest = 30,
                WidthRequest = 30,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent
            };

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
                Children = { this.iconImage, iconLabel },
                BackgroundColor = Color.Transparent
            };

            Content = viewLayout;

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.bindedData = BindingContext as HScrollViewCell2Data;
            if (this.bindedData != null)
            {
                //string imageSource = "HScrollDemo.Resources." + this.bindedData.icon;
                string imageSource = this.bindedData.icon;
                this.iconImage.Source = ImageSource.FromFile(imageSource);
            }
        }


    }
}
