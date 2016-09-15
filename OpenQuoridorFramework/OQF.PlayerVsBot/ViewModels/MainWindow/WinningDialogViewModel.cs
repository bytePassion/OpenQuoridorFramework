using System.ComponentModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.MainWindow
{
    class WinningDialogViewModel : ViewModel
    {
        private readonly string message;

        public WinningDialogViewModel(string message)
        {
            this.message = message;
        }

        public string Message => message;

        protected override void CleanUp()
        {
        }

        public override event PropertyChangedEventHandler PropertyChanged;
    }
}