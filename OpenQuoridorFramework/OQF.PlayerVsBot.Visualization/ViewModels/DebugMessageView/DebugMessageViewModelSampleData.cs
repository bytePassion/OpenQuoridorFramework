using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView
{
	internal class DebugMessageViewModelSampleData : IDebugMessageViewModel
	{
		public DebugMessageViewModelSampleData()
		{
			DebugMessages = new ObservableCollection<string>
			{
				"blubb1",
				"blubb2",
				"blubb3",
				"blubb4",
				"blubb5"
			};

			IsAutoScrollDebugMsgActive = true;

			AutoScrollDownCheckBoxCaption = "Automatisch scrollen";
			DumpDebugToFileButtonCaption  = "Speichern";
			DebugCaption                  = "Debug";
		}

		public ICommand DumpDebugToFile => null;

		public ObservableCollection<string> DebugMessages { get; }

		public bool IsAutoScrollDebugMsgActive { get; set; }

		public string AutoScrollDownCheckBoxCaption { get; }
		public string DumpDebugToFileButtonCaption  { get; }
		public string DebugCaption                  { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}