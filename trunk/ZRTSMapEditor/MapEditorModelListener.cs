using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSMapEditor
{
    public interface MapEditorModelListener
    {
        void notify(MapEditorModelOld model);
    }
}
