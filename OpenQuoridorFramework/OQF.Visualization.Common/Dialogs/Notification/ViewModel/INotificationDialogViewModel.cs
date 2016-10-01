﻿using Lib.Wpf.ViewModelBase;

namespace OQF.Visualization.Common.Dialogs.Notification.ViewModel
{
	internal interface INotificationDialogViewModel : IViewModel
	{		
		string Message       { get; }
		string ButtonCaption { get; }
	}
}