using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.Visualization.Common.Info.Pages.PageViewModels.ReplayViewerInfoPage
{
	internal class ReplayViewerInfoPageViewModel : ViewModel , IReplayViewerInfoPageViewModel 
	{
		protected override void CleanUp(){	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
