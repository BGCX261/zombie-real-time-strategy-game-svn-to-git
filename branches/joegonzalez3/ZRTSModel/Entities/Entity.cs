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
        Queue<ActionCommand> actionQueue;
        public EntityType entityType;
        int id;

        public enum EntityType { Unit, Building, Resource, Object, NotSet };

        public Entity(Player.Player owner, short health, short maxHealth)
        {
            this.owner = owner;
            this.health = health;
            this.maxHealth = health;

            state = new State();
            actionQueue = new Queue<ActionCommand>();

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

        public Queue<ActionCommand> getActionQueue()
        {
            return this.actionQueue;
        }

        public EntityType getEntityType()
        {
            return this.entityType;
        }
    }
}
