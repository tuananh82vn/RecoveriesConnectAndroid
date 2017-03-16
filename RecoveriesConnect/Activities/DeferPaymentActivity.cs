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
using System.Threading;
using Android.Views.InputMethods;
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "DeferPayment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class DeferPaymentActivity : Activity
	{
		Alert alert;

		Button bt_Defer;

		public List<PaymentTrackerModel> DeferList;

		public PaymentTrackerAdapter paymentTrackerAdapter;

		public ListView paymentTrackerListView;

		int totalDefer;

		int totalUsed;

		int selectedIndex = -1;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);


			SetContentView(Resource.Layout.DeferPayment);

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
			myTitle.Text = "Defer Payment";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			// Create your application here

			bt_Defer = FindViewById<Button>(Resource.Id.bt_Defer);
			bt_Defer.Click += bt_Defer_Click;

			this.paymentTrackerListView = FindViewById<ListView>(Resource.Id.deferListView);
			this.paymentTrackerListView.ItemClick += listView_ItemClick;

			DeferList = new List<PaymentTrackerModel>();

			LoadPaymentTracker();

			Keyboard.HideSoftKeyboard(this);

		}

		private void bt_Defer_Click(object sender, EventArgs e)
		{
			Intent schedule_callback = new Intent(this, typeof(ScheduleCallbackActivity));
			if (this.DeferList.Count > 0)
			{
				if (selectedIndex == -1)
				{
					alert = new Alert(this, "Error", Resources.GetString(Resource.String.SelectDefer));
					alert.Show();
				}
				else
				{
					if (this.totalUsed == this.totalDefer)
					{
						StartActivity(schedule_callback);
					}
					else
					{
						ThreadPool.QueueUserWorkItem(o => SendDefer());

					}
				}
			}
			else 
			{

				StartActivity(schedule_callback);
			}

		}



		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (this.totalUsed == this.totalDefer)
			{
				alert = new Alert(this, "Error", Resources.GetString(Resource.String.MaxDefer));
				alert.Show();
				this.bt_Defer.Text = "Schedule Callback";
			}
			else
			{
				selectedIndex = e.Position;
			}

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

		private void SendDefer()
		{
				AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

				string url = Settings.InstanceURL;
				var url2 = url + "/Api/DeferPayment";

				var selectedItem = this.DeferList.ElementAt(this.selectedIndex);

				var json2 = new
				{
					Item = new
					{
						ReferenceNumber = Settings.RefNumber,
						InstalDate = selectedItem.HistInstalDate,
						Amount = selectedItem.HistInstalAmount
					}
				};

				try
				{
					var ObjectReturn2 = new JsonReturnModel();

					string results = ConnectWebAPI.Request(url2, json2);
					
					AndHUD.Shared.Dismiss();

					if (string.IsNullOrEmpty(results))
					{
						this.RunOnUiThread(() => this.bt_Defer.Enabled = true);
						alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
						alert.Show();
					}
					else
					{
						ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

						if (ObjectReturn2.IsSuccess)
						{

							TrackingHelper.SendTracking("Defer");

							Intent Intent = new Intent(this, typeof(FinishActivity));

							Intent.PutExtra("Message", "You have successfully deferred this payment.");

							StartActivity(Intent);

							this.Finish();
						}
						else
						{
							this.RunOnUiThread(() => this.bt_Defer.Enabled = true);
							this.RunOnUiThread(() => alert = new Alert(this, "Error", ObjectReturn2.Errors[0].ErrorMessage));
							this.RunOnUiThread(() => alert.Show());
						}
					}
				}
				catch (Exception ee)
				{
					this.RunOnUiThread(() => this.bt_Defer.Enabled = true);
					AndHUD.Shared.Dismiss();
				}
		}

		private void LoadPaymentTracker()
		{
			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetDebtorPaymentHistory";
			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber
				}
			};

			try
			{
				var ObjectReturn = new DebtorInfoModel();

				string results = ConnectWebAPI.Request(url2, json2);



				if (string.IsNullOrEmpty(results))
				{
					alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
					alert.Show();
				}
				else
				{
					ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<DebtorInfoModel>(results);
					foreach (var item in ObjectReturn.HistoryInstalmentScheduleList) {
						var deferAmount = decimal.Parse(item.HistDeferredAmount);

						var payAmount = decimal.Parse(item.HistPaymentAmount);
						var payDate = item.HistPaymentDate;

						if (deferAmount > 0) { 
						}
						else if (payAmount == 0 && string.IsNullOrEmpty(payDate))
						{
							this.DeferList.Add(item);
						}
					}

					this.totalDefer = ObjectReturn.TotalDefer;
					this.totalUsed = ObjectReturn.TotalUsedDefer;

					if (this.DeferList.Count > 0)
					{
						
						this.bt_Defer.Text = "You used " + this.totalUsed + " of " + this.totalDefer + ". Continue?";
					}
					else
					{
						this.paymentTrackerListView.Selector = Resources.GetDrawable(Android.Resource.Color.Transparent);
						this.paymentTrackerListView.Clickable = false;
						this.paymentTrackerListView.ChoiceMode = ChoiceMode.None;

						this.bt_Defer.Text = "Schedule Callback";
					}

					paymentTrackerAdapter = new PaymentTrackerAdapter(this, this.DeferList, "Defer");
					this.paymentTrackerListView.Adapter = paymentTrackerAdapter;
					this.paymentTrackerListView.InvalidateViews();
				}
			}
			catch (Exception ee)
			{
			}

		}
	}
}

