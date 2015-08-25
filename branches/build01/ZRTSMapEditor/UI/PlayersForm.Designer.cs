namespace ZRTSMapEditor
{
    partial class PlayersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uiPlayerList = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.race = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wood = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.metal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addPlayerButton = new System.Windows.Forms.Button();
            this.removePlayerButton = new System.Windows.Forms.Button();
            this.submitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.uiPlayerList)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPlayerList
            // 
            this.uiPlayerList.AllowUserToAddRows = false;
            this.uiPlayerList.AllowUserToDeleteRows = false;
            this.uiPlayerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiPlayerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.race,
            this.gold,
            this.wood,
            this.metal});
            this.uiPlayerList.Location = new System.Drawing.Point(14, 19);
            this.uiPlayerList.Name = "uiPlayerList";
            this.uiPlayerList.Size = new System.Drawing.Size(449, 171);
            this.uiPlayerList.TabIndex = 0;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // race
            // 
            this.race.HeaderText = "Race";
            this.race.Items.AddRange(new object[] {
            "Human",
            "Zombie"});
            this.race.Name = "race";
            // 
            // gold
            // 
            this.gold.HeaderText = "Starting Gold";
            this.gold.Name = "gold";
            // 
            // wood
            // 
            this.wood.HeaderText = "Starting Wood";
            this.wood.Name = "wood";
            // 
            // metal
            // 
            this.metal.HeaderText = "Starting Metal";
            this.metal.Name = "metal";
            // 
            // addPlayerButton
            // 
            this.addPlayerButton.Location = new System.Drawing.Point(24, 196);
            this.addPlayerButton.Name = "addPlayerButton";
            this.addPlayerButton.Size = new System.Drawing.Size(110, 20);
            this.addPlayerButton.TabIndex = 1;
            this.addPlayerButton.Text = "Add Player";
            this.addPlayerButton.UseVisualStyleBackColor = true;
            this.addPlayerButton.Click += new System.EventHandler(this.addPlayerButton_Click);
            // 
            // removePlayerButton
            // 
            this.removePlayerButton.Location = new System.Drawing.Point(24, 222);
            this.removePlayerButton.Name = "removePlayerButton";
            this.removePlayerButton.Size = new System.Drawing.Size(110, 20);
            this.removePlayerButton.TabIndex = 2;
            this.removePlayerButton.Text = "Remove Player";
            this.removePlayerButton.UseVisualStyleBackColor = true;
            this.removePlayerButton.Click += new System.EventHandler(this.removePlayerButton_Click);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(237, 208);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(110, 20);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(353, 208);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 20);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // PlayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.removePlayerButton);
            this.Controls.Add(this.addPlayerButton);
            this.Controls.Add(this.uiPlayerList);
            this.Name = "PlayersForm";
            this.Text = "Players";
            ((System.ComponentModel.ISupportInitialize)(this.uiPlayerList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView uiPlayerList;
        private System.Windows.Forms.Button addPlayerButton;
        private System.Windows.Forms.Button removePlayerButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewComboBoxColumn race;
        private System.Windows.Forms.DataGridViewTextBoxColumn gold;
        private System.Windows.Forms.DataGridViewTextBoxColumn wood;
        private System.Windows.Forms.DataGridViewTextBoxColumn metal;
    }
}