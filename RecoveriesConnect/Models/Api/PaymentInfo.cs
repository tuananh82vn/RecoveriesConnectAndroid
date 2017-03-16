using System;
namespace RecoveriesConnect
{
	public class PaymentInfo
	{
		//public CardInfo card { get; set; }

		//public BankInfo bank { get; set; }

		//public PersonalInfo personalInfo { get; set; }

		public string RecType { get; set; }

		public string CCNo { get; set; }
		public string ExpiryDate { get; set; }

		public string BsbNo { get; set; }
		public string AccountNo { get; set; }
		public string AccountName { get; set; }

		public bool IsSuccess { get; set; }
		public string Error { get; set; }
	}
}

