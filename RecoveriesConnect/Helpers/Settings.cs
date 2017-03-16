using System;
using Refractored.Xam.Settings.Abstractions;
using Refractored.Xam.Settings;

namespace RecoveriesConnect.Helpers
{
	public static class Settings
	{

		private const string PathDatabaseKey = "PathDatabaseKey";
		private static readonly string PathDatabaseKey_Default = string.Empty;

		private const string DeviceTokenKey = "DeviceTokenKey";
		private static readonly string DeviceTokenKey_Default = string.Empty;

        private const string InstanceURLKey = "InstanceURLKey";
        private static readonly string InstanceURLKey_Default = "http://172.28.1.70:9999";
        //private static readonly string InstanceURLKey_Default = "http://180.94.113.19:3333";

        private const string ArrangementDebtorKey = "ArrangementDebtorKey";
		private static readonly string ArrangementDebtorKey_Default = string.Empty;

		private const string IsArrangementUnderThisDebtorKey = "IsArrangementUnderThisDebtorKey";
		private static readonly bool IsArrangementUnderThisDebtorKey_Default = false;

		private const string IsFuturePaymentKey = "IsFuturePaymentKey";
		private static readonly bool IsFuturePaymentKey_Default = false;

		private const string MakePaymentInFullKey = "MakePaymentInFullKey";
		private static readonly bool MakePaymentInFullKey_Default = false;

		private const string MakePaymentIn3PartKey = "MakePaymentIn3PartKey";
		private static readonly bool MakePaymentIn3PartKey_Default = false;

		private const string MakePaymentInstallmentKey = "MakePaymentInstallmentKey";
		private static readonly bool MakePaymentInstallmentKey_Default = false;

		private const string MakePaymentOtherAmountKey = "MakePaymentOtherAmountKey";
		private static readonly bool MakePaymentOtherAmountKey_Default = false;

		private const string RefNumberKey = "RefNumberKey";
		private static readonly string RefNumberKey_Default = string.Empty;

        private const string IsAlreadySetupPinKey = "IsAlreadySetupPinKey";
        private static readonly bool IsAlreadySetupPinKey_Default = false;

        private const string IsCoBorrowersKey = "IsCoBorrowersKey";
        private static readonly bool IsCoBorrowersKey_Default = false;

        private const string IsCoBorrowerSelectedKey = "IsCoBorrowerSelectedKey";
        private static readonly bool IsCoBorrowerSelectedKey_Default = false;

        private const string DebtorCodeSelectedKey = "DebtorCodeSelectedKey";
        private static readonly string DebtorCodeSelectedKey_Default = "";

        private const string PinNumberKey = "PinNumberKey";
        private static readonly string PinNumberKey_Default = "";

        private const string MaxNoPayKey = "MaxNoPayKey";
        private static readonly int MaxNoPayKey_Default = 0;

        private const string TotalPaidKey = "TotalPaidKey";
        private static readonly decimal TotalPaidKey_Default = 0;

        private const string TotalOverDueKey = "TotalOverDueKey";
        private static readonly decimal TotalOverDueKey_Default = 0;

        private const string WeeklyAmountKey = "WeeklyAmountKey";
        private static readonly decimal WeeklyAmountKey_Default = 0;

        private const string MonthlyAmountKey = "MonthlyAmountKey";
        private static readonly decimal MonthlyAmountKey_Default = 0;

        private const string FortnightAmountKey = "FortnightAmountKey";
        private static readonly decimal FortnightAmountKey_Default = 0;

        private const string TotalOutstandingKey = "TotalOutstandingKey";
        private static readonly decimal TotalOutstandingKey_Default = 0;

        private const string IsExistingArrangementKey = "IsExistingArrangementKey";
        private static readonly bool IsExistingArrangementKey_Default = false;

        private const string IsExistingArrangementCCKey = "IsExistingArrangementCCKey";
        private static readonly bool IsExistingArrangementCCKey_Default = false;

        private const string IsExistingArrangementDDKey = "IsExistingArrangementDDKey";
        private static readonly bool IsExistingArrangementDDKey_Default = false;

        private const string ThreePartDurationDaysKey = "ThreePartDurationDaysKey";
        private static readonly int ThreePartDurationDaysKey_Default = 0;

