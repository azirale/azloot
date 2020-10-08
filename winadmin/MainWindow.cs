using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace winadmin
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region custom code
        private static readonly string defaultFileFilter = "AzLoot Config|*.azloot";
        private static string lastKnownSaveLocation = null;

        private static void CreateNew()
        {
            CoreInteractions.CreateNewConfiguration();
        }

        private static void ChooseFileLocationAndSave()
        {
            
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save AzLoot Config";
            dialog.Filter = defaultFileFilter;
            if (lastKnownSaveLocation != null) dialog.FileName = lastKnownSaveLocation;
            var result = dialog.ShowDialog();
            // bail if we didn't get a save location
            if (result == DialogResult.Cancel) return;
            if (dialog.FileName == string.Empty) return;
            // save to selected location and remember
            SaveToLocation(dialog.FileName);
            lastKnownSaveLocation = dialog.FileName;
        }

        private static void DefaultSave()
        {
            // if we opened or saved from a location then we can re-use it
            if (lastKnownSaveLocation != null) SaveToLocation(lastKnownSaveLocation);
            // otherwise we need to pick it first
            else ChooseFileLocationAndSave();
        }

        private static void SaveToLocation(string filePath)
        {
            CoreInteractions.SaveConfiguration(filePath);
        }

        private static void OpenFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Open AzLoot File";
            dialog.Filter = defaultFileFilter;
            if (lastKnownSaveLocation != null) dialog.FileName = lastKnownSaveLocation;
            var result = dialog.ShowDialog();
            // bail if we didn't get a file location
            if (result == DialogResult.Cancel) return;
            if (dialog.FileName == string.Empty) return;
            CoreInteractions.OpenConfiguration(dialog.FileName);
            lastKnownSaveLocation = dialog.FileName;
        }

        private static void SaveOnExitCheck(FormClosingEventArgs e)
        {
            // if there is no unsaved data we are ok to close
            if (!CoreInteractions.HasUnsavedData()) return;
            // check with user if they want to save before closing (or cancel closing)
            DialogResult result = MessageBox.Show("Save before exit?", "Do you want to save before exiting?", MessageBoxButtons.YesNoCancel);
            switch (result)
            {
                // cancel close event
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
                // ok means go ahead and save then let exit go ahead
                case DialogResult.Yes:
                    DefaultSave();
                    return;
            }
        }
        #endregion

        private void menuFileSaveas_Click(object sender, EventArgs e)
        {
            ChooseFileLocationAndSave();
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void menuFileNew_Click(object sender, EventArgs e)
        {
            CreateNew();
        }

        private void menuFileSave_Click(object sender, EventArgs e)
        {
            DefaultSave();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOnExitCheck(e);
        }

        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
