using System;
using Android.Graphics;

namespace RecoveriesConnect.Helpers
{
	public static class ColorHelper
	{
		public static Color GetColor(string ColorName){
			try
			{
				if (ColorName.Equals("orange")) return Color.Orange;
				else if (ColorName.Equals("pink")) return Color.Pink;
					else if (ColorName.Equals("purple")) return Color.ParseColor("#865FBA");
						else if (ColorName.Equals("green")) return Color.ParseColor("#78BA10");
							else if (ColorName.Equals("blue")) return Color.ParseColor("#466BAA");
								else return Color.ParseColor(ColorName);
			}
			catch(Exception)
			{
				return Color.Black;
			}
		}

	}
}

