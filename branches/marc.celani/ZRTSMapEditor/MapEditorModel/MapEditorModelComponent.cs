using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSMapEditor.MapEditorModel
{
    public abstract class MapEditorModelComponent : ModelComponent
    {
        public abstract void Accept(MapEditorModelVisitor visitor);
    }
}
