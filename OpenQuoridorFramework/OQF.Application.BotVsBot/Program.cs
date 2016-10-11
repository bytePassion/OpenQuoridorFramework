using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Factories;
using OQF.Utils;

namespace OQF.Application.BotVsBot
{
    class Program
    {
        private IQuoridorBot bottomBot;
        private IQuoridorBot topBot;

        static void Main(string[] args)
        {
            IGameFactory gameFactory = new GameFactory();
            var assembly1 = Assembly.LoadFile(@"C:\Users\Alexander\Documents\Quoridor\OpenQuoridorFramework\Bots\SimpleWalkingBot\bin\Debug\SimpleWalkingBot.dll");
            var bot = BotLoader.LoadBot(assembly1);
            var bot2 = BotLoader.LoadBot(assembly1);

            var game = gameFactory.CreateNewGame(bot.Item1, bot2.Item1, new GameConstraints(TimeSpan.FromSeconds(60), 120));
           
            game.NextBoardstateAvailable += OnNextBoardState;


            var connectionManager = new ConnectionManager();
            var t1 = new Thread(connectionManager.StartClientConnection);
            t1.Start();
            Console.ReadLine();
        }

        private static void OnNextBoardState(BoardState obj)
        {
            Console.WriteLine(obj.LastMove);
        }
    }

    public class ConnectionManager
    {
        public void StartClientConnection()
        {
                using (var connection = new ResponseSocket("tcp://*:5555"))
                {
                    while (true)
                    {
                        var msg = connection.ReceiveFrameString();
                        if (msg.Equals("InitBot"))
                        {
                            Console.WriteLine(msg);
                            connection.SendFrame("Ok");
                        }
                        else
                        {
                            connection.SendFrame("Error");
                        }

                    }
                }
        }
    }
}
