using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Dialogs.Notification.ViewModel
{
	public class NotificationDialogViewModel : Lib.Wpf.ViewModelBase.ViewModel, INotificationDialogViewModel
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
