using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameEvent;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
using ZRTSLogic.Action;

namespace ZRTSLogic
{
	class GameEventLogic
	{
		/// <summary>
		/// This function will process the events that the observer has seen and will make the observer "react" to those events.
		/// </summary>
		/// <param name="observer">The GameEventObserver that will "react" to GameEvents it has seen.</param>
		/// <param name="gw">The GameWorld the GameEventObserver is in.</param>
		public static void processEvents(GameEventObserver observer, GameWorld gw)
		{
			// Does the observer have GameEvents to process>
			if (observer.getEventList().Count > 0)
			{
				// Check if observer is a Unit.
				if (observer.GetType().Name.Equals("Unit"))
				{
					Unit unit = (Unit)observer.getEntity();
					handleEventsForUnit(unit, gw);
				}



				// Clear the event list.
				observer.getEventList().Clear();
			}
		}

/**** FUNCTIONS FOR HANDLING EVENTS WHEN OBSERVER IS A UNIT  ******/

		/// <summary>
		/// Given a Unit and a GameWorld, this function will have the Unit "react" to all GameEvents in it's eventList.
		/// </summary>
		/// <param name="unit">The Unit reacting to it's events.</param>
		/// <param name="gw">The GameWorld the Unit is in.</param>
		private static void handleEventsForUnit(Unit unit, GameWorld gw)
		{
			foreach (GameEvent gameEvent in unit.getEventList())
			{
				// Handle MoveEvent
				if (gameEvent.type == GameEvent.EventType.MoveEvent)
				{
					handleMoveEventUnit(unit, gameEvent, gw);
				}
				// Handle AttackEvent
				else if (gameEvent.type == GameEvent.EventType.AttackEvent)
				{
					handleAttackEventUnit(unit, gameEvent, gw);
				}
			}
		}

		/// <summary>
		/// This function will process a MoveEvent and will make the unit "react" to that move event.
		/// </summary>
		/// <param name="unit">The Unit that observed the event.</param>
		/// <param name="gameEvent">The MoveEvent</param>
		/// <param name="gw">The GameWorld this is occurring in.</param>
		private static void handleMoveEventUnit(Unit unit, GameEvent gameEvent, GameWorld gw)
		{
			Console.Write("Unit at (" + unit.x + ", " + unit.y + "): ");
			// Check if unit caused this MoveEvent
			if (gameEvent.sourceEntity == (Entity)unit)
			{
				Unit target = searchCellsForEnemy(unit, gw);

				if (target != null)
				{
					Console.WriteLine("See Enemy at " + target.getCell().Xcoord + ", ("+ target.getCell().Ycoord + ")");
				}
			}

			// unit did not cause move event
			else
			{
				// Check if sourceEntity is an enemy
				if (unit.getOwner().isEnemy(gameEvent.sourceEntity.getOwner())) 
				{
					// Check if unit is in the Aggressive AttackStance
					if (unit.getAttackStance() == Unit.AttackStance.Agressive)
					{
						if(isInterruptable(unit))
						{
							AttackAction attackAction = new AttackAction(unit, gameEvent.sourceEntity, gw);
							ActionController.Instance.giveCommand(unit, attackAction);
							Console.WriteLine("Aggressive attack the entity in cell: " + gameEvent.orginCell.Xcoord + ", " + gameEvent.orginCell.Ycoord + ")");
						}
					}
					
					// Check if unit is in the Guard AttackStance
					else if (unit.getAttackStance() == Unit.AttackStance.Guard)
					{
						GuardAttack guardAttack = new GuardAttack(unit, gameEvent.sourceEntity, gw);
						ActionController.Instance.giveCommand(unit, guardAttack);
						Console.WriteLine("Guard attack the entity in cell: " + gameEvent.orginCell.Xcoord + ", " + gameEvent.orginCell.Ycoord + ")");
					}
				}
			}
		}

		/// <summary>
		/// This method will process an AttackEvent that the unit observed and will determine how the unit should "react" to it.
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="gameEvent"></param>
		/// <param name="gw"></param>
		private static void handleAttackEventUnit(Unit unit, GameEvent gameEvent, GameWorld gw)
		{
			// Check if unit is in the Passive AttackStance.
			if (unit.getAttackStance() == Unit.AttackStance.Passive)
			{
				// Ignore the AttackEvent.
				return;
			}

			// If an ally Entity is being attacked.
			if (gameEvent.targetEntity.getOwner() == unit.getOwner() && unit.getOwner().isEnemy(gameEvent.sourceEntity.getOwner()))
			{
				// If the unit is not performing any action.
				if (unit.getActionQueue().Count == 0)
				{
					// Attack the sourceEnemy
				}
			}
		}

/*** HELPER FUNCTIONS ****/

		/// <summary>
		/// This function will search the cells that the unit can see for the closest enemy Unit.
		/// </summary>
		/// <param name="unit">The unit doing the searching</param>
		/// <param name="gw">The GameWorld to search</param>
		/// <returns>The closest enemy Unit or null if none exists</returns>
		private static Unit searchCellsForEnemy(Unit unit, GameWorld gw)
		{
			byte offset = (byte)unit.stats.visibilityRange;

			int xStart = (short)unit.x - offset;
			int xEnd = (short)unit.x + offset;
			int yStart = (short)unit.y - offset;
			int yEnd = (short)unit.y + offset;

			// Make sure that our bounds are valid. (Assumes that no Unit has a visibility range longer than the map.)
			if (xStart < 0)
			{
				xStart = 0;
			}
			else if (xEnd >= gw.map.width)
			{
				xEnd = gw.map.width;
			}

			if (yStart < 0)
			{
				yStart = 0;
			}
			else if (yEnd >= gw.map.height)
			{
				yEnd = gw.map.height;
			}

			Unit target = null;
			float distance = 10000f;
			// Set all cell explored flags to true.
			for (int i = xStart; i < xEnd; i++)
			{
				for (int j = yStart; j < yEnd; j++)
				{
					Unit temp = gw.map.getCell(i,j).getUnit();

					if (temp != null && unit.getOwner().isEnemy(temp.getOwner()))
					{
						float tDis = EntityLocController.findDistance(unit.x, unit.y, temp.x, temp.y);

						if (tDis < distance)
						{
							target = temp;
							distance = tDis;
						}
					}
				}
			}

			return target;
		}

		/// <summary>
		/// This function will determine if an entity can be "interrupted" with a new Action.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		private static bool isInterruptable(Entity entity)
		{
			if (entity.getActionQueue().Count == 0)
			{
				return true;
			}

			return false;
		}
	}
}
