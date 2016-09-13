using System;

namespace Lib.Wpf.Commands.Updater
{
	public interface ICommandUpdater : IDisposable
    {
        event EventHandler UpdateOfCanExecuteChangedRequired;
    }
}