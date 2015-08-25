using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.Scenario;
using ZRTSLogic.Action;

namespace ZRTSLogic
{
    /// <summary>
    /// This class will control all access to the Entity's action queue. (Giving entities actions, having entities
    /// preform their current action, making changes to the Entity's action queue, act.)
    /// </summary>
    public class ActionController
    {

		static ActionController instance;
        private ActionController()
        {
		}

		public static ActionController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ActionController();
				}
				return instance;
			}
		}

        /// <summary>
        /// Given an entity, this function will cause the entity to perform whatever action is currently on the entity's
        /// action queue.
        /// </summary>
        /// <param name="entity"></param>
        public void update(Entity entity, EntityLocController locController)
        {
            List<ActionCommand> actionQueue = entity.getActionQueue();
            if(actionQueue.Count > 0)
            {
                ActionCommand command = actionQueue[0];
                //Console.WriteLine(command.actionType);
                if (command.work())
                {
                    // Action is done, remove and set Entity's state to idle.
                    actionQueue.RemoveAt(0);
                    entity.getState().setPrimaryState(State.PrimaryState.Idle);
                }

                if (entity.entityType == Entity.EntityType.Unit)
                {
                    // Unit may have moved, update it's location in the GameWorld.
                    locController.updateUnitLocation((Unit)entity);
                }
            }
        }

        /// <summary>
        /// Given an Entity and an ActionCommand, this function will attempt to assign the ActionCommand to the Entity's
        /// action queue.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <returns>false if the entity rejects the command. true otherwise.</returns>
        public bool giveCommand(Entity entity, ActionCommand command)
        {
            if (entity == null)
            {
                return false;
            }

            // Commands that only Units can execute.
            else if (command.actionType == ActionCommand.ActionType.Move || command.actionType == ActionCommand.ActionType.SimpleAttack
                        || command.actionType == ActionCommand.ActionType.BuildBuilding)
            {
                return handleUnitCommand(entity, command);
            }
            return false;
        }


        private bool handleUnitCommand(Entity entity, ActionCommand command)
        {
            // Check if the entity is a Unit. Reject the command if the entity is not a Unit.
            if (entity.getEntityType() != Entity.EntityType.Unit)
            {
                return false;
            }

            // Get the action queue from the entity and clear it.
            List<ActionCommand> queue = entity.getActionQueue();
            queue.Clear();

            // Give the entity the command.
            entity.getActionQueue().Add(command);
            return true;
        }

        /// <summary>
        /// This function will insert a command at the beginning of an Entity's action queue. This will interrupt the action currently
        /// being performed (if any) by the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        public static void insertIntoActionQueue(Entity entity, ActionCommand command)
        {
            entity.getActionQueue().Insert(0, command);
        }
    }
}
