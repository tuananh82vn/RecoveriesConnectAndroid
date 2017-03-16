using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Models.Api;
using System.Threading;
using RecoveriesConnect.Activities;
using AndroidHUD;

namespace RecoveriesConnect.Fragment
{
	[Activity(Theme = "@style/Theme.ThemeCustomNoActionBar", NoHistory = true)]
    public class Setup_Page2 : Android.Support.V4.App.Fragment
    {
        EditText et_RefNumber;
        EditText et_NetCode;
        TextView textView2;
        Button buttonNetCode;
        Button buttonContinue;
        List<CoDebtorModel> codebtor;
        string selectedDebtor = "";
        Alert alert;
        public Setup_Page2()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Setup_Page2, container, false);

            buttonNetCode = view.FindViewById<Button>(Resource.Id.buttonNetcode);
            buttonNetCode.Click += buttonNetcodeClick;

            buttonContinue = view.FindViewById<Button>(Resource.Id.buttonContinue);
            buttonContinue.Click += buttonContinueClick;
            buttonContinue.Visibility = ViewStates.Invisible;

            textView2 = view.FindViewById<TextView>(Resource.Id.textView2);
            et_NetCode = view.FindViewById<EditText>(Resource.Id.et_NetCode);

            et_NetCode.Visibility = ViewStates.Invisible;
            textView2.Visibility = ViewStates.Invisible;

            et_RefNumber = view.FindViewById<EditText>(Resource.Id.editTextRefNumber);
            //Andy testing
            //et_RefNumber.Text = "706170060";

