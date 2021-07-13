using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using mfmWeighment;
using mfmFFSDB;

namespace mfmFFS
{
    public partial class frmLogin : RadForm
    {

        #region Variable

        Boolean flgAllGood = false;

        #endregion

        #region Functions

        private void GetAppSettings()
        {
            try
            {
                if (!String.IsNullOrEmpty(APPSetting.Default.cfgSQLServer))
                {
                    txtSQLServerName.Text = APPSetting.Default.cfgSQLServer;
                    flgAllGood = true;
                }
                if (!String.IsNullOrEmpty(APPSetting.Default.cfgSQLUser))
                {
                    txtSQLUserID.Text = APPSetting.Default.cfgSQLUser;
                }
                else
                {
                    flgAllGood = false;
                }

                if (!String.IsNullOrEmpty(APPSetting.Default.cfgSapID))
                {
                    txtSapID.Text = APPSetting.Default.cfgSapID;
                    flgAllGood = true;
                }
                if (!String.IsNullOrEmpty(APPSetting.Default.cfgSapPw))
                {
                    txtSapPw.Text = APPSetting.Default.cfgSapPw;
                }
                else
                {
                    flgAllGood = false;
                }

                if (!String.IsNullOrEmpty(APPSetting.Default.cfgSQLPassword))
                {
                    txtSQLPassword.Text = APPSetting.Default.cfgSQLPassword;
                }
                else
                {
                    flgAllGood = false;
                }
                if (!String.IsNullOrEmpty(APPSetting.Default.cfgAPPDB))
                {
                    txtAppDBName.Text = APPSetting.Default.cfgAPPDB;
                }
                else
                {
                    flgAllGood = false;
                }
                if (!String.IsNullOrEmpty(APPSetting.Default.cfgSBODB))
                {
                    txtSBODBName.Text = APPSetting.Default.cfgSBODB;
                }
                else
                {
                    flgAllGood = false;
                }
                if (!flgAllGood) return;
                Program.ConStrApp = "Server=" + txtSQLServerName.Text + ";Database=" + txtAppDBName.Text + ";User ID=" + txtSQLUserID.Text + ";Password=" + txtSQLPassword.Text + " ;Trusted_Connection=False";
                Program.ConStrSAP = "Server=" + txtSQLServerName.Text + ";Database=" + txtSBODBName.Text + ";User ID=" + txtSQLUserID.Text + ";Password=" + txtSQLPassword.Text + " ;Trusted_Connection=False";
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show("GetAppSettings Exception : " + Ex.Message);
            }
        }

        private void SetAppSettings()
        {
            try
            {
                APPSetting.Default.cfgAPPDB = txtAppDBName.Text;
                APPSetting.Default.cfgSBODB = txtSBODBName.Text;
                APPSetting.Default.cfgSQLServer = txtSQLServerName.Text;
                APPSetting.Default.cfgSQLUser = txtSQLUserID.Text;
                APPSetting.Default.cfgSQLPassword = txtSQLPassword.Text;
                APPSetting.Default.cfgSapID = txtSapID.Text;
                APPSetting.Default.cfgSapPw = txtSapPw.Text;
                APPSetting.Default.Save();
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show("SetAppSettings Exception : " + Ex.Message);
            }
        }

        private void Login()
        {
            try
            {
                SetAppSettings();
                GetAppSettings();
                //if (frmMain.LoginSBO())
                //{
                String UserCode = "", Password = "";

                Boolean flgLoginStatus = false;
                if (btnDetail.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
                {
                    dbFFS oDB = new dbFFS(Program.ConStrApp);
                    UserCode = txtUserID.Text;
                    Password = txtPassword.Text;

                    MstUsers oUser = (from a in oDB.MstUsers
                                      where a.UserCode == UserCode && a.Password == Password
                                      select a).FirstOrDefault();
                    if (oUser != null)
                    {
                        flgLoginStatus = true;
                        Program.LoggedInUser = oUser.UserCode;
                    }
                    else
                    {
                        flgLoginStatus = false;
                    }
                }
                else
                {
                    dbFFS oDB = new dbFFS(Program.ConStrApp);
                    UserCode = txtUserID2.Text;
                    Password = txtPassword2.Text;
                    MstUsers oUser = (from a in oDB.MstUsers
                                      where a.UserCode == UserCode && a.Password == Password
                                      select a).FirstOrDefault();
                    if (oUser != null)
                    {
                        flgLoginStatus = true;
                        Program.LoggedInUser = oUser.UserCode;
                    }
                    else
                    {
                        flgLoginStatus = false;
                    }
                }

                if (flgLoginStatus)
                {
                    //frmMain oForm = new frmMain();
                    //oForm.Show();
                    //this.Hide();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    lblLoginStatus.Text = "Incorrect Login ID or Password";
                }
                //}
                //else
                //{
                //    RadMessageBox.Show(" SBO not Connected ");
                //}
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show(" Exception : " + Ex.Message);
            }
        }

        private void CloseApp()
        {
            try
            {
                Application.Exit();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("CloseApp Exception : " + Ex.Message);
            }
        }

        #endregion

        #region Form Events

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnDetail_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (btnDetail.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                pnlDetail.Visible = true;
                pnlSummery.Visible = false;
                this.AcceptButton = btnLogin;
                this.CancelButton = btnCancel;
            }
            else
            {
                pnlDetail.Visible = false;
                pnlSummery.Visible = true;
                this.AcceptButton = btnLogin2;
                this.CancelButton = btnCancel2;
                pnlSummery.BringToFront();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            lblLoginStatus.Text = "";
            GetAppSettings();
            //if (frmMain.LoginSBO())
            //{
            if (flgAllGood)
            {
                btnDetail.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
                this.AcceptButton = btnLogin;
                this.CancelButton = btnCancel;
            }
            else
            {
                btnDetail.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
                this.AcceptButton = btnLogin2;
                this.CancelButton = btnCancel2;
            }
            if (btnDetail.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                pnlDetail.Visible = true;
                pnlSummery.Visible = false;
            }
            else
            {
                pnlDetail.Visible = false;
                pnlSummery.Visible = true;
            }
            if (Environment.MachineName.ToUpper() == "DESKTOP-ACKBHIM")
            //if (Environment.MachineName.ToUpper() == "FAIZANANWAR-PC")
            {
                txtUserID2.Text = "manager";
                txtPassword2.Text = "super";

                //txtUserID2.Text = "Role3";
                //txtPassword2.Text = "mfm";
            }
        }

        private void btnLogin2_Click(object sender, EventArgs e)
        {
            try
            {
                Login();
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show(" Exception : " + Ex.Message);
            }
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            try
            {
                CloseApp();
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show(" Exception : " + Ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Login();
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show(" Exception : " + Ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CloseApp();
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show(" Exception : " + Ex.Message);
            }
        }

        #endregion

    }
}
