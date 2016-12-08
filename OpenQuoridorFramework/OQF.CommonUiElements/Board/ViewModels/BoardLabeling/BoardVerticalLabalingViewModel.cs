using System.ComponentModel;
using Lib.Communication.State;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Board.ViewModels.BoardLabeling
{
	public class BoardVerticalLabalingViewModel : ViewModel, IBoardLabelingViewModel
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


		public BoardVerticalLabalingViewModel (ISharedStateReadOnly<bool> isBoardRotatedVariable)
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

		private void IsBoardRotatedVariablChanged (bool isRotated)
		{
			if (isRotated)
			{
				Label1 = "9";
				Label2 = "8";
				Label3 = "7";
				Label4 = "6";
				Label5 = "5";
				Label6 = "4";
				Label7 = "3";
				Label8 = "2";
				Label9 = "1";
			}
			else
			{
				Label1 = "1";
				Label2 = "2";
				Label3 = "3";
				Label4 = "4";
				Label5 = "5";
				Label6 = "6";
				Label7 = "7";
				Label8 = "8";
				Label9 = "9";
			}
		}

		public string Label1 { get { return label1; } private set { PropertyChanged.ChangeAndNotify(this, ref label1, value); } }
		public string Label2 { get { return label2; } private set { PropertyChanged.ChangeAndNotify(this, ref label2, value); } }
		public string Label3 { get { return label3; } private set { PropertyChanged.ChangeAndNotify(this, ref label3, value); } }
		public string Label4 { get { return label4; } private set { PropertyChanged.ChangeAndNotify(this, ref label4, value); } }
		public string Label5 { get { return label5; } private set { PropertyChanged.ChangeAndNotify(this, ref label5, value); } }
		public string Label6 { get { return label6; } private set { PropertyChanged.ChangeAndNotify(this, ref label6, value); } }
		public string Label7 { get { return label7; } private set { PropertyChanged.ChangeAndNotify(this, ref label7, value); } }
		public string Label8 { get { return label8; } private set { PropertyChanged.ChangeAndNotify(this, ref label8, value); } }
		public string Label9 { get { return label9; } private set { PropertyChanged.ChangeAndNotify(this, ref label9, value); } }

		protected override void CleanUp ()
		{
			if (isBoardRotatedVariable != null)
				isBoardRotatedVariable.StateChanged -= IsBoardRotatedVariablChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}