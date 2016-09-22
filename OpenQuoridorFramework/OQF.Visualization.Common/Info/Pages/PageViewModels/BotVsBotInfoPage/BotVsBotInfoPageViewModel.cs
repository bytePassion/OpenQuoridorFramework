using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.BotVsBotInfoPage
{
	internal class BotVsBotInfoPageViewModel : ViewModel, IBotVsBotInfoPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
