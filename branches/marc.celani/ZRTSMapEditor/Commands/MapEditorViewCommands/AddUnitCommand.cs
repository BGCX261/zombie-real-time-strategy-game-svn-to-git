using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSModel.GameModel;

namespace ZRTSMapEditor.Commands.MapEditorViewCommands
{
    class AddUnitCommand : MapEditorCommand
    {
        private UnitComponent unit;
        private PlayerComponent player;
        private CellComponent cell;

        private AddUnitCommand()
        { }

        public AddUnitCommand(UnitComponent unit, PlayerComponent player, CellComponent cell)
        {
            this.unit = unit;
            this.player = player;
            this.cell = cell;
        }
        public void Do()
        {
            player.GetUnitList().AddChild(unit);
            unit.PointLocation = new PointF((float)cell.X, (float)cell.Y);
            cell.AddEntity(unit);
        }

        public void Undo()
        {
            player.GetUnitList().RemoveChild(unit);
            cell.RemoveEntity(unit);
        }

        public bool CanBeDone()
        {
            return ((cell != null) && (!cell.ContainsEntity()) && (player != null) && (unit != null));
        }
    }
}
