using System;
using System.Collections.Generic;

namespace OQF.Resources
{
	public class ApplicationInfo
	{
		public ApplicationInfo(string name, string version, string frameworkVersion, string licenceName, Uri licenceUri,
							   IEnumerable<string> developers, IEnumerable<ThirdPartyItem> thirdPartyItems)
		{
			Name = name;
			ApplicationVersion = version;
			LicenceName = licenceName;
			LicenceUri = licenceUri;
			Developers = developers;
			ThirdPartyItems = thirdPartyItems;
			FrameworkVersion = frameworkVersion;
		}

		public IEnumerable<string>         Developers      { get; }
		public IEnumerable<ThirdPartyItem> ThirdPartyItems { get; }

		public string Name               { get; }
		public string ApplicationVersion { get; }
		public string FrameworkVersion   { get; }
		public string LicenceName        { get; }
		public Uri    LicenceUri         { get; }
	}
}