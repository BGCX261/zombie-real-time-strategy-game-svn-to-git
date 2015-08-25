using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameWorld;
using ZRTSModel.GameEvent;

namespace ZRTSModel.Entities
{
    [Serializable()]
    public class Unit : Entity, GameEventObserver
    {
        /** UNIT TYPE **/
        public string unitType;

        /** UNIT STATS **/
        public UnitStats stats;

		/// <summary>
		/// Used to give a Unit an attack bonus ( greater than 1.0) or penalty (less than 1.0)
		/// </summary>
		public double attackBuff = 1.0;

		/// <summary>
		/// Used to give a Unit a movement speed bonus or penalty.
		/// </summary>
		public double speedBuff = 1.0;

		public enum AttackStance
		{
			Passive,	// Ignore enemies
			Guard,		// Attack enemies on site, chase for a short distance before returning to starting position.
			Agressive	// Attack enemies on site, chase until enemy is dead.
		};

		AttackStance attackStance;

		List<GameEvent.GameEvent> eventList = new List<GameEvent.GameEvent>();

        /** UNIT LOCATION INFO **/
        Cell myCell;        // The cell the unit currently occupies
		Cell guardCell;		// The Cell the Unit is currently Guarding at.
        //public float x, y;  // Unit's x and y coordinates in game space. (MOVE TO ENTITY)

		/** ORIENTATION INFO 
		 * Represents which direction a Unit is facing in the gamewold. **/
        public enum Orientation { N, S, E, W, NW, NE, SW, SE };
		public Orientation orientation = Orientation.S;

        /// <summary>
        /// Constructor for a unit
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="health">Current Health</param>
        public Unit(Player.Player owner, short health)
            : base(owner, health)
        {

            this.entityType = EntityType.Unit;
            this.stats = new UnitStats();
			attackStance = AttackStance.Guard;
        }

        public Unit(Player.Player owner, UnitStats stats) : base(owner, stats.maxHealth)
        {
            this.stats = stats;
            this.entityType = EntityType.Unit;
			attackStance = AttackStance.Guard;
        }

		/// <summary>
		/// Get the Cell the Unit is currently occupying.
		/// </summary>
		/// <returns></returns>
        public Cell getCell()
        {
            return this.myCell;
        }

		/// <summary>
		/// Set the Cell the Unit is currently occupying. NOTE: This does not update the Cell. That should be done seperately.
		/// </summary>
		/// <param name="cell"></param>
        public void setCell(Cell cell)
        {
            this.myCell = cell;
        }

		/// <summary>
		/// Set the Unit's AttackStance.
		/// </summary>
		/// <param name="stance"></param>
		public void setAttackStance(AttackStance stance)
		{
			this.attackStance = stance;
			if (stance == AttackStance.Guard)
			{
				this.guardCell = myCell;
			}
			else if (stance == AttackStance.Agressive)
			{
				this.guardCell = null;
			}
		}

		public Cell getGuardCell()
		{
			return this.guardCell;
		}

		public void setGuardCell(Cell cell)
		{
			this.guardCell = cell;
		}

		/// <summary>
		/// Get the Unit's AttackStance.
		/// </summary>
		/// <returns></returns>
		public AttackStance getAttackStance()
		{
			return this.attackStance;
		}

		/**** GameEventObserver functions ***/

		List<GameSubject> subjects = new List<GameSubject>();

		/// <summary>
		/// Add a GameEvent to process.
		/// </summary>
		/// <param name="gameEvent">The GameEvent to be processed.</param>
		public void notify(GameEvent.GameEvent gameEvent)
		{
			eventList.Add(gameEvent);
		}

		/// <summary>
		/// Start observing a GameSubject.
		/// </summary>
		/// <param name="subject">The GameSubject to be observed.</param>
		public void register(GameSubject subject)
		{
			if (!subjects.Contains(subject))
			{
				subjects.Add(subject);
				subject.registerObserver(this);
			}
		}

		/// <summary>
		/// Stop observing a GameSubject.
		/// </summary>
		/// <param name="subject">The GameSubject to stop observing.</param>
		public void unregister(GameSubject subject)
		{
			subject.unregisterObserver(this);
			subjects.Remove(subject);
		}

		/// <summary>
		/// Stop observing all GameSubjects.
		/// </summary>
		public void unregisterAll()
		{
			foreach (GameSubject s in subjects)
			{
				s.unregisterObserver(this);
			}
			subjects.Clear();
		}

		/// <summary>
		/// Get the list of GameEvents that need to be processed.
		/// </summary>
		/// <returns></returns>
		public List<GameEvent.GameEvent> getEventList()
		{
			return this.eventList;
		}

		public Entity getEntity()
		{
			return this;
		}
    }
}
