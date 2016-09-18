using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal class WinningDialogViewModelSampleData : IWinningDialogViewModel
	{
		public WinningDialogViewModelSampleData()
		{
			Message = "dialog-Message";
		}

		public string Message { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}