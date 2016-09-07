using System;
using System.Reflection;
using OQF.Contest.Contracts;

namespace OQF.GameEngine.Loader
{
	public class BotLoader
    {
        public IQuoridorBot LoadBot(Assembly assembly)
        {
            var type = assembly.GetTypes()[0];
            var c = Activator.CreateInstance(type) ;
            return c as IQuoridorBot;
        }
    }
}