        private const string IsAllowMonthlyInstallmentKey = "IsAllowMonthlyInstallmentKey";
        private static readonly bool IsAllowMonthlyInstallmentKey_Default = false;

        private const string FirstAmountOfInstallmentKey = "FirstAmountOfInstallmentKey";
        private static readonly decimal FirstAmountOfInstallmentKey_Default = 0;

        private const string NextPaymentInstallmentKey = "NextPaymentInstallmentKey";
        private static readonly decimal NextPaymentInstallmentKey_Default = 0;

        private const string FrequencyKey = "FrequencyKey";
        private static readonly int FrequencyKey_Default = 0;

        private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		public static string PathDatabase
		{
			get { return AppSettings.GetValueOrDefault(PathDatabaseKey, PathDatabaseKey_Default); }
			set { AppSettings.AddOrUpdateValue(PathDatabaseKey, value); }
		}

		public static string DeviceToken
        {
			get { return AppSettings.GetValueOrDefault(DeviceTokenKey, DeviceTokenKey_Default); }
			set { AppSettings.AddOrUpdateValue(DeviceTokenKey, value); }
		}

		public static string InstanceURL
        {
			get { return AppSettings.GetValueOrDefault(InstanceURLKey, InstanceURLKey_Default); }
			set { AppSettings.AddOrUpdateValue(InstanceURLKey, value); }
		}

		public static bool IsArrangementUnderThisDebtor
        {
			get { return AppSettings.GetValueOrDefault(IsArrangementUnderThisDebtorKey, IsArrangementUnderThisDebtorKey_Default); }
			set { AppSettings.AddOrUpdateValue(IsArrangementUnderThisDebtorKey, value); }
		}

		public static bool MakePaymentInFull
        {
			get { return AppSettings.GetValueOrDefault(MakePaymentInFullKey, MakePaymentInFullKey_Default); }
			set { AppSettings.AddOrUpdateValue(MakePaymentInFullKey, value); }
		}

		public static bool MakePaymentIn3Part
        {
			get { return AppSettings.GetValueOrDefault(MakePaymentIn3PartKey, MakePaymentIn3PartKey_Default); }
			set { AppSettings.AddOrUpdateValue(MakePaymentIn3PartKey, value); }
		}

		public static bool MakePaymentInstallment
        {
			get { return AppSettings.GetValueOrDefault(MakePaymentInstallmentKey, MakePaymentInstallmentKey_Default); }
			set { AppSettings.AddOrUpdateValue(MakePaymentInstallmentKey, value); }
		}

		public static bool MakePaymentOtherAmount
        {
			get { return AppSettings.GetValueOrDefault(MakePaymentOtherAmountKey, MakePaymentOtherAmountKey_Default); }
			set { AppSettings.AddOrUpdateValue(MakePaymentOtherAmountKey, value); }
		}

		public static string RefNumber
        {
			get { return AppSettings.GetValueOrDefault(RefNumberKey, RefNumberKey_Default); }
			set { AppSettings.AddOrUpdateValue(RefNumberKey, value); }
		}

        public static bool IsAlreadySetupPin
        {
            get { return AppSettings.GetValueOrDefault(IsAlreadySetupPinKey, IsAlreadySetupPinKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsAlreadySetupPinKey, value); }
        }

        public static bool IsCoBorrowers {
            get { return AppSettings.GetValueOrDefault(IsCoBorrowersKey, IsCoBorrowersKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsCoBorrowersKey, value); }
        }

        public static string ArrangementDebtor
        {
            get { return AppSettings.GetValueOrDefault(ArrangementDebtorKey, ArrangementDebtorKey_Default); }
            set { AppSettings.AddOrUpdateValue(ArrangementDebtorKey_Default, value); }
        }
        public static bool IsCoBorrowerSelected
        {
            get { return AppSettings.GetValueOrDefault(IsCoBorrowerSelectedKey, IsCoBorrowerSelectedKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsCoBorrowerSelectedKey, value); }
        }

        public static string DebtorCodeSelected
        {
            get { return AppSettings.GetValueOrDefault(DebtorCodeSelectedKey, DebtorCodeSelectedKey_Default); }
            set { AppSettings.AddOrUpdateValue(DebtorCodeSelectedKey, value); }
        }

