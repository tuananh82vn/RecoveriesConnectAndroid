using System;
using Android.Graphics;

namespace RecoveriesConnect.Helpers
{
	public static class ScreenHelper
	{
		public static Color GetColor(string ColorName){
			try
			{
				if (ColorName.Equals("orange")) return Color.Orange;
				else
					if (ColorName.Equals("pink")) return Color.Pink;
					else
						return Color.ParseColor(ColorName);
			}
			catch(Exception)
			{
				return Color.Black;
			}
		}

	}
}

