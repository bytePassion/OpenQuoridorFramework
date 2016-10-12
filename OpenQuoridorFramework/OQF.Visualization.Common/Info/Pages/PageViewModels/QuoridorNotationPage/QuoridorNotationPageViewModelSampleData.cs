using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorNotationPage
{
	internal class QuoridorNotationPageViewModelSampleData : IQuoridorNotationPageViewModel
	{
		public string PageHeader                => "blubb";
		public string GeneralParagraphHeader    => "blubb";
		public string GeneralParagraphText      => "blubb";
		public string TheBoadParagraphHeader    => "blubb";
		public string TheBoadParagraphText      => "blubb";
		public string FigureMoveParagraphHeader => "blubb";
		public string FigureMoveParagraphText   => "blubb";
		public string WallMoveParagraphHeader   => "blubb";
		public string WallMoveParagraphText     => "blubb";
		public string ExampleParagraphHeader    => "blubb";
		public string ExampleParagraphText      => "blubb";												   
		public string Picture1Caption           => "blubb";
		public string Picture2Caption           => "blubb";
        public string DisplayName => "test";


        public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}