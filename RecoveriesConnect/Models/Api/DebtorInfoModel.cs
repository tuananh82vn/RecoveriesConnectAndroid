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
    public class DebtorInfoModel
    {


        public virtual String ReferenceNumber { get; set; }

        public virtual Decimal TotalOutstanding { get; set; }

        public virtual Nullable<Double> OtherAmount { get; set; }

        public virtual String TotalOtherAmount { get { return string.Format("{0:c}", this.OtherAmount); } }

        public virtual Decimal MinimumWeeklyOutstanding { get; set; }

        public virtual Decimal MinimumFortnightlyOutstanding { get; set; }

        public virtual Decimal MinimumMonthlyOutstanding { get; set; }

        public virtual String AccountCode { get; set; }

        public virtual String ClientCode { get; set; }

        public virtual String ClientName { get; set; }

        public virtual String DebtorCode { get; set; }

        public virtual String FirstName { get; set; }

        public virtual String LastName { get; set; }

        public virtual String FullName { get; set; }

        public virtual String MobileNumbers { get; set; }

        public virtual String MobileNumberFormatted
        {
            get
            {
                string mobileno = string.Empty;
                if (MobileNumber != null)
                {
                    int mno = MobileNumber.Length;
                    if (mno > 4)
                        mobileno = "*** *** " + MobileNumber.Substring(mno - 4, 4);
                }
                return mobileno;
            }
        }

        public virtual String PaymentHistoryPresent { get; set; }

        public virtual Double FirstPaymentInstallment { get; set; }

        public virtual String FirstPaymentInstallmentAmount { get { return string.Format("{0:c}", this.FirstPaymentInstallment); } }

        public virtual Double NextPaymentInstallment { get; set; }

        public virtual String NextPaymentInstallmentAmount { get { return string.Format("{0:c}", this.NextPaymentInstallment); } }

        public virtual Int32 PaymentType { get; set; }


        public virtual string DateOfBirths { get; set; }

        public virtual String RegNumbers { get; set; }

        public virtual String Address1s { get; set; }

        public virtual String Address2s { get; set; }

        public virtual String Address3s { get; set; }

        public virtual String Suburbs { get; set; }

        public virtual String States { get; set; }

        public virtual String PostCodes { get; set; }

        public virtual String Countrys { get; set; }


        public virtual String TotalOutstandingAmount { get { return string.Format("{0:c}", this.TotalOutstanding); } }


        public virtual String Name
        {
            get
            {
                string fullName = string.Empty;
                if (!string.IsNullOrEmpty(this.FirstName) && !string.IsNullOrEmpty(this.LastName))
                {
                    fullName = this.FirstName + " " + this.LastName;
                }
                return fullName;
            }
        }

        public virtual String MobileNumber
        {
            get
            {
                return this.MobileNumbers;
            }
        }

        public virtual bool IsPaymentHistoryPresent
        {
            get;
            set;
        }

        public virtual String DriverLicenseNumber { get; set; }
        public virtual int MaxNoPay { get; set; }
        public virtual String DateOfDebt { get; set; }

        public virtual String NTID { get; set; }

        public virtual String MerchantId
        {
            get
            {
                string marchantid = string.Empty;
                if (!string.IsNullOrEmpty(this.NTID))
                {
                    int pos1 = this.NTID.IndexOf("/");
                    if (pos1 >= 0)
                    {
                        marchantid = this.NTID.Substring(pos1 + 1);
                    }
                    else
                    {
                        marchantid = this.NTID;
                    }
                }
                return marchantid;
            }


        }
        public virtual bool IsExistingArrangementDD { get; set; }

        public virtual bool IsExistingArrangementCC { get; set; }

        public virtual bool IsExistingArrangement { get; set; }

        public virtual bool IsCoBorrowers { get; set; }

        public virtual string PinNumber { get; set; }

        public virtual string PinNumberInput { get; set; }

        public virtual string Netcode { get; set; }

        public virtual string ArrangementDebtor { get; set; }

        public virtual bool IsArrangementUnderThisDebtor { get; set; }

        public virtual string ArrangementType { get; set; }

        //public virtual DebtorPayment CurrentPayment { get; set; }

        public virtual bool IsSMSchecked { get; set; }

        public virtual bool IsPinNumberChecked { get; set; }

        public virtual bool IsMobileAvailable { get; set; }

        public virtual bool isVerify { get; set; }

        public virtual bool IsSendFuturePaymentSmsReminder { get; set; }


        public virtual int SmsReminderDays { get; set; }

        public virtual bool IsSendFuturePaymentEmailReminder { get; set; }


        public virtual int EmailReminderDays { get; set; }

        public string EmailAddress { get; set; }

        public virtual bool IsFutureDate { get; set; }

        public virtual bool IsCoBorrowerSelected { get; set; }

        public virtual List<string> CoDebtorCode { get; set; }
        public virtual List<string> CoFirstName { get; set; }
        public virtual List<string> CoLastName { get; set; }
        public virtual List<string> CoMobileNumbers { get; set; }
        public virtual List<string> CoDriverLicenseNumber { get; set; }

        //public virtual List<InstalmentSchedule> InstalmentScheduleList { get; set; }
        //public virtual List<HistoryPayment> HistoryPaymentList { get; set; }
        //public virtual List<HistoryInstalmentSchedule> HistoryInstalmentScheduleList { get; set; }

        public virtual string Action { get; set; }
        public virtual string NumberInstalmentRemain { get; set; }
        public virtual string FinalInstalmentDate { get; set; }
        public virtual string TotalPaid { get; set; }
        public bool IsPinNumberExits { get; set; }


        // For API Controller
        public string CreditCardNumber { get; set; }
        public string CreditCardCVV { get; set; }
        public string CreditCardExpiryYear { get; set; }
        public string CreditCardExpiryMonth { get; set; }
        public string Amount { get; set; }
        public string NameOnCard { get; set; }
        public int CardType { get; set; }
        public bool IsSuccess { get; set; }

        public int? CurrentPaymentId { get; set; }

        public string DirectDebitAccountName { get; set; }

        public string DirectDebitAccountNumber { get; set; }

        public string DirectDebitBSB1 { get; set; }

        public string DirectDebitBSB2 { get; set; }

        public int PaymentMethod { get; set; }

        public string HomeNumber { get; set; }

        public string WorkNumber { get; set; }

        public bool IsAllowMonthlyInstallment { get; set; }

        public string Error { get; set; }

        public string DebtorPaymentInstallment { get; set; }
        public int TotalRemainingDefer { get; set; }
        public int TotalUsedDefer { get; set; }
        public int TotalDefer { get; set; }
        public string MessageNo { get; set; }

        public int? DebtorPaymentInstallmentId { get; set; }

        public int InstalmentPaymentFrequency { get; set; }

        public ClientModel client { get; set; }

        public PaymentTrackerModel[] HistoryInstalmentScheduleList { get; set; }

        public PaymentTrackerModel[] InstalmentScheduleList { get; set; }

        public string ClientAccNo { get; set; }

    }
}