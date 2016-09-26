using System.ComponentModel;
using System.Text;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;
using OQF.Visualization.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.WinningDialog
{
	internal class WinningDialogViewModel : ViewModel, IWinningDialogViewModel
    {
	    public WinningDialogViewModel(bool reportWinning, WinningReason winningReason, Move invalidMove)
        {
			var sb = new StringBuilder();

	        sb.Append(reportWinning 
							? $"{Captions.WD_WinningMessage}" 
							: $"{Captions.WD_LoosingMessage}");

	        sb.Append($"\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}");

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