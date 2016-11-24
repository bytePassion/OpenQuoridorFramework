using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.ProgressView
{
	internal class ProgressViewModelSampleData : IProgressViewModel
	{
		public ProgressViewModelSampleData()
		{
			GameProgress = new ObservableCollection<string>
			{
				"1. e2 e8",
				"2. e3 e7"
			};

			CompressedProgress = "sdflkjDlT46Ldsasd356FSlsdGlnxAzx";

			IsAutoScrollProgressActive = true;

			AutoScrollDownCheckBoxCaption       = "Automatisch scrollen";			
			CompressedProgressCaption           = "Komprimierter Spielfortschritt";
			CopyToClipboardButtonToolTipCpation = "In Zwischenablage kopieren";
			DumpProgressToFileButtonCaption     = "Speichern";
			ProgressCaption                     = "Spielverlauf";
		}

		public ICommand DumpProgressToFile                => null;
		public ICommand CopyCompressedProgressToClipBoard => null;

		public ObservableCollection<string> GameProgress { get; }

		public string CompressedProgress { get; }
		public bool IsAutoScrollProgressActive { get; set; }

		public string ProgressCaption                     { get; }
		public string CompressedProgressCaption           { get; }
		public string CopyToClipboardButtonToolTipCpation { get; }
		public string AutoScrollDownCheckBoxCaption       { get; }
		public string DumpProgressToFileButtonCaption     { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}