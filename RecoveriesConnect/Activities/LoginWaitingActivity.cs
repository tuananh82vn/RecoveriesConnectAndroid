using System;
using Android.App;
using Android.OS;
using Android.Widget;
using RecoveriesConnect.Helpers;
using Android.Views.Animations;
using RecoveriesConnect.Models.Api;
using System.Threading;

namespace RecoveriesConnect.Activities
{
	[Activity(Theme = "@style/Theme.ThemeCustomNoActionBar", NoHistory = true)]
    public class LoginWaitingActivity : Activity
    {
        public System.Timers.Timer _backgroundtimer;

        public TextView tv_Message;
        public ImageView imageLogo;
        public Animation rotateAboutCenterAnimation;


        int count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.LoginWaiting);

            tv_Message = FindViewById<TextView>(Resource.Id.tv_Message);
            imageLogo = FindViewById<ImageView>(Resource.Id.im_Logo);
            //var bar = new ProgressBarHelper(this);
            //bar.show();
            rotateAboutCenterAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_center);

            ThreadPool.QueueUserWorkItem(o => KeepChecking());

            ThreadPool.QueueUserWorkItem(o => GetDebtorInfo());

        }
        private void KeepChecking()
        {
            _backgroundtimer = new System.Timers.Timer();
            //Trigger event every second
            _backgroundtimer.Interval = 1000;
            _backgroundtimer.Elapsed += OnTimeBackgrounddEvent;
            _backgroundtimer.Start();
        }

        private void OnTimeBackgrounddEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            //RunOnUiThread(() => imageLogo.StartAnimation(rotateAboutCenterAnimation));
             count++;
            if (count == 1)
            {
                RunOnUiThread(() => tv_Message.Text = Resources.GetString(Resource.String.Waiting1));
            }
            else
            if (count == 2)
            {
                RunOnUiThread(() => tv_Message.Text = Resources.GetString(Resource.String.Waiting2));
            }
            else
            if (count == 3)
            {
                RunOnUiThread(() => tv_Message.Text = Resources.GetString(Resource.String.Waiting3));
                count = 0;
            }

        }

        private void GetDebtorInfo()
        {
            RunOnUiThread(() => imageLogo.StartAnimation(rotateAboutCenterAnimation));

            string url = Settings.InstanceURL;

            //Get Debtor Info
            var url2 = url + "/Api/GetDebtorInfo";

            var json2 = new
            {
                Item = new
                {
                    ReferenceNumber = Settings.RefNumber
                }
            };

            try
            {
                var debtor = new DebtorInfoModel();

				string results = ConnectWebAPI.Request(url2, json2);


                debtor = Newtonsoft.Json.JsonConvert.DeserializeObject<DebtorInfoModel>(results);

                if (debtor.IsSuccess)
                {
                    Settings.TotalOutstanding = debtor.TotalOutstanding;
                    Settings.NextPaymentInstallment = Decimal.Parse(debtor.NextPaymentInstallment.ToString());
                    Settings.IsExistingArrangement = debtor.IsExistingArrangement;
                    Settings.IsExistingArrangementCC = debtor.IsExistingArrangementCC;
                    Settings.IsExistingArrangementDD = debtor.IsExistingArrangementDD;
                    Settings.IsAllowMonthlyInstallment = debtor.IsAllowMonthlyInstallment;
                    Settings.WeeklyAmount = debtor.MinimumWeeklyOutstanding;
                    Settings.MonthlyAmount = debtor.MinimumMonthlyOutstanding;
                    Settings.FortnightAmount = debtor.MinimumFortnightlyOutstanding;
                    Settings.ClientAccountNumber = debtor.ClientAccNo;
                    Settings.OurClient = debtor.ClientName;

                    if (debtor.MaxNoPay > 3)
                    {
                        Settings.MaxNoPay = 3;
                    }
                    else
                    {
                        Settings.MaxNoPay = debtor.MaxNoPay;
                    }

                    if (debtor.client.ThreePartDateDurationDays != 0)
                    {
                        Settings.ThreePartDurationDays = debtor.client.ThreePartDateDurationDays;
                    }

                    Settings.ThreePartMaxDaysBetweenPayments = debtor.client.ThreePartMaxDaysBetweenPayments;


                    _backgroundtimer.Stop();

                    StartActivity(typeof(HomeActivity));

                    this.Finish();

                }
            }
            catch (Exception ee)
            {
            }
        }
    }
}