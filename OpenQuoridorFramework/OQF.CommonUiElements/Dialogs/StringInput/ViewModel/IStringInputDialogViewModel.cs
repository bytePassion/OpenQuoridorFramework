﻿using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Dialogs.StringInput.ViewModel
{
	internal interface IStringInputDialogViewModel : IViewModel
	{
		string Promt { get; }

		string OkButtonCaption { get; }
		string CancelButtonCaption { get; }
	}
}