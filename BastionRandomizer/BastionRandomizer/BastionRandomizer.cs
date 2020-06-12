﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using BastionRandomizer.Properties;

namespace WindowsFormsApp1
{
    public partial class BastionRandomizer : Form
    {
        int seed;
        Random rand;
        Randomizer randomizer;
        string folderPath;

        public BastionRandomizer()
        {
            InitializeComponent();
            
            randomizer = new Randomizer();
        }

        private void BastionRandomizer_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

            if (Settings.Default.SavedFolder == null)
            {
                folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = Settings.Default.SavedFolder;
            }

            PathTextBox.Text = folderBrowserDialog1.SelectedPath;
            folderPath = PathTextBox.Text;

            WeaponComboBox.Text = "Fully Random";
            
            randomizer.randomizeLevels = RandomizeLevelOrder.Checked;
            randomizer.randomizeWeapons = RandomizeWeapons.Checked;
            randomizer.noCutscenes = RemoveCutscenes.Checked;
            randomizer.noHub = RemoveHub.Checked;
        }

        // Levels
        private void RandomizeLevelOrder_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.randomizeLevels = RandomizeLevelOrder.Checked;
        }

        // Weapons
        private void RandomizeWeapons_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.randomizeWeapons = RandomizeWeapons.Checked;
            WeaponComboBox.Enabled = RandomizeWeapons.Checked;
        }

        private void WeaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(WeaponComboBox.Text == "Fully Randomized")
            {
                randomizer.weaponRandomizationType = WeaponRandomization.FullyRandom;
            }
            else if (WeaponComboBox.Text == "Weapons & Abilities Split")
            {
                randomizer.weaponRandomizationType = WeaponRandomization.WeaponAbilitySplit;
            }
        }

        // Cutscenes
        private void RemoveCutscenes_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.noCutscenes = RemoveCutscenes.Checked;
        }

        // No Hub
        private void RemoveHub_CheckedChanged(object sender, EventArgs e)
        {
            randomizer.noHub = RemoveHub.Checked;
        }

        // Set Seed
        private void SeedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SeedTextBox.Text != "")
                seed = int.Parse(SeedTextBox.Text);
        }

        private void SeedTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Randomize
        private void Randomize_Click(object sender, EventArgs e)
        {
            if (randomizer.randomizeLevels == false && randomizer.randomizeWeapons == false)
                return;

            if (SeedTextBox.Text == "")
                rand = new Random();
            else
                rand = new Random(seed);

            randomizer.BackupFiles(folderPath);

            if (randomizer.randomizeLevels)
                randomizer.RandomizeLevelOrder(rand, folderPath);

            if (randomizer.randomizeWeapons)
                randomizer.RandomizeWeapons(rand, folderPath);

            randomizer.EditLevelScripts(folderPath);
        }

        // Unrandomize
        private void Unrandomize_Click(object sender, EventArgs e)
        {
            randomizer.RestoreFiles(folderPath);
        }

        // Game Data Folder
        private void FolderSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.SavedFolder = folderBrowserDialog1.SelectedPath;
                PathTextBox.Text = folderBrowserDialog1.SelectedPath;
                folderPath = PathTextBox.Text;
            }
        }

        private void PathTextBox_TextChanged(object sender, EventArgs e)
        {
            folderPath = PathTextBox.Text;
        }

    }
}
