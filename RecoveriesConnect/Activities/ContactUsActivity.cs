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
using Android.Text;

namespace RecoveriesConnect.Activities
{
    [Activity(Label = "Contact", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class ContactUsActivity : Activity
    {
        public TextView textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ContactUs);

            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

            var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
            upArrow.SetColorFilter(Color.ParseColor("#006571"), PorterDuff.Mode.SrcIn);
            ActionBar.SetHomeAsUpIndicator(upArrow);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            textView = FindViewById<TextView>(Resource.Id.textViewContactUs);

            textView.TextFormatted = Html.FromHtml(Resources.GetString(Resource.String.ContactUs));
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