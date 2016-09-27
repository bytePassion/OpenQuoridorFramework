using System.ComponentModel;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.YesNoDialog
{
	internal class YesNoDialogViewModel : ViewModel, IYesNoDialogViewModel
    {
	    public YesNoDialogViewModel(string message)
	    {
		    Message = message;
	    }
		
        public string Message { get; }

	    public string YesButtonCaption => Captions.WD_YesButtonCaption;
	    public string NoButtonCaption  => Captions.WD_NoButtonCaption;			    

		protected override void CleanUp() {}
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}