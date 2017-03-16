using System;
using System.Collections.Generic;
using RecoveriesConnect.Models.Api;

namespace RecoveriesConnect
{
	public class CallbackReturnModel
	{

		public DateTime CallbackDate { get; set; }

		public List<string> CallbackSlot { get; set; }

		public bool IsSuccess { get; set; }

		public Error[] Errors { get; set; }
	}
}

