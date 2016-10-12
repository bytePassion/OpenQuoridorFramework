using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Language;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorRulesPage
{
	internal class QuoridorRulesPageViewModel : ViewModel, IQuoridorRulesPageViewModel
	{
		public QuoridorRulesPageViewModel()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}

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
	    public string DisplayName                => Captions.IP_QuoridorRulesButtonCaption;


        public string Picture1Caption => Captions.QRP_Picture1Caption;
		public string Picture2Caption => Captions.QRP_Picture2Caption;
		public string Picture3Caption => Captions.QRP_Picture3Caption;
		public string Picture4Caption => Captions.QRP_Picture4Caption;
		public string Picture5Caption => Captions.QRP_Picture5Caption;

		private void RefreshCaptions()
		{
			PropertyChanged.Notify(this, nameof(PageHeader),
										 nameof(GeneralParagraphHeader),
										 nameof(GeneralParagraphText),
										 nameof(GameSetupParagraphHeader),
										 nameof(GameSetupParagrphText),
										 nameof(GameGoalParagraphHeader),
										 nameof(GameGoalParagraphText),
										 nameof(GameFlowParagraphHeader),
										 nameof(GameFlowParagraphText),
										 nameof(SpecialMoveParagraphHeader),
										 nameof(SpecialMoveParagraphText),
										 nameof(Picture1Caption),
										 nameof(Picture2Caption),
										 nameof(Picture3Caption),
										 nameof(Picture4Caption),
										 nameof(Picture5Caption),
										 nameof(DisplayName));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
