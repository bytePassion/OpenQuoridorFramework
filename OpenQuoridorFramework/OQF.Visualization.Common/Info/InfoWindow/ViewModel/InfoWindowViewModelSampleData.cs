using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lib.Wpf.Commands;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModelSampleData : IInfoWindowViewModel
	{
		public InfoWindowViewModelSampleData()
		{
			SelectedPage = 2;

			PageSelectionCommands = new ObservableCollection<SelectionButtonData>
			{
				new SelectionButtonData(new Command(() => {}), "blubb1"),
				new SelectionButtonData(new Command(() => {}), "blubb2"),
				new SelectionButtonData(new Command(() => {}), "blubb3")
			};
		}

		public ICommand CloseWindow => null;

		public int SelectedPage { get; }
		public ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}