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
	[Activity(Label = "SelectPaymentMethod", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class SelectPaymentMethodActivity : Activity
	{
		public Spinner spinner_Method;
		public Button bt_Continue;
		List<string> MethodList;
		int selectedIndex = 0;
		public List<InstalmentSummaryModel> instalmentList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);


			SetContentView(Resource.Layout.SelectPaymentMethod);

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
			myTitle.Text = "Select Method";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			spinner_Method = FindViewById<Spinner>(Resource.Id.spinner_Method);

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

			LoadPaymentMethodList();
			// Create your application here

			LoadData();
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
		}

		public void LoadPaymentMethodList()
		{
			MethodList = new List<string>();
			MethodList.Add("Credit Card");
			MethodList.Add("Direct Debit");

			var MethodAdapter = new PaymentMethodSpinnerAdapter(this, MethodList);

			spinner_Method.Adapter = MethodAdapter;

			spinner_Method.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Method_ItemSelected);
		}

		private void Bt_Continue_Click(object sender, EventArgs e)
		{
			if (this.MethodList[this.selectedIndex] == "Credit Card")
			{
				Intent Intent = new Intent(this, typeof(MakeCCPaymentActivity));

				if (Settings.MakePaymentIn3Part || Settings.MakePaymentInstallment)
				{
					Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentList.ToArray());
				}
				//Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

				StartActivity(Intent);

			}
			else
			{

				Intent Intent = new Intent(this, typeof(MakeDDPaymentActivity));

				if (Settings.MakePaymentIn3Part || Settings.MakePaymentInstallment)
				{
					Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentList.ToArray());
				}
				//Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

				StartActivity(Intent);
			}
		}

		private void Method_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			selectedIndex = e.Position;
		}
	}
}

