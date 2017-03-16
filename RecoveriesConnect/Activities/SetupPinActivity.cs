using Android.App;
using Android.OS;
using Android.Widget;
using RecoveriesConnect.Helpers;
using Android.Views;
using System;
using Android.Views.InputMethods;
using Android.Content;
using Android.Text;

namespace RecoveriesConnect.Activities
{

	[Activity(Theme = "@style/Theme.ThemeCustomNoActionBar", NoHistory = true)]
    public class SetupPinActivity : Activity, TextView.IOnEditorActionListener
    {
        public LinearLayout view_OnTop;
        public InputMethodManager inputManager;
        public EditText et_Pin;
        public int numberOfPin =0;
        public TextView tv_Pin1;
        public TextView tv_Pin2;
        public TextView tv_Pin3;
        public TextView tv_Pin4;

        public string FirstPin;
        public string SecondPin;

        public bool InputFirstPin = false;
        public bool InputSecondPin = false;

        public bool FinishFirstPin = false;
        public bool FinishSecondPin = false;

        public TextView textView1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SetupPin);

            view_OnTop = FindViewById<LinearLayout>(Resource.Id.linearLayout_onTop);
            view_OnTop.Click += viewOnTopClick;

            et_Pin = FindViewById<EditText>(Resource.Id.et_Pin);

            tv_Pin1 = FindViewById<TextView>(Resource.Id.tv_Pin1);
            tv_Pin2 = FindViewById<TextView>(Resource.Id.tv_Pin2);
            tv_Pin3 = FindViewById<TextView>(Resource.Id.tv_Pin3);
            tv_Pin4 = FindViewById<TextView>(Resource.Id.tv_Pin4);

            textView1 = FindViewById<TextView>(Resource.Id.textView1);
            textView1.Text = Resources.GetString(Resource.String.EnterPinNumber);

            et_Pin.TextChanged += InputSearchOnTextChanged;
            inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            this.ShowKeyboard(et_Pin);

            this.InputFirstPin = true;
        }

        private void InputSearchOnTextChanged(object sender, TextChangedEventArgs args)
        {
            numberOfPin = et_Pin.Text.Length;

            if (this.InputFirstPin && !this.FinishFirstPin )
            {
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

                    this.FirstPin = this.et_Pin.Text;
                    this.et_Pin.Text = "";

                    tv_Pin1.Text = "";
                    tv_Pin2.Text = "";
                    tv_Pin3.Text = "";
                    tv_Pin4.Text = "";

                    this.FinishFirstPin = true;
                    this.InputFirstPin = false;
                    this.InputSecondPin = true;

                    textView1.Text = Resources.GetString(Resource.String.ReEnterPinNumber);

                }
            }


            if(this.InputSecondPin && !this.FinishSecondPin)
            {
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

                    this.FinishSecondPin = true;
                    this.SecondPin = this.et_Pin.Text;
                    this.et_Pin.Text = "";

                    if(this.FinishFirstPin && this.FinishSecondPin)
                    {
                        if (this.FirstPin.Equals(this.SecondPin))
                        {
							TrackingHelper.SendTracking("Setup Pin");

							this.HideKeyboard(et_Pin);
                            Settings.PinNumber = this.FirstPin;
                            Settings.IsAlreadySetupPin = true;
                            StartActivity(typeof(LoginWaitingActivity));
							this.Finish();
                        }
                        else
                        {
                            tv_Pin1.Text = "";
                            tv_Pin2.Text = "";
                            tv_Pin3.Text = "";
                            tv_Pin4.Text = "";
                            var alert = new Alert(this, "Error", Resources.GetString(Resource.String.NotMatchPinNumber));
                            alert.Show();
                            this.FirstPin = "";
                            this.SecondPin = "";
                            this.InputFirstPin = true;
                            this.InputSecondPin = false;
                            this.FinishFirstPin = false;
                            this.FinishSecondPin = false;
                            textView1.Text = Resources.GetString(Resource.String.EnterPinNumber);
                            this.ShowKeyboard(et_Pin);

                        }
                    }

                }
            }

          
            //Console.WriteLine(et_Pin.Text);
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            return true;
        }

        public void viewOnTopClick(object sender, EventArgs e)
        {
            this.ShowKeyboard(et_Pin);
        }

        public void ShowKeyboard(EditText pView)
        {
            pView.Focusable = true;
            pView.FocusableInTouchMode = true;
            pView.RequestFocus();

            InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        public void HideKeyboard(EditText pView)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
        }
    }
}