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
	public class InboxAdapter : BaseAdapter<Inbox>, IFilterable
    {

        private List<Inbox> _originalData;
        private List<Inbox> _OrderList;
        public Filter Filter { get; private set; }

        private int[] mAlternatingColors;

        Activity _activity;

        private string type;

        public InboxAdapter(Activity activity, List<Inbox> data)
        {
            _activity = activity;
            _OrderList = data;


            Filter = new InboxFilter(this);

            mAlternatingColors = new int[] { 0xF2F2F2, 0xC3C3C3 };
        }

        public override Inbox this[int position]
        {
            get { return _OrderList[position]; }
        }

        public Inbox GetItemAtPosition(int position)
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
				return long.Parse(_OrderList[position].ID.ToString());
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
				view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NoInbox, null, false);
            }
            else
            {
				view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.InboxList, parent, false);
                var tv_Date = view.FindViewById<TextView>(Resource.Id.tv_Date);
				var tv_Type = view.FindViewById<TextView>(Resource.Id.tv_Type);
                var iv_Status = view.FindViewById<ImageView>(Resource.Id.iv_Status);
                var linearLayout = view.FindViewById<LinearLayout>(Resource.Id.layout_middle);

				tv_Date.Text = _OrderList[position].Date;

				if (_OrderList[position].Type == "T")
				{
					tv_Type.Text = "Messsage";
				}
				else if (_OrderList[position].Type == "D")
				{
					tv_Type.Text = "Letter";
				}
				else if (_OrderList[position].Type == "P")
				{
					tv_Type.Text = "Payment";
				}
				else if (_OrderList[position].Type == "R")
				{
					tv_Type.Text = "Receipt";
				}

				if (_OrderList[position].Status == "Unread")
				{
					iv_Status.SetBackgroundResource(Resource.Drawable.red);
				}
				else
				{
					iv_Status.SetBackgroundResource(Resource.Drawable.blue);
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

        public List<Inbox> GetInboxList()
        {
            return _OrderList;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private class InboxFilter : Filter
        {
            private readonly InboxAdapter _adapter;
            public InboxFilter(InboxAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();

                var results = new List<Inbox>();

                if (_adapter._originalData == null)
                    _adapter._originalData = _adapter._OrderList;

                if (constraint == null) return returnObj;

                if (_adapter._originalData != null && _adapter._originalData.Any())
                {
					results.AddRange(_adapter._originalData.Where(t => t.Date.ToLower().Contains(constraint.ToString().ToLower())));
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
                        .Select(r => r.ToNetObject<Inbox>()).ToList();
                _adapter.NotifyDataSetChanged();

                // Don't do this and see GREF counts rising
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}


