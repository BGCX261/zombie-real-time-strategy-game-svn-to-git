using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ZRTSMapEditor.MapEditorModel;

namespace ZRTSMapEditor
{
    /// <summary>
    /// A form for the entire map editor.
    /// </summary>
    public partial class MapEditorView : Form
    {
       
        private MapEditorController controller;

        public MapEditorView()
        {
            InitializeComponent();

            // Create the model.
            MapEditorFullModel model = new MapEditorFullModel();
            this.controller = new MapEditorController(model);
            controller.view = this;
            // Further initialize pieces of UI.
            tilePalette.loadImageList();
            tilePalette.setController(controller);
            mapViewComposite.Init(controller, model);
            unitPalette.Init(controller, model);
            buildingPalette1.Init(controller, model);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (controller.isOkayToClose())
            {
                Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.saveScenario();
        }
        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.loadScenario();
        }

        internal void selectedTileChanged(Image image)
        {
           // scenarioView1.changeImage(image);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createNewScenario();
        }

        private void playerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.OpenPlayersForm();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.UndoLastCommand();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            controller.RedoLastUndoCommand();
        }

        public void setTilePreview(string type)
        {
            tilePalette.setPreviewImage(type);
        }

        public void setEntityPreview(string type)
        {
            unitPalette.setPreviewImage(type);
        }
    }
}
