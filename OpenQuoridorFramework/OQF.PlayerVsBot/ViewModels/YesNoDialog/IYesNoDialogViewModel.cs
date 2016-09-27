using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.YesNoDialog
{
	internal interface IYesNoDialogViewModel : IViewModel
	{
		string Message { get; }

		string YesButtonCaption { get; }
		string NoButtonCaption  { get; }
	}
}