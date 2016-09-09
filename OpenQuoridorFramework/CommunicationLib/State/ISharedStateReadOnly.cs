using System;

namespace CommunicationLib.State
{
	public interface ISharedStateReadOnly<out T>
    {
        event Action<T> StateChanged;

        T Value { get; }
    }
}