using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZRTSMapEditor.UI
{
    /// <summary>
    /// A dialog box for setting the size of a new scenario.
    /// </summary>
    public partial class CreateNewScenarioDialog : Form
    {
        private string name = null;
        private int width = 0;
        private int height = 0;
        private bool exitWithCreate = false;

        public string ScenarioName
        {
            get { return name; }
            set { name = value; }
        }
        
        public int ScenarioWidth
        {
            get { return width; }
            set { width = value; }
        }

        public int ScenarioHeight
        {
            get { return height; }
            set { height = value; }
        }

        public bool ExitWithCreate
        {
            get { return exitWithCreate; }
            set { exitWithCreate = value; }
        }

        public CreateNewScenarioDialog()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            name = nameBox.Text;
            try 
	        {
                width = Int32.Parse(widthBox.Text);
                height = Int32.Parse(heightBox.Text);

                // Ensure that the name is not just white space.
                exitWithCreate = !name.Replace(" ", "").Replace("\t", "").Equals("");
                if (exitWithCreate)
                {
                    // Ensure that the size is valid.
                    exitWithCreate = (width > 0);
                    if (exitWithCreate)
                    {
                        exitWithCreate = (height > 0);
                        if (exitWithCreate)
                        {
                            Close();
                        }
                    }
                }

                if (!exitWithCreate)
                {
                    // TODO: Add Error Box.
                }
	        }
	        catch (Exception)
	        {
		        // TODO: Add Error Box
	        }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
