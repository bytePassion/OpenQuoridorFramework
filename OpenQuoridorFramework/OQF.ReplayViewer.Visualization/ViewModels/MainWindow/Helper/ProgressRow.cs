using System.ComponentModel;
using Lib.FrameworkExtension;

namespace OQF.ReplayViewer.Visualization.ViewModels.MainWindow.Helper
{
	public class ProgressRow : INotifyPropertyChanged
	{
		private bool highlightBottomPlayerMove;
		private bool highlightTopPlayerMove;

		public ProgressRow(string line)
		{
			var parts = line.Trim().Split(' ');

			BottomPlayerMove = parts.Length > 1
									? parts[1]
									: string.Empty;

			TopPlayerMove = parts.Length > 2
									? parts[2]
									: string.Empty;

			LineNumber = parts.Length > 0
									? parts[0]
									: string.Empty;

			HighlightBottomPlayerMove = false;
			HighlightTopPlayerMove    = false;
		}

		public string LineNumber       { get; }
		public string BottomPlayerMove { get; }
		public string TopPlayerMove    { get; }

		public bool HighlightBottomPlayerMove
		{
			get { return highlightBottomPlayerMove; }
			set { PropertyChanged.ChangeAndNotify(this, ref highlightBottomPlayerMove, value); }
		}

		public bool HighlightTopPlayerMove
		{
			get { return highlightTopPlayerMove; }
			set { PropertyChanged.ChangeAndNotify(this, ref highlightTopPlayerMove, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
