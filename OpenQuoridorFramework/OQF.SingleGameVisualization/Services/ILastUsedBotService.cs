namespace OQF.SingleGameVisualization.Services
{
	internal interface ILastUsedBotService
	{
		string GetLastUsedBot();
		void SaveLastUsedBot(string botPath);
	}
}
