using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using System;
using Android.Views.InputMethods;
using Android.Text;
using Android.Gms.Common;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "Login", LaunchMode = LaunchMode.SingleTop, NoHistory = true, Theme = "@style/Theme.ThemeCustomNoActionBar")]
    public class LoginActivity : Activity
    {

        public LinearLayout view_OnTop;
        public EditText et_Pin;
        public InputMethodManager inputManager;
        public int numberOfPin = 0;

        public TextView tv_Pin1;
        public TextView tv_Pin2;
        public TextView tv_Pin3;
        public TextView tv_Pin4;
        public TextView textView1;

        public Button bt_reset;


		AlertDialog.Builder alert1;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            view_OnTop = FindViewById<LinearLayout>(Resource.Id.linearLayout_onTop);
            view_OnTop.Click += viewOnTopClick;


            bt_reset = FindViewById<Button>(Resource.Id.buttonReset);
            bt_reset.Click += Bt_reset_Click;

            et_Pin = FindViewById<EditText>(Resource.Id.et_Pin);
            et_Pin.TextChanged += InputSearchOnTextChanged;

            tv_Pin1 = FindViewById<TextView>(Resource.Id.tv_Pin1);
            tv_Pin2 = FindViewById<TextView>(Resource.Id.tv_Pin2);
            tv_Pin3 = FindViewById<TextView>(Resource.Id.tv_Pin3);
            tv_Pin4 = FindViewById<TextView>(Resource.Id.tv_Pin4);

            textView1 = FindViewById<TextView>(Resource.Id.textView1);
            textView1.Text = Resources.GetString(Resource.String.EnterPinNumber);

            inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
			Keyboard.ShowKeyboard(this, et_Pin);

			alert1 = new AlertDialog.Builder(this);
			alert1.SetTitle("Notice");
			alert1.SetCancelable(false);
			alert1.SetNegativeButton("Cancel", delegate { CancelMessage(); });
			alert1.SetPositiveButton("Yes", delegate { FinishMessage(); });
			alert1.SetMessage(Resources.GetString(Resource.String.ResetMessage));

			// Create your application here

			if (IsPlayServicesAvailable()) { 
				var intent = new Intent(this, typeof(RegistrationIntentService));
				StartService(intent);
			}
        }

		public bool IsPlayServicesAvailable()
		{
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (resultCode != ConnectionResult.Success)
			{
				if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
				{
					Toast.MakeText(this, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Short).Show();
				}
				else
				{
					Toast.MakeText(this, "Sorry, this device is not supported", ToastLength.Short).Show();
				}
				return false;
			}
			else
			{
				//Toast.MakeText(this, "Google Play Services is available.", ToastLength.Short).Show();
				return true;
			}
		}

        private void Bt_reset_Click(object sender, EventArgs e)
        {
            alert1.Show();
        }

        private void CancelMessage()
        {

		}

        private void FinishMessage()
        {

			Keyboard.HideKeyboard(this, et_Pin);

			this.RunOnUiThread(() => alert1.Dispose());

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			//Delete Inbox database
			Settings.PinNumber = "";
			Settings.DebtorCodeSelected = "";
			Settings.MaxNoPay = 0;
			Settings.TotalPaid = 0;
			Settings.IsAlreadySetupPin = false;
			Settings.TotalOverDue = 0;
			Settings.WeeklyAmount = 0;
			Settings.MonthlyAmount = 0;
			Settings.FortnightAmount = 0;
			Settings.TotalOutstanding = 0;

			Settings.IsExistingArrangement = false;
			Settings.IsExistingArrangementDD = false;
			Settings.IsExistingArrangementCC = false;

			Settings.IsCoBorrowerSelected = false;
			Settings.IsArrangementUnderThisDebtor = false;

			Settings.RefNumber = "";
			Settings.IsCoBorrowers = false;
			Settings.ArrangementDebtor = "";
			Settings.ThreePartDurationDays = 0;
			Settings.IsAllowMonthlyInstallment = false;

			Settings.FirstAmountOfInstallment = 0;
			Settings.NextPaymentInstallment = 0;

			Settings.MakePaymentOtherAmount = false;
			Settings.MakePaymentInstallment = false;
			Settings.MakePaymentIn3Part = false;
			Settings.MakePaymentInFull = false;

			DAL.DeleteAll(Settings.PathDatabase);

			Intent intent = new Intent(this, typeof(SetupActivity));
			StartActivity(intent);

			AndHUD.Shared.Dismiss();

			this.Finish();

		}
        private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
        {
            numberOfPin = et_Pin.Text.Length;


            if (numberOfPin == 0)
            {
                tv_Pin1.Text = "";
                tv_Pin2.Text = "";
                tv_Pin3.Text = "";
                tv_Pin4.Text = "";
            }
            else if (numberOfPin == 1)
            {
                tv_Pin1.Text = "*";
                tv_Pin2.Text = "";
                tv_Pin3.Text = "";
                tv_Pin4.Text = "";
            }
            else if (numberOfPin == 2)
            {
                tv_Pin1.Text = "*";
                tv_Pin2.Text = "*";
                tv_Pin3.Text = "";
                tv_Pin4.Text = "";
            }
            else if (numberOfPin == 3)
            {
                tv_Pin1.Text = "*";
                tv_Pin2.Text = "*";
                tv_Pin3.Text = "*";
                tv_Pin4.Text = "";
            }
            else if (numberOfPin == 4)
            {

                tv_Pin1.Text = "*";
                tv_Pin2.Text = "*";
                tv_Pin3.Text = "*";
                tv_Pin4.Text = "*";

                //Compare Pin
                if (Settings.PinNumber.Equals(this.et_Pin.Text))
                {

					TrackingHelper.SendTracking("Login");

					Keyboard.HideKeyboard(this,et_Pin);

                    StartActivity(typeof(LoginWaitingActivity));

					this.Finish();
                }
                else
                {
                    tv_Pin1.Text = "";
                    tv_Pin2.Text = "";
                    tv_Pin3.Text = "";
                    tv_Pin4.Text = "";
                    et_Pin.Text = "";

                    var alert = new Alert(this, "Error", Resources.GetString(Resource.String.NotMatchPinNumber));
                    alert.Show();
                }
            }
        }

        public void viewOnTopClick(object sender, EventArgs e)
        {
            Keyboard.ShowKeyboard(this,et_Pin);
        }

        
    }
}