using System;
namespace RecoveriesConnect.Helpers
{
	public static class SetPayment
	{
		public static void Set(string paymentType)
		{
			if (paymentType == "none") {
				Settings.MakePaymentInFull = false;
				Settings.MakePaymentIn3Part = false;
				Settings.MakePaymentInstallment = false;
				Settings.MakePaymentOtherAmount = false;

			}
			else
				if (paymentType == "full")
				{
					Settings.MakePaymentInFull = true;
					Settings.MakePaymentIn3Part = false;
					Settings.MakePaymentInstallment = false;
					Settings.MakePaymentOtherAmount = false;
				}
				else
					if (paymentType == "3part")
					{
						Settings.MakePaymentInFull = false;
						Settings.MakePaymentIn3Part = true;
						Settings.MakePaymentInstallment = false;
						Settings.MakePaymentOtherAmount = false;
					}
					else
					if (paymentType == "instalment")
					{
						Settings.MakePaymentInFull = false;
						Settings.MakePaymentIn3Part = false;
						Settings.MakePaymentInstallment = true;
						Settings.MakePaymentOtherAmount = false;
					}
					else
						if (paymentType == "other")
						{
							Settings.MakePaymentInFull = false;
							Settings.MakePaymentIn3Part = false;
							Settings.MakePaymentInstallment = false;
							Settings.MakePaymentOtherAmount = true;
						}
		}
	}
}

