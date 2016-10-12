using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.TournamentInfoPage
{
	internal class TournamentInfoPageViewModelSampleData : ITournamentInfoPageViewModel
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void Dispose() {	}
	    public string DisplayName => "Turnier";
	}
}