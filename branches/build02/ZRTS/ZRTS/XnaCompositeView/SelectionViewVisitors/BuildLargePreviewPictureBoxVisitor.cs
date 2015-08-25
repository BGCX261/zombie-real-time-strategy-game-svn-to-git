using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTS.XnaCompositeView.SelectionViewVisitors
{
    public class BuildLargePreviewPictureBoxVisitor : NoOpModelComponentVisitor
    {
        PictureBox pictureBox;

        public PictureBox PictureBox
        {
            get { return pictureBox; }
        }

        public override void Visit(UnitComponent unit)
        {
            this.pictureBox = ZRTSCompositeViewUIFactory.Instance.BuildPictureBox("bigAvatar", unit.Type);
        }

        public override void Visit(Building building)
        {
            this.pictureBox = ZRTSCompositeViewUIFactory.Instance.BuildPictureBox("bigAvatar", building.Type);
        }
    }
}
