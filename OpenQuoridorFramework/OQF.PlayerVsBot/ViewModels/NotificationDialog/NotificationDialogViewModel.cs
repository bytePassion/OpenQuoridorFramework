using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.NotificationDialog
{
	internal class NotificationDialogViewModel : ViewModel, INotificationDialogViewModel
	{
		public NotificationDialogViewModel(string message, string buttonCaption)
		{			
			Message = message;
			ButtonCaption = buttonCaption;
		}
		
		public string Message       { get; }
		public string ButtonCaption { get; }

		protected override void CleanUp() {}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
