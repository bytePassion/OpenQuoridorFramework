using System;
using Lib.Communication.State;
using Lib.FrameworkExtension;

namespace Lib.Wpf.Commands.Updater
{
	public class SharedStateReadOnlyCommandUpdate<T> : DisposingObject, ICommandUpdater
    {
        public event EventHandler UpdateOfCanExecuteChangedRequired;

        private readonly ISharedStateReadOnly<T> sharedState;

        public SharedStateReadOnlyCommandUpdate(ISharedStateReadOnly<T> sharedState)
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