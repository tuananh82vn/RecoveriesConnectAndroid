using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RecoveriesConnect.Helpers;

namespace RecoveriesConnect.Adapter
{
    public class MenuListAdapter : BaseAdapter
    {
        Activity context;

        public List<MenuItem> items;

        public MenuListAdapter(Activity context)
            : base()
        {
            this.context = context;

			this.items = new List<MenuItem>() {
				new MenuItem() { Name = "Main",     Img = Resource.Drawable.make_payment ,  Type="divider"}, //0
				new MenuItem() { Name = "Provide Feedback",     Img = Resource.Drawable.Quote ,  Type="item" }, //1
                new MenuItem() { Name = "About",     Img = Resource.Drawable.instalment_info ,  Type="item" }, //2
                new MenuItem() { Name = "Contact Us",     Img = Resource.Drawable.Phone ,  Type="item" }, //3
				new MenuItem() { Name = "Settings",     Img = Resource.Drawable.make_payment ,  Type="divider"}, //4
				new MenuItem() { Name = "View/Update Credit Card",     Img = Resource.Drawable.card ,  Type="item" }, //5
				new MenuItem() { Name = "View/Update Bank Account",     Img = Resource.Drawable.bank ,  Type="item" }, //6
				new MenuItem() { Name = "View/Update Information",     Img = Resource.Drawable.personal ,  Type="item" }, //7
			};

            //if (Settings.IsExistingArrangement || Settings.IsExistingArrangementCC || Settings.IsExistingArrangementDD)
            //{

            //new MenuItem() { Name = "Main", Img = Resource.Drawable.make_payment, Type = "divider" }, //0
            //new MenuItem() { Name = "Pay In Full", Img = Resource.Drawable.make_payment, Type = "item" }, //1
            //new MenuItem() { Name = "Provide Feedback", Img = Resource.Drawable.instalment_info, Type = "item" }, //2
            //new MenuItem() { Name = "Settings", Img = Resource.Drawable.make_payment, Type = "divider" }, //3
            //new MenuItem() { Name = "View/Update Credit Card", Img = Resource.Drawable.card, Type = "item" }, //4
            //new MenuItem() { Name = "View/Update Bank Account", Img = Resource.Drawable.bank, Type = "item" }, //5
            //new MenuItem() { Name = "View/Update Information", Img = Resource.Drawable.personal, Type = "item" }, //6

            //	this.items.RemoveAt(2);
            //}
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            var view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.NavigationMenu, parent, false)) as LinearLayout;



            if (item.Type.Equals("item"))
            {

                var menutxt = view.FindViewById(Resource.Id.tv_MenuName) as TextView;

                var menuimg = view.FindViewById(Resource.Id.imageView_Menu) as ImageView;

                menuimg.SetImageResource(item.Img);

                menutxt.SetText(item.Name, TextView.BufferType.Normal);
                menutxt.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                menutxt.Gravity = GravityFlags.CenterVertical;

            }
            else if (item.Type.Equals("divider"))
            {
                var menutxt = view.FindViewById(Resource.Id.tv_MenuName) as TextView;

                var menuimg = view.FindViewById(Resource.Id.imageView_Menu) as ImageView;
                menuimg.SetImageResource(Android.Resource.Color.Transparent);

                menutxt.SetText(item.Name, TextView.BufferType.Normal);
                menutxt.Gravity = GravityFlags.CenterVertical;
                menutxt.SetTextColor(Android.Graphics.Color.Aquamarine);
            }

            return view;
        }
    }

    public class MenuItem
    {
        public MenuItem()
        {

        }

        public MenuItem(string name, int img, string type)
        {
            this.Name = name;
            this.Img = img;
            this.Type = type;
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public int Img { get; set; }
    }
}