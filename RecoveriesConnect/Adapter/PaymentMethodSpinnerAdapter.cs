using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

namespace RecoveriesConnect.Adapter
{
	public class PaymentMethodSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		private readonly Activity _context;
		private List<string> _DebtorList;

		private readonly IList<View> _views = new List<View>();

		public PaymentMethodSpinnerAdapter(Activity context, List<string> data)
		{
			_context = context;
			_DebtorList = data;
		}

		public string GetItemAtPosition(int position)
		{
			return _DebtorList.ElementAt(position);
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int id)
		{
			return id;
		}

		public override int Count
		{
			get
			{
				return _DebtorList == null ? 0 : _DebtorList.Count();
			}
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			string item = _DebtorList.ElementAt(position);

			var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.SpinnerItemDropdown, parent, false);

			var text = view.FindViewById<TextView>(Resource.Id.text);

			if (text != null)
				text.Text = item;

			return view;
		}

		private void ClearViews()
		{
			foreach (var view in _views)
			{
				view.Dispose();
			}
			_views.Clear();
		}

		protected override void Dispose(bool disposing)
		{
			ClearViews();
			base.Dispose(disposing);
		}
	}
}

