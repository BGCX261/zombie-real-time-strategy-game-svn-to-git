using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Trigger
{
    [Serializable()]
    public abstract class TriggerDecorator : Trigger
    {
        private Trigger decorated = null;
        protected bool isMet;
        protected bool needsToBeEvaled = true;

        public TriggerDecorator()
        {

        }

        public TriggerDecorator(Trigger decorated)
        {
            this.decorated = decorated;
        }

        public virtual void PerformActions()
        {
            //decorated.PerformActions();
            //PerformMyAction();
        }

        public virtual bool Eval()
        {
			////Console.WriteLine("Condition Checked");
			return false;
        }

        public bool IsMet()
        {
            return isMet;
        }

        public bool NeedsToBeEvaluated()
        {
            return needsToBeEvaled;
        }

        public abstract void PerformMyAction();
        public abstract bool CheckMyCondition();
    }
}
