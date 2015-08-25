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
        Scenario scenario;
        public ActionController(Scenario scenario)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Given an entity, this function will cause the entity to perform whatever action is currently on the entity's
        /// action queue.
        /// </summary>
        /// <param name="entity"></param>
        public void update(Entity entity, EntityLocController locController)
        {
            Queue<ActionCommand> actionQueue = entity.getActionQueue();
            if(actionQueue.Count > 0)
            {
                ActionCommand command = actionQueue.Peek();

                if (command.work())
                {
                    actionQueue.Dequeue();
                }
                if (entity.entityType == Entity.EntityType.Unit)
                {
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
            else if (command.actionType == ActionCommand.ActionType.Move)
            {
                return handleUnitCommand(entity, command);
            }
            else if (command.actionType == ActionCommand.ActionType.SimpleAttack)
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
            Queue<ActionCommand> queue = entity.getActionQueue();
            queue.Clear();

            // Give the entity the command.
            entity.getActionQueue().Enqueue(command);
            return true;
        }
    }
}
