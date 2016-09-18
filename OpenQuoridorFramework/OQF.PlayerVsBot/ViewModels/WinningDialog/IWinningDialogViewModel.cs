using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal interface IWinningDialogViewModel : IViewModel
	{
		string Message { get; }

		string YesButtonCaption { get; }
		string NoButtonCaption  { get; }
	}
}