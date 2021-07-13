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

/**add*/
using System.IO.Ports;
using System.IO;
using System.Net;
/***/


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
                        return false;
                    }
                    else
                    {
                        Program.SapCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
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
                            Program.oErrMgn.LogEntry(Program.ANV, retvalue);
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

        /**add*/
        dbFFS oDB = null;
        string ComPort, BaudRate, Parity, StopBit, StartChar, DataBits, DataLenght, IndicatorType;
        string ComPort2, BaudRate2, Parity2, StopBit2, StartChar2, DataBits2, DataLenght2, IndicatorType2;
        private SerialPort Rawcomport = new SerialPort();
        private SerialPort Rawcomport2 = new SerialPort();
        Boolean alreadyReading = false;
        Boolean alreadyReading2 = false;
        long currentwt = 0;
        long currentwt2 = 0;
        delegate void delCWeight(string value);
        delCWeight DelCall;
        delegate void delCWeight2(string value2);
        delCWeight2 DelCall2;
        /***/

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

                /**add**/
                oDB = new dbFFS(Program.ConStrApp);
                txtFullText.Visible = false;
                tmrAlreadyReading.Interval = 1000;
                DelCall = new delCWeight(CallSafeCWeight);
                GetMachineSetting();

                txtFullText2.Visible = false;
                tmrAlreadyReading2.Interval = 1000;
                DelCall2 = new delCWeight2(CallSafeCWeight2);
                GetMachineSetting2();
                /****/
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

        /**add*/
        /*private void Indicator01()
        {

            if (!alreadyReading)
            {
                try
                {
                    alreadyReading = true;
                    Thread.Sleep(500);
                    string data = Rawcomport.ReadExisting();
                    //Program.oErrMgn.LogEntry(Program.ANV, " readed data :" + data);
                    //Program.oErrMgn.LogEntry(Program.ANV, " readed data ki lenght :" + data.Length.ToString());
                    if (data.Length < 9)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value 9 se kam hai");
                        alreadyReading = false;
                        return;
                    }
                    //Program.oErrMgn.LogEntry(Program.ANV, " config data ki lenght :" + DataLenght);
                    if (data.Length < Convert.ToInt32(DataLenght) + 2)
                    {
                        //Program.oErrMgn.LogEntry(Program.ANV, "crap data lenght ki value kam hai");
                        alreadyReading = false;
                        return;
                    }
                    txtFullText.Invoke(new EventHandler(delegate
                    {
                        int charindex = data.IndexOf(StartChar);
                        //Program.oErrMgn.LogEntry(Program.ANV, "charindex ki value " + Convert.ToString(charindex));
                        long currentWeight = 0;
                        if (charindex >= 0)
                        {
                            //Program.oErrMgn.LogEntry(Program.ANV, "char index ki bari value " + Convert.ToString(charindex + Convert.ToInt32(DataLenght)));
                            if (data.Length > charindex + Convert.ToInt32(DataLenght))
                            {
                                //string tempconvalue = data.Substring(charindex + 1, Convert.ToInt16(DataLenght));
                                string tempconvalue = data.Substring(charindex + 2, Convert.ToInt16(DataLenght));
                                //Program.oErrMgn.LogEntry(Program.ANV, "temp extracted value : " + tempconvalue);
                                string val = tempconvalue.TrimStart();
                                currentWeight = Convert.ToInt64(val);
                                currentwt = currentWeight;
                                //txtCWeight.Text = currentwt.ToString();
                                CallSafeCWeight(currentwt.ToString());
                                //Program.oErrMgn.LogEntry(Program.ANV, " Currentwt :" + currentwt.ToString());
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Program.oErrMgn.LogException(Program.ANV, ex);
                    alreadyReading = false;
                }
                alreadyReading = false;
            }

        }*/

        /*private void OldIndicator()
        {
            if (!alreadyReading)
            {
                try
                {
                    //DataLenght = "6";
                    //StartChar = "+";
                    alreadyReading = true;
                    Thread.Sleep(500);
                    //string data = "P+    50";
                    string data = Rawcomport.ReadExisting();
                    if (data.Length < 9)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value 9 se kam hai");
                        alreadyReading = false;
                        return;
                    }
                    if (data.Length < Convert.ToInt32(DataLenght) + 2)
                    {
                        alreadyReading = false;
                        return;
                    }
                    txtFullText.Invoke(new EventHandler(delegate
                    {
                        int charindex = data.IndexOf(StartChar);
                        string currentwt = "";
                        if (charindex >= 0)
                        {
                            if (data.Length > charindex + Convert.ToInt32(DataLenght))
                            {
                                string val = data.TrimStart();
                                for (int i = 0; i < val.Length; i++)
                                {
                                    if (char.IsNumber(val[i]))
                                    {
                                        currentwt += val[i].ToString();
                                    }
                                }
                                CallSafeCWeight(currentwt.ToString());
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Program.oErrMgn.LogException(Program.ANV, ex);
                    alreadyReading = false;
                }
                alreadyReading = false;
            }
        }*/

        private void Indicator01()
        {
            if (!alreadyReading)
            {
                try
                {
                    //DataLenght = "6";
                    //StartChar = "+";
                    alreadyReading = true;
                    Thread.Sleep(500);
                    //string data = "P+  2150";
                    string data = Rawcomport.ReadExisting();
                    if (data.Length < 8)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value 8 se kam hai");
                        alreadyReading = false;
                        return;
                    }
                    if (data.Length < Convert.ToInt32(DataLenght) + 2)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value kam hai");
                        alreadyReading = false;
                        return;
                    }
                    txtFullText.Invoke(new EventHandler(delegate
                    {
                        int charindex = data.IndexOf(StartChar);
                        string currentwt = "";
                        if (charindex >= 0)
                        {
                            if (data.Length > charindex + Convert.ToInt32(DataLenght))
                            {
                                string tempconvalue = data.Substring(charindex + 1, Convert.ToInt16(DataLenght));
                                string val = tempconvalue.TrimStart();
                                for (int i = 0; i < val.Length; i++)
                                {
                                    if (char.IsNumber(val[i]))
                                    {
                                        currentwt += val[i].ToString();
                                    }
                                }
                                CallSafeCWeight(currentwt.ToString());
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Program.oErrMgn.LogException(Program.ANV, ex);
                    alreadyReading = false;
                }
                alreadyReading = false;
            }
        }

        private void CallSafeCWeight(string pValue)
        {
            try
            {
                if (txtCWeight.InvokeRequired)
                {
                    DelCall = new delCWeight(CallSafeCWeight);
                    this.txtCWeight.Invoke(DelCall, new object[] { pValue });
                }
                else
                {
                    this.txtCWeight.Text = pValue;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Delegate Err");
                Program.oErrMgn.LogEntry(Program.ANV, ex.Message);
            }

        }

        private void GetMachineSetting()
        {
            try
            {
                MstWeighBridge oDoc = (from a in oDB.MstWeighBridge where a.WBCode == "WB1" select a).FirstOrDefault();
                if (oDoc != null)
                {
                    IndicatorType = oDoc.WBCode;
                    ComPort = oDoc.ComPort;
                    BaudRate = oDoc.BaudRate;
                    Parity = oDoc.Parity;
                    StopBit = oDoc.StopBits;
                    StartChar = oDoc.StartChar;
                    DataBits = oDoc.DataBits;
                    DataLenght = oDoc.Lenght;
                    //20/3/2019
                    if (Rawcomport.IsOpen)
                    {
                        Rawcomport.Close();
                    }
                    //20/3/2019
                    ConnectPort();
                }
                else
                {
                    Program.WarningMsg("Machine Settings not found Weighment brigde is not configure for this machine.");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ConnectPort()
        {
            Program.oErrMgn.LogEntry(Program.ANV, "Connecting Port");
            try
            {
                if (Rawcomport.IsOpen) Rawcomport.Close();
                bool tmrc = tmrCamFront.Enabled;

                // Set the port's settings
                // tmrCamFront.Enabled = false;
                Rawcomport.BaudRate = Convert.ToInt32(BaudRate);
                Rawcomport.DataBits = Convert.ToInt32(DataBits);
                Rawcomport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBit);
                Rawcomport.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                Rawcomport.PortName = ComPort;
                Rawcomport.ReadTimeout = 5000;

                Program.WarningMsg("Trying to connect.");
                Rawcomport.Open();
                Program.SuccesesMsg("Connected.");
                Program.oErrMgn.LogEntry(Program.ANV, "Connected");
            }
            catch (Exception Ex)
            {
                // MessageBox.Show(ex.Message);
                Program.ExceptionMsg("Error in connecting : " + Ex.Message);
                alreadyReading = false;
                // tmrCamFront.Enabled = tmrc;
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void Indicator02()
        {
            if (!alreadyReading2)
            {
                try
                {
                    //DataLenght = "6";
                    //StartChar = "+";
                    alreadyReading2 = true;
                    Thread.Sleep(500);
                    //string data = "P+  2150";
                    string data2 = Rawcomport2.ReadExisting();
                    if (data2.Length < 8)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value 8 se kam hai");
                        alreadyReading2 = false;
                        return;
                    }
                    if (data2.Length < Convert.ToInt32(DataLenght2) + 2)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value kam hai");
                        alreadyReading2 = false;
                        return;
                    }
                    txtFullText2.Invoke(new EventHandler(delegate
                    {
                        int charindex2 = data2.IndexOf(StartChar2);
                        string currentwt2 = "";
                        if (charindex2 >= 0)
                        {
                            if (data2.Length > charindex2 + Convert.ToInt32(DataLenght2))
                            {
                                string tempconvalue2 = data2.Substring(charindex2 + 1, Convert.ToInt16(DataLenght2));
                                string val2 = tempconvalue2.TrimStart();
                                for (int j = 0; j < val2.Length; j++)
                                {
                                    if (char.IsNumber(val2[j]))
                                    {
                                        currentwt2 += val2[j].ToString();
                                    }
                                }
                                CallSafeCWeight2(currentwt2.ToString());
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Program.oErrMgn.LogException(Program.ANV, ex);
                    alreadyReading2 = false;
                }
                alreadyReading2 = false;
            }
        }

        private void CallSafeCWeight2(string pValue2)
        {
            try
            {
                if (txtCWeight2.InvokeRequired)
                {
                    DelCall2 = new delCWeight2(CallSafeCWeight2);
                    this.txtCWeight2.Invoke(DelCall2, new object[] { pValue2 });
                }
                else
                {
                    this.txtCWeight2.Text = pValue2;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Delegate Err");
                Program.oErrMgn.LogEntry(Program.ANV, ex.Message);
            }

        }

        private void GetMachineSetting2()
        {
            try
            {
                MstWeighBridge oDoc = (from a in oDB.MstWeighBridge where a.WBCode == "WB2" select a).FirstOrDefault();
                if (oDoc != null)
                {
                    IndicatorType2 = oDoc.WBCode;
                    ComPort2 = oDoc.ComPort;
                    BaudRate2 = oDoc.BaudRate;
                    Parity2 = oDoc.Parity;
                    StopBit2 = oDoc.StopBits;
                    StartChar2 = oDoc.StartChar;
                    DataBits2 = oDoc.DataBits;
                    DataLenght2 = oDoc.Lenght;
                    //20/3/2019
                    if (Rawcomport2.IsOpen)
                    {
                        Rawcomport2.Close();
                    }
                    //20/3/2019
                    ConnectPort2();
                }
                else
                {
                    Program.WarningMsg("Machine Settings not found Weighment brigde is not configure for this machine.");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ConnectPort2()
        {
            Program.oErrMgn.LogEntry(Program.ANV, "Connecting Port");
            try
            {
                if (Rawcomport2.IsOpen) Rawcomport2.Close();
                bool tmrc = tmrCamFront2.Enabled;

                // Set the port's settings
                // tmrCamFront.Enabled = false;
                Rawcomport2.BaudRate = Convert.ToInt32(BaudRate2);
                Rawcomport2.DataBits = Convert.ToInt32(DataBits2);
                Rawcomport2.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBit2);
                Rawcomport2.Parity = (Parity)Enum.Parse(typeof(Parity), Parity2);
                Rawcomport2.PortName = ComPort2;
                Rawcomport2.ReadTimeout = 5000;

                Program.WarningMsg("Trying to connect.");
                Rawcomport2.Open();
                Program.SuccesesMsg("Connected.");
                Program.oErrMgn.LogEntry(Program.ANV, "Connected");
            }
            catch (Exception Ex)
            {
                // MessageBox.Show(ex.Message);
                Program.ExceptionMsg("Error in connecting : " + Ex.Message);
                alreadyReading2 = false;
                // tmrCamFront.Enabled = tmrc;
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        /***/
        #endregion

        #region Form Events

        public frmMain()
        {

            InitializeComponent();
            btnLogout.Visible = false;
            
            /**add*/
            Rawcomport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            txtCWeight.Visible = true;
            txtCWeight.Enabled = false;

            Rawcomport2.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived2);
            txtCWeight2.Visible = true;
            txtCWeight2.Enabled = false;
            /***/
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                /***add***/
                if (Rawcomport.IsOpen)
                {
                    Rawcomport.DiscardInBuffer();
                    Rawcomport.DiscardOutBuffer();
                    Rawcomport.Close();
                    // Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 103");
                }
                if (Rawcomport2.IsOpen)
                {
                    Rawcomport2.DiscardInBuffer();
                    Rawcomport2.DiscardOutBuffer();
                    Rawcomport2.Close();
                    // Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 103");
                }
                /****/
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
                    if (tb.SelectedTab.Text.Trim() == "Dispatch" || tb.SelectedTab.Text.Trim() == "DispatchReturn" || tb.SelectedTab.Text.Trim() == "RawMaterial" || tb.SelectedTab.Text.Trim() == "RawMaterialReturn" || tb.SelectedTab.Text.Trim() == "MultipleDispatch")
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

        /**add*/
        [STAThread]
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Indicator01();
        }

        [STAThread]
        private void port_DataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            Indicator02();
        }

        private void txtCWeight_TextChanged(object sender, EventArgs e)
        {
            Program.Bridge01Value = txtCWeight.Text;
        }

        private void txtCWeight2_TextChanged(object sender, EventArgs e)
        {
            Program.Bridge02Value = txtCWeight2.Text;
        }
        /***/
        #endregion
    }
}
