using System;
using System.Collections.Generic;

namespace OQF.Resources
{
	public class ApplicationInfo
	{
		public ApplicationInfo(string name, string version)
		{
			Name = name;
			ApplicationVersion = version;

			LicenceName      = OpenQuoridorFrameworkInfo.Licence;
			LicenceUri       = OpenQuoridorFrameworkInfo.LicenceUri;
			Developers       = OpenQuoridorFrameworkInfo.ActiveDevelopers;
			ThirdPartyItems  = OpenQuoridorFrameworkInfo.ThirdPartyItems;
			DevelopedBy      = OpenQuoridorFrameworkInfo.DevelopedBy;
			DevelopedByUri   = OpenQuoridorFrameworkInfo.DevelopedByUrl;
			FrameworkVersion = OpenQuoridorFrameworkInfo.FrameworkVersion;
		}

		public IEnumerable<string>         Developers      { get; }
		public IEnumerable<ThirdPartyItem> ThirdPartyItems { get; }

		public string Name               { get; }
		public string ApplicationVersion { get; }
		public string FrameworkVersion   { get; }
		public string LicenceName        { get; }
		public Uri    LicenceUri         { get; }
		public string DevelopedBy        { get; }
		public Uri    DevelopedByUri     { get; }
	}
}