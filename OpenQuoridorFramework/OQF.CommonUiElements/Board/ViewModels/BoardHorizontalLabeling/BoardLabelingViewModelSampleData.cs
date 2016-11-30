using System.ComponentModel;

namespace OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling
{
	internal class BoardLabelingViewModelSampleData : IBoardLabelingViewModel
	{
		public BoardLabelingViewModelSampleData()
		{
			Label1 = "A";
			Label2 = "B";
			Label3 = "C";
			Label4 = "D";
			Label5 = "E";
			Label6 = "F";
			Label7 = "G";
			Label8 = "H";
			Label9 = "I";
		}

		public string Label1 { get; }
		public string Label2 { get; }
		public string Label3 { get; }
		public string Label4 { get; }
		public string Label5 { get; }
		public string Label6 { get; }
		public string Label7 { get; }
		public string Label8 { get; }
		public string Label9 { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}