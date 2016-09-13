using System;

namespace Lib.Communication.State
{

	public class SharedState<T> : ISharedState<T>,
                                  ISharedStateReadOnly<T>,
                                  ISharedStateWriteOnly<T>
	{
		public event Action<T> StateChanged;

		private T stateValue;

	    public SharedState(T initialValue)
	    {
	        stateValue = initialValue;
	    }

		public SharedState() : this(default(T))
		{			
		}

		public T Value
		{
			get { return stateValue; }
			set
			{
				if (Equals(value, stateValue)) return;

				stateValue = value;					

				StateChanged?.Invoke(stateValue);
			}
		}		
	}
}