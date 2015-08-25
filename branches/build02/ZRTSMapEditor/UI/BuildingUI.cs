using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;
using ZRTSModel.GameWorld;
using ZRTSModel.EventHandlers;
using ZRTSMapEditor.MapEditorModel;

namespace ZRTSMapEditor
{
    public class BuildingUI : PictureBox
    {
        private MapEditorController controller;
        private Building building;

        public Building Building
        {
            get { return building; }
            set { building = value; }
        }

        public BuildingUI(MapEditorController controller, Building building)
        {
            InitializeComponent();

            this.controller = controller;
            this.building = building;

            // Initialize Image
            if (building != null)
            {
                // TODO: Register for move events.

                // Get Image
                BitmapManager manager = BitmapManager.Instance;
                this.Image = manager.getBitmap(building.Type);
            }

        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // BuildingUI
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
