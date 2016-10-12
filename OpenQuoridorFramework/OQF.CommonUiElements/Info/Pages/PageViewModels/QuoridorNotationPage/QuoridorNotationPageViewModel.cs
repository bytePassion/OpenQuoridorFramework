using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Language;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorNotationPage
{
	internal class QuoridorNotationPageViewModel : ViewModel, IQuoridorNotationPageViewModel
	{
		public QuoridorNotationPageViewModel()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}

		public string PageHeader                => Captions.QNP_PageHeader;
		public string GeneralParagraphHeader    => Captions.QNP_GeneralParagraphHeader;
		public string GeneralParagraphText      => Captions.QNP_GeneralParagraphText;
		public string TheBoadParagraphHeader    => Captions.QNP_TheBoardParagraphHeader;
		public string TheBoadParagraphText      => Captions.QNP_TheBoardParagraphText;
		public string FigureMoveParagraphHeader => Captions.QNP_FigureMoveParagraphHeader;
		public string FigureMoveParagraphText   => Captions.QNP_FigureMoveParagraphText;
		public string WallMoveParagraphHeader   => Captions.QNP_WallMoveParagraphHeader;
		public string WallMoveParagraphText     => Captions.QNP_WallMoveParagraphText;
		public string ExampleParagraphHeader    => Captions.QNP_ExampleParagraphHeader;
		public string ExampleParagraphText      => Captions.QNP_ExampleParagraphText;
		
		public string Picture1Caption           => Captions.QNP_Picture1Caption;
		public string Picture2Caption           => Captions.QNP_Picture2Caption;
        public string DisplayName               => Captions.IP_QuoridorNotationButtonCaption;


        private void RefreshCaptions()
		{
			PropertyChanged.Notify(this, nameof(PageHeader),
										 nameof(GeneralParagraphHeader),
										 nameof(GeneralParagraphText),
										 nameof(TheBoadParagraphHeader),
										 nameof(TheBoadParagraphText),
										 nameof(FigureMoveParagraphHeader),
										 nameof(FigureMoveParagraphText),
										 nameof(WallMoveParagraphHeader),
										 nameof(WallMoveParagraphText),
										 nameof(ExampleParagraphHeader),
										 nameof(ExampleParagraphText),
										 nameof(Picture1Caption),
										 nameof(Picture2Caption),
										 nameof(DisplayName));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
