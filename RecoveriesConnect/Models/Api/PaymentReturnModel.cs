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
    public class PaymentReturnModel
    {
        public int PaymentId { get; set; }
        public string Item { get; set; }
        public string ClientName { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Amount { get; set; }
        public string ReceiptNumber { get; set; }
        public string TransactionDescription { get; set; }
        public string FirstDebtorPaymentInstallmentId { get; set; }

        public Error[] Errors { get; set; }
        public bool IsSuccess { get; set; }

    }
}