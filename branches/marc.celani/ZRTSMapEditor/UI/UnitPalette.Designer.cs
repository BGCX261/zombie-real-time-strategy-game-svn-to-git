using System.Windows.Forms;
namespace ZRTSMapEditor
{
    partial class UnitPalette : UserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.uiPlayerList = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // previewBox
            // 
            this.previewBox.Location = new System.Drawing.Point(3, 3);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(124, 85);
            this.previewBox.TabIndex = 0;
            this.previewBox.TabStop = false;
            // 
            // uiPlayerList
            // 
            this.uiPlayerList.FormattingEnabled = true;
            this.uiPlayerList.Location = new System.Drawing.Point(3, 94);
            this.uiPlayerList.Name = "uiPlayerList";
            this.uiPlayerList.Size = new System.Drawing.Size(124, 21);
            this.uiPlayerList.TabIndex = 2;
            this.uiPlayerList.SelectedIndexChanged += new System.EventHandler(this.uiPlayerList_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 121);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(124, 161);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // UnitPalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.uiPlayerList);
            this.Controls.Add(this.previewBox);
            this.Name = "UnitPalette";
            this.Size = new System.Drawing.Size(130, 285);
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox previewBox;
        public ComboBox uiPlayerList;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
