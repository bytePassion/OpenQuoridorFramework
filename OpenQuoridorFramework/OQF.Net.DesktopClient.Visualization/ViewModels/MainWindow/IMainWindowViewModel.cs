using Lib.Wpf.ViewModelBase;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		string Text { get; }
	}
}