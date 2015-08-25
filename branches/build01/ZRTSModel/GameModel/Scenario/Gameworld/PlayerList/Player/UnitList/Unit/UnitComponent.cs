using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;
using ZRTSModel.GameModel;

namespace ZRTSModel
{
    /// <summary>
    /// A particular unit.  Contains an action queue.
    /// </summary>
    [Serializable()]
    public class UnitComponent : ModelComponent
	{
		#region Attributes and Fields
		#region Stats
		private string type;
		/// <summary>
		/// The string defining what type of Unit this is.
		/// </summary>
		public string Type
		{
			get { return type; }
			set { type = value; }
		}
		private short maxHealth = 100;       // The maximum health of a unit.

		/// <summary>
		/// The maximum health of the Unit.
		/// </summary>
		public short MaxHealth
		{
			get { return maxHealth; }
			set { maxHealth = value; }
		}

		private short currentHealth;

		/// <summary>
		/// How much health the Unit currently has.
		/// </summary>
		public short CurrentHealth
		{
			get { return currentHealth; }
			set
			{
				short prevHealth = currentHealth;
				currentHealth = value;
				if (HPChangedEventHandlers != null)
				{
					UnitHPChangedEventArgs args = new UnitHPChangedEventArgs();
					args.OldHP = (int)prevHealth;
					args.NewHP = (int)currentHealth;
					args.Unit = this;
					HPChangedEventHandlers(this, args);
				}
				if (CurrentHealth <= 0)
				{
					this.State = UnitState.DEAD;
					ModelComponent parent = Parent;
					Parent.RemoveChild(this);
				}
			}
		}

		private float speed = 0.13f;          // How much a Unit should move during a move cycle.
		/// <summary>
		/// How far a Unit moves per Move cycle.
		/// </summary>
		public float Speed
		{
			get { return speed; }
			set { speed = value; }
		}
		private float attackRange = 5.0f;    // How far a Unit can attack

		/// <summary>
		/// How far a Unit can attack.
		/// </summary>
		public float AttackRange
		{
			get { return attackRange; }
			set { attackRange = value; }
		}
		private short attack = 10;           // How much damage a Unit does when it attacks.

		/// <summary>
		/// How much damage a Unit does when it attacks.
		/// </summary>
		public short Attack
		{
			get { return attack; }
			set { attack = value; }
		}
		private byte attackTicks = 10;       // How many ticks occur per attack cycle.

		/// <summary>
		/// How many ticks occur per attack cycle.
		/// </summary>
		public byte AttackTicks
		{
			get { return attackTicks; }
			set { attackTicks = value; }
		}
		private float visibilityRange = 6.0f;// How far can the unit see.

		/// <summary>
		/// How far can the unit see.
		/// </summary>
		public float VisibilityRange
		{
			get { return visibilityRange; }
			set { visibilityRange = value; }
		}
		private byte buildSpeed = 30;        // How much health a building gets per build cycle the Unit completes when building or repairing a building.

		/// <summary>
		/// How much health a building gets per build cyycle a Unit completes.
		/// </summary>
		public byte BuildSpeed
		{
			get { return buildSpeed; }
			set { buildSpeed = value; }
		}

		#endregion

		#region Unit Abilities
		private bool canAttack = false;      // Can this Unit attack?

		/// <summary>
		/// Can this Unit attack?
		/// </summary>
		public bool CanAttack
		{
			get { return canAttack; }
			set { canAttack = value; }
		}
		private bool canHarvest = false;     // Can this Unit harvest resources?

		/// <summary>
		/// Can this Unit harvest resources?
		/// </summary>
		public bool CanHarvest
		{
			get { return canHarvest; }
			set { canHarvest = value; }
		}
		private bool canBuild = false;       // Can this Unit build buildings?

		/// <summary>
		/// Can this Unit build Buildings?
		/// </summary>
		public bool CanBuild
		{
			get { return canBuild; }
			set { canBuild = value; }
		}
		private bool isZombie = false;       // Is this Unit a zombie?

		/// <summary>
		/// Is this Unit a zombie?
		/// </summary>
		public bool IsZombie
		{
			get { return isZombie; }
			set { isZombie = value; }
		}
		#endregion

		#region Location Related Attributes
		private CellComponent location;
		/// <summary>
		/// The CellComponent containing the UnitComponent.
		/// </summary>
		public CellComponent Location
		{
			get { return location; }
		}

		public enum Orient { N, S, E, W, NW, NE, SW, SE };

		private Orient orient = Orient.S;
		/// <summary>
		/// Which direction the UnitComponent is facing in the GameWorld.
		/// </summary>
		public Orient UnitOrient
		{
			get { return this.orient; }
			set
			{

				// Fire off an UnitOrientationChanged event.
				if (UnitOrienationChangedHandlers != null && this.orient != value)
				{
					UnitOrienationChangedHandlers(this, null);
				}
				this.orient = value;
			}
		}


		private PointF pointLocation;

