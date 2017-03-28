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
	[Activity(Label = "UpdatePersonalInformation", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class UpdatePersonalInformationActivity : Activity
	{
		public EditText et_StreetAddress;
		public EditText et_MailAddress;
		public EditText et_HomePhone;
		public EditText et_WorkPhone;
		public EditText et_MobilePhone;



		public TextView err_StreetAddress;
		public TextView err_MailAddress;
		public TextView err_HomePhone;
		public TextView err_WorkPhone;
		public TextView err_MobilePhone;

		Alert alert;

		public bool IsValidate = true;

		public Button bt_Continue;


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


			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += Bt_Continue_Click;


			err_StreetAddress = FindViewById<TextView>(Resource.Id.err_StreetAddress);
			err_MailAddress = FindViewById<TextView>(Resource.Id.err_MailAddress);
			err_HomePhone = FindViewById<TextView>(Resource.Id.err_HomePhone);
			err_WorkPhone = FindViewById<TextView>(Resource.Id.err_WorkPhone);
			err_MobilePhone = FindViewById<TextView>(Resource.Id.err_MobilePhone);


			GetPersonalInfo();

		}
		private void Bt_Continue_Click(object sender, EventArgs e)
		{
			this.RunOnUiThread(() => this.bt_Continue.Enabled = false);

			err_StreetAddress.Text = "";
			err_MailAddress.Text = "";
			err_HomePhone.Text = "";
			err_WorkPhone.Text = "";
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
					MobileNumbers = this.et_MobilePhone.Text.Trim()
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

						Intent Intent = new Intent(this, typeof(FinishActivity));

						Intent.PutExtra("Message", "Your personal information has been updated successfully");

						StartActivity(Intent);

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

