using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal class WinningDialogViewModelSampleData : IWinningDialogViewModel
	{
		public WinningDialogViewModelSampleData()
		{
			Message = "dialog-Message";

			YesButtonCaption = "Ja";
			NoButtonCaption = "Nein";
		}

		public string Message { get; }

		public string YesButtonCaption { get; }
		public string NoButtonCaption  { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}