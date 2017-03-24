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
    [Activity(Label = "About", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.About);

            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

            var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
            upArrow.SetColorFilter(Color.ParseColor("#006571"), PorterDuff.Mode.SrcIn);
            ActionBar.SetHomeAsUpIndicator(upArrow);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);


            // Create your application here
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Keyboard.HideSoftKeyboard(this);
                    OnBackPressed();
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}