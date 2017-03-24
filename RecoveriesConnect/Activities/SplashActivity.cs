using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Threading.Tasks;
using RecoveriesConnect.Helpers;
using Android.Views;
using System;
using SQLite;

namespace RecoveriesConnect.Activities
{

    [Activity(Theme = "@style/Theme.ThemeCustomNoActionBar", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        public System.Timers.Timer _backgroundtimer;
        public ImageView imageLogo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Splash);

           // if (NetworkHelper.DetectNetwork())
            //{
            Init();

			Keyboard.HideSoftKeyboard(this);
           // }
           // else
           // {
            //    Toast.MakeText(this, "No Connection ...", ToastLength.Short).Show();
            //    KeepChecking();
            //}
            // Create your application here
        }

		public bool TableExists<T>(SQLiteConnection connection)
		{
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand(cmdText, typeof(T).Name);
			return cmd.ExecuteScalar<string>() != null;
		}

        private void Init()
        {
            // Simulate a long loading process on app startup.
            Task<bool>.Run(() => {

				string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

				Settings.PathDatabase = System.IO.Path.Combine(folder, "inbox.db");

				var conn = new SQLiteConnection(Settings.PathDatabase);

				if (this.TableExists<Inbox>(conn))
				{
					Console.WriteLine("table existed");
				}
				else
				{ 
					conn.CreateTable<Inbox>();
				}

                Thread.Sleep(2000);
                //Settings.IsAlreadySetupPin = false;

                if (!Settings.IsAlreadySetupPin)
                {
                        StartActivity(typeof(SetupActivity));
                }
                else
                {
                    if (!Settings.IsAgreePolicy)
                    {
                        StartActivity(typeof(PrivacyPolicyActivity));

                    }
                    else
                        StartActivity(typeof(LoginActivity));
                }
                this.Finish();
            });
        }

        private void KeepChecking()
        {
            _backgroundtimer = new System.Timers.Timer();
            //Trigger event every second
            _backgroundtimer.Interval = 6000;
            _backgroundtimer.Elapsed += OnTimeBackgrounddEvent;
            _backgroundtimer.Start();
        }

        private void OnTimeBackgrounddEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (NetworkHelper.DetectNetwork())
            {
                _backgroundtimer.Stop();
                Init();
            }
            else
            {
                RunOnUiThread(() => Toast.MakeText(this, "No Connection ...", ToastLength.Long).Show());
            }
        }
    }
}