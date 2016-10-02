using System.Collections.Generic;
using System.ComponentModel;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts;

namespace OQF.Application.Tournament
{
    public interface IMainViewModel : IViewModel
    {
        IEnumerable<IQuoridorBot> TournamentBots { get; }
    }

    public class MainViewModel : IMainViewModel
    {
        public MainViewModel(IEnumerable<IQuoridorBot> bots)
        {
            this.TournamentBots = bots;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Dispose()
        {
            
        }

        public IEnumerable<IQuoridorBot> TournamentBots { get; }
    }
}
