using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.TurnamentInfoPage
{
	internal class TurnamentInfoPageViewModel : ViewModel, ITurnamentInfoPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	    public string DisplayName => "Tournament";
	}
}
