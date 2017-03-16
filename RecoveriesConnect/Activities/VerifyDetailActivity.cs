using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Models.Api;
using Android.Content.PM;
using System;
using System.Globalization;

namespace RecoveriesConnect.Activities
{
    [Activity(Label = "VerifyDetail", NoHistory = true, LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.ThemeCustomNoActionBar")]
    public class VerifyDetailActivity : Activity
    {
        public DebtorInfoModel selectedDebtor;
        public DateTime DateOfBirth;
        public Button bt_Continue;

        public EditText et_FullName;
        public TextView err_FullName;

        public EditText et_DateOfBirth;
        public TextView err_DateOfBirth;

        public EditText et_PostCode;
        public TextView err_PostCode;

        const int Start_DATE_DIALOG_ID = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VerifyDetail);

            InitLayout();

            LoadDebtorList();

            LoadAdditinalInfomation();
       }

        private void InitLayout()
        {
            bt_Continue = FindViewById<Button>(Resource.Id.bt_Continue);


            et_FullName = FindViewById<EditText>(Resource.Id.et_FullName);
            et_DateOfBirth = FindViewById<EditText>(Resource.Id.et_DateOfBirth);
            et_PostCode = FindViewById<EditText>(Resource.Id.et_PostCode);

            err_FullName = FindViewById<TextView>(Resource.Id.err_FullName);
            err_DateOfBirth = FindViewById<TextView>(Resource.Id.err_DateOfBirth);
            err_PostCode = FindViewById<TextView>(Resource.Id.err_PostCode);

            bt_Continue.Click += Bt_Continue_Click;
            et_DateOfBirth.Click += delegate { ShowDialog(Start_DATE_DIALOG_ID); };

            DateOfBirth = DateTime.Today;

        }

        private void Bt_Continue_Click(object sender, EventArgs e)
        {
            bool IsValidate1 = true;
            bool IsValidate2 = true;
            bool IsValidate3 = true;

            err_FullName.Text = "";
            err_DateOfBirth.Text = "";
            err_PostCode.Text = "";

            var Fullname = et_FullName.Text;
            if (string.IsNullOrEmpty(Fullname))
            {
                IsValidate1 = false;
                this.err_FullName.Text = "Please enter Full Name";

            }
            if (!this.selectedDebtor.FullName.Equals(Fullname.ToUpper()))
            {
                IsValidate1 = false;
                this.err_FullName.Text = "Full Name is not correct";
            }


            var DateOfBirth = et_DateOfBirth.Text;
            if (string.IsNullOrEmpty(DateOfBirth))
            {
                IsValidate2 = false;
                this.err_DateOfBirth.Text = "Please enter Date Of Birth";
            }
            else
            {
                if (!string.IsNullOrEmpty(this.selectedDebtor.DateOfBirths))
                {
                    DateTime dob;
                    bool isvalid = DateTime.TryParseExact(this.selectedDebtor.DateOfBirths, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
                    if (isvalid)
                    {
                        if (!dob.Equals(DateTime.ParseExact(DateOfBirth,"dd/MM/yyyy", CultureInfo.InvariantCulture)))
                        {
                            IsValidate2 = false;
                            this.err_DateOfBirth.Text = "Date Of Birth is not correct";
                        }
                    }
                }
            }

                var PostCode = et_PostCode.Text;
            if (string.IsNullOrEmpty(PostCode))
            {
                IsValidate3 = false;
                this.err_PostCode.Text = "Please enter Post Code";

            }
            if (!this.selectedDebtor.PostCodes.Equals(PostCode))
            {
                IsValidate3 = false;
                this.err_FullName.Text = "Post Code is not correct";
            }

            if (IsValidate1 && IsValidate2 && IsValidate3)
            {
                Settings.IsCoBorrowerSelected = true;
                Settings.DebtorCodeSelected = this.selectedDebtor.DebtorCode;
                if (this.selectedDebtor.DebtorCode.Equals(Settings.ArrangementDebtor))
                {
                    Settings.IsArrangementUnderThisDebtor = true;
                }
                else
                {
                    Settings.IsArrangementUnderThisDebtor = false;
                }

				TrackingHelper.SendTracking("Verify Detail");

				Intent intent = new Intent(this, typeof(SetupPinActivity));
                StartActivity(intent);

            }
        }

        public void LoadDebtorList()
        {
            var items = Intent.GetParcelableArrayListExtra("SelectedDebtor");
            selectedDebtor = new DebtorInfoModel();
            if (items != null)
            {

                items = items.Cast<CoDebtorModel>().ToArray();

                foreach (CoDebtorModel item in items)
                {
                    selectedDebtor.DebtorCode = item.debtorCode;
                    selectedDebtor.FullName = item.fullName;
                }
            }
        }

        void OnStartDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            et_DateOfBirth.Text = e.Date.ToString("dd'/'MM'/'yyyy");
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case Start_DATE_DIALOG_ID:
                    return new DatePickerDialog(this, OnStartDateSet, DateOfBirth.Year, DateOfBirth.Month - 1, DateOfBirth.Day);
            }
            return null;
        }

        public void LoadAdditinalInfomation()
        {
            string url = Settings.InstanceURL;

            var url1 = url + "/Api/GetDebtorAdditionalInfor";

            var json = new
            {
                Item = new
                {
                    DebtorCode = selectedDebtor.DebtorCode,
                    ReferenceNumber = Settings.RefNumber,
                }
            };

            var ObjectReturn = new DebtorInfoModel();

            string results = ConnectWebAPI.Request(url1, json);

            ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<DebtorInfoModel>(results);
            if (ObjectReturn.IsSuccess)
            {
                this.selectedDebtor.PostCodes = ObjectReturn.PostCodes;
                this.selectedDebtor.RegNumbers = ObjectReturn.RegNumbers;
                this.selectedDebtor.Address1s = ObjectReturn.Address1s;
                this.selectedDebtor.Address2s = ObjectReturn.Address2s;
                this.selectedDebtor.Address3s = ObjectReturn.Address3s;
                this.selectedDebtor.Suburbs = ObjectReturn.Suburbs;
                this.selectedDebtor.States = ObjectReturn.States;
                this.selectedDebtor.DateOfBirths = ObjectReturn.DateOfBirths;
            }
        }
    }
}
