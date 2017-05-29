using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using System;
using RecoveriesConnect.Models.Api;
using System.Threading;
using AndroidHUD;
using System.Collections.Generic;
using RecoveriesConnect.Adapter;
using Android.Util;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "ScheduleCallback", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class ScheduleCallbackActivity : Activity
	{
		const int Start_DATE_DIALOG_ID = 0;
		const int Start_TimeFrom_DIALOG_ID = 1;
		const int Start_TimeTo_DIALOG_ID = 2;

		public EditText et_Name;
		public EditText et_Phone;
		public EditText et_Date;
		public EditText et_Notes;

		public DateTime Date;
		public Spinner  spinner_Callback;

		public Button bt_Continue;


		public TextView err_Name;
		public TextView err_Phone;
		public TextView err_Date;
		public TextView err_TimeFrom;
        public LinearLayout layout_button;

		public bool IsValidate = true;

		Alert alert;

		List<string> listCallBack;
		int selectedIndex = -1;
		public CallbackTimeSpinnerAdapter callBackAdapter;
		bool isAvailableCallBackTime = false;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.ScheduleCallback);

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
			myTitle.Text = "Schedule Callback";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			Date = DateTime.Now;

			et_Name = FindViewById<EditText>(Resource.Id.et_Name);
			et_Phone = FindViewById<EditText>(Resource.Id.et_Phone);
			et_Date = FindViewById<EditText>(Resource.Id.et_Date);
			spinner_Callback = FindViewById<Spinner>(Resource.Id.spinner_Callback);

            et_Notes = FindViewById<EditText>(Resource.Id.et_Notes);


			et_Date.Click += delegate { ShowDialog(Start_DATE_DIALOG_ID); };
            this.et_Date.Text = DateTime.Today.Date.ToShortDateString();
            LoadCallbackList(DateTime.Today);

            err_Name = FindViewById<TextView>(Resource.Id.err_Name);
			err_Phone = FindViewById<TextView>(Resource.Id.err_Phone);
			err_Date = FindViewById<TextView>(Resource.Id.err_Date);
			err_TimeFrom = FindViewById<TextView>(Resource.Id.err_TimeFrom);

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

            layout_button = FindViewById<LinearLayout>(Resource.Id.linearLayout_button);


            Keyboard.ShowKeyboard(this, this.et_Name);

			TrackingHelper.SendTracking("Open Schedule Callback");

            this.Window.SetSoftInputMode(SoftInput.AdjustResize);

            var metrics = Resources.DisplayMetrics;
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            // Gets the layout params that will allow you to resize the layout
            ViewGroup.LayoutParams temp = layout_button.LayoutParameters;
            // Changes the height and width to the specified *pixels*
            temp.Height = ConvertDpToPixel(heightInDp - 400);
            layout_button.LayoutParameters = temp;

        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int ConvertDpToPixel(float Dp)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, Dp, Resources.DisplayMetrics);

            return pixel;
        }
        //private void Spinner_Callback_Click(object sender, EventArgs e)
        //{
        //    if(this.listCallBack.Count == 0)
        //    {
        //        err_TimeFrom.Text = Resources.GetString(Resource.String.EnterDate);
        //    }
        //    else
        //    {
        //        err_TimeFrom.Text = "";
        //    }
        //}

        public void LoadCallbackList(DateTime selectedDate)
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetCallBackTime";
			var json2 = new
			{
				Item = new
				{
					ReferenceNumber  = Settings.RefNumber,
					CallbackDate 	 = selectedDate.ToString("yyyy/MM/dd"),
					CallbackTimeSlot = DateTime.Now.ToString("HH:mm:ss")
				}
			};

			try
			{
				var ObjectReturn = new CallbackReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results))
				{
                    AndHUD.Shared.Dismiss();
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{
					ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<CallbackReturnModel>(results);

					this.isAvailableCallBackTime = ObjectReturn.IsSuccess;

					AndHUD.Shared.Dismiss();

					this.listCallBack = ObjectReturn.CallbackSlot;
				}
			}
			catch (Exception ee)
			{
				AndHUD.Shared.Dismiss();
			}

			callBackAdapter = new CallbackTimeSpinnerAdapter(this, this.listCallBack);

			spinner_Callback.Adapter = callBackAdapter;

			spinner_Callback.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Callback_ItemSelected);
		}

        private void Callback_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			selectedIndex = e.Position;
		}

		private void Bt_Continue_Click(object sender, EventArgs e)
		{
			this.RunOnUiThread(() => this.bt_Continue.Enabled = false);
		
			err_Name.Text = "";
			err_Phone.Text = "";
			err_Date.Text = "";
			err_TimeFrom.Text = "";

			IsValidate = true;


			if (this.et_Name.Text.Length == 0)
			{
				err_Name.Text = Resources.GetString(Resource.String.EnterName);
				IsValidate = false;
			}


			if (this.et_Phone.Text.Length == 0)
			{
				err_Phone.Text = Resources.GetString(Resource.String.EnterPhoneNumber);
				IsValidate = false;
			}
			else
				if (!Validation.isValidPhone(this.et_Phone.Text)){
					err_Phone.Text = Resources.GetString(Resource.String.PhoneInvalid);
					IsValidate = false;
				}


			if (this.et_Date.Text.Length == 0)
			{
				err_Date.Text = Resources.GetString(Resource.String.EnterDate);
				IsValidate = false;
			}
			else if (!this.isAvailableCallBackTime) {
				err_Date.Text = Resources.GetString(Resource.String.EnterDiffDate);
				IsValidate = false;
			}
			else if (this.selectedIndex == -1)
			{
				err_TimeFrom.Text = Resources.GetString(Resource.String.EnterTimeFrom);
				IsValidate = false;
			}


			if (IsValidate)
			{
				//Do Payment
				ThreadPool.QueueUserWorkItem(o => DoRequest());
			}
			else
			{
				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
			}

		}

		private void DoRequest()
		{

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/RequestCallback";



			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					Number = this.et_Phone.Text,
					Name = this.et_Name.Text,
					Date = this.et_Date.Text,
					CallBackTimeSlot = this.listCallBack[selectedIndex],
					Notes = this.et_Notes.Text
				}
			};

			try
			{
				var ObjectReturn2 = new JsonReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results))
				{
					AndHUD.Shared.Dismiss();
					this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

					if (ObjectReturn2.IsSuccess)
					{

						Intent Intent = new Intent(this, typeof(FinishActivity));

						Intent.PutExtra("Message", "Callback request successfully submitted. One of our friendly operators will call you on the day and time you have requested");

						StartActivity(Intent);

						AndHUD.Shared.Dismiss();

						this.Finish();
					}
					else
					{
						AndHUD.Shared.Dismiss();
						this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
						this.RunOnUiThread(() => alert = new Alert(this, "Error", ObjectReturn2.Errors[0].ErrorMessage));
						this.RunOnUiThread(() => alert.Show());
					}
				}
			}
			catch (Exception ee)
			{
				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
				AndHUD.Shared.Dismiss();	
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

		protected override Dialog OnCreateDialog(int id)
		{
			switch (id)
			{
				case Start_DATE_DIALOG_ID:
					
					var datePickerDialog = new DatePickerDialog(this, OnDateSet, Date.Year, Date.Month - 1, Date.Day);
					var datePicker = datePickerDialog.DatePicker;

					DateTime maxdate = DateTime.Today.AddMonths(1);
					datePicker.MaxDate = new Java.Util.Date(maxdate.Year - 1900, maxdate.Month - 1, maxdate.Day).Time;

					DateTime minDate = DateTime.Today;
					datePicker.MinDate = new Java.Util.Date(minDate.Year - 1900, minDate.Month - 1, minDate.Day).Time;

					return datePickerDialog;

			}
			return null;
		}

		void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this.et_Date.Text = e.Date.ToShortDateString();
			this.LoadCallbackList(e.Date);
		}

		//void OnTimeFromSet(object sender, TimePickerDialog.TimeSetEventArgs e)
		//{
		//	var minute = e.Minute.ToString();

		//	if (e.Minute < 10) {
		//		minute = "0" + minute;
		//	}

		//	var hour = e.HourOfDay.ToString();

		//	if (e.HourOfDay < 10)
		//	{
		//		hour = "0" + hour;
		//	}

		//	this.et_TimeFrom.Text = hour + ":" + minute;
		//}

		//void OnTimeToSet(object sender, TimePickerDialog.TimeSetEventArgs e)
		//{
		//	var minute = e.Minute.ToString();

		//	if (e.Minute < 10)
		//	{
		//		minute = "0" + minute;
		//	}

		//	var hour = e.HourOfDay.ToString();

		//	if (e.HourOfDay < 10)
		//	{
		//		hour = "0" + hour;
		//	}

		//	this.et_TimeTo.Text = hour + ":" + minute;
		//}
	}
}