			return view;
        }

        


        public void buttonNetcodeClick(object sender, EventArgs e)
        {
			if (et_RefNumber.Text.Length > 9 || et_RefNumber.Text.Length < 9)
			{
				alert = new Alert(this.Activity, "Error", Resources.GetString(Resource.String.RefNumberInvalid));
				alert.Show();
				Keyboard.ShowKeyboard(this.Activity, et_RefNumber);
			}
			else
			{
				ThreadPool.QueueUserWorkItem(o => 
				{
					bool results = this.GetDebtorCode();
					if(results)
						GetNetCode();
				});
			}
        }

		private bool GetDebtorCode() {

			AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;

			//Get Debtor Info
			var url2 = url + "/Api/GetDebtorInfo";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = et_RefNumber.Text
				}
			};


			try
			{
				var ObjectReturn2 = new DebtorInfoModel();

				string results = ConnectWebAPI.Request(url2, json2);

				//int tryAgain = 0;
				//if (results.Equals("TryAgain"))
				//{
				//	tryAgain++;
				//	if (tryAgain > 3)
				//	{
				//		this.Activity.RunOnUiThread(() =>
				//		{
				//			alert = new Alert(this.Activity, "Error", "No internet or Server not found. Please close the app and try again.");
				//			alert.Show();
				//			return;
				//		});
				//	}
				//	this.GetDebtorCode();
				//}

				if (string.IsNullOrEmpty(results))
				{
					this.Activity.RunOnUiThread(() =>
					{
						AndHUD.Shared.Dismiss();
						alert = new Alert(this.Activity, "Error", Resources.GetString(Resource.String.NoServer));
						alert.Show();
					});
					return false;
				}
				else
				{
					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<DebtorInfoModel>(results);

					AndHUD.Shared.Dismiss();

					if (ObjectReturn2.IsSuccess)
					{
						if (ObjectReturn2.IsCoBorrowers)
						{
							codebtor = new List<CoDebtorModel>();
							for (int i = 0; i < ObjectReturn2.CoDebtorCode.Count; i++)
							{
								var DebtorCode = ObjectReturn2.CoDebtorCode[i];
								var FullName = ObjectReturn2.CoFirstName[i] + " " + ObjectReturn2.CoLastName[i];
								FullName.Trim();
								var Mobile = ObjectReturn2.CoMobileNumbers[i];
								var MarkMobile = "";
								if (string.IsNullOrEmpty(Mobile))
								{
									Mobile = "No Number";
									MarkMobile = "No Number";
								}
								else
								{
									MarkMobile = "XXXXX" + Mobile.Substring(Mobile.Count() - 3, 3);
								}
								var tempDebtor = new CoDebtorModel(DebtorCode, FullName, Mobile, MarkMobile);
								this.codebtor.Add(tempDebtor);
							}
						}

						selectedDebtor = ObjectReturn2.DebtorCode;
						Settings.IsCoBorrowers = ObjectReturn2.IsCoBorrowers;
						Settings.ArrangementDebtor = ObjectReturn2.ArrangementDebtor;
						Settings.RefNumber = this.et_RefNumber.Text;

						if (Settings.IsCoBorrowers)
						{
							Intent Intent = new Intent(this.Activity, typeof(SelectDebtorActivity));

							Intent.PutParcelableArrayListExtra("codebtor", this.codebtor.ToArray());

							Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

							StartActivity(Intent);

							return false;

						}
						else
						{
							return true;
						}
					}
					else
					{
						this.Activity.RunOnUiThread(() =>
					   	{
						   alert = new Alert(this.Activity, "Error", "The ref number is not valid");
						   alert.Show();
				   		});
						return false;

					}
				}
			}
			catch (Exception ex)
			{
				AndHUD.Shared.Dismiss();
				return false;
			}
		}

        private void GetNetCode()
        {
			AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;

			string url2 = url + "/Api/GetNetCode";

            var json = new
            {
                Item = new
                {
                    ReferenceNumber = et_RefNumber.Text,
                }
            };

            var ObjectReturn = new JsonReturnModel();
            try
            {
                string results = ConnectWebAPI.Request(url2, json);

				//int tryAgain = 0;
				//if (results.Equals("TryAgain")){
				//	tryAgain++;
				//	if (tryAgain > 3) {
				//		this.Activity.RunOnUiThread(() =>
				//		{
				//			alert = new Alert(this.Activity, "Error", "No internet or Server not found. Please close the app and try again.");
				//			alert.Show();
				//			return;
				//		});
				//	}
				//	this.GetNetCode();
				//}

				if (string.IsNullOrEmpty(results))
				{
					this.Activity.RunOnUiThread(() =>
					{
						AndHUD.Shared.Dismiss();
						alert = new Alert(this.Activity, "Error", Resources.GetString(Resource.String.NoServer));
						alert.Show();
					});
				}
				else
				{

					ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results);

					AndHUD.Shared.Dismiss();

					if (ObjectReturn.IsSuccess)
					{

						this.Activity.RunOnUiThread(() =>
						{
							alert = new Alert(this.Activity, "Notice", ObjectReturn.Errors[0].ErrorMessage);
							alert.Show();
							et_NetCode.Visibility = ViewStates.Visible;
							textView2.Visibility = ViewStates.Visible;
							buttonContinue.Visibility = ViewStates.Visible;
							buttonNetCode.Text = Resources.GetString(Resource.String.GetNetCode);
							Keyboard.ShowKeyboard(this.Activity, et_NetCode);
						});

					}
					else
					{
						this.Activity.RunOnUiThread(() =>
						{
							alert = new Alert(this.Activity, "Error", ObjectReturn.Errors[0].ErrorMessage);
							alert.Show();
						});


					}
				}

            }
            catch (Exception ex)
            {
				AndHUD.Shared.Dismiss();
            }
        }

		public void buttonContinueClick(object sender, EventArgs e)
		{
			AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

			if (et_NetCode.Text.Length > 6 || et_NetCode.Text.Length < 6)
			{
				AndHUD.Shared.Dismiss();

				alert = new Alert(this.Activity, "Error", Resources.GetString(Resource.String.NetCodeInvalid));
				//this.ShowKeyboard(et_NetCode);
				alert.Show();
			}
			else
			{

				string url = Settings.InstanceURL;

				var url1 = url + "/Api/VerifyNetCode";

				var json = new
				{
					Item = new
					{
						ReferenceNumber = et_RefNumber.Text,
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
						Keyboard.HideKeyboard(this.Activity, et_NetCode);

						Settings.DebtorCodeSelected = selectedDebtor;

						Intent intent = new Intent(this.Activity, typeof(SetupPinActivity));
						StartActivity(intent);

						this.Activity.Finish();
					}
					else
					{
						alert = new Alert(this.Activity, "Error", ObjectReturn.Errors[0].ErrorMessage);
						this.Activity.RunOnUiThread(() => alert.Show());
					}
				}
				catch (Exception ex)
				{
					AndHUD.Shared.Dismiss();
				}
			}

			AndHUD.Shared.Dismiss();

		}
    }
}