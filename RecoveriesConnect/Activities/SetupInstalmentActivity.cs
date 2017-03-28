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
using Android.Text;
using Android.Views.InputMethods;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "SetupInstalment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class SetupInstalmentActivity : Activity, TextView.IOnEditorActionListener
    {
		public Alert alert;

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
        public TextView tv_LastDate;
        public TextView tv_NumberInstalment;

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
			myTitle.Text = "Pay In Instalments";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			Date1 = DateTime.Now;

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

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

			et_Date = FindViewById<EditText>(Resource.Id.et_Date);
            this.et_Date.Text = DateTime.Today.Date.ToShortDateString();
            et_Date.Click += delegate { ShowDialog(DATE_1_DIALOG_ID); };

			et_FirstAmount = FindViewById<EditText>(Resource.Id.et_Amount);
            et_FirstAmount.TextChanged += InputSearchOnTextChanged;

            tv_LastDate = FindViewById<TextView>(Resource.Id.tv_LastDate);
            tv_NumberInstalment = FindViewById<TextView>(Resource.Id.tv_NumberInstalment);

        }

        private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
        {
            DoCalculation();
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            return true;
        }

        private void Bt_Continue_Click(object sender, EventArgs e)
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



		private void DoSetup()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			SetPayment.Set("instalment");

			Intent Intent = new Intent(this, typeof(InstalmentSummaryActivity));

			Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentSummaryList.ToArray());

			Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

			StartActivity(Intent);

			AndHUD.Shared.Dismiss();

		}

        private void DoCalculation()
        {
            var amountText = this.et_FirstAmount.Text;
            if (amountText.Trim().Length > 0)
            {
                decimal amount = Decimal.Parse(amountText);
                var firstDateText = this.et_Date.Text;
                if (firstDateText.Trim().Length > 0)
                {
                    DateTime firstDate = DateTime.Parse(firstDateText);

                    decimal totalAmount = Settings.TotalOutstanding;

                    decimal paidAmount = 0;
                    decimal instalmentAmount = amount < totalAmount ? amount : totalAmount;
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

                        if (Frequency == 0)
                        {
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

                    if (instalmentSummaryList.Count > 0)
                    {
                        var lastInstalment = this.instalmentSummaryList[instalmentSummaryList.Count - 1].Amount;
                        if (lastInstalment < 10)
                        {
                            this.instalmentSummaryList.RemoveAt(this.instalmentSummaryList.Count - 1);
                            this.instalmentSummaryList[this.instalmentSummaryList.Count - 1].Amount = this.instalmentSummaryList[this.instalmentSummaryList.Count - 1].Amount + lastInstalment;
                        }

                        this.tv_NumberInstalment.Text = this.instalmentSummaryList.Count.ToString();
                        this.tv_LastDate.Text = this.instalmentSummaryList[this.instalmentSummaryList.Count - 1].PaymentDate;
                    }
                }
            }
        }

		private void RadioButtonClick(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Text.Equals("Weekly"))
			{
				this.Frequency = 0;
                DoCalculation();

            }
			else
			if (rb.Text.Equals("Fortnightly"))
			{
				this.Frequency = 1;
                DoCalculation();
            }
			else
			if (rb.Text.Equals("Monthly"))
			{
				this.Frequency = 2;
                DoCalculation();
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
            DoCalculation();
        }
	}
}


