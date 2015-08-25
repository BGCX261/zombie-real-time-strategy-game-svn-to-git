using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZRTSMapEditor
{
    public partial class ScenarioView : UserControl
    {

        private Form mapEditorView;

        public ScenarioView()
        {
            InitializeComponent();
        }

        public ScenarioView(Form mapEditorView) : this()
        {
            this.mapEditorView = mapEditorView;
        }

        public void changeImage(Image tile)
        {
            Tile p = new Tile(mapEditorView);
            p.Image = tile;
            tableLayoutPanel1.Controls.Add(p);
        }
    }
}
