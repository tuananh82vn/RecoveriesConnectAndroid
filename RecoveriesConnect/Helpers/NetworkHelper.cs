using System;
using Android.Net;
using Java.Net;
using Java.Lang;
using System.IO;

namespace RecoveriesConnect.Helpers
{ 
    public static class NetworkHelper
    {
        public static bool DetectNetwork()
        {
            Runtime runtime = Runtime.GetRuntime();
            try
            {

                Process ipProcess = runtime.Exec("/system/bin/ping -c 1 8.8.8.8");
                int exitValue = ipProcess.WaitFor();
                return (exitValue == 0);

            }
            catch (IOException e) { }
            catch (InterruptedException e) { }

            return false;
        }
    }
}