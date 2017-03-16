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

namespace RecoveriesConnect
{
	[Activity(Label = "UpdateBankAccount", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class UpdateBankAccountActivity : Activity
	{
		public EditText et_AccountName;
		public EditText et_BSB1;
		public EditText et_BSB2;
		public EditText et_AccountNumber;


		public TextView err_AccountName;
		public TextView err_BSB1;
		public TextView err_BSB2;
		public TextView err_AccountNumber;

		Alert alert;

		public bool IsValidate = true;

		public Button bt_Continue;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.UpdateBankAccount);

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
			myTitle.Text = "View/Update Bank Account";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//


			et_AccountName = FindViewById<EditText>(Resource.Id.et_AccountName);
			et_BSB1 = FindViewById<EditText>(Resource.Id.et_BSB1);
			et_BSB2 = FindViewById<EditText>(Resource.Id.et_BSB2);
			et_AccountNumber = FindViewById<EditText>(Resource.Id.et_AccountNumber);


			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;


			err_AccountName = FindViewById<TextView>(Resource.Id.err_AccountName);
			err_BSB1 = FindViewById<TextView>(Resource.Id.err_BSB1);
			err_BSB2 = FindViewById<TextView>(Resource.Id.err_BSB2);
			err_AccountNumber = FindViewById<TextView>(Resource.Id.err_AccountNumber);

			//Andy Testing
			//this.et_Expire.Text = "07/2016";
			//this.et_CardNumber.Text = "4444333322221111";

			GetBankInfo();

		}
		private void Bt_Continue_Click(object sender, EventArgs e)
		{

			err_AccountName.Text = "";
			err_BSB1.Text = "";
			err_BSB2.Text = "";
			err_AccountNumber.Text = "";

			IsValidate = true;



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
				ThreadPool.QueueUserWorkItem(o => DoUpdate());
			}

		}

		private void GetBankInfo()
		{

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetPaymentDetail";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					Action = "G",
				}
			};

			try
			{
				var ObjectReturn2 = new PaymentInfo();

				string results2 = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results2))
				{
					AndHUD.Shared.Dismiss();
					alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
					alert.Show();
				}
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentInfo>(results2);

					AndHUD.Shared.Dismiss();

					if (ObjectReturn2.IsSuccess)
					{
						TrackingHelper.SendTracking("Update Bank Account");

						if (ObjectReturn2.RecType.Equals("DD"))
						{
							this.et_AccountName.Text = ObjectReturn2.AccountName;
							this.et_AccountNumber.Text = ObjectReturn2.AccountNo;
							this.et_BSB1.Text = ObjectReturn2.BsbNo.Substring(0,3);
							this.et_BSB2.Text = ObjectReturn2.BsbNo.Substring(3, 3);
						}
					}
				}
			}
			catch (Exception ee)
			{
				AndHUD.Shared.Dismiss();
			}
		}

		private void DoUpdate()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetPaymentDetail";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					Action = "U",
					RecType = "DD",
					CCNo = "",
					ExpiryDate = "",
					BsbNo = this.et_BSB1.Text.Trim() + this.et_BSB2.Text.Trim(),
					AccountNo = this.et_AccountNumber.Text,
					AccountName = this.et_AccountName.Text
				}
			};

			try
			{
				var ObjectReturn2 = new PaymentInfo();

				string results2 = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results2))
				{
					AndHUD.Shared.Dismiss();
					alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
					alert.Show();
				}
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentInfo>(results2);

					if (ObjectReturn2.IsSuccess)
					{

						Intent Intent = new Intent(this, typeof(FinishActivity));

						Intent.PutExtra("Message", "Your bank account has been updated successfully");

						StartActivity(Intent);

						AndHUD.Shared.Dismiss();

						this.Finish();
					}
					else
					{
						AndHUD.Shared.Dismiss();
						alert = new Alert(this, "Error", ObjectReturn2.Error);
						alert.Show();
					}
				}
			}
			catch (Exception ee)
			{
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

