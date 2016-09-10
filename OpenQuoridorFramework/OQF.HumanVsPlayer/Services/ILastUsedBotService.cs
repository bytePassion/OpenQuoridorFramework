namespace OQF.HumanVsPlayer.Services
{
	internal interface ILastUsedBotService
	{
		string GetLastUsedBot();
		void SaveLastUsedBot(string botPath);
	}
}
