using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.HowToWriteABotPage
{
	internal class HowToWriteABotPageViewModel : ViewModel, IHowToWriteABotPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
