using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Data;
using mfmWeighment;

using mfmFFS.Screens;
using mfmFFSDB;
using UFFU;
using System.IO.Ports;
using System.IO;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using SAPbobsCOM;

namespace mfmFFS
{
    static class Program
    {
        public static string AppName = "Weighment ";
        public static string AppVersion = "v1.2.0.11";
        public static string ANV = AppName + " " + AppVersion;
        public static string FormItemCode = "";
        public static int FormDocNum = 0;
        public static string rpt = "";
        public static string FormName = "";
        //public static SerialPort comport = new SerialPort();
        public static DataTable sealdt = new DataTable();
        public static dbFFS oDI = null;
        public static MstUsers oCurrentUser = null;
        public static string ConStrApp = "";
        public static string ConStrSAP = "";
        public static string LoggedInUser = "";
        public static string DocNum = "";
        public static string Screen = "";
        public static string DBaseLine = "";
        public static string DBaseEntry = "";
        public static string DBaseType = "";
        public static string DRBaseLine = "";
        public static string DRBaseEntry = "";
        public static string DRBaseType = "";
        public static string PBaseLine = "";
        public static string PBaseEntry = "";
        public static string PBaseType = "";
        public static string PRBaseLine = "";
        public static string PRBaseEntry = "";
        public static string PRBaseType = "";
        public static bool flgIndicator = false;
        public static mFm oErrMgn;
        public static string Bridge01Value = "";
        public static string Bridge02Value = "";

        public static frmMain MasterForm;
        //public static frmUser User;
        //public static frmAuthentication Auth;
        //public static frmMstYard MstYard;
        public static SAPbobsCOM.Company SapCompany;
        public static bool isSapConnected = false;



        /// <summary>
        /// 
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MasterForm = new mfmFFS.frmMain();
            //User = new mfmFFS.Screens.frmUser();
            //Auth = new mfmFFS.Screens.frmAuthentication();
            //MstYard = new mfmFFS.Screens.frmMstYard();
            Application.Run(MasterForm);
        }

        public static void ExceptionMsg(String msg)
        {
            if (MasterForm != null)
            {
                MasterForm.MsgType = "Exception";
                MasterForm.StatusMsg = DateTime.Now.ToString() + " : Exception Msg : " + msg;
            }
        }

        public static void NoMsg(String msg)
        {
            if (MasterForm != null)
            {
                MasterForm.MsgType = "Exception";
                MasterForm.StatusMsg = "";
            }
        }

        public static void SuccesesMsg(String msg)
        {
            if (MasterForm != null)
            {
                MasterForm.MsgType = "Success";
                MasterForm.StatusMsg = DateTime.Now.ToString() + " : Success Msg : " + msg;
            }
        }

        public static void WarningMsg(String msg)
        {
            if (MasterForm != null)
            {
                MasterForm.MsgType = "Warning";
                MasterForm.StatusMsg = DateTime.Now.ToString() + " : Warning Msg : " + msg;
            }
        }

