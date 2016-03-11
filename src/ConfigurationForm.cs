using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using deja_vu.Properties;
using deja_vu.Utilities;

namespace deja_vu
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void ConfigurationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void Button_ReplayPath_Click(object sender, EventArgs e)
        {
            var resDialog = replayBufferBrowser.ShowDialog();
            if (resDialog.ToString() == "OK")
            {
                var path = PathUtility.AddTrailingSlash(replayBufferBrowser.SelectedPath);
                Settings.Default.ReplayPath = path;
                Text_ReplayPath.Text = path;
            }
        }

        private void Button_MkvmergePath_Click(object sender, EventArgs e)
        {
            var resDialog = mkvmergeBrowser.ShowDialog();
            if (resDialog.ToString() == "OK")
            {
                MkvmergeUtility.MkvmergeLocation = mkvmergeBrowser.FileName;
                Text_MkvmergePath.Text = mkvmergeBrowser.FileName;
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.WebOverlayApiRoot = textBox1.Text;
        }

        private void Text_GfycatPostEndpoint_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
