using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lib.Communication.State;
using Lib.Wpf.Commands;
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

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModelSampleData : IInfoWindowViewModel
	{
		public InfoWindowViewModelSampleData()
		{
			QuoridorRulesPageViewModel    = new QuoridorRulesPageViewModelSampleData();
			QuoridorNotationPageViewModel = new QuoridorNotationPageViewModelSampleData();
			HowToWriteABotPageViewModel   = new HowToWriteABotPageViewModelSampleData();
			BotVsBotInfoPageViewModel     = new BotVsBotInfoPageViewModelSampleData();
			PlayerVsBotInfoPageViewModel  = new PlayerVsBotInfoPageViewModelSampleData();
			ReplayViewerInfoPageViewModel = new ReplayViewerInfoPageViewModelSampleData();
			TurnamentInfoPageViewModel    = new TurnamentInfoPageViewModelSampleData();			
			AboutPageViewModel            = new AboutPageViewModelSampleData();
			LanguageSelectionViewModel    = new LanguageSelectionViewModelSampleData();

			SelectedPage = 2;
			CloseButtonCaption = "Close";

			PageSelectionCommands = new ObservableCollection<SelectionButtonData>
			{
				new SelectionButtonData(new Command(() => {}), InfoPage.About,                   new SharedState<InfoPage>()),
				new SelectionButtonData(new Command(() => {}), InfoPage.BotVsBotApplicationInfo, new SharedState<InfoPage>()),
				new SelectionButtonData(new Command(() => {}), InfoPage.HowToWriteABot,          new SharedState<InfoPage>())
			};
		}

		public IQuoridorRulesPageViewModel    QuoridorRulesPageViewModel    { get; }
		public IQuoridorNotationPageViewModel QuoridorNotationPageViewModel { get; }
		public IHowToWriteABotPageViewModel   HowToWriteABotPageViewModel   { get; }
		public IBotVsBotInfoPageViewModel     BotVsBotInfoPageViewModel     { get; }
		public IPlayerVsBotInfoPageViewModel  PlayerVsBotInfoPageViewModel  { get; }
		public IReplayViewerInfoPageViewModel ReplayViewerInfoPageViewModel { get; }
		public ITurnamentInfoPageViewModel    TurnamentInfoPageViewModel    { get; }
		public IAboutPageViewModel            AboutPageViewModel            { get; }
		public ILanguageSelectionViewModel    LanguageSelectionViewModel    { get; }

		public ICommand CloseWindow => null;
		public string CloseButtonCaption { get; }
		public int SelectedPage { get; }
		public ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}