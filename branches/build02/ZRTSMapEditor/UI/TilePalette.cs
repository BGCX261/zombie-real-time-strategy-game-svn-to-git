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
    /// <summary>
    /// A UI piece for choosing a tile to be placed down on the map.
    /// TODO:  Implement SelectionStateVisitor and observe it.  On visiting, it should check if the selection state suggests that a tile
    /// is currently selected, and if so, should show its picture in a preview picture box.
    /// </summary>
    public partial class TilePalette : UserControl 
    {

        ImageList tileList = new ImageList();
        MapEditorController controller;

        public TilePalette()
        {
            InitializeComponent();
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

        public void setPreviewImage(string type)
        {
            TileFactory tf = TileFactory.Instance;
            tilePreview.Image = tf.getBitmap(type);
        }

        public string getSelectedImageName()
        {
            return (string) tileListBox.SelectedItem;
        }

    }
}
