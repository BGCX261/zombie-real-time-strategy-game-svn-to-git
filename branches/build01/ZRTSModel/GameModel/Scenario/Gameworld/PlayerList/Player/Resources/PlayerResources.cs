using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public class PlayerResources : ModelComponent
    {
        private int gold = 0;
        private int wood = 0;
        private int metal = 0;

        public int Gold
        {
            get
            {
                return gold;
            }
            set
            {
                gold = value;
            }
        }

        public int Wood
        {
            get
            {
                return wood;
            }
            set
            {
                wood = value;
            }
        }

        public int Metal
        {
            get
            {
                return metal;
            }
            set
            {
                metal = value;
            }
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
