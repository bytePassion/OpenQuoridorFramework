using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Info.Pages.PageViewModels;
using OQF.CommonUiElements.Info.Pages.PageViewModels.AboutPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.BotVsBotInfoPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.HowToWriteABotPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.PlayerVsBotInfoPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorNotationPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.ReplayViewerInfoPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.TournamentInfoPage;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModelSampleData : IInfoWindowViewModel
	{
		public InfoWindowViewModelSampleData()
		{
			Pages = new List<IPage>
			{
				new QuoridorRulesPageViewModelSampleData(),
				new QuoridorNotationPageViewModelSampleData(),
				new HowToWriteABotPageViewModelSampleData(),
				new BotVsBotInfoPageViewModelSampleData(),
				new PlayerVsBotInfoPageViewModelSampleData(),
				new ReplayViewerInfoPageViewModelSampleData(),
				new TournamentInfoPageViewModelSampleData(),
				new AboutPageViewModelSampleData(),

			};
			
			CloseButtonCaption = "Close";			
		}

		public IEnumerable<IPage> Pages { get; }

		public ICommand CloseWindow => null;
		public string CloseButtonCaption { get; }				

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}