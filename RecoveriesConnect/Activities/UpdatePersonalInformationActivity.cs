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
using System.Threading;
using RecoveriesConnect.Activities;
using AndroidHUD;
using System.Collections.Generic;
using RecoveriesConnect.Adapter;

namespace RecoveriesConnect
{
	[Activity(Label = "UpdatePersonalInformation", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class UpdatePersonalInformationActivity : Activity
	{
		public EditText et_StreetAddress1;
        public EditText et_StreetAddress2;
        public EditText et_StreetAddress3;
        public EditText et_StreetSuburb;
        public EditText et_StreetState;
        public EditText et_StreetPostCode;

        public EditText et_MailAddress1;
        public EditText et_MailAddress2;
        public EditText et_MailAddress3;
        public EditText et_MailState;
        public EditText et_MailSuburb;
        public EditText et_MailPostCode;

		public EditText et_HomePhone;
		public EditText et_WorkPhone;
		public EditText et_MobilePhone;
        public EditText et_Email;

		public TextView err_MobilePhone;

		Alert alert;

		public bool IsValidate = true;

		public Button bt_Continue;


        List<string> listPrefer = new List<string>(4);
        int selectedIndex = 0;
        public CallbackTimeSpinnerAdapter callBackAdapter;
        public Spinner spinner_Prefer;

        public string ScreenComeFrom = "";

        public string var_TransactionDescription,var_ReceiptNumber,var_Amount, var_Time, var_Date, var_Name, var_ClientName = "";
        public int var_PaymentType, var_PaymentMethod, var_PaymentId, var_FirstDebtorPaymentInstallmentId = 0;

        PersonalInfo ObjectReturn2;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.UpdatePersonalInformation);

            //**************************************************//
            ScreenComeFrom = Intent.GetStringExtra("ScreenComeFrom") ?? "";

            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

            if (ScreenComeFrom.Equals("HomeMenu"))
            {
                var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
                upArrow.SetColorFilter(Color.ParseColor("#006571"), PorterDuff.Mode.SrcIn);
                ActionBar.SetHomeAsUpIndicator(upArrow);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetHomeButtonEnabled(true);
            }



			LinearLayout lLayout = new LinearLayout(this);
			lLayout.SetGravity(GravityFlags.CenterVertical);
			LinearLayout.LayoutParams textViewParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			textViewParameters.RightMargin = (int)(30 * this.Resources.DisplayMetrics.Density);

			TextView myTitle = new TextView(this);
			myTitle.Text = "View/Update Personal Information";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			et_StreetAddress1 = FindViewById<EditText>(Resource.Id.et_StreetAddress1);
            et_StreetAddress2 = FindViewById<EditText>(Resource.Id.et_StreetAddress2);
            et_StreetAddress3 = FindViewById<EditText>(Resource.Id.et_StreetAddress3);

            et_StreetSuburb = FindViewById<EditText>(Resource.Id.et_StreetSuburb);
            et_StreetState = FindViewById<EditText>(Resource.Id.et_StreetState);
            et_StreetPostCode = FindViewById<EditText>(Resource.Id.et_StreetPostcode);

            et_MailAddress1 = FindViewById<EditText>(Resource.Id.et_MailAddress1);
            et_MailAddress2 = FindViewById<EditText>(Resource.Id.et_MailAddress2);
            et_MailAddress3 = FindViewById<EditText>(Resource.Id.et_MailAddress3);
            et_MailSuburb = FindViewById<EditText>(Resource.Id.et_MailSuburb);
            et_MailState = FindViewById<EditText>(Resource.Id.et_MailState);
            et_MailPostCode = FindViewById<EditText>(Resource.Id.et_MailPostcode);


            et_HomePhone = FindViewById<EditText>(Resource.Id.et_HomePhone);
			et_WorkPhone = FindViewById<EditText>(Resource.Id.et_WorkPhone);
			et_MobilePhone = FindViewById<EditText>(Resource.Id.et_MobilePhone);
            et_Email = FindViewById<EditText>(Resource.Id.et_Email);


            bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

			err_MobilePhone = FindViewById<TextView>(Resource.Id.err_MobilePhone);


            listPrefer.Add("");
            listPrefer.Add("Home Phone");
            listPrefer.Add("Work Phone");
            listPrefer.Add("Mobile Phone");

            spinner_Prefer = FindViewById<Spinner>(Resource.Id.spinner_Prefer);
            callBackAdapter = new CallbackTimeSpinnerAdapter(this, this.listPrefer);

            spinner_Prefer.Adapter = callBackAdapter;

            spinner_Prefer.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Prefer_ItemSelected);

            GetPersonalInfo();

            spinner_Prefer.SetSelection(selectedIndex);

            if (!ScreenComeFrom.Equals("HomeMenu"))
            {
                var_TransactionDescription = Intent.GetStringExtra("tv_TransactionDescription") ?? "";
                var_ReceiptNumber = Intent.GetStringExtra("tv_ReceiptNumber") ?? "";
                var_Amount = Intent.GetStringExtra("tv_Amount") ?? "";
                var_Time = Intent.GetStringExtra("tv_Time") ?? "";
                var_Date = Intent.GetStringExtra("tv_Date") ?? "";
                var_Name = Intent.GetStringExtra("tv_Name") ?? "";
                var_PaymentType = Intent.GetIntExtra("PaymentType", 0);
                var_PaymentMethod = Intent.GetIntExtra("PaymentMethod", 0);
                var_PaymentId = Intent.GetIntExtra("PaymentId", 0);
                var_ClientName = Intent.GetStringExtra("ClientName") ?? "";
                var_FirstDebtorPaymentInstallmentId = Intent.GetIntExtra("FirstDebtorPaymentInstallmentId", 0);
            }
        }
        private void Prefer_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            selectedIndex = e.Position;
        }

        private void Bt_Continue_Click(object sender, EventArgs e)
		{
			this.RunOnUiThread(() => this.bt_Continue.Enabled = false);

			err_MobilePhone.Text = "";

			IsValidate = true;

			if (this.et_MobilePhone.Text.Length == 0)
			{
				err_MobilePhone.Text = Resources.GetString(Resource.String.EnterPhoneNumber);
				IsValidate = false;
			}

			if (IsValidate)
			{
				//Do Payment
				ThreadPool.QueueUserWorkItem(o => DoUpdate());
			}
			else
			{
				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
			}
		}

		private void GetPersonalInfo()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetPersonalInformationDetail";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					DebtorCode = Settings.DebtorCodeSelected,
					Action = "G",
				}
			};

			try
			{
				ObjectReturn2 = new PersonalInfo();

				string results2 = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results2))
				{
                    AndHUD.Shared.Dismiss();
                    this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonalInfo>(results2);

					AndHUD.Shared.Dismiss();

					if (ObjectReturn2.IsSuccess)
					{

						this.et_StreetAddress1.Text = ObjectReturn2.StreetAddress1;
                        this.et_StreetAddress2.Text = ObjectReturn2.StreetAddress2;
                        this.et_StreetAddress3.Text = ObjectReturn2.StreetAddress3;
                        this.et_StreetSuburb.Text   = ObjectReturn2.StreetSuburb;
                        this.et_StreetState.Text    = ObjectReturn2.StreetState;
                        this.et_StreetPostCode.Text = ObjectReturn2.StreetPostCode;

                        this.et_MailAddress1.Text = ObjectReturn2.MailAddress1;
                        this.et_MailAddress2.Text = ObjectReturn2.MailAddress2;
                        this.et_MailAddress3.Text = ObjectReturn2.MailAddress3;
                        this.et_MailSuburb.Text = ObjectReturn2.MailSuburb;
                        this.et_MailState.Text = ObjectReturn2.MailState;
                        this.et_MailPostCode.Text = ObjectReturn2.MailPostCode;

                        this.et_HomePhone.Text = ObjectReturn2.HomeNumber;
						this.et_WorkPhone.Text = ObjectReturn2.WorkNumber;
						this.et_MobilePhone.Text = ObjectReturn2.MobileNumber;
                        this.et_Email.Text = ObjectReturn2.EmailAddress;

                        this.selectedIndex = ObjectReturn2.Preferred;
                    }
					else
					{
						this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
						this.RunOnUiThread(() => alert = new Alert(this, "Error", ObjectReturn2.Error));
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

		private void DoUpdate()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetPersonalInformationDetail";

            var json2 = new
            {
                Item = new
                {
                    ReferenceNumber = Settings.RefNumber,
                    DebtorCode = Settings.DebtorCodeSelected,
                    Action = "U",

                    StreetAddress1 = this.Compare(this.et_StreetAddress1.Text.Trim(), ObjectReturn2.Marked_StreetAddress1, ObjectReturn2.Origin_StreetAddress1),
                    StreetAddress2 = this.Compare(this.et_StreetAddress2.Text.Trim(), ObjectReturn2.Marked_StreetAddress2, ObjectReturn2.Origin_StreetAddress2),
                    StreetAddress3 = this.Compare(this.et_StreetAddress3.Text.Trim(), ObjectReturn2.Marked_StreetAddress3, ObjectReturn2.Origin_StreetAddress3),
                    StreetSuburb = this.Compare(this.et_StreetSuburb.Text.Trim(), ObjectReturn2.Marked_StreetSuburb, ObjectReturn2.Origin_StreetSuburb),
                    StreetState = this.Compare(this.et_StreetState.Text.Trim(), ObjectReturn2.Marked_StreetState, ObjectReturn2.Origin_StreetState),
                    StreetPostCode = this.Compare(this.et_StreetPostCode.Text.Trim(), ObjectReturn2.Marked_StreetPostCode, ObjectReturn2.Origin_StreetPostCode),

                    MailAddress1 = this.Compare(this.et_MailAddress1.Text.Trim(), ObjectReturn2.Marked_MailAddress1, ObjectReturn2.Origin_MailAddress1),
                    MailAddress2 = this.Compare(this.et_MailAddress2.Text.Trim(), ObjectReturn2.Marked_MailAddress2, ObjectReturn2.Origin_MailAddress2),
                    MailAddress3 = this.Compare(this.et_MailAddress3.Text.Trim(), ObjectReturn2.Marked_MailAddress3, ObjectReturn2.Origin_MailAddress3),
                    MailSuburb = this.Compare(this.et_MailSuburb.Text.Trim(), ObjectReturn2.Marked_MailSuburb, ObjectReturn2.Origin_MailSuburb),
                    MailState = this.Compare(this.et_MailState.Text.Trim(), ObjectReturn2.Marked_MailState, ObjectReturn2.Origin_MailState),
                    MailPostCode = this.Compare(this.et_MailPostCode.Text.Trim(), ObjectReturn2.Marked_MailPostCode, ObjectReturn2.Origin_MailPostCode),

                    HomeNumber = this.Compare(this.et_HomePhone.Text.Trim(), ObjectReturn2.Marked_HomeNumber, ObjectReturn2.Origin_HomeNumber),
                    WorkNumber = this.Compare(this.et_WorkPhone.Text.Trim(), ObjectReturn2.Marked_WorkNumber, ObjectReturn2.Origin_WorkNumber),
                    MobileNumbers = this.Compare(this.et_MobilePhone.Text.Trim(), ObjectReturn2.Marked_MobileNumbers, ObjectReturn2.Origin_MobileNumbers),
                    EmailAddress = this.Compare(this.et_Email.Text.Trim(), ObjectReturn2.Marked_EmailAddress, ObjectReturn2.Origin_EmailAddress),

                    Preferred = this.selectedIndex
				}
			};

			try
			{
				var ObjectReturn2 = new PaymentInfo();

				string results2 = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results2))
				{
					AndHUD.Shared.Dismiss();
					this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentInfo>(results2);


					if (ObjectReturn2.IsSuccess)
					{

						TrackingHelper.SendTracking("Update Personal Info");

                        if (this.ScreenComeFrom.Equals("HomeMenu"))
                        {
                            Intent Intent = new Intent(this, typeof(FinishActivity));
                            Intent.PutExtra("Message", "Your personal information has been updated successfully");
                            StartActivity(Intent);

                        }
                        else
                        {
                            Intent Intent = new Intent(this, typeof(SummaryActivity));

                            Intent.PutExtra("tv_TransactionDescription", var_TransactionDescription);
                            Intent.PutExtra("tv_ReceiptNumber", var_ReceiptNumber);
                            Intent.PutExtra("tv_Amount", var_Amount);
                            Intent.PutExtra("tv_Time", var_Time);
                            Intent.PutExtra("tv_Date", var_Date);
                            Intent.PutExtra("tv_Name", var_Name);
                            Intent.PutExtra("PaymentType", var_PaymentType);
                            Intent.PutExtra("PaymentMethod", var_PaymentMethod);
                            Intent.PutExtra("PaymentId", var_PaymentId);
                            Intent.PutExtra("ClientName", var_ClientName);
                            Intent.PutExtra("FirstDebtorPaymentInstallmentId", var_FirstDebtorPaymentInstallmentId);
                            StartActivity(Intent);

                        }

                        AndHUD.Shared.Dismiss();

                        this.Finish();
                    }
					else
					{
						AndHUD.Shared.Dismiss();
						this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
						alert = new Alert(this, "Error", ObjectReturn2.Error);
						alert.Show();
					}
				}
			}
			catch (Exception ee)
			{
				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
				AndHUD.Shared.Dismiss();
			}
		}

        public string Compare(string displayText, string encryptText, string originText)
        {
            if (displayText.Equals(encryptText))
            {
                return originText;
            }
            else
            {
                return displayText;
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

        public override void OnBackPressed()
        {
            if (this.ScreenComeFrom.Equals("HomeMenu"))
            {
                base.OnBackPressed();
            }
        }
    }
}

