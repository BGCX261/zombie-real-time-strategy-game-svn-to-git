using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.GameEvent
{
	public interface GameEventObserver
	{
		void notify(Event gameEvent);
		void register(GameSubject subject);
		void unregister(GameSubject subject);
		void unregisterAll();
	}
}
