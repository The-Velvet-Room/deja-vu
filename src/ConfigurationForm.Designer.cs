namespace deja_vu
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.replayBufferBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.Button_ReplayPath = new System.Windows.Forms.Button();
            this.Text_ReplayPath = new System.Windows.Forms.TextBox();
            this.Label_ReplayPath = new System.Windows.Forms.Label();
            this.mkvmergeBrowser = new System.Windows.Forms.OpenFileDialog();
            this.Label_MkvmergePath = new System.Windows.Forms.Label();
            this.Text_MkvmergePath = new System.Windows.Forms.TextBox();
            this.Button_MkvmergePath = new System.Windows.Forms.Button();
            this.Button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // replayBufferBrowser
            // 
            this.replayBufferBrowser.SelectedPath = global::deja_vu.Properties.Settings.Default.ReplayPath;
            // 
            // Button_ReplayPath
            // 
            this.Button_ReplayPath.Location = new System.Drawing.Point(214, 42);
            this.Button_ReplayPath.Name = "Button_ReplayPath";
            this.Button_ReplayPath.Size = new System.Drawing.Size(75, 22);
            this.Button_ReplayPath.TabIndex = 1;
            this.Button_ReplayPath.Text = "Browse";
            this.Button_ReplayPath.UseVisualStyleBackColor = true;
            this.Button_ReplayPath.Click += new System.EventHandler(this.Button_ReplayPath_Click);
            // 
            // Text_ReplayPath
            // 
            this.Text_ReplayPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::deja_vu.Properties.Settings.Default, "ReplayPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Text_ReplayPath.Location = new System.Drawing.Point(12, 43);
            this.Text_ReplayPath.Name = "Text_ReplayPath";
            this.Text_ReplayPath.Size = new System.Drawing.Size(196, 20);
            this.Text_ReplayPath.TabIndex = 0;
            this.Text_ReplayPath.Text = global::deja_vu.Properties.Settings.Default.ReplayPath;
            // 
            // Label_ReplayPath
            // 
            this.Label_ReplayPath.AutoSize = true;
            this.Label_ReplayPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_ReplayPath.Location = new System.Drawing.Point(9, 26);
            this.Label_ReplayPath.Name = "Label_ReplayPath";
            this.Label_ReplayPath.Size = new System.Drawing.Size(277, 13);
            this.Label_ReplayPath.TabIndex = 2;
            this.Label_ReplayPath.Text = "Replay Buffer Directory (OBS should write here)";
            // 
            // mkvmergeBrowser
            // 
            this.mkvmergeBrowser.FileName = global::deja_vu.Properties.Settings.Default.MkvmergePath;
            this.mkvmergeBrowser.Filter = "mkvmerge|mkvmerge.exe";
            // 
            // Label_MkvmergePath
            // 
            this.Label_MkvmergePath.AutoSize = true;
            this.Label_MkvmergePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_MkvmergePath.Location = new System.Drawing.Point(12, 124);
            this.Label_MkvmergePath.Name = "Label_MkvmergePath";
            this.Label_MkvmergePath.Size = new System.Drawing.Size(141, 13);
            this.Label_MkvmergePath.TabIndex = 3;
            this.Label_MkvmergePath.Text = "mkvmerge.exe Location";
            // 
            // Text_MkvmergePath
            // 
            this.Text_MkvmergePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::deja_vu.Properties.Settings.Default, "MkvmergePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Text_MkvmergePath.Location = new System.Drawing.Point(15, 140);
            this.Text_MkvmergePath.Name = "Text_MkvmergePath";
            this.Text_MkvmergePath.Size = new System.Drawing.Size(193, 20);
            this.Text_MkvmergePath.TabIndex = 4;
            this.Text_MkvmergePath.Text = global::deja_vu.Properties.Settings.Default.MkvmergePath;
            // 
            // Button_MkvmergePath
            // 
            this.Button_MkvmergePath.Location = new System.Drawing.Point(214, 139);
            this.Button_MkvmergePath.Name = "Button_MkvmergePath";
            this.Button_MkvmergePath.Size = new System.Drawing.Size(75, 22);
            this.Button_MkvmergePath.TabIndex = 5;
            this.Button_MkvmergePath.Text = "Browse";
            this.Button_MkvmergePath.UseVisualStyleBackColor = true;
            this.Button_MkvmergePath.Click += new System.EventHandler(this.Button_MkvmergePath_Click);
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(214, 226);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 23);
            this.Button_OK.TabIndex = 6;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 261);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Button_MkvmergePath);
            this.Controls.Add(this.Text_MkvmergePath);
            this.Controls.Add(this.Label_MkvmergePath);
            this.Controls.Add(this.Label_ReplayPath);
            this.Controls.Add(this.Button_ReplayPath);
            this.Controls.Add(this.Text_ReplayPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigurationForm";
            this.Text = "Settings...";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigurationForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_ReplayPath;
        private System.Windows.Forms.FolderBrowserDialog replayBufferBrowser;
        private System.Windows.Forms.Button Button_ReplayPath;
        private System.Windows.Forms.Label Label_ReplayPath;
        private System.Windows.Forms.OpenFileDialog mkvmergeBrowser;
        private System.Windows.Forms.Label Label_MkvmergePath;
        private System.Windows.Forms.TextBox Text_MkvmergePath;
        private System.Windows.Forms.Button Button_MkvmergePath;
        private System.Windows.Forms.Button Button_OK;
    }
}