using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Win32;
using System.Windows.Forms.VisualStyles;

/***************************
 * NOTES:
 * 
 * - Manipulating checkboxes through code triggers other checkboxes. Need to make sure they are exclusively doing what I need
 * - Need to figure out harp and water vial states
 * - Rework how save file/slot is stored in class
 * - During testing found bug when saving in game after loading various presets (still not sure what causes it). 
 * - Add way to delete a file from the tool in case the above happens.
 * - Look for cutscene triggers to allow a "skip cutscenes" option if it doesn't break progression of game.
 * - Change interface for potions to be a cycle with an arrow? Maybe for anything that has a state do this for clarity....
 ***************************/

namespace Aggelos_Save_Mod
{
    public partial class Form1 : Form
    {
        //Declaring global save file object for use throughout the program
        public Save saveFile = new Save();

        //Declaring preset file being used
        public string presetFile = "";

        public Form1()
        {
            InitializeComponent();

            //Show detected intall path
            tbInstallPath.Text = saveFile.InstallationPath;
        }

        /************************************************************
         * RefreshUI
         * 
         * This function is called to refresh the values displayed to
         * the user based on the currently loaded save file.
         ************************************************************/
        private void RefreshUI()
        {
            //Make sure we have loaded a file first
            if (saveFile.FileLoaded)
            {
                //Set the default save slot to whatever was found in the preset,
                //otherwise 3 in case the other two slots are for casual
                switch (saveFile.slotNumber)
                {
                    case "sauvegarde1":
                        radioSaveSlot1.Checked = true;
                        break;
                    case "sauvegarde2":
                        radioSaveSlot2.Checked = true; 
                        break;
                    case "savegarde3":
                        radioSaveSlot3.Checked = true;
                        break;
                    default:
                        radioSaveSlot3.Checked = true;
                        break;
                }

                tbFileSelected.Text = presetFile;
                btnSaveSlot.Enabled = true;

                //Main Stats
                tbGems.Enabled = true;
                tbGems.Text = saveFile.gem.ToString();

                tbLevel.Enabled = true;
                tbLevel.Text = saveFile.level.ToString();

                tbExp.Enabled = true;
                tbExp.Text = saveFile.exp.ToString();

                //Inventory
                //Items
                checkHerb.Enabled = true;
                checkHerb.Checked = saveFile.herbe == 1 ? true : false;

                //Check the potion value to see if there is a potion and what type it is
                checkPotion.Enabled = true;
                switch (saveFile.boule)
                {
                    case 1:
                        checkPotion.Checked = true;
                        panelPotions.Enabled = true;
                        radioSmallPotion.Checked = true;
                        break;
                    case 2:
                        checkPotion.Checked = true;
                        panelPotions.Enabled = true;
                        radioBigPotion.Checked = true;
                        break;
                    case 3:
                        checkPotion.Checked = true;
                        panelPotions.Enabled = true;
                        radioElixir.Checked = true;
                        break;
                    default:
                        checkPotion.Checked = false;
                        panelPotions.Enabled = false;
                        radioSmallPotion.Checked = false;
                        radioBigPotion.Checked = false;
                        radioElixir.Checked = false;
                        break;
                }

                checkLumenKey.Enabled = true;
                checkLumenKey.Checked = saveFile.kingkey == 1 ? true : false;

                checkAngelFeather.Enabled = true;
                checkAngelFeather.Checked = saveFile.plume == 1 ? true : false;

                checkSmallVial.Enabled = true;
                checkSmallVial.Checked = saveFile.map == 12 ? true : false;

                checkUniversalBook.Enabled = true;
                checkUniversalBook.Checked = saveFile.livre == 1 ? true : false;

                checkLyre.Enabled = true;
                checkLyre.Checked = saveFile.harpmax == 1 ? true : false;

                //Essences - Done first to ensure check box events fire properly for rings
                checkEarthEssence.Enabled = true;
                checkEarthEssence.Checked = saveFile.ring1 == 2 ? true : false;

                checkWaterEssence.Enabled = true;
                checkWaterEssence.Checked = saveFile.ring2 == 2 ? true : false;

                checkFireEssence.Enabled = true;
                checkFireEssence.Checked = saveFile.ring3 == 2 ? true : false;

                checkAirEssence.Enabled = true;
                checkAirEssence.Checked = saveFile.ring4 == 2 ? true : false;

                //Rings - "ring#" is 2.
                checkEarthRing.Enabled = true;
                checkEarthRing.Checked = saveFile.ring1 >= 1 ? true : false;

                checkWaterRing.Enabled = true;
                checkWaterRing.Checked = saveFile.ring2 >= 1 ? true : false;

                checkFireRing.Enabled = true;
                checkFireRing.Checked = saveFile.ring3 >= 1 ? true : false;

                checkAirRing.Enabled = true;
                checkAirRing.Checked = saveFile.ring4 >= 1 ? true : false;

                //Light Skills - Firefly scroll and light essence are tied together
                checkFireflyScroll.Enabled = true;
                checkFireflyScroll.Checked = saveFile.lightskill == 2 ? true : false;

                checkLightEssence.Enabled = true;
                checkLightEssence.Checked = saveFile.lightskill >= 1 ? true : false;

                //Scrolls - 1, 2, or 3 for each main skill. Can't seem to have laters without previous.
                //Be sure to clear the scrolls first in order to make sure we are setting them correctly on change event
                checkMoleScroll.Enabled = true;
                checkFleaScroll.Enabled = true;
                checkWoodpeckerScroll.Enabled = true;

                checkWoodpeckerScroll.Checked = false;
                checkFleaScroll.Checked = false;
                checkMoleScroll.Checked = false;

                checkWoodpeckerScroll.Checked = saveFile.scroll >= 3 ? true : false;
                checkFleaScroll.Checked = saveFile.scroll >= 2 ? true : false;
                checkMoleScroll.Checked = saveFile.scroll >= 1 ? true : false;

                //Weapons - "epee#" is 1. epee 1 is always available and epee7 is 1 or 2 based on blessed status
                //checkIronSword.Enabled = false;
                //checkIronSword.Checked = saveFile.epee1 == 1 ? true : true;

                checkSteelSword.Enabled = true;
                checkSteelSword.Checked = saveFile.epee2 == 1 ? true : false;

                checkBubbleSword.Enabled = true;
                checkBubbleSword.Checked = saveFile.epee3 == 1 ? true : false;

                checkMasamune.Enabled = true;
                checkMasamune.Checked = saveFile.epee4 == 1 ? true : false;

                checkDragonSword.Enabled = true;
                checkDragonSword.Checked = saveFile.epee5 == 1 ? true : false;

                checkLightningSword.Enabled = true;
                checkLightningSword.Checked = saveFile.epee6 == 1 ? true : false;

                //Control the visibility of the correct image
                btnSelectSwordLeft.Visible = true;
                btnSelectSwordRight.Visible = true;
                switch (saveFile.epee7)
                {
                    case 0:
                        picAggelosSword.Visible = false;
                        picSacredSword.Visible = false;
                        btnSelectSwordLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picAggelosSword.Visible = true;
                        picSacredSword.Visible = false;
                        break;
                    case 2:
                        picAggelosSword.Visible = false;
                        picSacredSword.Visible = true;
                        btnSelectSwordRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                //Armor - "armure#" is 1. armure 1 is always available and armure7 is 1 or 2 based on blessed status
                //checkIronArmor.Enabled = false;
                //checkIronArmor.Checked = saveFile.armure1 == 1 ? true : true;

                checkSteelArmor.Enabled = true;
                checkSteelArmor.Checked = saveFile.armure2 == 1 ? true : false;

                checkCoralArmor.Enabled = true;
                checkCoralArmor.Checked = saveFile.armure3 == 1 ? true : false;

                checkSamuraiArmor.Enabled = true;
                checkSamuraiArmor.Checked = saveFile.armure4 == 1 ? true : false;

                checkDragonArmor.Enabled = true;
                checkDragonArmor.Checked = saveFile.armure5 == 1 ? true : false;

                checkLightningArmor.Enabled = true;
                checkLightningArmor.Checked = saveFile.armure6 == 1 ? true : false;

                //Control the visibility of the correct image
                btnSelectArmorLeft.Visible = true;
                btnSelectArmorRight.Visible = true;
                switch (saveFile.armure7)
                {
                    case 0:
                        picAggelosArmor.Visible = false;
                        picSacredArmor.Visible = false;
                        btnSelectArmorLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picAggelosArmor.Visible = true;
                        picSacredArmor.Visible = false;
                        break;
                    case 2:
                        picAggelosArmor.Visible = false;
                        picSacredArmor.Visible = true;
                        btnSelectArmorRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }


                //Map

                //Split States

            }
            //Blank the values and disable the controls since no file is loaded
            else
            {
                btnSaveSlot.Enabled = false;

                tbGems.Enabled = false;
                tbGems.Text = "";

                tbLevel.Enabled = false;
                tbLevel.Text = "";

                tbExp.Enabled = false;
                tbExp.Text = "";
            }
        }

        /************************************************************
        * btnInstallPath_Click
        * 
        * This function is called when the user clicks the installation path
        * button. It prompts the user to select a new folder where save files
        * are located at for the game.
        ************************************************************/
        private void btnInstallPath_Click(object sender, EventArgs e)
        {
            //Create a new folder browser dialog reference
            using (var folderDialog = new FolderBrowserDialog())
            {
                //Call the dialog box
                DialogResult result = folderDialog.ShowDialog();

                //Check that OK button was clicked and that the selected path is not empty
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    //Set the new installation path based on the folder chosen
                    saveFile.ModifyInstallationPath(folderDialog.SelectedPath);

                    //Update the text box to show the save file path
                    tbInstallPath.Text = saveFile.InstallationPath;
                }
            }
        }

