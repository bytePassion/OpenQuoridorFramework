using System.Collections.ObjectModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.LogView
{
	public interface ILogViewModel : IViewModel
	{
		ObservableCollection<string> Output { get; }
	}
}