        public static string PinNumber
        {
            get { return AppSettings.GetValueOrDefault(PinNumberKey, PinNumberKey_Default); }
            set { AppSettings.AddOrUpdateValue(PinNumberKey, value); }
        }

        public static int MaxNoPay
        {
            get { return AppSettings.GetValueOrDefault(MaxNoPayKey, MaxNoPayKey_Default); }
            set { AppSettings.AddOrUpdateValue(MaxNoPayKey, value); }
        }

        public static decimal TotalPaid
        {
            get { return AppSettings.GetValueOrDefault(TotalPaidKey, TotalPaidKey_Default); }
            set { AppSettings.AddOrUpdateValue(TotalPaidKey, value); }
        }

        public static decimal TotalOverDue
        {
            get { return AppSettings.GetValueOrDefault(TotalOverDueKey, TotalOverDueKey_Default); }
            set { AppSettings.AddOrUpdateValue(TotalOverDueKey, value); }
        }

        public static decimal WeeklyAmount
        {
            get { return AppSettings.GetValueOrDefault(WeeklyAmountKey, WeeklyAmountKey_Default); }
            set { AppSettings.AddOrUpdateValue(WeeklyAmountKey, value); }
        }

        public static decimal MonthlyAmount
        {
            get { return AppSettings.GetValueOrDefault(MonthlyAmountKey, MonthlyAmountKey_Default); }
            set { AppSettings.AddOrUpdateValue(MonthlyAmountKey, value); }
        }

        public static decimal FortnightAmount
        {
            get { return AppSettings.GetValueOrDefault(FortnightAmountKey, FortnightAmountKey_Default); }
            set { AppSettings.AddOrUpdateValue(FortnightAmountKey, value); }
        }

        public static decimal TotalOutstanding
        {
            get { return AppSettings.GetValueOrDefault(TotalOutstandingKey, TotalOutstandingKey_Default); }
            set { AppSettings.AddOrUpdateValue(TotalOutstandingKey, value); }
        }

        public static bool IsExistingArrangement
        {
            get { return AppSettings.GetValueOrDefault(IsExistingArrangementKey, IsExistingArrangementKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsExistingArrangementKey, value); }
        }

        public static bool IsExistingArrangementCC
        {
            get { return AppSettings.GetValueOrDefault(IsExistingArrangementCCKey, IsExistingArrangementCCKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsExistingArrangementCCKey, value); }
        }

        public static bool IsExistingArrangementDD
        {
            get { return AppSettings.GetValueOrDefault(IsExistingArrangementDDKey, IsExistingArrangementDDKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsExistingArrangementDDKey, value); }
        }

        public static int ThreePartDurationDays
        {
            get { return AppSettings.GetValueOrDefault(ThreePartDurationDaysKey, ThreePartDurationDaysKey_Default); }
            set { AppSettings.AddOrUpdateValue(ThreePartDurationDaysKey, value); }
        }

        public static bool IsAllowMonthlyInstallment
        {
            get { return AppSettings.GetValueOrDefault(IsAllowMonthlyInstallmentKey, IsAllowMonthlyInstallmentKey_Default); }
            set { AppSettings.AddOrUpdateValue(IsAllowMonthlyInstallmentKey, value); }
        }

        public static decimal FirstAmountOfInstallment
        {
            get { return AppSettings.GetValueOrDefault(FirstAmountOfInstallmentKey, FirstAmountOfInstallmentKey_Default); }
            set { AppSettings.AddOrUpdateValue(FirstAmountOfInstallmentKey, value); }
        }

        public static decimal NextPaymentInstallment
        {
            get { return AppSettings.GetValueOrDefault(NextPaymentInstallmentKey, NextPaymentInstallmentKey_Default); }
            set { AppSettings.AddOrUpdateValue(NextPaymentInstallmentKey, value); }
        }

        public static int Frequency
        {
            get { return AppSettings.GetValueOrDefault(FrequencyKey, FrequencyKey_Default); }
            set { AppSettings.AddOrUpdateValue(FrequencyKey, value); }
        }
		public static bool IsFuturePayment
		{
			get { return AppSettings.GetValueOrDefault(IsFuturePaymentKey, IsFuturePaymentKey_Default); }
			set { AppSettings.AddOrUpdateValue(IsFuturePaymentKey, value); }
		}
    }
}