        /************************************************************
         * LoadFile
         * 
         * This function takes a file name and attempts to open and read
         * the save contents.
         * It returns a string array representation of the save data.
         ************************************************************/
        public string[] LoadFile(string fileName)
        {
            //Attempt to read the file
            try
            {
                //Store each line of the file in a string array
                string[] saveData = new string[196];
                saveData = File.ReadAllLines(fileName);

                //Update the file name with the file being read
                presetFile = fileName;

                //Return the array
                return saveData;
            }
            catch (SecurityException ex)
            {
                //Show any errors
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");

                //Reset the text box to show no file selected
                presetFile = "";

                //Return empty array if there was an error
                return new string[0];
            }
        }
        
        /************************************************************
        * btnLoadFile_Click
        * 
        * This function is called when the user clicks the load button on the form.
        * The file chosen is expected to be the save file contents.
        ************************************************************/
        private async void btnLoadFile_Click(object sender, EventArgs e)
        {
            //Call the dialog box and check that the OK button was clicked
            if (openDiag.ShowDialog() == DialogResult.OK)
            {
                //Load the file and store the results as a string array
                string[] saveData = LoadFile(openDiag.FileName);

                //Check if there was a load issue
                if (saveData.Length != 0)
                {
                    //Update the values based on the save data
                    if (saveFile.UpdateValuesFromSave(saveData))
                    {
                        //Be sure to refresh our app based on the loaded values
                        RefreshUI();

                        lblStatus.Text = "Save loaded successfully.";
                    }
                    else
                    {
                        lblStatus.Text = "Errors during update...";
                    }
                }
                else
                {
                    lblStatus.Text = "Failed to load file...";
                }

                //Leave status message on screen for 5 seconds then remove
                await Task.Delay(5000);
                lblStatus.Text = "";
            }
        }

        /************************************************************
        * btnSaveFile_Click
        * 
        * This function is called when the user clicks the save preset button 
        * on the form. The file chosen is updated with the values from the form.
        ************************************************************/
        private async void btnSaveFile_Click(object sender, EventArgs e)
        {
            //Declare a new save dialog
            SaveFileDialog savePresetDialog = new SaveFileDialog();

            //Set the default extension
            savePresetDialog.AddExtension = true;
            savePresetDialog.Filter = "Save Files(*.ini)|*.ini";
            savePresetDialog.DefaultExt = ".ini";

            //Call the dialog box and check that the OK button was clicked
            if (savePresetDialog.ShowDialog() == DialogResult.OK)
            {
                //Save the changes to the preset file
                if (saveFile.SaveChanges(savePresetDialog.FileName))
                {
                    lblStatus.Text = "Save changes completed.";
                }
                else
                {
                    lblStatus.Text = "Errors during last save...";
                }

                //Leave status message on screen for 5 seconds then remove
                await Task.Delay(5000);
                lblStatus.Text = "";
            }
        }

        /************************************************************
        * btnSaveSlot_Click
        * 
        * This function is called when the user clicks the save button on the form.
        * The file chosen is updated with the values from the form.
        ************************************************************/
        private async void btnSaveSlot_Click(object sender, EventArgs e)
        {
            //Save the changes based on the installation path and the slot number
            string savePath = saveFile.InstallationPath + "\\" + saveFile.slotNumber + ".ini";

            if (saveFile.SaveChanges(savePath))
            {
                lblStatus.Text = "Save changes completed.";
            }
            else
            {
                lblStatus.Text = "Errors during last save...";
            }

            await Task.Delay(3000);
            lblStatus.Text = "";
        }

        /************************************************************
        * btnLoadDefault_Click
        * 
        * This function is called when the user clicks the load defaul button on the form.
        * The UI is set to default save file data.
        ************************************************************/
        private async void btnLoadDefault_Click(object sender, EventArgs e)
        {
            //Set the default properties
            saveFile.SetDefault();

            //Refresh the UI
            RefreshUI();

            lblStatus.Text = "Defaults loaded.";

            await Task.Delay(3000);
            lblStatus.Text = "";
        }

        /************************************************************
        * STATE CHANGES
        * 
        * The following functions are called for typical stat changes.
        ************************************************************/
        private void tbGems_ValueChanged(object sender, EventArgs e)
        {
            if(tbGems.Value > tbGems.Maximum)
            {
                tbGems.Value = tbGems.Maximum;
            }
            else if (tbGems.Value < tbGems.Minimum)
            {
                tbGems.Value = tbGems.Minimum;
            }

            saveFile.gem = ((int)tbGems.Value);
        }

        private void tbLevel_ValueChanged(object sender, EventArgs e)
        {
            if (tbLevel.Value > tbLevel.Maximum)
            {
                tbLevel.Value = tbLevel.Maximum;
            }
            else if (tbLevel.Value < tbLevel.Minimum)
            {
                tbLevel.Value = tbLevel.Minimum;
            }

            saveFile.level = ((int)tbLevel.Value);
        }

        private void tbExp_ValueChanged(object sender, EventArgs e)
        {
            if (tbExp.Value > tbExp.Maximum)
            {
                tbExp.Value = tbExp.Maximum;
            }
            else if (tbExp.Value < tbExp.Minimum)
            {
                tbExp.Value = tbExp.Minimum;
            }

            saveFile.exp = ((int)tbExp.Value);
        }

        private void checkHerb_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.herbe = checkHerb.Checked == true ? 1 : 0;
        }

        /************************************************************
        * INVENTORY CHANGES
        * 
        * The following functions are called for inventory changes.
        ************************************************************/
        private void checkPotion_CheckedChanged(object sender, EventArgs e)
        {
            //Make sure to disable the radio buttons if we do not want a potion
            if (checkPotion.Checked)
            {
                panelPotions.Enabled = true;
            }
            else
            {
                panelPotions.Enabled = false;
                radioSmallPotion.Checked = false;
                radioBigPotion.Checked = false;
                radioElixir.Checked = false;
                saveFile.boule = 0;
            }
        }

