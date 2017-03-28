using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Adapter;
using System;
using System.Linq;
using RecoveriesConnect.Models.Api;
using System.Collections.Generic;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "InstalmentSummary", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class InstalmentSummaryActivity : Activity
	{
		public Button bt_Continue;

		public List<InstalmentSummaryModel> instalmentList;

		public InstalmentSummaryAdapter instalmentSummaryAdapter;

		public ListView instalmentSummaryListView;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.InstalmentSummary);

			//**************************************************//

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
			myTitle.Text = "Instalment Summary";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//


			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += bt_Continue_Click;

			this.instalmentSummaryListView = FindViewById<ListView>(Resource.Id.instalmentListView);

			LoadData();

			TrackingHelper.SendTracking("Open Instalment Summary");

		}

		private void bt_Continue_Click(object sender, EventArgs e)
		{
			Settings.FirstAmountOfInstallment = decimal.Parse(this.instalmentList.FirstOrDefault().Amount.ToString());
				
			Intent Intent = new Intent(this, typeof(MakeCCPaymentActivity));

			Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentList.ToArray());

			Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

			StartActivity(Intent);
		}

		public void LoadData()
		{
			var items = Intent.GetParcelableArrayListExtra("InstalmentSummary");
			if (items != null)
			{
				items = items.Cast<InstalmentSummaryModel>().ToArray();

				instalmentList = new List<InstalmentSummaryModel>();

				foreach (InstalmentSummaryModel item in items)
				{
					InstalmentSummaryModel instalment = new InstalmentSummaryModel();
					instalment.PaymentDate = item.PaymentDate;
					instalment.Amount = item.Amount;
					instalmentList.Add(instalment);
				}			
			}

			instalmentSummaryAdapter = new InstalmentSummaryAdapter(this, this.instalmentList);
			this.instalmentSummaryListView.Adapter = instalmentSummaryAdapter;
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

