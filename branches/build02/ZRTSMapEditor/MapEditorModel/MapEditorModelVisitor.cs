using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSMapEditor.MapEditorModel;

namespace ZRTSMapEditor
{
    public interface MapEditorModelVisitor : ModelComponentVisitor
    {
        void Visit(SelectionState selectionState);
        void Visit(MapEditorFullModel model);
        void Visit(CommandStack commandStack);
    }
}
