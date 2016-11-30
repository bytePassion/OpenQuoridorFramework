using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using Microsoft.Win32;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.AnalysisAndProgress.ProgressUtils.Validation;
using OQF.Bot.Contracts.GameElements;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling;
using OQF.CommonUiElements.Dialogs.Notification;
using OQF.CommonUiElements.Info;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.ReplayViewer.Contracts;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow.Helper;
using OQF.Resources;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.ReplayViewer.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly IReplayService replayService;
		private readonly IApplicationSettingsRepository applicationSettingsRepository;

		private int moveIndex;
		private int maxMoveIndex;
		private string progressFilePath;
		private bool isReplayLoaded;
		

		public MainWindowViewModel (IBoardViewModel boardViewModel,
									ILanguageSelectionViewModel languageSelectionViewModel,
									IBoardLabelingViewModel boardHorizontalLabelingViewModel, 
									IBoardLabelingViewModel boardVerticalLabelingViewModel,
									IReplayService replayService,
									IApplicationSettingsRepository applicationSettingsRepository)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			BoardViewModel = boardViewModel;
			this.replayService = replayService;
			this.applicationSettingsRepository = applicationSettingsRepository;
			BoardHorizontalLabelingViewModel = boardHorizontalLabelingViewModel;
			BoardVerticalLabelingViewModel = boardVerticalLabelingViewModel;

			LanguageSelectionViewModel = languageSelectionViewModel;

			LodingString = applicationSettingsRepository.LastPlayedReplayString;

			replayService.NewBoardStateAvailable += OnNewBoardStateAvailable;

			LoadGame      = new Command(DoLoadGame,
										() => !string.IsNullOrWhiteSpace(LodingString),
										new PropertyChangedCommandUpdater(this, nameof(LodingString)));
			BrowseFile    = new Command(DoBrowseFile);
			NextMove      = new Command(DoNextMove,
									    () => IsReplayLoaded && MoveIndex < MaxMoveIndex,
										new PropertyChangedCommandUpdater(this, nameof(IsReplayLoaded), 
																				nameof(MoveIndex), 
																				nameof(MaxMoveIndex)));
			PreviousMove  = new Command(DoPreviousMove,
										() => IsReplayLoaded && MoveIndex > 0,
										new PropertyChangedCommandUpdater(this, nameof(IsReplayLoaded), 
																				nameof(MoveIndex)));
			ShowAboutHelp = new Command(DoShowAboutHelp);

			ProgressRows = new ObservableCollection<ProgressRow>();
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			IsReplayLoaded = boardState != null;
		}

		private void DoShowAboutHelp()
		{
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.ReplayViewer.Info,
								   InfoPage.ReplayViewerApplicationInfo,
								   InfoPage.QuoridorRules,
								   InfoPage.QuoridorNotation,								   
								   InfoPage.About);
		}

		private void DoNextMove()
		{
			if (MoveIndex >= MaxMoveIndex)
				return;

			replayService.NextMove();
			moveIndex++;
			SetHighlightning(MoveIndex);
			PropertyChanged.Notify(this, nameof(MoveIndex));
		}

		private void DoPreviousMove()
		{
			if (MoveIndex <= 0)
				return;

			replayService.PreviousMove();
			moveIndex--;
			SetHighlightning(MoveIndex);
			PropertyChanged.Notify(this, nameof(MoveIndex));
		}
	
		public IBoardViewModel BoardViewModel { get; }
		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }
		public IBoardLabelingViewModel BoardHorizontalLabelingViewModel { get; }
		public IBoardLabelingViewModel BoardVerticalLabelingViewModel { get; }

		public ICommand LoadGame      { get; }
		public ICommand BrowseFile    { get; }
		public ICommand ShowAboutHelp { get; }
		public ICommand NextMove      { get; }
		public ICommand PreviousMove  { get; }

		public int MoveIndex
		{
			get { return moveIndex; }
			set
			{
				if (moveIndex != value)
				{
					SetHighlightning(value);
					replayService.JumpToMove(value);
				}

				PropertyChanged.ChangeAndNotify(this, ref moveIndex, value);
			}
		}

		public int MaxMoveIndex
		{
			get { return maxMoveIndex; }
			private set { PropertyChanged.ChangeAndNotify(this, ref maxMoveIndex, value);}
		}

		public bool IsReplayLoaded
		{
			get { return isReplayLoaded; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isReplayLoaded, value); }
		}

		public ObservableCollection<ProgressRow> ProgressRows { get; }

		public string LodingString
		{
			get { return progressFilePath; }
			set { PropertyChanged.ChangeAndNotify(this, ref progressFilePath, value); }
		}		

		private void DoBrowseFile()
		{
			var dialog = new OpenFileDialog
			{
				Filter = "text-file|*.txt"
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
				if (result.Value)
					LodingString = dialog.FileName;
		}



		private async Task LoadGameFromString ()
		{
			var progress = CreateQProgress.FromCompressedProgressString(LodingString);
			await LoadProgress(progress);
		}

		private async Task LoadGameFromFile()
		{
			
			if (!File.Exists(LodingString))
			{
				await NotificationDialogService.Show($"{Captions.ErrorMsg_FileDoesNotExist} [{LodingString}]", 
													 Captions.ND_OkButtonCaption);				
				return;
			}

			string fileText;

			try
			{
				fileText = File.ReadAllText(LodingString);
			}
			catch
			{
				await NotificationDialogService.Show($"{Captions.ErrorMsg_FileCannotBeLoadedAsText} [{LodingString}]",
													 Captions.ND_OkButtonCaption);
				return;
			}

			var progress = CreateQProgress.FromReadableProgressTextFile(fileText);
			await LoadProgress(progress);
		}

		private async Task LoadProgress(QProgress progress)
		{
			var verificationResult = ProgressVerifier.Verify(progress,
															 int.MaxValue,
															 false);

			if (verificationResult.Result != VerificationResult.Valid)
			{
				await NotificationDialogService.Show(verificationResult.ErrorMessage,
													 Captions.ND_OkButtonCaption);
				return;
			}

			applicationSettingsRepository.LastPlayedReplayString = LodingString;			

			replayService.NewReplay(progress);

			ProgressRows.Clear();
			ProgressRows.Add(new ProgressRow(string.Empty, true));
			
			CreateProgressText.FromMoveList(progress.Moves.Select(move => move.ToString()).ToList())
							  .Select(line => new ProgressRow(line, false))
							  .Do(ProgressRows.Add);

			moveIndex = 0;
			MaxMoveIndex = replayService.MoveCount - 1;

			PropertyChanged.Notify(this, nameof(MoveIndex));
		}

		private async void DoLoadGame()
		{
			if (IsStringAFilePath(LodingString))
			{
				await LoadGameFromFile();	
			}
			else
			{
				await LoadGameFromString();
			}

			SetHighlightning(MoveIndex);
		}		

		private bool IsStringAFilePath(string s)
		{
			return s.Contains(".") || s.Contains("\\") || s.Contains("/");			
		}

		private void ClearHightning()
		{
			foreach (var progressRow in ProgressRows)
			{
				progressRow.HighlightBottomPlayerMove = false;
				progressRow.HighlightTopPlayerMove    = false;
			}
		}

		private void SetHighlightning(int index)
		{
			ClearHightning();

			if (index == 0)
			{
				ProgressRows[0].HighlightBottomPlayerMove = true;
			}
			else
			{
				var rowIndex = (int)Math.Floor((index-1)/2.0);
				var progressRow = ProgressRows[rowIndex+1];
				
				if ((index-1)%2 == 0)
					progressRow.HighlightBottomPlayerMove = true;
				else
					progressRow.HighlightTopPlayerMove = true;
			}						
		}


		public string BrowseFileButtonCaption    => Captions.RV_BrowseFileButtonCaption;
		public string InputPromtLabelCaption     => Captions.RV_InputPromtLabelCaption;
		public string LoadAndStartButtonCaption  => Captions.RV_LoadAndStartButtonCaption;
		public string NextStepButtonCaption      => Captions.RV_NextStepButtonCaption;
		public string PrevStepButtonCaption      => Captions.RV_PrevStepButtonCaption;
		public string ProgressSectionHeader      => Captions.RV_ProgressSectionHeader;
		public string ShowAboutHelpButtonCaption => Captions.RV_ShowAboutHelpButtonCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(BrowseFileButtonCaption),
										 nameof(InputPromtLabelCaption),
										 nameof(LoadAndStartButtonCaption),
										 nameof(NextStepButtonCaption),
										 nameof(PrevStepButtonCaption),
										 nameof(ProgressSectionHeader),										 
										 nameof(ShowAboutHelpButtonCaption));
		}	

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
			replayService.NewBoardStateAvailable -= OnNewBoardStateAvailable;
		}

		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}