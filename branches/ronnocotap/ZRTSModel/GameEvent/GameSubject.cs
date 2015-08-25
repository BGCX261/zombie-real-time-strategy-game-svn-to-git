using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.GameEvent
{
	public class GameSubject
	{
		private List<GameEventObserver> observers = new List<GameEventObserver>();

		public void notify(Event gameEvent)
		{
			foreach (GameEventObserver o in observers)
			{
				o.notify(gameEvent);
			}
		}

		public void registerObserver(GameEventObserver observer)
		{
			if (!observers.Contains(observer))
			{
				observers.Add(observer);
			}
		}

		public void unregisterObserver(GameEventObserver observer)
		{
			observers.Remove(observer);
		}
	}
}
