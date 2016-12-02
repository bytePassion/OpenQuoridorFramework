using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar
{
	public class ConnectionBarViewModel : ViewModel, IConnectionBarViewModel
	{
		protected override void CleanUp()
		{			
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
