using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Data;
using mfmWeighment;
using mfmFFS.Dialog;
using mfmFFS.Screens;
using mfmFFSDB;
using UFFU;
using System.IO.Ports;
using System.IO;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;

namespace mfmFFS.Screens
{
    public partial class frmReportViewer : frmBaseForm
    {

        #region Variable

        dbFFS oDB = null;

        public string rptLayoutFor = "";
        public string rptCritaria = "";
        public string rptName = "";
        public string FromDate = "";
        public string ToDate = "";
        public string Customer = "";
        public string Customer2 = "";
        public string ItemCode = "";
        public string ItemCode2 = "";
        public string Supplier = "";
        public string Supplier2 = "";
        public string Transport = "";
        public string Transport2 = "";
        //private ReportDocument rptDocument;

        #endregion

        #region Functions

        private void LoadSelectedReport()
        {
            try
            {
                ReportDocument rptDocument;
                //Program.oErrMgn.LogEntry(Program.ANV, "LoaSelectedReport function start");
                if (!string.IsNullOrEmpty(rptLayoutFor))
                {
                    MstReports oRpt = (from a in oDB.MstReports where a.ReportIn == rptLayoutFor && a.FlgDefault == true select a).FirstOrDefault();
                    if (oRpt == null)
                    {
                        //RadMessageBox.Show("No Layout Found.");
                        Program.oErrMgn.LogEntry(Program.ANV, "report not found for: " + rptLayoutFor);
                    }
                    else
                    {
                        rptDocument = new ReportDocument();
                        byte[] rptBytes = oRpt.RptFile.ToArray();
                        string ReportFileName = Application.StartupPath + "\\" + rptLayoutFor + ".rpt";
                        if (!File.Exists(ReportFileName))
                        {
                            using (FileStream oFS = new FileStream(ReportFileName, System.IO.FileMode.Create))
                            {

                                int len = rptBytes.Length;
                                oFS.Write(rptBytes, 0, len);
                                oFS.Flush();
                                oFS.Close();
                            }
                        }
                        else
                        {
                            //Program.oErrMgn.LogEntry(Program.ANV, "1stwei File.Delete(ReportFileName) start");
                            File.Delete(ReportFileName);
                            //Program.oErrMgn.LogEntry(Program.ANV, "1stwei File.Delete(ReportFileName) End");
                            using (FileStream oFS = new FileStream(ReportFileName, System.IO.FileMode.Create))
                            {
                                int len = rptBytes.Length;
                                oFS.Write(rptBytes, 0, len);
                                oFS.Flush();
                                oFS.Close();
                            }
                        }
                        rptDocument.Load(ReportFileName);
                        SetReport(rptDocument);
                        //rptDocument.SetParameterValue("Critaria", rptCritaria);
                        rptDocument.SetParameterValue("DocNum", rptCritaria);
                        System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
                        rptDocument.PrintOptions.PrinterName = doctoprint.DefaultPageSettings.PrinterSettings.PrinterName;
                        crvMain.ReportSource = rptDocument;
                        //rptDocument.Close();
                        //rptDocument.Dispose();
                    }
                }
                else
                {
                    MstReports oRpt = (from a in oDB.MstReports where a.RptCode == Program.rpt && a.FlgDefault == true select a).FirstOrDefault();
                    if (oRpt == null)
                    {
                        //RadMessageBox.Show("No Report Found.");
                        Program.oErrMgn.LogEntry(Program.ANV, "report not found direct for: " + Program.rpt);
                    }
                    else
                    {
                        byte[] rptBytes = oRpt.RptFile.ToArray();
                        string ReportFileName = Application.StartupPath + "\\" + Program.Screen + ".rpt";
                        if (!File.Exists(ReportFileName))
                        {
                            using (FileStream oFS = new FileStream(ReportFileName, System.IO.FileMode.Create))
                            {
                                int len = rptBytes.Length;
                                oFS.Write(rptBytes, 0, len);
                                oFS.Flush();
                                oFS.Close();
                            }
                        }
                        else
                        {
                            //Program.oErrMgn.LogEntry(Program.ANV, "2ndWei File.Delete(ReportFileName) start");
                            File.Delete(ReportFileName);
                            //Program.oErrMgn.LogEntry(Program.ANV, "2ndWei File.Delete(ReportFileName) End");
                            using (FileStream oFS = new FileStream(ReportFileName, System.IO.FileMode.Create))
                            {
                                int len = rptBytes.Length;
                                oFS.Write(rptBytes, 0, len);
                                oFS.Flush();
                                oFS.Close();
                            }
                        }
                        
                        rptDocument = new ReportDocument();

                        rptDocument.Load(ReportFileName);
                        SetReport(rptDocument);

                        if (FromDate != "" && ToDate != "")
                        {
                            rptDocument.SetParameterValue("FromDate", FromDate);
                            rptDocument.SetParameterValue("ToDate", ToDate);
                        }

                        if (!string.IsNullOrEmpty(Supplier) && !string.IsNullOrEmpty(Supplier2))
                        {
                            rptDocument.SetParameterValue("FromSupp", Supplier);
                            rptDocument.SetParameterValue("ToSupp", Supplier2);
                        }
                        else if (!string.IsNullOrEmpty(Transport) && !string.IsNullOrEmpty(Transport2))
                        {
                            rptDocument.SetParameterValue("ToTrans", Transport);
                            rptDocument.SetParameterValue("FromTrans", Transport2);
                        }
                        else if (!string.IsNullOrEmpty(Customer) && !string.IsNullOrEmpty(Customer2))
                        {
                            rptDocument.SetParameterValue("FromCus", Customer);
                            rptDocument.SetParameterValue("ToCus", Customer2);
                        }
                        else if (!string.IsNullOrEmpty(ItemCode) && !string.IsNullOrEmpty(ItemCode2))
                        {
                            rptDocument.SetParameterValue("FromItem", ItemCode);
                            rptDocument.SetParameterValue("ToItem", ItemCode2);
                        }
                        //else
                        //{
                        //    Program.WarningMsg("Select Values.");
                        //    return;
                        //}

                        //rptDocument.SetParameterValue("Critaria2", rptCritaria);
                        System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
                        rptDocument.PrintOptions.PrinterName = doctoprint.DefaultPageSettings.PrinterSettings.PrinterName;
                        crvMain.ReportSource = rptDocument;

                    }
                }
                //Program.oErrMgn.LogEntry(Program.ANV, "LoadSelectedReport function start");
            }
            catch (Exception Ex)
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "LoadSelectedReport function start: " + Ex);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        public bool OpenReport(string rptLayoutFor, string rptCritaria, string rptName, string ToDate, string FromDate, string Customer,
    string Customer2, string Transport, string Transport2, string Supplier, string Supplier2, string ItemCode, string ItemCode2)
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "OpenReport+ Function Start");
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
                oForm.frmParentRef = Program.MasterForm;

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
                Program.MasterForm.tbMain.Controls.Add(ctpl);
                Program.MasterForm.tbMain.Tabs.Add(oPage);

