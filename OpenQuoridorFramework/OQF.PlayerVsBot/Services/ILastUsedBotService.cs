namespace OQF.PlayerVsBot.Services
{
	internal interface ILastUsedBotService
	{
		string GetLastUsedBot();
		void SaveLastUsedBot(string botPath);
	}
}
