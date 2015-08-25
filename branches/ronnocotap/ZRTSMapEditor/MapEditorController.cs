using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using ZRTSModel.Scenario;
using ZRTSModel.GameWorld;
using ZRTSMapEditor.MapEditorModel;
using ZRTSModel;

namespace ZRTSMapEditor
{
    public class MapEditorController
    {
        private MapEditorModelOld model;
        private ImprovedMapEditorModel improvedModel;

        public MapEditorController(MapEditorModelOld model)
        {
            this.model = model;
            this.improvedModel = new ImprovedMapEditorModel();
        }

        public MapEditorController(ImprovedMapEditorModel model)
        {
            this.model = null;
            this.improvedModel = model;
        }

        public void saveScenario()
        {
            if (improvedModel.GetScenario() != null)
            {
                Stream saveStream;
                SaveFileDialog saveMapDialog = new SaveFileDialog();
                saveMapDialog.InitialDirectory = Application.StartupPath + "\\Maps\\";
                saveMapDialog.Filter = "Map files (*.map)| *.map";

                if (saveMapDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((saveStream = saveMapDialog.OpenFile()) != null)
                    {
                        BinaryFormatter bin = new BinaryFormatter();

                        ScenarioComponent scenario = improvedModel.GetScenario();
                        scenario.SetContainer(null);
                        bin.Serialize(saveStream, scenario);
                        saveStream.Close();
                        improvedModel.AddChild(scenario);
                        // TODO: Change so that the SaveInfo model is updated.
                    }
                }
            }
        }

        public void loadScenario()
        {
            if (improvedModel.GetScenario() != null)
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
                // Deserialize the file and load it into the model.
                BinaryFormatter bin = new BinaryFormatter();
                ScenarioComponent scenario = (ScenarioComponent)bin.Deserialize(openMapDialog.OpenFile());
                CreateObserverListVisitor visitor = new CreateObserverListVisitor();
                scenario.Accept(visitor);
                scenario.GetGameWorld().GetMap().SetCellsToBeContainedInMap();
                improvedModel.AddChild(scenario);

                // Invalidate the Scenario view.
                scenario.GetGameWorld().NotifyAll();

                // TODO: Update the SaveInfo state.
            }
        }

        public void createNewScenario()
        {
            /********************
            // TODO Extract this logic with loadscenario.
            if (model.scenario != null)
            {
                // TODO: Ask if the user wants to discard the current scenario.
            }
            model.scenario = new ZRTSModel.Scenario.Scenario(20, 20);

            // Since this is a new scenario, it has not been saved and does not have a filename
            model.filename = null;
            model.saved = false;

            //The model has changed, so notify all views of the change.
            model.notifyAll();
            *************/

            if (improvedModel.GetScenario() != null)
            {
                // TODO: Ask if the user wants to discard the current scenario or save it.
            }
            ScenarioComponent scenario = new ScenarioComponent(20, 20);
            
            // TODO: Update SaveInfo model to change filename and UpToDate flag.
            
            // Automatically discards old scenario, by overloaded AddChild function.
            improvedModel.AddChild(scenario);
        }


        /* TODO public void selectEntities(List<Entity> entities)
        {

        }*/

        /* TODO public void selectEntityTypeToAdd(Entity entity)
        {

        }*/
        // TODO public void setTile(Tile tile, int x, int y)
        // TODO public void addEntity(Entity entity, int x, int y)
        // TODO public void moveEntities(List<Entity> entities, int x, int y)
        // TODO public void addTrigger(Trigger trigger)
        // TODO public void removeTrigger(Trigger trigger)
        public bool isOkayToClose()
        {
            // TODO Open dialog box to see if it is okay to discard changes, or to save first.
            // return model.saved;
            return true;
        }


        internal void updateCellType(int x, int y)
        {
            TileFactory tf = TileFactory.Instance;
            //Cell selectedCell = model.scenario.getGameWorld().map.getCell(x, y);
            //selectedCell.tile = tf.getTile(model.TileTypeSelected);
            //model.notifyAll();
            CellComponent cell = improvedModel.GetScenario().GetGameWorld().GetMap().GetCellAt(x, y);
            
            // Cell automatically removes old tile from overrided AddChild.
            cell.AddChild(tf.GetImprovedTile(improvedModel.TileTypeSelected)); // TODO: Change to new improvedModel here.
        }

        internal void selectTileType(string type)
        {
            // TODO Update to a proper interface.
            improvedModel.TileTypeSelected = type;

            //model.TileTypeSelected = type;
            //model.SelectionType = SelectionType.Tile;

            //The model has changed, so notify all views of the change.
            //model.notifyAll();
        }
    }
}