		/// <summary>
		/// The UnitComponents exact location.
		/// </summary>
		public PointF PointLocation
		{
			get { return pointLocation; }
			set
			{
				UnitMovedEventArgs args = new UnitMovedEventArgs();
				args.Unit = this;
				args.OldPoint = pointLocation;
				args.NewPoint = value;

				pointLocation = value;
				if (location != null)
				{
					if (location.X != (int)pointLocation.X || location.Y != (int)pointLocation.Y)
					{
						// Stop listening to 
						stopListeningToCells(args.OldPoint);

						// Remove UnitComponent from old CellComponent.
						location.RemoveEntity(this);
						if (pointLocation != null)
						{
							// Add UnitComponent to new CellComponent.
							Map map = (Map)location.Parent;
							location = map.GetCellAt((int)pointLocation.X, (int)pointLocation.Y);
							location.AddEntity(this);

							// Have UnitComponent listen to cells within its visibility range.
							listenToCellsWithinVisibilityRange();
						}
					}
				}
				else if (pointLocation != null && Parent != null)
				{
					// Add UnitComponent to new CellComponent
					Map map = ((Gameworld)(Parent.Parent.Parent.Parent)).GetMap();
					location = map.GetCellAt((int)pointLocation.X, (int)pointLocation.Y);
					location.AddEntity(this);

					// Have UnitComponent listen to cells withing its visibility range.
					listenToCellsWithinVisibilityRange();
				}
				if (MovedEventHandlers != null)
				{
					MovedEventHandlers(this, args);
				}

			}
		}

		#endregion

		#region State / Attackstance
		/// <summary>
		/// Determines how a UnitComponent reacts to enemies that it sees.
		/// Passive - Ignore
		/// Guard - Chase and attack enemy for a short distance, then return to original cell.
		/// Aggressive - Chase and attack enemy until it is dead.
		/// </summary>
		public enum UnitAttackStance { Passive, Guard, Aggressive };

		private UnitAttackStance attackStance = UnitAttackStance.Aggressive;

		/// <summary>
		/// The UnitComponent's current attack stance. Assigning to this fires off a UnitAttackStanceChanged event.
		/// </summary>
		public UnitAttackStance AttackStance
		{
			get { return this.attackStance; }
			set
			{
				UnitAttackStanceChangedArgs args = new UnitAttackStanceChangedArgs(this, value, this.attackStance);
				this.attackStance = value;

				if (UnitAttackStanceEventHandlers != null)
				{
					// Fire off an attack stance changed event.
					UnitAttackStanceEventHandlers(this, args);
				}
			}
		}

		public enum UnitState { IDLE, ATTACKING, MOVING, HARVESTING, BUILDING, DEAD };

		private UnitState state = UnitState.IDLE;

		/// <summary>
		/// The UnitComponent's current state (ie. what action the UnitComponent is currently doing)
		/// </summary>
		public UnitState State
		{
			get { return this.state; }
			set
			{
				UnitStateChangedEventArgs args = new UnitStateChangedEventArgs(this, this.state, value);
				this.state = value;

				if (this.state == UnitState.DEAD)
				{
					location.RemoveEntity(this);
				}
				// Fire off a UnitStateChanged event.
				if (UnitStateChangedHandlers != null)
				{
					UnitStateChangedHandlers(this, args);
				}
			}
		}

		#endregion

		private ActionQueue actionQueue;

		public ActionQueue GetActionQueue()
		{
			return actionQueue;
		}

		#endregion

		#region Event Handlers
		/// <summary>
		/// Handlers listening to when the HP of this Unit changes.
		/// </summary>
        public event UnitHPChangedHandler HPChangedEventHandlers;
		/// <summary>
		/// Handlers listening to when this Unit moves.
		/// </summary>
        public event UnitMovedHandler MovedEventHandlers;
		/// <summary>
		/// Handlers listening to when this Unit moves to a new Cell.
		/// </summary>
		public event EntityInCellChangedHandler UnitAddedToCellHandlers;
		/// <summary>
		/// Handlers listening to when this Unit changes its attack stance.
		/// </summary>
		public event UnitAttackStanceChangedHandler UnitAttackStanceEventHandlers;
		/// <summary>
		/// Handlers listening to when this Unit attacks an Enemy.
		/// </summary>
		public event UnitAttackedEnemyHandler UnitAttackedEnemyHanlders;
		/// <summary>
		/// Handlers listening to when the Unit's state changes.
		/// </summary>
		public event UnitStateChangedHanlder UnitStateChangedHandlers;

		public event UnitOrientationChangedHandler UnitOrienationChangedHandlers;

		#endregion

		#region Constructors
		public UnitComponent()
        {
            actionQueue = new ActionQueue();
            AddChild(actionQueue);
			UnitAddedToCellHandlers = handleUnitAddedToCell;
        }

		public UnitComponent(Entities.UnitStats stats)
		{
			actionQueue = new ActionQueue();
			AddChild(actionQueue);
			UnitAddedToCellHandlers = handleUnitAddedToCell;

			this.maxHealth = stats.maxHealth;
			this.currentHealth = stats.maxHealth;
			this.speed = stats.speed;
			this.buildSpeed = stats.buildSpeed;
			this.canAttack = stats.canAttack;
			this.canBuild = stats.canBuild;
			this.canHarvest = stats.canHarvest;
			this.isZombie = stats.isZombie;
			this.type = stats.type;
			this.visibilityRange = stats.visibilityRange;
			this.attack = stats.attack;
			this.attackRange = stats.attackRange;
			this.attackTicks = stats.attackTicks;
		}
		#endregion

