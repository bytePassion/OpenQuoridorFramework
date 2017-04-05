using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.Commands;
using Microsoft.Win32;
using OQF.Bot.Contracts;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Info;
using OQF.Resources;
using OQF.Tournament.Contracts;
using OQF.Tournament.Contracts.DTO;
using OQF.Tournament.Contracts.Logger;

#pragma warning disable 0067

namespace OQF.Tournament.Visualization.ViewModels.Main
{
	public class MainViewModel : IMainViewModel
    {
        
        private readonly ITournament tournament;
	    private readonly IDataLogger dataLogger;


	    public MainViewModel(ITournament tournament,
							 IBoardViewModel boardViewModel,
							 IDataLogger dataLogger)
        {
            this.tournament = tournament;
	        this.dataLogger = dataLogger;
	        BoardViewModel = boardViewModel;
	        MaximumMoveDurationInSeconds = 60;
            MaximumMoveCount = 100;
	        Participants = new ObservableCollection<TournamentParticipant>();
            LogMessages = new ObservableCollection<string>();
            TournamentModes = Enum.GetValues(typeof(TournamentType)).OfType<TournamentType>();


            AddBotsToTournament = new Command(OnAddBot);
            RemoveBotFromTournament = new Command(OnRemoveBot);

            StartTournament = new Command(OnStartTournament);
            AbortTournament = new Command(OnAbortTournament);

			ShowAboutHelp = new Command(DoShowAboutHelp);

			dataLogger.OnNewLogEntry += OnNewLogEntry;
			tournament.TournamentOver += OnTournamentOver;
		}

	    private void OnNewLogEntry(LogEntry logEntry)
	    {
		    Application.Current?.Dispatcher.Invoke(() =>
		    {
				LogMessages.Add(logEntry.LogMessage);
		    });
	    }

	    private void OnRemoveBot()
        {
            // TODO
        }

        private void OnAbortTournament()
        {
            tournament.AbortTournament();
        }

        private void OnStartTournament()
        {          
			dataLogger.ClearLogger();
			LogMessages.Clear();
			  
			dataLogger.ReportLog("======================== Tournament started ========================", LogLevel.OnlyGameResults);            
	        
            tournament.StartTournament(
				Participants.ToList(), 
                new GameConstraints(TimeSpan.FromSeconds(MaximumMoveDurationInSeconds), MaximumMoveCount),
                SelectedTournamentMode
			);
        }

		private void DoShowAboutHelp ()
		{
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.Tournament.Info,								   							  
								   InfoPage.About);
		}

		private void OnAddBot()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "dll|*.dll",
                Multiselect = true
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                foreach (var dialogFileName in dialog.FileNames)
                {
                    Participants.Add(new TournamentParticipant(dialogFileName));
                }
            }
        }

	    public IBoardViewModel BoardViewModel { get; }

	    public ObservableCollection<TournamentParticipant> Participants { get; }
        public ObservableCollection<string> LogMessages { get; }
        public ICommand AddBotsToTournament { get; }
        public ICommand RemoveBotFromTournament { get; }
        public ICommand StartTournament { get; }
        public ICommand AbortTournament { get; }
	    public ICommand ShowAboutHelp { get; }
	    public TournamentType SelectedTournamentMode { get; set; }
        public IEnumerable<TournamentType> TournamentModes { get; }
        public int MaximumMoveCount { get; set; }
        public int MaximumMoveDurationInSeconds { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
			dataLogger.OnNewLogEntry -= OnNewLogEntry;
			tournament.TournamentOver -= OnTournamentOver;
		}

        private void OnTournamentOver(List<TournamentParticipant> result)
        {
			dataLogger.ReportLog("======================== Tournament over ========================", LogLevel.OnlyGameResults);

			dataLogger.AnalyzeDataAndSaveToFile("analysis.txt");
			dataLogger.SaveLogHistoryToFile("logHistory.txt");
		}       
    }
}