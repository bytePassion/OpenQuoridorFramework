using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Dialogs.YesNo.ViewModel
{
	internal interface IYesNoDialogViewModel : IViewModel
	{
		string Message { get; }

		string YesButtonCaption { get; }
		string NoButtonCaption  { get; }
	}
}