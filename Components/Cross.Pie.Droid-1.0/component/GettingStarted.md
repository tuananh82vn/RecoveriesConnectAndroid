# Getting Started with Cross.Pie.Droid
---

#STEP 1 - Create and Set Properties of CrossPie

#STEP 2 - Create new PieItem object per segment and set properties
## Title - is Name of each pie segment
## Value - is Value of each segment

#STEP 3 - Refresh(Redraw) Pie chart using 'Update' method.

#See Sample Code of a page with a pie chart.

    using Android.App;
    using Android.OS;

    namespace Cross.Pie.Droid.Sample
    {
        [Activity (Label = "SimplestActivity")]			
        public class SimplestActivity : Activity
        {
            CrossPie Pie { get; set; }

            protected override void OnCreate (Bundle savedInstanceState)
            {
                base.OnCreate (savedInstanceState);

                SetContentView (Resource.Layout.Simplest);

                Pie = FindViewById<CrossPie> (Resource.Id.myPie);

                AddItems ();
            }
            void AddItems ()
            {
                Pie.Add (new PieItem { Title="one", Value = 1.5});
                Pie.Add (new PieItem { Title="two",Value = 2});
                Pie.Add (new PieItem { Title="three",Value = 2.5});
                Pie.Update ();
            }
        }
    }

#SUCCESS
