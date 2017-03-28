using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using RecoveriesConnect.Helpers;
using System;
using Com.Telerik.Widget.Chart.Visualization.PieChart;
using Com.Telerik.Widget.Chart.Engine.Databinding;
using Com.Telerik.Widget.Primitives.Legend;
using Com.Telerik.Widget.Chart.Engine.DataPoints;
using Com.Telerik.Widget.Palettes;
using System.Collections.Generic;
using AndroidHUD;

namespace RecoveriesConnect.Activities
{
	[Activity(Label = "InstalmentInfo", LaunchMode = LaunchMode.SingleTop, Theme = "@style/Theme.Themecustom")]
	public class InstalmentInfoActivity : Activity
    {
		public TextView tv_Amount;
		public TextView tv_Frequency;
		public TextView tv_Paid;
		public TextView tv_Remaining;
		public TextView tv_Status;
		public TextView tv_Overdue;
		public TextView tv_NextPayment;

		public LinearLayout ln_root;
		public LinearLayout ln_top;

		public LinearLayout ln_Chart;
		public LinearLayout ln_BottomChart;

		public LinearLayout ln_left;
		public LinearLayout ln_right;

		Alert alert;

		private Java.Util.ArrayList monthResults;
		private RadPieChartView pieChartView;

		private bool IsBlue = false;
		private bool IsGreen = false;
		private bool IsRed = false;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.ActionBar);

			SetContentView(Resource.Layout.InstalmentInfo);


			//**************************************************//

			ActionBar.NavigationMode = ActionBarNavigationMode.Standard;

			var upArrow = Resources.GetDrawable(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
			upArrow.SetColorFilter(Color.ParseColor("#006571"), PorterDuff.Mode.SrcIn);
			ActionBar.SetHomeAsUpIndicator(upArrow);

			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			LinearLayout lLayout = new LinearLayout(this);
			lLayout.SetGravity(GravityFlags.CenterVertical);
			LinearLayout.LayoutParams textViewParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			textViewParameters.RightMargin = (int)(30 * this.Resources.DisplayMetrics.Density);

			TextView myTitle = new TextView(this);
			myTitle.Text = "Instalment Info";
			myTitle.TextSize = 20;
			myTitle.Gravity = GravityFlags.Center;
			lLayout.AddView(myTitle, textViewParameters);

			ActionBar.LayoutParams actionbarParams = new ActionBar.LayoutParams(ActionBar.LayoutParams.MatchParent, ActionBar.LayoutParams.MatchParent);
			ActionBar.SetCustomView(lLayout, actionbarParams);
			ActionBar.SetDisplayShowCustomEnabled(true);

			//**************************************************//

			// Create your application here

			tv_Amount = FindViewById<TextView>(Resource.Id.tv_Amount);
		  	tv_Frequency = FindViewById<TextView>(Resource.Id.tv_Frequency); 
		  	tv_Paid = FindViewById<TextView>(Resource.Id.tv_Paid);
		  	tv_Remaining = FindViewById<TextView>(Resource.Id.tv_Remaining);
		  	tv_Status = FindViewById<TextView>(Resource.Id.tv_Status);
		  	tv_Overdue = FindViewById<TextView>(Resource.Id.tv_Overdue);
		  	tv_NextPayment = FindViewById<TextView>(Resource.Id.tv_NextPayment);

			ln_Chart = FindViewById<LinearLayout>(Resource.Id.ln_Chart);
			ln_right = FindViewById<LinearLayout>(Resource.Id.ln_right);
			ln_top = FindViewById<LinearLayout>(Resource.Id.ln_top);
			ln_left = FindViewById<LinearLayout>(Resource.Id.ln_left);
			ln_root = FindViewById<LinearLayout>(Resource.Id.ln_root);
			ln_BottomChart= FindViewById<LinearLayout>(Resource.Id.ln_BottomChart);

			LoadData();

			TrackingHelper.SendTracking("Open Instalment Info");


		}

