using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
using ZRTSModel.Player;

namespace ZRTSLogic
{
	/// <summary>
	/// This class will control updates to the visiblity map (Cell.explored)
	/// 
	/// Daniel Gephardt
	/// </summary>
	public class VisibilityMapLogic
	{
		GameWorld gw;
		Map map;
		Player humanPlayer;
		const float STATIC_ENTITY_VIS_RANGE = 6.0f;

		public VisibilityMapLogic(GameWorld gw, Player humanPlayer)
		{
			this.gw = gw;
			this.map = gw.map;
			this.humanPlayer = humanPlayer;
		}

		/// <summary>
		/// This method will update the visibility map to show that all Cells in a Units visibilityRange have been explored.
		/// </summary>
		/// <param name="unit"></param>
		public void updateVisMap(Unit unit)
		{
			// Only update the map if the unit belongs to the Human player.
			if (unit.getOwner() != humanPlayer)
			{
				return;
			}

			byte offset = (byte)unit.stats.visibilityRange;

			int xStart = (short)unit.x - offset;
			int xEnd = (short)unit.x + offset;
			int yStart = (short)unit.y - offset;
			int yEnd = (short)unit.y + offset;

			exploreMap(xStart, xEnd, yStart, yEnd);
		}

		/// <summary>
		/// Given a Building, this method will update the visibility map to show that all cells in the range of the building have
		/// been explored.
		/// </summary>
		/// <param name="building"></param>
		public void updateVisMap(Building building)
		{
			// Only update the visibilty map for buildings belonging to the Human player.
			if (building.getOwner() != humanPlayer)
			{
				return;
			}

			byte offset = (byte)STATIC_ENTITY_VIS_RANGE;

			int xStart = (short)building.orginCell.Xcoord - offset;
			int xEnd = (short)building.orginCell.Xcoord + building.width + offset;
			int yStart = (short)building.orginCell.Ycoord - offset;
			int yEnd = (short)building.orginCell.Ycoord + building.height + offset;

			exploreMap(xStart, xEnd, yStart, yEnd);
		}

		private void exploreMap(int xStart, int xEnd, int yStart, int yEnd)
		{
			// Make sure that our bounds are valid. (Assumes that no Unit has a visibility range longer than the map.)
			if (xStart < 0)
			{
				xStart = 0;
			}
			else if (xEnd >= map.width)
			{
				xEnd = map.width;
			}

			if (yStart < 0)
			{
				yStart = 0;
			}
			else if (yEnd >= map.height)
			{
				yEnd = map.height;
			}

			// Set all cell explored flags to true.
			for (int i = xStart; i < xEnd; i++)
			{
				for (int j = yStart; j < yEnd; j++)
				{
					map.getCell(i, j).explored = true;
				}
			}
		}
	}
}
