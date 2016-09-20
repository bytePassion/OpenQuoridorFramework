using System.ComponentModel;
using System.Text;
using Lib.Wpf.ViewModelBase;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal class WinningDialogViewModel : ViewModel, IWinningDialogViewModel
    {
	    public WinningDialogViewModel(bool reportWinning, WinningReason winningReason, Move invalidMove)
        {
			var sb = new StringBuilder();

			if (reportWinning)
				sb.Append($"{Captions.WD_WinningMessage}\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}");

			if (!reportWinning)
				sb.Append($"{Captions.WD_LoosingMessage}\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}");

	        if (winningReason == WinningReason.InvalidMove)
		        sb.Append($" [{invalidMove}]");

			sb.Append($"\n\n{Captions.WD_SaveGameRequest}");

	        Message = sb.ToString();
        }

        public string Message { get; }

	    public string YesButtonCaption => Captions.WD_YesButtonCaption;
	    public string NoButtonCaption  => Captions.WD_NoButtonCaption;		

	    private string WinningReasonToString(WinningReason winningReason)
	    {
		    switch (winningReason)
		    {
			    case WinningReason.Capitulation:            return Captions.WinningReason_Capitulation;				
				case WinningReason.InvalidMove:             return Captions.WinningReason_InvalidMode;
				case WinningReason.ExceedanceOfMaxMoves:    return Captions.WinningReason_ExceedanceOfMaxMoves;
				case WinningReason.ExceedanceOfThoughtTime: return Captions.WinningReason_ExceedanceOfThoughtTime;
				case WinningReason.RegularQuoridorWin:      return Captions.WinningReason_RegularQuoridorWin;			    
		    }

		    return "";
	    }

		protected override void CleanUp() {}
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}