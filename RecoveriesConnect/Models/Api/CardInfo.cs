namespace RecoveriesConnect
{
	public class CardInfo
	{
		public decimal Amount { get; set; }
		public int CardType { get; set; }
		public string NameOnCard { get; set; }
		public string CardNumber { get; set; }
		public int ExpiryMonth { get; set; }
		public int ExpiryYear { get; set; }
		public string Expirydate { get; set; }
		public string Cvv { get; set; }
	}
}