using System;
namespace RecoveriesConnect
{
	public class ArrangeDetails
	{
		public string ReferenceNumber { get; set; }
		public decimal ArrangeAmount { get; set; }
		public string Frequency { get; set; }
		public decimal PaidAmount { get; set; }
		public decimal LeftToPay { get; set; }
		public string Status { get; set; }
		public decimal OverdueAmount { get; set; }
		public string NextInstalmentDate { get; set; }
		public bool IsSuccess { get; set; }
		public string Error { get; set; }
	}
}

