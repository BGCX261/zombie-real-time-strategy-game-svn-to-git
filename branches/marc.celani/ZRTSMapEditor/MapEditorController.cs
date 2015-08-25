using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using ZRTSModel.GameWorld;
using ZRTSMapEditor.MapEditorModel;
using ZRTSModel;
using ZRTSMapEditor.UI;
using ZRTSMapEditor.Commands.MapEditorViewCommands;
using ZRTSModel.Factories;
using ZRTSModel.GameModel;
using System.Drawing;

namespace ZRTSMapEditor
{
    public class MapEditorController
    {
        private MapEditorFullModel model;
        public MapEditorView view;

        

        public MapEditorController(MapEditorFullModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Serializes and saves the current scenario.
        /// </summary>
        public void saveScenario()
        {
            if (model.GetScenario() != null)
            {
                Stream saveStream;
                SaveFileDialog saveMapDialog = new SaveFileDialog();
                saveMapDialog.InitialDirectory = Application.StartupPath + "\\Maps\\";
                saveMapDialog.Filter = "Map files (*.map)| *.map";

                if (saveMapDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((saveStream = saveMapDialog.OpenFile()) != null)
                    {
                        ScenarioXMLWriter writer = new ScenarioXMLWriter(saveStream);
                        writer.BeginWrite();
                        model.GetScenario().Accept(writer);
                        writer.EndWrite();
                        saveStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads a scenario from disc by deserializing.  Generates empty observer lists for each model component.
        /// </summary>
        public void loadScenario()
        {
            if (model.GetScenario() != null)
            {
                // TODO: Ask if the user wants to discard the current scenario.
            }
            
            // Displays an OpenFileDialog so the user can select a Map.
            OpenFileDialog openMapDialog = new OpenFileDialog();
            openMapDialog.InitialDirectory = Application.StartupPath + "\\Maps\\";
            openMapDialog.Filter = "Map Files|*.map";
            openMapDialog.Title = "Select a Map File";

            if (openMapDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream mapFile = openMapDialog.OpenFile();
                ScenarioXMLReader reader = new ScenarioXMLReader(mapFile);
                ScenarioComponent scenario = reader.GenerateScenarioFromXML();
                mapFile.Close();
                model.AddChild(scenario);

                // Clear the stack because we now have a new scenario in context.
                model.GetCommandStack().EmptyStacks();
                // TODO: Update the SaveInfo state.
            }
        }

        /// <summary>
        /// Creates a CreateNewScenario dialog and uses it to determine the name and size of the new scenario.  Then, generates a 
        /// new scenario of the appropriate size.
        /// </summary>
        public void createNewScenario()
        {
            if (model.GetScenario() != null)
            {
                model.GetScenario().RemoveChild(model.GetScenario().GetGameWorld());
                // TODO: Ask if the user wants to discard the current scenario or save it.
            }
            CreateNewScenarioDialog dialog = new CreateNewScenarioDialog();
            dialog.ShowDialog();

            if (dialog.ExitWithCreate)
            {
                // Create a scenario with a map of the appropriate size
                ScenarioComponent scenario = new ScenarioComponent(dialog.ScenarioWidth, dialog.ScenarioHeight);

                // Add grass cells at each cell.
                ZRTSModel.Map map = scenario.GetGameWorld().GetMap();
                for (int i = 0; i < map.GetWidth(); i++)
                {
                    for (int j = 0; j < map.GetHeight(); j++)
                    {
                        CellComponent cell = new CellComponent();
                        cell.AddChild(new Grass());
                        cell.X = i;
                        cell.Y = j;
                        map.AddChild(cell);
                    }
                }

                // TODO: Update SaveInfo model to change filename and UpToDate flag.

                // Automatically discards old scenario, by overloaded AddChild function.
                model.AddChild(scenario);

                // Empty the command queue
                model.GetCommandStack().EmptyStacks();

                // We may have just destroyed a large scenario, so collect that garbage.
                // Commented out - only used for testing purposes.  The C# garbage collector takes a LONG time to be activated if this call is not made,
                // but if the call is made, it disrupts UI.
                // GC.Collect();
            }
            
        }

        public bool isOkayToClose()
        {
            // TODO Open dialog box to see if it is okay to discard changes, or to save first
            return true;
        }

        /// <summary>
        /// Determines from the selection state if we are placing a resource, building, unit, or tile, and then places it based on
        /// the selection state.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        internal void OnClickMapCell(int x, int y)
        {
            if (model.GetSelectionState().SelectionType == typeof(ZRTSModel.Tile))
            {
                TileFactory tf = TileFactory.Instance;
                CellComponent cell = model.GetScenario().GetGameWorld().GetMap().GetCellAt(x, y);
                ZRTSModel.Tile tile = tf.GetImprovedTile(model.GetSelectionState().SelectedTileType);
                ChangeCellTileCommand command = new ChangeCellTileCommand(cell, tile);

                if (command.CanBeDone())
                {
                    model.GetCommandStack().ExecuteCommand(command);
                }
            }
            else if (model.GetSelectionState().SelectionType == typeof(UnitComponent))
            {
                // Get instance of Unit Factory, produce unit, and place on map for the given player.
            }
        }

        /// <summary>
        /// Called by the tile palette when a tile is selected - updates the selection model.
        /// </summary>
        /// <param name="type"></param>
        internal void selectTileType(string type)
        {
            model.GetSelectionState().SelectionType = typeof(ZRTSModel.Tile);
            model.GetSelectionState().SelectedTileType = type;
            view.setTilePreview(type);
        }

        /// <summary>
        /// Opens a dialog box to change the players list.
        /// </summary>
        internal void OpenPlayersForm()
        {
            if (model.GetScenario() != null)
            {
                PlayersForm form = new PlayersForm(model.GetScenario().GetGameWorld().GetPlayerList());
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Delegates undo command to command stack.
        /// </summary>
        internal void UndoLastCommand()
        {
            model.GetCommandStack().UndoLastCommand();
        }

        /// <summary>
        /// Delegates redo request to command stack.
        /// </summary>
        internal void RedoLastUndoCommand()
        {
            model.GetCommandStack().RedoLastUndoneCommand();
        }

        /// <summary>
        /// Called by unit and building palettes to determine for which player the building or unit should be added.
        /// </summary>
        /// <param name="playerName"></param>
        internal void SelectPlayer(string playerName)
        {
            model.GetSelectionState().SelectedPlayer = playerName;
        }

        /// <summary>
        /// Called by unit palette to set in the selection state what type of unit has been selected.
        /// </summary>
        /// <param name="unitType"></param>
        internal void SelectUnitType(string unitType)
        {
            model.GetSelectionState().SelectedUnitType = unitType;
            model.GetSelectionState().SelectionType = typeof(UnitComponent);
            view.setEntityPreview(unitType);
        }

        /// <summary>
        /// Called by building palette to set in the selection state what type of building has been selected.
        /// </summary>
        /// <param name="unitType"></param>
        internal void SelectBuildingType(string buildingType)
        {
            model.GetSelectionState().SelectedBuildingType = buildingType;
            model.GetSelectionState().SelectionType = typeof(Building);
            view.setEntityPreview(buildingType);
        }

        internal void OnClickMapCell(CellComponent cellComponent, float xPercent, float yPercent)
        {
            if (model.GetSelectionState().SelectionType == typeof(ZRTSModel.Tile))
            {
                TileFactory tf = TileFactory.Instance;
                ZRTSModel.Tile tile = tf.GetImprovedTile(model.GetSelectionState().SelectedTileType);
                ChangeCellTileCommand command = new ChangeCellTileCommand(cellComponent, tile);

                if (command.CanBeDone())
                {
                    model.GetCommandStack().ExecuteCommand(command);
                }
            }
            else if (model.GetSelectionState().SelectionType == typeof(UnitComponent))
            {
                UnitFactory uf = UnitFactory.Instance;
                UnitComponent unit = uf.Create(model.GetSelectionState().SelectedUnitType);
                //unit.PointLocation = new PointF((float)cellComponent.X + xPercent, (float)cellComponent.Y + yPercent);
                PlayerComponent player = model.GetScenario().GetGameWorld().GetPlayerList().GetPlayerByName(model.GetSelectionState().SelectedPlayer);
                AddUnitCommand command = new AddUnitCommand(unit, player, cellComponent);

                if (command.CanBeDone())
                {
                    model.GetCommandStack().ExecuteCommand(command);
                }
            }
            else if (model.GetSelectionState().SelectionType == typeof(Building))
            {
                BuildingFactory bf = BuildingFactory.Instance;
                Building building = bf.Build(model.GetSelectionState().SelectedBuildingType, true);
                //building.PointLocation = new PointF((float)cellComponent.X + xPercent, (float)cellComponent.Y + yPercent);
                PlayerComponent player = model.GetScenario().GetGameWorld().GetPlayerList().GetPlayerByName(model.GetSelectionState().SelectedPlayer);
                AddBuildingCommand command = new AddBuildingCommand(building, player, cellComponent);

                if (command.CanBeDone())
                {
                    model.GetCommandStack().ExecuteCommand(command);
                }
            }
        }

        internal void OnDragMapCell(CellComponent cell)
        {
            if (model.GetSelectionState().SelectionType == typeof(ZRTSModel.Tile))
            {
                TileFactory tf = TileFactory.Instance;
                ZRTSModel.Tile tile = tf.GetImprovedTile(model.GetSelectionState().SelectedTileType);
                ChangeCellTileCommand command = new ChangeCellTileCommand(cell, tile);

                if (command.CanBeDone())
                {
                    model.GetCommandStack().ExecuteCommand(command);
                }
            }
        }

    }
}
