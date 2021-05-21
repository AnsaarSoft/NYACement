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
using mfmFFSDB;
using mfmFFS.Dialog;

namespace mfmFFS.Screens
{
    public partial class frmWBS : frmBaseForm
    {

        #region Variable

        dbFFS oDI = null;
        int OpenObjectID = 1;

        #endregion

        #region Functions

        private void InitializeForm()
        {
            try
            {
                chkAscii.Visible = false;
                oDI = new dbFFS(Program.ConStrApp);
                byte Auth = Convert.ToByte(oDI.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "WeighBrigde").FirstOrDefault().GivenRight);
                if (Auth == 4)
                {
                    btnSubmit.Enabled = false;
                }
                bool super = Convert.ToBoolean(oDI.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                if (super == true)
                {
                    btnSubmit.Enabled = true;
                }
                // FillSeries();
            }
            catch (Exception Ex)
            {
            }
        }

        private Boolean AddRecord()
        {
            Boolean retValue = true;
            try
            {

                MstWeighBridge oNew = new MstWeighBridge();

                oNew.WBCode = txtWBCode.Text;

                oNew.ComPort = cmbCOMPort.SelectedItem.Text;
                oNew.BaudRate = cmbBaudRate.SelectedItem.Text;
                oNew.DataBits = cmbDataBits.SelectedItem.Text;
                oNew.Parity = cmbParity.SelectedItem.Text;
                oNew.StartChar = txtStarChar.Text;
                oNew.StopBits = cmbStopBit.SelectedItem.Text;
                oNew.Lenght = txtLenght.Text;
                oNew.Series = cmbSeries.SelectedItem.Text; 
                oNew.MachineName = txtMachine.Text;
                oNew.MachineIP = txtMachineIP.Text;
             
                oNew.CreatedDt = DateTime.Now;
                oNew.UpdatedDt = DateTime.Now;
                oNew.CreatedBy = Program.oCurrentUser.UserCode;
                oNew.UpdatedBy = Program.oCurrentUser.UserCode;

                oDI.MstWeighBridge.InsertOnSubmit(oNew);
                oDI.SubmitChanges();

            }
            catch (Exception Ex)
            {
                retValue = false;
            }
            return retValue;
        }

        private Boolean UpdateRecord()
        {
            Boolean retValue = true;
            try
            {

                MstWeighBridge oNew = (from a in oDI.MstWeighBridge where a.ID == OpenObjectID select a).FirstOrDefault();

                oNew.Series = cmbSeries.SelectedItem.Text;
              
                oNew.ComPort = cmbCOMPort.SelectedItem.Text;
                oNew.BaudRate = cmbBaudRate.SelectedItem.Text;
                oNew.DataBits = cmbDataBits.SelectedItem.Text;
                oNew.Parity = cmbParity.SelectedItem.Text;
                oNew.StartChar = txtStarChar.Text;
                oNew.StopBits = cmbStopBit.SelectedItem.Text;
                oNew.Lenght = txtLenght.Text;
              
                oNew.MachineName = txtMachine.Text;
                oNew.MachineIP = txtMachineIP.Text;
             

                oNew.UpdatedDt = DateTime.Now;
                oNew.UpdatedBy = Program.oCurrentUser.UserCode;

                oDI.SubmitChanges();

            }
            catch (Exception Ex)
            {
                retValue = false;
            }
            return retValue;
        }

        private void FillRecord()
        {
            try
            {
                MstWeighBridge oDoc = (from a in oDI.MstWeighBridge where a.ID == OpenObjectID select a).FirstOrDefault();
                OpenObjectID = Convert.ToInt32(oDoc.ID);
                txtWBCode.Text = oDoc.WBCode;
                cmbSeries.SelectedValue = Convert.ToString(oDoc.Series);
               // chkAscii.Checked = Convert.ToBoolean(oDoc.as);
                cmbCOMPort.SelectedValue = oDoc.ComPort;
                cmbBaudRate.SelectedValue = oDoc.BaudRate;
                cmbDataBits.SelectedValue = oDoc.DataBits;
                cmbParity.SelectedValue = oDoc.Parity;
                txtStarChar.Text = oDoc.StartChar;
                cmbStopBit.SelectedValue = oDoc.StopBits;
                txtLenght.Text = oDoc.Lenght;
             
                txtMachine.Text = oDoc.MachineName;
                txtMachineIP.Text = oDoc.MachineIP;
               
                Program.SuccesesMsg("Record Successfully Loaded.");
                btnSubmit.Text = "&Update";
            }
            catch (Exception Ex)
            {
            }
        }

