using System.ComponentModel;
using OQF.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Dialogs.StringInput.ViewModel
{
	internal class StringInputDialogViewModel : bytePassion.Lib.WpfLib.ViewModelBase.ViewModel, IStringInputDialogViewModel
	{
		public StringInputDialogViewModel(string promt)
		{
			Promt = promt;
		}

		public string Promt { get; }

		public string OkButtonCaption     => Captions.SID_OkButtonCaption;
		public string CancelButtonCaption => Captions.SID_CancelButtonCaption;

		protected override void CleanUp() {}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
