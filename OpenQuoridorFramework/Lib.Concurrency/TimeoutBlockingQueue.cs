using System;
using System.Collections.Generic;
using System.Threading;
using Lib.FrameworkExtension;

namespace Lib.Concurrency
{
	/**
		Achtung: kann sein, dass es nur mit einem abnehmer wirklich gut funktioniert ....
		muss noch genauer untersucht werden
		Problem: pulseAll lässt vll nicht alle den timeout ausführen
	 */

	public class TimeoutBlockingQueue<T> : DisposingObject
	{
		private readonly Queue<T> queue;

		private readonly Timer takeTimeoutTimer;

		public TimeoutBlockingQueue (uint takeTimeout)
		{
			queue = new Queue<T>();

			takeTimeoutTimer = new Timer(TimerTick, null,
										 TimeSpan.FromMilliseconds(takeTimeout),
										 TimeSpan.FromMilliseconds(takeTimeout));
			
		}

		private void TimerTick (object state)
		{
			lock (queue)
			{
				Monitor.PulseAll(queue);
			}			
		}

		public void Clear()
		{
			lock (queue)
			{
				queue.Clear();
			}
		}

		public void Put (T item)
		{
			lock (queue)
			{
				queue.Enqueue(item);

				if (queue.Count == 1)
					Monitor.PulseAll(queue);
			}
		}

		public T TimeoutTake ()
		{
			if (takeTimeoutTimer == null)
				throw new InvalidOperationException();

			lock (queue)
			{
				if (queue.Count == 0)
				{
					Monitor.Wait(queue);

					if (queue.Count == 0)
						return default(T);
				}

				return queue.Dequeue();
			}
		}		

		protected override void CleanUp ()
		{
			takeTimeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
			takeTimeoutTimer.Dispose();
		}
	}
}