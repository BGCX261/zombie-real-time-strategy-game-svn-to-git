using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Trigger
{
    [Serializable()]
    class EmptyTrigger : Trigger
    {
        public void PerformActions()
        {
            // Do nothing
        }

        public bool Eval()
        {
            return true;
        }

        public bool NeedsToBeEvaluated()
        {
            return false;
        }

        public bool IsMet()
        {
            return true;
        }
    }
}
