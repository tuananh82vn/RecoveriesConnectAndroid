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

namespace RecoveriesConnect.Models.Api
{
    public class JsonReturnModel
    {
        public string Item { get; set; }

        public bool IsSuccess { get; set; }

        public Error[] Errors { get; set; }
    }

    public class Error
    {
        public string ErrorMessage { get; set; }
    } 
}