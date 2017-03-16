using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Adapter;
using System;
using System.Linq;
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "Inbox", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class InboxActivity : Activity
	{
		Alert alert;

		public List<Inbox> InboxOldList;
		public List<Inbox> InboxNewList;
		public List<Inbox> InboxFinalList;

		public ListView inboxListView;

		public InboxAdapter inboxAdapter;
		public int selectedIndex = -1;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.Inbox);

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
			myTitle.Text = "Inbox";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			this.inboxListView = FindViewById<ListView>(Resource.Id.inboxListView);

			GetInboxLocal();

			GetInboxRemote();

			TrackingHelper.SendTracking("Open Inbox");


		}

		protected override void OnRestart()
		{
			base.OnRestart();

			Intent intent = Intent;
			Finish();
			StartActivity(intent);
		}

		//protected override void OnResume()
		//{
		//	base.OnResume();

		//	GetInboxLocal();

		//	GetInboxRemote();
		//}

		void inboxList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			selectedIndex = e.Position;

			if (this.InboxFinalList.ElementAt(this.selectedIndex).Type == "T")
			{

				Intent Intent1 = new Intent(this, typeof(InboxDetailMessageActivity));

				Intent1.PutExtra("MessageNo", this.InboxFinalList.ElementAt(this.selectedIndex).MessageNo);

				StartActivity(Intent1);
			}
			else
			{
				Intent Intent2 = new Intent(this, typeof(InboxDetailLetterActivity));

				Intent2.PutExtra("MessageNo", this.InboxFinalList.ElementAt(this.selectedIndex).MessageNo);

				StartActivity(Intent2);
			}

		}

		private void GetInboxLocal() {
			this.InboxOldList = DAL.GetAll(Settings.PathDatabase);
		}

		private void GetInboxRemote() {

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;

			var url2 = url + "/Api/GetInboxItems";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
				}
			};

			try
			{
				var ObjectReturn2 = new InboxReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);



				if (string.IsNullOrEmpty(results))
				{
					AndHUD.Shared.Dismiss();
					alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
					alert.Show();
				}
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<InboxReturnModel>(results);

					AndHUD.Shared.Dismiss();

					if (ObjectReturn2.IsSuccess)
					{
						this.InboxNewList = ObjectReturn2.InboxList.ToList();
						// Save New inbox item into Local Database
						InsertNewInboxItem();
						//Sort List
						this.InboxFinalList = InboxFinalList.OrderByDescending(o => o.MessageNo).ToList();

						if (this.InboxFinalList.Count == 0)
						{
							this.inboxListView.Clickable = false;
							this.inboxListView.ChoiceMode = ChoiceMode.None;
							this.inboxListView.Selector = Resources.GetDrawable(Android.Resource.Color.Transparent);
						}
						else
						{
							this.inboxListView.ItemClick += inboxList_ItemClick;
						}

						this.inboxAdapter = new InboxAdapter(this, this.InboxFinalList);
						this.inboxListView.Adapter = this.inboxAdapter;
						this.inboxListView.InvalidateViews();

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

		private void InsertNewInboxItem() {
			this.InboxFinalList = this.InboxOldList;

			if (this.InboxNewList.Count > 0) {

				//Find in the old list
				foreach (var item in this.InboxNewList)
				{
					var results = this.InboxOldList.Find(x => x.MessageNo == item.MessageNo);
					if (results == null) {
						//Convert Date Format
						item.Date = DateTime.ParseExact(item.Date, "yyyy/MM/dd", null).ToShortDateString();
						item.Status = "Unread";

						var IsInserted = DAL.insertInboxItem(item, Settings.PathDatabase);
						this.InboxFinalList.Add(item);
					}
				}
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

