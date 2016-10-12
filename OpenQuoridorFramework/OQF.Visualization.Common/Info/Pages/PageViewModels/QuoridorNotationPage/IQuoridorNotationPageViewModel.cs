using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorNotationPage
{
	internal interface IQuoridorNotationPageViewModel : IViewModel, IPage
	{
		string PageHeader                { get; }
		string GeneralParagraphHeader    { get; }		
		string GeneralParagraphText      { get; }		
		string TheBoadParagraphHeader    { get; }
		string TheBoadParagraphText      { get; }
		string FigureMoveParagraphHeader { get; }
		string FigureMoveParagraphText   { get; }
		string WallMoveParagraphHeader   { get; }
		string WallMoveParagraphText     { get; }
		string ExampleParagraphHeader    { get; }
		string ExampleParagraphText      { get; }

		string Picture1Caption           { get; }
		string Picture2Caption           { get; }
	}
}