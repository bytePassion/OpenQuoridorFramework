using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView
{
	public interface IDebugMessageViewModel : IViewModel
	{
		ICommand DumpDebugToFile { get; }

		ObservableCollection<string> DebugMessages { get; }

		bool IsAutoScrollDebugMsgActive { get; set; }

		string DebugCaption                  { get; }
		string AutoScrollDownCheckBoxCaption { get; }
		string DumpDebugToFileButtonCaption  { get; }
	}
}