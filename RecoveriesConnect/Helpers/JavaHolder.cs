using System;

namespace RecoveriesConnect.Helpers
{
	public class JavaHolder : Java.Lang.Object
	{
		public readonly object Instance;

		public JavaHolder(object instance)
		{
			Instance = instance;
		}
	} 
}

