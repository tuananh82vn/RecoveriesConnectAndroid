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
	public class InstalmentSummaryAdapter : BaseAdapter<InstalmentSummaryModel>, IFilterable
	{

		private List<InstalmentSummaryModel> _originalData;
		private List<InstalmentSummaryModel> _OrderList;
		public Filter Filter { get; private set; }

		private int[] mAlternatingColors;

		Activity _activity;

		private string type;

		public InstalmentSummaryAdapter(Activity activity, List<InstalmentSummaryModel> data)
		{
			_activity = activity;
			_OrderList = data;

			Filter = new InstalmentSummaryFilter(this);

			mAlternatingColors = new int[] { 0xF2F2F2, 0xC3C3C3 };
		}

		public override InstalmentSummaryModel this[int position]
		{
			get { return _OrderList[position]; }
		}

		public InstalmentSummaryModel GetItemAtPosition(int position)
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
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view;
			if (this._OrderList.Count == 0)
			{
				view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NoPaymentTrackerItem1, null, false);
			}
			else
			{
				view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.InstalmentSummaryList, parent, false);
				var tv_Date = view.FindViewById<TextView>(Resource.Id.tv_Date);
				var tv_Amount = view.FindViewById<TextView>(Resource.Id.tv_Amount);

				var linearLayout = view.FindViewById<LinearLayout>(Resource.Id.layout_middle);

				tv_Date.Text = _OrderList[position].PaymentDate;

				var amount = _OrderList[position].Amount;

				tv_Amount.Text = MoneyFormat.Convert(decimal.Parse(amount.ToString()));


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

		public List<InstalmentSummaryModel> GetPaymentTrackerList()
		{
			return _OrderList;
		}

		private Color GetColorFromInteger(int color)
		{
			return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
		}

		private class InstalmentSummaryFilter : Filter
		{
			private readonly InstalmentSummaryAdapter _adapter;
			public InstalmentSummaryFilter(InstalmentSummaryAdapter adapter)
			{
				_adapter = adapter;
			}

			protected override FilterResults PerformFiltering(ICharSequence constraint)
			{
				var returnObj = new FilterResults();

				var results = new List<InstalmentSummaryModel>();

				if (_adapter._originalData == null)
					_adapter._originalData = _adapter._OrderList;

				if (constraint == null) return returnObj;

				if (_adapter._originalData != null && _adapter._originalData.Any())
				{
					results.AddRange(_adapter._originalData.Where(t => t.PaymentDate.ToLower().Contains(constraint.ToString().ToLower())));
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
						.Select(r => r.ToNetObject<InstalmentSummaryModel>()).ToList();
				_adapter.NotifyDataSetChanged();

				// Don't do this and see GREF counts rising
				constraint.Dispose();
				results.Dispose();
			}
		}
	}
}


