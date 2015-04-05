using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using deja_vu.Utilities;

namespace deja_vu
{
    public partial class GfycatResultForm : Form
    {
        public GfycatResult Result { get; set; }

        public GfycatResultForm(GfycatResult result)
        {
            InitializeComponent();
            this.Result = result;
            if (Result.Status == GfycatStatus.Complete)
            {
                Text = "Gfycat Upload Complete!";
                Label_Message.Text = "Your gfycat is available at:";
                Link_Gfycat.Text = Result.Url;
                Link_Gfycat.Show();
                Button_Copy.Show();
            }
            if (Result.Status == GfycatStatus.Error)
            {
                Text = "Gfycat Error";
                Label_Message.Text = Result.ErrorMessage;
                Link_Gfycat.Hide();
                Button_Copy.Hide();
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.Result.Url);
        }
    }
}
