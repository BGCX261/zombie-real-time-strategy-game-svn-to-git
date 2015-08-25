using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSMapEditor.MapEditorModel
{
    /// <summary>
    /// A representation of what is currently selected in the Map Editor.
    /// </summary>
    public class SelectionState : MapEditorModelComponent
    {
        // The player selected in the Unit/Build palettes for adding an entity for a given player.
        private string selectedPlayer = null;

        // Used for determining what type of unit, building, or tile should be placed on the map when the user clicks on the map.
        private string selectedUnitType = null;
        private string selectedTileType = null;
        private string selectedBuildingType = null;

        public string SelectedTileType
        {
            get { return selectedTileType; }
            set { selectedTileType = value; }
        }
        private Type selectionType = null;

        public string SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; }
        }

        public string SelectedUnitType
        {
            get { return selectedUnitType; }
            set { selectedUnitType = value; }
        }

        public string SelectedBuildingType
        {
            get { return selectedBuildingType; }
            set { selectedBuildingType = value; }
        }

        public Type SelectionType
        {
            get { return selectionType; }
            set { selectionType = value; }
        }

        public override void Accept(MapEditorModelVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
