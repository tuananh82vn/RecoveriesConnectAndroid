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
using AndroidHUD;

namespace RecoveriesConnect
{
	[Activity(Label = "InboxDetailMessage", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class InboxDetailMessageActivity : Activity
	{
		Alert alert;

		public Inbox item;

		Button bt_Delete;
		public TextView tv_Date;
		public TextView tv_Content;
		AlertDialog.Builder alert1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.InboxDetailMessage);

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
			myTitle.Text = "Inbox Detail";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//


			bt_Delete = FindViewById<Button>(Resource.Id.bt_Delete);
			bt_Delete.Click += bt_Delete_Click;


			tv_Date = FindViewById<TextView>(Resource.Id.tv_Date);
			tv_Content = FindViewById<TextView>(Resource.Id.tv_Content);

			alert1 = new AlertDialog.Builder(this);
			alert1.SetTitle("Notice");
			alert1.SetCancelable(false);
			alert1.SetNegativeButton("Cancel", delegate { CancelMessage(); });
			alert1.SetPositiveButton("Yes", delegate { FinishMessage(); });
			alert1.SetMessage("Are you sure to delete this message");

			this.LoadInbox();

			TrackingHelper.SendTracking("Open Inbox Message");


		}

		private void CancelMessage()
		{

		}

		private void FinishMessage()
		{

			//delete in Local Database
			DAL.deleteInboxItem(this.item, Settings.PathDatabase);

			//Update RCS
			this.sendbackRCS("D");

			OnBackPressed();


		}

		private void bt_Delete_Click(object sender, EventArgs e)
		{
			alert1.Show();
		}

		private void LoadInbox() { 
			
			var MessageNo = Intent.GetStringExtra("MessageNo");

			var InboxOldList = DAL.GetAll(Settings.PathDatabase);

			var inbox = DAL.GetByMessageNo(Settings.PathDatabase, MessageNo);

			if (inbox != null) {
				this.item = inbox.FirstOrDefault();
				this.tv_Date.Text = this.item.Date;
				this.tv_Content.Text = this.item.MessagePathText;
				//Update Status of this item
				this.updateItem();
				//Send Status of this item to RCS
				this.sendbackRCS("R");
			}
		}

		private void sendbackRCS(string action)
		{

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;

			var url2 = url + "/Api/UpdateInboxItemMessage";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					MessageNo = this.item.MessageNo,
					Action = action
				}
			};

			try
			{
				var ObjectReturn2 = new JsonReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);



				if (string.IsNullOrEmpty(results))
				{
                    AndHUD.Shared.Dismiss();
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

					AndHUD.Shared.Dismiss();

					if (ObjectReturn2.IsSuccess)
					{
					}
					else
					{
						this.RunOnUiThread(() => alert = new Alert(this, "Error", ObjectReturn2.Errors[0].ErrorMessage));
						this.RunOnUiThread(() => alert.Show());
					}
				}
			}
			catch (Exception ee)
			{
				AndHUD.Shared.Dismiss();
			}

		}

		private void updateItem() {
				if (this.item.Status == "Unread")
				{
					this.item.Status = "Read";
					DAL.updateInboxItem(this.item, Settings.PathDatabase);
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

