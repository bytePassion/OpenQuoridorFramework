using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorRulesPage
{
	internal interface IQuoridorRulesPageViewModel : IViewModel, IPage
	{
		string PageHeader                 { get; }
		string GeneralParagraphHeader     { get; }
		string GeneralParagraphText       { get; }
		string GameSetupParagraphHeader   { get; }
		string GameSetupParagrphText      { get; }
		string GameGoalParagraphHeader    { get; }
		string GameGoalParagraphText      { get; }
		string GameFlowParagraphHeader    { get; }
		string GameFlowParagraphText      { get; }
		string SpecialMoveParagraphHeader { get; }
		string SpecialMoveParagraphText   { get; }

		string Picture1Caption { get; }
		string Picture2Caption { get; }
		string Picture3Caption { get; }
		string Picture4Caption { get; }
		string Picture5Caption { get; }
	}
}