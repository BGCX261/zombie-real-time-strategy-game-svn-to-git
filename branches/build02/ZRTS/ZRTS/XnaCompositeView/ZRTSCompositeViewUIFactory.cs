using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView
{

    /// <summary>
    /// ZRTSCompositeViewUIFactory
    /// 
    /// This class will act as a manager to handle different image representation of user interfaces (buttons, menu) as well as game model.
    /// </summary>
    public class ZRTSCompositeViewUIFactory
    {
        private static ZRTSCompositeViewUIFactory instance;
        private Game game;

        private ZRTSCompositeViewUIFactory(){}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        private ZRTSCompositeViewUIFactory(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Initialize game instance
        /// </summary>
        /// <param name="game"></param>
        public static void Initialize(Game game)
        {
            if (instance == null)
            {
                instance = new ZRTSCompositeViewUIFactory(game);
            }
        }

        public static ZRTSCompositeViewUIFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Select Entity UI (Icon represent the unit)
        /// </summary>
        /// <param name="unit">Unit object</param>
        /// <returns>Unit selected by the user</returns>
        public SelectedEntityUI BuildSelectedEntityUI(UnitComponent unit)
        {
            SelectedEntityUI seui = new SelectedEntityUI(game, unit);
            seui.DrawBox = new Rectangle(0, 0, 75, 75);

            // Add the HP Bar to the UI.
            HPBar hpBar = new HPBar(game);
            hpBar.MaxHP = unit.MaxHealth;
            hpBar.CurrentHP = unit.CurrentHealth;
            hpBar.DrawBox = new Rectangle(5, 67, 65, 5);

            PictureBox pictureBox = BuildPictureBox("selectionAvatar",unit.Type);
            pictureBox.DrawBox = new Rectangle(7, 3, 61, 61);
            seui.AddChild(pictureBox);
            seui.AddChild(hpBar);
            return seui;
        }

        /// <summary>
        /// Create User Interface (icon) for each selected entity (Building)
        /// </summary>
        /// <param name="building">Building object</param>
        /// <returns>Building selected by the user</returns>
        public SelectedEntityUI BuildSelectedEntityUI(Building building)
        {
            SelectedEntityUI seui = new SelectedEntityUI(game, building);
            seui.DrawBox = new Rectangle(0, 0, 75, 75);

            // Add the HP Bar to the UI.
            HPBar hpBar = new HPBar(game);
            hpBar.MaxHP = building.MaxHealth;
            hpBar.CurrentHP = building.CurrentHealth;
            hpBar.DrawBox = new Rectangle(5, 67, 65, 5);

            PictureBox pictureBox = BuildPictureBox("selectionAvatar", building.Type);
            pictureBox.DrawBox = new Rectangle(7, 3, 61, 61);
            seui.AddChild(pictureBox);
            seui.AddChild(hpBar);
            return seui;
        }


        /// <summary>
        /// create a picture representation for each type
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="subtype">Subcategory type</param>
        /// <returns>picture representing user interface</returns>
        public PictureBox BuildPictureBox(string type, string subtype)
        {
            PictureBox pictureBox = null;

            if (type.Equals("button"))
            {
                if (subtype.Equals("move"))
                {
                    return new PictureBox(game, new Rectangle(GameConfig.BUTTON_MOVE*GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
                else if (subtype.Equals("attack"))
                {
                    return new PictureBox(game, new Rectangle(GameConfig.BUTTON_ATTACK*GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
                else if (subtype.Equals("build"))
                {
                    return new PictureBox(game, new Rectangle(GameConfig.BUTTON_BUILD*GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
                else if (subtype.Equals("stop"))
                {
                    return new PictureBox(game, new Rectangle(GameConfig.BUTTON_STOP * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
                else if (subtype.Equals("harvest"))
                {
                    return new PictureBox(game, new Rectangle(GameConfig.BUTTON_HARVEST * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
            }

            // Building Icon
            if (type.Equals("building"))
            {
                if (subtype.Equals("house"))
                {
                    return new PictureBox(game, new Rectangle(1141 + GameConfig.BUTTON_DIM, 1038 + GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
                else if (subtype.Equals("barracks"))
                {
                    return new PictureBox(game, new Rectangle(1141, 1038 + GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
                else if (subtype.Equals("hospital"))
                {
                    return new PictureBox(game, new Rectangle(1141 + GameConfig.BUTTON_DIM*2, 1038 + GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
            }

            // UnitIcon
            if (type.Equals("unitBuild"))
            {
                if (subtype.Equals("soldier"))
                {
                    return new PictureBox(game, new Rectangle(1141, 1038, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM ));
                }
                else if (subtype.Equals("worker"))
                {
                    return new PictureBox(game, new Rectangle(1141 + GameConfig.BUTTON_DIM, 1038, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
                }
            }

            // Selection Avator
            if (type.Equals("selectionAvatar"))
            {
                if (subtype.Equals("soldier"))
                {
                    return new PictureBox(game, new Rectangle(0, GameConfig.SELECT_AVATAR_START_Y, GameConfig.BUTTON_UNIT_DIM, GameConfig.BUTTON_UNIT_DIM));
                }
                else if (subtype.Equals("worker"))
                {
                    return new PictureBox(game, new Rectangle(77, GameConfig.SELECT_AVATAR_START_Y, GameConfig.BUTTON_UNIT_DIM, GameConfig.BUTTON_UNIT_DIM));
                }
            }


            // Big Avator
            if (type.Equals("bigAvatar"))
            {
                if (subtype.Equals("soldier"))
                {
                    return new PictureBox(game, new Rectangle(0, GameConfig.BIG_AVATAR_START_Y, 140, 152));
                }
                else if (subtype.Equals("worker"))
                {
                    return new PictureBox(game, new Rectangle(140, GameConfig.BIG_AVATAR_START_Y, 140, 152));
                }
            }

            else
                pictureBox = new PictureBox(game, new Rectangle(0, 0, 1, 1));


            return pictureBox;
        }
        

        /// <summary>
        /// Create a produce unit button for user menu
        /// </summary>
        /// <param name="unitType">Type of unit being produced</param>
        /// <returns>A button representing unit being produced</returns>
		public ProduceUnitButton BuildProduceUnitButton(string unitType)
		{
			Rectangle sourceRect = sourceRect = new Rectangle(0, GameConfig.SELECT_AVATAR_START_Y, 76, 76);
			ProduceUnitButton button = new ProduceUnitButton(game, sourceRect, unitType);
			button.UnitType = unitType;

			return button;
		}

        /// <summary>
        /// Construct a unit UI (image representation)
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>Image of unit</returns>
        public UnitUI BuildUnitUI(UnitComponent unit)
        {
            UnitUI unitUI = null;
            if (unit.Type.Equals("soldier"))
            {
                unitUI = new UnitUI(game, unit, new Rectangle(0, GameConfig.SOLDIER_START_Y, 36, 36));
                unitUI.DrawBox = new Rectangle(0, 0, GameConfig.UNIT_WIDTH, GameConfig.UNIT_HEIGHT);
            }
             
            else if (unit.Type.Equals("zombie"))
            {
                unit.IsZombie = true;
                unitUI = new UnitUI(game, unit, new Rectangle(0, GameConfig.ZOMBIE_START_Y, 36, 36));
                unitUI.DrawBox = new Rectangle(0, 0, GameConfig.UNIT_WIDTH, GameConfig.UNIT_HEIGHT);
            }

            else if (unit.Type.Equals("worker"))
            {
                unitUI = new UnitUI(game, unit, new Rectangle(0, GameConfig.WORKER_START_Y, 36, 36));
                unitUI.DrawBox = new Rectangle(0, 0, GameConfig.UNIT_WIDTH, GameConfig.UNIT_HEIGHT);
            }
            return unitUI;
        }

        /// <summary>
        /// Create an image representing the building
        /// </summary>
        /// <param name="building">Building</param>
        /// <returns>An image of the building</returns>
        internal BuildingUI BuildBuildingUI(Building building)
        {
            BuildingUI buildingUI = null;
            buildingUI = new BuildingUI(game, building, new Rectangle(0, 0, 1, 1));
            buildingUI.DrawBox = new Rectangle(0, 0, building.Width * GameConfig.TILE_DIM, building.Height * GameConfig.TILE_DIM);

            return buildingUI;
        }
    }
}
