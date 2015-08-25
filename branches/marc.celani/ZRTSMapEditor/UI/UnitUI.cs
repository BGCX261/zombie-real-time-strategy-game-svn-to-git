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
    public class UnitUI : PictureBox
    {
        private MapEditorController controller;
        private UnitComponent unit;

        public UnitComponent Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public UnitUI(MapEditorController controller, UnitComponent unit)
        {
            InitializeComponent();

            this.controller = controller;
            this.unit = unit;

            // Initialize Image
            if (unit != null)
            {
                // TODO: Register for move events.

                // Get Image
                BitmapManager manager = BitmapManager.Instance;
                this.Image = manager.getBitmap(unit.Type);
            }

        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // UnitUI
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(20, 20);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
