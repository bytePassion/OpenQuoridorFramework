using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper;
using OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.BotVsBotInfoPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.HowToWriteABotPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.PlayerVsBotInfoPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorNotationPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.ReplayViewerInfoPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.TurnamentInfoPage;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal interface IInfoWindowViewModel : IViewModel
	{
		IQuoridorRulesPageViewModel    QuoridorRulesPageViewModel    { get; }
		IQuoridorNotationPageViewModel QuoridorNotationPageViewModel { get; }
		IHowToWriteABotPageViewModel   HowToWriteABotPageViewModel   { get; }
		IBotVsBotInfoPageViewModel     BotVsBotInfoPageViewModel     { get; }
		IPlayerVsBotInfoPageViewModel  PlayerVsBotInfoPageViewModel  { get; }
		IReplayViewerInfoPageViewModel ReplayViewerInfoPageViewModel { get; }
		ITurnamentInfoPageViewModel    TurnamentInfoPageViewModel    { get; }
		IAboutPageViewModel            AboutPageViewModel            { get; }

		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		ICommand CloseWindow { get; }
		string CloseButtonCaption { get; }
		int SelectedPage { get; }
		ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }
	}
}