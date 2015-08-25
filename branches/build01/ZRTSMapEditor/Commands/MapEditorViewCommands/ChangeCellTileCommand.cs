using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSMapEditor.Commands.MapEditorViewCommands
{
    /// <summary>
    /// A command for changing a tile to a new target tile.
    /// </summary>
    public class ChangeCellTileCommand : MapEditorCommand
    {
        private CellComponent targetCell;
        private ZRTSModel.Tile targetTile;
        
        // Keep track of the previous tile in order to undo.
        private ZRTSModel.Tile previousTile;

        /// <summary>
        /// Requires that the command is given a target cell and tile.
        /// </summary>
        private ChangeCellTileCommand()
        { }

        public ChangeCellTileCommand(CellComponent cell, ZRTSModel.Tile tile)
        {
            targetCell = cell;
            targetTile = tile;
        }

        /// <summary>
        /// Changes the target cell to include the new target tile.
        /// </summary>
        public void Do()
        {
            if (CanBeDone())
            {
                previousTile = targetCell.GetTile();
                
                // Cell.AddChild is overriden to ensure that only one tile exists.  This action
                // removes the previous tile and replaces it with the new one.
                targetCell.AddChild(targetTile);
            }
        }

        /// <summary>
        /// Replaces the new tile with the previous tile.
        /// </summary>
        public void Undo()
        {
            // Cell.AddChild is overriden to ensure that only one tile exists.  This action
            // removes the previous tile and replaces it with the new one.
            targetCell.AddChild(previousTile); 
        }

        /// <summary>
        /// Ensures that the cell and tile is valid, and that the new tile is actually different than the old tile.
        /// </summary>
        /// <returns></returns>
        public bool CanBeDone()
        {
            bool canBeDone = ((targetCell != null) && (targetTile != null));
            if (canBeDone)
            {
                // Cannot change a tile if the two types are already the same.
                canBeDone = (!targetCell.GetTile().GetType().Equals(targetTile.GetType()));
            }
            return canBeDone;
        }
    }
}
