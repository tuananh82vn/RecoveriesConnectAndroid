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
using System.Text.RegularExpressions;

namespace RecoveriesConnect.Helpers
{
    public static class Validation
    {
        public static bool IsValidCreditCardNumber(int cardtype, string ccnum)
        {
            if (cardtype == 0)
                return false;

            string regExp = "";

            if (ccnum.Trim().Length > 16)
                return false;

            //1 : Master
            //2 : Visa

            if (cardtype == 2)
                regExp = "^[4]([0-9]{15}$|[0-9]{12}$)";
            else if (cardtype == 1)
                regExp = "^[5][1-5][0-9]{14}$";


            if (!Regex.IsMatch(ccnum, regExp))
                return false;

            string[] tempNo = ccnum.Split('-');
            ccnum = String.Join("", tempNo);

            int checksum = 0;
            for (int i = (2 - (ccnum.Length % 2)); i <= ccnum.Length; i += 2)
            {
                checksum += Convert.ToInt32(ccnum[i - 1].ToString());
            }

            int digit = 0;
            for (int i = (ccnum.Length % 2) + 1; i < ccnum.Length; i += 2)
            {
                digit = 0;
                digit = Convert.ToInt32(ccnum[i - 1].ToString()) * 2;
                if (digit < 10)
                { checksum += digit; }
                else
                { checksum += (digit - 9); }
            }
            if ((checksum % 10) == 0)
                return true;
            else
                return false;
        }

        public static bool IsValidCreditCardName(string nameoncard)
        {
            bool IsValid = false;
            string[] cardname = nameoncard.Trim().Split(' ');
            string regexp = "^[-a-zA-Z ]*$";
            if (cardname.Count() >= 2)
            {
                foreach (var p in cardname)
                {
                    if (Regex.IsMatch(p, regexp))
                        IsValid = true;
                    else
                    {
                        IsValid = false;
                        return IsValid;
                    }
                }
            }
            return IsValid;
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

		public static bool isValidPhone(string inputPhone)
		{
			string strRegex = "^\\({0,1}((0|\\+61)(2|4|3|7|8)){0,1}\\){0,1}(\\ |-){0,1}[0-9]{2}(\\ |-){0,1}[0-9]{2}(\\ |-){0,1}[0-9]{1}(\\ |-){0,1}[0-9]{3}$";
			Regex re = new Regex(strRegex);
			if (re.IsMatch(inputPhone))
				return (true);
			else
				return (false);
		}
    }
}