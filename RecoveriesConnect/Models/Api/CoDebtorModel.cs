using System;
using Android.OS;
using Java.Interop;

namespace RecoveriesConnect.Models.Api
{
    public sealed class CoDebtorModel : Java.Lang.Object, IParcelable
    {
        public CoDebtorModel()
        {

        }

        public string debtorCode { get; set; }
        public string fullName { get; set; }
        public string mobile { get; set; }
        public string markMobile { get; set; }

        public CoDebtorModel(string DebtorCode, string FullName, string Mobile, string MarkMobile)
        {
            debtorCode = DebtorCode;
            fullName = FullName;
            mobile = Mobile;
            markMobile = MarkMobile;
        }

        private CoDebtorModel(Parcel parcel)
        {
            debtorCode =    parcel.ReadString();
            fullName =      parcel.ReadString();
            mobile =        parcel.ReadString();
            markMobile =    parcel.ReadString();
        }




        public int DescribeContents()
        {
            return 0;
        }

        // Save this instance's values to the parcel
        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteString(debtorCode);
            dest.WriteString(fullName);
            dest.WriteString(mobile);
            dest.WriteString(markMobile);
        }

        // The creator creates an instance of the specified object
        private static readonly GenericParcelableCreator<CoDebtorModel> _creator
        = new GenericParcelableCreator<CoDebtorModel>((parcel) => new CoDebtorModel(parcel));

        [ExportField("CREATOR")]
        public static GenericParcelableCreator<CoDebtorModel> GetCreator()
        {
            return _creator;
        }

    }

    public sealed class GenericParcelableCreator<T> : Java.Lang.Object, IParcelableCreator
        where T : Java.Lang.Object, new()
    {
        private readonly Func<Parcel, T> _createFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParcelableDemo.GenericParcelableCreator`1"/> class.
        /// </summary>
        /// <param name='createFromParcelFunc'>
        /// Func that creates an instance of T, populated with the values from the parcel parameter
        /// </param>
        public GenericParcelableCreator(Func<Parcel, T> createFromParcelFunc)
        {
            _createFunc = createFromParcelFunc;
        }

        #region IParcelableCreator Implementation

        public Java.Lang.Object CreateFromParcel(Parcel source)
        {
            return _createFunc(source);
        }

        public Java.Lang.Object[] NewArray(int size)
        {
            return new T[size];
        }

        #endregion
    }

}

