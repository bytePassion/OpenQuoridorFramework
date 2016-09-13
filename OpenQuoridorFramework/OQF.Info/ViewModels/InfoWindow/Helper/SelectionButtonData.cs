using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.Info.ViewModels.InfoWindow.Helper
{
	internal class SelectionButtonData : INotifyPropertyChanged
	{
		public SelectionButtonData(ICommand command, string buttonCaption)
		{
			Command = command;
			ButtonCaption = buttonCaption;
		}

		public ICommand Command { get; }
		public string ButtonCaption { get; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
