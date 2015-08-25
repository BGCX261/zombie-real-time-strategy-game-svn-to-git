using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTS.XnaCompositeView.SelectionViewVisitors
{
    public class BuildSelectedEntityUIVisitor : NoOpModelComponentVisitor
    {
        private SelectedEntityUI ui = null;

        public SelectedEntityUI UI
        {
          get { return ui; }
        }

        public override void Visit(UnitComponent unit)
        {
            ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
            ui = factory.BuildSelectedEntityUI(unit);
        }

        public override void Visit(Building building)
        {
            ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
            ui = factory.BuildSelectedEntityUI(building);
        }
    }
}
