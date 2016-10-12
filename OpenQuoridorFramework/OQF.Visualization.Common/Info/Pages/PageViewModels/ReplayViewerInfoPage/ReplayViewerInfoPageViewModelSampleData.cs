using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.ReplayViewerInfoPage
{
	internal class ReplayViewerInfoPageViewModelSampleData : IReplayViewerInfoPageViewModel
	{
		public string DisplayName => "Info";

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;        
    }
}