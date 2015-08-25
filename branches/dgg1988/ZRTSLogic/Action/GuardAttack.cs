using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
namespace ZRTSLogic.Action
{
	/// <summary>
	/// This class will define the GuardAttack action for a unit. 
	/// 
	/// When a Unit is given the GuardAttack action, it will chase the target for a short distance. Once the target is dead or
	/// has moved beyond that distance, the Unit will return to it's original location.
	/// </summary>
	public class GuardAttack : ActionCommand
	{
		MoveAction moveAction = null;
		AttackAction attackAction = null;
		Entity target; // target Entity of the AttackAction
		GameWorld gw;
		Unit unit; // Unit performing the AttackAction
		const float MAX_DIST = 10.0f; // The maximum distance to chase the target.
		bool targetDead = false;

		/// <summary>
		/// Given a Unit, an Entity and a GameWorld, this constructor will create a GuardAttack object where the Unit is
		/// attacking the Entity in the GameWorld.
		/// </summary>
		/// <param name="unit">The Unit being given the GuardAttack command.</param>
		/// <param name="target">The Entity being targeted by the command.</param>
		/// <param name="gw">The GameWorld this is occurring in.</param>
		public GuardAttack(Unit unit, Entity target, GameWorld gw)
		{
			this.unit = unit;
			this.target = target;
			this.gw = gw;
			this.actionType = ActionType.GuardAttack;
		}

		/// <summary>
		/// Performs the GuardAttack action.
		/// </summary>
		/// <returns>false if the action is not complete, true if the action is complete.</returns>
		public override bool work()
		{
			State targetState = target.getState();

			// Check if target's state is DEAD or REMOVE
			if (!targetDead && targetState.getPrimaryState() == State.PrimaryState.Dead || targetState.getPrimaryState() == State.PrimaryState.Remove)
			{
				targetDead = true;
			}

			// unit cannot attack, return true.
			if (!unit.stats.canAttack)
			{
				return true;
			}

			// Distance between unit and the Cell it is guarding.
			float dis = EntityLocController.findDistance(unit.x, unit.y, unit.getGuardCell().Xcoord, unit.getGuardCell().Ycoord); 

			// Is target dead or out of range?
			if (targetDead || dis > MAX_DIST)
			{
				// Create a new MoveAction if it is needed.
				if (moveAction == null)
				{
					moveAction = new MoveAction(unit.getGuardCell().Xcoord, unit.getGuardCell().Ycoord, gw, unit);
				}

				// Move back to original position
				return moveAction.work();
			}
			// Attack the enemy.
			else
			{
				// Create a new AttackAction if it is needed.
				if (attackAction == null)
				{
					attackAction = new AttackAction(unit, target, gw);
				}

				// Chase or attack the target.
				attackAction.work();
			}

			return false;
		}
	}
}
