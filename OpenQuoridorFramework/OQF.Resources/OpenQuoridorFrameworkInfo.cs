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

		public const string DevelopedBy = "bytePassion";
		public static readonly Uri DevelopedByUrl = new Uri(@"http://www.bytePassion.de");

		public const string Licence = "Apache 2.0";
		public static readonly Uri LicenceUri = new Uri(@"http://www.apache.org/licenses/LICENSE-2.0");

		public const string QuoridorInventor  = "Mirko Marchesi";
		public const string QuoridorPublisher = "Gigamic";
		public static readonly Uri QuoridorPublisherUri = new Uri(@"http://en.gigamic.com/");

		public static readonly Uri GitHubUri = new Uri(GitHubUriText);
		public const string GitHubUriText = @"https://github.com/bytePassion/OpenQuoridorFramework.git";

		public const string FrameworkVersion = "4.7.1";

		public static class Applications
		{
			public static class PlayerVsBot          { public static readonly ApplicationInfo Info = new ApplicationInfo("Player vs. Bot",        "1.2");       }
			public static class ReplayViewer         { public static readonly ApplicationInfo Info = new ApplicationInfo("Replay Viewer",         "1.1");       }
			public static class NetworkLanServer     { public static readonly ApplicationInfo Info = new ApplicationInfo("Network.LanServer",     "1.0");       }
			public static class NetworkDesktopClient { public static readonly ApplicationInfo Info = new ApplicationInfo("Network.DesktopClient", "1.0.1");     }
			public static class Tournament           { public static readonly ApplicationInfo Info = new ApplicationInfo("Tournament",            "0.4 Alpha"); }			
		}
	}
}
