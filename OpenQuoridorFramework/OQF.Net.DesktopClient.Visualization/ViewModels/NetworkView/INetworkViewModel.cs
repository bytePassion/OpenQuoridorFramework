using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView
{
	public interface INetworkViewModel : IViewModel
	{
		ConnectionStatus ConnectionStatus { get; }
		GameStatus GameStatus { get; }

		ICommand ConnectToServer      { get; }
		ICommand DisconnectFromServer { get; }
		ICommand CreateGame           { get; }
		ICommand CancelCreatedGame    { get; }
		ICommand JoinGame             { get; }
		ICommand LeaveGame            { get; }

		string NewGameName   { get; set; }
		string ServerAddress { get; set; }
		string PlayerName    { get; set; }		

		ObservableCollection<GameDisplayData> AvailableOpenGames { get; }
		GameDisplayData SelectedOpenGame { get; set; }

		string ServerAddressPromt                  { get; }
		string ServerAddressHint                   { get; }
		string PlayerNamePromt                     { get; }
		string PlayerNameHint                      { get; }
		string ConnectToServerButtonCaption        { get; }
		string DisconnectFromServerButtonsCaptions { get; }
		string NewGameNamePromt                    { get; }
		string NewGameNameHint                     { get; }
		string OpenGameListSectionHeader           { get; }
		string JoinGameButtonCaption               { get; }
		string CancelCreatedGameButtonCaption      { get; }
		string LeaveGameButtonCaption              { get; }		
	}
}