        private void radioSmallPotion_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSmallPotion.Checked == true)
            {
                saveFile.boule = 1;
            }
        }

        private void radioBigPotion_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBigPotion.Checked == true)
            {
                saveFile.boule = 2;
            }
        }

        private void radioElixir_CheckedChanged(object sender, EventArgs e)
        {
            if (radioElixir.Checked == true)
            {
                saveFile.boule = 3;
            }
        }

        private void checkLumenKey_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.kingkey = checkLumenKey.Checked == true ? 1 : 0;
        }

        private void checkAngelFeather_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.plume = checkAngelFeather.Checked == true ? 1 : 0;
        }

        private void checkSmallVial_CheckedChanged(object sender, EventArgs e)
        {
            //This value may be dependent on trade sequence state.
            //16 is "done and no vial"
            //11 is "done and empty small vial"
            //12 is "done and full small vial"
            saveFile.map = checkSmallVial.Checked == true ? 12 : 16; 
        }

        private void checkUniversalBook_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.livre = checkUniversalBook.Checked == true ? 1 : 0;
        }

        private void checkLyre_CheckedChanged(object sender, EventArgs e)
        {
            //Make sure to disable the radio buttons if we do not want a potion
            if (checkLyre.Checked)
            {
                //panelPotions.Enabled = true;
                saveFile.harpefil = 1;
                saveFile.harpechassis = 1;
                saveFile.harpmax = 1;
            }
            else
            {
                //panelPotions.Enabled = false;
                //radioLyreString.Checked = false;
                //radioLyreChassis.Checked = false;
                //radioElixir.Checked = false;
                saveFile.harpefil = 0;
                saveFile.harpechassis = 0;
                saveFile.harpmax = 0;
            }
        }

        /************************************************************
        * RING CHANGES
        * 
        * The following functions are called for ring changes.
        ************************************************************/
        private void checkEarthRing_CheckedChanged(object sender, EventArgs e)
        {
            //Only change to 1 if checked and we didn't get trigger from the essence checkbox
            if (checkEarthRing.Checked && !checkEarthEssence.Checked)
            {
                saveFile.ring1 = 1;
            }
            //If we are not checked be sure we dont have essence checked and set to 0
            else if (!checkEarthRing.Checked)
            {
                checkEarthEssence.Checked = false;
                saveFile.ring1 = 0;
            }
            //Otherwise do nothing because this event was triggered from another function that will handle the variable.
        }

        private void checkWaterRing_CheckedChanged(object sender, EventArgs e)
        {
            //Only change to 1 if checked and we didn't get trigger from the essence checkbox
            if (checkWaterRing.Checked && !checkWaterEssence.Checked)
            {
                saveFile.ring2 = 1;
            }
            //If we are not checked be sure we dont have essence checked and set to 0
            else if (!checkWaterRing.Checked)
            {
                checkWaterEssence.Checked = false;
                saveFile.ring2 = 0;
            }
            //Otherwise do nothing because this event was triggered from another function that will handle the variable.
        }

        private void checkFireRing_CheckedChanged(object sender, EventArgs e)
        {
            //Only change to 1 if checked and we didn't get trigger from the essence checkbox
            if (checkFireRing.Checked && !checkFireEssence.Checked)
            {
                saveFile.ring3 = 1;
            }
            //If we are not checked be sure we dont have essence checked and set to 0
            else if (!checkFireRing.Checked)
            {
                checkFireEssence.Checked = false;
                saveFile.ring3 = 0;
            }
            //Otherwise do nothing because this event was triggered from another function that will handle the variable.
        }

        private void checkAirRing_CheckedChanged(object sender, EventArgs e)
        {
            //Only change to 1 if checked and we didn't get trigger from the essence checkbox
            if (checkAirRing.Checked && !checkAirEssence.Checked)
            {
                saveFile.ring4 = 1;
            }
            //If we are not checked be sure we dont have essence checked and set to 0
            else if (!checkAirRing.Checked)
            {
                checkAirEssence.Checked = false;
                saveFile.ring4 = 0;
            }
            //Otherwise do nothing because this event was triggered from another function that will handle the variable.
        }

        /************************************************************
        * SCROLL CHANGES
        * 
        * The following functions are called for scroll changes.
        ************************************************************/
        private void checkMoleScroll_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("MOLE");
            //Since you can't have later scrolls without earlier scrolls, check/uncheck all that apply if unchecked here
            if (checkMoleScroll.Checked)
            {
                saveFile.scroll = 1;
            }
            else
            {
                saveFile.scroll = 0;
                checkFleaScroll.Checked = false;
                checkWoodpeckerScroll.Checked = false;
            }

        }

        private void checkFleaScroll_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("FLEA");
            //Since you can't have later scrolls without earlier scrolls, check/uncheck all that apply if unchecked here
            if (checkFleaScroll.Checked)
            {
                saveFile.scroll = 2;
                checkMoleScroll.Checked = true;
            }
            else
            {
                saveFile.scroll = 1;
                checkWoodpeckerScroll.Checked = false;
            }
        }

        private void checkWoodpeckerScroll_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("WOODPECKER");
            //Since you can't have later scrolls without earlier scrolls, check/uncheck all that apply if unchecked here
            if (checkWoodpeckerScroll.Checked)
            {
                saveFile.scroll = 3;
                checkMoleScroll.Checked = true;
                checkFleaScroll.Checked = true;
            }
            else
            {
                saveFile.scroll = 2;
            }
        }

        //Can't have the firefly scroll without the light essence
        private void checkFireflyScroll_CheckedChanged(object sender, EventArgs e)
        {
            //If checked always make sure light essence is checked and set to 2
            if (checkFireflyScroll.Checked)
            {
                checkLightEssence.Checked = true;
                saveFile.lightskill = 2;
            }
            //Otherwise if we have just gotten unchecked while the light essence was checked we leave it at 1
            else if (!checkFireflyScroll.Checked && checkLightEssence.Checked)
            {
                saveFile.lightskill = 1;
            }
            //Otherwise this was triggered from unchecking the light essence and we don't want to do anything as it will handle variables.
        }

        /************************************************************
        * ESSENCE CHANGES
        * 
        * The following functions are called for essence changes.
        * There is no way to give yourself the essence and not the ring.
        ************************************************************/
        private void checkEarthEssence_CheckedChanged(object sender, EventArgs e)
        {
            //If checked always make sure ring is checked and set to 2
            if (checkEarthEssence.Checked)
            {
                checkEarthRing.Checked = true;
                saveFile.ring1 = 2;
            }
            //Otherwise if we have just gotten unchecked while the ring was checked we leave it at 1
            else if (!checkEarthEssence.Checked && checkEarthRing.Checked)
            {
                saveFile.ring1 = 1;
            }
            //Otherwise this was triggered from unchecking the ring and we don't want to do anything as it will handle variables.
        }

        private void checkWaterEssence_CheckedChanged(object sender, EventArgs e)
        {
            //If checked always make sure ring is checked and set to 2
            if (checkWaterEssence.Checked)
            {
                checkWaterRing.Checked = true;
                saveFile.ring2 = 2;
            }
            //Otherwise if we have just gotten unchecked while the ring was checked we leave it at 1
            else if (!checkWaterEssence.Checked && checkWaterRing.Checked)
            {
                saveFile.ring2 = 1;
            }
            //Otherwise this was triggered from unchecking the ring and we don't want to do anything as it will handle variables.
        }

        private void checkFireEssence_CheckedChanged(object sender, EventArgs e)
        {
            //If checked always make sure ring is checked and set to 2
            if (checkFireEssence.Checked)
            {
                checkFireRing.Checked = true;
                saveFile.ring3 = 2;
            }
            //Otherwise if we have just gotten unchecked while the ring was checked we leave it at 1
            else if (!checkFireEssence.Checked && checkFireRing.Checked)
            {
                saveFile.ring3 = 1;
            }
            //Otherwise this was triggered from unchecking the ring and we don't want to do anything as it will handle variables.
        }

        private void checkAirEssence_CheckedChanged(object sender, EventArgs e)
        {
            //If checked always make sure ring is checked and set to 2
            if (checkAirEssence.Checked)
            {
                checkAirRing.Checked = true;
                saveFile.ring4 = 2;
            }
            //Otherwise if we have just gotten unchecked while the ring was checked we leave it at 1
            else if (!checkAirEssence.Checked && checkAirRing.Checked)
            {
                saveFile.ring4 = 1;
            }
            //Otherwise this was triggered from unchecking the ring and we don't want to do anything as it will handle variables.
        }

        private void checkLightEssence_CheckedChanged(object sender, EventArgs e)
        {
            //Only change to 1 if checked and we didn't get trigger from the firefly scroll checkbox
            if (checkLightEssence.Checked && !checkFireflyScroll.Checked)
            {
                saveFile.lightskill = 1;
            }
            //If we are not checked be sure we dont have firefly checked and set to 0
            else if (!checkLightEssence.Checked)
            {
                checkFireflyScroll.Checked = false;
                saveFile.lightskill = 0;
            }
            //Otherwise do nothing because this event was triggered from another function that will handle the variable.
        }

        /************************************************************
        * WEAPON CHANGES
        * 
        * The following functions are called for weapon changes.
        ************************************************************/
        private void checkSteelSword_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.epee2 = checkSteelSword.Checked == true ? 1 : 0;
        }

        private void checkBubbleSword_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.epee3 = checkBubbleSword.Checked == true ? 1 : 0;
        }

        private void checkMasamune_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.epee4 = checkMasamune.Checked == true ? 1 : 0;
        }

        private void checkDragonSword_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.epee5 = checkDragonSword.Checked == true ? 1 : 0;
        }

        private void checkLightningSword_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.epee6 = checkLightningSword.Checked == true ? 1 : 0;
        }

        private void btnSelectSwordLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for sword slot as long as we aren't at min
            if (saveFile.epee7 > 0)
            {
                saveFile.epee7 -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectSwordRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.epee7 == 0)
            {
                btnSelectSwordLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.epee7)
            {
                case 0:
                    picAggelosSword.Visible = false;
                    picSacredSword.Visible = false;
                    break;
                case 1:
                    picAggelosSword.Visible = true;
                    picSacredSword.Visible = false;
                    break;
                case 2:
                    picAggelosSword.Visible = false;
                    picSacredSword.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectSwordRight_Click(object sender, EventArgs e)
        {
            //Increase the value for sword slot as long as we aren't at max
            if (saveFile.epee7 < 2)
            {
                saveFile.epee7 += 1;
                //Make sure to re-enable the left select now that we've decreased
                btnSelectSwordLeft.Visible = true;
            }
            
            //If we ever become the max, disable the control
            if (saveFile.epee7 == 2)
            {
                btnSelectSwordRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.epee7)
            {
                case 0:
                    picAggelosSword.Visible = false;
                    picSacredSword.Visible = false;
                    break;
                case 1:
                    picAggelosSword.Visible = true;
                    picSacredSword.Visible = false;
                    break;
                case 2:
                    picAggelosSword.Visible = false;
                    picSacredSword.Visible = true;
                    break;
                default:
                    break;
            }
        }

        /************************************************************
        * ARMOR CHANGES
        * 
        * The following functions are called for armor changes.
        ************************************************************/
        private void checkSteelArmor_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.armure2 = checkSteelArmor.Checked == true ? 1 : 0;
        }

        private void checkCoralArmor_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.armure3 = checkCoralArmor.Checked == true ? 1 : 0;
        }

        private void checkSamuraiArmor_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.armure4 = checkSamuraiArmor.Checked == true ? 1 : 0;
        }

        private void checkDragonArmor_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.armure5 = checkDragonArmor.Checked == true ? 1 : 0;
        }

        private void checkLightningArmor_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.armure6 = checkLightningArmor.Checked == true ? 1 : 0;
        }

        private void btnSelectArmorLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for armor slot as long as we aren't at min
            if (saveFile.armure7 > 0)
            {
                saveFile.armure7 -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectArmorRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.armure7 == 0)
            {
                btnSelectArmorLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.armure7)
            {
                case 0:
                    picAggelosArmor.Visible = false;
                    picSacredArmor.Visible = false;
                    break;
                case 1:
                    picAggelosArmor.Visible = true;
                    picSacredArmor.Visible = false;
                    break;
                case 2:
                    picAggelosArmor.Visible = false;
                    picSacredArmor.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectArmorRight_Click(object sender, EventArgs e)
        {
            //Increase the value for sword slot as long as we aren't at max
            if (saveFile.armure7 < 2)
            {
                saveFile.armure7 += 1;
                //Make sure to re-enable the left select now that we've decreased
                btnSelectArmorLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.armure7 == 2)
            {
                btnSelectArmorRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.armure7)
            {
                case 0:
                    picAggelosArmor.Visible = false;
                    picSacredArmor.Visible = false;
                    break;
                case 1:
                    picAggelosArmor.Visible = true;
                    picSacredArmor.Visible = false;
                    break;
                case 2:
                    picAggelosArmor.Visible = false;
                    picSacredArmor.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void radioSaveSlot1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSaveSlot1.Checked == true)
            {
                saveFile.slotNumber = "sauvegarde1";
            }
        }

        private void radioSaveSlot2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSaveSlot2.Checked == true)
            {
                saveFile.slotNumber = "sauvegarde2";
            }
        }

        private void radioSaveSlot3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSaveSlot3.Checked == true)
            {
                saveFile.slotNumber = "sauvegarde3";
            }
        }
    }

    /************************************************************
     * Save
     * 
     * This class represents all aspects of the save file that we
     * want to store and modify. Each instance of this class
     * represents an individual save file.
     ************************************************************/
    public class Save
    {
        //File Information
        public string InstallationPath { get; set; }
        public bool FileLoaded { get; set; }
        
        //Slot Number
        public string slotNumber { get; set; }

        //Stats
        public Int32 gem { get; set; }
        public int coeur { get; set; }
        public int magie { get; set; }
        public int level { get; set; }
        public int hard { get; set; }
        public int heure { get; set; }
        public int minute { get; set; }
        public Int32 seconde { get; set; }
        public int death { get; set; }
        public Int32 exp { get; set; }
        public int clé { get; set; }
        public int pow { get; set; }
        public int def { get; set; }

        //Skills
        public int scroll { get; set; }
        public int ring1 { get; set; }
        public int ring2 { get; set; }
        public int ring3 { get; set; }
        public int ring4 { get; set; }

        //Map you start in, x/y coords, and percent complete
        public int scene { get; set; }
        public Int32 x { get; set; }
        public Int32 y { get; set; }
        public int pourcent { get; set; }

        //Healing
        public int water { get; set; }
        public int herbe { get; set; }

        //?????
        public int histoire { get; set; }

        //Tutorials?
        public int tutocoffre { get; set; }
        public int tutosave { get; set; }

        public int lightskill { get; set; }

        //Save Point Unlocks
        public int savep1 { get; set; }
        public int savep2 { get; set; }
        public int savep3 { get; set; }
        public int savep4 { get; set; }
        public int savep5 { get; set; }
        public int savep6 { get; set; }
        public int savep7 { get; set; }
        public int savep8 { get; set; }
        public int savep9 { get; set; }
        public int savep10 { get; set; }
        public int savep11 { get; set; }
        public int savep12 { get; set; }
        public int savep13 { get; set; }
        public int savep14 { get; set; }
        public int savep15 { get; set; }

        //Something to do with Armor and Weapon
        public int numeroarmure { get; set; }
        public int numeroepee { get; set; }

        //Armors
        public int armure1 { get; set; }
        public int armure2 { get; set; }
        public int armure3 { get; set; }
        public int armure4 { get; set; }
        public int armure5 { get; set; }
        public int armure6 { get; set; }
        public int armure7 { get; set; }

        //Weapons
        public int epee1 { get; set; }
        public int epee2 { get; set; }
        public int epee3 { get; set; }
        public int epee4 { get; set; }
        public int epee5 { get; set; }
        public int epee6 { get; set; }
        public int epee7 { get; set; }

        //Regions?
        public int region1 { get; set; }
        public int region2 { get; set; }
        public int region3 { get; set; }            //Went from "4" to "6" after opening two chests in sun crest room, one being empty vial
        public int region4 { get; set; }
        public int region5 { get; set; }
        public int region6 { get; set; }
        public int region7 { get; set; }
        public int region8 { get; set; }
        public int region9 { get; set; }

        //Story Items
        public int kingkey { get; set; }
        public int livre { get; set; }
        public int plume { get; set; }
        public int harpefil { get; set; }           //String
        public int harpechassis { get; set; }       //Body
        public int harpmax { get; set; }            //Repaired

        //Map appears to be related to the armor quest. Getting the bananas sets it to 1.
        //The value is changed as you progress through the quest.
        public int map { get; set; }                //1 = Bananas
                                                    //2 = Necklace
                                                    //3 = Broken Crown
                                                    //4 = Princess' Tiara
                                                    //5 = Moon Symbol
                                                    //6 = Shell
                                                    //7 = Star Symbol
                                                    //8 = Crystal Ball
                                                    //9 = King Bartelele's Scepter
                                                    //10 = Sun Symbol
                                                    //11 = Empty Vial
                                                    //12 = Full Vial
                                                    //13 = 
                                                    //14 = 
                                                    //15 = Used Star Symbol or Used Moon Symbol and Scepter is not in collection in this state
                                                    //16 = Shows scepter in collection after using Sun Symbol

        //Potions State
        public int boule { get; set; }

        //??????
        public int coffre20xp { get; set; }
        public int coffre50xp { get; set; }
        public int groscoffre { get; set; }
        public int groscoffreelement { get; set; }
        public int porte1 { get; set; }
        public int porte2 { get; set; }
        public int coffre_herbe1 { get; set; }
        public int coffre_contener1 { get; set; }
        public int coffre_power_up { get; set; }
        public int coffre_defense_up { get; set; }
        public int coffre_50xp2 { get; set; }
        public int coffre_magie_up { get; set; }
        public int clé1 { get; set; }
        public int power_mont { get; set; }
        public int contener_mont { get; set; }
        public int herb_mont { get; set; }
        public int chest_A1 { get; set; }
        public int chest_B1 { get; set; }
        public int chest_C1 { get; set; }
        public int chest_D1 { get; set; }
        public int chest_E1 { get; set; }
        public int chest_F1 { get; set; }
        public int chest_G1 { get; set; }
        public int chest_H1 { get; set; }
        public int chest_I1 { get; set; }
        public int chest_J1 { get; set; }
        public int chest_K1 { get; set; }
        public int chest_L1 { get; set; }
        public int chest_M1 { get; set; }
        public int chest_N1 { get; set; }
        public int chest_O1 { get; set; }
        public int chest_P1 { get; set; }
        public int chest_Q1 { get; set; }
        public int chest_R1 { get; set; }
        public int chest_S1 { get; set; }
        public int chest_T1 { get; set; }
        public int chest_U1 { get; set; }
        public int chest_V1 { get; set; }
        public int chest_W1 { get; set; }
        public int chest_X1 { get; set; }
        public int chest_Y1 { get; set; }
        public int chest_Z1 { get; set; }
        public int chest_30 { get; set; }
        public int chest_31 { get; set; }
        public int chest_32 { get; set; }
        public int chest_33 { get; set; }
        public int chest_34 { get; set; }
        public int chest_35 { get; set; }
        public int chest_36 { get; set; }
        public int chest_37 { get; set; }
        public int chest_38 { get; set; }
        public int chest_39 { get; set; }
        public int chest_40 { get; set; }
        public int chest_41 { get; set; }
        public int chest_42 { get; set; }
        public int chest_43 { get; set; }
        public int chest_44 { get; set; }
        public int chest_45 { get; set; }
        public int chest_46 { get; set; }
        public int chest_47 { get; set; }
        public int chest_48 { get; set; }
        public int chest_49 { get; set; }
        public int chest_50 { get; set; }
        public int chest_51 { get; set; }
        public int chest_52 { get; set; }           //elixir chest in sun crest room
        public int chest_53 { get; set; }           //small vial chest in sun crest room (does not give vile if set to open)
        public int chest_54 { get; set; }
        public int chest_55 { get; set; }
        public int chest_56 { get; set; }
        public int chest_57 { get; set; }
        public int chest_58 { get; set; }
        public int chest_59 { get; set; }
        public int chest_60 { get; set; }
        public int chest_61 { get; set; }
        public int chest_62 { get; set; }
        public int chest_63 { get; set; }
        public int chest_64 { get; set; }
        public int chest_65 { get; set; }
        public int chest_66 { get; set; }
        public int chest_67 { get; set; }
        public int chest_68 { get; set; }
        public int chest_69 { get; set; }
        public int chest_70 { get; set; }
        public int chest_71 { get; set; }
        public int chest_72 { get; set; }
        public int chest_73 { get; set; }
        public int chest_74 { get; set; }
        public int chest_75 { get; set; }
        public int chest_76 { get; set; }
        public int chest_77 { get; set; }
        public int chest_78 { get; set; }
        public int chest_79 { get; set; }
        public int chest_80 { get; set; }
        public int chest_81 { get; set; }
        public int chest_82 { get; set; }
        public int chest_83 { get; set; }
        public int chest_84 { get; set; }
        public int chest_85 { get; set; }
        public int chest_86 { get; set; }
        public int chest_87 { get; set; }
        public int chest_88 { get; set; }
        public int chest_89 { get; set; }
        public int chest_90 { get; set; }
        public int chest_91 { get; set; }
        public int chest_94 { get; set; }
        public int chest_95 { get; set; }
        public int chest_96 { get; set; }
        public int chest_97 { get; set; }
        public int chest_98 { get; set; }
        public int chest_99 { get; set; }
        public int chest_100 { get; set; }
        public int chest_101 { get; set; }
        public int chest_103 { get; set; }
        public int chest_104 { get; set; }
        public int chest_105 { get; set; }
        public int chest_106 { get; set; }
        public int chest_108 { get; set; }
        public int chest_109 { get; set; }
        public int chest_110 { get; set; }


        /************************************************************
         * Constructor
         * 
         * This ensures if a new save file was generated that the properties
         * are given their default working values for a new game.
         ************************************************************/
        public Save()
        {
            //Get the installation path for the save
            GetInstallationPath();

            //Mark that no file is loaded
            FileLoaded = false;

            //Set the default values for each item to an empty loadable save
            SetDefault();
        }

        /************************************************************
         * GetInstallationPath
         * 
         * This function gets the installation path of Aggelos.
         * It currently assumes Steam as the default application to use.
         ************************************************************/
        private void GetInstallationPath()
        {
            // Only call if we haven't already found the installation path
            if (InstallationPath == "" || InstallationPath == null)
            {
                /*if (RegQueryStringValue(HKLM64,
                                            'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 475150',
                                            'InstallLocation', 
                                            InstallationPath))*/

                try
                {
                    //Search the registry for a steam installation path key for Aggelos (app ID 717310)
                    //NOTE: Target Platform must be set to x64 in order to successfully read this registry key.
                    using (var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 717310", false))
                    {
                        //If we found a key at this location we know Aggelos is installed through Steam
                        if (key != null)
                        {
                            InstallationPath = key.GetValue("InstallLocation").ToString();
                            Console.WriteLine("Detected Steam installation: " + InstallationPath);
                        }
                        else
                        {
                            InstallationPath = "C:\\Steam\\steamapps\\common\\Aggelos";
                            Console.WriteLine("No installation detected, using the default path: " + InstallationPath);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error obtaining installation path: " + e.Message);
                }
            }
        }

        /************************************************************
         * GetInstallationPath
         * 
         * This function is called to modify the installation path of Aggelos.
         ************************************************************/
        public void ModifyInstallationPath(string folderName)
        {
            InstallationPath = folderName;
        }

        /************************************************************
         * UpdateValuesFromSave
         * 
         * This function reads the save contents provided from a string array
         * and stores each value in the appropriate property variable.
         * It returns true if success and false if failed.
         ************************************************************/
        public bool UpdateValuesFromSave(string[] saveData)
        {
            try
            {
                //Check if the file name is formatted properly
                if (!saveData[0].Contains("sauvegarde") || saveData.Length != 196)
                {
                    throw new Exception("This is not a valid save file.");
                }

                //For now each line is pased individually for simplicity
                slotNumber = saveData[0].Substring(1, saveData[0].Length-2);
                gem = Int32.Parse(saveData[1].Substring(saveData[1].IndexOf("=") + 1));
                coeur = Int16.Parse(saveData[2].Substring(saveData[2].IndexOf("=") + 1));
                magie = Int16.Parse(saveData[3].Substring(saveData[2].IndexOf("=") + 1));
                level = Int16.Parse(saveData[4].Substring(saveData[4].IndexOf("=") + 1));
                hard = Int16.Parse(saveData[5].Substring(saveData[5].IndexOf("=") + 1));
                heure = Int16.Parse(saveData[6].Substring(saveData[6].IndexOf("=") + 1));
                minute = Int16.Parse(saveData[7].Substring(saveData[7].IndexOf("=") + 1));
                seconde = Int32.Parse(saveData[8].Substring(saveData[8].IndexOf("=") + 1));
                death = Int16.Parse(saveData[9].Substring(saveData[9].IndexOf("=") + 1));
                exp = Int32.Parse(saveData[10].Substring(saveData[10].IndexOf("=") + 1));
                clé = Int16.Parse(saveData[11].Substring(saveData[11].IndexOf("=") + 1));
                pow = Int16.Parse(saveData[12].Substring(saveData[12].IndexOf("=") + 1));
                def = Int16.Parse(saveData[13].Substring(saveData[13].IndexOf("=") + 1));
                scroll = Int16.Parse(saveData[14].Substring(saveData[14].IndexOf("=") + 1));
                ring1 = Int16.Parse(saveData[15].Substring(saveData[15].IndexOf("=") + 1));
                ring2 = Int16.Parse(saveData[16].Substring(saveData[16].IndexOf("=") + 1));
                ring3 = Int16.Parse(saveData[17].Substring(saveData[17].IndexOf("=") + 1));
                ring4 = Int16.Parse(saveData[18].Substring(saveData[18].IndexOf("=") + 1));
                scene = Int16.Parse(saveData[19].Substring(saveData[19].IndexOf("=") + 1));
                x = Int32.Parse(saveData[20].Substring(saveData[20].IndexOf("=") + 1));
                y = Int32.Parse(saveData[21].Substring(saveData[21].IndexOf("=") + 1));
                pourcent = Int16.Parse(saveData[22].Substring(saveData[22].IndexOf("=") + 1));
                water = Int16.Parse(saveData[23].Substring(saveData[23].IndexOf("=") + 1));
                herbe = Int16.Parse(saveData[24].Substring(saveData[24].IndexOf("=") + 1));
                histoire = Int16.Parse(saveData[25].Substring(saveData[25].IndexOf("=") + 1));
                tutocoffre = Int16.Parse(saveData[26].Substring(saveData[26].IndexOf("=") + 1));
                tutosave = Int16.Parse(saveData[27].Substring(saveData[27].IndexOf("=") + 1));
                lightskill = Int16.Parse(saveData[28].Substring(saveData[28].IndexOf("=") + 1));
                savep1 = Int16.Parse(saveData[29].Substring(saveData[29].IndexOf("=") + 1));
                savep2 = Int16.Parse(saveData[30].Substring(saveData[30].IndexOf("=") + 1));
                savep3 = Int16.Parse(saveData[31].Substring(saveData[31].IndexOf("=") + 1));
                savep4 = Int16.Parse(saveData[32].Substring(saveData[32].IndexOf("=") + 1));
                savep5 = Int16.Parse(saveData[33].Substring(saveData[33].IndexOf("=") + 1));
                savep6 = Int16.Parse(saveData[34].Substring(saveData[34].IndexOf("=") + 1));
                savep7 = Int16.Parse(saveData[35].Substring(saveData[35].IndexOf("=") + 1));
                savep8 = Int16.Parse(saveData[36].Substring(saveData[36].IndexOf("=") + 1));
                savep9 = Int16.Parse(saveData[37].Substring(saveData[37].IndexOf("=") + 1));
                savep10 = Int16.Parse(saveData[38].Substring(saveData[38].IndexOf("=") + 1));
                savep11 = Int16.Parse(saveData[39].Substring(saveData[39].IndexOf("=") + 1));
                savep12 = Int16.Parse(saveData[40].Substring(saveData[40].IndexOf("=") + 1));
                savep13 = Int16.Parse(saveData[41].Substring(saveData[41].IndexOf("=") + 1));
                savep14 = Int16.Parse(saveData[42].Substring(saveData[42].IndexOf("=") + 1));
                savep15 = Int16.Parse(saveData[43].Substring(saveData[43].IndexOf("=") + 1));
                numeroarmure = Int16.Parse(saveData[44].Substring(saveData[44].IndexOf("=") + 1));
                numeroepee = Int16.Parse(saveData[45].Substring(saveData[45].IndexOf("=") + 1));
                armure1 = Int16.Parse(saveData[46].Substring(saveData[46].IndexOf("=") + 1));
                armure2 = Int16.Parse(saveData[47].Substring(saveData[47].IndexOf("=") + 1));
                armure3 = Int16.Parse(saveData[48].Substring(saveData[48].IndexOf("=") + 1));
                armure4 = Int16.Parse(saveData[49].Substring(saveData[49].IndexOf("=") + 1));
                armure5 = Int16.Parse(saveData[50].Substring(saveData[50].IndexOf("=") + 1));
                armure6 = Int16.Parse(saveData[51].Substring(saveData[51].IndexOf("=") + 1));
                armure7 = Int16.Parse(saveData[52].Substring(saveData[52].IndexOf("=") + 1));
                epee1 = Int16.Parse(saveData[53].Substring(saveData[53].IndexOf("=") + 1));
                epee2 = Int16.Parse(saveData[54].Substring(saveData[54].IndexOf("=") + 1));
                epee3 = Int16.Parse(saveData[55].Substring(saveData[55].IndexOf("=") + 1));
                epee4 = Int16.Parse(saveData[56].Substring(saveData[56].IndexOf("=") + 1));
                epee5 = Int16.Parse(saveData[57].Substring(saveData[57].IndexOf("=") + 1));
                epee6 = Int16.Parse(saveData[58].Substring(saveData[58].IndexOf("=") + 1));
                epee7 = Int16.Parse(saveData[59].Substring(saveData[59].IndexOf("=") + 1));
                region1 = Int16.Parse(saveData[60].Substring(saveData[60].IndexOf("=") + 1));
                region2 = Int16.Parse(saveData[61].Substring(saveData[61].IndexOf("=") + 1));
                region3 = Int16.Parse(saveData[62].Substring(saveData[62].IndexOf("=") + 1));
                region4 = Int16.Parse(saveData[63].Substring(saveData[63].IndexOf("=") + 1));
                region5 = Int16.Parse(saveData[64].Substring(saveData[64].IndexOf("=") + 1));
                region6 = Int16.Parse(saveData[65].Substring(saveData[65].IndexOf("=") + 1));
                region7 = Int16.Parse(saveData[66].Substring(saveData[66].IndexOf("=") + 1));
                region8 = Int16.Parse(saveData[67].Substring(saveData[67].IndexOf("=") + 1));
                region9 = Int16.Parse(saveData[68].Substring(saveData[68].IndexOf("=") + 1));
                kingkey = Int16.Parse(saveData[69].Substring(saveData[69].IndexOf("=") + 1));
                livre = Int16.Parse(saveData[70].Substring(saveData[70].IndexOf("=") + 1));
                plume = Int16.Parse(saveData[71].Substring(saveData[71].IndexOf("=") + 1));
                harpefil = Int16.Parse(saveData[72].Substring(saveData[72].IndexOf("=") + 1));
                harpechassis = Int16.Parse(saveData[73].Substring(saveData[73].IndexOf("=") + 1));
                harpmax = Int16.Parse(saveData[74].Substring(saveData[74].IndexOf("=") + 1));
                map = Int16.Parse(saveData[75].Substring(saveData[75].IndexOf("=") + 1));
                boule = Int16.Parse(saveData[76].Substring(saveData[76].IndexOf("=") + 1));
                coffre20xp = Int16.Parse(saveData[77].Substring(saveData[77].IndexOf("=") + 1));
                coffre50xp = Int16.Parse(saveData[78].Substring(saveData[78].IndexOf("=") + 1));
                groscoffre = Int16.Parse(saveData[79].Substring(saveData[79].IndexOf("=") + 1));
                groscoffreelement = Int16.Parse(saveData[80].Substring(saveData[80].IndexOf("=") + 1));
                porte1 = Int16.Parse(saveData[81].Substring(saveData[81].IndexOf("=") + 1));
                porte2 = Int16.Parse(saveData[82].Substring(saveData[82].IndexOf("=") + 1));
                coffre_herbe1 = Int16.Parse(saveData[83].Substring(saveData[83].IndexOf("=") + 1));
                coffre_contener1 = Int16.Parse(saveData[84].Substring(saveData[84].IndexOf("=") + 1));
                coffre_power_up = Int16.Parse(saveData[85].Substring(saveData[85].IndexOf("=") + 1));
                coffre_defense_up = Int16.Parse(saveData[86].Substring(saveData[86].IndexOf("=") + 1));
                coffre_50xp2 = Int16.Parse(saveData[87].Substring(saveData[87].IndexOf("=") + 1));
                coffre_magie_up = Int16.Parse(saveData[88].Substring(saveData[88].IndexOf("=") + 1));
                clé1 = Int16.Parse(saveData[89].Substring(saveData[89].IndexOf("=") + 1));
                power_mont = Int16.Parse(saveData[90].Substring(saveData[90].IndexOf("=") + 1));
                contener_mont = Int16.Parse(saveData[91].Substring(saveData[91].IndexOf("=") + 1));
                herb_mont = Int16.Parse(saveData[92].Substring(saveData[92].IndexOf("=") + 1));
                chest_A1 = Int16.Parse(saveData[93].Substring(saveData[93].IndexOf("=") + 1));
                chest_B1 = Int16.Parse(saveData[94].Substring(saveData[94].IndexOf("=") + 1));
                chest_C1 = Int16.Parse(saveData[95].Substring(saveData[95].IndexOf("=") + 1));
                chest_D1 = Int16.Parse(saveData[96].Substring(saveData[96].IndexOf("=") + 1));
                chest_E1 = Int16.Parse(saveData[97].Substring(saveData[97].IndexOf("=") + 1));
                chest_F1 = Int16.Parse(saveData[98].Substring(saveData[98].IndexOf("=") + 1));
                chest_G1 = Int16.Parse(saveData[99].Substring(saveData[99].IndexOf("=") + 1));
                chest_H1 = Int16.Parse(saveData[100].Substring(saveData[100].IndexOf("=") + 1));
                chest_I1 = Int16.Parse(saveData[101].Substring(saveData[101].IndexOf("=") + 1));
                chest_J1 = Int16.Parse(saveData[102].Substring(saveData[102].IndexOf("=") + 1));
                chest_K1 = Int16.Parse(saveData[103].Substring(saveData[103].IndexOf("=") + 1));
                chest_L1 = Int16.Parse(saveData[104].Substring(saveData[104].IndexOf("=") + 1));
                chest_M1 = Int16.Parse(saveData[105].Substring(saveData[105].IndexOf("=") + 1));
                chest_N1 = Int16.Parse(saveData[106].Substring(saveData[106].IndexOf("=") + 1));
                chest_O1 = Int16.Parse(saveData[107].Substring(saveData[107].IndexOf("=") + 1));
                chest_P1 = Int16.Parse(saveData[108].Substring(saveData[108].IndexOf("=") + 1));
                chest_Q1 = Int16.Parse(saveData[109].Substring(saveData[109].IndexOf("=") + 1));
                chest_R1 = Int16.Parse(saveData[110].Substring(saveData[110].IndexOf("=") + 1));
                chest_S1 = Int16.Parse(saveData[111].Substring(saveData[111].IndexOf("=") + 1));
                chest_T1 = Int16.Parse(saveData[112].Substring(saveData[112].IndexOf("=") + 1));
                chest_U1 = Int16.Parse(saveData[113].Substring(saveData[113].IndexOf("=") + 1));
                chest_V1 = Int16.Parse(saveData[114].Substring(saveData[114].IndexOf("=") + 1));
                chest_W1 = Int16.Parse(saveData[115].Substring(saveData[115].IndexOf("=") + 1));
                chest_X1 = Int16.Parse(saveData[116].Substring(saveData[116].IndexOf("=") + 1));
                chest_Y1 = Int16.Parse(saveData[117].Substring(saveData[117].IndexOf("=") + 1));
                chest_Z1 = Int16.Parse(saveData[118].Substring(saveData[118].IndexOf("=") + 1));
                chest_30 = Int16.Parse(saveData[119].Substring(saveData[119].IndexOf("=") + 1));
                chest_31 = Int16.Parse(saveData[120].Substring(saveData[120].IndexOf("=") + 1));
                chest_32 = Int16.Parse(saveData[121].Substring(saveData[121].IndexOf("=") + 1));
                chest_33 = Int16.Parse(saveData[122].Substring(saveData[122].IndexOf("=") + 1));
                chest_34 = Int16.Parse(saveData[123].Substring(saveData[123].IndexOf("=") + 1));
                chest_35 = Int16.Parse(saveData[124].Substring(saveData[124].IndexOf("=") + 1));
                chest_36 = Int16.Parse(saveData[125].Substring(saveData[125].IndexOf("=") + 1));
                chest_37 = Int16.Parse(saveData[126].Substring(saveData[126].IndexOf("=") + 1));
                chest_38 = Int16.Parse(saveData[127].Substring(saveData[127].IndexOf("=") + 1));
                chest_39 = Int16.Parse(saveData[128].Substring(saveData[128].IndexOf("=") + 1));
                chest_40 = Int16.Parse(saveData[129].Substring(saveData[129].IndexOf("=") + 1));
                chest_41 = Int16.Parse(saveData[130].Substring(saveData[130].IndexOf("=") + 1));
                chest_42 = Int16.Parse(saveData[131].Substring(saveData[131].IndexOf("=") + 1));
                chest_43 = Int16.Parse(saveData[132].Substring(saveData[132].IndexOf("=") + 1));
                chest_44 = Int16.Parse(saveData[133].Substring(saveData[133].IndexOf("=") + 1));
                chest_45 = Int16.Parse(saveData[134].Substring(saveData[134].IndexOf("=") + 1));
                chest_46 = Int16.Parse(saveData[135].Substring(saveData[135].IndexOf("=") + 1));
                chest_47 = Int16.Parse(saveData[136].Substring(saveData[136].IndexOf("=") + 1));
                chest_48 = Int16.Parse(saveData[137].Substring(saveData[137].IndexOf("=") + 1));
                chest_49 = Int16.Parse(saveData[138].Substring(saveData[138].IndexOf("=") + 1));
                chest_50 = Int16.Parse(saveData[139].Substring(saveData[139].IndexOf("=") + 1));
                chest_51 = Int16.Parse(saveData[140].Substring(saveData[140].IndexOf("=") + 1));
                chest_52 = Int16.Parse(saveData[141].Substring(saveData[141].IndexOf("=") + 1));
                chest_53 = Int16.Parse(saveData[142].Substring(saveData[142].IndexOf("=") + 1));
                chest_54 = Int16.Parse(saveData[143].Substring(saveData[143].IndexOf("=") + 1));
                chest_55 = Int16.Parse(saveData[144].Substring(saveData[144].IndexOf("=") + 1));
                chest_56 = Int16.Parse(saveData[145].Substring(saveData[145].IndexOf("=") + 1));
                chest_57 = Int16.Parse(saveData[146].Substring(saveData[146].IndexOf("=") + 1));
                chest_58 = Int16.Parse(saveData[147].Substring(saveData[147].IndexOf("=") + 1));
                chest_59 = Int16.Parse(saveData[148].Substring(saveData[148].IndexOf("=") + 1));
                chest_60 = Int16.Parse(saveData[149].Substring(saveData[149].IndexOf("=") + 1));
                chest_61 = Int16.Parse(saveData[150].Substring(saveData[150].IndexOf("=") + 1));
                chest_62 = Int16.Parse(saveData[151].Substring(saveData[151].IndexOf("=") + 1));
                chest_63 = Int16.Parse(saveData[152].Substring(saveData[152].IndexOf("=") + 1));
                chest_64 = Int16.Parse(saveData[153].Substring(saveData[153].IndexOf("=") + 1));
                chest_65 = Int16.Parse(saveData[154].Substring(saveData[154].IndexOf("=") + 1));
                chest_66 = Int16.Parse(saveData[155].Substring(saveData[155].IndexOf("=") + 1));
                chest_67 = Int16.Parse(saveData[156].Substring(saveData[156].IndexOf("=") + 1));
                chest_68 = Int16.Parse(saveData[157].Substring(saveData[157].IndexOf("=") + 1));
                chest_69 = Int16.Parse(saveData[158].Substring(saveData[158].IndexOf("=") + 1));
                chest_70 = Int16.Parse(saveData[159].Substring(saveData[159].IndexOf("=") + 1));
                chest_71 = Int16.Parse(saveData[160].Substring(saveData[160].IndexOf("=") + 1));
                chest_72 = Int16.Parse(saveData[161].Substring(saveData[161].IndexOf("=") + 1));
                chest_73 = Int16.Parse(saveData[162].Substring(saveData[162].IndexOf("=") + 1));
                chest_74 = Int16.Parse(saveData[163].Substring(saveData[163].IndexOf("=") + 1));
                chest_75 = Int16.Parse(saveData[164].Substring(saveData[164].IndexOf("=") + 1));
                chest_76 = Int16.Parse(saveData[165].Substring(saveData[165].IndexOf("=") + 1));
                chest_77 = Int16.Parse(saveData[166].Substring(saveData[166].IndexOf("=") + 1));
                chest_78 = Int16.Parse(saveData[167].Substring(saveData[167].IndexOf("=") + 1));
                chest_79 = Int16.Parse(saveData[168].Substring(saveData[168].IndexOf("=") + 1));
                chest_80 = Int16.Parse(saveData[169].Substring(saveData[169].IndexOf("=") + 1));
                chest_81 = Int16.Parse(saveData[170].Substring(saveData[170].IndexOf("=") + 1));
                chest_82 = Int16.Parse(saveData[171].Substring(saveData[171].IndexOf("=") + 1));
                chest_83 = Int16.Parse(saveData[172].Substring(saveData[172].IndexOf("=") + 1));
                chest_84 = Int16.Parse(saveData[173].Substring(saveData[173].IndexOf("=") + 1));
                chest_85 = Int16.Parse(saveData[174].Substring(saveData[174].IndexOf("=") + 1));
                chest_86 = Int16.Parse(saveData[175].Substring(saveData[175].IndexOf("=") + 1));
                chest_87 = Int16.Parse(saveData[176].Substring(saveData[176].IndexOf("=") + 1));
                chest_88 = Int16.Parse(saveData[177].Substring(saveData[177].IndexOf("=") + 1));
                chest_89 = Int16.Parse(saveData[178].Substring(saveData[178].IndexOf("=") + 1));
                chest_90 = Int16.Parse(saveData[179].Substring(saveData[179].IndexOf("=") + 1));
                chest_91 = Int16.Parse(saveData[180].Substring(saveData[180].IndexOf("=") + 1));
                chest_94 = Int16.Parse(saveData[181].Substring(saveData[181].IndexOf("=") + 1));
                chest_95 = Int16.Parse(saveData[182].Substring(saveData[182].IndexOf("=") + 1));
                chest_96 = Int16.Parse(saveData[183].Substring(saveData[183].IndexOf("=") + 1));
                chest_97 = Int16.Parse(saveData[184].Substring(saveData[184].IndexOf("=") + 1));
                chest_98 = Int16.Parse(saveData[185].Substring(saveData[185].IndexOf("=") + 1));
                chest_99 = Int16.Parse(saveData[186].Substring(saveData[186].IndexOf("=") + 1));
                chest_100 = Int16.Parse(saveData[187].Substring(saveData[187].IndexOf("=") + 1));
                chest_101 = Int16.Parse(saveData[188].Substring(saveData[188].IndexOf("=") + 1));
                chest_103 = Int16.Parse(saveData[189].Substring(saveData[189].IndexOf("=") + 1));
                chest_104 = Int16.Parse(saveData[190].Substring(saveData[190].IndexOf("=") + 1));
                chest_105 = Int16.Parse(saveData[191].Substring(saveData[191].IndexOf("=") + 1));
                chest_106 = Int16.Parse(saveData[192].Substring(saveData[192].IndexOf("=") + 1));
                chest_108 = Int16.Parse(saveData[193].Substring(saveData[193].IndexOf("=") + 1));
                chest_109 = Int16.Parse(saveData[194].Substring(saveData[194].IndexOf("=") + 1));
                chest_110 = Int16.Parse(saveData[195].Substring(saveData[195].IndexOf("=") + 1));

                //Mark file as loaded if everything has successfully been set
                FileLoaded = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /************************************************************
         * SetDefault
         * 
         * This function sets each property to its default 
         * value for a fresh save.
         ************************************************************/
        public void SetDefault()
        {
            gem = 0;
            coeur = 30;
            magie = 3;
            level = 1;
            hard = 0;
            heure = 0;
            minute = 1;
            seconde = 299;
            death = 0;
            exp = 10;
            clé = 0;
            pow = 5;
            def = 5;
            scroll = 0;
            ring1 = 0;
            ring2 = 0;
            ring3 = 0;
            ring4 = 0;
            scene = 13;
            x = 2768;
            y = 191;
            pourcent = 0;
            water = 0;
            herbe = 0;
            histoire = 3;
            tutocoffre = 0;
            tutosave = 1;
            lightskill = 0;
            savep1 = 1;
            savep2 = 0;
            savep3 = 0;
            savep4 = 0;
            savep5 = 0;
            savep6 = 0;
            savep7 = 0;
            savep8 = 0;
            savep9 = 0;
            savep10 = 0;
            savep11 = 0;
            savep12 = 0;
            savep13 = 0;
            savep14 = 0;
            savep15 = 0;
            numeroarmure = 0;
            numeroepee = 0;
            armure1 = 0;
            armure2 = 0;
            armure3 = 0;
            armure4 = 0;
            armure5 = 0;
            armure6 = 0;
            armure7 = 0;
            epee1 = 0;
            epee2 = 0;
            epee3 = 0;
            epee4 = 0;
            epee5 = 0;
            epee6 = 0;
            epee7 = 0;
            region1 = 0;
            region2 = 0;
            region3 = 0;
            region4 = 0;
            region5 = 0;
            region6 = 0;
            region7 = 0;
            region8 = 0;
            region9 = 0;
            kingkey = 0;
            livre = 0;
            plume = 0;
            harpefil = 0;
            harpechassis = 0;
            harpmax = 0;
            map = 0;
            boule = 0;
            coffre20xp = 0;
            coffre50xp = 0;
            groscoffre = 0;
            groscoffreelement = 0;
            porte1 = 0;
            porte2 = 0;
            coffre_herbe1 = 0;
            coffre_contener1 = 0;
            coffre_power_up = 0;
            coffre_defense_up = 0;
            coffre_50xp2 = 0;
            coffre_magie_up = 0;
            clé1 = 0;
            power_mont = 0;
            contener_mont = 0;
            herb_mont = 0;
            chest_A1 = 0;
            chest_B1 = 0;
            chest_C1 = 0;
            chest_D1 = 0;
            chest_E1 = 0;
            chest_F1 = 0;
            chest_G1 = 0;
            chest_H1 = 0;
            chest_I1 = 0;
            chest_J1 = 0;
            chest_K1 = 0;
            chest_L1 = 0;
            chest_M1 = 0;
            chest_N1 = 0;
            chest_O1 = 0;
            chest_P1 = 0;
            chest_Q1 = 0;
            chest_R1 = 0;
            chest_S1 = 0;
            chest_T1 = 0;
            chest_U1 = 0;
            chest_V1 = 0;
            chest_W1 = 0;
            chest_X1 = 0;
            chest_Y1 = 0;
            chest_Z1 = 0;
            chest_30 = 0;
            chest_31 = 0;
            chest_32 = 0;
            chest_33 = 0;
            chest_34 = 0;
            chest_35 = 0;
            chest_36 = 0;
            chest_37 = 0;
            chest_38 = 0;
            chest_39 = 0;
            chest_40 = 0;
            chest_41 = 0;
            chest_42 = 0;
            chest_43 = 0;
            chest_44 = 0;
            chest_45 = 0;
            chest_46 = 0;
            chest_47 = 0;
            chest_48 = 0;
            chest_49 = 0;
            chest_50 = 0;
            chest_51 = 0;
            chest_52 = 0;
            chest_53 = 0;
            chest_54 = 0;
            chest_55 = 0;
            chest_56 = 0;
            chest_57 = 0;
            chest_58 = 0;
            chest_59 = 0;
            chest_60 = 0;
            chest_61 = 0;
            chest_62 = 0;
            chest_63 = 0;
            chest_64 = 0;
            chest_65 = 0;
            chest_66 = 0;
            chest_67 = 0;
            chest_68 = 0;
            chest_69 = 0;
            chest_70 = 0;
            chest_71 = 0;
            chest_72 = 0;
            chest_73 = 0;
            chest_74 = 0;
            chest_75 = 0;
            chest_76 = 0;
            chest_77 = 0;
            chest_78 = 0;
            chest_79 = 0;
            chest_80 = 0;
            chest_81 = 0;
            chest_82 = 0;
            chest_83 = 0;
            chest_84 = 0;
            chest_85 = 0;
            chest_86 = 0;
            chest_87 = 0;
            chest_88 = 0;
            chest_89 = 0;
            chest_90 = 0;
            chest_91 = 0;
            chest_94 = 0;
            chest_95 = 0;
            chest_96 = 0;
            chest_97 = 0;
            chest_98 = 0;
            chest_99 = 0;
            chest_100 = 0;
            chest_101 = 0;
            chest_103 = 0;
            chest_104 = 0;
            chest_105 = 0;
            chest_106 = 0;
            chest_108 = 0;
            chest_109 = 0;
            chest_110 = 0;
        }

        /************************************************************
         * SaveChanges
         * 
         * This function creates a string of save data for writing back
         * to the main save file.
         ************************************************************/
        public bool SaveChanges(string savePath)
        {
            //Store the data in an array
            string[] saveData = new string[196];

            //Set each element of the array to the appropriate string
            saveData[0] = "[" + slotNumber + "]"; //expected formatting should be "[sauvegarde#]
            saveData[1] = "gem=" + gem;
            saveData[2] = "coeur=" + coeur;
            saveData[3] = "magie=" + magie;
            saveData[4] = "level=" + level;
            saveData[5] = "hard=" + hard;
            saveData[6] = "heure=" + heure;
            saveData[7] = "minute=" + minute;
            saveData[8] = "seconde=" + seconde;
            saveData[9] = "death=" + death;
            saveData[10] = "exp=" + exp;
            saveData[11] = "clé=" + clé;
            saveData[12] = "pow=" + pow;
            saveData[13] = "def=" + def;
            saveData[14] = "scroll=" + scroll;
            saveData[15] = "ring1=" + ring1;
            saveData[16] = "ring2=" + ring2;
            saveData[17] = "ring3=" + ring3;
            saveData[18] = "ring4=" + ring4;
            saveData[19] = "scene=" + scene;
            saveData[20] = "x=" + x;
            saveData[21] = "y=" + y;
            saveData[22] = "pourcent=" + pourcent;
            saveData[23] = "water=" + water;
            saveData[24] = "herbe=" + herbe;
            saveData[25] = "histoire=" + histoire;
            saveData[26] = "tutocoffre=" + tutocoffre;
            saveData[27] = "tutosave=" + tutosave;
            saveData[28] = "lightskill=" + lightskill;
            saveData[29] = "savep1=" + savep1;
            saveData[30] = "savep2=" + savep2;
            saveData[31] = "savep3=" + savep3;
            saveData[32] = "savep4=" + savep4;
            saveData[33] = "savep5=" + savep5;
            saveData[34] = "savep6=" + savep6;
            saveData[35] = "savep7=" + savep7;
            saveData[36] = "savep8=" + savep8;
            saveData[37] = "savep9=" + savep9;
            saveData[38] = "savep10=" + savep10;
            saveData[39] = "savep11=" + savep11;
            saveData[40] = "savep12=" + savep12;
            saveData[41] = "savep13=" + savep13;
            saveData[42] = "savep14=" + savep14;
            saveData[43] = "savep15=" + savep15;
            saveData[44] = "numeroarmure=" + numeroarmure;
            saveData[45] = "numeroepee=" + numeroepee;
            saveData[46] = "armure1=" + armure1;
            saveData[47] = "armure2=" + armure2;
            saveData[48] = "armure3=" + armure3;
            saveData[49] = "armure4=" + armure4;
            saveData[50] = "armure5=" + armure5;
            saveData[51] = "armure6=" + armure6;
            saveData[52] = "armure7=" + armure7;
            saveData[53] = "epee1=" + epee1;
            saveData[54] = "epee2=" + epee2;
            saveData[55] = "epee3=" + epee3;
            saveData[56] = "epee4=" + epee4;
            saveData[57] = "epee5=" + epee5;
            saveData[58] = "epee6=" + epee6;
            saveData[59] = "epee7=" + epee7;
            saveData[60] = "region1=" + region1;
            saveData[61] = "region2=" + region2;
            saveData[62] = "region3=" + region3;
            saveData[63] = "region4=" + region4;
            saveData[64] = "region5=" + region5;
            saveData[65] = "region6=" + region6;
            saveData[66] = "region7=" + region7;
            saveData[67] = "region8=" + region8;
            saveData[68] = "region9=" + region9;
            saveData[69] = "kingkey=" + kingkey;
            saveData[70] = "livre=" + livre;
            saveData[71] = "plume=" + plume;
            saveData[72] = "harpefil=" + harpefil;
            saveData[73] = "harpechassis=" + harpechassis;
            saveData[74] = "harpmax=" + harpmax;
            saveData[75] = "map=" + map;
            saveData[76] = "boule=" + boule;
            saveData[77] = "coffre20xp=" + coffre20xp;
            saveData[78] = "coffre50xp=" + coffre50xp;
            saveData[79] = "groscoffre=" + groscoffre;
            saveData[80] = "groscoffreelement=" + groscoffreelement;
            saveData[81] = "porte1=" + porte1;
            saveData[82] = "porte2=" + porte2;
            saveData[83] = "coffre_herbe1=" + coffre_herbe1;
            saveData[84] = "coffre_contener1=" + coffre_contener1;
            saveData[85] = "coffre_power_up=" + coffre_power_up;
            saveData[86] = "coffre_defense_up=" + coffre_defense_up;
            saveData[87] = "coffre_50xp2=" + coffre_50xp2;
            saveData[88] = "coffre_magie_up=" + coffre_magie_up;
            saveData[89] = "clé1=" + clé1;
            saveData[90] = "power_mont=" + power_mont;
            saveData[91] = "contener_mont=" + contener_mont;
            saveData[92] = "herb_mont=" + herb_mont;
            saveData[93] = "A1=" + chest_A1;
            saveData[94] = "B1=" + chest_B1;
            saveData[95] = "C1=" + chest_C1;
            saveData[96] = "D1=" + chest_D1;
            saveData[97] = "E1=" + chest_E1;
            saveData[98] = "F1=" + chest_F1;
            saveData[99] = "G1=" + chest_G1;
            saveData[100] = "H1=" + chest_H1;
            saveData[101] = "I1=" + chest_I1;
            saveData[102] = "J1=" + chest_J1;
            saveData[103] = "K1=" + chest_K1;
            saveData[104] = "L1=" + chest_L1;
            saveData[105] = "M1=" + chest_M1;
            saveData[106] = "N1=" + chest_N1;
            saveData[107] = "O1=" + chest_O1;
            saveData[108] = "P1=" + chest_P1;
            saveData[109] = "Q1=" + chest_Q1;
            saveData[110] = "R1=" + chest_R1;
            saveData[111] = "S1=" + chest_S1;
            saveData[112] = "T1=" + chest_T1;
            saveData[113] = "U1=" + chest_U1;
            saveData[114] = "V1=" + chest_V1;
            saveData[115] = "W1=" + chest_W1;
            saveData[116] = "X1=" + chest_X1;
            saveData[117] = "Y1=" + chest_Y1;
            saveData[118] = "Z1=" + chest_Z1;
            saveData[119] = "30=" + chest_30;
            saveData[120] = "31=" + chest_31;
            saveData[121] = "32=" + chest_32;
            saveData[122] = "33=" + chest_33;
            saveData[123] = "34=" + chest_34;
            saveData[124] = "35=" + chest_35;
            saveData[125] = "36=" + chest_36;
            saveData[126] = "37=" + chest_37;
            saveData[127] = "38=" + chest_38;
            saveData[128] = "39=" + chest_39;
            saveData[129] = "40=" + chest_40;
            saveData[130] = "41=" + chest_41;
            saveData[131] = "42=" + chest_42;
            saveData[132] = "43=" + chest_43;
            saveData[133] = "44=" + chest_44;
            saveData[134] = "45=" + chest_45;
            saveData[135] = "46=" + chest_46;
            saveData[136] = "47=" + chest_47;
            saveData[137] = "48=" + chest_48;
            saveData[138] = "49=" + chest_49;
            saveData[139] = "50=" + chest_50;
            saveData[140] = "51=" + chest_51;
            saveData[141] = "52=" + chest_52;
            saveData[142] = "53=" + chest_53;
            saveData[143] = "54=" + chest_54;
            saveData[144] = "55=" + chest_55;
            saveData[145] = "56=" + chest_56;
            saveData[146] = "57=" + chest_57;
            saveData[147] = "58=" + chest_58;
            saveData[148] = "59=" + chest_59;
            saveData[149] = "60=" + chest_60;
            saveData[150] = "61=" + chest_61;
            saveData[151] = "62=" + chest_62;
            saveData[152] = "63=" + chest_63;
            saveData[153] = "64=" + chest_64;
            saveData[154] = "65=" + chest_65;
            saveData[155] = "66=" + chest_66;
            saveData[156] = "67=" + chest_67;
            saveData[157] = "68=" + chest_68;
            saveData[158] = "69=" + chest_69;
            saveData[159] = "70=" + chest_70;
            saveData[160] = "71=" + chest_71;
            saveData[161] = "72=" + chest_72;
            saveData[162] = "73=" + chest_73;
            saveData[163] = "74=" + chest_74;
            saveData[164] = "75=" + chest_75;
            saveData[165] = "76=" + chest_76;
            saveData[166] = "77=" + chest_77;
            saveData[167] = "78=" + chest_78;
            saveData[168] = "79=" + chest_79;
            saveData[169] = "80=" + chest_80;
            saveData[170] = "81=" + chest_81;
            saveData[171] = "82=" + chest_82;
            saveData[172] = "83=" + chest_83;
            saveData[173] = "84=" + chest_84;
            saveData[174] = "85=" + chest_85;
            saveData[175] = "86=" + chest_86;
            saveData[176] = "87=" + chest_87;
            saveData[177] = "88=" + chest_88;
            saveData[178] = "89=" + chest_89;
            saveData[179] = "90=" + chest_90;
            saveData[180] = "91=" + chest_91;
            saveData[181] = "94=" + chest_94;
            saveData[182] = "95=" + chest_95;
            saveData[183] = "96=" + chest_96;
            saveData[184] = "97=" + chest_97;
            saveData[185] = "98=" + chest_98;
            saveData[186] = "99=" + chest_99;
            saveData[187] = "100=" + chest_100;
            saveData[188] = "101=" + chest_101;
            saveData[189] = "103=" + chest_103;
            saveData[190] = "104=" + chest_104;
            saveData[191] = "105=" + chest_105;
            saveData[192] = "106=" + chest_106;
            saveData[193] = "108=" + chest_108;
            saveData[194] = "109=" + chest_109;
            saveData[195] = "110=" + chest_110;

            try
            {
                //Write all lines from the array to the file
                System.IO.File.WriteAllLines(savePath, saveData);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }
    }
}
