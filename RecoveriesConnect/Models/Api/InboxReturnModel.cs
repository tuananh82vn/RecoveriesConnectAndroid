using System;
using RecoveriesConnect.Models.Api;

namespace RecoveriesConnect
{
	public class InboxReturnModel
	{
		public Inbox[] InboxList { get; set; }

		public bool IsSuccess { get; set; }

		public Error[] Errors { get; set; }
	}
}

