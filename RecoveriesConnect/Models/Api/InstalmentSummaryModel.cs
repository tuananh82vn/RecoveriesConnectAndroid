using System;
using Android.OS;
using Java.Interop;

namespace RecoveriesConnect.Models.Api
{
	public sealed class InstalmentSummaryModel : Java.Lang.Object, IParcelable
	{
		public InstalmentSummaryModel()
		{
		}

		public string PaymentDate { get; set; }
		public double Amount { get; set; }

		public InstalmentSummaryModel(string payDate, double amount)
		{
			PaymentDate = payDate;
			Amount = amount;
		}

		private InstalmentSummaryModel(Parcel parcel)
		{
			PaymentDate = parcel.ReadString();
			Amount = parcel.ReadDouble();
		}

		public int DescribeContents()
		{
			return 0;
		}

		// Save this instance's values to the parcel
		public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
		{
			dest.WriteString(PaymentDate);
			dest.WriteDouble(Amount);
		}

		// The creator creates an instance of the specified object
		private static readonly GenericParcelableCreator<InstalmentSummaryModel> _creator
		= new GenericParcelableCreator<InstalmentSummaryModel>((parcel) => new InstalmentSummaryModel(parcel));

		[ExportField("CREATOR")]
		public static GenericParcelableCreator<InstalmentSummaryModel> GetCreator()
		{
			return _creator;
		}

	}

}

