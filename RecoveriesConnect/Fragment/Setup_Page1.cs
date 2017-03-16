using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Models.Api;
using Android.App;


namespace RecoveriesConnect.Fragment
{
    [Activity(Theme = "@style/Theme.ThemeCustomNoActionBar", NoHistory = true)]
    public class Setup_Page1 : Android.Support.V4.App.Fragment
    {
        ViewPager pager_local;
		TextView tv;
        public Setup_Page1(ViewPager pager)
        {
            pager_local = pager;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			View view = inflater.Inflate(Resource.Layout.Setup_Page1, container, false);

            Button button = view.FindViewById<Button>(Resource.Id.buttonNext);
            button.Click += buttonNextClick;

            tv = view.FindViewById<TextView>(Resource.Id.tv_Message);

			Keyboard.HideSoftKeyboard(this.Activity);

			this.LoadWelcomeMessage();

			return view;
        }

		private void LoadWelcomeMessage() {

			string url = Settings.InstanceURL;

			url = url + "/Api/GetWelcomeMessage";

			var json = new
			{
			};

			var ObjectReturn = new JsonReturnModel();
			try
			{
				string results = ConnectWebAPI.Request(url, json);
				ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);
			}
			catch (Exception ex)
			{
				//Default message in case of error
				Error temp = new Error();
				temp.ErrorMessage = Resources.GetString(Resource.String.WelcomeMessage);
				ObjectReturn.Errors = new Error[1];
				ObjectReturn.Errors[0] = temp;
			}
			if (ObjectReturn != null)
			{
				tv.Text = ObjectReturn.Errors[0].ErrorMessage;
			}
		}

        public void buttonNextClick(object sender, EventArgs e)
        {
            pager_local.SetCurrentItem(1, true);
        }

    }
}