﻿using System;
using System.Collections.Generic;
using Lib.Wpf.ViewModelBase;
using OQF.Resources;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.AboutPage
{
	internal interface IAboutPageViewModel : IViewModel, IPage
	{
		string PageHeader                { get; }
		string ApplicationSectionHeader  { get; }	
		string ApplicationNameSubItem    { get; }
		string ApplicationVersionSubItem { get; }
		string VersionSectionHeader      { get; }		
		string DeveloperSectionHeader    { get; }
		string SourceCodeSectionHeader   { get; }		
		string SourceCodeSectionText     { get; }
		string ThirdPartySectionHeader   { get; }		
		string QuoridorSectionHeader     { get; }
		string QuoridorInventerSubItem   { get; }
		string QuoridorPublisherSubItem  { get; }
		string LicenceSectionHeader      { get; }
		string DevelopedBy               { get; }
		
		IEnumerable<string>         Developers      { get; }
		IEnumerable<ThirdPartyItem> ThridPartyItems { get; }

		string ApplicationName   { get; }
		string VersionIdentifier { get; }
		string FrameworkVersion  { get; }
		string LicenceName       { get; }
		Uri    LicenceUri        { get; }
		string BytePassionName   { get; }
		Uri    BytePassionUri    { get; }
		
	}
}