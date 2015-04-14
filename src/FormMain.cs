using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using deja_vu.Utilities;
using deja_vu.Properties;
using System.Threading.Tasks;

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
        static List<string> _replayBuffers;
        private const string ReplayFolderPrefix = "tvr-replay-";
        private string _nextBufferPath;
        private bool _useCycle;
        private int _cycleSize;
        private int _cycleIndex = -1;

        public FrmNotifier()
        {
            InitializeComponent();
            _mSb = new StringBuilder();
            _mBDirty = false;
            _mBIsWatching = false;
            _replayBuffers = new List<string>();
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
                        Path = Settings.Default.ReplayPath,
                        IncludeSubdirectories = false,
                        NotifyFilter = NotifyFilters.LastWrite
                                       | NotifyFilters.FileName | NotifyFilters.DirectoryName
                    };

                //_mWatcher.Changed += OnChanged;
                _mWatcher.Created += OnCreated;
                //_mWatcher.Deleted += OnChanged;
                //_mWatcher.Renamed += OnRenamed;
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
                Trace.TraceInformation(String.Format("Picked up a file creation event: {0}", e.FullPath));
                _nextBufferPath = e.FullPath;
                MapBufferToReplay(e.FullPath);
                _mBDirty = true;
            }
        }

        private void CreateCurrentReplayFolderIfNecessary()
        {
            //Create folder for currently chosen replay
            //Must always be active as this is the main file
            var replayPath = Settings.Default.ReplayPath + ReplayFolderPrefix + "current";

            if (!Directory.Exists(replayPath))
            {
                Directory.CreateDirectory(replayPath);
            }
        }

        private string GetCurrentReplayFolder()
        {
            var replayPath = Settings.Default.ReplayPath + ReplayFolderPrefix + "current";
            return replayPath;
        }

        private string GetNextBufferFileExtension()
        {
            return _nextBufferPath == null ? "" : Path.GetExtension(_nextBufferPath);
        }

        private void MapBufferToReplay(string dir)
        {
            var replayPath = "";

            //Boundless Replays
            if (!_useCycle)
            {
                var replayIndex = _replayBuffers.Count;
                replayPath = dir.Substring(0, dir.LastIndexOf("\\", StringComparison.Ordinal)) + "\\" +
                                ReplayFolderPrefix + replayIndex;
            }
            //Circle around to maintain a max number of replays.
            else
            {
                _cycleIndex++;
                if (_cycleIndex >= _cycleSize)
                {
                    _cycleIndex = 0;
                }

                replayPath = dir.Substring(0, dir.LastIndexOf("\\", StringComparison.Ordinal)) + "\\" +
                                ReplayFolderPrefix + _cycleIndex;
            }


            if (!Directory.Exists(replayPath))
            {
                Directory.CreateDirectory(replayPath);
                //_mSb.AppendLine("Created new slot " + replayIndex+". ");
            }

            CreateCurrentReplayFolderIfNecessary();

            if ((_useCycle && _replayBuffers.Count < _cycleSize) || !_useCycle)
                _replayBuffers.Add(replayPath);

            //Copy and move the buffer into two replay folders
            //The tail of the list, and the current
            try
            {
                FileUtility.OnceDoneWriting(_nextBufferPath, (f) =>
                {
                    var nextReplaySlot = Path.Combine(replayPath, "replay" + GetNextBufferFileExtension());
                    var currentReplay = Path.Combine(GetCurrentReplayFolder(), "replay" + GetNextBufferFileExtension());
                    Trace.TraceInformation(String.Format("Copying {0} to {1}...", _nextBufferPath, nextReplaySlot));
                    using (var result = File.Create(nextReplaySlot))
                    {
                        f.CopyTo(result);
                    }
                    Trace.TraceInformation(String.Format("File copied to {0}", nextReplaySlot));
                });

                var nextReplaySlotSlow = Path.Combine(replayPath, "replay-slow.mkv");
                Trace.TraceInformation(String.Format("Starting to process {0} to {1}", _nextBufferPath, nextReplaySlotSlow));
                MkvmergeUtility.StretchMp4(_nextBufferPath, nextReplaySlotSlow, GetVideoRate(), (object mkvSender, EventArgs mkvArgs) =>
                {
                    Trace.TraceInformation(String.Format("Finished processing {0}", nextReplaySlotSlow));
                });
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while moving replays to a different slot", new { Error = ex });
                Console.WriteLine("We tried to access a deleted file?");
            }


            UpdateListBoxWithReplays();
            //_mSb.Append(DateTime.Now);
        }

        private delegate void AddListBoxItemDelegate(object item);

        private void AddListBoxItem(object item)
        {
            if (listBox1.InvokeRequired)
            {
                // This is a worker thread to delegate the task.
                listBox1.Invoke(new AddListBoxItemDelegate(AddListBoxItem), item);
            }
            else
            {
                // This is the UI thread to perform the task.
                listBox1.Items.Insert(0, item);
            }
        }

        private void UpdateListBoxWithReplays()
        {
            //Add most recent to listbox
            if ((_useCycle && _replayBuffers.Count <= _cycleSize))
            {
                AddListBoxItem(_replayBuffers[_cycleIndex]);
            }
            else
            {
                AddListBoxItem(_replayBuffers[_replayBuffers.Count - 1]);
            }

            //Set the most recent as the selected item
            listBox1.Invoke(() => listBox1.SetSelected(0, true));
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
                lstNotification.Items.Insert(0, _mSb.ToString());
                lstNotification.EndUpdate();
                _mBDirty = false;
            }
        }

        private void FrmNotifier_Load(object sender, EventArgs e)
        {

        }

        private void lstNotification_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Clear Button
        private void button1_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < _replayBuffers.Count; i++)
            {
                var replayPath = Settings.Default.ReplayPath + ReplayFolderPrefix + i;
                Directory.Delete(replayPath, true);
            }

            //Empty the replay buffers
            _replayBuffers.Clear();

            //Reset listbox
            listBox1.Invoke(() => listBox1.Items.Clear());

            //Inform User
            _mSb.Remove(0, _mSb.Length);
            _mSb.AppendLine("Cleared all slots. ");
            _mSb.Append(DateTime.Now);
            _mBDirty = true;
        }

        //Current Replay Slot
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get which slot is selected
            var senderAsListControl = (ListBox)sender;
            var newSlot =
                senderAsListControl.SelectedItem.ToString()
                                   .Substring(senderAsListControl.SelectedItem.ToString().LastIndexOf("-") + 1);
            var replaySlot = Settings.Default.ReplayPath + ReplayFolderPrefix + newSlot;
            var replayFile = Path.Combine(replaySlot, "replay" + GetNextBufferFileExtension());

            //Copy replay to current slot
            try
            {
                FileUtility.OnceDoneWriting(replayFile, (f) =>
                {
                    var newFile = Path.Combine(GetCurrentReplayFolder(), "replay" + GetNextBufferFileExtension());
                    Trace.TraceInformation(String.Format("Copying {0} to {1}...", replayFile, newFile));
                    using (var result = File.Create(newFile))
                    {
                        f.CopyTo(result);
                    }
                    Trace.TraceInformation(String.Format("File copied to {0}", newFile));
                });

                var slowFile = Path.Combine(GetCurrentReplayFolder(), "replay-slow.mkv");
                Trace.TraceInformation(String.Format("Starting to process {0} to {1}", replayFile, slowFile));
                MkvmergeUtility.StretchMp4(replayFile, slowFile, GetVideoRate(), (object mkvSender, EventArgs mkvArgs) =>
                {
                    Trace.TraceInformation(String.Format("Finished processing {0}", slowFile));
                });

                _mSb.Remove(0, _mSb.Length);
                _mSb.AppendLine("Switched to slot " + newSlot + ". " + _replayBuffers.Count + " slots available. ");
                _mSb.Append(DateTime.Now);
                _mBDirty = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while copying replay to current slot", new { Error = ex });
                Console.WriteLine(ex.Message);
                _mSb.Remove(0, _mSb.Length);
                _mSb.AppendLine("Error. Could not find valid replay file in slot " + newSlot + ". ");
                _mSb.Append(DateTime.Now);
                _mBDirty = true;
            }

        }

        //Use Cycling Checkbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var buttonBase = (CheckBox)sender;
            numericUpDown1.Enabled = buttonBase.CheckState == CheckState.Checked;
            _useCycle = numericUpDown1.Enabled;

        }

        //Replay Cycles
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var upDown = (NumericUpDown)sender;
            _cycleSize = Decimal.ToInt32(upDown.Value);
        }

        private void MenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuItem_Settings_Click(object sender, EventArgs e)
        {
            var form = new ConfigurationForm();
            form.ShowDialog(this);
        }

        private decimal GetVideoRate()
        {
            return 1 / (Settings.Default.SlowReplaySpeed / 100);
        }

        private async void Button_Gfycat_Click(object sender, EventArgs e)
        {
            var oldText = Button_Gfycat.Text;
            Button_Gfycat.Enabled = false;
            Button_Gfycat.Text = "Uploading...";

            var result = await GfycatUtility.Upload(Path.Combine(GetCurrentReplayFolder(), "replay" + GetNextBufferFileExtension()));
            if (Check_SendToBot.Checked && result.Status == GfycatStatus.Complete)
            {
                GfycatUtility.PostUrl(Settings.Default.GfycatPostEndpoint, result.Url);
            }
            var gfycatForm = new GfycatResultForm(result);
            gfycatForm.ShowDialog();

            Button_Gfycat.Text = oldText;
            Button_Gfycat.Enabled = true;
        }
    }
}