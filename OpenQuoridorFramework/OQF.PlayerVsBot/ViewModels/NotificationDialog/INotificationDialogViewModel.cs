using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.NotificationDialog
{
	internal interface INotificationDialogViewModel : IViewModel
	{		
		string Message       { get; }
		string ButtonCaption { get; }
	}
}