using System.ComponentModel;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.ReplayViewerInfoPage
{
	internal class ReplayViewerInfoPageViewModel : ViewModel , IReplayViewerInfoPageViewModel 
	{
		public string DisplayName => Captions.IP_ReplayViewerInfoButtonCaption;

		protected override void CleanUp(){	}
		public override event PropertyChangedEventHandler PropertyChanged;        
    }
}
