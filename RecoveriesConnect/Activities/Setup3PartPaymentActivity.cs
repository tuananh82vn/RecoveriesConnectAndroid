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

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "Setup3PartPayment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class Setup3PartPaymentActivity : Activity
	{
		Alert alert;

		const int DATE_1_DIALOG_ID = 0;
		const int DATE_2_DIALOG_ID = 1;
		const int DATE_3_DIALOG_ID = 2;

		public EditText et_FirstAmount;
		public EditText et_SecondAmount;
		public EditText et_ThirdAmount;

		public EditText et_FirstDate;
		public EditText et_SecondDate;
		public EditText et_ThirdDate;

		public DateTime Date1;
		public DateTime Date2;
		public DateTime Date3;

		public Button bt_Continue;

		public TextView err_FirstAmount;
		public TextView err_SecondAmount;
		public TextView err_ThirdAmount;

		public TextView err_FirstDate;
		public TextView err_SecondDate;
		public TextView err_ThirdDate;

		public TextView tv_Message1;
        public TextView tv_Message2;


        public bool IsValidate = true;

		public LinearLayout ln_Pay3;

		public List<InstalmentSummaryModel> instalmentSummaryList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.Setup3PartPayment);

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
			myTitle.Text = "Setup " + Settings.MaxNoPay.ToString() + " Payments";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//
			Date1 = DateTime.Now;
			Date2 = DateTime.Now;
			Date3 = DateTime.Now;

			et_FirstDate = FindViewById<EditText>(Resource.Id.et_FirstDate);
			et_FirstDate.Click += delegate { ShowDialog(DATE_1_DIALOG_ID); };

			et_SecondDate = FindViewById<EditText>(Resource.Id.et_SecondDate);
			et_SecondDate.Click += delegate { ShowDialog(DATE_2_DIALOG_ID); };

			et_ThirdDate = FindViewById<EditText>(Resource.Id.et_ThirdDate);
			et_ThirdDate.Click += delegate { ShowDialog(DATE_3_DIALOG_ID); };


			et_FirstAmount = FindViewById<EditText>(Resource.Id.et_FirstAmount);
			et_SecondAmount = FindViewById<EditText>(Resource.Id.et_SecondAmount);
			et_ThirdAmount = FindViewById<EditText>(Resource.Id.et_ThirdAmount);

			err_FirstAmount = FindViewById<TextView>(Resource.Id.err_FirstAmount);
			err_SecondAmount = FindViewById<TextView>(Resource.Id.err_SecondAmount);
			err_ThirdAmount = FindViewById<TextView>(Resource.Id.err_ThirdAmount);
			err_FirstDate = FindViewById<TextView>(Resource.Id.err_FirstDate);
			err_SecondDate = FindViewById<TextView>(Resource.Id.err_SecondDate);
			err_ThirdDate = FindViewById<TextView>(Resource.Id.err_ThirdDate);

			tv_Message1 = FindViewById<TextView>(Resource.Id.tv_Message1);
            var message = "";

            if (Settings.MaxNoPay == 2)
            {
                message = "Please enter the dates for the first and second payments.";
            }
            if (Settings.MaxNoPay == 3)
            {
                message = "Please enter the dates for the first, second and the third payments.";
            }
            tv_Message1.Text = message;

            tv_Message2 = FindViewById<TextView>(Resource.Id.tv_Message2);
            tv_Message2.Text = "You must pay the full amount with in " + Settings.ThreePartDurationDays +" days.";

			ln_Pay3 = FindViewById<LinearLayout>(Resource.Id.ln_Pay3);

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

			initView();

		}

		private void Bt_Continue_Click(object sender, EventArgs e)
		{

			
				err_FirstAmount.Text = "";
				err_SecondAmount.Text = "";
				err_ThirdAmount.Text = "";
				err_FirstDate.Text = "";
				err_SecondDate.Text = "";
				err_ThirdDate.Text = "";

				IsValidate = true;

				if (et_FirstAmount.Text.Length == 0)
				{
					err_FirstAmount.Text = Resources.GetString(Resource.String.EnterFirstPayment);
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
				}

				if (this.et_FirstDate.Text.Length == 0)
				{
					err_FirstDate.Text = Resources.GetString(Resource.String.EnterFirstPaymentDate);
					IsValidate = false;
				}
				else
				{
					var enterDate = DateTime.Parse(this.et_FirstDate.Text);
					var maxDate = DateTime.Today.AddDays(7);
					if (enterDate > maxDate)
					{
						err_FirstDate.Text = Resources.GetString(Resource.String.FirstPaymentIn7);
						IsValidate = false;
					}
				}

				if (et_SecondAmount.Text.Length == 0)
				{
					err_SecondAmount.Text = Resources.GetString(Resource.String.EnterSecondPayment);
					IsValidate = false;
				}
				else
				{
					var amountValue = decimal.Parse(et_SecondAmount.Text);
					if (amountValue <= 10)
					{
						err_SecondAmount.Text = Resources.GetString(Resource.String.AmountLessThanRequired);
						IsValidate = false;
					}
					if (amountValue > Settings.TotalOutstanding)
					{
						err_SecondAmount.Text = Resources.GetString(Resource.String.AmountLessThanOutStanding);
						IsValidate = false;
					}
				}

				if (this.et_SecondDate.Text.Length == 0)
				{
					err_SecondDate.Text = Resources.GetString(Resource.String.EnterSecondPaymentDate);
					IsValidate = false;
				}
				else
				{
					var enterDate = DateTime.Parse(this.et_SecondDate.Text);
					var firstDate = DateTime.Parse(this.et_FirstDate.Text);
					var MaxDate = firstDate.AddDays(Settings.ThreePartMaxDaysBetweenPayments);

					if (enterDate <= firstDate)
					{
						err_SecondDate.Text = Resources.GetString(Resource.String.SecondPaymentMusAfterFirstPayment);
						IsValidate = false;
					}
					if (enterDate > MaxDate)
					{
						err_SecondDate.Text = Resources.GetString(Resource.String.MaxTime2Payment) + Settings.ThreePartMaxDaysBetweenPayments.ToString() + " days.";
						IsValidate = false;
					}
				}

				if (Settings.MaxNoPay == 3)
				{
					if (this.et_ThirdAmount.Text.Length == 0)
					{
						err_ThirdAmount.Text = Resources.GetString(Resource.String.EnterThirdPayment);
						IsValidate = false;
					}
					else
					{
						var amountValue = decimal.Parse(et_ThirdAmount.Text);
						if (amountValue <= 10)
						{
							err_ThirdAmount.Text = Resources.GetString(Resource.String.AmountLessThanRequired);
							IsValidate = false;
						}
						if (amountValue > Settings.TotalOutstanding)
						{
							err_ThirdAmount.Text = Resources.GetString(Resource.String.AmountLessThanOutStanding);
							IsValidate = false;
						}
					}

					if (this.et_ThirdDate.Text.Length == 0)
					{
						err_ThirdDate.Text = Resources.GetString(Resource.String.EnterThirdPaymentDate);
						IsValidate = false;
					}
					else
					{
						var secondDate = DateTime.Parse(this.et_SecondDate.Text);
						var thirdDate = DateTime.Parse(this.et_ThirdDate.Text);

						var MaxDate = thirdDate.AddDays(Settings.ThreePartMaxDaysBetweenPayments);

                        var totalMaxDate = DateTime.Parse(this.et_FirstDate.Text).AddDays(Settings.ThreePartDurationDays);

                        if (thirdDate <= secondDate)
						{
							err_ThirdDate.Text = Resources.GetString(Resource.String.ThirdPaymentMusAfterSecondPayment);
							IsValidate = false;
						}
						if (thirdDate > MaxDate)
						{
							err_ThirdDate.Text = Resources.GetString(Resource.String.MaxTime2Payment) + Settings.ThreePartMaxDaysBetweenPayments.ToString() + " days.";
                            IsValidate = false;
						}

                        if (thirdDate > totalMaxDate)
                        {
                            err_ThirdDate.Text = Resources.GetString(Resource.String.Total3MaxTime) + Settings.ThreePartDurationDays.ToString() + " days from today.";
                            IsValidate = false;
                        }
					}
				}
                else
                {
                    var secondDate = DateTime.Parse(this.et_SecondDate.Text);

                    var totalMaxDate = DateTime.Parse(this.et_FirstDate.Text).AddDays(Settings.ThreePartDurationDays);

                    if (secondDate > totalMaxDate)
                    {
                        err_ThirdDate.Text = Resources.GetString(Resource.String.Total2MaxTime) + Settings.ThreePartDurationDays.ToString() + " days from today.";
                        IsValidate = false;
                    }
                }

				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);

				if (IsValidate)
				{
					//Do Payment
					ThreadPool.QueueUserWorkItem(o => DoSetup());
				}
		}

		public void DoSetup() {

			var firstAmount = Decimal.Parse(this.et_FirstAmount.Text);
			var secondAmount = Decimal.Parse(this.et_SecondAmount.Text);

			instalmentSummaryList = new List<InstalmentSummaryModel>();

			TrackingHelper.SendTracking("Setup 3 part");

			if (Settings.MaxNoPay == 2)
			{
				if (Settings.TotalOutstanding != (firstAmount + secondAmount))
				{
						this.RunOnUiThread(() =>
					    {
						   alert = new Alert(this, "Error", Resources.GetString(Resource.String.InstalmentAmountInvalid));
						   alert.Show();
						   this.bt_Continue.Enabled = true;

					    });
				}
				else
				{


					SetPayment.Set("3part");
					Settings.Frequency = 0;

					if (DateTime.Parse(this.et_FirstDate.Text) > DateTime.Today)
					{
						Settings.IsFuturePayment = true;
					}
					else
					{
						Settings.IsFuturePayment = false;
					}

					var instalmentModel1 = new InstalmentSummaryModel(this.et_FirstDate.Text, double.Parse(this.et_FirstAmount.Text));
					var instalmentModel2 = new InstalmentSummaryModel(this.et_SecondDate.Text, double.Parse(this.et_SecondAmount.Text));
					instalmentSummaryList.Add(instalmentModel1);
					instalmentSummaryList.Add(instalmentModel2);

					Intent Intent = new Intent(this, typeof(InstalmentSummaryActivity));

					Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentSummaryList.ToArray());

					Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

					StartActivity(Intent);
				}
			}
			else
			{
				var thirdAmount = Decimal.Parse(this.et_ThirdAmount.Text);

				if (Settings.TotalOutstanding != (firstAmount + secondAmount + thirdAmount))
				{
					this.RunOnUiThread(() =>
					   {
						   alert = new Alert(this, "Error", Resources.GetString(Resource.String.InstalmentAmountInvalid));
						   alert.Show();
						   this.bt_Continue.Enabled = true;

					   });
				}
				else
				{
					SetPayment.Set("3part");
					Settings.Frequency = 0;

					if (DateTime.Parse(this.et_FirstDate.Text) > DateTime.Today)
					{
						Settings.IsFuturePayment = true;
					}
					else
					{
						Settings.IsFuturePayment = false;
					}

					var instalmentModel1 = new InstalmentSummaryModel(this.et_FirstDate.Text, double.Parse(this.et_FirstAmount.Text));
					var instalmentModel2 = new InstalmentSummaryModel(this.et_SecondDate.Text, double.Parse(this.et_SecondDate.Text));
					var instalmentModel3 = new InstalmentSummaryModel(this.et_ThirdDate.Text, double.Parse(this.et_ThirdAmount.Text));

					instalmentSummaryList.Add(instalmentModel1);
					instalmentSummaryList.Add(instalmentModel2);
					instalmentSummaryList.Add(instalmentModel3);

					Intent Intent = new Intent(this, typeof(InstalmentSummaryActivity));

					Intent.PutParcelableArrayListExtra("InstalmentSummary", instalmentSummaryList.ToArray());

					Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

					StartActivity(Intent);				
				}
			}
		}

		public void initView() {
			if (Settings.MaxNoPay == 2)
			{
				ln_Pay3.Visibility = ViewStates.Invisible;
			}

            var totalAmount = Settings.TotalOutstanding;

            if (Settings.MaxNoPay == 2)
            {
                var firstAmount = Math.Round(totalAmount / 2, 2, MidpointRounding.ToEven);
                this.et_FirstAmount.Text = firstAmount.ToString();
                this.et_SecondAmount.Text = (totalAmount - firstAmount).ToString();
            }
            else
                if (Settings.MaxNoPay == 3)
                {
                    var firstAmount = Math.Round(totalAmount / 3, 2, MidpointRounding.ToEven);
                    var secondAmount = Math.Round(totalAmount / 3, 2, MidpointRounding.ToEven);

                    this.et_FirstAmount.Text = firstAmount.ToString();
                    this.et_SecondAmount.Text = secondAmount.ToString();
                    this.et_ThirdAmount.Text = (totalAmount - firstAmount - secondAmount).ToString();
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

					//DateTime maxdate = DateTime.Today.AddMonths(1);
					//datePicker.MaxDate = new Java.Util.Date(maxdate.Year - 1900, maxdate.Month - 1, maxdate.Day).Time;

					DateTime minDate = DateTime.Today;
					datePicker1.MinDate = new Java.Util.Date(minDate.Year - 1900, minDate.Month - 1, minDate.Day).Time;
					return datePickerDialog1;
					
				case DATE_2_DIALOG_ID:

					var datePickerDialog2 = new DatePickerDialog(this, OnDate2Set, this.Date2.Year, Date2.Month - 1, Date2.Day);
					var datePicker2 = datePickerDialog2.DatePicker;

					//DateTime maxdate = DateTime.Today.AddMonths(1);
					//datePicker.MaxDate = new Java.Util.Date(maxdate.Year - 1900, maxdate.Month - 1, maxdate.Day).Time;

					DateTime minDate2 = DateTime.Today;
					datePicker2.MinDate = new Java.Util.Date(minDate2.Year - 1900, minDate2.Month - 1, minDate2.Day).Time;

					return datePickerDialog2;

				case DATE_3_DIALOG_ID:

					var datePickerDialog3 = new DatePickerDialog(this, OnDate3Set, this.Date3.Year, Date3.Month - 1, Date3.Day);
					var datePicker3 = datePickerDialog3.DatePicker;

					//DateTime maxdate = DateTime.Today.AddMonths(1);
					//datePicker.MaxDate = new Java.Util.Date(maxdate.Year - 1900, maxdate.Month - 1, maxdate.Day).Time;

					DateTime minDate3 = DateTime.Today;
					datePicker3.MinDate = new Java.Util.Date(minDate3.Year - 1900, minDate3.Month - 1, minDate3.Day).Time;

					return datePickerDialog3;
			}
			return null;
		}

		void OnDate1Set(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this.et_FirstDate.Text = e.Date.ToShortDateString();
		}
		void OnDate2Set(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this.et_SecondDate.Text = e.Date.ToShortDateString();
		}
		void OnDate3Set(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this.et_ThirdDate.Text = e.Date.ToShortDateString();
		}
	}
}

