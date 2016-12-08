using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using Microsoft.Win32;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.Board.ViewModels.BoardPlacement;
using OQF.CommonUiElements.Dialogs.YesNo;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar;
using OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar;
using OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView;
using OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;
using OQF.Utils.Enum;

namespace OQF.PlayerVsBot.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly IGameService gameService;
		private readonly IApplicationSettingsRepository applicationSettingsRepository;		
		private readonly bool disableClosingDialog;
				
		private bool isDisabledOverlayVisible;
		private bool isProgressSectionExpanded;
		private bool isDebugSectionExpanded;
		
						
		public MainWindowViewModel (IBoardViewModel boardViewModel, 
									IBoardPlacementViewModel boardPlacementViewModel,									
									IActionBarViewModel actionBarViewModel,
									IBotStatusBarViewModel botStatusBarViewModel,
									IHumanPlayerBarViewModel humanPlayerBarViewModel,
									IProgressViewModel progressViewModel,
									IDebugMessageViewModel debugMessageViewModel,
									IBoardLabelingViewModel boardHorizontalLabelingViewModel, 
									IBoardLabelingViewModel boardVerticalLabelingViewModel,
									IGameService gameService, 
									IApplicationSettingsRepository applicationSettingsRepository,									
									bool disableClosingDialog)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;
			this.applicationSettingsRepository = applicationSettingsRepository;			
			this.disableClosingDialog = disableClosingDialog;
			BoardHorizontalLabelingViewModel = boardHorizontalLabelingViewModel;
			BoardVerticalLabelingViewModel = boardVerticalLabelingViewModel;

			DebugMessageViewModel      = debugMessageViewModel;
			ProgressViewModel          = progressViewModel;
			HumanPlayerBarViewModel    = humanPlayerBarViewModel;
			BotStatusBarViewModel      = botStatusBarViewModel;
			ActionBarViewModel         = actionBarViewModel;			
			BoardPlacementViewModel    = boardPlacementViewModel;
			BoardViewModel             = boardViewModel;
						
			gameService.WinnerAvailable        += OnWinnerAvailable;
			gameService.NewGameStatusAvailable += OnNewGameStatusAvailable;
						
			PreventWindowClosingToAskUser = !disableClosingDialog;			
			
			CloseWindow = new Command(DoCloseWindow);		
			
			IsDebugSectionExpanded    = applicationSettingsRepository.IsDebugSectionExpanded;
			IsProgressSectionExpanded = applicationSettingsRepository.IsProgressSecionExpanded;			
		}

		private void OnNewGameStatusAvailable(GameStatus gameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				IsDisabledOverlayVisible = gameStatus != GameStatus.Active;
				PreventWindowClosingToAskUser = !disableClosingDialog && gameStatus == GameStatus.Active;
			});			
		}


		private void DoCloseWindow()
		{
			PreventWindowClosingToAskUser = false;
			Application.Current.Windows
							   .OfType<Windows.MainWindow>()
							   .FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
							   ?.Close();
		}

				
		private static string GetWinningOrLoosingMessage(bool reportWinning, WinningReason winningReason, Move invalidMove)
		{
			var sb = new StringBuilder();

			sb.Append(reportWinning
							? $"{Captions.WD_WinningMessage}"
							: $"{Captions.WD_LoosingMessage}");

			sb.Append($"\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}");

			if (winningReason == WinningReason.InvalidMove)
				sb.Append($" [{invalidMove}]");

			sb.Append($"\n\n{Captions.WD_SaveGameRequest}");

			return sb.ToString();
		}

		private static string WinningReasonToString(WinningReason winningReason)
	    {
		    switch (winningReason)
		    {
			    case WinningReason.Capitulation:            return Captions.WinningReason_Capitulation;				
				case WinningReason.InvalidMove:             return Captions.WinningReason_InvalidMode;
				case WinningReason.ExceedanceOfMaxMoves:    return Captions.WinningReason_ExceedanceOfMaxMoves;
				case WinningReason.ExceedanceOfThoughtTime: return Captions.WinningReason_ExceedanceOfThoughtTime;
				case WinningReason.RegularQuoridorWin:      return Captions.WinningReason_RegularQuoridorWin;			    
		    }

		    return "";
	    }

		private async void ExecuteWinDialog(bool reportWinning, Player player, WinningReason winningReason, Move invalidMove)
		{
			var dialogResult = await YesNoDialogService.Show(GetWinningOrLoosingMessage(reportWinning, winningReason, invalidMove));
			 
            if (dialogResult)
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
                    Title = Captions.PvB_SaveGameProgressFileDialogTitle
                };

                var result = dialog.ShowDialog();

                if (result.HasValue)
                {
                    if (result.Value)
                    {
	                    var fileText = CreateProgressText.FromBoardState(gameService.CurrentBoardState)
                                                         .AndAppendWinnerAndReason(player, winningReason, invalidMove);

                        File.WriteAllText(dialog.FileName, fileText);
                    }
                }
            }            			
        }

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{				
				var reportWinning = player.PlayerType == PlayerType.BottomPlayer;						
				ExecuteWinDialog(reportWinning, player, winningReason, invalidMove);				
			});			
		}
		
		public IBoardViewModel             BoardViewModel                   { get; }
		public IBoardPlacementViewModel    BoardPlacementViewModel          { get; }		
		public IActionBarViewModel         ActionBarViewModel               { get; }
		public IBotStatusBarViewModel      BotStatusBarViewModel            { get; }
		public IHumanPlayerBarViewModel    HumanPlayerBarViewModel          { get; }
		public IProgressViewModel          ProgressViewModel                { get; }
		public IDebugMessageViewModel      DebugMessageViewModel            { get; }
		public IBoardLabelingViewModel     BoardHorizontalLabelingViewModel { get; }
		public IBoardLabelingViewModel     BoardVerticalLabelingViewModel   { get; }

		public ICommand CloseWindow { get; }
						
		public bool IsProgressSectionExpanded
		{
			get { return isProgressSectionExpanded; }
			set
			{
				if (value != isProgressSectionExpanded)				
					applicationSettingsRepository.IsProgressSecionExpanded = value;
				
				PropertyChanged.ChangeAndNotify(this, ref isProgressSectionExpanded, value);
			}
		}

		public bool IsDebugSectionExpanded
		{
			get { return isDebugSectionExpanded; }
			set
			{
				if (value != isDebugSectionExpanded)				
					applicationSettingsRepository.IsDebugSectionExpanded = value;
				
				PropertyChanged.ChangeAndNotify(this, ref isDebugSectionExpanded, value);
			}
		}

		public bool IsDisabledOverlayVisible
		{
			get { return isDisabledOverlayVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isDisabledOverlayVisible, value); }
		}
		
		public bool PreventWindowClosingToAskUser { get; private set; }

		public string ProgressCaption => Captions.PvB_ProgressCaption;		
		public string DebugCaption    => Captions.PvB_DebugCaption;	
				
		private void RefreshCaptions()
		{
			PropertyChanged.Notify(this, nameof(DebugCaption),										 								 																			 
										 nameof(ProgressCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}