        public static int doNonQuery(string strSql, string dbConstr)
        {
            int result = 0;
            SqlConnection con = new SqlConnection(dbConstr);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = strSql;
                result = cmd.ExecuteNonQuery();

            }
            catch
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return result;
        }

        public static int doScaler(string prQuery, string prCon)
        {
            int retValue = 0;
            SqlConnection con = new SqlConnection(prCon);
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = prQuery;
                retValue = Convert.ToInt32(com.ExecuteScalar());

            }
            catch (Exception)
            {

            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retValue;
        }

        public static DataTable getDataTable(string strsql, string dtConstr)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(dtConstr);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = strsql;
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                dt.Clear();
                dt.Load(dr);

                dr.Close();
            }
            catch
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return dt;
        }

        public static bool OpenLayout(string rptLayoutFor, string rptCritaria, string rptName)
        {
            try
            {
                Screens.frmReportViewer oForm = new mfmFFS.Screens.frmReportViewer();
                oForm.rptLayoutFor = rptLayoutFor;
                oForm.rptCritaria = rptCritaria;
                oForm.TopLevel = false;
                oForm.Visible = true;
                oForm.Dock = DockStyle.Fill;
                oForm.frmParentRef = MasterForm;

                DevComponents.DotNetBar.TabItem oPage = new DevComponents.DotNetBar.TabItem();
                oPage.Text = "Report - " + rptName;
                //  fr.MinimizeBox = false;
                // fr.MaximizeBox = false;


                DevComponents.DotNetBar.TabControlPanel ctpl = new DevComponents.DotNetBar.TabControlPanel();
                ctpl.Controls.Add(oForm);
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
                ctpl.TabItem = oPage;

                oForm.mytabpage = ctpl;
                oPage.AttachedControl = ctpl;


                ctpl.Controls.Add(oForm);
                ctpl.Focus();
                MasterForm.tbMain.Controls.Add(ctpl);
                MasterForm.tbMain.Tabs.Add(oPage);

                oForm.Show();
                //oForm.Focus();
                ctpl.Focus();
                MasterForm.tbMain.SelectedTabIndex = MasterForm.tbMain.Tabs.Count - 1;
                MasterForm.tbMain.ResumeLayout(true);
                MasterForm.tbMain.Refresh();

                oForm.Focus();
                return true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
                return false;
            }
        }

        public static bool OpenReport(string rptLayoutFor, string rptCritaria, string rptName, string ToDate, string FromDate, string Customer,
            string Customer2, string Transport, string Transport2, string Supplier, string Supplier2, string ItemCode, string ItemCode2)
        {
            try
            {
                Screens.frmReportViewer oForm = new mfmFFS.Screens.frmReportViewer();
                oForm.rptLayoutFor = rptLayoutFor;
                oForm.rptCritaria = rptCritaria;

                oForm.Customer = Customer;
                oForm.Customer2 = Customer2;
                oForm.Supplier = Supplier;
                oForm.Supplier2 = Supplier2;
                oForm.Transport = Transport;
                oForm.Transport2 = Transport2;
                oForm.ItemCode = ItemCode;
                oForm.ItemCode2 = ItemCode2;
                oForm.FromDate = FromDate;
                oForm.ToDate = ToDate;

                oForm.TopLevel = false;
                oForm.Visible = true;
                oForm.Dock = DockStyle.Fill;
                oForm.frmParentRef = MasterForm;

                DevComponents.DotNetBar.TabItem oPage = new DevComponents.DotNetBar.TabItem();
                oPage.Text = "Report - " + rptName;
                //  fr.MinimizeBox = false;
                // fr.MaximizeBox = false;


                DevComponents.DotNetBar.TabControlPanel ctpl = new DevComponents.DotNetBar.TabControlPanel();
                ctpl.Controls.Add(oForm);
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
                ctpl.TabItem = oPage;

                oForm.mytabpage = ctpl;
                oPage.AttachedControl = ctpl;


                ctpl.Controls.Add(oForm);
                ctpl.Focus();
                MasterForm.tbMain.Controls.Add(ctpl);
                MasterForm.tbMain.Tabs.Add(oPage);

                oForm.Show();
                //oForm.Focus();
                ctpl.Focus();
                MasterForm.tbMain.SelectedTabIndex = MasterForm.tbMain.Tabs.Count - 1;
                MasterForm.tbMain.ResumeLayout(true);
                MasterForm.tbMain.Refresh();

                oForm.Focus();
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public static void SetReport(ReportDocument rep)
        {
            bool flgAllGood = true;
            string SqlServer = "", SqlUser = "", SqlPwd = "", DBName = "";
            if (!String.IsNullOrEmpty(APPSetting.Default.cfgSQLServer))
            {
                SqlServer = APPSetting.Default.cfgSQLServer;
                flgAllGood = true;
            }
            if (!String.IsNullOrEmpty(APPSetting.Default.cfgSQLUser))
            {
                SqlUser = APPSetting.Default.cfgSQLUser;
            }
            else
            {
                flgAllGood = false;
            }
            if (!String.IsNullOrEmpty(APPSetting.Default.cfgSQLPassword))
            {
                SqlPwd = APPSetting.Default.cfgSQLPassword;
            }
            else
            {
                flgAllGood = false;
            }
            if (!String.IsNullOrEmpty(APPSetting.Default.cfgAPPDB))
            {
                DBName = APPSetting.Default.cfgAPPDB;
            }
            else
            {
                flgAllGood = false;
            }

            foreach (CrystalDecisions.CrystalReports.Engine.Table Table in rep.Database.Tables)
            {
                CrystalDecisions.Shared.TableLogOnInfo Logon;
                Logon = Table.LogOnInfo;
                Logon.ConnectionInfo.ServerName = SqlServer;
                Logon.ConnectionInfo.DatabaseName = DBName;
                Logon.ConnectionInfo.Password = SqlPwd;
                Logon.ConnectionInfo.UserID = SqlUser;
                Table.ApplyLogOnInfo(Logon);

            }

        }

        public static string TimeValidation(string Result, string Time)
        {
            string[] b = Time.Split(':');
            if (Convert.ToInt32(b.Length) != 2)
            {
                Program.ExceptionMsg("DurationTo is not in a valid format.");
                return null;

            }
            string b1 = b[0];
            string b2 = b[1];

            if (Convert.ToInt32(b1) > 23 || Convert.ToInt32(b1) < 0 || b1.Length > 2)
            {
                Program.ExceptionMsg("DurationTo is not in a valid format.");
                return null;
            }
            if (Convert.ToInt32(b2) > 59 || Convert.ToInt32(b2) < 0 || b2.Length > 2)
            {
                Program.ExceptionMsg("DurationTo is not in a valid format.");
                return null;
            }

            return Result;
        }

    }
}
