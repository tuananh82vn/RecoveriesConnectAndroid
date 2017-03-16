using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;

namespace RecoveriesConnect.Helpers
{
    public class Alert
    {
        public AlertDialog alertDialog;

        public Alert(Context context, string title, string message)
        {
            Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(context);
            alertDialog = builder.Create();
            alertDialog.SetTitle(title);
            //alertDialog.SetIcon(Resource.Drawable.Icon);
            alertDialog.SetMessage(message);
            //YES

            //NO
            alertDialog.SetButton3("Ok", (s, ev) =>
            {
                alertDialog.Hide();
            });

        }

        public void Show()
        {
            alertDialog.Show();

            new Handler().PostDelayed(() =>
            {
                alertDialog.Hide();

            }, 3000);
        }
    }

}