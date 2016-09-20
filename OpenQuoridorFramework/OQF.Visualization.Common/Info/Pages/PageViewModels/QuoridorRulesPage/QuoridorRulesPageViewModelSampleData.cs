using System.ComponentModel;
using OQF.Visualization.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorRulesPage
{
	internal class QuoridorRulesPageViewModelSampleData : IQuoridorRulesPageViewModel
	{
		public string PageHeader                 => Captions.QRP_PageHeader;
		public string GeneralParagraphHeader     => Captions.QRP_GeneralParagraphHeader;
		public string GeneralParagraphText       => Captions.QRP_GeneralParagraphText;
		public string GameSetupParagraphHeader   => Captions.QRP_GameSetupParagraphHeader;
		public string GameSetupParagrphText      => Captions.QRP_GameSetupParagraphText;
		public string GameGoalParagraphHeader    => Captions.QRP_GameGoalParagraphHeader;
		public string GameGoalParagraphText      => Captions.QRP_GameGoalParagraphText;
		public string GameFlowParagraphHeader    => Captions.QRP_GameFlowParagraphHeader;
		public string GameFlowParagraphText      => Captions.QRP_GameFlowParagraphText;
		public string SpecialMoveParagraphHeader => Captions.QRP_SpecialMoveParagraphHeader;
		public string SpecialMoveParagraphText   => Captions.QRP_SpecialMoveParagraphText;

		public string Picture1Caption => Captions.QRP_Picture1Caption;
		public string Picture2Caption => Captions.QRP_Picture2Caption;
		public string Picture3Caption => Captions.QRP_Picture3Caption;
		public string Picture4Caption => Captions.QRP_Picture4Caption;
		public string Picture5Caption => Captions.QRP_Picture5Caption;
		
		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}