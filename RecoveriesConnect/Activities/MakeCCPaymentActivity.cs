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
using Android.Views.InputMethods;
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "MakeCCPayment", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class MakeCCPaymentActivity : Activity
    {
        const int Start_DATE_DIALOG_ID = 0;

        public EditText et_Amount;
        public EditText et_Expire;
        public EditText et_NameOnCard;
        public EditText et_CardNumber;
        public EditText et_CVV;
        public int card_type = 0;

        public DateTime Expire;
        public ImageButton bt_Master;
        public ImageButton bt_Visa;
        public Button bt_Continue;


        public TextView err_Amount;
        public TextView err_CardType;
        public TextView err_NameOnCard;
        public TextView err_CardNumber;
        public TextView err_Expiry;
        public TextView err_CVV;

        public bool IsValidate = true;

        Alert alert;
        public int paymentType = 0;

		public InputMethodManager inputManager;

		public List<InstalmentSummaryModel> instalmentList;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.MakeCCPayment);

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


			Expire = DateTime.Today;

            et_Expire = FindViewById<EditText>(Resource.Id.et_ExpiryMonth);
            et_Expire.Click += delegate { ShowDialog(Start_DATE_DIALOG_ID); };

            et_Amount = FindViewById<EditText>(Resource.Id.et_Amount);
            et_NameOnCard = FindViewById<EditText>(Resource.Id.et_NameOnCard);
            et_CardNumber = FindViewById<EditText>(Resource.Id.et_CardNumber);
            et_CVV = FindViewById<EditText>(Resource.Id.et_CVV);


            bt_Master = FindViewById<ImageButton>(Resource.Id.bt_Master);
            bt_Master.Click += Bt_Master_Click;


            bt_Visa = FindViewById<ImageButton>(Resource.Id.bt_Visa);
            bt_Visa.Click += bt_Visa_Click;

            bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
            bt_Continue.Click += Bt_Continue_Click;

            err_Amount = FindViewById<TextView>(Resource.Id.err_Amount);
            err_CardType = FindViewById<TextView>(Resource.Id.err_CardType);
            err_NameOnCard = FindViewById<TextView>(Resource.Id.err_NameOnCard);
            err_CardNumber = FindViewById<TextView>(Resource.Id.err_CardNumber);
            err_Expiry = FindViewById<TextView>(Resource.Id.err_Expiry);
            err_CVV = FindViewById<TextView>(Resource.Id.err_CVV);


			//BuildVersionCodes currentapiVersion = Android.OS.Build.VERSION.SdkInt;

			//Andy Testing
			//this.et_Amount.Text = "10.1";
			//this.card_type = 2;
			//this.et_CVV.Text = "123";
			//this.et_Expire.Text = "07/2016";
			//this.et_NameOnCard.Text = "Andy Pham";
			//this.et_CardNumber.Text = "4444333322221111";

			if (Settings.MakePaymentInFull)
			{
				this.et_Amount.Text = Settings.TotalOutstanding.ToString();
				this.et_Amount.Enabled = false;
			}
			else if (Settings.MakePaymentIn3Part || Settings.MakePaymentInstallment) {
				this.et_Amount.Text = Settings.FirstAmountOfInstallment.ToString();
				this.et_Amount.Enabled = false;
			}
			else
			{
				inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
				Keyboard.ShowKeyboard(this,this.et_Amount);
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
            err_CardType.Text = "";
            err_NameOnCard.Text = "";
            err_CardNumber.Text = "";
            err_Expiry.Text = "";
            err_CVV.Text = "";
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

            if (this.card_type == 0)
            {
                err_CardType.Text = Resources.GetString(Resource.String.EnterCardType);
                IsValidate = false;
            }

            if (et_NameOnCard.Text.Length == 0)
            {
                err_NameOnCard.Text = Resources.GetString(Resource.String.EnterName);
                IsValidate = false;
            }
            else
            {
                if (!Validation.IsValidCreditCardName(et_NameOnCard.Text))
                {
                    err_NameOnCard.Text = Resources.GetString(Resource.String.NOCInvalid);
                    IsValidate = false;
                }
            }

            if (et_CardNumber.Text.Length == 0)
            {
                err_CardNumber.Text = Resources.GetString(Resource.String.EnterCardNumber);
                IsValidate = false;
            }
            else
            {
                if (!Validation.IsValidCreditCardNumber(this.card_type, et_CardNumber.Text))
                {
                    err_CardNumber.Text = Resources.GetString(Resource.String.CNInvalid);
                    IsValidate = false;
                }
            }

            if (et_Expire.Text.Length == 0)
            {
                err_Expiry.Text = Resources.GetString(Resource.String.EnterCardExpiry);
                IsValidate = false;
            }

            if (et_CVV.Text.Length == 0)
            {
                err_CVV.Text = Resources.GetString(Resource.String.EnterCVV);
                IsValidate = false;
            }
            else
            {
                if (et_CVV.Text.Length != 3)
                {
                    err_CVV.Text = Resources.GetString(Resource.String.CVVInvalid);
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

            //});
        }

        private void DoPayment()
        {
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
            var url2 = url + "/Api/MakeCreditCardPayment";

            var expiry = this.et_Expire.Text.Split('/');
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
                    CardType = this.card_type,
                    NameOnCard = this.et_NameOnCard.Text,
                    CreditCardNumber = this.et_CardNumber.Text,
                    CreditCardExpiryYear = expiry[1],
                    CreditCardExpiryMonth = expiry[0],
                    CreditCardCVV = this.et_CVV.Text,
                    PaymentType = paymentType,
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
						TrackingHelper.SendTracking("Make CC Payment");

						Intent Intent = new Intent(this, typeof(SummaryActivity));

                        Intent.PutExtra("tv_TransactionDescription", ObjectReturn2.TransactionDescription);
                        Intent.PutExtra("tv_ReceiptNumber", ObjectReturn2.ReceiptNumber);
                        Intent.PutExtra("tv_Amount", ObjectReturn2.Amount);
                        Intent.PutExtra("tv_Time", ObjectReturn2.Time);
                        Intent.PutExtra("tv_Date", ObjectReturn2.Date);
                        Intent.PutExtra("tv_Name", ObjectReturn2.Name);
                        Intent.PutExtra("PaymentMethod", 1);
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
				AndHUD.Shared.Dismiss();
                this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
            }
        }


        private void Bt_Master_Click(object sender, EventArgs e)
        {
            if (this.card_type == 0 || this.card_type == 2)
            {
                bt_Master.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.Master_Color));
                bt_Visa.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.visa));
                card_type = 1;
            }
            else
            if (this.card_type == 1)
            {
                bt_Master.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.master));
                bt_Visa.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.visa));
                card_type = 0;
            }
        }

        private void bt_Visa_Click(object sender, EventArgs e)
        {
            if (this.card_type == 0 || this.card_type == 1)
            {
                this.bt_Master.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.master));
                this.bt_Visa.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.Visa_Color));
                this.card_type = 2;
            }
            else
            if (this.card_type == 2)
            {
                this.bt_Master.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.master));
                this.bt_Visa.SetBackgroundDrawable(this.Resources.GetDrawable(Resource.Drawable.visa));
                this.card_type = 0;
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
                    return new DatePickerDialogNoYear(this, OnExpireMonthSet, Expire.Year, Expire.Month - 1, Expire.Day);
            }
            return null;
        }

        void OnExpireMonthSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.et_Expire.Text = e.Date.ToString("MM'/'yyyy");
        }
    }
}