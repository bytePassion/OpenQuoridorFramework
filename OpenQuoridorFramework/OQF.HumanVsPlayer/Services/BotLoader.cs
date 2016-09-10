using System;
using System.Reflection;
using OQF.Contest.Contracts;

namespace OQF.HumanVsPlayer.Services
{
	public static class BotLoader
	{
		public static IQuoridorBot LoadBot(Assembly assembly)
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
								return (IQuoridorBot)instance;
							}
							catch
							{
								// ignored
							}
						}
			        }
		        }
	        }

	        return null;
        }
    }
}
