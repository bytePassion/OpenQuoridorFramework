using System;
using System.Collections.Generic;
using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.Language;
using OQF.Visualization.Resources;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage
{
	internal class AboutPageViewModel : ViewModel, IAboutPageViewModel
	{
		public AboutPageViewModel(ApplicationInfo applicationInfo)
		{
			Developers        = applicationInfo.Developers;
			ThridPartyItems   = applicationInfo.ThirdPartyItems;
			ApplicationName   = applicationInfo.Name;
			VersionIdentifier = applicationInfo.Version;
			LicenceName       = applicationInfo.LicenceName;
			LicenceUri        = applicationInfo.LicenceUri;

			CultureManager.CultureChanged += RefreshCaptions;
		}


		public string PageHeader               => Captions.AP_PageHeader;
		public string ApplicationSectionHeader => Captions.AP_ApplicationSectionHeader;
		public string VersionSectionHeader     => Captions.AP_VersionSectionHeader;
		public string DeveloperSectionHeader   => Captions.AP_DeveloperSectionHeader;
		public string SourceCodeSectionHeader  => Captions.AP_SourceCodeSectionHeader;
		public string SourceCodeSectionText    => Captions.AP_SourceCodeSectionText;
		public string ThirdPartySectionHeader  => Captions.AP_ThirdPartySectionHeader;
		public string QuoridorSectionHeader    => Captions.AP_QuoridorSectionHeader;
		public string QuoridorInventerSubItem  => Captions.AP_QuoridorInventorSubItem;
		public string QuoridorPublisherSubItem => Captions.AP_QuoridorPublisherSubItem;
		public string LicenceSectionHeader     => Captions.AP_LicenceSectionHeader;
		
		public IEnumerable<string>         Developers      { get; }
		public IEnumerable<ThirdPartyItem> ThridPartyItems { get; }
		
		public string ApplicationName   { get; }
		public string VersionIdentifier { get; }
		public string LicenceName       { get; }
		public Uri    LicenceUri        { get; }		

		private void RefreshCaptions()
		{
			PropertyChanged.Notify(this, nameof(PageHeader),
										 nameof(ApplicationSectionHeader),
										 nameof(VersionSectionHeader),
										 nameof(DeveloperSectionHeader),
										 nameof(ThirdPartySectionHeader),
										 nameof(QuoridorSectionHeader),
										 nameof(QuoridorInventerSubItem),
										 nameof(QuoridorPublisherSubItem),
										 nameof(LicenceSectionHeader),
										 nameof(SourceCodeSectionHeader),
										 nameof(SourceCodeSectionText));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
