using System.Collections.Generic;

namespace OQF.Visualization.Resources
{
	public static class OpenQuoridorFrameworkInfo
	{
		public static readonly IEnumerable<string> AvailableLanguageCodes = new List<string>
		{
			"de",
			"en"
		};
		
		public static readonly IEnumerable<string> ActiveDevelopers = new List<string>
		{
			"Matthias Drescher (matthias.drescher@bytePassion.de)",
			"Alexander Horn (alex.horn@bytePassion.de)"
		};
		
		public const string Licence = "Apache 2.0";

		public const string QuoridorInventor  = "Mirko Marchesi";
		public const string QuoridorPublisher = "Gigamic";

		public static class Applications
		{
			public static class PlayerVsBot
			{
				private const string Name    = "Player vs. Bot";
				private const string Version = "1.0.0.0";
				private static readonly IEnumerable<string> ThirdPartyItems = new List<string>
				{
					"Flagicons from www.iconDrawer.com"
				};

				public static readonly ApplicationInfo Info = new ApplicationInfo(Name, Version, Licence, ActiveDevelopers, ThirdPartyItems);
			}

			public static class ReplayViewer
			{
				public const string Name = "Replay Viewer";
				public const string Version = "1.0.0.0";
				public static readonly IEnumerable<string> ThirdPartyItems = new List<string>
				{
					"Flagicons from www.iconDrawer.com"
				};

				public static readonly ApplicationInfo Info = new ApplicationInfo(Name, Version, Licence, ActiveDevelopers, ThirdPartyItems);
			}			
		}
	}
}
