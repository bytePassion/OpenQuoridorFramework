using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.BotVsBotInfoPage
{
	internal class BotVsBotInfoPageViewModelSampleData : IBotVsBotInfoPageViewModel
	{
		public void Dispose () {}
		public event PropertyChangedEventHandler PropertyChanged;
	    public string DisplayName => "BotVsBot";
	}
}