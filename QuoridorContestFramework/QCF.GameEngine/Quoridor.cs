using System;
using System.Reflection;

namespace QCF.GameEngine
{
    class Quoridor
    {
        public static void Main()
        {
            var loader = new BotLoader();
            var bot = loader.LoadBot(Assembly.LoadFrom("QFC.SimpleWalkingBot.dll"));

            Console.ReadLine();
        }
    }
}
