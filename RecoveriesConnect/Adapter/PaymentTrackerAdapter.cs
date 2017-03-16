using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Java.Lang;
using Object = Java.Lang.Object;
using System.Linq;
using Android.Graphics;
using RecoveriesConnect.Models.Api;
using RecoveriesConnect.Helpers;
using System.Globalization;

namespace RecoveriesConnect.Adapter
{
    public class PaymentTrackerAdapter : BaseAdapter<PaymentTrackerModel>, IFilterable
    {

        private List<PaymentTrackerModel> _originalData;
        private List<PaymentTrackerModel> _OrderList;
        public Filter Filter { get; private set; }

        private int[] mAlternatingColors;

        Activity _activity;

        private string type;

        public PaymentTrackerAdapter(Activity activity, List<PaymentTrackerModel> data, string Type)
        {
            _activity = activity;
            _OrderList = data;

            this.type = Type;

            Filter = new PaymentTrackerFilter(this);

            mAlternatingColors = new int[] { 0xF2F2F2, 0xC3C3C3 };
        }

        public override PaymentTrackerModel this[int position]
        {
            get { return _OrderList[position]; }
        }

        public PaymentTrackerModel GetItemAtPosition(int position)
        {
            return _OrderList[position];
        }

        public override int Count
        {
            get
            {
                if (_OrderList == null || _OrderList.Count == 0)
                {
                    return 1;
                }
                else
                {
                    return _OrderList.Count;
                }
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            // could wrap a Contact in a Java.Lang.Object
            // to return it here if needed
            return null;
        }

        public override long GetItemId(int position)
        {
            if(_OrderList.Count == 0)
            {
                return 0;
            }
            else
                return long.Parse(_OrderList[position].Id.ToString());
        }

        //public string GetItemName(int position)
        //{
        //    return _OrderList[position].StockName;
        //}

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            if (this._OrderList.Count == 0)
            {
				if (this.type == "Defer")
				{
					view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NoPaymentDeferItem, null, false);
				}
				else if (this.type == "Schedule")
				{
					view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NoPaymentTrackerItem1, null, false);
				}
				else
				{
					view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NoPaymentTrackerItem2, null, false);
				}

            }
            else
            {
                view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.PaymentTrackerList, parent, false);
                var tv_Date = view.FindViewById<TextView>(Resource.Id.tv_Date);
                var tv_Amount = view.FindViewById<TextView>(Resource.Id.tv_Amount);
                var iv_Status = view.FindViewById<ImageView>(Resource.Id.iv_Status);
                var linearLayout = view.FindViewById<LinearLayout>(Resource.Id.layout_middle);


				if (this.type == "Schedule")
				{
					tv_Date.Text = _OrderList[position].InstalmentDate;

					var amount = _OrderList[position].InstalmentAmount;
					tv_Amount.Text = MoneyFormat.Convert(decimal.Parse(amount));


					var DueDate = DateTime.ParseExact(tv_Date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

					if (DueDate < DateTime.Today)
					{
						iv_Status.SetBackgroundResource(Resource.Drawable.red);
					}
					else
					{
						iv_Status.SetBackgroundResource(Resource.Color.transparent);
					}
				}
				else if (this.type == "History")
				{
					tv_Date.Text = _OrderList[position].HistInstalDate;


					var amount = _OrderList[position].HistInstalAmount;
					tv_Amount.Text = MoneyFormat.Convert(decimal.Parse(amount));


					//var DueDate = DateTime.ParseExact(tv_Date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
					//var PayDate = DateTime.ParseExact(tv_Date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
					var dueAmount = decimal.Parse(amount);
					decimal deferAmount = 0;
					if (!string.IsNullOrEmpty(_OrderList[position].HistDeferredAmount))
					{
						deferAmount = decimal.Parse(_OrderList[position].HistDeferredAmount);
					}

					decimal payAmount = 0;
					if (!string.IsNullOrEmpty(_OrderList[position].HistPaymentAmount))
					{
						payAmount = decimal.Parse(_OrderList[position].HistPaymentAmount);
					}

					var payDate = _OrderList[position].HistPaymentDate;

					if (decimal.Parse(amount) <= 0)
					{
						iv_Status.SetBackgroundResource(Resource.Drawable.red);
					}
					else if (deferAmount > 0)
					{
						iv_Status.SetBackgroundResource(Resource.Drawable.yellow);
					}
					else if (payAmount == 0 && string.IsNullOrEmpty(payDate))
					{
						iv_Status.SetBackgroundResource(Resource.Drawable.red);
					}
					else if (dueAmount < Settings.NextPaymentInstallment)
					{
						iv_Status.SetBackgroundResource(Resource.Drawable.red);
					}
					else
					{
						iv_Status.SetBackgroundResource(Resource.Drawable.blue);
					}
				}
				else if (this.type == "Defer")
				{
					tv_Date.Text = _OrderList[position].HistInstalDate;

					var amount = _OrderList[position].HistInstalAmount;
					tv_Amount.Text = MoneyFormat.Convert(decimal.Parse(amount));

					iv_Status.SetBackgroundResource(Resource.Drawable.red);

				}

				if (position % 2 == 0)
				{
					linearLayout.SetBackgroundColor(Color.ParseColor("#EDEDED"));
				}
				else
				{
					linearLayout.SetBackgroundColor(Color.ParseColor("#FFFFFF"));
				}
            }
            return view;
        }

        public List<PaymentTrackerModel> GetPaymentTrackerList()
        {
            return _OrderList;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private class PaymentTrackerFilter : Filter
        {
            private readonly PaymentTrackerAdapter _adapter;
            public PaymentTrackerFilter(PaymentTrackerAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();

                var results = new List<PaymentTrackerModel>();

                if (_adapter._originalData == null)
                    _adapter._originalData = _adapter._OrderList;

                if (constraint == null) return returnObj;

                if (_adapter._originalData != null && _adapter._originalData.Any())
                {
                    results.AddRange(_adapter._originalData.Where(t => t.HistInstalDate.ToLower().Contains(constraint.ToString().ToLower())));
                }

                // Nasty piece of .NET to Java wrapping, be careful with this!
                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter._OrderList = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<PaymentTrackerModel>()).ToList();
                _adapter.NotifyDataSetChanged();

                // Don't do this and see GREF counts rising
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}


