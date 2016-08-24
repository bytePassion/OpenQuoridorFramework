using System;

namespace QCF.UiTools.Communication.State
{
	public interface ISharedState<T>
	{
		event Action<T> StateChanged;

		T Value { get; set; }
	}
}