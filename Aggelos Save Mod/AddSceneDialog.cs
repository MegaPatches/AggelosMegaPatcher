using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aggelos_Save_Mod
{
    public partial class AddSceneDialog : Form
    {
        private int startLocationX;
        private int startLocationY;

        Scene[] scenesList;
        public Scene newScene = new Scene("My Scene", 13, 0, 0);

        public AddSceneDialog()
        {
            InitializeComponent();
        }

        //Adding a constructor override for specifying starting position of popup
        public AddSceneDialog(int x, int y) : this()
        {
            this.startLocationX = x;
            this.startLocationY = y;

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

            scenesList = new Scene[]
            {
                new Scene("Lumen Woods", 13, 2768, 191),
                new Scene("Bosco", 14, 3792, 447),
                new Scene("Castle / Basement", 15, 3680, 399),
                new Scene("Bosco Cave", 16, 1264, 895),
                new Scene("Earth Temple", 17, 50, 800),
                new Scene("Symbol Locations and Shops", 18, 0, 0),
                new Scene("Atlant", 19, 2848, 1247),
                new Scene("Cave Systems", 20, 0, 0),
                new Scene("Palulu / Outside Valion's Castle", 21, 1056, 399),
                new Scene("The Abyss", 22, 2672, 543),
                new Scene("Water Temple", 23, 50, 800),
                new Scene("Fira", 24, 2256, 655),
                new Scene("Fira Volcano", 25, 1328, 1071),
                new Scene("Fire Temple", 26, 3950, 575),
                new Scene("Woodpecker Trials", 27, 0, 0),
                new Scene("Darkness Opens Cutscene", 28, 0, 0),
                new Scene("Babel Tower", 29, 896, 1462),
                new Scene("Celestia", 30, 2016, 2047),
                new Scene("Dark Clouds", 31, 2544, 287),
                new Scene("Air Temple", 32, 700, 1630),
                new Scene("Castle Portal of Darkness", 33, 50, 553),
                new Scene("Fira Portal of Darkness", 34, 50, 610),
                new Scene("Valion's Castle", 35, 2168, 1199),
                new Scene("Downpour Portal of Darkness", 36, 50, 600)
            };

            //Load the combo box with the scenes list and their appropriate labels
            cbScenes.DataSource = scenesList;
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
            tbX.Text = scenesList[cbScenes.SelectedIndex].x.ToString();
            tbY.Text = scenesList[cbScenes.SelectedIndex].y.ToString();
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

    public class Scene
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Scene(string name, int id, int x, int y)
        {
            this.Name = name;
            this.ID = id;
            this.x = x;
            this.y = y;
        }
    }
}
