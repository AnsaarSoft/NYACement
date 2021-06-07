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
using DevComponents.WinForms;
using DevComponents.DotNetBar;
using System.Threading;
using mfmFFSDB;
using UFFU;
using SAPbobsCOM;
using mfmWeighment;


namespace mfmFFS
{
    public partial class frmMain : RadForm
    {

        #region Variable

        public string MsgType
        {
            set
            {
                if (value == "Exception")
                {
                    lblAppStatus.BackColor = System.Drawing.Color.White;
                    lblAppStatus.ForeColor = System.Drawing.Color.Red;
                }
                if (value == "Success")
                {
                    lblAppStatus.BackColor = System.Drawing.Color.White;
                    lblAppStatus.ForeColor = System.Drawing.Color.Green;
                }
                if (value == "Warning")
                {
                    lblAppStatus.BackColor = System.Drawing.Color.White;
                    lblAppStatus.ForeColor = System.Drawing.Color.BlueViolet;
                }
            }
        }

        public static bool LoginSBO()
        {

            string retvalue = "";
            try
            {
                Program.SapCompany = new Company();
                if (Program.SapCompany != null && !Program.isSapConnected)
                {
                    if (Program.SapCompany.Connected)
                    {
                        Program.SapCompany.Disconnect();
                        //return false;
                    }
                    else
                    {
                        Program.SapCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                        Program.SapCompany.Server = APPSetting.Default.cfgSQLServer;
                        Program.SapCompany.CompanyDB = APPSetting.Default.cfgSBODB;
                        Program.SapCompany.UserName = APPSetting.Default.cfgSapID;
                        Program.SapCompany.Password = APPSetting.Default.cfgSapPw;
                        Program.SapCompany.DbUserName = APPSetting.Default.cfgSQLUser;
                        Program.SapCompany.DbPassword = APPSetting.Default.cfgSQLPassword;
                        //Program.SapCompany.UseTrusted = true;
                        //Program.SapCompany.LicenseServer = APPSetting.Default.cfgSQLServer + ":40000";
                        Program.oErrMgn.LogEntry(Program.ANV, $"server name: {APPSetting.Default.cfgSQLServer} dbname: {APPSetting.Default.cfgSBODB} sapuser: {APPSetting.Default.cfgSapID} sappas: {APPSetting.Default.cfgSapPw} sqluserid: {APPSetting.Default.cfgSQLUser} sqlpas: {APPSetting.Default.cfgSQLPassword}");
                        int retCode = Program.SapCompany.Connect();
                        if (retCode == 0)
                        {
                            Program.isSapConnected = true;
                            //MasterForm.SAPStatus = "Connected";
                            //Program.SuccesesMsg("BusinessOne Connected Successfully.");
                            retvalue = "Connected to SAP B1";
                            Program.oErrMgn.LogEntry(Program.ANV, "sap connected.");
                            return true;
                        }
                        else
                        {
                            int erroCode = 0;
                            string errDescr = "";
                            Program.isSapConnected = false;
                            Program.SapCompany.GetLastError(out erroCode, out errDescr);
                            retvalue = "Error Code : " + erroCode + " Description : " + errDescr;
                            RadMessageBox.Show("SBO Exception : " + "Error Code : " + erroCode + " Description : " + errDescr);
                            Program.oErrMgn.LogEntry(Program.ANV, "sap error.");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("SBO Exception : " + ex.Message);
                Program.oErrMgn.LogException(Program.ANV, ex);
                return false;
            }
        }

        public String StatusMsg
        {
            get
            {
                return lblAppStatus.Text;
            }

            set
            {
                lblAppStatus.Text = value;
            }
        }
        #endregion

        #region Functions

        private void InitiallizeSetting()
        {
            try
            {
                Program.oDI = new dbFFS(Program.ConStrApp);
                if (Program.oDI == null)
                {
                    //Error Close Application
                }
                Program.oCurrentUser = (from a in Program.oDI.MstUsers
                                        where a.UserCode == Program.LoggedInUser
                                        select a).FirstOrDefault();
                if (Program.oCurrentUser == null)
                {
                    //Eror close application
                }
                //if (LoginSBO())
                //{
                SystemState();
                GetUserDetailsDisplay();
                LoadMenu();
                if (string.IsNullOrEmpty(txtNameDisplay.Text))
                {
                    btnLogout.Visible = false;
                }
                else
                {
                    btnLogout.Visible = true;
                }
                Program.oErrMgn = new mFm(Application.StartupPath, true, false);//,true,false
                this.Text = Program.ANV;
                //}

                //else
                //{
                //RadMessageBox.Show(" SBO not Connected ");
                //}
                Program.oErrMgn = new mFm(Application.StartupPath, true, false);//,true,false);//,true,false
                this.Text = Program.ANV;
            }
            catch (Exception Ex)
            {
                RadMessageBox.Show("InitiallizeSetting Function Exception : " + Ex.Message);
            }
        }

        private void LoadMenu()
        {
            try
            {
                IEnumerable<CnfMenues> oMenues = null;
                tvMain.Nodes.Clear();
                if (Program.oCurrentUser.UserCode == "manager")
                {
                    oMenues = from a in Program.oDI.CnfMenues
                              where a.MenuParent == 0
                              select a;
                    foreach (CnfMenues oRow in oMenues)
                    {
                        DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                        pNode.Text = oRow.MenuName;
                        pNode.FullRowBackground = true;
                        pNode.Name = oRow.ID.ToString();
                        pNode.Tag = oRow.MenuLink;
                        pNode.ImageIndex = Convert.ToInt32(oRow.ImageID);
                        //pNode.ImageIndex = "";
                        ElementStyle StyleParent = new ElementStyle();
                        StyleParent.BackColor = Color.FromArgb(191, 219, 255);
                        StyleParent.BackColorGradientAngle = 90;
                        //StyleParent.BackgroundImage = DMS.Properties.Resources.MenuHead;
                        StyleParent.BackColor2 = Color.White;
                        StyleParent.TextColor = Color.Black;
                        StyleParent.PaddingTop = 5;
                        StyleParent.PaddingBottom = 3;
                        pNode.Style = StyleParent;

                        IEnumerable<CnfMenues> oChilds = from a in Program.oDI.CnfMenues
                                                         where a.MenuParent == oRow.ID
                                                         select a;
                        foreach (CnfMenues One in oChilds)
                        {
                            DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node();
                            cNode.Text = One.MenuName;
                            cNode.FullRowBackground = true;
                            cNode.Name = One.ID.ToString();
                            cNode.Tag = One.MenuLink;
                            cNode.ImageIndex = Convert.ToInt32(One.ImageID);
                            ElementStyle StyleChild = new ElementStyle();
                            StyleChild.BackColor = Color.FromArgb(191, 219, 255);
                            StyleChild.BackColorGradientAngle = 90;
                            //StyleChild.BackgroundImage = DMS.Properties.Resources.MenuChild;
                            StyleChild.BackColor2 = Color.White;
                            StyleChild.TextColor = Color.Black;
                            StyleChild.PaddingTop = 5;
                            StyleChild.PaddingBottom = 3;
                            cNode.Style = StyleChild;
                            pNode.Nodes.Add(cNode);
                        }
                        tvMain.Nodes.Add(pNode);
                    }


                }
                else
                {
                    oMenues = from a in Program.oDI.CnfMenues
                              where a.MenuParent == 0
                              select a;
                    foreach (CnfMenues oRow in oMenues)
                    {
                        DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                        pNode.Text = oRow.MenuName;
                        pNode.FullRowBackground = true;
                        pNode.Name = oRow.ID.ToString();
                        pNode.Tag = oRow.MenuLink;
                        pNode.ImageIndex = Convert.ToInt32(oRow.ImageID);
                        ElementStyle StyleParent = new ElementStyle();
                        StyleParent.BackColor = Color.FromArgb(191, 219, 255);
                        StyleParent.BackColorGradientAngle = 90;
                        StyleParent.BackColor2 = Color.White;
                        StyleParent.TextColor = Color.Black;
                        StyleParent.PaddingTop = 5;
                        StyleParent.PaddingBottom = 3;
                        pNode.Style = StyleParent;

                        IEnumerable<CnfMenues> oChilds = from a in Program.oDI.CnfMenues
                                                         where a.MenuParent == oRow.ID
                                                         select a;
                        foreach (CnfMenues One in oChilds)
                        {
                            DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node();
                            CnfRolesDetail oRights = (from a in Program.oDI.CnfRolesDetail
                                                      where a.RoleID == Program.oCurrentUser.RoleID
                                                      && a.MenuID == One.ID
                                                      select a).FirstOrDefault();
                            cNode.Text = One.MenuName;
                            cNode.FullRowBackground = true;
                            cNode.Name = One.ID.ToString();
                            cNode.Tag = One.MenuLink;
                            cNode.ImageIndex = Convert.ToInt32(One.ImageID);
                            ElementStyle StyleChild = new ElementStyle();
                            StyleChild.BackColor = Color.FromArgb(191, 219, 255);
                            StyleChild.BackColorGradientAngle = 90;
                            //StyleChild.BackgroundImage = DMS.Properties.Resources.MenuChild;
                            StyleChild.BackColor2 = Color.White;
                            StyleChild.TextColor = Color.Black;
                            StyleChild.PaddingTop = 5;
                            StyleChild.PaddingBottom = 3;
                            cNode.Style = StyleChild;
                            if (oRights.GivenRight != 5)
                            {
                                pNode.Nodes.Add(cNode);
                            }


                        }
                        tvMain.Nodes.Add(pNode);
                    }
                }


                //Formulate Parent Nodes


            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void OpenMenuForm(String pScreenLink, String pScreenName, Int32 pDocEntry)
        {
            try
            {
                Type oFormType = Type.GetType("mfmFFS." + pScreenLink);

                Screens.frmBaseForm fr = ((Screens.frmBaseForm)oFormType.GetConstructor(System.Type.EmptyTypes).Invoke(null));
                fr.DocEntry = pDocEntry;
                fr.WindowState = FormWindowState.Maximized;
                fr.TopLevel = false;
                fr.Visible = true;
                fr.Dock = DockStyle.Fill;
                fr.frmParentRef = Program.MasterForm;

                DevComponents.DotNetBar.TabItem pg = new DevComponents.DotNetBar.TabItem();
                pg.Text = pScreenName + "   ";
                //  fr.MinimizeBox = false;
                // fr.MaximizeBox = false;
                Program.Screen = pScreenName;

                DevComponents.DotNetBar.TabControlPanel ctpl = new DevComponents.DotNetBar.TabControlPanel();
                ctpl.Controls.Add(fr);
                ctpl.Dock = System.Windows.Forms.DockStyle.Fill;
                ctpl.Location = new System.Drawing.Point(0, 26);
                ctpl.Name = "tabControlPanel1";
                ctpl.Padding = new System.Windows.Forms.Padding(1);
                ctpl.Size = new System.Drawing.Size(474, 259);
                ctpl.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
                ctpl.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
                ctpl.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
                ctpl.Style.BorderColor.Color = System.Drawing.SystemColors.ControlDark;
                ctpl.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                            | DevComponents.DotNetBar.eBorderSide.Bottom)));
                ctpl.Style.GradientAngle = 90;
                ctpl.TabIndex = 1;
                ctpl.TabItem = pg;

                fr.mytabpage = ctpl;
                pg.AttachedControl = ctpl;
                pg.CloseButtonVisible = false;

                ctpl.Controls.Add(fr);
                ctpl.Focus();
                Program.MasterForm.tbMain.Controls.Add(ctpl);
                Program.MasterForm.tbMain.Tabs.Add(pg);

                fr.Show();
                fr.Focus();
                ctpl.Focus();
                Program.MasterForm.tbMain.SelectedTabIndex = Program.MasterForm.tbMain.Tabs.Count - 1;
                Program.MasterForm.tbMain.ResumeLayout(true);
                Program.MasterForm.tbMain.Refresh();
                fr.Focus();
            }
            catch (Exception Ex)
            {
            }
        }

        private void GetUserDetailsDisplay()
        {
            try
            {
                txtLoginDisplay.Text = DateTime.Now.ToString();
                txtNameDisplay.Text = Program.oCurrentUser.UserName;
                txtUserCodeDisplay.Text = Program.oCurrentUser.UserCode;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void SystemState()
        {
            try
            {
                if (Program.oCurrentUser.FlgSuper == true || Program.oCurrentUser.UserCode == "manager")
                {

                }
                else
                {

                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }
        #endregion

        #region Form Events

        public frmMain()
        {

            InitializeComponent();
            btnLogout.Visible = false;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception)
            {
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            try
            {
                frmLogin oLogin = new frmLogin();
                oLogin.ShowDialog();
                if (oLogin.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    InitiallizeSetting();
                }
                else
                {
                    this.Dispose();
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void tvMain_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
                if (e.Node.Tag.ToString().Contains("Head"))
                {
                    if (e.Node.Expanded)
                    {
                        e.Node.Collapse();
                    }
                    else
                    {
                        e.Node.Expand();
                    }
                    return;
                }
                if (e.Node.Text == "Dispatch" || e.Node.Text == "RawMaterial" || e.Node.Text == "DispatchReturn" || e.Node.Text == "RawMaterialReturn" || e.Node.Text == "MultipleDispatch")
                {
                    if (Program.flgIndicator)
                    {
                        Program.WarningMsg("Already opened, Release com port form.");
                        return;
                    }
                }
                OpenMenuForm(e.Node.Tag.ToString(), e.Node.Text, 0);
            }
            catch (Exception Ex)
            {
            }
        }

        public void tbMain_TabItemClose(object sender, DevComponents.DotNetBar.TabStripActionEventArgs e)
        {
            DevComponents.DotNetBar.TabControl tb = (DevComponents.DotNetBar.TabControl)sender;
            if (!tb.SelectedTab.Text.Contains("Report"))
            {
                Dialog.frmSimpleDlg dlgSimple = new Dialog.frmSimpleDlg();
                dlgSimple.MsgToShow = "Are you sure you want to close " + tb.SelectedTab.Text + " Form.";
                dlgSimple.ShowDialog();
                if (dlgSimple.DialogResult != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                }
                if (dlgSimple.DialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    if (tb.SelectedTab.Text.Trim() == "Dispatch" || tb.SelectedTab.Text.Trim() == "DispatchReturn" || tb.SelectedTab.Text.Trim() == "RawMaterial" || tb.SelectedTab.Text.Trim() == "RawMaterialReturn")
                    {
                        //Program.comport.Close();
                    }
                }
                Program.NoMsg("");
            }
            else
            {
                Program.WarningMsg(Program.MasterForm.tbMain.Tabs.Count.ToString());
            }
        }

        private void tmrIntegration_Tick(object sender, EventArgs e)
        {
            try
            {
                // LoginSBO();
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("tmrIntegration_Tick Exception ." + Ex.Message);
            }
        }

        private void tmrSync_Tick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("tmrIntegration_Tick Exception ." + Ex.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
            if (btnLogout.Text == "&LogOut")
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
            }
            if (oResult == System.Windows.Forms.DialogResult.Yes)
            {
                for (int i = 1; i < Program.MasterForm.tbMain.Tabs.Count; i = 1)
                {
                    Program.MasterForm.tbMain.Tabs.RemoveAt(i);
                }
                //Application.Exit();
                frmLogin oLogin = new frmLogin();
                oLogin.ShowDialog();
                if (oLogin.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    InitiallizeSetting();
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void tbMain_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            try
            {
                DevComponents.DotNetBar.TabControl tb = (DevComponents.DotNetBar.TabControl)sender;
                if (!tb.SelectedTab.Text.Contains("Report"))
                {
                    Program.Screen = tb.SelectedTab.Text;
                }
            }
            catch (Exception ex)
            {
                Program.WarningMsg(ex.Message);
            }
        }
        #endregion

    }
}
