using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar
{
	public interface ILocalPlayerBarViewModel : IViewModel
	{
		ICommand Capitulate { get; }

		bool IsGameInitiator { get; }
		int WallsLeft { get; }
	}
}