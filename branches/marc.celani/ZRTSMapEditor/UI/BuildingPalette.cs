using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;
using ZRTSMapEditor.MapEditorModel;
using ZRTSModel.Factories;

namespace ZRTSMapEditor
{
    /// <summary>
    /// A palette for selecting a building and a player to add it to so that it may be added on the map.
    /// Currently in a demo state.
    /// </summary>
    public partial class BuildingPalette : UserControl
    {
        private MapEditorController controller;
        private ScenarioComponent context;

        public BuildingPalette()
        {
            InitializeComponent();
            BuildingFactory uf = BuildingFactory.Instance;
            this.flowLayoutPanel1.SuspendLayout();
            foreach (string type in uf.getBuildingTypes())
            {
                PictureBox buildingBox = new PictureBox();
                buildingBox.Name = type;
                buildingBox.Size = new Size(50, 50);
                BitmapManager manager = BitmapManager.Instance;
                buildingBox.Image = manager.getBitmap(type);
                buildingBox.SizeMode = PictureBoxSizeMode.StretchImage;
                buildingBox.Margin = new Padding(1, 1, 1, 1);
                buildingBox.Click += uiBuildingIcon_Click;
                this.flowLayoutPanel1.Controls.Add(buildingBox);
            }
            this.flowLayoutPanel1.ResumeLayout();
        }

        public void Init(MapEditorController controller, MapEditorFullModel model)
        {
            this.controller = controller;
            
            context = model.GetScenario();
            model.ScenarioChangedEvent += this.ChangeScenario;
        }

        private void ChangeScenario(object sender, EventArgs e)
        {
            if (sender is MapEditorFullModel) // sanity check
            {
                context = ((MapEditorFullModel)sender).GetScenario();
            }
        }

        private void uiBuildingIcon_Click(object sender, EventArgs e)
        {
            controller.SelectBuildingType(((PictureBox)sender).Name);
        }

    }
}
