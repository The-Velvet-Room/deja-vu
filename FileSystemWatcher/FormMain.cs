using System;
using System.Collections.Generic;
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
        
        //List of replay buffers
        //Using a List allows n buffers
        static List<string> replayBuffers;
        private const string ReplayFolderPrefix = "tvr-replay-";
        private string _nextBufferPath;

        public FrmNotifier()
        {
            InitializeComponent();
            _mSb = new StringBuilder();
            _mBDirty = false;
            _mBIsWatching = false;
            replayBuffers = new List<string>();
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
                _mWatcher.Created += OnCreated;
                _mWatcher.Deleted += OnChanged;
                _mWatcher.Renamed += OnRenamed;
                _mWatcher.EnableRaisingEvents = true;

                CreateCurrentReplayFolderIfNecessary();
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (!_mBDirty)
            {
                _mSb.Remove(0, _mSb.Length);
                //Skip directories with our file prefix
                if (e.FullPath.Contains(ReplayFolderPrefix))
                {
                    return;
                }                

                _mSb.Append("New raw buffer at "+e.FullPath);
                _nextBufferPath = e.FullPath;
                MapBufferToReplay(e.FullPath);
                _mBDirty = true;               
            }
        }

        private void CreateCurrentReplayFolderIfNecessary()
        {
            //Create folder for currently chosen replay
            //Must always be active as this is the main file
            var replayPath = txtFile.Text + "\\" + ReplayFolderPrefix + "current";

            if (!Directory.Exists(replayPath))
            {
                Directory.CreateDirectory(replayPath);
            }
        }

        private string GetCurrentReplayFolder()
        {
            var replayPath = txtFile.Text + "\\" + ReplayFolderPrefix + "current";
            return replayPath;
        }

        private string GetNextBufferFileExtension()
        {
            return _nextBufferPath.Substring(_nextBufferPath.LastIndexOf("."));
        }

        private void MapBufferToReplay(string dir)
        {
            var replayIndex = replayBuffers.Count;
            var replayPath = dir.Substring(0, dir.LastIndexOf("\\", StringComparison.Ordinal)) + "\\" + ReplayFolderPrefix + replayIndex;

            if (!Directory.Exists(replayPath))
            {
                Directory.CreateDirectory(replayPath);
                _mSb.Append("Created new replay slot " + replayIndex);
            }

            CreateCurrentReplayFolderIfNecessary();            
            replayBuffers.Add(replayPath);

            //Copy and move the buffer into two replay folders
            //The tail of the list, and the current
            File.Copy(_nextBufferPath, replayPath+"\\"+"replay" + GetNextBufferFileExtension(), true);
            _mSb.Append("Wrote replay to slot " + replayIndex);
            File.Copy(_nextBufferPath, GetCurrentReplayFolder() + "\\" + "replay" + GetNextBufferFileExtension(), true);
            _mSb.Append("Overwrote main replay slot");
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!_mBDirty)
            {
                /*_mSb.Remove(0, _mSb.Length);
                _mSb.Append(e.FullPath);
                _mSb.Append(" ");
                _mSb.Append(e.ChangeType);
                _mSb.Append("    ");
                _mSb.Append(DateTime.Now);
                _mBDirty = true;*/
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

        private void lstNotification_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Empty the replay buffers
            replayBuffers.Clear();

            //TODO Delete files in the buffer folders
        }
    }
}