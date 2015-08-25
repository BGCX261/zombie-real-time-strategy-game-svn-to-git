using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    /// <summary>
    /// The Entity class is the superclass of all "entities" that exist in the "gameworld."
    /// </summary>
    [Serializable()]
    public class Entity
    {
        static int nextID = 0;
		 
        Player.Player owner; // Determines which Player this entity belongs to.
        public short health, maxHealth;
        State state;
        List<ActionCommand> actionQueue;
        public EntityType entityType;
        int id;

        public float x, y;  // Unit's x and y coordinates in game space.

        public long tickKilled = 0;

        public enum EntityType { Unit, Building, Resource, Object, NotSet };

        public Entity(Player.Player owner, short health)
        {
            this.owner = owner;
            this.health = health;
            this.maxHealth = health;

            state = new State();
            actionQueue = new List<ActionCommand>();

            this.entityType = EntityType.NotSet;

            // Give this entity a unique id.
            this.id = nextID;
            nextID++;
        }

        public State getState()
        {
            return this.state;
        }

        public Player.Player getOwner()
        {
            return this.owner;
        }

        public List<ActionCommand> getActionQueue()
        {
            return this.actionQueue;
        }

        public EntityType getEntityType()
        {
            return this.entityType;
        }
    }
}
