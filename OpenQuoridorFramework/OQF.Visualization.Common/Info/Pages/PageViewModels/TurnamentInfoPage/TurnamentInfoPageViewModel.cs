using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.TurnamentInfoPage
{
	internal class TurnamentInfoPageViewModel : ViewModel, ITurnamentInfoPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
