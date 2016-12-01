using System.ComponentModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar
{
	public class LocalPlayerBarViewModel : ViewModel, ILocalPlayerBarViewModel
	{
		private readonly INetworkGameService networkGameService;

		public LocalPlayerBarViewModel(INetworkGameService networkGameService)
		{
			this.networkGameService = networkGameService;
		}

		public ICommand Capitulate { get; }
		public bool IsGameInitiator { get; }
		public int WallsLeft { get; }

		protected override void CleanUp()
		{
			
		}

		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
