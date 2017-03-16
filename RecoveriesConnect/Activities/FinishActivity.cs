using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using System;
using System.Threading;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "Finish", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class FinishActivity : Activity
	{
		private TextView tv_Message;
		private Button bt_Finish;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);


			SetContentView(Resource.Layout.Finish);

			//**************************************************//

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

			ActionBar.SetDisplayHomeAsUpEnabled(false);
			ActionBar.SetHomeButtonEnabled(false);

			LinearLayout lLayout = new LinearLayout(this);
			lLayout.SetGravity(GravityFlags.CenterVertical);
			LinearLayout.LayoutParams textViewParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			textViewParameters.RightMargin = (int)(30 * this.Resources.DisplayMetrics.Density);

			TextView myTitle = new TextView(this);
			myTitle.Text = "Finish";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			bt_Finish = FindViewById<Button>(Resource.Id.bt_Finish);
			bt_Finish.Click += bt_Finish_Click;

			tv_Message = FindViewById<TextView>(Resource.Id.tv_Message);
			GetData();

		}

		private void GetData()
		{
			var message = Intent.GetStringExtra("Message");
			tv_Message.Text = message;
		}

		private void bt_Finish_Click(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(o => GoHome());
		}

		private void GoHome()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);
			StartActivity(typeof(HomeActivity));
			AndHUD.Shared.Dismiss();
			this.Finish();
		}
	}
}

