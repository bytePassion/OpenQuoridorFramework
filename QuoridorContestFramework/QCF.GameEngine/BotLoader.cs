using System;
using System.Reflection;
using QCF.Contest.Contracts;

namespace QCF.GameEngine
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
