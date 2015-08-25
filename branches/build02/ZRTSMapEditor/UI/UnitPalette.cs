using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;
using ZRTSMapEditor.MapEditorModel;
using ZRTSModel.Factories;

namespace ZRTSMapEditor
{
    /// <summary>
    /// A palette for selecting a unit and a player to add it to so that it may be added on the map.
    /// Currently in a demo state.
    /// </summary>
    public partial class UnitPalette : UserControl
    {
        private MapEditorController controller;
        private ScenarioComponent context;
        private PlayerList playerList;

        public UnitPalette()
        {
            InitializeComponent();
            UnitFactory uf = UnitFactory.Instance;
            this.flowLayoutPanel1.SuspendLayout();
            foreach (string type in uf.getPrefixes())
            {
                PictureBox unitBox = new PictureBox();
                unitBox.Name = type;
                unitBox.Size = new Size(50, 50);
                BitmapManager manager = BitmapManager.Instance;
                unitBox.Image = manager.getBitmap(type);
                unitBox.SizeMode = PictureBoxSizeMode.StretchImage;
                unitBox.Margin = new Padding(1, 1, 1, 1);
                unitBox.Click += uiUnitIcon_Click;
                this.flowLayoutPanel1.Controls.Add(unitBox);
            }
            this.flowLayoutPanel1.ResumeLayout();
        }

        public void Init(MapEditorController controller, MapEditorFullModel model)
        {
            this.controller = controller;
            
            context = model.GetScenario();
            InitContext();
            model.ScenarioChangedEvent += this.ChangeScenario;
        }

        private void InitContext()
        {
            if (context != null)
            {
                playerList = context.GetGameWorld().GetPlayerList();
                playerList.PlayerAddedEvent += this.ChangePlayerList;
                //playerList.PlayerRemovedEvent += this.ChangePlayerList;
                ChangePlayerList(playerList, null);
				playerList.PlayerRemovedEvent += this.ChangePlayerList;
            }
            else
            {
                playerList = null;
            }
        }

        private void ChangeScenario(object sender, EventArgs e)
        {
            if (sender is MapEditorFullModel) // sanity check
            {
                context = ((MapEditorFullModel)sender).GetScenario();
                InitContext();
            }
        }

        private void ChangePlayerList(object sender, EventArgs e)
        {
            if (sender is PlayerList) // sanity check
            {
                uiPlayerList.Items.Clear();
                foreach (PlayerComponent player in ((PlayerList)sender).GetChildren())
                {
                    uiPlayerList.Items.Add(player.Name);
                }
                uiPlayerList.Text = "";
            }
        }

        private void uiPlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            controller.SelectPlayer(uiPlayerList.SelectedItem.ToString());
        }

        private void uiUnitIcon_Click(object sender, EventArgs e)
        {
            controller.SelectUnitType(((PictureBox)sender).Name);
        }


        internal void setPreviewImage(string type)
        {
            BitmapManager manager = BitmapManager.Instance;
            previewBox.Image = manager.getBitmap(type);
        }
    }
}
