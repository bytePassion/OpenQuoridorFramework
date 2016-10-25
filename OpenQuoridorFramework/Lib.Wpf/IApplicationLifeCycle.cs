using System.Windows;

namespace Lib.Wpf
{
	public interface IApplicationLifeCycle
	{ 
		void BuildAndStart(StartupEventArgs startupEventArgs);		
		void CleanUp(ExitEventArgs exitEventArgs);
	}
}
