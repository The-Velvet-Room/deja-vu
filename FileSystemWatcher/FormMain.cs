using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace deja_vu
{
    public partial class FrmNotifier : Form
    {
        private readonly StringBuilder _mSb;
        private bool _mBDirty;
        private System.IO.FileSystemWatcher _mWatcher;
        private bool _mBIsWatching;

        public FrmNotifier()
        {
            InitializeComponent();
            _mSb = new StringBuilder();
            _mBDirty = false;
            _mBIsWatching = false;
        }

        private void btnWatchFile_Click(object sender, EventArgs e)
        {
            if (_mBIsWatching)
            {
                _mBIsWatching = false;
                _mWatcher.EnableRaisingEvents = false;
                _mWatcher.Dispose();
                btnWatchFile.BackColor = Color.LightSkyBlue;
                btnWatchFile.Text = "Start Watching";
                
            }
            else
            {
                _mBIsWatching = true;
                btnWatchFile.BackColor = Color.Red;
                btnWatchFile.Text = "Stop Watching";

                _mWatcher = new System.IO.FileSystemWatcher
                    {
                        Filter = "*.*",
                        Path = txtFile.Text + "\\",
                        IncludeSubdirectories = true,
                        NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                       | NotifyFilters.FileName | NotifyFilters.DirectoryName
                    };


                _mWatcher.Changed += OnChanged;
                _mWatcher.Created += OnChanged;
                _mWatcher.Deleted += OnChanged;
                _mWatcher.Renamed += OnRenamed;
                _mWatcher.EnableRaisingEvents = true;
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!_mBDirty)
            {
                _mSb.Remove(0, _mSb.Length);
                _mSb.Append(e.FullPath);
                _mSb.Append(" ");
                _mSb.Append(e.ChangeType);
                _mSb.Append("    ");
                _mSb.Append(DateTime.Now);
                _mBDirty = true;
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (!_mBDirty)
            {
                _mSb.Remove(0, _mSb.Length);
                _mSb.Append(e.OldFullPath);
                _mSb.Append(" ");
                _mSb.Append(e.ChangeType);
                _mSb.Append(" ");
                _mSb.Append("to ");
                _mSb.Append(e.Name);
                _mSb.Append("    ");
                _mSb.Append(DateTime.Now);
                _mBDirty = true;
            }            
        }

        private void tmrEditNotify_Tick(object sender, EventArgs e)
        {
            if (_mBDirty)
            {
                lstNotification.BeginUpdate();
                //lstNotification.Items.Add(_mSb.ToString());
                lstNotification.Items.Insert(0, _mSb.ToString());
                lstNotification.EndUpdate();
                _mBDirty = false;
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            var resDialog = dlgOpenDir.ShowDialog();
            if (resDialog.ToString() == "OK")
            {
                txtFile.Text = dlgOpenDir.SelectedPath;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FrmNotifier_Load(object sender, EventArgs e)
        {

        }
    }
}