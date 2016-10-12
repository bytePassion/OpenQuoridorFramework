using System.ComponentModel;
using OQF.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Dialogs.YesNo.ViewModel
{
	public class YesNoDialogViewModel : Lib.Wpf.ViewModelBase.ViewModel, IYesNoDialogViewModel
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