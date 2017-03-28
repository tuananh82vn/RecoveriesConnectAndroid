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
		public EditText et_StreetAddress;
		public EditText et_MailAddress;
		public EditText et_HomePhone;
		public EditText et_WorkPhone;
		public EditText et_MobilePhone;
        public EditText et_Email;



        public TextView err_StreetAddress;
		public TextView err_MailAddress;
		public TextView err_HomePhone;
		public TextView err_WorkPhone;
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

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.UpdatePersonalInformation);

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
			myTitle.Text = "View/Update Personal Information";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			et_StreetAddress = FindViewById<EditText>(Resource.Id.et_StreetAddress);
			et_MailAddress = FindViewById<EditText>(Resource.Id.et_MailAddress);
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

            ScreenComeFrom = Intent.GetStringExtra("ScreenComeFrom") ?? "";
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
				var ObjectReturn2 = new PersonalInfo();

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

						this.et_StreetAddress.Text = ObjectReturn2.Address1s;
						this.et_MailAddress.Text = ObjectReturn2.Address2s;
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
					DebtorCode  = Settings.DebtorCodeSelected,
					Action = "U",
					Address1s = this.et_StreetAddress.Text.Trim(),
					Address2s = this.et_MailAddress.Text.Trim(),
					HomeNumber = this.et_HomePhone.Text.Trim(),
					WorkNumber = this.et_WorkPhone.Text.Trim(),
					MobileNumbers = this.et_MobilePhone.Text.Trim(),
                    EmailAddress = this.et_Email.Text.Trim(),
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

