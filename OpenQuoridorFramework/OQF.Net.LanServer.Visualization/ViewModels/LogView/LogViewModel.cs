﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.Net.LanServer.Contracts;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.LogView
{
	public class LogViewModel : ViewModel, ILogViewModel
	{
		private readonly INetworkGameServer networkGameServer;

		public LogViewModel(INetworkGameServer networkGameServer)
		{
			this.networkGameServer = networkGameServer;
			Output = new ObservableCollection<string>();

			networkGameServer.NewOutputAvailable += OnNewOutputAvailable;
		}

		private void OnNewOutputAvailable (string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Output.Add(s);
			});
		}

		public ObservableCollection<string> Output { get; }

		protected override void CleanUp ()
		{
			networkGameServer.NewOutputAvailable -= OnNewOutputAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
