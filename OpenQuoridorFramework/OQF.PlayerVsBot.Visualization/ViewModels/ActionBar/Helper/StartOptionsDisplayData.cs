using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using OQF.Bot.Contracts.Coordination;

namespace OQF.PlayerVsBot.Visualization.ViewModels.ActionBar.Helper
{
	public class StartOptionsDisplayData : INotifyPropertyChanged
	{
		private string displayCaption;

		public StartOptionsDisplayData(PlayerType playerStartingType, string displayCaption)
		{
			PlayerStartingType = playerStartingType;
			DisplayCaption = displayCaption;
		}

		public string DisplayCaption
		{
			get { return displayCaption; }
			set { PropertyChanged.ChangeAndNotify(this, ref displayCaption, value); }
		}

		public PlayerType PlayerStartingType { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
