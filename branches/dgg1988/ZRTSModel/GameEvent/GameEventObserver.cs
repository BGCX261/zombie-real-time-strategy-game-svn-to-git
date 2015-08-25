using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;

namespace ZRTSModel.GameEvent
{
	public interface GameEventObserver
	{
		void notify(GameEvent gameEvent);
		void register(GameSubject subject);
		void unregister(GameSubject subject);
		void unregisterAll();
		List<GameEvent> getEventList();
		Entity getEntity();
	}
}
