using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.Utils;
using ProgressCodingTest.Coding;

namespace ProgressCodingTest.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMailWindowViewModel
	{
		private string progress;
		private string progressAsString;
		public ICommand ConvertProgressToString { get; }
		public ICommand ConvertStringToProgress { get; }

		public MainWindowViewModel()
		{
			ConvertProgressToString = new Command(ConvertToString);
			ConvertStringToProgress = new Command(ConvertToProgress);
		}

		private void ConvertToProgress()
		{
			var progressAsCompressedString = ProgressAsString;

			var progressAsNumber = Base64Coding.Decode(progressAsCompressedString);
			var progressAsMoves = ProgressCoding.ConvertBigIntegerToMoveList(progressAsNumber);
			var progressAsStringMoves = progressAsMoves.Select(move => move.ToString());
			var progressTextList = CreateProgressText.FromMoveList(progressAsStringMoves.ToList());

			var progressText = new StringBuilder();

			foreach (var row in progressTextList)
			{
				progressText.Append(row + "\n");
			}

			Progress = progressText.ToString();
		}

		private void ConvertToString()
		{
			var progressText = Progress;

			var progressAsStringMoves = ParseProgressText.FromFileText(progressText);
			var progressAsMoves = progressAsStringMoves.Select(MoveParser.GetMove);
			var progressAsNumber = ProgressCoding.ConvertMoveListToBigInteger(progressAsMoves);
			var progressAsCompressedString = Base64Coding.Encode(progressAsNumber);

			ProgressAsString = progressAsCompressedString;
		}

		public string Progress
		{
			get { return progress; }
			set { PropertyChanged.ChangeAndNotify(this, ref progress, value); }
		}

		public string ProgressAsString
		{
			get { return progressAsString; }
			set { PropertyChanged.ChangeAndNotify(this, ref progressAsString, value); }
		}

		protected override void CleanUp() {}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
