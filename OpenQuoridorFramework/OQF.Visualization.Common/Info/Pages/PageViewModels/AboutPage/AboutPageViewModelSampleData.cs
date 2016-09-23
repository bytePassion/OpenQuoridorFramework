using System.Collections.Generic;
using System.ComponentModel;
using OQF.Visualization.Resources;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage
{
	internal class AboutPageViewModelSampleData : IAboutPageViewModel
	{
		public AboutPageViewModelSampleData()
		{
			var applicationInfo = OpenQuoridorFrameworkInfo.Applications.PlayerVsBot.Info;

			Developers        = applicationInfo.Developers;
			ThridPartyItems   = applicationInfo.ThirdPartyItems;
			ApplicationName   = applicationInfo.Name;
			VersionIdentifier = applicationInfo.Version;
			Licence           = applicationInfo.Licence;			
		}

		public string PageHeader               => "blablubb";
		public string ApplicationSectionHeader => "blablubb";
		public string VersionSectionHeader     => "blablubb";
		public string DeveloperSectionHeader   => "blablubb";
		public string SourceCodeSectionHeader  => "blablubb";
		public string SourceCodeSectionText    => "blablubb";
		public string ThirdPartySectionHeader  => "blablubb";
		public string QuoridorSectionHeader    => "blablubb";
		public string QuoridorInventerSubItem  => "blablubb";
		public string QuoridorPublisherSubItem => "blablubb";
		public string LicenceSectionHeader     => "blablubb";
		
		public IEnumerable<string> Developers      { get; }
		public IEnumerable<string> ThridPartyItems { get; }
		public string ApplicationName   { get; }
		public string VersionIdentifier { get; }
		public string Licence           { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}