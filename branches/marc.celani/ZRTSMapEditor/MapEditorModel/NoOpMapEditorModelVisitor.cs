using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSMapEditor.MapEditorModel
{
    public class NoOpMapEditorModelVisitor : NoOpModelComponentVisitor, MapEditorModelVisitor
    {

        public virtual void Visit(SelectionState selectionState)
        {
            Visit((ModelComponent)selectionState);
        }

        public virtual void Visit(MapEditorFullModel model)
        {
            Visit((ModelComponent)model);
        }


        public void Visit(CommandStack commandStack)
        {
            Visit((ModelComponent)commandStack);
        }
    }
}
