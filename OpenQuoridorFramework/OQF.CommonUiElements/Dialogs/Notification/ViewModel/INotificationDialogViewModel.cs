using bytePassion.Lib.WpfLib.ViewModelBase;

namespace OQF.CommonUiElements.Dialogs.Notification.ViewModel
{
	internal interface INotificationDialogViewModel : IViewModel
	{		
		string Message       { get; }
		string ButtonCaption { get; }
	}
}