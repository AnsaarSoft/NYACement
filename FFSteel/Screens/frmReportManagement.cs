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
using mfmFFS;
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
    public partial class frmReportManagement : frmBaseForm
    {

        #region Variable

        dbFFS oDB = null;
        DataTable dtReports = null;

        #endregion

        #region Functions

        private void FormInitiazliation()
        {
            try
            {
                cmbRptName.Visible = false;
                txtName.Visible = true;
                oDB = new dbFFS(Program.ConStrApp);
                btnFirstRecord.Enabled = false;
                btnLastRecord.Enabled = false;
                btnNextRecord.Enabled = false;
                btnPreviosRecord.Enabled = false;
                btnSearch.Enabled = false;
                cmbMenu.Enabled = false;
                cmbSourceDoc.Enabled = true;
                chlReport.Visible = true;
                CreateDt();
                FillGrid();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ClearControls()
        {
            try
            {
                txtCode.Enabled = true;
                txtCode.Text = "";
                txtName.Text = "";
                txtFilePath.Text = "";
                cmbMenu.SelectedValue = "";
                cmbSourceDoc.SelectedValue = "";
                chkLayout.CheckState = CheckState.Checked;
                btnImport.Text = "&Import";
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void CreateDt()
        {
            try
            {
                dtReports = new DataTable();
                dtReports.Columns.Add("ID");
                dtReports.Columns.Add("Code");
                dtReports.Columns.Add("Name");
                dtReports.Columns.Add("ShowIn");
                dtReports.Columns.Add("Type");
                dtReports.Columns.Add("Default");
            }
            catch (Exception Ex)
            {
            }
        }

        private static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = File.OpenRead(fullFilePath);
            byte[] bytes = null;
            try
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            finally
            {
                fs.Close();
            }
            return bytes;
        }

        private void AddReport()
        {
            try
            {
                MstReports oRpt = new MstReports();
                // oRpt.RptCode = txtCode.Text;
                //oRpt.RptName = txtName.Text;
                oRpt.RptFile = GetBytesFromFile(txtFilePath.Text);
                if (chkLayout.IsChecked)
                {
                    oRpt.RptCode = txtCode.Text;
                    oRpt.RptName = txtName.Text;
                    oRpt.ReportIn = cmbSourceDoc.SelectedItem.Text;
                }
                else if (chlReport.IsChecked)
                {
                    //oRpt.ReportIn = cmbMenu.SelectedItem.Text;
                    oRpt.RptCode = txtCode.Text;
                    oRpt.RptName = cmbRptName.Text;

                }

                oRpt.FlgLayout = Convert.ToBoolean(chkLayout.IsChecked);
                oRpt.FlgReport = Convert.ToBoolean(chlReport.IsChecked);
                oRpt.FlgActive = true;
                oRpt.FlgDefault = false;
                oRpt.CreateDt = DateTime.Now;
                oRpt.UpdateDt = DateTime.Now;
                oRpt.CreatedBy = Program.oCurrentUser.UserCode;
                oRpt.UpdatedBy = Program.oCurrentUser.UserCode;

                oDB.MstReports.InsertOnSubmit(oRpt);
                oDB.SubmitChanges();
                //if (chlReport.IsChecked)
                //{

                //    CnfMenues oMnu = new CnfMenues();
                //    oMnu.ReportsID = Convert.ToInt32(oDB.MstReports.Where(x => x.RptCode == oRpt.RptCode).FirstOrDefault().ID);
                //    oMnu.MenuName = oRpt.RptName;
                //    oMnu.MenuLink = "Screens.frmReportViewer";
                //    oMnu.MenuParent = Convert.ToInt32(oDB.CnfMenues.Where(x => x.MenuName == oRpt.ReportIn).FirstOrDefault().ID);
                //    oDB.CnfMenues.InsertOnSubmit(oMnu);
                //oDB.SubmitChanges();
                //}

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
                Program.WarningMsg(Ex.ToString());
            }
            FillGrid();
        }

        private void UpdateReport()
        {
            try
            {
                int id = Convert.ToInt32(txtID.Text.Trim());
                MstReports oRpt = (from a in oDB.MstReports where a.ID == id select a).FirstOrDefault();

                
                if (!string.IsNullOrEmpty(txtFilePath.Text))
                {
                    oRpt.RptFile = GetBytesFromFile(txtFilePath.Text);
                }
                if (chlReport.IsChecked == true)
                {
                    oRpt.RptCode = txtCode.Text;
                    oRpt.RptName = cmbRptName.Text;
                }
                else
                {
                    oRpt.RptName = txtName.Text;
                }

                oRpt.FlgActive = true;
                oRpt.UpdateDt = DateTime.Now;
                oRpt.UpdatedBy = Program.oCurrentUser.UserCode;
                oDB.SubmitChanges();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            FillGrid();
        }

        private void FillReport(string pReportID)
        {
            try
            {
                if (!string.IsNullOrEmpty(pReportID))
                {
                    MstReports oRpt = (from a in oDB.MstReports where a.ID.ToString() == pReportID select a).FirstOrDefault();
                    if (oRpt != null)
                    {
                        txtCode.Text = oRpt.RptCode;
                        txtID.Text = Convert.ToString(oRpt.ID);

                        if (oRpt.FlgLayout == true)
                        {
                            cmbRptName.Visible = false;
                            txtName.Visible = true;
                            txtName.Text = oRpt.RptName;
                            chkLayout.CheckState = CheckState.Checked;
                            cmbSourceDoc.Enabled = true;
                            cmbSourceDoc.SelectedValue = Convert.ToString(oRpt.ReportIn);
                            txtCode.Enabled = false;
                        }
                        else if (oRpt.FlgReport == true)
                        {
                            cmbRptName.Visible = true;
                            cmbRptName.Text = oRpt.RptName;
                            txtName.Visible = false;
                            chlReport.CheckState = CheckState.Checked;
                            cmbMenu.Enabled = true;
                            txtCode.Enabled = true;
                        }

                        btnImport.Text = "&Update";
                       
                    }
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void FillGrid()
        {
            try
            {
                dtReports.Rows.Clear();
                IEnumerable<MstReports> oCollection = from a in oDB.MstReports where a.FlgActive == true select a;
                foreach (MstReports One in oCollection)
                {
                    DataRow dtRow = dtReports.NewRow();
                    dtRow["ID"] = One.ID;
                    dtRow["Code"] = One.RptCode;
                    dtRow["Name"] = One.RptName;
                    dtRow["ShowIn"] = One.ReportIn;
                    if (One.FlgLayout == true)
                    {
                        dtRow["Type"] = "Layout";
                        //txtName.Visible = true;
                        //cmbRptName.Visible = false;
                    }
                    else if (One.FlgReport == true)
                    {
                        dtRow["Type"] = "Report";
   
                    }
                    if (One.FlgDefault == true)
                    {
                        dtRow["Default"] = "Yes";
                        
                    }
                    else
                    {
                        dtRow["Default"] = "No";
                    }
                    dtReports.Rows.Add(dtRow);
                }
                grdReports.DataSource = dtReports;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ExportReport()
        {
            try
            {
                MstReports oRpt = (from a in oDB.MstReports where a.RptCode == txtCode.Text.Trim() select a).FirstOrDefault();
                if (oRpt != null)
                {
                    FolderBrowserDialog oFBD = new FolderBrowserDialog();
                    oFBD.ShowDialog();
                    if (!string.IsNullOrEmpty(oFBD.SelectedPath))
                    {
                        string FilePathToExport = Path.Combine(oFBD.SelectedPath, oRpt.RptName + ".rpt");
                        RadMessageBox.Show(FilePathToExport);
                        byte[] rptBytes = oRpt.RptFile.ToArray();

                        FileStream oFS = new FileStream(FilePathToExport, System.IO.FileMode.Create);
                        int len = rptBytes.Length;
                        oFS.Write(rptBytes, 0, len);
                        oFS.Flush();
                        oFS.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void RemoveReport()
        {
            try
            {
                MstReports oRpt = (from a in oDB.MstReports where a.RptCode == txtCode.Text.Trim() select a).FirstOrDefault();
                if (oRpt != null)
                {
                    oRpt.FlgActive = false;
                }
                oDB.SubmitChanges();
                FillGrid();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void DefaultReport()
        {
            try
            {
                MstReports oRpt = (from a in oDB.MstReports where a.RptCode == txtCode.Text.Trim() select a).FirstOrDefault();
                if (oRpt != null)
                {
                    if (Convert.ToBoolean(oRpt.FlgDefault))
                    {
                        oRpt.FlgDefault = false;
                    }
                    else
                    {
                        oRpt.FlgDefault = true;
                    }
                }
                oDB.SubmitChanges();
                FillGrid();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SelectFileForImport()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Report Files (*.rpt)|*.rpt";
                dialog.Title = "Select a report file";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (ValidReport(dialog.FileName))
                    {
                        // MessageBox.Show("report is ok");
                        txtFilePath.Text = dialog.FileName;

                    }
                    else
                    {
                        MessageBox.Show("report is not in correct format");
                        txtFilePath.Text = "";

                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private Boolean ValidReport(string rptPath)
        {
            Boolean retValue = false;
            try
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(rptPath);
                //ReportDoument rpt = new ReportDocument();
                //rpt.Load(rptPath);
                retValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                retValue = false;
            }
            return retValue;

        }

        #endregion

        #region Form Events

        public frmReportManagement()
        {
            InitializeComponent();
        }

        private void frmReportManagement_Load(object sender, EventArgs e)
        {
            try
            {
                FormInitiazliation();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
            if (btnCancel.Text == "&Close")
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
            }
            if (oResult == System.Windows.Forms.DialogResult.Yes)
            {
                this.Dispose();
                base.mytabpage.Dispose();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnImport.Text == "&Import")
                {
                    AddReport();
                }
                if (btnImport.Text == "&Update")
                {
                    UpdateReport();
                }
                ClearControls();
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportReport();
                ClearControls();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveReport();
            ClearControls();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            DefaultReport();
            ClearControls();
        }

        private void chkLayout_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            cmbMenu.Enabled = false;
            cmbSourceDoc.Enabled = true;
        }

        private void chlReport_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            cmbMenu.Enabled = true;
            cmbSourceDoc.Enabled = false;
        }

        private void btnpicker_click(object sender, EventArgs e)
        {
            //selectfileforimport();
        }

        private void grdReports_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    GridViewRowInfo row = grdReports.Rows[e.RowIndex];
                    FillReport(Convert.ToString(row.Cells[0].Value));
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        #endregion

        private void btnPicker_Click_1(object sender, EventArgs e)
        {
            SelectFileForImport();
        }

        private void chlReport_Click(object sender, EventArgs e)
        {
            try
            {
                cmbRptName.Visible = true;
                txtName.Visible = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void chkLayout_Click(object sender, EventArgs e)
        {
            try
            {
                cmbRptName.Visible = false;
                txtName.Visible = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}