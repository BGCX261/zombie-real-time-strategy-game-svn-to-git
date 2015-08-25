using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;
using Pathfinder;
using ZRTSModel;
using ZRTSModel.GameModel;

namespace ZRTSModel
{
	/// <summary>
	/// This class represents an command for a Building to produce a UnitComponent.
	/// </summary>
	public class ProduceUnit : BuildingAction
	{
		Building building;
		public string unitType;
		Entities.UnitStats stats;
		public int totalCost; // How many "points" are required to make the Unit.
		public int points = 0; // How manu "points" have been spent.
		byte currentTicks = 0;

		byte TICKS_PER_POINT = 2;

		public ProduceUnit(Building building, string unitType)
		{
			this.building = building;
			this.unitType = unitType;


			this.stats = Factories.UnitFactory.Instance.getStats(unitType);

			totalCost = stats.foodCost + stats.lumberCost + stats.metalCost + stats.waterCost;
		}

		public override bool Work()
		{
			if (this.stats == null)
			{
				return true;
			}
			currentTicks++;

			if (currentTicks % TICKS_PER_POINT == 0)
			{
				currentTicks = 0;
				points++;
			}

			if (points >= totalCost)
			{
				return addUnit();
			}

			return false;
		}

		private bool addUnit()
		{
			// Get the player who owns this building.
			ModelComponent temp = building.Parent;
			while (!(temp is PlayerComponent))
			{
				temp = temp.Parent;
			}
			PlayerComponent player = (PlayerComponent)temp;

			// Get the Gameworld.
			while (!(temp is ZRTSModel.GameModel.GameModel))
			{
				temp = temp.Parent;
			}
			ZRTSModel.GameModel.GameModel model = (ZRTSModel.GameModel.GameModel)temp;

			// Get the CellComponent to insert into.
			CellComponent insertCell = findEmptyNeighborCell(model);
			if (insertCell == null)
			{
				return false; // No empty CellComponent.
			}

			// Add Unit to the Map.
			UnitComponent unit = new UnitComponent(stats);
			unit.PointLocation = new PointF(insertCell.X + 0.5f, insertCell.Y + 0.5f);

			// Add Unit to the Player who owns the building.
			player.GetUnitList().AddChild(unit);
			return true;
		}

		private CellComponent findEmptyNeighborCell(ZRTSModel.GameModel.GameModel model)
		{
			CellComponent insertCell = null;
			int width = model.GetScenario().GetGameWorld().GetMap().GetWidth();
			int height = model.GetScenario().GetGameWorld().GetMap().GetWidth();

			foreach (CellComponent cell in building.CellsContainedWithin)
			{
				int x = cell.X;
				int y = cell.Y;

				if (x < width - 1)
				{
					CellComponent c = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x + 1, y);
					if (c.GetTile().Passable() && c.EntitiesContainedWithin.Count == 0)
					{
						insertCell = c;
						break;
					}

					if (y < height - 1)
					{
						c = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x + 1, y + 1);
						if (c.GetTile().Passable() && c.EntitiesContainedWithin.Count == 0)
						{
							insertCell = c;
							break;
						}
					}

					if (y > 0)
					{
						c = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x + 1, y);
						if (c.GetTile().Passable() && c.EntitiesContainedWithin.Count == 0)
						{
							insertCell = c;
							break;
						}
					}

				}

				if (x > 0)
				{
					CellComponent c = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x - 1, y);
					if (c.GetTile().Passable() && c.EntitiesContainedWithin.Count == 0)
					{
						insertCell = c;
						break;
					}

					if (y < height - 1)
					{
						c = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x - 1, y + 1);
						if (c.GetTile().Passable() && c.EntitiesContainedWithin.Count == 0)
						{
							insertCell = c;
							break;
						}
					}

					if (y > 0)
					{
						c = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x - 1, y);
						if (c.GetTile().Passable() && c.EntitiesContainedWithin.Count == 0)
						{
							insertCell = c;
							break;
						}
					}
				}
			}

			return insertCell;
		}
	}
}
