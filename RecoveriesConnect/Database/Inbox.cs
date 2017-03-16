using System;
using SQLite;

namespace RecoveriesConnect
{
	public class Inbox
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string MessageNo { get; set; }

		public string Date { get; set; }

		public string Type { get; set; }

		public string Status { get; set; }

		public string MessagePathText { get; set; }

		public string IsLocal { get; set; }

		public string FileName1 { get; set; }

		public override string ToString()
		{
			return string.Format("[Inbox: ID={0}, MessageNo={1}, Date={2}]", ID, MessageNo, Date);
		}
	}
}

