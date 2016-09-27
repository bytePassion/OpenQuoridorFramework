using System.ComponentModel;
using System.Windows.Input;
using Lib.Communication.State;
using Lib.FrameworkExtension;
using OQF.Visualization.Common.Language;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper
{
	internal class SelectionButtonData : DisposingObject, INotifyPropertyChanged
	{
		private readonly ICommand command;
		private readonly InfoPage pageType;
		private readonly ISharedStateReadOnly<InfoPage> selectedPage;
		private bool isChecked;

		public SelectionButtonData(ICommand command, InfoPage pageType, ISharedStateReadOnly<InfoPage> selectedPage)
		{
			this.command = command;
			this.pageType = pageType;
			this.selectedPage = selectedPage;

			selectedPage.StateChanged += OnSelectedPageChanged;
			CultureManager.CultureChanged += RefreshCaption;
		}

		private void OnSelectedPageChanged(InfoPage infoPage)
		{
			if (infoPage != pageType)
			{
				IsChecked = false;
			}
		}

		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				if (value != isChecked)				
					if (value)
						if (command.CanExecute(null))
							command.Execute(null);
				
				PropertyChanged.ChangeAndNotify(this, ref isChecked, value);
			}
		}

		public string ButtonCaption
		{
			get
			{
				switch (pageType)
				{
					case InfoPage.About:						return Captions.IP_AboutButtonCaption;
					case InfoPage.BotVsBotApplicationInfo:		return Captions.IP_BotVsBotIntoButtonCaption;
					case InfoPage.HowToWriteABot:				return Captions.IP_HowToWriteABotButtonCaption;
					case InfoPage.PlayerVsBotApplicationInfo:	return Captions.IP_PlayerVsBotInfoButtonCaption;
					case InfoPage.QuoridorNotation:				return Captions.IP_QuoridorNotationButtonCaption;
					case InfoPage.QuoridorRules:				return Captions.IP_QuoridorRulesButtonCaption;
					case InfoPage.ReplayViewerApplicationInfo:	return Captions.IP_ReplayViewerInfoButtonCaption;
					case InfoPage.TurnamentApplicationInfo:		return Captions.IP_TurnamentInfoButtonCaption;
				}

				return "error";
			}
		}

		private void RefreshCaption ()
		{
			PropertyChanged.Notify(this, nameof(ButtonCaption));
		}
		
		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaption;
			selectedPage.StateChanged -= OnSelectedPageChanged;
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