		#region Boundary Checking Functions
		private int getStartX()
		{
			int startX = (int)pointLocation.X - (int)visibilityRange;

			if (startX <= 0)
			{
				startX = 0;
			}
			return startX;
		}

		private int getEndX()
		{
			int endX = (int)pointLocation.X + (int)visibilityRange;
			ModelComponent gameWorldComponent = location;
			while (!(gameWorldComponent is Gameworld))
				gameWorldComponent = gameWorldComponent.Parent;
			Map map = ((Gameworld)(gameWorldComponent)).GetMap();

			if (endX >= map.GetWidth())
			{
				endX = map.GetWidth() - 1;
			}

			return endX;
		}


		private int getStartY()
		{
			int startY = (int)pointLocation.Y - (int)visibilityRange;

			if (startY <= 0)
			{
				startY = 0;
			}
			return startY;
		}

		private int getEndY()
		{
			int endY = (int)pointLocation.Y + (int)visibilityRange;
			ModelComponent gameWorldComponent = location;
			while (!(gameWorldComponent is Gameworld))
				gameWorldComponent = gameWorldComponent.Parent;
			Map map = ((Gameworld)(gameWorldComponent)).GetMap();

			if (endY >= map.GetHeight())
			{
				endY = map.GetHeight() - 1;
			}

			return endY;
		}

		#endregion

		#region Methods for Updating CellComponents being listened to.
		private void listenToCellsWithinVisibilityRange()
		{
			int startX = getStartX();
			int endX = getEndX();
			int startY = getStartY();
			int endY = getEndY();

			ModelComponent gameWorldComponent = location;
			while (!(gameWorldComponent is Gameworld))
				gameWorldComponent = gameWorldComponent.Parent;
			Map map = ((Gameworld)(gameWorldComponent)).GetMap();

			for (int i = startX; i <= endX; i++)
			{
				for (int j = startY; j <= endY; j++)
				{
					CellComponent cell = map.GetCellAt(i, j);
					cell.UnitAddedEvent += new EntityInCellChangedHandler(handleUnitAddedToCell);
				}
			}
		}


		private void stopListeningToCells(PointF oldPoint)
		{
			int startX = getStartX();
			int endX = getEndX();
			int startY = getStartX();
			int endY = getEndY();

			ModelComponent gameWorldComponent = location;
			while (!(gameWorldComponent is Gameworld))
				gameWorldComponent = gameWorldComponent.Parent;
			Map map = ((Gameworld)(gameWorldComponent)).GetMap();

			for (int i = startX; i < endX; i++)
			{
				for (int j = startY; j < endY; j++)
				{
					CellComponent cell = map.GetCellAt(i, j);
					cell.UnitAddedEvent -= new EntityInCellChangedHandler(handleUnitAddedToCell);
				}
			}
		}
		#endregion

		#region Composite Pattern Methods
		public override void RemoveChild(ModelComponent child)
        {
            if (child != actionQueue)
                base.RemoveChild(child);
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
		#endregion

		#region Event Related Methods
		/// <summary>
		/// Creates and fires an UnitAttackedEnemyEvent
		/// </summary>
		/// <param name="target">The target the UnitComponent is attacking.</param>
		public void createAttackEvent(ModelComponent target)
		{
			if (UnitAttackedEnemyHanlders != null)
			{
				UnitAttackedEnemyHanlders(this, new UnitAttackedEnemyArgs(target));
			}
		}

		private void handleUnitAddedToCell(object obj, UnitArgs e)
		{
			if (e.Unit == this) // I'm seeing my own move event.
			{}
			else // Saw another Unit added to a CellComponent.
			{
				if (unitIsAnEnemy(e.Unit) && this.AttackStance == UnitAttackStance.Aggressive && this.actionQueue.GetChildren().Count == 0)
				{
					ModelComponent temp = Parent;
					while (!(temp is Gameworld))
					{
						temp = temp.Parent;
					}
					AttackAction attackAction = new AttackAction(this, e.Unit,(Gameworld)temp);
					actionQueue.AddChild(attackAction);
				}
			}
		}

		private bool unitIsAnEnemy(UnitComponent unit)
		{
			// Find the PlayerComponent of unit.
			ModelComponent temp = unit.Parent;
			while(!(temp is PlayerComponent || temp == null))
			{
				temp = temp.Parent;
			}
			if (temp == null)
			{
				return false;
			}
			PlayerComponent unitOwner = (PlayerComponent)temp;

			// Find the PlayerComponent of this UnitComponent;
			temp = this.Parent;
			while (!(temp is PlayerComponent || temp == null))
			{
				temp = temp.Parent;
			}
			if (temp == null)
			{
				return false;
			}
			PlayerComponent myOwner = (PlayerComponent)temp;

			return myOwner.EnemyList.Contains(unitOwner);
		}

		#endregion
	}
}
