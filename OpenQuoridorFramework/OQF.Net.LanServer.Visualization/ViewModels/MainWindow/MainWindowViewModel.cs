using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		public MainWindowViewModel(string text)
		{
			Text = text;
		}

		public string Text { get; }

		protected override void CleanUp()
		{			
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
