using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aggelos_Mega_Patcher
{
    public partial class AddSceneDialog : Form
    {
        private int startLocationX;
        private int startLocationY;
        public Scene newScene = new Scene("My Scene", 13, 0, 0);
        public Scene[] defaultScenesList;

        public AddSceneDialog()
        {
            InitializeComponent();
        }

        //Adding a constructor override for specifying starting position of popup
        public AddSceneDialog(int x, int y, Scene[] scenesList) : this()
        {
            this.startLocationX = x;
            this.startLocationY = y;
            this.defaultScenesList= scenesList;

            //Load the combo box with the scenes list and their appropriate labels
            cbScenes.DataSource = defaultScenesList;

            Load += new EventHandler(AddSceneDialog_Load);
        }

        //Adding a constructor override for specifying starting position of popup alongside a default to load for newScene
        public AddSceneDialog(int x, int y, Scene[] scenesList, Scene selectedScene) : this()
        {
            this.startLocationX = x;
            this.startLocationY = y;
            this.defaultScenesList = scenesList;

            //Load the combo box with the scenes list and their appropriate labels
            cbScenes.DataSource = defaultScenesList;

            this.newScene = selectedScene;

            Load += new EventHandler(AddSceneDialog_Load);
        }

        /************************************************************
        * AddSceneDialog_Load
        * 
        * This function handles loading defaults and setting desktop position
        ************************************************************/
        private void AddSceneDialog_Load(object sender, System.EventArgs e)
        {
            //Set the location of the popup
            this.SetDesktopLocation(startLocationX, startLocationY);

            //Set the combo box selection to the new scene value (in case one was passed in for copy)
            cbScenes.SelectedValue = newScene.ID;

            //Set the text box to a default prompt
            tbSceneName.Text = newScene.Name;
        }

        /************************************************************
        * btnCancel_Click
        * 
        * This function cancels the dialog and closes out.
        ************************************************************/
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Return a cancel result and close the popup
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /************************************************************
        * btnAdd_Click
        * 
        * This function sets the dialog result and closes out.
        ************************************************************/
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Return a cancel result and close the popup
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /************************************************************
        * cbScenes_SelectedIndexChanged
        * 
        * This function sets the return scene id based on the selection
        * made by the user in the combo box.
        ************************************************************/
        private void cbScenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Store the selected value into the ID of our scene to return
            newScene.ID = Int32.Parse(cbScenes.SelectedValue.ToString());

            //Set the default x/y values based on the selection
            tbX.Text = defaultScenesList[cbScenes.SelectedIndex].x.ToString();
            tbY.Text = defaultScenesList[cbScenes.SelectedIndex].y.ToString();
        }

        private void tbX_ValueChanged(object sender, EventArgs e)
        {
            //Set the return x value
            newScene.x = Int32.Parse(tbX.Text);
        }

        private void tbY_ValueChanged(object sender, EventArgs e)
        {
            //Set the return y value
            newScene.y = Int32.Parse(tbY.Text);
        }

        private void tbSceneName_TextChanged(object sender, EventArgs e)
        {
            //Set the return name of the scene save
            newScene.Name = tbSceneName.Text;
        }
    }
}
