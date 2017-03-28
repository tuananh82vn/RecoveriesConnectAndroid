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
using Android.Views.InputMethods;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "PaymentTracker", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class PaymentTrackerActivity : Activity
    {
        Alert alert;

        Button bt_Schedule;
        Button bt_History;
        Button bt_MakePayment;

        public PaymentTrackerModel[] HistoryInstalmentScheduleList;

        public PaymentTrackerModel[] InstalmentScheduleList;

        public PaymentTrackerModel[] PaymentTrackerList;

        public PaymentTrackerAdapter paymentTrackerAdapter;

        public ListView paymentTrackerListView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.PaymentTracker);

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
			myTitle.Text = "Payment Tracker";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//


			bt_Schedule = FindViewById<Button>(Resource.Id.bt_Schedule);
            bt_Schedule.Click += Bt_Schedule_Click;

            bt_History = FindViewById<Button>(Resource.Id.bt_History);
            bt_History.Click += Bt_History_Click;

            bt_MakePayment = FindViewById<Button>(Resource.Id.bt_MakePayment);
            bt_MakePayment.Click += Bt_MakePayment_Click;

            bt_Schedule.Selected = true;
            bt_History.Selected = false;

            this.paymentTrackerListView = FindViewById<ListView>(Resource.Id.paymentTrackerListView);

			Keyboard.HideSoftKeyboard(this);

            LoadPaymentTracker();

			TrackingHelper.SendTracking("Open Payment Tracker");


		}

        private void Bt_MakePayment_Click(object sender, EventArgs e)
        {
			SetPayment.Set("other");
			StartActivity(typeof(MakeCCPaymentActivity));
		}

        public void HideSoftKeyboard1(Activity activity)
        {
            var view = activity.CurrentFocus;
            if (view != null)
            {
                InputMethodManager manager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
                manager.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }

      
        private void LoadPaymentTracker()
        {
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

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
                    AndHUD.Shared.Dismiss();
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
                else
                {
                    ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<DebtorInfoModel>(results);

					AndHUD.Shared.Dismiss();

                    this.HistoryInstalmentScheduleList = ObjectReturn.HistoryInstalmentScheduleList;
                    this.InstalmentScheduleList = ObjectReturn.InstalmentScheduleList;
                    this.PaymentTrackerList = this.InstalmentScheduleList;

                    paymentTrackerAdapter = new PaymentTrackerAdapter(this, this.PaymentTrackerList.ToList(),"Schedule");
                    this.paymentTrackerListView.Adapter = paymentTrackerAdapter;

					if (this.PaymentTrackerList.Count() == 0)
					{
						this.paymentTrackerListView.Selector = Resources.GetDrawable(Android.Resource.Color.Transparent);
						this.paymentTrackerListView.Clickable = false;
						this.paymentTrackerListView.ChoiceMode = ChoiceMode.None;
					}

                }
            }
            catch (Exception ee)
            {
				AndHUD.Shared.Dismiss();
			}

        }
        private void Bt_History_Click(object sender, EventArgs e)
        {
            bt_History.Selected = true;
            bt_Schedule.Selected = false;

            this.PaymentTrackerList = this.HistoryInstalmentScheduleList;
            paymentTrackerAdapter = new PaymentTrackerAdapter(this, this.PaymentTrackerList.ToList(), "History");
            this.paymentTrackerListView.Adapter = paymentTrackerAdapter;
            this.paymentTrackerListView.InvalidateViews();

			if (this.PaymentTrackerList.Count() == 0)
			{
				this.paymentTrackerListView.Selector = Resources.GetDrawable(Android.Resource.Color.Transparent);
				this.paymentTrackerListView.Clickable = false;
				this.paymentTrackerListView.ChoiceMode = ChoiceMode.None;
			}
        }

        private void Bt_Schedule_Click(object sender, EventArgs e)
        {
            bt_Schedule.Selected = true;
            bt_History.Selected = false;

            this.PaymentTrackerList = this.InstalmentScheduleList;
            paymentTrackerAdapter = new PaymentTrackerAdapter(this, this.PaymentTrackerList.ToList(), "Schedule");
            this.paymentTrackerListView.Adapter = paymentTrackerAdapter;
            this.paymentTrackerListView.InvalidateViews();

			if (this.PaymentTrackerList.Count() == 0)
			{
				this.paymentTrackerListView.Selector = Resources.GetDrawable(Android.Resource.Color.Transparent);
				this.paymentTrackerListView.Clickable = false;
				this.paymentTrackerListView.ChoiceMode = ChoiceMode.None;
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
    }
}