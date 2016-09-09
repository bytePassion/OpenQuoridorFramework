using System;

namespace CommunicationLib.State
{
	public interface ISharedState<T>
	{
		event Action<T> StateChanged;

		T Value { get; set; }
	}
}