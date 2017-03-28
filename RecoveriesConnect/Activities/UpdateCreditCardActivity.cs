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
	[Activity(Label = "UpdateCreditCard", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class UpdateCreditCardActivity : Activity
	{
		const int Start_DATE_DIALOG_ID = 0;
		public EditText et_CardNumber;
		public EditText et_Expiry;

		public DateTime Expire;

		public TextView err_CardNumber;
		public TextView err_Expiry;

		Alert alert;
		public bool IsValidate = true;
		public Button bt_Continue;

		public ImageButton bt_Master;
		public ImageButton bt_Visa;
		public TextView err_CardType;

		public int card_type = 0;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.UpdateCreditCard);

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
			myTitle.Text = "View/Update Credit Card";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//


			Expire = DateTime.Today;
			et_Expiry = FindViewById<EditText>(Resource.Id.et_Expiry);
			et_Expiry.Click += delegate { ShowDialog(Start_DATE_DIALOG_ID); };

			et_CardNumber = FindViewById<EditText>(Resource.Id.et_CardNumber);


			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;

			bt_Master = FindViewById<ImageButton>(Resource.Id.bt_Master);
			bt_Master.Click += Bt_Master_Click;

			bt_Visa = FindViewById<ImageButton>(Resource.Id.bt_Visa);
			bt_Visa.Click += bt_Visa_Click;

			err_Expiry = FindViewById<TextView>(Resource.Id.err_Expiry);
			err_CardType = FindViewById<TextView>(Resource.Id.err_CardType);
			err_CardNumber = FindViewById<TextView>(Resource.Id.err_CardNumber);

			//Andy Testing
			//this.et_Expire.Text = "07/2016";
			//this.et_CardNumber.Text = "4444333322221111";

			GetCardInfo();

		}
		private void Bt_Continue_Click(object sender, EventArgs e)
		{

			err_CardNumber.Text = "";
			err_Expiry.Text = "";
			err_CardType.Text = "";

			IsValidate = true;

			if (this.card_type == 0)
			{
				err_CardType.Text = Resources.GetString(Resource.String.EnterCardType);
				IsValidate = false;
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


			if (et_Expiry.Text.Length == 0)
			{
				err_Expiry.Text = Resources.GetString(Resource.String.EnterCardExpiry);
				IsValidate = false;
			}


			if (IsValidate)
			{
				//Do Payment
				ThreadPool.QueueUserWorkItem(o => DoUpdate());
			}

		}

		private void GetCardInfo() {


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
                        this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                        this.RunOnUiThread(() => alert.Show());
                    }
					else
					{

						ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentInfo>(results2);

						AndHUD.Shared.Dismiss();

						if (ObjectReturn2.IsSuccess)
						{
							if (ObjectReturn2.RecType.Equals("CC"))
							{
								this.et_CardNumber.Text = ObjectReturn2.CCNo;
								this.et_Expiry.Text = ObjectReturn2.ExpiryDate.Substring(0,2)+"/"+ObjectReturn2.ExpiryDate.Substring(2, 4);
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

			var expiry = this.et_Expiry.Text.Replace("/", "");


			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					Action = "U",
					RecType = "CC",
					CCNo = this.et_CardNumber.Text,
					ExpiryDate = expiry,
					BsbNo = "",
					AccountNo = "",
					AccountName = ""
				}
			};

			try
			{
				var ObjectReturn2 = new PaymentInfo();

				string results2 = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results2))
				{
                    AndHUD.Shared.Dismiss();
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentInfo>(results2);


					if (ObjectReturn2.IsSuccess)
					{

						TrackingHelper.SendTracking("Update Credit Card");

						Intent Intent = new Intent(this, typeof(FinishActivity));

						Intent.PutExtra("Message", "Your credit card has been updated successfully");

						StartActivity(Intent);

						AndHUD.Shared.Dismiss();

						this.Finish();
					}
					else
					{
						AndHUD.Shared.Dismiss();
                        this.RunOnUiThread(() => alert = new Alert(this, "Error", ObjectReturn2.Error));
                        this.RunOnUiThread(() => alert.Show());
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
			this.et_Expiry.Text = e.Date.ToString("MM'/'yyyy");
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
	}
}

