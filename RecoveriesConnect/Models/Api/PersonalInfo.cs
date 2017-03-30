namespace RecoveriesConnect
{
	public class PersonalInfo
	{
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string StreetAddress3 { get; set; }
        public string StreetSuburb { get; set; }
        public string StreetState { get; set; }
        public string StreetPostCode { get; set; }

        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public string MailAddress3 { get; set; }
        public string MailSuburb { get; set; }
        public string MailState { get; set; }
        public string MailPostCode { get; set; }

        public string Origin_StreetAddress1 { get; set; }
        public string Origin_StreetAddress2 { get; set; }
        public string Origin_StreetAddress3 { get; set; }
        public string Origin_StreetSuburb { get; set; }
        public string Origin_StreetState { get; set; }
        public string Origin_StreetPostCode { get; set; }

        public string Origin_MailAddress1 { get; set; }
        public string Origin_MailAddress2 { get; set; }
        public string Origin_MailAddress3 { get; set; }
        public string Origin_MailSuburb { get; set; }
        public string Origin_MailState { get; set; }
        public string Origin_MailPostCode { get; set; }

        public string Marked_StreetAddress1 { get; set; }
        public string Marked_StreetAddress2 { get; set; }
        public string Marked_StreetAddress3 { get; set; }
        public string Marked_StreetSuburb { get; set; }
        public string Marked_StreetState { get; set; }
        public string Marked_StreetPostCode { get; set; }

        public string Marked_MailAddress1 { get; set; }
        public string Marked_MailAddress2 { get; set; }
        public string Marked_MailAddress3 { get; set; }
        public string Marked_MailSuburb { get; set; }
        public string Marked_MailState { get; set; }
        public string Marked_MailPostCode { get; set; }

        public string Marked_HomeNumber { get; set; }
        public string Marked_WorkNumber { get; set; }
        public string Marked_MobileNumbers { get; set; }
        public string Marked_EmailAddress { get; set; }

        public string HomeNumber { get; set; }
		public string WorkNumber { get; set; }
		public string MobileNumber { get;  set; }

        public string EmailAddress { get; set; }

        public string Origin_HomeNumber { get; set; }
        public string Origin_WorkNumber { get; set; }
        public string Origin_MobileNumbers { get; set; }
        public string Origin_EmailAddress { get; set; }

        public int Preferred { get; set; }

        public bool IsSuccess { get; set; }
		public string Error { get; set; }
	}
}