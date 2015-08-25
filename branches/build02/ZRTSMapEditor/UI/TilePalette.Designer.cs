namespace ZRTSMapEditor
{
    partial class TilePalette
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
            this.tileListBox = new System.Windows.Forms.ListBox();
            this.tilePreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tilePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // tileListBox
            // 
            this.tileListBox.FormattingEnabled = true;
            this.tileListBox.Location = new System.Drawing.Point(15, 59);
            this.tileListBox.Name = "tileListBox";
            this.tileListBox.Size = new System.Drawing.Size(96, 69);
            this.tileListBox.TabIndex = 0;
            this.tileListBox.SelectedIndexChanged += new System.EventHandler(this.tileListBox_SelectedIndexChanged);
            // 
            // tilePreview
            // 
            this.tilePreview.Location = new System.Drawing.Point(15, 13);
            this.tilePreview.Name = "tilePreview";
            this.tilePreview.Size = new System.Drawing.Size(73, 40);
            this.tilePreview.TabIndex = 1;
            this.tilePreview.TabStop = false;
            // 
            // TilePalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tilePreview);
            this.Controls.Add(this.tileListBox);
            this.Name = "TilePalette";
            this.Size = new System.Drawing.Size(129, 140);
            ((System.ComponentModel.ISupportInitialize)(this.tilePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox tileListBox;
        private System.Windows.Forms.PictureBox tilePreview;

    }
}
