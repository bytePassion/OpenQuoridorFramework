using System.Collections.ObjectModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.LogView
{
	public interface ILogViewModel : IViewModel
	{
		ObservableCollection<string> Output { get; }
	}
}