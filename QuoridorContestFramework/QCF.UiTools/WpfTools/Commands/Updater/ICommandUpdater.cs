using System;

namespace QCF.UiTools.WpfTools.Commands.Updater
{
	public interface ICommandUpdater : IDisposable
    {
        event EventHandler UpdateOfCanExecuteChangedRequired;
    }
}