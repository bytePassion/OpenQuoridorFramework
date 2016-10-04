using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OQF.Application.Tournament.ViewModels;

namespace OQF.Application.Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        public void StartTournament(IEnumerable<BotData> contestants)
        {
            CreateServerThread();
            StartMatch();
        }

        private void StartMatch()
        {
            var path = @"..\..\..\OQF.Application.PlayerVsBot\bin\Debug\OQF.Application.PlayerVsBot.exe";
            var info =
                new ProcessStartInfo()
                    
                {
                    FileName = path,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(path)
                };
            var process = new Process() { StartInfo = info };
            process.Start();
        }

        private void CreateServerThread()
        {
        }
    }
}