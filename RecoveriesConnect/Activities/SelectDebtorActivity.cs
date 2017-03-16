using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Android.Content;
using Android.Content.PM;
using RecoveriesConnect.Models.Api;
using System.Collections.Generic;
using System.Linq;
using RecoveriesConnect.Adapter;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "SelectDebtor", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.ThemeCustomNoActionBar", NoHistory = true)]
    public class SelectDebtorActivity : Activity
    {
        List<CoDebtorModel> CoDebtorList;
		int selectedIndex = 0;

        List<CoDebtorModel> SelectedDebtorList;

        public Spinner spinner_Debtor;

        public DebtorSpinnerAdapter DebtorAdapter;

        

        public Button bt_Continue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SelectDebtor);

            spinner_Debtor = FindViewById<Spinner>(Resource.Id.spinner_Debtor);

            bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);
            bt_Continue.Click += Bt_Continue_Click;


            LoadDebtorList();

        }

        private void Bt_Continue_Click(object sender, EventArgs e)
        {
            if (this.CoDebtorList[this.selectedIndex].mobile == "No Number")
            {

                Intent Intent = new Intent(this, typeof(VerifyDetailActivity));

                SelectedDebtorList = new List<CoDebtorModel>(1);

                SelectedDebtorList.Add(this.CoDebtorList[this.selectedIndex]);

                Intent.PutParcelableArrayListExtra("SelectedDebtor", SelectedDebtorList.ToArray());

                Intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

                StartActivity(Intent);

            }
            else
            {
                Intent Intent = new Intent(this, typeof(VerifyCoDebtorActivity));
                StartActivity(Intent);
            }
        }

        public void LoadDebtorList()
        {
            var items = Intent.GetParcelableArrayListExtra("codebtor");
            if (items != null)
            {

                items = items.Cast<CoDebtorModel>().ToArray();

                CoDebtorList = new List<CoDebtorModel>();

                foreach (CoDebtorModel item in items)
                {

                    CoDebtorModel order = new CoDebtorModel();
                    order.debtorCode = item.debtorCode;
                    order.fullName = item.fullName;
                    order.mobile = item.mobile;
                    order.markMobile = item.markMobile;
                    CoDebtorList.Add(order);
                }
            }


            DebtorAdapter = new DebtorSpinnerAdapter(this, this.CoDebtorList);

            spinner_Debtor.Adapter = DebtorAdapter;

            spinner_Debtor.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Debtor_ItemSelected);
        }

        private void Debtor_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            selectedIndex = e.Position;
        }
    }
}