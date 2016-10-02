﻿using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using Microsoft.Win32;
using OQF.Bot.Contracts.GameElements;
using OQF.ReplayViewer.Services;
using OQF.Utils;
using OQF.Visualization.Common.Board.BoardViewModelBase;
using OQF.Visualization.Common.Info;
using OQF.Visualization.Resources;

namespace OQF.ReplayViewer.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly IReplayService replayService;
		private readonly ILastPlayedReplayService lastPlayedReplayService;
		private int moveIndex;
		private int maxMoveIndex;
		private string progressFilePath;
		private bool isReplayLoaded;


		public MainWindowViewModel (IBoardViewModel boardViewModel, 
									IReplayService replayService,								
									ILastPlayedReplayService lastPlayedReplayService)
		{
			BoardViewModel = boardViewModel;
			this.replayService = replayService;
			this.lastPlayedReplayService = lastPlayedReplayService;

			ProgressFilePath = lastPlayedReplayService.GetLastReplay();

			replayService.NewBoardStateAvailable += OnNewBoardStateAvailable;

			LoadGame      = new Command(DoLoadGame);
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
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			IsReplayLoaded = boardState != null;
		}

		private void DoShowAboutHelp()
		{
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.ReplayViewer.Info,
								   InfoPage.About);
		}

		private void DoNextMove()
		{
			replayService.NextMove();
			moveIndex++;
			PropertyChanged.Notify(this, nameof(MoveIndex));
		}

		private void DoPreviousMove()
		{
			replayService.PreviousMove();
			moveIndex--;
			PropertyChanged.Notify(this, nameof(MoveIndex));
		}
	
		public IBoardViewModel BoardViewModel { get; }

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

		public string ProgressFilePath
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
					ProgressFilePath = dialog.FileName;
		}

		private void DoLoadGame()
		{


			if (string.IsNullOrWhiteSpace(ProgressFilePath))
			{
				MessageBox.Show("bevor das Replay gestartet werden kann muss eine Replay-Datei ausgewählt werden");
				return;
			}

			if (!File.Exists(ProgressFilePath))
			{
				MessageBox.Show($"die datei {ProgressFilePath} existiert nicht");
				return;
			}

			string fileText;

			try
			{
				fileText = File.ReadAllText(ProgressFilePath);
			}
			catch
			{
				MessageBox.Show($"die datei {ProgressFilePath} kann nicht als text geladen werden");
				return;
			}

			var splittedMoves = ParseProgressText.FromFileText(fileText);

			if (!splittedMoves.Any())
			{
				MessageBox.Show("die datei beschreibt keinen gültigen spielverlauf");
				return;
			}


			lastPlayedReplayService.SaveLastReplay(ProgressFilePath);

			var moveCount = replayService.NewReplay(splittedMoves);

			MaxMoveIndex = moveCount - 1;
		}
				
		

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}