        private Boolean RecordValidationAdd()
        {
            Boolean retValue = true;
            try
            {
                using (dbFFS oDC = new dbFFS(Program.ConStrApp))
                {
                    //Check Already WB record
                    Int32 count = (from a in oDC.MstWeighBridge where a.WBCode == txtWBCode.Text select a).Count();
                    if (count > 0)
                    {
                        retValue = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                retValue = false;
            }
            return retValue;
        }

        private Boolean RecordValidationUpdate()
        {
            Boolean retValue = true;
            try
            {
                using (dbFFS oDC = new dbFFS(Program.ConStrApp))
                {
                    //Check Already WB record
                    Int32 count = (from a in oDC.MstWeighBridge where a.WBCode == txtWBCode.Text select a).Count();
                    if (count == 0)
                    {
                        retValue = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                retValue = false;
            }
            return retValue;
        }

        private void InitiallizeDocument()
        {
            try
            {
                txtWBCode.Text = "";
                txtStarChar.Text = "";
                txtMachineIP.Text = "";
                txtMachine.Text = "";
                txtLenght.Text = "";
                cmbBaudRate.Text = "";
                cmbCOMPort.Text = "";
                cmbDataBits.Text = "";
                cmbParity.Text = "";
                cmbSeries.Text = "";
                cmbStopBit.Text = "";


                btnSubmit.Text = "&Add";
            }
            catch (Exception Ex)
            {
            }

        }

        private void HandleDialogControl()
        {
            try
            {
                //frmOpenDlg oDlg = new frmOpenDlg();
                //oDlg.OpenFor = "WBSetting";
                //oDlg.ShowDialog();
                //if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                //{
                //    OpenObjectID = oDlg.SelectedObjectID;
                //}
                //if (!String.IsNullOrEmpty(OpenObjectID))
                //{
                //    FillRecord();
                //}
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void FillSeries()
        {
            try
            {
                IEnumerable<MstLov> oCollection = from a in oDI.MstLov where a.Type == "Bridges" select a;
                cmbSeries.DataSource = oCollection;
                cmbSeries.DisplayMember = "cValue";
                cmbSeries.ValueMember = "cCode";
            }
            catch (Exception Ex)
            {
            }
        }

#endregion

        #region Form Events

        public frmWBS()
        {
            InitializeComponent();
        }

        private void frmWBS_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeForm();
                InitiallizeDocument();
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSubmit.Text == "&Add")
                {
                    if (RecordValidationAdd())
                    {
                        if (AddRecord())
                        {
                            InitiallizeDocument();
                        }
                    }
                }
                if (btnSubmit.Text == "&Update")
                {
                    if (RecordValidationUpdate())
                    {
                        if (UpdateRecord())
                        {
                            InitiallizeDocument();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult oResult;
            if (btnSubmit.Text != "Add")
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. You lose all entered information.", "Confirmation.", MessageBoxButtons.YesNo);
            }
            else
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
            }
            if (oResult == System.Windows.Forms.DialogResult.Yes)
            {
                this.Dispose();
                base.mytabpage.Dispose();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //HandleDialogControl();
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                InitiallizeDocument();
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnFirstRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDI.MstWeighBridge select a.ID).First();

                if (MaxRecord != 0)
                {
                    OpenObjectID = Convert.ToInt32(MaxRecord);
                    FillRecord();

                    Program.WarningMsg("Reached To First Record");
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnPreviosRecord_Click(object sender, EventArgs e)
        {
            try
            {
                int DocNum = OpenObjectID;
                OpenObjectID = DocNum - 1;
                int checkDoc = 0;
                checkDoc = (from a in oDI.MstWeighBridge where a.ID == OpenObjectID select a).Count();

                if (checkDoc > 0)
                {
                    FillRecord();

                }
                else
                {
                    Program.WarningMsg("Reached To First Record");
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnNextRecord_Click(object sender, EventArgs e)
        {
            try
            {
                int DocNum = OpenObjectID;
                OpenObjectID = DocNum + 1;
                int checkDoc = 0;
                checkDoc = (from a in oDI.MstWeighBridge where a.ID == OpenObjectID select a).Count();

                if (checkDoc > 0)
                {
                    FillRecord();

                }
                else
                {
                    Program.WarningMsg("Reached To Last Record");
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnLastRecord_Click(object sender, EventArgs e)
        {
            try
            {

                var MaxRecord = (from a in oDI.MstWeighBridge select a.ID).Max();

                if (MaxRecord != 0)
                {
                    OpenObjectID = Convert.ToInt32(MaxRecord);
                    FillRecord();

                    Program.WarningMsg("Reached To Last Record");
                }
            }
            catch (Exception Ex)
            {
            }
        }

        #endregion

    }
}
