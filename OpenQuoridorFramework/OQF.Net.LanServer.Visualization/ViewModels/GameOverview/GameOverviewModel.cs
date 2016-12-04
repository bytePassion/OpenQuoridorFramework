using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview.Helper;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.GameOverview
{
	public class GameOverviewModel : ViewModel, IGameOverviewModel
	{
		private readonly IGameRepository gameRepository;

		public GameOverviewModel(IGameRepository gameRepository)
		{
			this.gameRepository = gameRepository;
			CurrentGames = new ObservableCollection<GameDisplayData>();

			gameRepository.RepositoryChanged += OnRepositoryChanged;
			OnRepositoryChanged();
		}

		private void OnRepositoryChanged()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				CurrentGames.Clear();

				gameRepository.GetAllGames()
							  .Select(game => new GameDisplayData(game.GameName,
																  game.GameInitiator.PlayerName,
																  game.Opponend?.PlayerName))
							  .Do(CurrentGames.Add);
			});			
		}

		public ObservableCollection<GameDisplayData> CurrentGames { get; }

		protected override void CleanUp()
		{
			gameRepository.RepositoryChanged -= OnRepositoryChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
