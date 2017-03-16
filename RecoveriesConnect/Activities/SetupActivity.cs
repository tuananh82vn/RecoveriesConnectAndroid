using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Content.PM;
using RecoveriesConnect.Adapter;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "Main", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.ThemeCustomNoActionBar")]
    public class SetupActivity : FragmentActivity, Android.Support.V4.View.ViewPager.IOnPageChangeListener
    {
        ViewPager pager;
        SetupAdapter pageAdapter;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            pager = FindViewById<ViewPager>(Resource.Id.myViewPager);

            pageAdapter = new SetupAdapter(SupportFragmentManager, pager);

            pager.Adapter = pageAdapter;

            pager.SetCurrentItem(0, true);

            pager.AddOnPageChangeListener(this);

			Keyboard.HideSoftKeyboard(this);


		}
        public void OnPageScrollStateChanged(int state)
        {
            //Console.WriteLine("OnPageScrollStateChanged " + " " + state);
        }
        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            //Console.WriteLine("OnPageScrolled " + " " + position);
        }

        public void OnPageSelected(int position)
        {
            //Console.WriteLine("OnPageSelected" + " " + position);
            //if(position == 1)
            //{
            //    var fragment2 = pageAdapter.GetItem(position) as Fragment_Page2;
            //}
        }
    }
}

