using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Lib.Wpf.Commands;
using OQF.Application.Tournament.Services;

namespace OQF.Application.Tournament.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        private readonly ITournamentService service;

        public MainViewModel(ITournamentService service)
        {
            this.service = service;
            TournamentBots = new ObservableCollection<BotData>();
            TournamentBots.Add(new BotData {Name = "Bot1"});
            TournamentBots.Add(new BotData {Name = "Bot2"});

            StartTournament = new Command(() => service.StartTournament(TournamentBots.ToList()));
        }

        public ICommand StartTournament { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Dispose()
        {
            
        }

        public ObservableCollection<BotData> TournamentBots { get; } 
    }

    public class BotData
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}