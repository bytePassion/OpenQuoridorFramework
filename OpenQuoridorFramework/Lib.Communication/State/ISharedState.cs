using System;

namespace Lib.Communication.State
{
	public interface ISharedState<T>
	{
		event Action<T> StateChanged;

		T Value { get; set; }
	}
}