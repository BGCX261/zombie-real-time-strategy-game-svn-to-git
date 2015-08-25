using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZRTSMapEditor
{
    public class Tile : PictureBox
    {
        private Form mapEditorView;

        public Tile(Form mapEditorView)
        {
            // TODO: Complete member initialization
            this.mapEditorView = mapEditorView;
        }
        protected override void OnClick(EventArgs e)
        {
            ((MapEditorView)mapEditorView).tileClicked(this);
            base.OnClick(e);
        }
    }
}
