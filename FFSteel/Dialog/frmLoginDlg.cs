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
using mfmFFSDB;


namespace mfmFFS.Dialog
{

    public partial class frmLoginDlg : RadForm
    {
        #region variable

        dbFFS oDB = new dbFFS(Program.ConStrApp);
        public string ToleranceApprovedBy = string.Empty;
        public bool flgChkStatus = false;
        public bool lblToleranceLimit = false;
        public bool txtToleranceLimitVisible = false;

        #endregion

        #region function

        public frmLoginDlg()
        {
            InitializeComponent();
        }

        private void ChkAdministratorUser()
        {
            try
            {
                String UserCode = "", Password = "";
                UserCode = txtUserID.Text;
                Password = txtPassword.Text;
                MstUsers oUser = (from a in oDB.MstUsers
                                  where a.UserCode == UserCode && a.Password == Password && a.FlgTolerance == true
                                  select a).FirstOrDefault();
                if (oUser != null)
                {
                    ToleranceApprovedBy = oUser.UserCode;
                    lblToleranceLimit = true;
                    txtToleranceLimitVisible = true;
                    flgChkStatus = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    lblLoginStatus.Text = "You cannot authorized user.";
                    flgChkStatus = false;
                }

            }
            catch (Exception Ex)
            {
                RadMessageBox.Show(" Exception : " + Ex.Message);
                flgChkStatus = false;
            }
        }

        #endregion

        #region Event

        private void frmSimpleDlg_Load(object sender, EventArgs e)
        {
            
        }
        
        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            ChkAdministratorUser();
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            lblLoginStatus.Text = string.Empty;
            txtUserID.Text = string.Empty;
            txtPassword.Text = string.Empty;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        #endregion
    }
}
