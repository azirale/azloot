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

        private void menuFileNew_Click(object sender, EventArgs e)
        {
            CoreInteractions.CreateNewConfiguration();
        }

        private void menuFileSave_Click(object sender, EventArgs e)
        {
            // if we opened or saved from a location then we can re-use it
            if (lastKnownSaveLocation != null) SaveToLocation(lastKnownSaveLocation);
            // otherwise we need to pick it first
            else ChooseFileLocation();
        }

        #region custom code
        private static string lastKnownSaveLocation = null;

        private static void ChooseFileLocation()
        {
            
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "AzLoot Config|*.azloot";
            dialog.Title = "Save AzLoot Config";
            if (lastKnownSaveLocation != null) dialog.FileName = lastKnownSaveLocation;
            dialog.ShowDialog();
            // bail if we didn't get a save location
            if (dialog.FileName == string.Empty) return;
            // save to selected location and remember
            SaveToLocation(dialog.FileName);
            lastKnownSaveLocation = dialog.FileName;
        }

        private static void SaveToLocation(string filePath)
        {
            CoreInteractions.SaveConfiguration(filePath);
        }
        #endregion
    }
}
