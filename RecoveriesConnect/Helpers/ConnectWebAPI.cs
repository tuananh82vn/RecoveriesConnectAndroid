using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Text;
using Android.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RecoveriesConnect.Helpers
{
    public static class ConnectWebAPI
    {
		
        public static string Request(string url, object json)
        {
			// Get Response
			// HttpWebResponse response = null;
			try
            {
				HttpWebRequest request = null;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.KeepAlive = true;
				//request.ProtocolVersion = HttpVersion.Version11;
				//request.Timeout = 10000;
				//request.ReadWriteTimeout = 10000;

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(json);

                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

                request.ContentLength = requestBytes.Length;

				using (var requestStream = request.GetRequestStream())
				{
					requestStream.Write(requestBytes, 0, requestBytes.Length);

					requestStream.Close();
				}

				using (WebResponse myHttpWebResponse = request.GetResponse())
				{
					string responseText = string.Empty;

					WebHeaderCollection header = myHttpWebResponse.Headers;

					var encoding = Encoding.ASCII;

					using (var reader = new System.IO.StreamReader(myHttpWebResponse.GetResponseStream(), encoding))
					{
						responseText = reader.ReadToEnd();
					}

					myHttpWebResponse.Dispose();
					return responseText;
				}
            }
            catch (Exception wex)
            {
                return "";
            }
            //Return Response 
        }
    }
}