using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using Microsoft.Win32;
using OQF.PlayerVsBot.Contracts;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView
{
	public class DebugMessageViewModel : ViewModel, IDebugMessageViewModel
	{
		private readonly IGameService gameService;
		private bool isAutoScrollDebugMsgActive;

		public DebugMessageViewModel(IGameService gameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;

			gameService.NewDebugMsgAvailable += OnNewDebugMsgAvailable;

			DebugMessages = new ObservableCollection<string>();
			IsAutoScrollDebugMsgActive = true;
			DumpDebugToFile = new Command(DoDumpDebugToFile);
		}

		private void OnNewDebugMsgAvailable(string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				DebugMessages.Add(s);
			});
		}

		public ICommand DumpDebugToFile { get; }

		public ObservableCollection<string> DebugMessages { get; }

		public bool IsAutoScrollDebugMsgActive
		{
			get { return isAutoScrollDebugMsgActive; }
			set { PropertyChanged.ChangeAndNotify(this, ref isAutoScrollDebugMsgActive, value); }
		}


		private void DoDumpDebugToFile ()
		{
			var dialog = new SaveFileDialog()
			{
				Filter = "textFiles |*.txt",
				AddExtension = true,
				CheckFileExists = false,
				OverwritePrompt = true,
				ValidateNames = true,
				CheckPathExists = true,
				CreatePrompt = false,
				Title = Captions.PvB_DumpDebugFileDialogTitle
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
			{
				if (result.Value)
				{
					var sb = new StringBuilder();

					for (int index = 0; index < DebugMessages.Count; index++)
					{
						var debugMessage = DebugMessages[index];
						sb.Append(debugMessage);

						if (index != DebugMessages.Count - 1)
							sb.Append(Environment.NewLine);
					}

					File.WriteAllText(dialog.FileName, sb.ToString());
				}
			}
		}

		public string AutoScrollDownCheckBoxCaption => Captions.PvB_AutoScrollDownCheckBoxCaption;
		public string DumpDebugToFileButtonCaption  => Captions.PvB_DumpDebugToFileButtonCaption;
		public string DebugCaption                  => Captions.PvB_DebugCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(AutoScrollDownCheckBoxCaption),
										 nameof(DebugCaption),
										 nameof(DumpDebugToFileButtonCaption));
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			gameService.NewDebugMsgAvailable -= OnNewDebugMsgAvailable;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
