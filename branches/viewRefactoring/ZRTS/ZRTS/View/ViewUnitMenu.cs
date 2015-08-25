using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTS.View
{
    /// <summary>
    /// viewUnitMenu (Similar to soon to be deprecated class "viewGamePlayMenu.cs"
    /// Base:View
    /// 
    /// NOTe: This class may be using Observer interface as well since it should observe viewSelect if there is anything selected!
    /// 
    /// Author: Jason
    /// 
    /// This class will act as an user interface (aka menu) for unit(s) selected.
    /// Requirement
    /// (1) If multiple type of units selected, only common (commands) icons will be displayed.
    /// (2) Different type of unit selected will have different (commands) icons being displayed. 
    /// </summary>
    public class ViewUnitMenu:ViewAbstract
    {


      public override void loadSheet(SpriteSheet sheet)
      {
            base.loadSheet(sheet);
      }

   
       public override void Draw()
       {
 	        //base.draw();
       }
    }
}
