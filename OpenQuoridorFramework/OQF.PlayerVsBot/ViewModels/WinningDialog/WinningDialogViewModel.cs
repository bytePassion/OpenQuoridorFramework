using System.ComponentModel;
using Lib.Wpf.ViewModelBase;
using OQF.GameEngine.Contracts;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal class WinningDialogViewModel : ViewModel, IWinningDialogViewModel
    {
	    public WinningDialogViewModel(bool reportWinning, WinningReason winningReason)
        {
			var winOrLooseMsg = reportWinning
									? $"{Captions.WD_WinningMessage}\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}"
									: $"{Captions.WD_LoosingMessage}\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}";

	        Message = $"{winOrLooseMsg}\n\n{Captions.WD_SaveGameRequest}";			
        }

        public string Message { get; }

	    public string YesButtonCaption => Captions.WD_YesButtonCaption;
	    public string NoButtonCaption  => Captions.WD_NoButtonCaption;		

	    private string WinningReasonToString(WinningReason winningReason)
	    {
		    switch (winningReason)
		    {
			    case WinningReason.Capitulation: return Captions.WinningReason_Capitulation;
				case WinningReason.AiThougtMoreThanAnMinute: return Captions.WinningReason_BotExceededThoughtTime;
				case WinningReason.InvalidMove: return Captions.WinningReason_InvalidMode;
				case WinningReason.MaximumOfMovesExceded: return Captions.WinningReason_ExceedanceOfMaxMoves;
				case WinningReason.RegularQuoridorWin: return Captions.WinningReason_RegularQuoridorWin;			    
		    }

		    return "";
	    }

		protected override void CleanUp() {}
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}