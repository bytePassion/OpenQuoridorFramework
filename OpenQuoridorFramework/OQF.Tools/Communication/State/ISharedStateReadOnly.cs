using System;

namespace OQF.Tools.Communication.State
{
	public interface ISharedStateReadOnly<out T>
    {
        event Action<T> StateChanged;

        T Value { get; }
    }
}