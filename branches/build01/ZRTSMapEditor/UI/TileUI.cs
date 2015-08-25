using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;
using ZRTSModel.GameWorld;
using ZRTSModel.EventHandlers;
using System.Diagnostics;

namespace ZRTSMapEditor
{
    public class TileUI : PictureBox
    {
        private MapEditorController controller;
        private CellComponent cell;

        public CellComponent Cell
        {
            get { return cell; }
            set { cell = value; }
        }

        public TileUI(MapEditorController controller, CellComponent observable)
        {
            InitializeComponent();
            this.controller = controller;
            this.cell = observable;

            // Initialize Image
            if (observable != null)
            {
                // Register for TileChange event.
                observable.TileChangedEvent += this.ChangeTile;
                observable.UnitAddedEvent += this.UnitAddedToCell;
                observable.UnitRemovedEvent += this.UnitRemovedFromCell;

                TileFactory tf = TileFactory.Instance;
                this.Image = tf.getBitmapImproved(observable.GetTile());

                foreach (ModelComponent m in observable.EntitiesContainedWithin)
                {
                    if (m is UnitComponent)
                    {
                        UnitUI unitUI = new UnitUI(controller, m as UnitComponent);
                        Controls.Add(unitUI);
                        unitUI.MouseClick += TileUI_MouseDown;
                    }
                }
            }

            

            AllowDrop = true;

        }

        private void ChangeTile(Object sender, TileChangedEventArgs args)
        {
            TileFactory tf = TileFactory.Instance;
            this.Image = tf.getBitmapImproved(args.Tile);
        }

        private void UnitAddedToCell(Object sender, UnitArgs args)
        {

            for (int i = 0; i < Controls.Count; )
            {
                if (Controls[0] != null)
                    Controls[0].Dispose();
            }
            UnitUI unitUI = new UnitUI(controller, args.Unit);
            Controls.Add(unitUI);
            unitUI.MouseClick += TileUI_MouseDown;
        }

        private void UnitRemovedFromCell(Object sender, UnitArgs args)
        {
            for (int i = 0; i < Controls.Count; )
            {
                if (Controls[0] != null)
                    Controls[0].Dispose();
            }
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // TileUI
            // 
            this.Margin = new System.Windows.Forms.Padding(0);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TileUI_DragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TileUI_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        private void TileUI_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.DoDragDrop("paint", DragDropEffects.None);
                float xpercent = 0, ypercent = 0;
                // xpercent = (float)e.X / (float)this.Width;
                // ypercent = (float)e.Y / (float)this.Height;
                controller.OnClickMapCell(cell, xpercent, ypercent);
                base.OnClick(e);
            }
        }

        private void TileUI_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(String)))
                return;

            if (e.Data.GetData(typeof(String)).Equals("paint"))
            {
                e.Effect = DragDropEffects.None;
                controller.OnDragMapCell(cell);
                base.OnClick(e);
            }
        }

        public void UnregisterFromEvents()
        {
            cell.TileChangedEvent -= this.ChangeTile;
            cell.UnitAddedEvent -= this.UnitAddedToCell;
            cell.UnitRemovedEvent -= this.UnitRemovedFromCell;
            AllowDrop = false;
            cell = null;
            // Keep the image from disposing.
            this.Image = null;
        }
    }
}
