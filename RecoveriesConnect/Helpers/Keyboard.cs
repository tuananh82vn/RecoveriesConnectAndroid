using Android.App;
using Android.Widget;
using Android.Content;
using Android.Views.InputMethods;

namespace RecoveriesConnect
{
	public static class Keyboard
	{
		public static void ShowKeyboard(Activity activity, EditText pView)
		{
			pView.Focusable = true;
			pView.FocusableInTouchMode = true;
			pView.RequestFocus();

			InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
			inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
		}

		public static void HideKeyboard(Activity activity, EditText pView)
		{
			InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
			inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
		}

		public static void HideSoftKeyboard(Activity activity)
		{
			var view = activity.CurrentFocus;
			if (view != null)
			{
				InputMethodManager manager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
				manager.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
			}
		}
	}
}

