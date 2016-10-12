using System;
using System.Collections.Generic;
using System.ComponentModel;
using OQF.Resources;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.AboutPage
{
	internal class AboutPageViewModelSampleData : IAboutPageViewModel
	{		
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
		public string ApplicationName          => "blablubb";
		public string VersionIdentifier        => "blablubb";
		public string LicenceName              => "blablubb";
		public string DisplayName              => "About";
		public Uri LicenceUri                  => new Uri("blablubb");

		public IEnumerable<string>         Developers      { get; }
		public IEnumerable<ThirdPartyItem> ThridPartyItems { get; }
				
		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;	    
	}
}