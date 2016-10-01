using System.ComponentModel;
using System.Windows.Input;
using OQF.Visualization.Common.Board.BoardViewModelBase;

#pragma warning disable 0067

namespace OQF.ReplayViewer.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardViewModel = new BoardViewModelSampleData();
			ProgressFilePath = "blubb.txt";
			MoveIndex = 5;
			MaxMoveIndex = 15;
			IsReplayLoaded = true;
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
		public string ProgressFilePath { get; set; }		

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}