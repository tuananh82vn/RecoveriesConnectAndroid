using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using System;
using RecoveriesConnect.Models.Api;
using System.Threading;
using AndroidHUD;


namespace RecoveriesConnect.Activities
{
    [Activity(Label = "MakePayment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class MakePaymentActivity : Activity
    {
        public Button bt_Pay;

        public Button bt_Create;
        bool isExistingPlan = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MakePayment);

            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

            var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
            upArrow.SetColorFilter(Color.ParseColor("#006571"), PorterDuff.Mode.SrcIn);
            ActionBar.SetHomeAsUpIndicator(upArrow);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            // Create your application here

            bt_Pay = FindViewById<Button>(Resource.Id.bt_Pay);
            bt_Pay.Click += bt_Pay_Click;

            bt_Create = FindViewById<Button>(Resource.Id.bt_Create);
            bt_Create.Click += bt_Create_Click;

            if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
            {
                bt_Create.SetText("View My Instalment Plan", TextView.BufferType.Normal);
                isExistingPlan = true;
            }
            else
            {
                bt_Create.SetText("Create an Instalment Plan", TextView.BufferType.Normal);
            }

        }

        private void bt_Pay_Click(object sender, EventArgs e)
        {
            Intent Intent = new Intent(this, typeof(MakeCCPaymentActivity));

            StartActivity(Intent);
        }

        private void bt_Create_Click(object sender, EventArgs e)
        {
            if (isExistingPlan)
            {
                Intent Intent = new Intent(this, typeof(InstalmentInfoActivity));

                StartActivity(Intent);
            }
            else
            {
                Intent Intent = new Intent(this, typeof(SetupInstalmentActivity));

                //Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

                StartActivity(Intent);
                
            }
        }
    }
}