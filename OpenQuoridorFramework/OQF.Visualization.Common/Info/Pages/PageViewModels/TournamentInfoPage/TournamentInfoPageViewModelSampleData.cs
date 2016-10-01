using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.TournamentInfoPage
{
	internal class TournamentInfoPageViewModelSampleData : ITournamentInfoPageViewModel
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void Dispose() {	}
	    public string DisplayName => "Turnier";
	}
}