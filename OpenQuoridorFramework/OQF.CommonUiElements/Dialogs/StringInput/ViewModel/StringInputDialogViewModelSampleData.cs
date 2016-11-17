using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Dialogs.StringInput.ViewModel
{
	internal class StringInputDialogViewModelSampleData : IStringInputDialogViewModel
	{
		public StringInputDialogViewModelSampleData()
		{
			Promt = "Enter String";
			OkButtonCaption = "Ok";
			CancelButtonCaption = "Cancel";
		}

		public string Promt { get; }
		public string OkButtonCaption { get; }
		public string CancelButtonCaption { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}