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
using Java.Text;
using Java.Util;

namespace RecoveriesConnect.Helpers
{
    public static class MoneyFormat
    {
        public static string Convert(decimal number)
        {
            Locale locale = new Locale("en", "AU");
            NumberFormat formatter = NumberFormat.GetCurrencyInstance(locale);
            return formatter.Format(double.Parse(number.ToString()));
        }
    }
}