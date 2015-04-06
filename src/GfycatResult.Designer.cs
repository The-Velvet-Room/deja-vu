namespace deja_vu
{
    partial class GfycatResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GfycatResultForm));
            this.Button_OK = new System.Windows.Forms.Button();
            this.Button_Copy = new System.Windows.Forms.Button();
            this.Label_Message = new System.Windows.Forms.Label();
            this.Link_Gfycat = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // Button_OK
            // 
            this.Button_OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_OK.Location = new System.Drawing.Point(217, 72);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 23);
            this.Button_OK.TabIndex = 0;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // Button_Copy
            // 
            this.Button_Copy.Location = new System.Drawing.Point(136, 72);
            this.Button_Copy.Name = "Button_Copy";
            this.Button_Copy.Size = new System.Drawing.Size(75, 23);
            this.Button_Copy.TabIndex = 1;
            this.Button_Copy.Text = "Copy URL";
            this.Button_Copy.UseVisualStyleBackColor = true;
            this.Button_Copy.Click += new System.EventHandler(this.Button_Copy_Click);
            // 
            // Label_Message
            // 
            this.Label_Message.Location = new System.Drawing.Point(12, 9);
            this.Label_Message.Name = "Label_Message";
            this.Label_Message.Size = new System.Drawing.Size(280, 43);
            this.Label_Message.TabIndex = 2;
            this.Label_Message.Text = "Works";
            this.Label_Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Link_Gfycat
            // 
            this.Link_Gfycat.Location = new System.Drawing.Point(12, 52);
            this.Link_Gfycat.Name = "Link_Gfycat";
            this.Link_Gfycat.Size = new System.Drawing.Size(280, 17);
            this.Link_Gfycat.TabIndex = 3;
            this.Link_Gfycat.TabStop = true;
            this.Link_Gfycat.Text = "linkLabel1";
            this.Link_Gfycat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GfycatResultForm
            // 
            this.AcceptButton = this.Button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Button_OK;
            this.ClientSize = new System.Drawing.Size(304, 107);
            this.Controls.Add(this.Link_Gfycat);
            this.Controls.Add(this.Label_Message);
            this.Controls.Add(this.Button_Copy);
            this.Controls.Add(this.Button_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GfycatResultForm";
            this.Text = "GfycatResult";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Button Button_Copy;
        private System.Windows.Forms.Label Label_Message;
        private System.Windows.Forms.LinkLabel Link_Gfycat;
    }
}