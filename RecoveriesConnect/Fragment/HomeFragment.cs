using System;
using System.Collections.Generic;

using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Widget;
using RecoveriesConnect.Helpers;
using RecoveriesConnect.Activities;
using AndroidHUD;

namespace RecoveriesConnect.Fragment
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {
        TextView tv_AccountNumber;
        TextView tv_OurClient;
        TextView tv_RefNumber;
        TextView tv_OutStanding;
        TextView tv_NextInstalment;

        LinearLayout ln_make_payment;
        LinearLayout ln_payment_tracker;
        LinearLayout ln_installment_info;

        LinearLayout ln_defer_payment;
        LinearLayout ln_schedule_callback;
        LinearLayout ln_inbox;


		LinearLayout ll_NextInstalment;


		public HomeFragment(Activity context)
        {
            this.RetainInstance = true;
        }
        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            this.HasOptionsMenu = true;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.MainContentLayout2, null);

            tv_AccountNumber = view.FindViewById<TextView>(Resource.Id.tv_ClientAccountNumber);
            tv_AccountNumber.Text = Settings.ClientAccountNumber;

            tv_OurClient = view.FindViewById<TextView>(Resource.Id.tv_OurClient);
            tv_OurClient.Text = Settings.OurClient;

            tv_RefNumber = view.FindViewById<TextView>(Resource.Id.tv_RefNumber);
            tv_RefNumber.Text = Settings.RefNumber;

            tv_OutStanding = view.FindViewById<TextView>(Resource.Id.tv_OutStanding);
            tv_OutStanding.Text = MoneyFormat.Convert(Settings.TotalOutstanding);


            tv_NextInstalment = view.FindViewById<TextView>(Resource.Id.tv_NextInstalment);

			ll_NextInstalment = view.FindViewById<LinearLayout>(Resource.Id.ll_NextInstalment_0);

			if (Settings.NextPaymentInstallment > 0)
			{
				tv_NextInstalment.Text = MoneyFormat.Convert(Settings.NextPaymentInstallment);
			}
			else
			{
				ll_NextInstalment.Visibility = ViewStates.Invisible;
			}


            ln_make_payment = view.FindViewById<LinearLayout>(Resource.Id.ln_make_payment);
            ln_make_payment.Touch += ln_make_paymentTouch;

            ln_payment_tracker = view.FindViewById<LinearLayout>(Resource.Id.ln_payment_tracker);
            ln_payment_tracker.Touch += Ln_payment_tracker_Touch;

            ln_installment_info = view.FindViewById<LinearLayout>(Resource.Id.ln_instalment_info);
            ln_installment_info.Touch += Ln_installment_Info;

            ln_defer_payment = view.FindViewById<LinearLayout>(Resource.Id.ln_defer_payment);
            ln_defer_payment.Touch += Ln_defer_payment_Touch;

            ln_schedule_callback = view.FindViewById<LinearLayout>(Resource.Id.ln_schedule_callback);
            ln_schedule_callback.Touch += Ln_schedule_callback_Touch;

            ln_inbox = view.FindViewById<LinearLayout>(Resource.Id.ln_inbox);
            ln_inbox.Touch += Ln_inbox_Touch;

			return view;
        }

		private void Ln_inbox_Touch(object sender, View.TouchEventArgs touchEventArgs)
		{
			switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
			{
				case MotionEventActions.Down:

				case MotionEventActions.Move:

					ln_inbox.SetBackgroundColor(Color.ParseColor("#3D984E"));
					break;

				case MotionEventActions.Up:

					ln_inbox.SetBackgroundColor(Color.ParseColor("#006571"));

					AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

					Intent inbox = new Intent(this.Activity, typeof(InboxActivity));
					StartActivity(inbox);

					AndHUD.Shared.Dismiss();

					break;

				default:
					break;
			}
		}

		private void Ln_schedule_callback_Touch(object sender, View.TouchEventArgs touchEventArgs)
		{
			switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
			{
				case MotionEventActions.Down:

				case MotionEventActions.Move:

					ln_schedule_callback.SetBackgroundColor(Color.ParseColor("#3D984E"));
					break;

				case MotionEventActions.Up:

					ln_schedule_callback.SetBackgroundColor(Color.ParseColor("#006571"));

					AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

					Intent schedule_callback = new Intent(this.Activity, typeof(ScheduleCallbackActivity));
					StartActivity(schedule_callback);

					AndHUD.Shared.Dismiss();


					break;

				default:
					break;
			}
		}

		private void Ln_defer_payment_Touch(object sender, View.TouchEventArgs touchEventArgs)
		{
			switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
			{
				case MotionEventActions.Down:

				case MotionEventActions.Move:

					ln_defer_payment.SetBackgroundColor(Color.ParseColor("#3D984E"));
					break;

				case MotionEventActions.Up:

					ln_defer_payment.SetBackgroundColor(Color.ParseColor("#006571"));

					AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

					Intent defer_payment = new Intent(this.Activity, typeof(DeferPaymentActivity));
					StartActivity(defer_payment);

					AndHUD.Shared.Dismiss();

					break;

				default:
					break;
			}
		}

        private void ln_make_paymentTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:

                case MotionEventActions.Move:

                    ln_make_payment.SetBackgroundColor(Color.ParseColor("#3D984E"));
                    break;

                case MotionEventActions.Up:

                    ln_make_payment.SetBackgroundColor(Color.ParseColor("#006571"));

					//SetPayment.Set("other");

					AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

					Intent make_payment = new Intent(this.Activity, typeof(MakePaymentActivity));
                    StartActivity(make_payment);

					AndHUD.Shared.Dismiss();

					break;

                default:
                    break;
            }
        }

        private void Ln_payment_tracker_Touch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:

                case MotionEventActions.Move:

                    ln_payment_tracker.SetBackgroundColor(Color.ParseColor("#3D984E"));
                    break;

                case MotionEventActions.Up:

                    ln_payment_tracker.SetBackgroundColor(Color.ParseColor("#006571"));

					AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

					Intent payment_tracker = new Intent(this.Activity, typeof(PaymentTrackerActivity));
                    StartActivity(payment_tracker);

					AndHUD.Shared.Dismiss();


					break;

                default:
                    break;
            }
        }

		private void Ln_installment_Info(object sender, View.TouchEventArgs touchEventArgs)
		{
			switch (touchEventArgs.Event.Action & MotionEventActions.Mask)
			{
				case MotionEventActions.Down:

				case MotionEventActions.Move:

					this.ln_installment_info.SetBackgroundColor(Color.ParseColor("#3D984E"));
					break;

				case MotionEventActions.Up:

					ln_installment_info.SetBackgroundColor(Color.ParseColor("#006571"));

					AndHUD.Shared.Show(this.Activity, "Please wait ...", -1, MaskType.Clear);

					Intent instalment_info = new Intent(this.Activity, typeof(InstalmentInfoActivity));
					StartActivity(instalment_info);

					AndHUD.Shared.Dismiss();

					break;

				default:
					break;
			}
        }



    }
}