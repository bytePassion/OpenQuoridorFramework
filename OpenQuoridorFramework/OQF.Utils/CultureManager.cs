using System;
using System.Globalization;
using System.Threading;

namespace OQF.Utils
{
	public static class CultureManager
	{
		private static CultureInfo currentCulture;
		public static event Action CultureChanged;

		static CultureManager()
		{
			CurrentCulture = new CultureInfo("de");
		}
				
		public static CultureInfo CurrentCulture
		{
			get { return currentCulture; }
			set
			{
				if (!Equals(value, currentCulture))
				{
					currentCulture = value;

					Thread.CurrentThread.CurrentCulture   = value;
					Thread.CurrentThread.CurrentUICulture = value;

					CultureChanged?.Invoke();
				}				
			}
		}
	}
}
