using System;

using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using RecoveriesConnect.Helpers;


namespace RecoveriesConnect.Activities
{
    [Activity(Theme = "@style/Theme.ThemeCustomNoActionBar", NoHistory = true)]
    public class PrivacyPolicyActivity : Activity
    {
        public TextView textView;
        public Button bt_Agree;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PrivacyPolicy);

            textView = FindViewById<TextView>(Resource.Id.textViewPolicy);
            textView.TextFormatted = Html.FromHtml(Resources.GetString(Resource.String.PrivacyPolicy));

            bt_Agree = FindViewById<Button>(Resource.Id.bt_Agree);
            bt_Agree.Click += Bt_Agree_Click;
        }

        private void Bt_Agree_Click(object sender, EventArgs e)
        {
            Settings.IsAgreePolicy = true;
            StartActivity(typeof(LoginWaitingActivity));
            this.Finish();
        }
    }
}