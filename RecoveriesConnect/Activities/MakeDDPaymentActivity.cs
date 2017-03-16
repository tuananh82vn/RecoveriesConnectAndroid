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
using System.Linq;
using RecoveriesConnect.Models.Api;
using System.Threading;
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "MakeDDPayment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class MakeDDPaymentActivity : Activity
	{

		public EditText et_Amount;
		public EditText et_AccountName;
		public EditText et_BSB1;
		public EditText et_BSB2;
		public EditText et_AccountNumber;

		public Button bt_Continue;

		public TextView err_Amount;
		public TextView err_AccountName;
		public TextView err_BSB1;
		public TextView err_BSB2;
		public TextView err_AccountNumber;

		public bool IsValidate = true;

		Alert alert;
		public int paymentType = 0;

		public List<InstalmentSummaryModel> instalmentList;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.MakeDDPayment);

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
			myTitle.Text = "Make a Payment";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//


			et_Amount = FindViewById<EditText>(Resource.Id.et_Amount);
			et_AccountName = FindViewById<EditText>(Resource.Id.et_AccountName);
			et_BSB1 = FindViewById<EditText>(Resource.Id.et_BSB1);
			et_BSB2 = FindViewById<EditText>(Resource.Id.et_BSB2);
			et_AccountNumber = FindViewById<EditText>(Resource.Id.et_AccountNumber);

			err_Amount = FindViewById<TextView>(Resource.Id.err_Amount);
			err_AccountName = FindViewById<TextView>(Resource.Id.err_AccountName);
			err_BSB1 = FindViewById<TextView>(Resource.Id.err_BSB1);
			err_BSB2 = FindViewById<TextView>(Resource.Id.err_BSB2);
			err_AccountNumber = FindViewById<TextView>(Resource.Id.err_AccountNumber);

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

			//Andy Testing
			//this.et_Amount.Text = "10.1";
			//this.et_AccountName.Text = "Andy Pham";
			//this.et_BSB1.Text = "123";
			//this.et_BSB2.Text = "123";
			//this.et_AccountNumber.Text = "123456";

			if (Settings.MakePaymentInFull)
			{
				this.et_Amount.Text = Settings.TotalOutstanding.ToString();
				this.et_Amount.Enabled = false;
			}
			else if (Settings.MakePaymentIn3Part || Settings.MakePaymentInstallment)
			{
				this.et_Amount.Text = Settings.FirstAmountOfInstallment.ToString();
				this.et_Amount.Enabled = false;
			}
			LoadData();
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
					instalment.PaymentDate = DateTime.Parse(item.PaymentDate).ToString("yyyy-MM-dd");
					instalment.Amount = item.Amount;
					instalmentList.Add(instalment);
				}
			}
		}
		private void Bt_Continue_Click(object sender, EventArgs e)
		{
			this.RunOnUiThread(() => this.bt_Continue.Enabled = false);

			err_Amount.Text = "";
			err_AccountName.Text = "";
			err_BSB1.Text = "";
			err_BSB2.Text = "";
			err_AccountNumber.Text = "";

			IsValidate = true;

			if (et_Amount.Text.Length == 0)
			{
				err_Amount.Text = Resources.GetString(Resource.String.EnterAmount);
				IsValidate = false;
			}
			else
			{
				var amountValue = decimal.Parse(et_Amount.Text);
				if (amountValue <= 10)
				{
					err_Amount.Text = Resources.GetString(Resource.String.AmountLessThanRequired);
					IsValidate = false;
				}
				if (amountValue > Settings.TotalOutstanding)
				{
					err_Amount.Text = Resources.GetString(Resource.String.AmountLessThanOutStanding);
					IsValidate = false;
				}
			}

			if (et_AccountName.Text.Length == 0)
			{
				err_AccountName.Text = Resources.GetString(Resource.String.EnterAccountName);
				IsValidate = false;
			}
			else
			{
				if (!Validation.IsValidCreditCardName(et_AccountName.Text))
				{
					err_AccountName.Text = Resources.GetString(Resource.String.AccountNameInvalid);
					IsValidate = false;
				}
			}

			if (et_BSB1.Text.Length == 0)
			{
				err_BSB1.Text = Resources.GetString(Resource.String.EnterBSB1);
				IsValidate = false;
			}
			else
			{
				if (et_BSB1.Text.Length != 3)
				{
					err_BSB1.Text = Resources.GetString(Resource.String.BSB1Invalid);
					IsValidate = false;
				}
			}

			if (et_BSB2.Text.Length == 0)
			{
				err_BSB2.Text = Resources.GetString(Resource.String.EnterBSB2);
				IsValidate = false;
			}
			else
			{
				if (et_BSB2.Text.Length != 3)
				{
					err_BSB2.Text = Resources.GetString(Resource.String.BSB2Invalid);
					IsValidate = false;
				}
			}

			if (et_AccountNumber.Text.Length == 0)
			{
				err_AccountNumber.Text = Resources.GetString(Resource.String.EnterBSB2);
				IsValidate = false;
			}
			else
			{
				if (et_AccountNumber.Text.Length < 5 || et_AccountNumber.Text.Length > 15)
				{
					err_AccountNumber.Text = Resources.GetString(Resource.String.AccountNumberInvalid);
					IsValidate = false;
				}
			}

			if (IsValidate)
			{
				//Do Payment
				ThreadPool.QueueUserWorkItem(o => DoPayment());
			}
			else
			{
				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
			}

		}

		private void DoPayment()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);
			string url = Settings.InstanceURL;
			var url2 = url + "/Api/MakeDebitPayment";

			if (Settings.MakePaymentInFull)
			{
				paymentType = 1;
			}
			else
		  	if (Settings.MakePaymentIn3Part)
			{
				paymentType = 2;
			}
			else
		  	if (Settings.MakePaymentInstallment)
			{
				paymentType = 3;
			}
			else
		  	if (Settings.MakePaymentOtherAmount)
			{
				paymentType = 4;
			}

			var DebtorPaymentInstallment = "";

			if (Settings.MakePaymentIn3Part || Settings.MakePaymentInstallment)
			{
				JSonHelper helper = new JSonHelper();
				string jsonResult = helper.ConvertObjectToJSon(this.instalmentList);

				DebtorPaymentInstallment = jsonResult;
			}

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					Amount = this.et_Amount.Text,
					DirectDebitAccountName = this.et_AccountName.Text,
					DirectDebitAccountNumber = this.et_AccountNumber.Text,
					DirectDebitBSB1 = this.et_BSB1.Text,
					DirectDebitBSB2 = this.et_BSB2.Text,
					PaymentType = paymentType,
					PaymentMethod = "2",
					DebtorPaymentInstallment = DebtorPaymentInstallment,
					InstalmentPaymentFrequency = Settings.Frequency
				}
			};

			try
			{
				var ObjectReturn2 = new PaymentReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);




				if (string.IsNullOrEmpty(results))
				{

					AndHUD.Shared.Dismiss();
					this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
					alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
					alert.Show();
				}
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentReturnModel>(results);

					if (ObjectReturn2.IsSuccess)
					{
						TrackingHelper.SendTracking("Make DD Payment");

						Intent Intent = new Intent(this, typeof(SummaryActivity));

						Intent.PutExtra("tv_TransactionDescription", ObjectReturn2.TransactionDescription);
						Intent.PutExtra("tv_ReceiptNumber", ObjectReturn2.ReceiptNumber);
						Intent.PutExtra("tv_Amount", ObjectReturn2.Amount);
						Intent.PutExtra("tv_Time", ObjectReturn2.Time);
						Intent.PutExtra("tv_Date", ObjectReturn2.Date);
						Intent.PutExtra("tv_Name", ObjectReturn2.Name);
						Intent.PutExtra("PaymentMethod", 2);
						Intent.PutExtra("PaymentType", this.paymentType);
						Intent.PutExtra("PaymentId", ObjectReturn2.PaymentId);
						Intent.PutExtra("ClientName", ObjectReturn2.ClientName);
						Intent.PutExtra("FirstDebtorPaymentInstallmentId", ObjectReturn2.FirstDebtorPaymentInstallmentId);

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

	}
}

