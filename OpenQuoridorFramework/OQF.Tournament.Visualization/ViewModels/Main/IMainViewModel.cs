using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.Tournament.Contracts.DTO;

namespace OQF.Tournament.Visualization.ViewModels.Main
{
	public interface IMainViewModel : IViewModel
    {
		IBoardViewModel BoardViewModel { get; }

        ObservableCollection<TournamentParticipant> Participants { get; }
        ObservableCollection<string> LogMessages { get; }

        ICommand AddBotsToTournament { get; }
        ICommand RemoveBotFromTournament { get; }
        ICommand StartTournament { get; }
        ICommand AbortTournament { get; }
    }
}
