using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
    public class UnitHPChangedEventArgs : EventArgs
    {
        private int newHP;
        private int oldHP;
        private UnitComponent unit;

        public int NewHP
        {
            get { return newHP; }
            set { newHP = value; }
        }
        public int OldHP
        {
            get { return oldHP; }
            set { oldHP = value; }
        }        
        public UnitComponent Unit
        {
            get { return unit; }
            set { unit = value; }
        }


    }
}
