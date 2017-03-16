using System;
using Android.OS;
using Java.Interop;

namespace RecoveriesConnect.Models.Api
{
    public class PaymentTrackerModel
    {
        public string InstalmentDate { get; set; }
        public string InstalmentAmount { get; set; }
        public string HistInstalDate { get; set; }
        public string HistInstalAmount { get; set; }
        public string HistPaymentDate { get; set; }
        public string HistPaymentAmount { get; set; }
        public string HistDeferredAmount { get; set; }
        public int Id { get; set; }
    }
}