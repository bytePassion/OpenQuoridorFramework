using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorRulesPage
{
	internal class QuoridorRulesPageViewModelSampleData : IQuoridorRulesPageViewModel
	{
		public string PageHeader                 => "blaBlubb";
		public string GeneralParagraphHeader     => "blaBlubb";
		public string GeneralParagraphText       => "blaBlubb";
		public string GameSetupParagraphHeader   => "blaBlubb";
		public string GameSetupParagrphText      => "blaBlubb";
		public string GameGoalParagraphHeader    => "blaBlubb";
		public string GameGoalParagraphText      => "blaBlubb";
		public string GameFlowParagraphHeader    => "blaBlubb";
		public string GameFlowParagraphText      => "blaBlubb";
		public string SpecialMoveParagraphHeader => "blaBlubb";
		public string SpecialMoveParagraphText   => "blaBlubb";

		public string Picture1Caption => "Bild1";
		public string Picture2Caption => "Bild2";
		public string Picture3Caption => "Bild3";
		public string Picture4Caption => "Bild4";
		public string Picture5Caption => "Bild5";
		
		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	    public string DisplayName => "blaBlubb";
        }
}