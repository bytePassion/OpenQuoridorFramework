﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView
{
	public interface INetworkViewModel : IViewModel
	{
		ICommand ConnectToServer { get; }
		ICommand CreateGame { get; }
		ICommand JoinGame { get; }

		string NewGameName { get; set; }
		string ServerAddress { get; set; }
		string PlayerName { get; set; }

		string Response { get; }

		ObservableCollection<GameDisplayData> AvailableOpenGames { get; }
		GameDisplayData SelectedOpenGame { get; set; }
	}
}