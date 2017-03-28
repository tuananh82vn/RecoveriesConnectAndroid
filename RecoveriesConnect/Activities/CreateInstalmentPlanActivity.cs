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
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
    [Activity(Label = "CreateInstalmentPlan", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class CreateInstalmentPlanActivity : Activity
    {
        public Button bt_Yes;
        public Button bt_No;
        public TextView tv_Message;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.CreateInstalmentPlan);

            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

            var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
            upArrow.SetColorFilter(Color.ParseColor("#006571"), PorterDuff.Mode.SrcIn);
            ActionBar.SetHomeAsUpIndicator(upArrow);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            LinearLayout lLayout = new LinearLayout(this);
            lLayout.SetGravity(GravityFlags.CenterVertical);
            LinearLayout.LayoutParams textViewParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
            textViewParameters.RightMargin = (int)(30 * this.Resources.DisplayMetrics.Density);

            TextView myTitle = new TextView(this);
            myTitle.Text = "Pay In Instalments";
            myTitle.TextSize = 20;
            myTitle.Gravity = GravityFlags.Center;
            lLayout.AddView(myTitle, textViewParameters);

            ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
            ActionBar.SetCustomView(lLayout, actionbarParams);
            ActionBar.SetDisplayShowCustomEnabled(true);


            tv_Message = FindViewById<TextView>(Resource.Id.tv_Message);
            var message = "";
            message = "Can you pay the total amount in " + Settings.MaxNoPay.ToString() +" payments within " + Settings.ThreePartDurationDays.ToString() +" days?";
            tv_Message.Text = message;

            bt_Yes = FindViewById<Button>(Resource.Id.bt_Yes);
            bt_Yes.Click += bt_Yes_Click;

            bt_No = FindViewById<Button>(Resource.Id.bt_No);
            bt_No.Click += bt_No_Click;

            // Create your application here
        }

        private void bt_No_Click(object sender, EventArgs e)
        {
            Intent Intent = new Intent(this, typeof(SetupInstalmentActivity));
            StartActivity(Intent);
        }

        private void bt_Yes_Click(object sender, EventArgs e)
        {
            Intent Intent = new Intent(this, typeof(Setup3PartPaymentActivity));
            StartActivity(Intent);
        }
    }
}