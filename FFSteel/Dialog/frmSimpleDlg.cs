using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using mfmFFS;

namespace mfmFFS.Dialog
{
    public partial class frmSimpleDlg : RadForm
    {
        public String MsgToShow = "";
        
        public frmSimpleDlg()
        {
            InitializeComponent();
        }

        private void frmSimpleDlg_Load(object sender, EventArgs e)
        {
            lblMsg.Text = MsgToShow;
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;

        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (this.Text == "Home")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
