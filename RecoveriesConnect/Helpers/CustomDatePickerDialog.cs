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
using Java.Lang.Reflect;
using Android.Content.Res;

namespace RecoveriesConnect.Helpers
{
    public class DatePickerDialogNoYear : DatePickerDialog
    {
        public DatePickerDialogNoYear(Context context, EventHandler<DateSetEventArgs> callBack, int year, int monthOfYear, int dayOfMonth): base(context, callBack, year, monthOfYear, dayOfMonth)
        {
            hideDay();
        }
        private void hideDay()
        {
            DatePicker datePicker = this.DatePicker;

            int daySpinnerId = Resources.System.GetIdentifier("day", "id", "android");
            if (daySpinnerId != 0)
            {
                View daySpinner = datePicker.FindViewById(daySpinnerId);
                if (daySpinner != null)
                {
                    daySpinner.Visibility = ViewStates.Gone;
                }
            }

            DateTime maxdate = DateTime.Today.AddYears(10);
            datePicker.MaxDate = new Java.Util.Date(maxdate.Year - 1900, maxdate.Month - 1, maxdate.Day).Time;

			DateTime minDate = DateTime.Today;
			datePicker.MinDate = new Java.Util.Date(minDate.Year - 1900, minDate.Month - 1, minDate.Day).Time;

        }

        public override void OnDateChanged(DatePicker view, int year, int month, int day)
        {
            base.OnDateChanged(view, year, month, day);
            SetTitle(month+1 + "/" + year);
        }
    }
}
