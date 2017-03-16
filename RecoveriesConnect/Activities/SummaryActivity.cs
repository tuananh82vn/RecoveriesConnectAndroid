using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using Android.Graphics.Drawables;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Adapter;
using RecoveriesConnect.Fragment;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using RecoveriesConnect.Models.Api;
using Android.Views.InputMethods;
using System.Threading;
using System.Net;
using System.IO;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
    [Activity(Label = "Summary", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
    public class SummaryActivity : Activity
    {
        Alert alert;

        public TextView tv_TransactionDescription;
        public TextView tv_ReceiptNumber;
        public TextView tv_Amount;
        public TextView tv_Time;
        public TextView tv_Date;
        public TextView tv_Name;
        public TextView tv_Message;

		public TextView tv_ReceiptLabel;

        public Button bt_Email;
        public Button bt_Finish;
		public Button bt_Inbox;


		public int paymentType = 0;
        public int paymentMethod = 0;

        public int paymentId = 0;
        public string ClientName = "";
        public int FirstDebtorPaymentInstallmentId = 0;

        public EditText txtEmail;
        public bool isOpen = false;

        public AlertDialog alertDialog;
        public AlertDialog.Builder builder;

		public LinearLayout ll_receipt;
		public LinearLayout ll_Body;

		private string _documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		private string _pdfPath;
		private string _pdfFileName;
		private string _pdfFilePath;

		private WebClient _webClient = new WebClient();
		private string title = "";

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);


			SetContentView(Resource.Layout.Summary);

			//**************************************************//

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

			ActionBar.SetDisplayHomeAsUpEnabled(false);
			ActionBar.SetHomeButtonEnabled(false);

			LinearLayout lLayout = new LinearLayout(this);
			lLayout.SetGravity(GravityFlags.CenterVertical);
			LinearLayout.LayoutParams textViewParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			textViewParameters.RightMargin = (int)(30 * this.Resources.DisplayMetrics.Density);

			TextView myTitle = new TextView(this);


			//**************************************************//

			// Create your application here

			ll_receipt = FindViewById<LinearLayout>(Resource.Id.ll_receipt);
			ll_Body = FindViewById<LinearLayout>(Resource.Id.ll_Body);


			tv_TransactionDescription = this.FindViewById<TextView>(Resource.Id.tv_TransactionDescription);
            tv_ReceiptNumber = this.FindViewById<TextView>(Resource.Id.tv_ReceiptNumber);
            tv_Amount = this.FindViewById<TextView>(Resource.Id.tv_Amount);
            tv_Time = this.FindViewById<TextView>(Resource.Id.tv_Time);
            tv_Date = this.FindViewById<TextView>(Resource.Id.tv_Date);
            tv_Name = this.FindViewById<TextView>(Resource.Id.tv_Name);
            tv_Message = this.FindViewById<TextView>(Resource.Id.tv_Message);
			tv_ReceiptLabel = this.FindViewById<TextView>(Resource.Id.tv_ReceiptLabel);



			//Load data from previous activity
			tv_TransactionDescription.Text= Intent.GetStringExtra("tv_TransactionDescription") ?? "";
            tv_ReceiptNumber.Text = Intent.GetStringExtra("tv_ReceiptNumber") ?? "";
            tv_Amount.Text = Intent.GetStringExtra("tv_Amount") ?? "";
            tv_Time.Text = Intent.GetStringExtra("tv_Time") ?? "";
            tv_Date.Text = Intent.GetStringExtra("tv_Date") ?? "";
            tv_Name.Text = Intent.GetStringExtra("tv_Name") ?? "";

			if (Settings.IsFuturePayment)
			{
				tv_Message.Text = "Your payments will be processed according to the Future Payments schedule.";
				ll_Body.Visibility = ViewStates.Invisible;
			}
			else
			{
				tv_Message.Text = "Your payment has been processed against your account with Reference Number " + Settings.RefNumber + ". Please be aware, payments will appear on your statement as payment to 'Recoveriescorp'";
				paymentType = Intent.GetIntExtra("PaymentType", 0);
				paymentMethod = Intent.GetIntExtra("PaymentMethod", 0);
				if (paymentMethod == 1)
				{
					//ActionBar.SetTitle(Resource.String.Receipt);
					myTitle.Text = "Receipt";

					this.title = "Receipt";
				}
				else
				{
					//ActionBar.SetTitle(Resource.String.PaymentSummary);
					myTitle.Text = "Payment Summary";

					this.ll_receipt.Visibility = ViewStates.Invisible;
					tv_ReceiptLabel.Text = "Payment Summary";
					this.title = "Payment summary";
				}

				paymentId = Intent.GetIntExtra("PaymentId", 0);
				FirstDebtorPaymentInstallmentId = Intent.GetIntExtra("FirstDebtorPaymentInstallmentId", 0);
				ClientName = Intent.GetStringExtra("ClientName") ?? "";


				bt_Email = this.FindViewById<Button>(Resource.Id.bt_Email);
				bt_Email.Click += Bt_Email_Click;

				txtEmail = new EditText(this);
				builder = new AlertDialog.Builder(this);
				txtEmail.InputType = Android.Text.InputTypes.TextVariationWebEmailAddress;
			}

            bt_Finish = this.FindViewById<Button>(Resource.Id.bt_Finish);
            bt_Finish.Click += Bt_Finish_Click;

			bt_Inbox = this.FindViewById<Button>(Resource.Id.bt_Inbox);
			bt_Inbox.Click += bt_Inbox_Click;


			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);
        }



        private void Bt_Finish_Click(object sender, EventArgs e)
        {
            Intent Intent = new Intent(this, typeof(HomeActivity));
            Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(Intent);
        }

        private void Bt_Email_Click(object sender, EventArgs e)
        {
            alertDialog = builder.Create();
            alertDialog.SetTitle("Enter your email");
            if(txtEmail.Parent != null)
            {
                ((ViewGroup)txtEmail.Parent).RemoveView(txtEmail);
            }
            alertDialog.SetView(txtEmail);
			alertDialog.SetIcon(Resource.Color.transparent);

            alertDialog.SetButton("Email", (s, ev) =>
            {
                ThreadPool.QueueUserWorkItem(o => Email(txtEmail.Text));
				Keyboard.HideKeyboard(this, this.txtEmail);
			});

            alertDialog.SetButton3("Cancel", (s, ev) =>
            {
                alertDialog.Hide();
				Keyboard.HideKeyboard(this, this.txtEmail);
            });

            alertDialog.Show();
			Keyboard.ShowKeyboard(this, txtEmail);
        }

        public void Email(string email)
        {
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			//Send Email
			if (Validation.isValidEmail(email))
            {
                string url = Settings.InstanceURL;
                var url2 = url + "/Api/EmailReceipt";

                var json2 = new
                {
                    Item = new
                    {
                        Name = this.tv_Name.Text,
                        CurrentPaymentId = paymentId,
                        ClientName = ClientName,
                        PaymentType = paymentType,
                        EmailAddress = email,
                        PaymentMethod = paymentMethod
                    }
                };
                try
                {

                    var ObjectReturn = new JsonReturnModel();

                    string results = ConnectWebAPI.Request(url2, json2);

                    if (string.IsNullOrEmpty(results))
                    {

						this.RunOnUiThread(() =>
                        {
							AndHUD.Shared.Dismiss();
                            alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
                            alert.Show();

						});


					}
                    else
                    {
                        ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

						AndHUD.Shared.Dismiss();

                        if (ObjectReturn.IsSuccess)
                        {
                            this.RunOnUiThread(() =>
                            {
								alert = new Alert(this, "Notice", this.title +" has been sent to " + email);
                                alert.Show();
								Keyboard.HideKeyboard(this, this.txtEmail);

							});


						}
                        else
                        {
                            this.RunOnUiThread(() =>
                            {
                                alert = new Alert(this, "Error", "Error");
                                alert.Show();
								Keyboard.HideKeyboard(this, this.txtEmail);

							});
                        }
                    }
                }
                catch (Exception ee)
                {
					AndHUD.Shared.Dismiss();
					Keyboard.HideKeyboard(this, this.txtEmail);

				}
            }
            else
            {
                this.RunOnUiThread(() =>
                {
					AndHUD.Shared.Dismiss();
                    alert = new Alert(this, "Error", "Email is not valid");
                    alert.Show();
					Keyboard.HideKeyboard(this, this.txtEmail);


				});
            }
        }

		private void bt_Inbox_Click(object sender, EventArgs e)
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/SaveInbox";

			var json2 = new
			{
				Item = new
				{
					Name = this.tv_Name.Text,
					CurrentPaymentId = paymentId,
					ClientName = ClientName,
					PaymentType = paymentType,
					EmailAddress = "",
					PaymentMethod = paymentMethod
				}
			};
			try
			{

				var ObjectReturn = new JsonReturnModel();

				string results = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results))
				{
					this.RunOnUiThread(() =>
					{
						AndHUD.Shared.Dismiss();
						alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer));
						alert.Show();
					});
				}
				else
				{
					ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);


					if (ObjectReturn.IsSuccess)
					{
						var filePath = ObjectReturn.Errors[0].ErrorMessage;
						this.DownloadPDFDocument(filePath);
						this.RunOnUiThread(() =>
						{
							AndHUD.Shared.Dismiss();
							alert = new Alert(this, "Notice", this.title +" has been saved into Inbox");
							alert.Show();
						});
					}
					else
					{
						this.RunOnUiThread(() =>
						{
							AndHUD.Shared.Dismiss();
							alert = new Alert(this, "Error", "Error");
							alert.Show();
						});
					}
				}
			}
			catch (Exception ee)
			{
				AndHUD.Shared.Dismiss();
			}
		}

        public override void OnBackPressed()
        {
           //disable on back press
        }

		private void DownloadPDFDocument(string URL)
		{
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			_pdfFileName = Settings.RefNumber + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".pdf";
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

		private void OnPDFDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
		{

			// Okay the download's done, Lets now save the data and reload the webview.
			var pdfBytes = e.Result;
			File.WriteAllBytes(_pdfFilePath, pdfBytes);

			if (File.Exists(_pdfFilePath))
			{
				var bytes = File.ReadAllBytes(_pdfFilePath);
			}

			Inbox item = new Inbox();

			//Update Item in Local Database
			item.Date = DateTime.Now.ToShortDateString();
			if (paymentMethod == 1)
			{
				item.Type = "R";
			}
			else
			{
				item.Type = "P";
			}
			item.IsLocal = "true";
			item.Status = "Unread";
			item.FileName1 = _pdfFilePath;
			item.MessageNo = DateTime.Now.Ticks.ToString();

			DAL.insertInboxItem(item, Settings.PathDatabase);

			AndHUD.Shared.Dismiss();


		}
    }
}