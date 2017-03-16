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
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "SetupInstalment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class SetupInstalmentActivity : Activity
	{
		public LinearLayout ln_Body;
		public Alert alert;
		public Switch sw_Message;

		public RadioButton rb_Weekly;
		public RadioButton rb_Monthly;
		public RadioButton rb_Fortnightly;
		public TextView tv_Message;

		public int Frequency = -1;

		public TextView err_FirstAmount;
		public TextView err_FirstDate;
		public TextView err_Frequency;

		public EditText et_FirstAmount;
		public EditText et_Date;
		const int DATE_1_DIALOG_ID = 0;

		public DateTime Date1;

		public Button bt_Continue;

		public List<InstalmentSummaryModel> instalmentSummaryList;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.SetupInstalment);

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
			myTitle.Text = "Setup Instalment";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			Date1 = DateTime.Now;

			ln_Body = FindViewById<LinearLayout>(Resource.Id.ln_Body);
			ln_Body.Visibility = ViewStates.Invisible;

			sw_Message = FindViewById<Switch>(Resource.Id.sw_Message);
			sw_Message.CheckedChange += sw_Message_Change;

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;
			bt_Continue.Enabled = false;

			rb_Weekly = FindViewById<RadioButton>(Resource.Id.rb_Weekly);
			rb_Monthly = FindViewById<RadioButton>(Resource.Id.rb_Monthly);
			rb_Fortnightly = FindViewById<RadioButton>(Resource.Id.rb_Fortnightly);

			rb_Weekly.Click += RadioButtonClick;
			rb_Fortnightly.Click += RadioButtonClick;
			rb_Monthly.Click += RadioButtonClick;

			tv_Message = FindViewById<TextView>(Resource.Id.tv_Message);
			err_FirstAmount = FindViewById<TextView>(Resource.Id.err_FirstAmount);
			err_FirstDate = FindViewById<TextView>(Resource.Id.err_FirstDate);
			err_Frequency = FindViewById<TextView>(Resource.Id.err_Frequency);


			var message = "";
			if (Settings.IsAllowMonthlyInstallment)
			{
				message = "Would you like to check if you qualify for a weekly/fortnightly or monthly payment schedule to meet your obligation in paying the amount of " + MoneyFormat.Convert(Settings.TotalOutstanding) + " ?";
			}
			else
			{
				message = "Would you like to check if you qualify for a weekly/fortnightly payment schedule to meet your obligation in paying the amount of " + MoneyFormat.Convert(Settings.TotalOutstanding) + " ?";
				this.rb_Monthly.Visibility = ViewStates.Invisible;
			}
			tv_Message.Text = message;

			et_Date = FindViewById<EditText>(Resource.Id.et_Date);
			et_Date.Click += delegate { ShowDialog(DATE_1_DIALOG_ID); };

			et_FirstAmount = FindViewById<EditText>(Resource.Id.et_Amount);
		}

		public void sw_Message_Change(object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			if (e.IsChecked)
			{
				ln_Body.Visibility = ViewStates.Visible;
				bt_Continue.Enabled = true;
			}
			else
			{
				bt_Continue.Enabled = false;
				OnBackPressed();
			}
		}

		private void Bt_Continue_Click(object sender, EventArgs e)
		{
			if (sw_Message.Checked)
			{
				err_FirstAmount.Text = "";
				err_FirstDate.Text = "";
				err_Frequency.Text = "";

				var IsValidate = true;

				if (!this.rb_Weekly.Checked && !this.rb_Fortnightly.Checked && !this.rb_Monthly.Checked) { 
					this.err_Frequency.Text = Resources.GetString(Resource.String.EnterFrequency);
					IsValidate = false;
				}

				if (et_FirstAmount.Text.Length == 0)
				{
					this.err_FirstAmount.Text = Resources.GetString(Resource.String.EnterFirstPayment);
					IsValidate = false;
				}
				else
				{
					var amountValue = decimal.Parse(et_FirstAmount.Text);
					if (amountValue <= 10)
					{
						err_FirstAmount.Text = Resources.GetString(Resource.String.AmountLessThanRequired);
						IsValidate = false;
					}
					if (amountValue > Settings.TotalOutstanding)
					{
						err_FirstAmount.Text = Resources.GetString(Resource.String.AmountLessThanOutStanding);
						IsValidate = false;
					}

					if (this.Frequency == 0)
					{
						if (amountValue < Settings.WeeklyAmount)
						{
							err_FirstAmount.Text = Resources.GetString(Resource.String.AmountLessThanNegotiated);
							IsValidate = false;
						}
					}
					else
					if (this.Frequency == 1)
					{
						if (amountValue < Settings.FortnightAmount)
						{
							err_FirstAmount.Text = Resources.GetString(Resource.String.AmountLessThanNegotiated);
							IsValidate = false;
						}
					}
					else
					if (this.Frequency == 2)
					{
						if (amountValue < Settings.MonthlyAmount)
						{
							err_FirstAmount.Text = Resources.GetString(Resource.String.AmountLessThanNegotiated);
							IsValidate = false;
						}
					}
				}

				if (this.et_Date.Text.Length == 0)
				{
					err_FirstDate.Text = Resources.GetString(Resource.String.EnterFirstPaymentDate);
					IsValidate = false;
				}
				else
				{
					var enterDate = DateTime.Parse(this.et_Date.Text);
					var maxDate = DateTime.Today.AddDays(30);
					if (enterDate > maxDate)
					{
						err_FirstDate.Text = Resources.GetString(Resource.String.FirstPaymentIn30);
						IsValidate = false;
					}
				}


				if (IsValidate)
				{
					ThreadPool.QueueUserWorkItem(o => DoSetup());
				}
			}
			else
			{
				this.RunOnUiThread(() =>
				{
					alert = new Alert(this, "Notice", "PLease asnwer the question first.");
					alert.Show();
				});
			}
		}



		private void DoSetup()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);
			SetPayment.Set("instalment");

			decimal totalAmount = Settings.TotalOutstanding;
			decimal amount = Decimal.Parse(this.et_FirstAmount.Text);
			decimal paidAmount = 0;
			decimal instalmentAmount = amount < totalAmount ? amount : totalAmount;
			DateTime firstDate = DateTime.Parse(this.et_Date.Text);
			if (firstDate > DateTime.Today)
			{
				Settings.IsFuturePayment = true;
			}
			else
			{
				Settings.IsFuturePayment = false;
			}
			Settings.Frequency = this.Frequency + 1;

			instalmentSummaryList = new List<InstalmentSummaryModel>();

			while (totalAmount > paidAmount && instalmentAmount > 0)
			{
				
				var instalmentModel1 = new InstalmentSummaryModel(firstDate.ToShortDateString(), Double.Parse(instalmentAmount.ToString()));
				instalmentSummaryList.Add(instalmentModel1);

				paidAmount = paidAmount + instalmentAmount;

				if (paidAmount + amount <= totalAmount)
				{
					instalmentAmount = amount;
				}
				else
				{
					instalmentAmount = Math.Round(totalAmount - paidAmount, 2, MidpointRounding.ToEven);
				}

				if (Frequency == 0) {
					firstDate = firstDate.AddDays(7);
				}
				else
					if (Frequency == 1)
					{
						firstDate = firstDate.AddDays(14);
					}
					else
					if (Frequency == 2)
					{
						firstDate = firstDate.AddMonths(1);
					}

			}

			if (instalmentSummaryList.Count > 0) {
				var lastInstalment = this.instalmentSummaryList[instalmentSummaryList.Count - 1].Amount;
				if (lastInstalment < 10) {
					this.instalmentSummaryList.RemoveAt(this.instalmentSummaryList.Count - 1);
					this.instalmentSummaryList[this.instalmentSummaryList.Count - 1].Amount = this.instalmentSummaryList[this.instalmentSummaryList.Count - 1].Amount + lastInstalment;
				}
			}

			Intent Intent = new Intent(this, typeof(InstalmentSummaryActivity));

			Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentSummaryList.ToArray());

			Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

			StartActivity(Intent);

			AndHUD.Shared.Dismiss();

		}

		private void RadioButtonClick(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Text.Equals("Weekly"))
			{
				this.Frequency = 0;
			}
			else
			if (rb.Text.Equals("Fortnightly"))
			{
				this.Frequency = 1;
			}
			else
			if (rb.Text.Equals("Monthly"))
			{
				this.Frequency = 2;
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
				case DATE_1_DIALOG_ID:

					var datePickerDialog1 = new DatePickerDialog(this, OnDate1Set, this.Date1.Year, Date1.Month - 1, Date1.Day);
					var datePicker1 = datePickerDialog1.DatePicker;

					DateTime maxdate = DateTime.Today.AddDays(30);
					datePicker1.MaxDate = new Java.Util.Date(maxdate.Year - 1900, maxdate.Month - 1, maxdate.Day).Time;

					DateTime minDate = DateTime.Today;
					datePicker1.MinDate = new Java.Util.Date(minDate.Year - 1900, minDate.Month - 1, minDate.Day).Time;
					return datePickerDialog1;

			}
			return null;
		}

		void OnDate1Set(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this.et_Date.Text = e.Date.ToShortDateString();
		}
	}
}


