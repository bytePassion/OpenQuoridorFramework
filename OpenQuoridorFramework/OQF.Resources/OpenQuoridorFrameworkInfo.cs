using System;
using System.Collections.Generic;

namespace OQF.Resources
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
			"Alexander Horn (alexander.horn@bytePassion.de)"
		};

		public static readonly IEnumerable<ThirdPartyItem> ThirdPartyItems = new List<ThirdPartyItem>
		{
			new ThirdPartyItem("Flag-Icons from", "www.iconDrawer.com", new Uri(@"http://www.icondrawer.com/"))
		};

		public const string Licence = "Apache 2.0";
		public static readonly Uri LicenceUri = new Uri(@"http://www.apache.org/licenses/LICENSE-2.0");

		public const string QuoridorInventor  = "Mirko Marchesi";
		public const string QuoridorPublisher = "Gigamic";
		public static readonly Uri QuoridorPublisherUri = new Uri(@"http://en.gigamic.com/");

		public static readonly Uri GitHubUri = new Uri(GitHubUriText);
		public const string GitHubUriText = @"https://github.com/bytePassion/OpenQuoridorFramework.git";

		public const string FrameworkVersion = "4.2";

		public static class Applications
		{
			public static class PlayerVsBot
			{
				private const string Name               = "Player vs. Bot";
				private const string ApplicationVersion = "1.1";
				
				public static readonly ApplicationInfo Info = new ApplicationInfo(Name, 
																				  ApplicationVersion,
																				  FrameworkVersion, 
																				  Licence, 
																				  LicenceUri, 
																				  ActiveDevelopers, 
																				  ThirdPartyItems);
			}

			public static class ReplayViewer
			{
				public const string Name               = "Replay Viewer";
				public const string ApplicationVersion = "1.1";
				
				public static readonly ApplicationInfo Info = new ApplicationInfo(Name,
																				  ApplicationVersion,
																				  FrameworkVersion,
																				  Licence, 
																				  LicenceUri, 
																				  ActiveDevelopers, 
																				  ThirdPartyItems);
			}

			public static class NetworkLanServer
			{
				public const string Name               = "Network.LanServer";
				public const string ApplicationVersion = "1.0";

				public static readonly ApplicationInfo Info = new ApplicationInfo(Name,
																				  ApplicationVersion,
																				  FrameworkVersion,
																				  Licence,
																				  LicenceUri,
																				  ActiveDevelopers,
																				  ThirdPartyItems);
			}

			public static class NetworkDesktopClient
			{
				public const string Name               = "Network.DesktopClient";
				public const string ApplicationVersion = "1.0";

				public static readonly ApplicationInfo Info = new ApplicationInfo(Name,
																				  ApplicationVersion,
																				  FrameworkVersion,
																				  Licence,
																				  LicenceUri,
																				  ActiveDevelopers,
																				  ThirdPartyItems);
			}

			public static class Tournament
			{
				public const string Name               = "Tournament";
				public const string ApplicationVersion = "0.0";

				public static readonly ApplicationInfo Info = new ApplicationInfo(Name,
																				  ApplicationVersion,
																				  FrameworkVersion,
																				  Licence,
																				  LicenceUri,
																				  ActiveDevelopers,
																				  ThirdPartyItems);
			}			
		}
	}
}
