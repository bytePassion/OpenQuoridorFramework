using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.HowToWriteABotPage
{
	internal class HowToWriteABotPageViewModel : ViewModel, IHowToWriteABotPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
