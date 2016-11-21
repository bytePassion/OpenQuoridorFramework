using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Board.BoardViewModel;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow.Helper;

#pragma warning disable 0067

namespace OQF.ReplayViewer.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardViewModel = new BoardViewModelSampleData();
			LodingString = "blubb.txt";
			MoveIndex = 5;
			MaxMoveIndex = 15;
			IsReplayLoaded = true;

			ProgressRows = new ObservableCollection<ProgressRow>
			{
				new ProgressRow("1. e2 e8", false),
				new ProgressRow("1. e3 e7", false),
				new ProgressRow("1. e4 e6", false)
			};
		}

		public IBoardViewModel BoardViewModel { get; }

		public ICommand LoadGame      => null;
		public ICommand BrowseFile    => null;
		public ICommand ShowAboutHelp => null;
		public ICommand NextMove      => null;
		public ICommand PreviousMove  => null;

		public int MoveIndex { get; set; }
		public int MaxMoveIndex { get; }
		public bool IsReplayLoaded { get; }
		public ObservableCollection<ProgressRow> ProgressRows { get; }
		public string LodingString { get; set; }		

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}