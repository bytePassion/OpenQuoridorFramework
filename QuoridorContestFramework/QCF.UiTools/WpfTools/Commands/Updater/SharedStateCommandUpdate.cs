using System;
using QCF.Tools.Communication.State;
using QCF.Tools.FrameworkExtensions;

namespace QCF.Tools.WpfTools.Commands.Updater
{
	public class SharedStateCommandUpdate<T> : DisposingObject, ICommandUpdater
    {
        public event EventHandler UpdateOfCanExecuteChangedRequired;

        private readonly ISharedState<T> sharedState;
        
        public SharedStateCommandUpdate(ISharedState<T> sharedState)
        {
            this.sharedState = sharedState;

            sharedState.StateChanged += OnGlobalStateChanged;
        }

        private void OnGlobalStateChanged(T newValue)
        {
            UpdateOfCanExecuteChangedRequired?.Invoke(this, new EventArgs());
        }

        protected override void CleanUp()
        {
            sharedState.StateChanged -= OnGlobalStateChanged;
        }        
    }
}