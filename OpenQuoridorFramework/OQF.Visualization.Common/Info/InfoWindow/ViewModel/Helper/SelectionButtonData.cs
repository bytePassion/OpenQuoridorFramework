using System.ComponentModel;
using System.Windows.Input;
using Lib.FrameworkExtension;
using OQF.Visualization.Common.Language;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper
{
	internal class SelectionButtonData : DisposingObject, INotifyPropertyChanged
	{
		private readonly InfoPage pageType;

		public SelectionButtonData(ICommand command, InfoPage pageType)
		{
			this.pageType = pageType;
			Command = command;

			CultureManager.CultureChanged += RefreshCaption;
		}		

		public ICommand Command { get; }

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
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
