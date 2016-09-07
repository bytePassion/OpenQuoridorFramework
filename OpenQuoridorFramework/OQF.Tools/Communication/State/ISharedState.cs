using System;

namespace OQF.Tools.Communication.State
{
	public interface ISharedState<T>
	{
		event Action<T> StateChanged;

		T Value { get; set; }
	}
}