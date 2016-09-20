using System.Collections.Generic;

namespace OQF.Visualization.Resources
{
	public class ApplicationInfo
	{
		public ApplicationInfo(string name, string version, string licence, 
							   IEnumerable<string> developers, IEnumerable<string> thirdPartyItems)
		{
			Name = name;
			Version = version;
			Licence = licence;
			Developers = developers;
			ThirdPartyItems = thirdPartyItems;
		}

		public IEnumerable<string> Developers { get; }
		public IEnumerable<string> ThirdPartyItems { get; }

		public string Name    { get; }
		public string Version { get; }
		public string Licence { get; }
	}
}