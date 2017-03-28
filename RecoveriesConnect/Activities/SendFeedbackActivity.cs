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
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "SendFeedback", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class SendFeedbackActivity : Activity
	{
		public Button bt_Continue;
		public EditText et_Subject;
		public EditText et_Content;
		public TextView err_Subject;
		public TextView err_Content;

		public bool IsValidate = true;

		Alert alert;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.SendFeedback);

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
			myTitle.Text = "Send Feedback";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
			bt_Continue.Click += bt_Continue_Click;

			et_Subject = FindViewById<EditText>(Resource.Id.et_Subject);
			et_Content = FindViewById<EditText>(Resource.Id.et_Content);

			err_Subject = FindViewById<TextView>(Resource.Id.err_Subject);
			err_Content = FindViewById<TextView>(Resource.Id.err_Content);


			Keyboard.ShowKeyboard(this, et_Subject);

		}

		private void bt_Continue_Click(object sender, EventArgs e)
		{

			this.RunOnUiThread(() => this.bt_Continue.Enabled = false);

			err_Subject.Text = "";
			err_Content.Text = "";
			this.IsValidate = true;

			if (this.et_Subject.Text.Length == 0)
			{
				err_Subject.Text = Resources.GetString(Resource.String.EnterSubject);
				IsValidate = false;
			}

			if (this.et_Content.Text.Length == 0)
			{
				err_Content.Text = Resources.GetString(Resource.String.EnterContent);
				IsValidate = false;
			}

			if (IsValidate)
			{
				//Do Payment
				ThreadPool.QueueUserWorkItem(o => DoSendFeedback());
			}
			else
			{
				this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
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

		private void DoSendFeedback()
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);
			string url = Settings.InstanceURL;
			var url2 = url + "/Api/SendFeedback";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					Subject = this.et_Subject.Text,
					Content = this.et_Content.Text
				}
			};

			try
			{
				var ObjectReturn2 = new JsonReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);

			
				if (string.IsNullOrEmpty(results))
				{
					AndHUD.Shared.Dismiss();
					this.RunOnUiThread(() => this.bt_Continue.Enabled = true);
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);


					if (ObjectReturn2.IsSuccess)
					{

						TrackingHelper.SendTracking("Sent Feedback");

						Intent Intent = new Intent(this, typeof(FinishActivity));

						Intent.PutExtra("Message", "Your feedback has been sent successfully");

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
	}
}

