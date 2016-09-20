using System.Collections.Generic;
using Lib.Wpf.ViewModelBase;

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage
{
	internal interface IAboutPageViewModel : IViewModel
	{
		string PageHeader               { get; }
		string ApplicationSectionHeader { get; }		
		string VersionSectionHeader     { get; }		
		string DeveloperSectionHeader   { get; }		
		string ThirdPartySectionHeader  { get; }		
		string QuoridorSectionHeader    { get; }
		string QuoridorInventerSubItem  { get; }
		string QuoridorPublisherSubItem { get; }
		string LicenceSectionHeader     { get; }
		
		IEnumerable<string> Developers      { get; }
		IEnumerable<string> ThridPartyItems { get; }
		string ApplicationName   { get; }
		string VersionIdentifier { get; }
		string Licence           { get; }
	}
}