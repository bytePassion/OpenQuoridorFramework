using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.ReplayViewerInfoPage
{
	internal class ReplayViewerInfoPageViewModelSampleData : IReplayViewerInfoPageViewModel
	{
		public string PageHeader    => "blubb";		
		public string Explanation01 => "1 blubb";
		public string Explanation02 => "2 blubb";
		public string Explanation03 => "3 blubb";
		public string Explanation04 => "4 blubb";
		public string Explanation05 => "5 blubb";
		public string Explanation06 => "6 blubb";
		public string Explanation07 => "7 blubb";
		public string Explanation08 => "8 blubb";
		public string Explanation09 => "9 blubb";
		public string Explanation10 => "10 blubb";

		public string DisplayName => "ReplayViewer";				

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}