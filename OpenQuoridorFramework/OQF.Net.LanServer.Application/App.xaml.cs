using System.Windows;
using bytePassion.Lib.Utils;

namespace OQF.Net.LanServer.Application
{
	public partial class App
	{
		private IApplicationLifeCycle applicationLifeCycle;

		protected override void OnExit (ExitEventArgs e)
		{
			base.OnExit(e);
			applicationLifeCycle.CleanUp(e);
		}

		protected override void OnStartup (StartupEventArgs e)
		{
			base.OnStartup(e);

			applicationLifeCycle = new ApplicationLifeCycle();
			applicationLifeCycle.BuildAndStart(e);
		}
	}
}
