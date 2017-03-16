using System;
using Android.Net;
using Java.Net;
using Java.Lang;
using System.IO;

namespace RecoveriesConnect.Helpers
{ 
    public static class TrackingHelper
    {
        public static void SendTracking(string activity)
        {
           string url = Settings.InstanceURL;

			var url2 = url + "/Api/SendActivityTracking";

			var json2 = new
			{

				ReferenceNumber = Settings.RefNumber,
				From  = "Android",
				Activity = activity

			};

			try
			{
				ConnectWebAPI.Request(url2, json2);
			}
			catch (System.Exception e)
			{
				
			}
        }
    }
}