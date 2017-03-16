using System;

using Xamarin.Forms;
using mcpNetwork.Components.Xaml;

namespace HScrollDemo.UserControls
{
    
    public class HScrollViewCell1Data
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool shortSelectionTime { get; set; }
    }

    public partial class HScrollViewCell1 : ContentView
    {
        
        protected HScrollViewCell1Data bindedData;
        public  object BindedData
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
            InitializeComponent();
        }
    }
}
