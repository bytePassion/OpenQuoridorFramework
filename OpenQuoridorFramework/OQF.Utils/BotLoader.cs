using System;
using System.Reflection;
using OQF.Bot.Contracts;

namespace OQF.Utils
{
	public static class BotLoader
	{
		public static Tuple<IQuoridorBot,string> LoadBot(Assembly assembly)
		{
			try
			{
				var allTypesInAssembly = assembly.GetTypes();

				foreach (var type in allTypesInAssembly)
				{
					var interfaces = type.GetInterfaces();

					foreach (var @interface in interfaces)
					{
						if (@interface.Name == nameof(IQuoridorBot))
						{
							var constructor = type.GetConstructor(Type.EmptyTypes);
							if (constructor != null)
							{
								try
								{
									var instance = Activator.CreateInstance(type);
									return new Tuple<IQuoridorBot, string>((IQuoridorBot)instance, type.Name);
								}
								catch
								{
									// ignored
								}
							}
						}
					}
				}
			}
			catch
			{
				// ignored
			}
			
	        return null;
        }
    }
}
