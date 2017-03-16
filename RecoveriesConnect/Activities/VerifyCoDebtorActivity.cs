using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Models.Api;
using Android.Content.PM;
using System;
using Android.Views;
using Android.Views.InputMethods;
using System.Threading;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
    [Activity(Label = "VerifyCoDebtor", NoHistory = true, LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.ThemeCustomNoActionBar")]
    public class VerifyCoDebtorActivity : Activity
    {
        EditText et_NetCode;
        TextView textView2;
        Button buttonNetCode;
        Button buttonContinue;
        string selectedDebtor = "";
        Alert alert;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VerifyCoDebtor);

            buttonNetCode = FindViewById<Button>(Resource.Id.buttonNetcode);
            buttonNetCode.Click += buttonNetcodeClick;

            buttonContinue = FindViewById<Button>(Resource.Id.buttonContinue);
            buttonContinue.Click += buttonContinueClick;
            buttonContinue.Visibility = ViewStates.Invisible;

            textView2 = FindViewById<TextView>(Resource.Id.textView2);
            et_NetCode = FindViewById<EditText>(Resource.Id.et_NetCode);

            et_NetCode.Visibility = ViewStates.Invisible;
            textView2.Visibility = ViewStates.Invisible;

            // Create your application here
        }

        public void buttonContinueClick(object sender, EventArgs e)
        {
            if (et_NetCode.Text.Length > 6 || et_NetCode.Text.Length < 6)
            {
                alert = new Alert(this, "Error", Resources.GetString(Resource.String.NetCodeInvalid));
                alert.Show();
            }
            else
            {
				AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

                string url = Settings.InstanceURL;

                var url1 = url + "/Api/VerifyNetCode";

                var json = new
                {
                    Item = new
                    {
                        ReferenceNumber = Settings.RefNumber,
                        Netcode = et_NetCode.Text,
                    }
                };

                var ObjectReturn = new JsonReturnModel();
                try
                {
                    string results = ConnectWebAPI.Request(url1, json);

                    ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

                    if (ObjectReturn.IsSuccess)
                    {
						Keyboard.HideSoftKeyboard(this);

                        Settings.DebtorCodeSelected = selectedDebtor;

                        Intent intent = new Intent(this, typeof(SetupPinActivity));
                        StartActivity(intent);

						AndHUD.Shared.Dismiss();

                    }
                    else
                    {
						AndHUD.Shared.Dismiss();
                        alert = new Alert(this, "Error", ObjectReturn.Errors[0].ErrorMessage);
                        RunOnUiThread(() => alert.Show());
                    }
                }
                catch (Exception ex)
                {
					AndHUD.Shared.Dismiss();
				}
            }

        }

       

        public void buttonNetcodeClick(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o => GetNetCode());
        }

        private void GetNetCode()
        {
			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);
            try
            {

                string url = Settings.InstanceURL;
                url = url + "/Api/GetNetCode";

                var json = new
                {
                    Item = new
                    {
                        ReferenceNumber = Settings.RefNumber,
                    }
                };

                var ObjectReturn = new JsonReturnModel();
                try
                {
                    string results = ConnectWebAPI.Request(url, json);

                    ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

					AndHUD.Shared.Dismiss();

                    if (ObjectReturn.IsSuccess)
                    {
                        this.RunOnUiThread(() =>
                        {
                            alert = new Alert(this, "Notice", ObjectReturn.Errors[0].ErrorMessage);
                            alert.Show();
                            et_NetCode.Visibility = ViewStates.Visible;
                            textView2.Visibility = ViewStates.Visible;
                            buttonContinue.Visibility = ViewStates.Visible;
                            buttonNetCode.Text = Resources.GetString(Resource.String.GetNetCode);
                            Keyboard.ShowKeyboard(this,et_NetCode);
                        });
                    }
                    else
                    {
                        this.RunOnUiThread(() =>
                        {
                            alert = new Alert(this, "Error", ObjectReturn.Errors[0].ErrorMessage);
                            alert.Show();
                        });
                    }

                }
                catch (Exception ex)
                {
					AndHUD.Shared.Dismiss();
                }
            }
            catch (Exception ex)
            {
				AndHUD.Shared.Dismiss();
			}
        }

    }
}
