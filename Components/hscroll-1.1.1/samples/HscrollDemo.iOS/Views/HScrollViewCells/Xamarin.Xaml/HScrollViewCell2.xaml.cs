using System;

using Xamarin.Forms;
using mcpNetwork.Components.Xaml;

namespace HScrollDemo.UserControls
{
    
    public class HScrollViewCell2Data
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool resetSelection { get; set; }
    }

    public partial class HScrollViewCell2 : ContentView
    {
        
        protected HScrollViewCell2Data bindedData;
        public  object BindedData
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
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.bindedData = BindingContext as HScrollViewCell2Data;
            if (this.bindedData != null)
            {
                HScrollViewCell2Data bindedData = (HScrollViewCell2Data)this.bindedData;
                //string imageSource = "HScrollDemo.Resources." + bindedData.icon;
                string imageSource = bindedData.icon;
                iconImage.Source = ImageSource.FromFile(imageSource);
            }
        }

    }
}
