using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSMapEditor.UI;

namespace ZRTSMapEditor.Commands
{
    class RefreshUICommand : ModelCommand
    {
        private RefreshableUI targetUI;

        private RefreshUICommand()
        { }

        public RefreshUICommand(RefreshableUI target)
        {
            targetUI = target;
        }

        public void Do()
        {
            targetUI.Refresh();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool CanBeDone()
        {
            return true;
        }
    }
}
