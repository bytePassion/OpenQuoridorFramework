using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfLib.Commands;
using WpfLib.ViewModelBase;

#pragma warning disable 0067

namespace OQF.SingleGameVisualization.ViewModels.AboutHelpWindow
{
	internal class AboutHelpWindowViewModel : ViewModel, IAboutHelpWindowViewModel
	{
		public AboutHelpWindowViewModel()
		{
			Close = new Command(CloseWindow);
		}

		private static void CloseWindow ()
		{
			var windows = Application.Current.Windows
											 .OfType<Windows.AboutHelpWindow>()
											 .ToList();

			if (windows.Count == 1)
				windows[0].Close();
			else
				throw new Exception("inner error");
		}

		public ICommand Close { get; }

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
		
	}
}
