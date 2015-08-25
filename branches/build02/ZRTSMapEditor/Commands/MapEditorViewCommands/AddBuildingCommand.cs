using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSModel.GameModel;

namespace ZRTSMapEditor.Commands.MapEditorViewCommands
{
    class AddBuildingCommand : MapEditorCommand
    {
        private Building building;
        private PlayerComponent player;
        private CellComponent cell;

        private AddBuildingCommand()
        { }

        public AddBuildingCommand(Building building, PlayerComponent player, CellComponent cell)
        {
            this.building = building;
            this.player = player;
            this.cell = cell;
        }
        public void Do()
        {
            int x = cell.X;
            int y = cell.Y;
            building.PointLocation = new PointF((float)x, (float)y); 
            player.BuildingList.AddChild(building);
            // Set it again!
            building.PointLocation = new PointF((float)x, (float)y); 
        }

        public void Undo()
        {
            building.PointLocation = null;
            player.BuildingList.RemoveChild(building);
        }

        public bool CanBeDone()
        {
            return ((cell != null) && (!cell.ContainsEntity()) && (player != null) && (building != null));
        }
    }
}