		private void LoadData()
		{

			AndHUD.Shared.Show(this, "Please wait ...", -1, MaskType.Clear);

			string url = Settings.InstanceURL;
			var url2 = url + "/Api/GetArrangeDetails";

			var json = new
			{
				Item = new
				{
					ReferenceNumber = Settings.RefNumber
				}
			};

			try
			{
				string results = ConnectWebAPI.Request(url2, json);



				if (string.IsNullOrEmpty(results))
				{
                    AndHUD.Shared.Dismiss();
                    this.RunOnUiThread(() => alert = new Alert(this, "Error", Resources.GetString(Resource.String.NoServer)));
                    this.RunOnUiThread(() => alert.Show());
                }

				var ObjectReturn = new ArrangeDetails();


				ObjectReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<ArrangeDetails>(results);

				AndHUD.Shared.Dismiss();

				if (ObjectReturn.IsSuccess)
				{
					this.tv_Amount.Text = MoneyFormat.Convert(ObjectReturn.ArrangeAmount);
					this.tv_Paid.Text = MoneyFormat.Convert(ObjectReturn.PaidAmount);
					this.tv_Status.Text = ObjectReturn.Status;
					this.tv_Overdue.Text = MoneyFormat.Convert(ObjectReturn.OverdueAmount);
					this.tv_Frequency.Text = ObjectReturn.Frequency;
					this.tv_Remaining.Text = MoneyFormat.Convert(ObjectReturn.LeftToPay);
					this.tv_NextPayment.Text = ObjectReturn.NextInstalmentDate;

					Settings.TotalPaid = ObjectReturn.PaidAmount;
					Settings.TotalOverDue = ObjectReturn.OverdueAmount;

					pieChartView = this.createChart();
					ln_Chart.AddView(pieChartView);
					ln_right.AddView(this.createLegend());

				}
				else
				{
					//Hide Top View
					var params_Top = this.ln_top.LayoutParameters;
					params_Top.Height = 0;
					this.ln_top.LayoutParameters = params_Top;
					ln_top.Visibility = ViewStates.Invisible;

					//Hide Left View
					LinearLayout.LayoutParams params_Left = new LinearLayout.LayoutParams(0, 0, 0);
					this.ln_left.LayoutParameters = params_Left;
					ln_left.Visibility = ViewStates.Invisible;

					//Add label into Middle View
					var aLabel = new TextView(this);
					aLabel.Text = Resources.GetString(Resource.String.NoArrangement);
					aLabel.Gravity = GravityFlags.Center;
					aLabel.SetTextColor(Color.ParseColor("#006571"));
					aLabel.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);

					this.ln_Chart.AddView(aLabel);

					//Add button into Left View
					var aButton = new Button(this);
					aButton.Text = "Make a Payment";
					aButton.SetTextColor(Color.White);
					aButton.SetBackgroundColor(Color.ParseColor("#006571"));
					aButton.Click += Bt_Make_Payment_Click;

					//Make this Left view big as parent
					this.ln_right.AddView(aButton);
					this.ln_right.WeightSum = 0;	

					var param_Bottom = this.ln_BottomChart.LayoutParameters;
					param_Bottom.Height = 0;
					this.ln_BottomChart.LayoutParameters = param_Bottom;
				}
			}
			catch (Exception ee)
			{
				AndHUD.Shared.Dismiss();
			}

		}

		private void Bt_Make_Payment_Click(object sender, EventArgs e)
		{
			SetPayment.Set("other");
			Intent make_payment = new Intent(this, typeof(MakeCCPaymentActivity));
			StartActivity(make_payment);
		}

		private void InitChartData()
		{
			monthResults = new Java.Util.ArrayList();

			if (Settings.TotalPaid > 0)
			{
				this.IsGreen = true;
				monthResults.Add(new MonthResult("Paid", double.Parse(Settings.TotalPaid.ToString())));
			}
			if (Settings.TotalOutstanding > 0)
			{
				this.IsBlue = true;
				monthResults.Add(new MonthResult("Remaining", double.Parse(Settings.TotalOutstanding.ToString())));
			}
			if (Settings.TotalOverDue > 0)
			{
				this.IsRed = true;
				monthResults.Add(new MonthResult("Overdue", double.Parse(Settings.TotalOverDue.ToString())));
			}
		}

		private RadLegendView createLegend()
		{
			RadLegendView legendView = new RadLegendView(this);
			legendView.LegendProvider = this.pieChartView;

			return legendView;
		}

		public class ValueToStringConverter : Java.Lang.Object, Com.Telerik.Android.Common.IFunction
		{
			public Java.Lang.Object Apply(Java.Lang.Object arg)
			{
				Java.Lang.Double value = (Java.Lang.Double)arg;
				return string.Format("{0:C}", value.DoubleValue());
			}
		}


		private RadPieChartView createChart()
		{

			InitChartData();

			RadPieChartView pieChartView = new RadPieChartView(this);

			CustomPieSeries pieSeries = new CustomPieSeries();
			pieSeries.ValueBinding = new MonthResultDataBinding("Result");
			pieSeries.Data = (Java.Lang.IIterable)this.monthResults;
			pieSeries.ShowLabels = true;
			pieSeries.LabelValueToStringConverter = new ValueToStringConverter();
			pieSeries.LabelOffset = -50;

			ChartPalette customPalette = pieChartView.Palette;

			PaletteEntry paid = customPalette.GetEntry(ChartPalette.PieFamily, 0);
			paid.Fill = Color.ParseColor("#00757D");

			PaletteEntry remaining = customPalette.GetEntry(ChartPalette.PieFamily, 1);
			remaining.Fill = Color.ParseColor("#75c283");

			PaletteEntry overdue = customPalette.GetEntry(ChartPalette.PieFamily, 2);
			overdue.Fill = Color.ParseColor("#f14844");

			//pieChartView.Palette  = customPalette;


			SliceStyle blueStyle = new SliceStyle();
			blueStyle.FillColor = Color.ParseColor("#00757D");
			blueStyle.StrokeColor = Color.ParseColor("#00757D");
			blueStyle.StrokeWidth = 2;
			blueStyle.ArcColor = Color.White;
			blueStyle.ArcWidth = 2;

			SliceStyle greenStyle = new SliceStyle();
			greenStyle.FillColor = Color.ParseColor("#75c283");
			greenStyle.StrokeColor = Color.ParseColor("#75c283");
			greenStyle.StrokeWidth = 2;
			greenStyle.ArcColor = Color.White;
			greenStyle.ArcWidth = 2;

			SliceStyle redStyle = new SliceStyle();
			redStyle.FillColor = Color.ParseColor("#f14844");
			redStyle.StrokeColor = Color.ParseColor("#f14844");
			redStyle.StrokeWidth = 2;
			redStyle.ArcColor = Color.White;
			redStyle.ArcWidth = 2;

			List<SliceStyle> styles = new List<SliceStyle>();


			if (this.IsBlue && this.IsGreen && this.IsRed)
			{
				styles.Add(blueStyle);
				styles.Add(greenStyle);
				styles.Add(redStyle);
			}
			else if (this.IsBlue && this.IsGreen)
			{
				styles.Add(blueStyle);
				styles.Add(greenStyle);
			}
			else if (this.IsBlue && this.IsRed)
			{
				styles.Add(blueStyle);
				styles.Add(redStyle);
			}
			else if (this.IsGreen && this.IsRed)
			{
				styles.Add(greenStyle);
				styles.Add(redStyle);
			}
			else if (this.IsBlue)
			{
				styles.Add(blueStyle);
			}
			else if (this.IsGreen)
			{
				styles.Add(greenStyle);
			}
			else if (this.IsRed)
			{
				styles.Add(redStyle);
			}

			pieSeries.SliceStyles = styles;

			pieChartView.Series.Add(pieSeries);
			return pieChartView;
		}


		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			base.OnOptionsItemSelected(item);

			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					Keyboard.HideSoftKeyboard(this);
					OnBackPressed();
					break;
				default:
					break;
			}

			return true;
		}

    }

	public class MonthResultDataBinding : DataPointBinding
	{

		private string propertyName;

		public MonthResultDataBinding(string propertyName)
		{
			this.propertyName = propertyName;
		}

		public override Java.Lang.Object GetValue(Java.Lang.Object p0)
		{
			if (propertyName == "Month")
			{
				return ((MonthResult)(p0)).Month;
			}
			return ((MonthResult)(p0)).Result;
		}
	}

	public class MonthResult : Java.Lang.Object
	{

		public double Result { get; set; }
		public String Month { get; set; }

		public MonthResult(String month, double result)
		{
			this.Month = month;
			this.Result = result;
		}
	}


	public class CustomPieSeries: PieSeries
	{
		protected override String GetLegendTitle(PieDataPoint point)
		{
			return ((MonthResult)point.DataItem).Month;
		}
	}


}