using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using RecoveriesConnect.Fragment;
using Android.Support.V4.View;

namespace RecoveriesConnect.Adapter
{
    public class SetupAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> fragments;
        int fragmentCount;

        public SetupAdapter(Android.Support.V4.App.FragmentManager fm, ViewPager pager) : base(fm)
        {
            this.fragments = new List<Android.Support.V4.App.Fragment>();
            //fragments.Add(new Setup_Page1(pager));
            fragments.Add(new Setup_Page2());

            fragmentCount = fragments.Count;
        }

        public override int Count
        {
            get
            {
                return fragmentCount;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }


    }
}