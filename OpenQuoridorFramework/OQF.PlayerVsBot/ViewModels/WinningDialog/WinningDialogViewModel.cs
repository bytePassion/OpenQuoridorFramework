using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal class WinningDialogViewModel : ViewModel, IWinningDialogViewModel
    {
	    public WinningDialogViewModel(string message)
        {
            Message = message;
        }

        public string Message { get; }

	    protected override void CleanUp() {}
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}