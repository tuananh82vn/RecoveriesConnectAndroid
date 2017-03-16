using System;

using Xamarin.Forms;

namespace HScrollDemo
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            //var toto = new ListView();

            var button1 = new Button
            {
                Text = "Using Xamarin.Forms"
            };
            button1.Clicked += showFormsDemo;

            var button2 = new Button
            {
                Text = "Using XAML"
            };
            button2.Clicked += showXamlDemo;

            
            Content = new StackLayout
            {
                Children = { button1, button2 }
            };
        }
         
        private void showFormsDemo(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new HScrollDemo.FormsPage());
        }

        private void showXamlDemo(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new XamlPage());
        }

    }
}
