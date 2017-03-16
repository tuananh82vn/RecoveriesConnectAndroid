
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
			Pie.StartAngle = 90.0;
			Pie.Add (new PieItem { Title="one", Value = 1.5});
			Pie.Add (new PieItem { Title="two",Value = 2});
			Pie.Add (new PieItem { Title="three",Value = 2.5});
			Pie.Add (new PieItem { Title="four",Value = 3.5});
			Pie.Update ();
		}
	}
}

