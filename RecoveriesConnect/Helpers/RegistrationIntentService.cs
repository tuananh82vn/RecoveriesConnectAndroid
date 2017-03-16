using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Gcm.Iid;
using Android.Widget;
using RecoveriesConnect.Helpers;

namespace RecoveriesConnect
{
	[Service(Exported = false)]
	public class RegistrationIntentService : IntentService
	{
		static object locker = new object();

		public RegistrationIntentService() : base("RegistrationIntentService") { }

		protected override void OnHandleIntent(Intent intent)
		{
			try
			{
				Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");
				lock (locker)
				{
					//This is the project ID - get from https://console.developers.google.com/apis/library?project=recoveriesconnect
					var sender_id = "935907664932";

					var instanceID = InstanceID.GetInstance(this);
					var token = instanceID.GetToken(sender_id, GoogleCloudMessaging.InstanceIdScope, null);
					SendRegistrationToAppServer(token);
					Subscribe(token);
				}
			}
			catch (Exception e)
			{
				return;
			}
		}

		void SendRegistrationToAppServer(string token)
		{
			Settings.DeviceToken = token;
			//Toast.MakeText(this, token, ToastLength.Short).Show();
			// Add custom implementation here as needed.
		}

		void Subscribe(string token)
		{
			var pubSub = GcmPubSub.GetInstance(this);
			pubSub.Subscribe(token, "/topics/global", null);
		}
	}
}

