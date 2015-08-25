using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSMapEditor
{
    /// <summary>
    /// An interface for do/undoable commands that can be placed into the CommandStack.
    /// </summary>
    public interface MapEditorCommand
    {
        void Do();
        void Undo();
        bool CanBeDone();
    }
}
