using System;

namespace WpfLib.Commands.Updater
{
	public interface ICommandUpdater : IDisposable
    {
        event EventHandler UpdateOfCanExecuteChangedRequired;
    }
}