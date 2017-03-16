using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Adapter;
using RecoveriesConnect.Fragment;
using RecoveriesConnect.Models.Api;
using System;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "Home", LaunchMode = LaunchMode.SingleTop,  Theme = "@style/Theme.Themecustom")]
    public class HomeActivity : FragmentActivity
    {

        private MyActionBarDrawerToggle drawerToggle;
        private string drawerTitle;
        private string title;

        private DrawerLayout drawerLayout;

        private ListView drawerListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home);

            this.title = this.drawerTitle = this.Title;

            this.drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            this.drawerListView = this.FindViewById<ListView>(Resource.Id.left_drawer);


            //Create Adapter for drawer List
            this.drawerListView.Adapter = new MenuListAdapter(this);
            drawerListView.DividerHeight = 0;


            //Set click handler when item is selected
            this.drawerListView.ItemClick += (sender, args) => ListItemClicked(args.Position);

            //Set Drawer Shadow
            this.drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);

            //DrawerToggle is the animation that happens with the indicator next to the actionbar
            this.drawerToggle = new MyActionBarDrawerToggle(this, this.drawerLayout,
                Resource.Drawable.action_menu,
                Resource.String.drawer_open,
                Resource.String.drawer_close);

            //Display the current fragments title and update the options menu
            this.drawerToggle.DrawerClosed += (o, args) => {
                this.ActionBar.Title = this.title;
                this.InvalidateOptionsMenu();
            };

            //Display the drawer title and update the options menu
            this.drawerToggle.DrawerOpened += (o, args) => {
                this.ActionBar.Title = this.drawerTitle;
                this.InvalidateOptionsMenu();
            };

            //Set the drawer lister to be the toggle.
            this.drawerLayout.SetDrawerListener(this.drawerToggle);

            Android.Support.V4.App.Fragment fragment = new HomeFragment(this);

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();


            this.ActionBar.SetDisplayHomeAsUpEnabled(true);
            this.ActionBar.SetHomeButtonEnabled(true);

			//************************************************//
			LinearLayout lLayout = new LinearLayout(this);
			lLayout.SetGravity(GravityFlags.CenterVertical);
			LinearLayout.LayoutParams textViewParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			//textViewParameters.RightMargin = (int)(30 * this.Resources.DisplayMetrics.Density);

			TextView myTitle = new TextView(this);
			myTitle.Text = "Home";
			myTitle.TextSize = 20;

			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle,textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			this.ActionBar.SetCustomView(lLayout, actionbarParams);
			this.ActionBar.SetDisplayShowCustomEnabled(true);
			//************************************************//



			SetPayment.Set("none");
			Settings.Frequency = 0;
			Settings.IsFuturePayment = false;
			SendAppsDetail();

			Keyboard.HideSoftKeyboard(this);


		}

		protected override void OnResume()
		{
			base.OnResume();
			SetPayment.Set("none");
			Settings.Frequency = 0;
			Settings.IsFuturePayment = false;
			Keyboard.HideSoftKeyboard(this);
		}


		private void SendAppsDetail() { 
			
		string url = Settings.InstanceURL;
			var url2 = url + "/Api/SendAppDetails";

			var json2 = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber,
					PinNumber = Settings.PinNumber,
					DeviceToken = Settings.DeviceToken,
					DeviceType  = "Android"
				}
			};

			try
			{
				var ObjectReturn2 = new JsonReturnModel();

				string results2 = ConnectWebAPI.Request(url2, json2);

				if (string.IsNullOrEmpty(results2))
				{
				}
				else
				{

					ObjectReturn2 = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonReturnModel>(results2);

					if (ObjectReturn2.IsSuccess)
					{

					}

				}
			}
			catch (Exception ee)
			{
			}
		}

        private void ListItemClicked(int position)
        {

            Intent activity = null;
            switch (position)
            {
                //case 0:
                //    activity = new Intent(this, typeof(LoginActivity));
                //    StartActivity(activity);
                //    break;
				case 1:
					SetPayment.Set("full");
					activity = new Intent(this, typeof(SelectPaymentMethodActivity));
					StartActivity(activity);
					break;
				case 2:
					if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
					{
						activity = new Intent(this, typeof(SendFeedbackActivity));
						StartActivity(activity);
					}
					else
					{
						activity = new Intent(this, typeof(SetupScheduleActivity));
						StartActivity(activity);
					}
					break;
				case 3:
					if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
					{
					}
					else
					{
						activity = new Intent(this, typeof(SendFeedbackActivity));
						StartActivity(activity);
					}
					break;
				case 4:
					if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
					{
						activity = new Intent(this, typeof(UpdateCreditCardActivity));
						StartActivity(activity);
					}
					break;
				case 5:
					if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
					{
						activity = new Intent(this, typeof(UpdateBankAccountActivity));
						StartActivity(activity);
					}
					else
					{
						activity = new Intent(this, typeof(UpdateCreditCardActivity));
						StartActivity(activity);
					}
					break;
				case 6:
					if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
					{
						activity = new Intent(this, typeof(UpdatePersonalInformationActivity));
						StartActivity(activity);
					}
					else
					{
						activity = new Intent(this, typeof(UpdateBankAccountActivity));
						StartActivity(activity);
					}
					break;
				case 7:
					activity = new Intent(this, typeof(UpdatePersonalInformationActivity));
					StartActivity(activity);
					break;
            }

            this.drawerLayout.CloseDrawers();
        }

        //Init menu on action bar
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            MenuInflater inflater = this.MenuInflater;

            inflater.Inflate(Resource.Menu.RefeshMenu, menu);

            return true;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {

            var drawerOpen = this.drawerLayout.IsDrawerOpen((int)GravityFlags.Left);
            //when open don't show anything
            //for (int i = 0; i < menu.Size(); i++)
            //    menu.GetItem(i).SetVisible(!drawerOpen);


            return base.OnPrepareOptionsMenu(menu);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            this.drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            this.drawerToggle.OnConfigurationChanged(newConfig);
        }

        // Pass the event to ActionBarDrawerToggle, if it returns
        // true, then it has handled the app icon touch event
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (this.drawerToggle.OnOptionsItemSelected(item))
                return true;

            base.OnOptionsItemSelected(item);

            switch (item.ItemId)
            {

                case Resource.Id.Lock:
                    ShowAlert();
                    break;

                default:
                    break;
            }

            return true;
        }

        public override void OnBackPressed()
        {
            ShowAlert();
        }

        public void ShowAlert()
        {
            Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alertDialog = builder.Create();
            alertDialog.SetTitle("RecoveriesConnect");
            //alertDialog.SetIcon(Resource.Drawable.Icon);
            alertDialog.SetMessage("Do you really want to exit?");
            //YES
            alertDialog.SetButton("Yes", (s, ev) =>
            {
                finish();
            });

            //NO
            alertDialog.SetButton3("No", (s, ev) =>
            {
                alertDialog.Hide();
            });

            alertDialog.Show();
        }

        private void finish()
        {
            //SaveData();     
            //base.OnBackPressed();
            this.Finish();
            Process.KillProcess(Process.MyPid());
        }

    }
}