                oForm.Show();
                //oForm.Focus();
                ctpl.Focus();
                Program.MasterForm.tbMain.SelectedTabIndex = Program.MasterForm.tbMain.Tabs.Count - 1;
                Program.MasterForm.tbMain.ResumeLayout(true);
                Program.MasterForm.tbMain.Refresh();

                oForm.Focus();
                //Program.oErrMgn.LogEntry(Program.ANV, "OpenReport+ function start");
               // oForm = null;
                return true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
                return false;
            }
        }
        public void SetReport(ReportDocument rep)
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "SetReport function start");
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
                //Program.oErrMgn.LogEntry(Program.ANV, "SetReport function End");
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                throw;
            }

        }

        #endregion

        #region Form Events

        public frmReportViewer()
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "frmReportViewer function start");
                InitializeComponent();
                //Program.oErrMgn.LogEntry(Program.ANV, "frmReportViewer function start");
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
                //Program.ExceptionMsg("ReprtViwerIniti" + Ex.ToString());
            }
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "frmReportViewer_Load function start");
                oDB = new dbFFS(Program.ConStrApp);
                LoadSelectedReport();
                //Program.oErrMgn.LogEntry(Program.ANV, "frmReportViewer_Load function End");
            }
            catch (Exception Ex)
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "frmReportViewer_Load function Eception: " + Ex);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }



        #endregion

        private void frmReportViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
