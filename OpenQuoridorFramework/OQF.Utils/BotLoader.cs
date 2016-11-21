using System;
using System.IO;
using System.Reflection;
using OQF.Bot.Contracts;
using OQF.Resources.LanguageDictionaries;

namespace OQF.Utils
{
	public static class BotLoader
	{
		public static BotLoadingResult GetUninitializedBot(string botDllPath)
		{
			if (string.IsNullOrWhiteSpace(botDllPath))
			{
				return new BotLoadingResult(null, null, false, Captions.ErrorMsg_NoDllPath);								
			}

			if (!File.Exists(botDllPath))
			{
				return new BotLoadingResult(null, null, false, $"{Captions.ErrorMsg_FileDoesNotExist} [{botDllPath}]");								
			}

			Assembly dllToLoad;

			try
			{
				dllToLoad = Assembly.LoadFile(botDllPath);
			}
			catch
			{
				return new BotLoadingResult(null, null, false, $"{Captions.ErrorMsg_FileIsNoAssembly} [{botDllPath}]");				
			}
			
			try
			{
				var allTypesInAssembly = dllToLoad.GetTypes();

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
									return new BotLoadingResult((IQuoridorBot)instance, type.Name, true, null);									
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
						
			return new BotLoadingResult(null, null, false, $"{Captions.ErrorMsg_BotCanNotBeLoadedFromAsembly} [{dllToLoad.FullName}]");						
		}

	}
}
