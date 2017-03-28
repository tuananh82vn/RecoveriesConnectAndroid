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
using System.IO;
using System.Net;
using Com.Joanzapata.Pdfview;
using Com.Joanzapata.Pdfview.Listener;

namespace RecoveriesConnect
{
	[Activity(Label = "InboxDetailLetter", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class InboxDetailLetterActivity : Activity, IOnPageChangeListener
	{
		Alert alert;

		public Inbox item;

		Button bt_Delete;
		public TextView tv_Date;

		AlertDialog.Builder alert1;

		public Com.Joanzapata.Pdfview.PDFView _webView;

		private string _documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		private string _pdfPath;
		private string _pdfFileName;
		private string _pdfFilePath;

		private WebClient _webClient = new WebClient();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.InboxDetailLetter);

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
			_webView = FindViewById<PDFView>(Resource.Id.wv_Pdf);

			alert1 = new AlertDialog.Builder(this);
			alert1.SetTitle("Notice");
			alert1.SetCancelable(false);
			alert1.SetNegativeButton("Cancel", delegate { CancelMessage(); });
			alert1.SetPositiveButton("Yes", delegate { FinishMessage(); });
			alert1.SetMessage("Are you sure to delete this message");

			this.LoadInboxItem();

			TrackingHelper.SendTracking("Open Inbox Document");


		}

		private void LoadInboxItem() {

			var MessageNo = Intent.GetStringExtra("MessageNo");

			var InboxOldList = DAL.GetAll(Settings.PathDatabase);

			var inbox = DAL.GetByMessageNo(Settings.PathDatabase, MessageNo);

			if (inbox != null) {
				this.item = inbox.FirstOrDefault();
				if (this.item != null)
				{
					tv_Date.Text = this.item.Date;
					if (this.item.Status == "Unread")
					{
						this.UpdateStatusItem();
						this.SendStatusBackRCS("R");
					}
					if (!String.IsNullOrEmpty(this.item.IsLocal) && this.item.IsLocal.Equals("true"))
					{
						this.GetLocalDocumentPath();
					}
					else
					{
						this.GetRemoteDocumentPath();
					}
				}
			}
		}

		private void DownloadPDFDocument(string URL)
		{
			_pdfFileName = Settings.RefNumber+ "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".pdf";
			_pdfPath = _documentsPath + "/PDFView";
			_pdfFilePath = System.IO.Path.Combine(_pdfPath, _pdfFileName);

			// Check if the PDFDirectory Exists
			if (!Directory.Exists(_pdfPath))
			{
				Directory.CreateDirectory(_pdfPath);
			}
			else 
			{
				// Check if the pdf is there, If Yes Delete It. Because we will download the fresh one just in a moment
				if (File.Exists(_pdfFilePath))
				{
					File.Delete(_pdfFilePath);
				}
			}

			// This will be executed when the pdf download is completed
			_webClient.DownloadDataCompleted += OnPDFDownloadCompleted;
			// Lets downlaod the PDF Document
			var url = new Uri(URL);

			_webClient.DownloadDataAsync(url);
		}

		void IOnPageChangeListener.OnPageChanged(int p0, int p1)
		{
			System.Diagnostics.Debug.WriteLine("Change to Page " + p0);
		}

		private void OnPDFDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			// Okay the download's done, Lets now save the data and reload the webview.
			var pdfBytes = e.Result;
			File.WriteAllBytes(_pdfFilePath, pdfBytes);

			if (File.Exists(_pdfFilePath))
			{
				var bytes = File.ReadAllBytes(_pdfFilePath);
			}

			Java.IO.File file = new Java.IO.File(_pdfFilePath);

			this._webView.FromFile(file)
			.DefaultPage(1)
			.OnPageChange(this)
			.EnableDoubletap(true)
			.SwipeVertical(true)
			.Load();

			//Update Item in Local Database
			this.item.IsLocal = "true";
			this.item.FileName1 = _pdfFilePath;
			DAL.updateInboxItem(this.item, Settings.PathDatabase);

			AndHUD.Shared.Dismiss();
		}
				    
		private void GetLocalDocumentPath()
		{
			Java.IO.File file = new Java.IO.File(this.item.FileName1);

			this._webView.FromFile(file)
			.DefaultPage(1)
			.OnPageChange(this)
			.EnableDoubletap(true)
			.SwipeVertical(true)
			.Load();
		}
		private void GetRemoteDocumentPath() {

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;

			var url2 = url + "/Api/GetInboxItemDocument";

			var json2 = new
			{
					ReferenceNumber = Settings.RefNumber,
					DocumentPath = this.item.MessagePathText,
					FileName = ""                   
			};

			try
			{
				var ObjectReturn2 = new JsonReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);



				if (string.IsNullOrEmpty(results))
				{
					this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
					this.RunOnUiThread(() => alert.Show());
					AndHUD.Shared.Dismiss();
				}
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

					if (ObjectReturn2.IsSuccess)
					{
						var documentPath = ObjectReturn2.Errors[0].ErrorMessage;
						DownloadPDFDocument(documentPath);
					}
					else
					{
						this.RunOnUiThread(() => alert = new Alert(this, "Error", ObjectReturn2.Errors[0].ErrorMessage));
						this.RunOnUiThread(() => alert.Show());
						AndHUD.Shared.Dismiss();

					}
				}
			}
			catch (Exception ee)
			{
				AndHUD.Shared.Dismiss();
			}
		}

		private void SendStatusBackRCS(string action)
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

					if (!ObjectReturn2.IsSuccess) { 
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

		private void UpdateStatusItem() {

				if (this.item.Status == "Unread")
				{
					this.item.Status = "Read";
					DAL.updateInboxItem(this.item, Settings.PathDatabase);

				}
		}

		private void bt_Delete_Click(object sender, EventArgs e)
		{
			alert1.Show();
		}

		private void CancelMessage()
		{

		}

		private void FinishMessage()
		{
			//delete in Local Database
			DAL.deleteInboxItem(this.item, Settings.PathDatabase);

			//Delete file in Local Device
			File.Delete(this.item.FileName1);

			//Update RCS
			this.SendStatusBackRCS("D");

			OnBackPressed();
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

