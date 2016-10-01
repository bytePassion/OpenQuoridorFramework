using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using OQF.Visualization.Common.Info.Pages.PageViewModels;
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
			LanguageSelectionViewModel    = new LanguageSelectionViewModelSampleData();

			Pages = new List<IPage>
			{
				new QuoridorRulesPageViewModelSampleData(),
				new QuoridorNotationPageViewModelSampleData(),
				new HowToWriteABotPageViewModelSampleData(),
				new BotVsBotInfoPageViewModelSampleData(),
				new PlayerVsBotInfoPageViewModelSampleData(),
				new ReplayViewerInfoPageViewModelSampleData(),
				new TurnamentInfoPageViewModelSampleData(),
				new AboutPageViewModelSampleData(),

			};
			
			CloseButtonCaption = "Close";			
		}

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public IEnumerable<IPage> Pages { get; }

		public ICommand CloseWindow => null;
		public string CloseButtonCaption { get; }				

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}