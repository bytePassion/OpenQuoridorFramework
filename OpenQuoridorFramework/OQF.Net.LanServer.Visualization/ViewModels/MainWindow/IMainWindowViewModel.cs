﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		ICommand ActivateServer   { get; }
		ICommand DeactivateServer { get; }

		ObservableCollection<string> Output { get; }
	}
}