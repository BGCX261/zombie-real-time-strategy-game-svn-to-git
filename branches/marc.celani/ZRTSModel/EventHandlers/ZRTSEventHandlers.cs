using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZRTSModel.EventHandlers
{
    #region ZRTS Delegates

    public delegate void ScenarioChangedHandler(Object sender, ScenarioChangedEventArgs args);
    public delegate void TileChangedHandler(Object sender, TileChangedEventArgs args);
    public delegate void PlayerListChangedHandler(Object sender, PlayerListChangedEventArgs args);
    public delegate void EntityInCellChangedHandler(Object sender, UnitArgs args);
    public delegate void UnitHPChangedHandler(Object sender, UnitHPChangedEventArgs args);
    public delegate void SelectionStateChangedHandler(Object sender, SelectionStateChangedArgs args);
    public delegate void BuildingAddedOrRemovedHandler(Object sender, BuildingAddedEventArgs e);
	public delegate void ModelComponentSelectedHandler(Object sender, bool selected);
	public delegate void UnitListChangedHandler(Object sender, UnitListChangedEventArgs args);
	public delegate void GameVictoryStateChangedHandler(Object sender, GameVictoryStateChangeEventArgs args);


	/** Unit Based Events **/
	public delegate void UnitAttackStanceChangedHandler(Object sender, UnitAttackStanceChangedArgs args);
	public delegate void UnitAttackedEnemyHandler(Object sender, UnitAttackedEnemyArgs args);
	public delegate void UnitStateChangedHanlder(Object sender, UnitStateChangedEventArgs args);
	public delegate void UnitOrientationChangedHandler(Object sender, UnitOrientationChangedEventArgs args);
	public delegate void UnitMovedHandler(Object sender, UnitMovedEventArgs args);
	public delegate void UnitAddedToPlayerListHandler(Object sender, UnitAddedEventArgs args);
	public delegate void UnitRemovedFromPlayerListHandler(Object sender, UnitRemovedEventArgs args);

    #endregion
}
