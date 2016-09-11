﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using FrameworkExtensionLib;
using OQF.Contest.Contracts.GameElements;
using OQF.ReplayViewer.Services;
using WpfLib.ViewModelBase;

namespace OQF.ReplayViewer.ViewModels.Board
{
	internal class BoardViewModel : ViewModel, IBoardViewModel
	{
		private readonly IReplayService replayService;
				
		private Size boardSize;

		public BoardViewModel(IReplayService replayService)
		{
			this.replayService = replayService;

			VisiblePlayers = new ObservableCollection<PlayerState>();
			VisibleWalls   = new ObservableCollection<Wall>();

			replayService.NewBoardStateAvailable += OnDisplayedBoardStateVariableChanged;
			OnDisplayedBoardStateVariableChanged(replayService.GetCurrentBoardState());
									
			BoardSize = new Size(100,100);			
		}
		
		private void OnDisplayedBoardStateVariableChanged(BoardState newBoardState)
		{
			VisiblePlayers.Clear();
			VisibleWalls.Clear();

			if (newBoardState != null)
			{
				newBoardState.PlacedWalls.Do(VisibleWalls.Add);

				VisiblePlayers.Add(newBoardState.TopPlayer);
				VisiblePlayers.Add(newBoardState.BottomPlayer);
			}
		}		

		public ObservableCollection<Wall>        VisibleWalls   { get; }
		public ObservableCollection<PlayerState> VisiblePlayers { get; }

		public Size BoardSize
		{
			get { return boardSize; }
			set { PropertyChanged.ChangeAndNotify(this, ref boardSize, value); }
		}

		protected override void CleanUp()
		{
			replayService.NewBoardStateAvailable -= OnDisplayedBoardStateVariableChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
