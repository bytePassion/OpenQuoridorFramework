using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.ReplayViewerInfoPage
{
	internal interface IReplayViewerInfoPageViewModel : IViewModel, IPage
	{
		string PageHeader { get; }		

		string Explanation01 { get; }
		string Explanation02 { get; }
		string Explanation03 { get; }
		string Explanation04 { get; }
		string Explanation05 { get; }
		string Explanation06 { get; }
		string Explanation07 { get; }
		string Explanation08 { get; }
		string Explanation09 { get; }
		string Explanation10 { get; }		
	}
}