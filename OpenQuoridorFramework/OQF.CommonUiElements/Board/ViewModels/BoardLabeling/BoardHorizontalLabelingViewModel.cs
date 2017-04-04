using System.ComponentModel;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.ViewModelBase;

namespace OQF.CommonUiElements.Board.ViewModels.BoardLabeling
{
	public class BoardHorizontalLabelingViewModel : ViewModel, IBoardLabelingViewModel
	{
		private readonly ISharedStateReadOnly<bool> isBoardRotatedVariable;

		private string label1;
		private string label2;
		private string label3;
		private string label4;
		private string label5;
		private string label6;
		private string label7;
		private string label8;
		private string label9;


		public BoardHorizontalLabelingViewModel(ISharedStateReadOnly<bool> isBoardRotatedVariable)
		{
			this.isBoardRotatedVariable = isBoardRotatedVariable;

			if (isBoardRotatedVariable != null)
			{
				isBoardRotatedVariable.StateChanged += IsBoardRotatedVariablChanged;
				IsBoardRotatedVariablChanged(isBoardRotatedVariable.Value);
			}
			else
			{
				IsBoardRotatedVariablChanged(false);
			}			
		}

		private void IsBoardRotatedVariablChanged(bool isRotated)
		{
			if (isRotated)
			{
				Label1 = "I";
				Label2 = "H";
				Label3 = "G";
				Label4 = "F";
				Label5 = "E";
				Label6 = "D";
				Label7 = "C";
				Label8 = "B";
				Label9 = "A";
			}
			else
			{
				Label1 = "A";
				Label2 = "B";
				Label3 = "C";
				Label4 = "D";
				Label5 = "E";
				Label6 = "F";
				Label7 = "G";
				Label8 = "H";
				Label9 = "I";
			}
		}

		public string Label1 { get { return label1; } private set { PropertyChanged.ChangeAndNotify(this, ref label1, value); }}
		public string Label2 { get { return label2; } private set { PropertyChanged.ChangeAndNotify(this, ref label2, value); }}
		public string Label3 { get { return label3; } private set { PropertyChanged.ChangeAndNotify(this, ref label3, value); }}
		public string Label4 { get { return label4; } private set { PropertyChanged.ChangeAndNotify(this, ref label4, value); }}
		public string Label5 { get { return label5; } private set { PropertyChanged.ChangeAndNotify(this, ref label5, value); }}
		public string Label6 { get { return label6; } private set { PropertyChanged.ChangeAndNotify(this, ref label6, value); }}
		public string Label7 { get { return label7; } private set { PropertyChanged.ChangeAndNotify(this, ref label7, value); }}
		public string Label8 { get { return label8; } private set { PropertyChanged.ChangeAndNotify(this, ref label8, value); }}
		public string Label9 { get { return label9; } private set { PropertyChanged.ChangeAndNotify(this, ref label9, value); }}

		protected override void CleanUp()
		{
			if (isBoardRotatedVariable != null)
				isBoardRotatedVariable.StateChanged -= IsBoardRotatedVariablChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;				
	}
}
