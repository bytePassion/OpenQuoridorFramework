using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.Visualization.ViewModels.ProgressView
{
	public interface IProgressViewModel : IViewModel
	{
		ICommand DumpProgressToFile                { get; }
		ICommand CopyCompressedProgressToClipBoard { get; }

		ObservableCollection<string> GameProgress { get; }

		string CompressedProgress { get; }

		bool IsAutoScrollProgressActive { get; set; }

		string ProgressCaption                     { get; }
		string CompressedProgressCaption           { get; }
		string CopyToClipboardButtonToolTipCpation { get; }
		string AutoScrollDownCheckBoxCaption       { get; }
		string DumpProgressToFileButtonCaption     { get; }
	}
}