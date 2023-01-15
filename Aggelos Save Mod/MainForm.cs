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
 * - Features:
 *      - Add way to delete a file from the tool.
 *      - Look for cutscene triggers to allow a "skip cutscenes" option if it doesn't break progression of game.
 *      - Add a help files section that has guides and/or documentation of things that are currently commented in code.
 *      - Add copy/duplicate to scene selector
 *      - Store/load scenes from Scene class instead of hardcoded
 *      - Fix default coordinates for things
 * - Cleanup:
 *      - 
 * - Bugs:
 *      - 
 ***************************/

namespace Aggelos_Save_Mod
{
    public partial class MainForm : Form
    {
        //Declaring global save file object for use throughout the program
        public Save saveFile = new Save();

        //Declaring preset file being used
        public string presetFile = "";

        //Declaring a list of default scenes and coordinates to use for each area
        public Scene[] defaultScenesList = new Scene[]
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

        public MainForm()
        {
            InitializeComponent();

            //Show detected intall path
            tbInstallPath.Text = saveFile.InstallationPath;

            //Load scenes list from file
            LoadScenes();
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

                tbPower.Enabled = true;
                tbPower.Text = saveFile.pow.ToString();

                tbDefense.Enabled = true;
                tbDefense.Text = saveFile.def.ToString();

                tbHealth.Enabled = true;
                tbHealth.Text = saveFile.coeur.ToString();

                tbMagic.Enabled = true;
                tbMagic.Text = saveFile.magie.ToString();

                //Inventory
                //Items
                checkHerb.Enabled = true;
                checkHerb.Checked = saveFile.herbe == 1 ? true : false;

                //Control the visibility of the correct image
                btnSelectPotionLeft.Visible = true;
                btnSelectPotionRight.Visible = true;
                switch (saveFile.boule)
                {
                    case 0:
                        picSmallPotion.Visible = false;
                        picBigPotion.Visible = false;
                        picElixir.Visible = false;
                        btnSelectPotionLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picSmallPotion.Visible = true;
                        picBigPotion.Visible = false;
                        picElixir.Visible = false;
                        break;
                    case 2:
                        picSmallPotion.Visible = false;
                        picBigPotion.Visible = true;
                        picElixir.Visible = false;
                        break;
                    case 3:
                        picSmallPotion.Visible = false;
                        picBigPotion.Visible = false;
                        picElixir.Visible = true;
                        btnSelectPotionRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                checkLumenKey.Enabled = true;
                checkLumenKey.Checked = saveFile.kingkey == 1 ? true : false;

                checkAngelFeather.Enabled = true;
                checkAngelFeather.Checked = saveFile.plume == 1 ? true : false;

                //Control the visibility of the correct image
                btnSelectQuestItemLeft.Visible = true;
                btnSelectQuestItemRight.Visible = true;
                switch (saveFile.map)
                {
                    case 0:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        btnSelectQuestItemLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picBananas.Visible = true;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 2:
                        picBananas.Visible = false;
                        picNecklace.Visible = true;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 3:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = true;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 4:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = true;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 5:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = true;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 6:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = true;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 7:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = true;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 8:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = true;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 9:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = true;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 10:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = true;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = false;
                        break;
                    case 11:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = true;
                        picFullVial.Visible = false;
                        break;
                    case 12:
                        picBananas.Visible = false;
                        picNecklace.Visible = false;
                        picBrokenCrown.Visible = false;
                        picTiara.Visible = false;
                        picMoon.Visible = false;
                        picShell.Visible = false;
                        picStar.Visible = false;
                        picCrystal.Visible = false;
                        picScepter.Visible = false;
                        picSun.Visible = false;
                        picEmptyVial.Visible = false;
                        picFullVial.Visible = true;
                        btnSelectQuestItemRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                checkUniversalBook.Enabled = true;
                checkUniversalBook.Checked = saveFile.livre == 1 ? true : false;

                //Determine the current state of the lyre based on the three variables
                int currentLyreState = 0;
                if (saveFile.harpefil == 0 && saveFile.harpechassis == 0 && saveFile.harpmax == 0)
                {
                    currentLyreState = 0;
                }
                else if (saveFile.harpefil == 1 && saveFile.harpechassis == 0 && saveFile.harpmax == 0)
                {
                    currentLyreState = 1;
                }
                else if (saveFile.harpefil == 0 && saveFile.harpechassis == 1 && saveFile.harpmax == 0)
                {
                    currentLyreState = 2;
                }
                else if (saveFile.harpefil == 1 && saveFile.harpechassis == 1 && saveFile.harpmax == 0)
                {
                    currentLyreState = 3;
                }
                else if (saveFile.harpefil == 1 && saveFile.harpechassis == 1 && saveFile.harpmax == 1)
                {
                    currentLyreState = 4;
                }
                //Control the visibility of the correct image and set the correct variables
                btnSelectLyreLeft.Visible = true;
                btnSelectLyreRight.Visible = true;
                switch (currentLyreState)
                {
                    //Have none
                    case 0:
                        saveFile.harpefil = 0;
                        saveFile.harpechassis = 0;
                        saveFile.harpmax = 0;
                        picLyreStrings.Visible = false;
                        picLyreBody.Visible = false;
                        picLyreWithStrings.Visible = false;
                        picLyreRepaired.Visible = false;
                        btnSelectLyreLeft.Visible = false; //Re-disable if at min
                        break;
                    //Have string only
                    case 1:
                        saveFile.harpefil = 1;
                        saveFile.harpechassis = 0;
                        saveFile.harpmax = 0;
                        picLyreStrings.Visible = true;
                        picLyreBody.Visible = false;
                        picLyreWithStrings.Visible = false;
                        picLyreRepaired.Visible = false;
                        break;
                    //Have body only
                    case 2:
                        saveFile.harpefil = 0;
                        saveFile.harpechassis = 1;
                        saveFile.harpmax = 0;
                        picLyreStrings.Visible = false;
                        picLyreBody.Visible = true;
                        picLyreWithStrings.Visible = false;
                        picLyreRepaired.Visible = false;
                        break;
                    //Have body and string
                    case 3:
                        saveFile.harpefil = 1;
                        saveFile.harpechassis = 1;
                        saveFile.harpmax = 0;
                        picLyreStrings.Visible = false;
                        picLyreBody.Visible = false;
                        picLyreWithStrings.Visible = true;
                        picLyreRepaired.Visible = false;
                        break;
                    //Repaired
                    case 4:
                        saveFile.harpefil = 1;
                        saveFile.harpechassis = 1;
                        saveFile.harpmax = 1;
                        picLyreStrings.Visible = false;
                        picLyreBody.Visible = false;
                        picLyreWithStrings.Visible = false;
                        picLyreRepaired.Visible = true;
                        btnSelectLyreRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                //Rings and Essences - 1 or 2 for each ring. Can't seem to have essences without rings.
                //Control the visibility of the correct image
                btnSelectEarthLeft.Visible = true;
                btnSelectEarthRight.Visible = true;
                switch (saveFile.ring1)
                {
                    case 0:
                        picEarthRing.Visible = false;
                        picEarthEssence.Visible = false;
                        btnSelectEarthLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picEarthRing.Visible = true;
                        picEarthEssence.Visible = false;
                        break;
                    case 2:
                        picEarthRing.Visible = false;
                        picEarthEssence.Visible = true;
                        btnSelectEarthRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                btnSelectWaterLeft.Visible = true;
                btnSelectWaterRight.Visible = true;
                switch (saveFile.ring2)
                {
                    case 0:
                        picWaterRing.Visible = false;
                        picWaterEssence.Visible = false;
                        btnSelectWaterLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picWaterRing.Visible = true;
                        picWaterEssence.Visible = false;
                        break;
                    case 2:
                        picWaterRing.Visible = false;
                        picWaterEssence.Visible = true;
                        btnSelectWaterRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                btnSelectFireLeft.Visible = true;
                btnSelectFireRight.Visible = true;
                switch (saveFile.ring3)
                {
                    case 0:
                        picFireRing.Visible = false;
                        picFireEssence.Visible = false;
                        btnSelectFireLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picFireRing.Visible = true;
                        picFireEssence.Visible = false;
                        break;
                    case 2:
                        picFireRing.Visible = false;
                        picFireEssence.Visible = true;
                        btnSelectFireRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                btnSelectAirLeft.Visible = true;
                btnSelectAirRight.Visible = true;
                switch (saveFile.ring4)
                {
                    case 0:
                        picAirRing.Visible = false;
                        picAirEssence.Visible = false;
                        btnSelectAirLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picAirRing.Visible = true;
                        picAirEssence.Visible = false;
                        break;
                    case 2:
                        picAirRing.Visible = false;
                        picAirEssence.Visible = true;
                        btnSelectAirRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                //Scrolls - 1, 2, or 3 for each main skill. Can't seem to have laters without previous.
                //Control the visibility of the correct image
                btnSelectScrollLeft.Visible = true;
                btnSelectScrollRight.Visible = true;
                switch (saveFile.scroll)
                {
                    case 0:
                        picMoleScroll.Visible = false;
                        picMoleText.Visible = false;
                        picFleaScroll.Visible = false;
                        picFleaText.Visible = false;
                        picWoodpeckerScroll.Visible = false;
                        picWoodpeckerText.Visible = false;
                        btnSelectScrollLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picMoleScroll.Visible = true;
                        picMoleText.Visible = true;
                        picFleaScroll.Visible = false;
                        picFleaText.Visible = false;
                        picWoodpeckerScroll.Visible = false;
                        picWoodpeckerText.Visible = false;
                        break;
                    case 2:
                        picMoleScroll.Visible = false;
                        picMoleText.Visible = false;
                        picFleaScroll.Visible = true;
                        picFleaText.Visible = true;
                        picWoodpeckerScroll.Visible = false;
                        picWoodpeckerText.Visible = false;
                        break;
                    case 3:
                        picMoleScroll.Visible = false;
                        picMoleText.Visible = false;
                        picFleaScroll.Visible = false;
                        picFleaText.Visible = false;
                        picWoodpeckerScroll.Visible = true;
                        picWoodpeckerText.Visible = true;
                        btnSelectScrollRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

                //Light Skills - Firefly scroll and light essence are tied together
                btnSelectLightSkillLeft.Visible = true;
                btnSelectLightSkillRight.Visible = true;
                switch (saveFile.lightskill)
                {
                    case 0:
                        picLightEssence.Visible = false;
                        picFireflyScroll.Visible = false;
                        btnSelectLightSkillLeft.Visible = false; //Re-disable if at min
                        break;
                    case 1:
                        picLightEssence.Visible = true;
                        picFireflyScroll.Visible = false;
                        break;
                    case 2:
                        picLightEssence.Visible = false;
                        picFireflyScroll.Visible = true;
                        btnSelectLightSkillRight.Visible = false; //Re-disable if at max
                        break;
                    default:
                        break;
                }

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


                //Save Point Map
                checkSaveLumenWoods.Enabled = true;
                checkSaveLumenWoods.Checked = saveFile.savep1 == 1 ? true : false;

                checkSaveBoscoVillage.Enabled = true;
                checkSaveBoscoVillage.Checked = saveFile.savep2 == 1 ? true : false;

                checkSaveBoscoCave.Enabled = true;
                checkSaveBoscoCave.Checked = saveFile.savep3 == 1 ? true : false;

                checkSaveLumenCastle.Enabled = true;
                checkSaveLumenCastle.Checked = saveFile.savep4 == 1 ? true : false;

                checkSaveAtlantVillage.Enabled = true;
                checkSaveAtlantVillage.Checked = saveFile.savep5 == 1 ? true : false;

                checkSaveTheAbyss.Enabled = true;
                checkSaveTheAbyss.Checked = saveFile.savep6 == 1 ? true : false;

                checkSavePaluluTown.Enabled = true;
                checkSavePaluluTown.Checked = saveFile.savep7 == 1 ? true : false;

                checkSaveTheWall.Enabled = true;
                checkSaveTheWall.Checked = saveFile.savep8 == 1 ? true : false;

                checkSaveFiraVillage.Enabled = true;
                checkSaveFiraVillage.Checked = saveFile.savep9 == 1 ? true : false;

                checkSaveFiraVolcano.Enabled = true;
                checkSaveFiraVolcano.Checked = saveFile.savep10 == 1 ? true : false;

                checkSaveCelestiaVillage.Enabled = true;
                checkSaveCelestiaVillage.Checked = saveFile.savep11 == 1 ? true : false;

                checkSaveDarkClouds.Enabled = true;
                checkSaveDarkClouds.Checked = saveFile.savep12 == 1 ? true : false;

                checkSaveValionsCastle.Enabled = true;
                checkSaveValionsCastle.Checked = saveFile.savep13 == 1 ? true : false;


                //Scenes
                //Figure out which scene is selected in the list view
                //Exit once one is found in case there are multiple
                foreach (ListViewItem item in listViewScenes.Items)
                {
                    if (item.Tag.ToString() == saveFile.scene.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }

                //Load X/Y coordinates
                tbX.Enabled = true;
                tbX.Text = saveFile.x.ToString();

                tbY.Enabled = true;
                tbY.Text = saveFile.y.ToString();

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
            if (tbGems.Value > tbGems.Maximum)
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

        private void tbPower_ValueChanged(object sender, EventArgs e)
        {
            if (tbPower.Value > tbPower.Maximum)
            {
                tbPower.Value = tbPower.Maximum;
            }
            else if (tbPower.Value < tbPower.Minimum)
            {
                tbPower.Value = tbPower.Minimum;
            }

            saveFile.pow = ((int)tbPower.Value);
        }

        private void tbDefense_ValueChanged(object sender, EventArgs e)
        {
            if (tbDefense.Value > tbDefense.Maximum)
            {
                tbDefense.Value = tbDefense.Maximum;
            }
            else if (tbDefense.Value < tbDefense.Minimum)
            {
                tbDefense.Value = tbDefense.Minimum;
            }

            saveFile.def = ((int)tbDefense.Value);
        }

        private void tbHealth_ValueChanged(object sender, EventArgs e)
        {
            if (tbHealth.Value > tbHealth.Maximum)
            {
                tbHealth.Value = tbHealth.Maximum;
            }
            else if (tbHealth.Value < tbHealth.Minimum)
            {
                tbHealth.Value = tbHealth.Minimum;
            }

            saveFile.coeur = ((int)tbHealth.Value);
        }

        private void tbMagic_ValueChanged(object sender, EventArgs e)
        {
            if (tbMagic.Value > tbMagic.Maximum)
            {
                tbMagic.Value = tbMagic.Maximum;
            }
            else if (tbMagic.Value < tbMagic.Minimum)
            {
                tbMagic.Value = tbMagic.Minimum;
            }

            saveFile.magie = ((int)tbMagic.Value);
        }

        /************************************************************
        * INVENTORY CHANGES
        * 
        * The following functions are called for inventory changes.
        ************************************************************/
        private void btnSelectPotionLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for potion slot as long as we aren't at min
            if (saveFile.boule > 0)
            {
                saveFile.boule -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectPotionRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.boule == 0)
            {
                btnSelectPotionLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.boule)
            {
                case 0:
                    picSmallPotion.Visible = false;
                    picBigPotion.Visible = false;
                    picElixir.Visible = false;
                    break;
                case 1:
                    picSmallPotion.Visible = true;
                    picBigPotion.Visible = false;
                    picElixir.Visible = false;
                    break;
                case 2:
                    picSmallPotion.Visible = false;
                    picBigPotion.Visible = true;
                    picElixir.Visible = false;
                    break;
                case 3:
                    picSmallPotion.Visible = false;
                    picBigPotion.Visible = false;
                    picElixir.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectPotionRight_Click(object sender, EventArgs e)
        {
            //Increase the value for potion slot as long as we aren't at max
            if (saveFile.boule < 3)
            {
                saveFile.boule += 1;
                //Make sure to re-enable the left select now that we've decreased
                btnSelectPotionLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.boule == 3)
            {
                btnSelectPotionRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.boule)
            {
                case 0:
                    picSmallPotion.Visible = false;
                    picBigPotion.Visible = false;
                    picElixir.Visible = false;
                    break;
                case 1:
                    picSmallPotion.Visible = true;
                    picBigPotion.Visible = false;
                    picElixir.Visible = false;
                    break;
                case 2:
                    picSmallPotion.Visible = false;
                    picBigPotion.Visible = true;
                    picElixir.Visible = false;
                    break;
                case 3:
                    picSmallPotion.Visible = false;
                    picBigPotion.Visible = false;
                    picElixir.Visible = true;
                    break;
                default:
                    break;
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

        private void btnSelectQuestItemLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for quest item slot as long as we aren't at min
            if (saveFile.map > 0)
            {
                saveFile.map -= 1;
                //Make sure to re-enable the right select now that we've decreased
                btnSelectQuestItemRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.map == 0)
            {
                btnSelectQuestItemLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.map)
            {
                case 0:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 1:
                    picBananas.Visible = true;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 2:
                    picBananas.Visible = false;
                    picNecklace.Visible = true;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 3:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = true;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 4:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = true;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 5:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = true;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 6:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = true;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 7:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = true;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 8:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = true;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 9:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = true;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 10:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = true;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 11:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = true;
                    picFullVial.Visible = false;
                    break;
                case 12:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectQuestItemRight_Click(object sender, EventArgs e)
        {
            //Increase the value for quest item slot as long as we aren't at max
            if (saveFile.map < 12)
            {
                saveFile.map += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectQuestItemLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.map == 12)
            {
                btnSelectQuestItemRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.map)
            {
                case 0:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 1:
                    picBananas.Visible = true;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 2:
                    picBananas.Visible = false;
                    picNecklace.Visible = true;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 3:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = true;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 4:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = true;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 5:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = true;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 6:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = true;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 7:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = true;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 8:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = true;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 9:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = true;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 10:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = true;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = false;
                    break;
                case 11:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = true;
                    picFullVial.Visible = false;
                    break;
                case 12:
                    picBananas.Visible = false;
                    picNecklace.Visible = false;
                    picBrokenCrown.Visible = false;
                    picTiara.Visible = false;
                    picMoon.Visible = false;
                    picShell.Visible = false;
                    picStar.Visible = false;
                    picCrystal.Visible = false;
                    picScepter.Visible = false;
                    picSun.Visible = false;
                    picEmptyVial.Visible = false;
                    picFullVial.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void checkUniversalBook_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.livre = checkUniversalBook.Checked == true ? 1 : 0;
        }

        private void btnSelectLyreLeft_Click(object sender, EventArgs e)
        {
            //Determine the current state of the lyre based on the three variables
            int currentLyreState = 0;
            if (saveFile.harpefil == 0 && saveFile.harpechassis == 0 && saveFile.harpmax == 0)
            {
                currentLyreState = 0;
            }
            else if (saveFile.harpefil == 1 && saveFile.harpechassis == 0 && saveFile.harpmax == 0)
            {
                currentLyreState = 1;
            }
            else if (saveFile.harpefil == 0 && saveFile.harpechassis == 1 && saveFile.harpmax == 0)
            {
                currentLyreState = 2;
            }
            else if (saveFile.harpefil == 1 && saveFile.harpechassis == 1 && saveFile.harpmax == 0)
            {
                currentLyreState = 3;
            }
            else if (saveFile.harpefil == 1 && saveFile.harpechassis == 1 && saveFile.harpmax == 1)
            {
                currentLyreState = 4;
            }

            //Decrease the value for lyre slot as long as we aren't at min
            if (currentLyreState > 0)
            {
                currentLyreState -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectLyreRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (currentLyreState == 0)
            {
                btnSelectLyreLeft.Visible = false;
            }

            //Control the visibility of the correct image and set the correct variables
            switch (currentLyreState)
            {
                //Have none
                case 0:
                    saveFile.harpefil = 0;
                    saveFile.harpechassis = 0;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = false;
                    break;
                //Have string only
                case 1:
                    saveFile.harpefil = 1;
                    saveFile.harpechassis = 0;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = true;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = false;
                    break;
                //Have body only
                case 2:
                    saveFile.harpefil = 0;
                    saveFile.harpechassis = 1;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = true;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = false;
                    break;
                //Have body and string
                case 3:
                    saveFile.harpefil = 1;
                    saveFile.harpechassis = 1;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = true;
                    picLyreRepaired.Visible = false;
                    break;
                //Repaired
                case 4:
                    saveFile.harpefil = 1;
                    saveFile.harpechassis = 1;
                    saveFile.harpmax = 1;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectLyreRight_Click(object sender, EventArgs e)
        {
            //Determine the current state of the lyre based on the three variables
            int currentLyreState = 0;
            if (saveFile.harpefil == 0 && saveFile.harpechassis == 0 && saveFile.harpmax == 0)
            {
                currentLyreState = 0;
            }
            else if (saveFile.harpefil == 1 && saveFile.harpechassis == 0 && saveFile.harpmax == 0)
            {
                currentLyreState = 1;
            }
            else if (saveFile.harpefil == 0 && saveFile.harpechassis == 1 && saveFile.harpmax == 0)
            {
                currentLyreState = 2;
            }
            else if (saveFile.harpefil == 1 && saveFile.harpechassis == 1 && saveFile.harpmax == 0)
            {
                currentLyreState = 3;
            }
            else if (saveFile.harpefil == 1 && saveFile.harpechassis == 1 && saveFile.harpmax == 1)
            {
                currentLyreState = 4;
            }

            //Increase the value for lyre slot as long as we aren't at max
            if (currentLyreState < 4)
            {
                currentLyreState += 1;

                //Make sure to re-enable the left select now that we've increased
                btnSelectLyreLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (currentLyreState == 4)
            {
                btnSelectLyreRight.Visible = false;
            }

            //Control the visibility of the correct image and set the correct variables
            switch (currentLyreState)
            {
                //Have none
                case 0:
                    saveFile.harpefil = 0;
                    saveFile.harpechassis = 0;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = false;
                    break;
                //Have string only
                case 1:
                    saveFile.harpefil = 1;
                    saveFile.harpechassis = 0;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = true;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = false;
                    break;
                //Have body only
                case 2:
                    saveFile.harpefil = 0;
                    saveFile.harpechassis = 1;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = true;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = false;
                    break;
                //Have body and string
                case 3:
                    saveFile.harpefil = 1;
                    saveFile.harpechassis = 1;
                    saveFile.harpmax = 0;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = true;
                    picLyreRepaired.Visible = false;
                    break;
                //Repaired
                case 4:
                    saveFile.harpefil = 1;
                    saveFile.harpechassis = 1;
                    saveFile.harpmax = 1;
                    picLyreStrings.Visible = false;
                    picLyreBody.Visible = false;
                    picLyreWithStrings.Visible = false;
                    picLyreRepaired.Visible = true;
                    break;
                default:
                    break;
            }
        }

        /************************************************************
        * SCROLL CHANGES
        * 
        * The following functions are called for scroll changes.
        ************************************************************/
        private void btnSelectScrollLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for ring slot as long as we aren't at min
            if (saveFile.scroll > 0)
            {
                saveFile.scroll -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectScrollRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.scroll == 0)
            {
                btnSelectScrollLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.scroll)
            {
                case 0:
                    picMoleScroll.Visible = false;
                    picMoleText.Visible = false;
                    picFleaScroll.Visible = false;
                    picFleaText.Visible = false;
                    picWoodpeckerScroll.Visible = false;
                    picWoodpeckerText.Visible = false;
                    break;
                case 1:
                    picMoleScroll.Visible = true;
                    picMoleText.Visible = true;
                    picFleaScroll.Visible = false;
                    picFleaText.Visible = false;
                    picWoodpeckerScroll.Visible = false;
                    picWoodpeckerText.Visible = false;
                    break;
                case 2:
                    picMoleScroll.Visible = false;
                    picMoleText.Visible = false;
                    picFleaScroll.Visible = true;
                    picFleaText.Visible = true;
                    picWoodpeckerScroll.Visible = false;
                    picWoodpeckerText.Visible = false;
                    break;
                case 3:
                    picMoleScroll.Visible = false;
                    picMoleText.Visible = false;
                    picFleaScroll.Visible = false;
                    picFleaText.Visible = false;
                    picWoodpeckerScroll.Visible = true;
                    picWoodpeckerText.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectScrollRight_Click(object sender, EventArgs e)
        {
            //Increase the value for ring slot as long as we aren't at max
            if (saveFile.scroll < 3)
            {
                saveFile.scroll += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectScrollLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.scroll == 3)
            {
                btnSelectScrollRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.scroll)
            {
                case 0:
                    picMoleScroll.Visible = false;
                    picMoleText.Visible = false;
                    picFleaScroll.Visible = false;
                    picFleaText.Visible = false;
                    picWoodpeckerScroll.Visible = false;
                    picWoodpeckerText.Visible = false;
                    break;
                case 1:
                    picMoleScroll.Visible = true;
                    picMoleText.Visible = true;
                    picFleaScroll.Visible = false;
                    picFleaText.Visible = false;
                    picWoodpeckerScroll.Visible = false;
                    picWoodpeckerText.Visible = false;
                    break;
                case 2:
                    picMoleScroll.Visible = false;
                    picMoleText.Visible = false;
                    picFleaScroll.Visible = true;
                    picFleaText.Visible = true;
                    picWoodpeckerScroll.Visible = false;
                    picWoodpeckerText.Visible = false;
                    break;
                case 3:
                    picMoleScroll.Visible = false;
                    picMoleText.Visible = false;
                    picFleaScroll.Visible = false;
                    picFleaText.Visible = false;
                    picWoodpeckerScroll.Visible = true;
                    picWoodpeckerText.Visible = true;
                    break;
                default:
                    break;
            }
        }

        /************************************************************
        * RING AND ESSENCE CHANGES
        * 
        * The following functions are called for the ring and essence changes.
        * There is no way to give yourself the essence and not the ring.
        ************************************************************/
        private void btnSelectEarthLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for ring slot as long as we aren't at min
            if (saveFile.ring1 > 0)
            {
                saveFile.ring1 -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectEarthRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.ring1 == 0)
            {
                btnSelectEarthLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring1)
            {
                case 0:
                    picEarthRing.Visible = false;
                    picEarthEssence.Visible = false;
                    break;
                case 1:
                    picEarthRing.Visible = true;
                    picEarthEssence.Visible = false;
                    break;
                case 2:
                    picEarthRing.Visible = true;
                    picEarthEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectEarthRight_Click(object sender, EventArgs e)
        {
            //Increase the value for ring slot as long as we aren't at max
            if (saveFile.ring1 < 2)
            {
                saveFile.ring1 += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectEarthLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.ring1 == 2)
            {
                btnSelectEarthRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring1)
            {
                case 0:
                    picEarthRing.Visible = false;
                    picEarthEssence.Visible = false;
                    break;
                case 1:
                    picEarthRing.Visible = true;
                    picEarthEssence.Visible = false;
                    break;
                case 2:
                    picEarthRing.Visible = true;
                    picEarthEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectWaterLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for ring slot as long as we aren't at min
            if (saveFile.ring2 > 0)
            {
                saveFile.ring2 -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectWaterRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.ring2 == 0)
            {
                btnSelectWaterLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring2)
            {
                case 0:
                    picWaterRing.Visible = false;
                    picWaterEssence.Visible = false;
                    break;
                case 1:
                    picWaterRing.Visible = true;
                    picWaterEssence.Visible = false;
                    break;
                case 2:
                    picEarthRing.Visible = true;
                    picWaterEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectWaterRight_Click(object sender, EventArgs e)
        {
            //Increase the value for ring slot as long as we aren't at max
            if (saveFile.ring2 < 2)
            {
                saveFile.ring2 += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectWaterLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.ring2 == 2)
            {
                btnSelectWaterRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring2)
            {
                case 0:
                    picWaterRing.Visible = false;
                    picWaterEssence.Visible = false;
                    break;
                case 1:
                    picWaterRing.Visible = true;
                    picWaterEssence.Visible = false;
                    break;
                case 2:
                    picWaterRing.Visible = true;
                    picWaterEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectFireLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for ring slot as long as we aren't at min
            if (saveFile.ring3 > 0)
            {
                saveFile.ring3 -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectFireRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.ring3 == 0)
            {
                btnSelectFireLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring3)
            {
                case 0:
                    picFireRing.Visible = false;
                    picFireEssence.Visible = false;
                    break;
                case 1:
                    picFireRing.Visible = true;
                    picFireEssence.Visible = false;
                    break;
                case 2:
                    picFireRing.Visible = true;
                    picFireEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectFireRight_Click(object sender, EventArgs e)
        {
            //Increase the value for ring slot as long as we aren't at max
            if (saveFile.ring3 < 2)
            {
                saveFile.ring3 += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectFireLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.ring3 == 2)
            {
                btnSelectFireRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring3)
            {
                case 0:
                    picFireRing.Visible = false;
                    picFireEssence.Visible = false;
                    break;
                case 1:
                    picFireRing.Visible = true;
                    picFireEssence.Visible = false;
                    break;
                case 2:
                    picFireRing.Visible = true;
                    picFireEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectAirLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for ring slot as long as we aren't at min
            if (saveFile.ring4 > 0)
            {
                saveFile.ring4 -= 1;

                //Make sure to re-enable the right select now that we've decreased
                btnSelectAirRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.ring4 == 0)
            {
                btnSelectAirLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring4)
            {
                case 0:
                    picAirRing.Visible = false;
                    picAirEssence.Visible = false;
                    break;
                case 1:
                    picAirRing.Visible = true;
                    picAirEssence.Visible = false;
                    break;
                case 2:
                    picAirRing.Visible = true;
                    picAirEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectAirRight_Click(object sender, EventArgs e)
        {
            //Increase the value for ring slot as long as we aren't at max
            if (saveFile.ring4 < 2)
            {
                saveFile.ring4 += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectAirLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.ring4 == 2)
            {
                btnSelectAirRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.ring4)
            {
                case 0:
                    picAirRing.Visible = false;
                    picAirEssence.Visible = false;
                    break;
                case 1:
                    picAirRing.Visible = true;
                    picAirEssence.Visible = false;
                    break;
                case 2:
                    picAirRing.Visible = true;
                    picAirEssence.Visible = true;
                    break;
                default:
                    break;
            }
        }

        /************************************************************
        * LIGHT SKILL CHANGES
        * 
        * The following functions are called for the light essence and firefly scroll.
        * There is no way to give yourself the scroll and not the essence.
        ************************************************************/
        private void btnSelectLightSkillLeft_Click(object sender, EventArgs e)
        {
            //Decrease the value for ring slot as long as we aren't at min
            if (saveFile.lightskill > 0)
            {
                saveFile.lightskill -= 1;

                btnSelectLightSkillRight.Visible = true;
            }

            //If we ever become the min, disable the control
            if (saveFile.lightskill == 0)
            {
                btnSelectLightSkillLeft.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.lightskill)
            {
                case 0:
                    picLightEssence.Visible = false;
                    picFireflyScroll.Visible = false;
                    break;
                case 1:
                    picLightEssence.Visible = true;
                    picFireflyScroll.Visible = false;
                    break;
                case 2:
                    picLightEssence.Visible = false;
                    picFireflyScroll.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btnSelectLightSkillRight_Click(object sender, EventArgs e)
        {
            //Increase the value for ring slot as long as we aren't at max
            if (saveFile.lightskill < 2)
            {
                saveFile.lightskill += 1;
                //Make sure to re-enable the left select now that we've increased
                btnSelectLightSkillLeft.Visible = true;
            }

            //If we ever become the max, disable the control
            if (saveFile.lightskill == 2)
            {
                btnSelectLightSkillRight.Visible = false;
            }

            //Control the visibility of the correct image
            switch (saveFile.lightskill)
            {
                case 0:
                    picLightEssence.Visible = false;
                    picFireflyScroll.Visible = false;
                    break;
                case 1:
                    picLightEssence.Visible = true;
                    picFireflyScroll.Visible = false;
                    break;
                case 2:
                    picLightEssence.Visible = false;
                    picFireflyScroll.Visible = true;
                    break;
                default:
                    break;
            }
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
                //Make sure to re-enable the left select now that we've increased
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
                //Make sure to re-enable the left select now that we've increased
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

        /************************************************************
        * SAVE SLOTS
        * 
        * The following functions are called for save slot changes.
        ************************************************************/
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

        /************************************************************
        * SAVE POINT UNLOCKS
        * 
        * The following functions are called for save point unlocks.
        ************************************************************/
        private void checkSaveLumenWoods_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep1 = checkSaveLumenWoods.Checked == true ? 1 : 0;

            //Update the image
            checkSaveLumenWoods.ImageIndex = saveFile.savep1;
        }

        private void checkSaveBoscoVillage_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep2 = checkSaveBoscoVillage.Checked == true ? 1 : 0;

            //Update the image
            checkSaveBoscoVillage.ImageIndex = saveFile.savep2;
        }

        private void checkSaveBoscoCave_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep3 = checkSaveBoscoCave.Checked == true ? 1 : 0;

            //Update the image
            checkSaveBoscoCave.ImageIndex = saveFile.savep3;
        }

        private void checkSaveLumenCastle_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep4 = checkSaveLumenCastle.Checked == true ? 1 : 0;

            //Update the image
            checkSaveLumenCastle.ImageIndex = saveFile.savep4;
        }

        private void checkSaveAtlantVillage_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep5 = checkSaveAtlantVillage.Checked == true ? 1 : 0;

            //Update the image
            checkSaveAtlantVillage.ImageIndex = saveFile.savep5;
        }

        private void checkSaveTheAbyss_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep6 = checkSaveTheAbyss.Checked == true ? 1 : 0;

            //Update the image
            checkSaveTheAbyss.ImageIndex = saveFile.savep6;
        }

        private void checkSavePaluluTown_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep7 = checkSavePaluluTown.Checked == true ? 1 : 0;

            //Update the image
            checkSavePaluluTown.ImageIndex = saveFile.savep7;
        }

        private void checkSaveTheWall_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep8 = checkSaveTheWall.Checked == true ? 1 : 0;

            //Update the image
            checkSaveTheWall.ImageIndex = saveFile.savep8;
        }

        private void checkSaveFiraVillage_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep9 = checkSaveFiraVillage.Checked == true ? 1 : 0;

            //Update the image
            checkSaveFiraVillage.ImageIndex = saveFile.savep9;
        }

        private void checkSaveFiraVolcano_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep10 = checkSaveFiraVolcano.Checked == true ? 1 : 0;

            //Update the image
            checkSaveFiraVolcano.ImageIndex = saveFile.savep10;
        }

        private void checkSaveCelestiaVillage_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep11 = checkSaveCelestiaVillage.Checked == true ? 1 : 0;

            //Update the image
            checkSaveCelestiaVillage.ImageIndex = saveFile.savep11;
        }

        private void checkSaveDarkClouds_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep12 = checkSaveDarkClouds.Checked == true ? 1 : 0;

            //Update the image
            checkSaveDarkClouds.ImageIndex = saveFile.savep12;
        }

        private void checkSaveValionsCastle_CheckedChanged(object sender, EventArgs e)
        {
            saveFile.savep13 = checkSaveValionsCastle.Checked == true ? 1 : 0;

            //Update the image
            checkSaveValionsCastle.ImageIndex = saveFile.savep13;
        }

        /************************************************************
        * SCENE SELECTIONS
        * 
        * This function is called when the user selects a new scene
        * from the list provided. The collection is defined in the 
        * control and each item's "Tag" value holds the scene number
        * to set for the save.
        * Control is called twice on selection, once for "unselecting"
        * the first item and again for "selecting" the second.
        ************************************************************/
        private void listViewScenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Since this function can be called on deselect, only run it if we have selected an item.
            if (listViewScenes.SelectedItems.Count > 0)
            {
                //Get the selected scene number and information
                int sceneNumber = 0;
                string x = "0";
                string y = "0";
                try
                {
                    sceneNumber = Int32.Parse(listViewScenes.SelectedItems[0].Tag.ToString());
                    x = listViewScenes.SelectedItems[0].SubItems[1].Text;
                    y = listViewScenes.SelectedItems[0].SubItems[2].Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid scene number selected: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Set the scene to the save file
                saveFile.scene = sceneNumber;

                //Update the x/y values displayed to the user (this should update the save on their change event)
                tbX.Text = x;
                tbY.Text = y;
            }
        }

        private void tbX_ValueChanged(object sender, EventArgs e)
        {
            if (tbX.Value > tbX.Maximum)
            {
                tbX.Value = tbX.Maximum;
            }
            else if (tbX.Value < tbX.Minimum)
            {
                tbX.Value = tbX.Minimum;
            }

            saveFile.x = ((int)tbX.Value);
        }

        private void tbY_ValueChanged(object sender, EventArgs e)
        {
            if (tbY.Value > tbY.Maximum)
            {
                tbY.Value = tbY.Maximum;
            }
            else if (tbY.Value < tbY.Minimum)
            {
                tbY.Value = tbY.Minimum;
            }

            saveFile.y = ((int)tbY.Value);
        }

        /************************************************************
         * LoadScenes
         * 
         * This function is called to load a list of scenes from a file
         * expected to be in the directory with the executeable. This
         * allows us to have the ability to save custom scene selections
         * that will preselect desired x/y coordinates.
         ************************************************************/
        private void LoadScenes()
        {
            //Ensure the list of scenes is empty before attempting to load
            listViewScenes.Clear();

            //Set up a blank list of scenes to fill with loaded data
            List<Scene> scenesList = new List<Scene>();

            //Attempt to open the Scenes.txt file we expect to use for loading the listViewScenes list with selectable presets
            try
            {
                //Store each line of the file in a string array
                string[] scenesData = File.ReadAllLines(".\\Scenes.txt");

                //For each row read in, add a new scene to the array
                foreach (string line in scenesData)
                {
                    //Split the pieces of scene data on commas
                    string[] scene = line.Split(',');

                    //Add a new scene with the line data to the scenesList
                    scenesList.Add(new Scene(scene[0], Int32.Parse(scene[1]), Int32.Parse(scene[2]), Int32.Parse(scene[3])));
                }

            }
            catch (Exception e)
            {
                //If there were any issues loading the scenes, set up the default scenes and warn the user
                scenesList = new List<Scene>(defaultScenesList);

                
                MessageBox.Show("Could not load scenes file. Loading default scenes list: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //For each scene in the array create a new item in the list view
            foreach (Scene scene in scenesList)
            {
                //Set up the item and sub items
                ListViewItem item = new ListViewItem();
                item.Text = scene.Name; //Name to display
                item.Tag = scene.ID; //Scene number to use
                item.ImageIndex = scene.ID; //Sets the preview picture to use for the area

                //Set up the X sub item
                ListViewItem.ListViewSubItem subItemX = new ListViewItem.ListViewSubItem();
                subItemX.Tag = "X";
                subItemX.Text = scene.x.ToString();
                item.SubItems.Add(subItemX);

                //Set up the Y sub item
                ListViewItem.ListViewSubItem subItemY = new ListViewItem.ListViewSubItem();
                subItemY.Tag = "Y";
                subItemY.Text = scene.y.ToString();
                item.SubItems.Add(subItemY);

                //Add the item to the list view
                listViewScenes.Items.Add(item);
            }
        }

        /************************************************************
         * SaveScenes
         * 
         * This function is called to save a list of scenes to a file
         * expected to be in the directory with the executeable. This
         * allows us to have the ability to save custom scene selections
         * that will preselect desired x/y coordinates.
         ************************************************************/
        private async void SaveScenes()
        {
            //Open a Scenes.txt file in the same directory as our program
            StreamWriter file = new StreamWriter(".\\Scenes.txt");

            //For each item in our scenes list, save a row with relevant information
            try
            {
                foreach (ListViewItem item in listViewScenes.Items)
                {
                    //Write a line to the file
                    await file.WriteLineAsync(item.Text + "," + item.Tag + "," + item.SubItems[1].Text + "," + item.SubItems[2].Text);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not save scenes file: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            file.Close();
        }

        /************************************************************
        * btnSaveAllScenes_Click
        * 
        * This function is called when the user clicks the save all
        * scenes button on the form.
        ************************************************************/
        private void btnSaveAllScenes_Click(object sender, EventArgs e)
        {
            SaveScenes();
        }

        /************************************************************
        * btnDeleteScene_Click
        * 
        * This function is called when the user clicks the delete
        * scene button on the form. This will remove the currently
        * selected scene from the loaded list (not the file).
        ************************************************************/
        private void btnDeleteScene_Click(object sender, EventArgs e)
        {
            //Ask the user to confirm deletion of the current scene selection from the list
            DialogResult result = MessageBox.Show("Are you sure you want to delete the scene \"" + listViewScenes.SelectedItems[0].Text + "\"?",
                                                "Confirm Deletion",
                                                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            //Only delete if the user has clicked OK
            if (result == DialogResult.OK)
            {
                //Remove the currently selected item from the list
                listViewScenes.Items.Remove(listViewScenes.SelectedItems[0]);
            }
        }

        /************************************************************
        * btnReloadScenes_Click
        * 
        * This function is called when the user clicks the reload
        * scenes button on the form. This will reload the scenes list
        * from the default file.
        ************************************************************/
        private void btnReloadScenes_Click(object sender, EventArgs e)
        {
            LoadScenes();
        }

        /************************************************************
        * btnAddScene_Click
        * 
        * This function is called when the user clicks the add
        * scene button on the form. This will show a popup to the user
        * for entering in the information for a new scene selection in
        * the scene list.
        ************************************************************/
        private void btnAddScene_Click(object sender, EventArgs e)
        {
            //Create a new instance of the AddSceneDialog
            var addSceneDialog = new AddSceneDialog(Cursor.Position.X, Cursor.Position.Y, defaultScenesList);

            //Show the dialog and capture its result
            var result = addSceneDialog.ShowDialog();

            //Check the result of the dialog and add the scene if it was OK
            if (result == DialogResult.OK)
            {
                //Set up the item and sub items
                ListViewItem item = new ListViewItem();
                item.Text = addSceneDialog.newScene.Name; //Name to display
                item.Tag = addSceneDialog.newScene.ID; //Scene number to use
                item.ImageIndex = Int32.Parse(addSceneDialog.newScene.ID.ToString()); //Sets the preview picture to use for the area

                //Set up the X sub item
                ListViewItem.ListViewSubItem subItemX = new ListViewItem.ListViewSubItem();
                subItemX.Tag = "X";
                subItemX.Text = addSceneDialog.newScene.x.ToString();
                item.SubItems.Add(subItemX);

                //Set up the Y sub item
                ListViewItem.ListViewSubItem subItemY = new ListViewItem.ListViewSubItem();
                subItemY.Tag = "Y";
                subItemY.Text = addSceneDialog.newScene.y.ToString();
                item.SubItems.Add(subItemY);

                //Add the item to the list view
                listViewScenes.Items.Add(item);
            }

            //Automatically save our scenes list for the user
            SaveScenes();
        }

        /************************************************************
        * btnAbout_Click
        * 
        * This function is called when the user clicks the about
        * button on the form. This will show a popup to the user
        * showing product and version information.
        ************************************************************/
        private void btnAbout_Click(object sender, EventArgs e)
        {
            //Create a new instance of the AboutBox
            var aboutDialog = new AboutBox();

            //Show the dialog and capture its result
            var result = aboutDialog.ShowDialog();
        }

        /************************************************************
        * btnSceneGuide_Click
        * 
        * This function is called when the user clicks the guide
        * button on the scenes form. This will show a popup to the user
        * showing a scenes map to use as a guide for adding new scenes.
        ************************************************************/
        private void btnSceneGuide_Click(object sender, EventArgs e)
        {
            //Create a new instance of the SceneGuide
            var sceneGuideDialog = new SceneGuideDialog();

            //Show the dialog and capture its result
            var result = sceneGuideDialog.ShowDialog();
        }
    }
}