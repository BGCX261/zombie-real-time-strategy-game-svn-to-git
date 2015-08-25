using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;

namespace ZRTSModel.GameEvent
{
	public class GameEvent
	{
		/// <summary>
		/// The Cell that the Event "occurred" on.
		/// </summary>
		public Cell orginCell;

		/// <summary>
		/// The Entity that "caused" the Event (For example, in a MoveEvent, this is the Entity that moved. In an AttackEvent
		/// this is the Entity that is attacking.)
		/// </summary>
		public Entity sourceEntity;

		/// <summary>
		/// The Entity that is being affected by the event. (In a MoveEvent, this is the Entity that moved. In an AttackEvent, 
		/// this is the Entity that is being attacked.)
		/// </summary>
		public Entity targetEntity;

		/// <summary>
		/// What kind of Event this is.
		/// </summary>
		public EventType type;


		public enum EventType { MoveEvent, AttackEvent };

		public GameEvent(Cell orginCell, Entity sourceEntity, Entity targetEntity, EventType type)
		{
			this.orginCell = orginCell;
			this.sourceEntity = sourceEntity;
			this.targetEntity = targetEntity;
			this.type = type;
		}
	}
}
