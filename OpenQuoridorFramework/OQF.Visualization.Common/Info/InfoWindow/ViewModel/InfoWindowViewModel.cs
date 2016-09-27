using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.Communication.State;
using Lib.FrameworkExtension;
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
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModel : Lib.Wpf.ViewModelBase.ViewModel, IInfoWindowViewModel
	{
		
		private int selectedPage;

		private readonly SharedState<InfoPage> selectedPageVariable;

		public InfoWindowViewModel(IEnumerable<InfoPage> visibleInfoPages,
								   ILanguageSelectionViewModel languageSelectionViewModel,
								   IQuoridorRulesPageViewModel quoridorRulesPageViewModel,
								   IQuoridorNotationPageViewModel quoridorNotationPageViewModel,
								   IHowToWriteABotPageViewModel howToWriteABotPageViewModel, 
								   IBotVsBotInfoPageViewModel botVsBotInfoPageViewModel, 
								   IPlayerVsBotInfoPageViewModel playerVsBotInfoPageViewModel, 
								   IReplayViewerInfoPageViewModel replayViewerInfoPageViewModel, 
								   ITurnamentInfoPageViewModel turnamentInfoPageViewModel,
								   IAboutPageViewModel aboutPageViewModel)
		{
			QuoridorRulesPageViewModel = quoridorRulesPageViewModel;
			AboutPageViewModel = aboutPageViewModel;
			HowToWriteABotPageViewModel = howToWriteABotPageViewModel;
			BotVsBotInfoPageViewModel = botVsBotInfoPageViewModel;
			PlayerVsBotInfoPageViewModel = playerVsBotInfoPageViewModel;
			ReplayViewerInfoPageViewModel = replayViewerInfoPageViewModel;
			TurnamentInfoPageViewModel = turnamentInfoPageViewModel;
			QuoridorNotationPageViewModel = quoridorNotationPageViewModel;
			LanguageSelectionViewModel = languageSelectionViewModel;

			selectedPageVariable = new SharedState<InfoPage>();

			CloseWindow = new Command(DoCloseWindow);
			PageSelectionCommands = new ObservableCollection<SelectionButtonData>();

			foreach (var page in visibleInfoPages)
			{
				var command = new Command(() =>
				{
					var pageNr = (int) page;
					SelectedPage = pageNr;
				});
				
				PageSelectionCommands.Add(new SelectionButtonData(command, page, selectedPageVariable));
			}

			var firstPage = PageSelectionCommands.FirstOrDefault();
			if (firstPage != null)
			{
				firstPage.IsChecked = true;
			}
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

		public ICommand CloseWindow { get; }

		public string CloseButtonCaption => Captions.IP_CloseButtonCaption;

		private void DoCloseWindow ()
		{
			Application.Current.Windows
							   .OfType<InfoWindow>()
							   .FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
							   ?.Close();			
		}

		public int SelectedPage
		{
			get { return selectedPage; }
			private set
			{
				selectedPageVariable.Value = (InfoPage) value;
				PropertyChanged.ChangeAndNotify(this, ref selectedPage, value);
			}
		}

		public ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }

		protected override void CleanUp()
		{
			foreach (var pageSelectionCommand in PageSelectionCommands)
			{
				pageSelectionCommand.Dispose();
			}
		}
		public override event PropertyChangedEventHandler PropertyChanged;
		
	}
}
