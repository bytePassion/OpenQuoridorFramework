using System.ComponentModel;
using Lib.Wpf.ViewModelBase;
using OQF.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.TournamentInfoPage
{
	internal class TournamentInfoPageViewModel : ViewModel, ITournamentInfoPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	    public string DisplayName => Captions.IP_TournamentInfoButtonCaption;
	}
}
