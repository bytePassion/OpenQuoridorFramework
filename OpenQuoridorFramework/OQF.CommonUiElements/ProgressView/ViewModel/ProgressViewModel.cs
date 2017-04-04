using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.Commands.Updater;
using Microsoft.Win32;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.CommonUiElements.Board.ViewModels;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.CommonUiElements.ProgressView.ViewModel
{
	public class ProgressViewModel : bytePassion.Lib.WpfLib.ViewModelBase.ViewModel, IProgressViewModel
	{
		private readonly IBoardStateProvider gameService;

		private bool isAutoScrollProgressActive;
		private string compressedProgress;

		public ProgressViewModel(IBoardStateProvider gameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;

			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;

			GameProgress = new ObservableCollection<string>();
			IsAutoScrollProgressActive = true;

			CopyCompressedProgressToClipBoard = new Command(DoCopyCompressedProgressToClipBoard,
															() => !string.IsNullOrWhiteSpace(CompressedProgress),
															new PropertyChangedCommandUpdater(this, nameof(CompressedProgress)));

			DumpProgressToFile = new Command(DoDumpProgressToFile);
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (boardState == null)
				{
					GameProgress.Clear();
					CompressedProgress = "";
				}
				else
				{
					if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
					{
						if (GameProgress.Count > 0)
						{
							GameProgress[GameProgress.Count - 1] = GameProgress[GameProgress.Count - 1] + $" {boardState.LastMove}";
							
							CompressedProgress = CreateQProgress.FromBoardState(boardState).Compressed;
						}
					}
					else
					{
						GameProgress.Add($"{GameProgress.Count + 1}: " + $"{boardState.LastMove}");
						
						CompressedProgress = CreateQProgress.FromBoardState(boardState).Compressed;
					}
				}
			});
		}

		public ICommand DumpProgressToFile                { get; }
		public ICommand CopyCompressedProgressToClipBoard { get; }

		public ObservableCollection<string> GameProgress { get; }

		public string CompressedProgress
		{
			get { return compressedProgress; }
			private set { PropertyChanged.ChangeAndNotify(this, ref compressedProgress, value); }
		}


		public bool IsAutoScrollProgressActive
		{
			get { return isAutoScrollProgressActive; }
			set { PropertyChanged.ChangeAndNotify(this, ref isAutoScrollProgressActive, value); }
		}

		private void DoCopyCompressedProgressToClipBoard ()
		{
			Clipboard.SetText(CompressedProgress);
		}

		private void DoDumpProgressToFile ()
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
				Title = Captions.PvB_DumpProgressFileDialogTitle
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
			{
				if (result.Value)
				{
					var fileText = CreateProgressText.FromBoardState(gameService.CurrentBoardState);
					File.WriteAllText(dialog.FileName, fileText);
				}
			}
		}

		public string ProgressCaption                      => Captions.PvB_ProgressCaption;
		public string CompressedProgressCaption            => Captions.PvB_CompressedProgressCaption;
		public string CopyToClipboardButtonToolTipCpation  => Captions.PvB_CopyToClipboardButtonToolTipCpation;
		public string AutoScrollDownCheckBoxCaption        => Captions.PvB_AutoScrollDownCheckBoxCaption;
		public string DumpProgressToFileButtonCaption      => Captions.PvB_DumpProgressToFileButtonCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(AutoScrollDownCheckBoxCaption),										
										 nameof(CompressedProgressCaption),
										 nameof(ProgressCaption),
										 nameof(DumpProgressToFileButtonCaption),
										 nameof(CopyToClipboardButtonToolTipCpation));
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
