using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZRTSModel.GameWorld;

namespace ZRTSMapEditor
{
    public partial class TilePalette : UserControl, MapEditorModelListener 
    {

        ImageList tileList = new ImageList();
        MapEditorController controller;
        string currentlyDisplayedTileType = null;
        private Form mapEditorView;

        public TilePalette()
        {
            InitializeComponent();
            //loadImageList();
        }

        public TilePalette(Form mapEditorView) : this()
        {
            // TODO: Complete member initialization
            this.mapEditorView = mapEditorView;
        }

        public void setController(MapEditorController controller)
        {
            this.controller = controller;
        }

        public void loadImageList()
        {
            TileFactory tf = TileFactory.Instance;
            foreach (string type in tf.getTileTypes())
            {
                tileList.Images.Add(tf.getBitmap(type));
                tileListBox.Items.Add(type);
            }
        }

        private void tileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controller != null)
            {
                controller.selectTileType(tileListBox.SelectedItem.ToString());
            }
        }

        public Image getSelectedImage()
        {
            return tilePreview.Image;
        }

        public string getSelectedImageName()
        {
            return (string) tileListBox.SelectedItem;
        }

        public void notify(MapEditorModelOld model)
        {
            if (model.SelectionType == SelectionType.Tile)
            {
                currentlyDisplayedTileType = model.TileTypeSelected;
                TileFactory tf = TileFactory.Instance;
                tilePreview.Image = tf.getBitmap(currentlyDisplayedTileType);
            }
            else
            {
                currentlyDisplayedTileType = null;
                tilePreview.Image = null;
            }
        }

    }
}
