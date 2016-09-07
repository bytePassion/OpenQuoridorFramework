using System;

namespace OQF.Tools.WpfTools.Commands.Updater
{
	public interface ICommandUpdater : IDisposable
    {
        event EventHandler UpdateOfCanExecuteChangedRequired;
    }
}