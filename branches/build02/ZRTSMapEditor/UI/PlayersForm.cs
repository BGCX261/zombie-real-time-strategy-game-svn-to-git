

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;
using System.Runtime.CompilerServices;
using ZRTSModel.EventHandlers;


namespace ZRTSMapEditor
{
    /// <summary>
    /// PlayersForm Class
    /// An dialog that is part of the MapEditor UI
    /// Used for addingm, removing and editing PlayerComponents to the game Scenario
    /// 
    /// </summary>
    public partial class PlayersForm : Form
    {
        private PlayerList playerList;
        private BindingSource source;

        private PlayersForm()
        {
        }

        /// <summary>
        /// Constructs the PlayersForm from a PlayerList
        /// </summary>
        /// <param name="list">contains a list of the players</param>
        public PlayersForm(PlayerList list)
        {
            InitializeComponent();
            playerList = list;
            source = new BindingSource();

            // Because PlayerComponents do not encapsulate all of their data within fields, we must adapt them to
            // do so that the UI can bind itself to the data source.
            // The adapter merely places all of the relevant data in fields, and allows us to change the player list before
            // commiting the changes to the actual player list.
            foreach (PlayerComponent player in list.GetChildren())
            {
                PlayerDataGridAdapter adapter = new PlayerDataGridAdapter(player, playerList);
                source.Add(adapter);
            }


            // Initialize Player List to bind data properly.
            uiPlayerList.AutoGenerateColumns = false;
            uiPlayerList.AutoSize = true;
            uiPlayerList.DataSource = source;

            name.DataPropertyName = "Player_Name";
            race.DataPropertyName = "RaceMember";
            gold.DataPropertyName = "GoldMember";
            wood.DataPropertyName = "WoodMember";
            metal.DataPropertyName = "MetalMember";
        }

        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            PlayerDataGridAdapter adapter = new PlayerDataGridAdapter(new PlayerComponent(), playerList);
            // Default Values
            adapter.RaceMember = "Human";
            adapter.AddedMember = true;
            adapter.GoldMember = 100;
            adapter.WoodMember = 100;
            adapter.MetalMember = 100;
            source.Add(adapter);
            // Move Focus to added Player
            source.MoveLast();
            uiPlayerList.Focus();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            List<PlayerDataGridAdapter> adapters = new List<PlayerDataGridAdapter>();
            foreach (Object o in source.List)
            {
                adapters.Add((PlayerDataGridAdapter)o);
            }
            PlayerDataGridAdapterCommitter committer = new PlayerDataGridAdapterCommitter(adapters, playerList);
            if (committer.CanBeDone())
            {
                committer.Do();
                Close();
            }            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void removePlayerButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in uiPlayerList.SelectedRows)
            {
                PlayerDataGridAdapter adapter = (PlayerDataGridAdapter)row.DataBoundItem;
                adapter.RemovedMember = true;
                row.Selected = false;
                row.DefaultCellStyle.BackColor = Color.Red;
            }
        }
    }
}
