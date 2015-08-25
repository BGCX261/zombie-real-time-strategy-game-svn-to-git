using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;

namespace ZRTSMapEditor
{
    class ZRTSMapEditor
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MapEditorView view = new MapEditorView();
            Application.Run(view);
        }
    }
}
