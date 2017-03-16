//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace RecoveriesConnect.Helpers
//{
//    public class ProgressBarHelper
//    {

//        private ProgressBar mProgressBar;
//        private Activity mContext;

//        public ProgressBarHelper(Activity activity)
//        {
//            mContext = activity;

//            ViewGroup Rootlayout = (ViewGroup)mContext.Window.DecorView.FindViewById(Android.Resource.Id.Content);


//            mProgressBar = new ProgressBar(mContext);
//            mProgressBar.IndeterminateDrawable.SetColorFilter(Android.Graphics.Color.ParseColor("#006571"), Android.Graphics.PorterDuff.Mode.Multiply);
//            mProgressBar.Indeterminate = true;
//            mProgressBar.Visibility = ViewStates.Invisible;

//            RelativeLayout.LayoutParams paramsa = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);

//            RelativeLayout rl = new RelativeLayout(mContext);
//            rl.SetGravity(GravityFlags.Center);
//            rl.AddView(mProgressBar);


//            Rootlayout.AddView(rl, paramsa);
//        }

//        public void show()
//        {
//            mProgressBar.Visibility = ViewStates.Visible;
//        }

//        public void hide()
//        {
//            mProgressBar.Visibility = ViewStates.Invisible;
//        }
//    }
//}