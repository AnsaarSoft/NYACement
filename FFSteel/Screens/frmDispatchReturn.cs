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
using System.Threading;

namespace mfmFFS.Screens
{
    public partial class frmDispatchReturn : frmBaseForm
    {

        #region Variable

        string OpenObjectID = "";
        dbFFS oDB = null;
        string ActiveSeries = "";
        string ComPort, BaudRate, Parity, StopBit, StartChar, DataBits, DataLenght, IndicatorType;
        Boolean flgAscii = false;
        Boolean flgToggle = false;
        Int64 mostRecentWeight = 0;
        Boolean isLoadingImageFront = false;
        Boolean isLoadingImageBack = false;
        int FormState = 0;
        DataTable dtRows = null;
        private SerialPort comportDispatchR = new SerialPort();
        Boolean alreadyReading = false;
        DataTable dt = new DataTable();
        string ItemGroupCode = "";
        string shiftName = "";
        long shiftid = 0;
        bool flgSetValues = false;
        decimal val1 = 0;
        decimal val2 = 0;
        int ZeroHit = 0;
        long currentwt = 0;
        delegate void delCWeight(string value);
        #endregion

        #region Events

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                Program.DocNum = null;
                HandleDialogSearch();
                if (Program.DocNum != null)
                {
                    setValues(Convert.ToInt32(Program.DocNum));
                    txtDriverCnic.Enabled = true;
                    txtDriverName.Enabled = true;
                    txtTransportName.Enabled = false;
                    txtVehicleRegNo.Enabled = true;
                    // cm.Enabled = false;
                    cmbTransportType.Enabled = true;
                    cmbTransportCode.Enabled = true;
                    flgSetValues = false;
                    btnSubmit.Text = "&Update";
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                }
                else
                {
                    btnItem.Enabled = false;
                    radButton5.Enabled = false;
                    btnSubmit.Enabled = false;
                }
                txtCWeight.Enabled = false;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        private void btnLastRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDB.TrnsDispatchReturn where  a.FlgSecondWeight == true && a.FlgPosted == null select a.DocNum).Max();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    txtDriverCnic.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleRegNo.Enabled = false;
                    // cm.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
                    btnSubmit.Text = "&Update";
                    flgSetValues = false;
                  
                    txtCWeight.Enabled = false;
                    Program.WarningMsg("Reached To Last Record");
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                }
                else
                {
                    btnItem.Enabled = false;
                    radButton5.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnNextRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                int DocNum = Convert.ToInt32(txtVoucherNo.Text);
                int previusDocNum = DocNum + 1;
                int checkDoc = 0;
                checkDoc = (from a in oDB.TrnsDispatchReturn where a.DocNum == previusDocNum && a.FlgSecondWeight == true && a.FlgPosted == null select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    txtDriverCnic.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleRegNo.Enabled = false;
                    // cm.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
                    btnSubmit.Text = "&Update";
                    flgSetValues = false;
                 
                    txtCWeight.Enabled = false;
                }
                else
                {
                    Program.WarningMsg("Reached To Last Record");
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                }
                else
                {
                    btnItem.Enabled = false;
                    radButton5.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void btnPreviosRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                int DocNum = Convert.ToInt32(txtVoucherNo.Text);
                int previusDocNum = DocNum - 1;
                int checkDoc = 0;
                checkDoc = (from a in oDB.TrnsDispatchReturn where a.DocNum == previusDocNum && a.FlgSecondWeight == true && a.FlgPosted == null select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    txtDriverCnic.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleRegNo.Enabled = false;
                    // cm.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
                    btnSubmit.Text = "&Update";
                    flgSetValues = false;
                  
                    txtCWeight.Enabled = false;
                }
                else
                {
                    Program.WarningMsg("Reached To First Record");
                }

                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                }
                else
                {
                    btnItem.Enabled = false;
                    radButton5.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnFirstRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDB.TrnsDispatchReturn where  a.FlgSecondWeight == true && a.FlgPosted == null select a.DocNum).First();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    txtDriverCnic.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleRegNo.Enabled = false;
                    // cm.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
                    btnSubmit.Text = "&Update";
                    flgSetValues = false;
                   
                    txtCWeight.Enabled = false;
                    Program.WarningMsg("Reached To First Record");
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                }
                else
                {
                    btnItem.Enabled = false;
                    radButton5.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void txtVoucherNo_TextChanged(object sender, EventArgs e)
        {
            if (Program.sealdt != null)
            {
                Program.sealdt.Clear();
            }
        }

        private void txtFullText_TextChanged(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txt1WeightKg.Text) || txt1WeightKg.Text == "0")
            //{
            //    txt1WeightKg.Text = txtFullText.Text;
            //}
            //else if (flgSetValues)
            //{
            //    txt2WeightKG.Text = txtFullText.Text;
            //}
            if (flgSetValues)
            {
                txt2WeightKG.Text = txtFullText.Text;
            }
            else if (string.IsNullOrEmpty(txt2WeightKG.Text))
            {
                txt1WeightKg.Text = txtFullText.Text;
            }
            lblWeight.Text = txtFullText.Text;
        }
        private void btnItem_Click(object sender, EventArgs e)
        {
            try
            {
                HandleDialogControlOrder();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        private void tmrAlreadyReading_Tick(object sender, EventArgs e)
        {
            alreadyReading = false;
        }
        private void btnBulkerSeal_Click(object sender, EventArgs e)
        {

            try
            {
                HandleDialogControlBulkSealer();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        private void btnAddNew_Click_1(object sender, EventArgs e)
        {
            InitiallizeDocument();
            flgSetValues = false;
            btnSubmit.Text = "&Add";
           // btnSubmit.Enabled = true;
            btnItem.Enabled = true;
            radButton5.Enabled = true;
            btnSubmit.Enabled = true;
            txtDriverCnic.Enabled = true;
            txtDriverName.Enabled = true;
            txtTransportName.Enabled = false;
            txtVehicleRegNo.Enabled = true;
             //.Enabled = false;
            cmbTransportType.Enabled = true;
            cmbTransportCode.Enabled = true;
            //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);


        }

        private void txt1WeightKg_TextChanged(object sender, EventArgs e)
        {
            if (flgSetValues == true)
            {
                lblWeight.Text = "";
            }
            else
            {
                lblWeight.Text = txt1WeightKg.Text;
            }

            if (!string.IsNullOrEmpty(txt1WeightKg.Text))
            {
                txt1WeightTons.Text = Convert.ToString(Convert.ToDecimal(txt1WeightKg.Text) / 1000);
            }
            else
            {
                txt1WeightTons.Text = null;
            }
        }

        private void txt2WeightKG_TextChanged(object sender, EventArgs e)
        {
            if (flgSetValues == false)
            {
                lblWeight.Text = "";
            }
            else
            {
                lblWeight.Text = txt2WeightKG.Text;
            }

            if (!string.IsNullOrWhiteSpace(txt2WeightKG.Text))
            {
                if (!string.IsNullOrEmpty(txt2WeightKG.Text))
                {
                    txt2WeightTons.Text = Convert.ToString(Convert.ToDecimal(txt2WeightKG.Text) / 1000);
                    txtNetWeightKG.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightKg.Text) - Convert.ToDecimal(txt2WeightKG.Text)));
                    txtNetWeightTons.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightTons.Text) - Convert.ToDecimal(txt2WeightTons.Text)));
                    txtDifferenceNWTons.Text = Convert.ToString(Convert.ToDecimal(txtDOQuantity.Text) - Convert.ToDecimal(txtNetWeightTons.Text));
                }
            }
            else
            {
                txt2WeightTons.Text = null;
                txtNetWeightKG.Text = null;
                txtNetWeightTons.Text = null;
                txtDifferenceNWTons.Text = null;
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
        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                HandleDialogControlItems();
                txtDaySerial.Text = Convert.ToString(daySeries());
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                frmOpenDlg od = new frmOpenDlg();
                string critaria = txtVoucherNo.Text;
                string PldFor = Program.Screen;

                var Doc = oDB.TrnsDispatchReturn.Where(x => x.DocNum == Convert.ToInt32(critaria)).FirstOrDefault();

                if (btnSubmit.Text == "&Add")
                {
                    if (AddRecord())
                    {
                        var dispatchRetAdd = oDB.TrnsDispatchReturn.Where(x => x.DocNum == Convert.ToInt32(critaria)).FirstOrDefault();
                        if (od.AddSealToDb())
                        {
                            if (Program.OpenLayout(PldFor, critaria, PldFor + " " + critaria))
                            {
                                try
                                {
                                    dispatchRetAdd.Flg1Rpt = true;

                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            oDB.SubmitChanges();
                            InitiallizeDocument();
                            LoadGrid();
                        }
                    }
                }

                if (btnSubmit.Text == "&Update")
                {

                    string flgSecondWeight = Convert.ToString(Doc.FlgSecondWeight);
                    if (AddRecord())
                    {
                        if (od.AddSealToDb())
                        {
                            if (flgSecondWeight == "False")
                            {
                                if (Program.OpenLayout(PldFor, critaria, PldFor + " " + critaria))
                                {
                                    Doc.Flg2Rpt = true;
                                    oDB.SubmitChanges();
                                }
                            }

                            InitiallizeDocument();
                            LoadGrid();
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.ToString());
            }

            //try
            //{
            //    frmOpenDlg od = new frmOpenDlg();
            //    if (btnSubmit.Text == "&Add")
            //    {
            //        if (AddRecord())
            //        {
            //            if (od.AddSealToDb())
            //            {
            //                string critaria = txtVoucherNo.Text;
            //                string PldFor = Program.Screen;
            //                Program.OpenLayout(PldFor, critaria, PldFor + " " + txtVoucherNo.Text);
            //                InitiallizeDocument();
            //                LoadGrid();
            //            }
            //        }
            //    }

            //    if (btnSubmit.Text == "&Update")
            //    {
            //        if (AddRecord())
            //        {
            //            if (od.AddSealToDb())
            //            {
            //                InitiallizeDocument();
            //                LoadGrid();
            //            }
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    Program.ExceptionMsg(Ex.ToString());
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try {
                DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
                if (btnCancel.Text == "Close")
                {
                    oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
                }
                if (oResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //comportDispatchR.Close();

                    if (comportDispatchR.IsOpen)
                    {
                        comportDispatchR.DiscardInBuffer();
                        comportDispatchR.DiscardOutBuffer();
                        comportDispatchR.Close();

                        // Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 103");
                    }
                    Program.flgIndicator = false;
                    this.Dispose();
                    base.mytabpage.Dispose();

                }
            }
            catch(Exception ex)
            {

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string critaria = " WHERE GatePass IN ('" + txtDate.Text + "') ";
                //string PldFor = cmbSourceDoc.SelectedItem.Text;
                //Program.OpenLayout(PldFor, critaria, PldFor + " " + txtGatePass.Text);
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void grdDetails_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            int DocNum = 0;
            DocNum = Convert.ToInt32(e.Row.Cells["Wmnt#"].Value);
            setValues(DocNum);
            txtDriverCnic.Enabled = false;
            txtDriverName.Enabled = false;
            txtTransportName.Enabled = false;
            txtVehicleRegNo.Enabled = false;
           // cm.Enabled = false;
            cmbTransportType.Enabled = false;
            cmbTransportCode.Enabled = false;
           // txtOrderQuantity.Enabled = false;
            txtDOQuantity.Enabled = false;
            bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
            if (super == true)
            {
                foreach (Control o in this.Controls)
                {
                    // special handling for the menu
                    o.Enabled = true;

                }
            }
            else
            {
                //btnPOItem.Enabled = true;
                //btnDocNum.Enabled = true;
                btnSubmit.Enabled = true;
            }
            txtCWeight.Enabled = false;
            // comportDispatchR.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            btnSubmit.Text = "&Update";
            // flgSetValues = false;

        }
        #endregion

        #region Functions

        private void InitiallizeForm()
        {
            try
            {
                Program.NoMsg("");
                Program.flgIndicator = true;
                dt.Clear();
                //dtRows.Rows.Clear();
                oDB = new dbFFS(Program.ConStrApp);
                GetMachineSetting();
                FillTransportCode();
                FillTransporttype();
                CreateDt1();
                InitiallizeDocument();

                LoadGrid();
                txt2WeightDate.Text = "";
                txt2WeightKG.Text = "";
                txt2WeightTime.Text = "";
                txt2WeightTons.Text = "";
                txtNetWeightKG.Text = "";
                txtNetWeightTons.Text = "";
                txtDifferenceNWTons.Text = "";

                txtVoucherNo.Enabled = false;
                txtDONum.Enabled = false;
                txtDODATE1.Enabled = false;
                txtShift.Enabled = false;
                txtCusName.Enabled = false;
                txtCusCode.Enabled = false;
                txtItemCode.Enabled = false;
                txtItemGrpName.Enabled = false;
                txtItemName.Enabled = false;
                txtBalQuan.Enabled = false;
                txtDOQuantity.Enabled = true;
                txtDate.Enabled = false;
                txtTime.Enabled = false;
                txtDaySerial.Enabled = false;

                txt1WeightDate.Enabled = false;
                txt1WeightKg.Enabled = false;
                txt1WeightTime.Enabled = false;
                txt1WeightTons.Enabled = false;
                txt2WeightDate.Enabled = false;
                txt2WeightKG.Enabled = false;
                txt2WeightTime.Enabled = false;
                txt2WeightTons.Enabled = false;
                txtNetWeightKG.Enabled = false;
                txtNetWeightTons.Enabled = false;
                txtDifferenceNWTons.Enabled = false;
                txtFullText.Visible = false;
                tmrAlreadyReading.Interval = 1000;
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "DispatchReturn").FirstOrDefault().GivenRight);
                if (Auth == 4)
                {
                    btnSubmit.Enabled = false;
                }
                //if (Auth == 3)
                //{
                //    txtVoucherNo.Enabled = true;
                //    //txtSBRNum.Enabled = true;
                //    // txtSBRDate.Enabled = true;
                //    txtShift.Enabled = true;
                //    txtCusCode.Enabled = true;
                //    txtCusName.Enabled = true;
                //    txtItemCode.Enabled = true;
                //    txtItemGrpName.Enabled = true;
                //    txtItemName.Enabled = true;
                //    txtBalQuan.Enabled = true;
                //    txtReturnQuan.Enabled = true;
                //    txtDate.Enabled = true;
                //    txtTime.Enabled = true;
                //    txtDaySerial.Enabled = true;

                //    //txt1WeightDate.Enabled = true;
                //    txt1WeightKg.Enabled = true;
                //    //txt1WeightTime.Enabled = true;
                //    txt1WeightTons.Enabled = true;
                //    // txt2WeightDate.Enabled = true;
                //    txt2WeightKG.Enabled = true;
                //    //txt2WeightTime.Enabled = true;
                //    txt2WeightTons.Enabled = true;
                //    txtNetWeightKG.Enabled = true;
                //    txtNetWeightTons.Enabled = true;
                //    txtDifferenceNWTons.Enabled = true;
                //}
                //if (Auth == 5)
                //{
                //    txtVoucherNo.Enabled = false;
                //    txtDONum.Enabled = false;
                //    txtDODATE1.Enabled = false;
                //    txtShift.Enabled = false;
                //    txtCusCode.Enabled = false;
                //    txtCusName.Enabled = false;
                //    txtItemCode.Enabled = false;
                //    txtItemGrpName.Enabled = false;
                //    txtItemName.Enabled = false;
                //    txtBalQuan.Enabled = false;
                //    txtDOQuantity.Enabled = false;
                //    txtReturnQuan.Enabled = false;
                //    txtDate.Enabled = false;
                //    txtTime.Enabled = false;
                //    txtDaySerial.Enabled = false;

                //    txt1WeightDate.Enabled = false;
                //    txt1WeightKg.Enabled = false;
                //    txt1WeightTime.Enabled = false;
                //    txt1WeightTons.Enabled = false;
                //    txt2WeightDate.Enabled = false;
                //    txt2WeightKG.Enabled = false;
                //    txt2WeightTime.Enabled = false;
                //    txt2WeightTons.Enabled = false;
                //    txtNetWeightKG.Enabled = false;
                //    txtNetWeightTons.Enabled = false;
                //    txtDifferenceNWTons.Enabled = false;
                //    btnSubmit.Enabled = false;
                //    radButton5.Enabled = false;
                //    btnItem.Enabled = false;
                //    btnNextRecord.Enabled = false;
                //    btnLastRecord.Enabled = false;
                //    btnPreviosRecord.Enabled = false;
                //    btnLastRecord.Enabled = false;
                //    btnSearch.Enabled = false;
                //    btnAddNew.Enabled = false;

                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = false;

                //    }
                //}

                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                    //txtVoucherNo.Enabled = true;
                    //txtDODATE1.Enabled = true;
                    //txtDONum.Enabled = true;
                    //txtShift.Enabled = true;
                    //txtCusCode.Enabled = true;
                    //txtCusName.Enabled = true;
                    //txtItemCode.Enabled = true;
                    //txtItemGrpName.Enabled = true;
                    //txtItemName.Enabled = true;
                    //txtBalQuan.Enabled = true;
                    //txtReturnQuan.Enabled = true;
                    //txtDOQuantity.Enabled = true;
                    //txtDate.Enabled = true;
                    //txtTime.Enabled = true;
                    //txtDaySerial.Enabled = true;

                    //txt1WeightDate.Enabled = true;
                    //txt1WeightKg.Enabled = true;
                    //txt1WeightTime.Enabled = true;
                    //txt1WeightTons.Enabled = true;
                    //txt2WeightDate.Enabled = true;
                    //txt2WeightKG.Enabled = true;
                    //txt2WeightTime.Enabled = true;
                    //txt2WeightTons.Enabled = true;
                    //txtNetWeightKG.Enabled = true;
                    //txtNetWeightTons.Enabled = true;
                    //txtDifferenceNWTons.Enabled = true;
                    //btnSubmit.Enabled = true;
                    //btnItem.Enabled = true;
                    //radButton5.Enabled = true;
                    //btnNextRecord.Enabled = true;
                    //btnLastRecord.Enabled = true;
                    //btnPreviosRecord.Enabled = true;
                    //btnLastRecord.Enabled = true;
                    //btnSearch.Enabled = true;
                    //btnAddNew.Enabled = true;
                }
                txtCWeight.Enabled = false;
                txtPathCnicDriver.Enabled = false;
                txtTransportName.Enabled = false;
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
                if (dt.Columns.Count == 0)
                {
                    dtRows = new DataTable();
                    dtRows.Columns.Add("ID");
                    dtRows.Columns.Add("Wmnt#");
                    dtRows.Columns.Add("ItemCode");
                    dtRows.Columns.Add("ItemName");
                    dtRows.Columns.Add("Gross");
                    dtRows.Columns.Add("Tare");
                    dtRows.Columns.Add("NetWeight");
                    dtRows.Columns.Add("Status");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void InitiallizeDocument()
        {
            try
            {
                txtVoucherNo.Text = GetDocNumber();
                txtDate.Text = DateTime.Now.ToShortDateString();
                txtTime.Text = DateTime.Now.ToShortTimeString();
                txtDONum.Text = "";
                txtCusName.Text = "";
                txtCusCode.Text = "";
                txtDODATE1.Text = "";
                txtVoucherNo.Enabled = false;
                txtShift.Text = shiftName;
                txtItemCode.Text = "";
                txtItemName.Text = "";
                txtBalQuan.Text = "";
                txtDOQuantity.Text = "";
                txtItemGrpName.Text = "";
                txtReturnQuan.Text = "";
                txtVehicleRegNo.Text = "";
                txtDaySerial.Text = "";
                txtDriverCnic.Text = "";
                txtDriverName.Text = "";
                txt1WeightDate.Text = DateTime.Now.ToShortDateString();
                txt1WeightTime.Text = DateTime.Now.ToShortTimeString();
                txt1WeightKg.Text = "";
                txt1WeightTons.Text = "";
                cmbTransportCode.Text = "";
                cmbTransportType.Text = "";

                txtTransportName.Text = "";
                txtPathBrand.Text = "";
                txtPathCnicDriver.Text = "";
                txt2WeightDate.Text = DateTime.Now.ToShortDateString();
                txt2WeightTime.Text = DateTime.Now.ToShortTimeString();
                txt2WeightKG.Text = "";
                txt2WeightTons.Text = "";
                txtNetWeightKG.Text = "";
                txtNetWeightTons.Text = "";
                flgSetValues = false;
                txtDOQuantity.Enabled = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GetMachineSetting()
        {
            try
            {
                MstWeighBridge oDoc = (from a in oDB.MstWeighBridge where a.MachineName == System.Environment.MachineName select a).FirstOrDefault();
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

                    ActiveSeries = oDoc.Series;

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

        private Boolean AddRecord()
        {
            Boolean retValue = true;
            try
            {

                //string value = txtVoucherNo.Text;
                //string purpose = cmbSourceDoc.SelectedItem.Text;
                TrnsDispatchReturn oDoc = new TrnsDispatchReturn();

                oDoc.FCreateDate = DateTime.Now;
                oDoc.FCreateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateDate = DateTime.Now;

                var getTrnsDispatch = oDB.TrnsDispatchReturn.Where(x => x.DocNum == Convert.ToInt32(txtVoucherNo.Text));

                if (getTrnsDispatch.Count() > 0)
                {
                    var oOld = getTrnsDispatch.FirstOrDefault();
                    //   oOld.DocNum = Convert.ToInt32(txtVoucherNo.Text);
                    //oOld.FDocDate = Convert.ToDateTime(txtDate.Text);
                    //oOld.FShift = Convert.ToInt32(shiftid);
                    //oOld.FTime = txtTime.Text;
                    oOld.DONum = txtDONum.Text;
                    oOld.DODate = Convert.ToDateTime(txtDODATE1.Text);
                    oOld.Flg2Rpt = false;
                    oOld.FlgWBrpt = false;
                    oOld.CustomerCode = txtCusCode.Text;
                    oOld.CustomerName = txtCusName.Text;
                    oOld.ItemCode = txtItemCode.Text;
                    oOld.ItemName = txtItemName.Text;
                    if (!ChkTolerence())
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(txtDOQuantity.Text) || Convert.ToDecimal(txtDOQuantity.Text) == 0)
                    {
                        Program.WarningMsg("DO Quantity cannot b empty or 0");
                        return false;
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtDOQuantity.Text) > Convert.ToDecimal(txtBalQuan.Text))
                        {
                            Program.WarningMsg("Delivery Order Quantity cannot b greater than Balance quantity");
                            return false;
                        }
                        else
                        {
                            oOld.DOQuantity = Convert.ToDecimal(txtDOQuantity.Text);
                        }
                    }
                    oOld.VehicleNum = txtVehicleRegNo.Text;
                    oOld.DriverCNIC = txtDriverCnic.Text;
                    oOld.DriverName = txtDriverName.Text;
                    oOld.DriverDocument = txtPathCnicDriver.Text;
                    //oOld.FWeighmentDate = Convert.ToDateTime(txt1WeightDate.Text);
                    //oOld.FWeighmentTime = txt1WeightTime.Text;
                    if (Convert.ToDecimal(txt1WeightKg.Text) > 0)

                    {
                        if (!string.IsNullOrEmpty(txt1WeightKg.Text))
                        {
                            oOld.FWeighmentTon = Convert.ToDecimal(txt1WeightTons.Text);
                            oOld.FWeighmentKG = Convert.ToDecimal(txt1WeightKg.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("First Weight can not be Empty");
                            return false;
                        }
                    }
                    else
                    {
                        Program.ExceptionMsg("First Weight can not be 0");
                        return false;
                    }

                    if (Convert.ToDecimal(txt2WeightKG.Text) > 0)
                    {
                        oOld.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                        oOld.SWeighmentTon = Convert.ToDecimal(txt2WeightTons.Text);
                    }
                    else
                    {
                        Program.ExceptionMsg("Second Weight can not be 0");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportType.SelectedValue)))
                    {
                        oOld.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.SelectedValue)))
                    {
                        oOld.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                    }

                    oOld.TransportName = txtTransportName.Text;



                    if (!string.IsNullOrEmpty(txt2WeightKG.Text))
                    {
                        oOld.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                    }
                    else
                    {
                        return true;
                    }

                    if (!string.IsNullOrEmpty(txt2WeightTons.Text))
                    {
                        oOld.SWeighmentTon = Convert.ToDecimal(txt2WeightTons.Text);
                    }
                    else
                    {
                        return true;
                    }
                    if (string.IsNullOrEmpty(oOld.SWeighmentKG.ToString()))
                    {

                        oOld.SWeighmentDate = Convert.ToDateTime(txt2WeightDate.Text);
                        oOld.SWeighmentTime = txt2WeightTime.Text;
                        oOld.SDocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOld.SShift = Convert.ToInt32(txtShift.Text);
                        oOld.STime = txt2WeightTime.Text;
                    }
                    oOld.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                    oOld.NetWeightTon = Convert.ToDecimal(txtNetWeightTons.Text);
                    oOld.ReturnQuantity = Convert.ToDecimal(txtReturnQuan.Text);

                    if (string.IsNullOrEmpty(oOld.SCreateBy))
                    {
                        oOld.SCreateBy = Program.oCurrentUser.UserCode;
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(oOld.SCreateDate)))
                    {
                        oOld.SCreateDate = (DateTime.Now);
                    }

                    oOld.SUpdateBy = Program.oCurrentUser.UserCode;
                    oOld.SUpdateDate = (DateTime.Now);
                    //oOld.ItemGroupCode = txtItemCode.Text;
                    //oOld.ItemGroupName = txtItemGrpName.Text;
                    oOld.BalanceQuantity = Convert.ToDecimal(txtBalQuan.Text);
                    if (!string.IsNullOrWhiteSpace(txt2WeightKG.Text) && !string.IsNullOrEmpty(txt2WeightKG.Text))
                    {
                        oOld.FlgSecondWeight = true;
                    }
                    else
                    {
                        Program.SuccesesMsg("Second Weight can not be empty");
                        return false;
                    }

                    //oDB.TrnsDispatchReturn.InsertOnSubmit(oDoc);
                }
                else
                {
                    oDoc.DocNum = Convert.ToInt32(txtVoucherNo.Text);
                    oDoc.FDocDate = Convert.ToDateTime(txtDate.Text);
                    if (!string.IsNullOrEmpty(txtShift.Text))
                    {
                        oDoc.FShift = Convert.ToInt32(shiftid);

                    }
                    else
                    {
                        Program.ExceptionMsg("Shift can not be Empty");
                        return false;
                    }
                    oDoc.Flg1Rpt = false;
                    oDoc.FTime = txtTime.Text;
                    oDoc.DONum = txtDONum.Text;
                    oDoc.DODate = Convert.ToDateTime(txtDODATE1.Text);
                    oDoc.CustomerCode = txtCusCode.Text;
                    oDoc.CustomerName = txtCusName.Text;
                    oDoc.ItemCode = txtItemCode.Text;
                    oDoc.ItemName = txtItemName.Text;
                    if (string.IsNullOrEmpty(txtDOQuantity.Text) || Convert.ToDecimal(txtDOQuantity.Text) == 0)
                    {
                        Program.WarningMsg("DO Quantity cannot b empty or 0");
                        return false;
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtDOQuantity.Text) > Convert.ToDecimal(txtBalQuan.Text))
                        {
                            Program.WarningMsg("Delivery Order Quantity cannot b greater than Balance quantity");
                            return false;
                        }
                        else
                        {
                            oDoc.DOQuantity = Convert.ToDecimal(txtDOQuantity.Text);
                        }
                    }
                    oDoc.VehicleNum = txtVehicleRegNo.Text;
                    oDoc.DriverCNIC = txtDriverCnic.Text;
                    oDoc.DriverName = txtDriverName.Text;
                    oDoc.DriverDocument = txtPathCnicDriver.Text;
                    oDoc.FWeighmentDate = Convert.ToDateTime(txt1WeightDate.Text);
                    oDoc.FWeighmentTime = txt1WeightTime.Text;
                    if (Convert.ToDecimal(txt1WeightKg.Text) > 0)

                    {
                        if (!string.IsNullOrEmpty(txt1WeightKg.Text))
                        {
                            oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTons.Text);
                            oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKg.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("First Weight can not be Empty");
                            return false;
                        }
                    }
                    else
                    {
                        Program.ExceptionMsg("First Weight can not be 0");
                        return false;
                    }
                    oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                    oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                    oDoc.TransportName = txtTransportName.Text;
                    oDoc.FlgSecondWeight = false;

                    oDoc.DaySeries = Convert.ToInt32(txtDaySerial.Text);
                    //oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                    //oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTons.Text);
                    //oDoc.SWeighmentDate = DateTime.Now;
                    //oDoc.SWeighmentTime = txt2WeightTime.Text;
                    //oDoc.SDocDate = DateTime.Now;
                    //oDoc.SShift = Convert.ToInt32(shiftid);
                    //oDoc.STime = txt2WeightTime.Text;
                    //oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                    //oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTons.Text);
                    if (!string.IsNullOrEmpty(txtReturnQuan.Text))
                    {
                        oDoc.ReturnQuantity = Convert.ToDecimal(txtReturnQuan.Text);

                    }
                    else
                    {
                        oDoc.ReturnQuantity = 0;
                    }
                    //oDoc.SCreateBy = Program.oCurrentUser.UserCode;
                    //oDoc.SCreateDate = DateTime.Now;
                    //oDoc.SUpdateBy = Program.oCurrentUser.UserCode;
                    //oDoc.SUpdateDate = DateTime.Now;
                    oDoc.ItemGroupCode = ItemGroupCode;
                    oDoc.ItemGroupName = txtItemGrpName.Text;
                    if (!string.IsNullOrEmpty(txtBalQuan.Text))
                    {
                        oDoc.BalanceQuantity = Convert.ToDecimal(txtBalQuan.Text);
                    }
                    else
                    {
                        oDoc.BalanceQuantity = 0;
                    }


                    oDB.TrnsDispatchReturn.InsertOnSubmit(oDoc);
                }
                oDB.SubmitChanges();
                Program.SuccesesMsg("Record Successfully Added.");
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
                Program.ExceptionMsg("Values are Missing or Incorrect.");
                retValue = false;
            }

            return retValue;
        }

        private void HandleDialogControlOrder()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oDoOrder";
                // oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.Text = "Order Selector";
                oDlg.SourceDocNum = txtCusCode.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtDODATE1.Text = oDlg.DocDate;

                    txtDONum.Text = oDlg.DocNum;
                    Program.DocNum = txtDONum.Text;

                    txtCusCode.Text = oDlg.CustomerCode;
                    txtCusName.Text = oDlg.CustomerName;
                    txtPathBrand.Text = oDlg.Image;
                }
                oDlg.Dispose();

            }
            catch (Exception Ex)
            {
                //Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlItems()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oDoItems";
                // oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtCusCode.Text;
                oDlg.Text = "Item Selector";
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtBalQuan.Text = oDlg.Balance;
                    // txtSBRDate.Text = oDlg.DocDate;
                    //txtDOQuantity.Text = oDlg.Quantity;
                    // txtSBRNum.Text = oDlg.DocNum;

                    //Code By : BILAL AHMED
                    //Chaneged By : Ghulam
                    //Date : 04-10-2019
                    //txtVehicleRegNo.Text = oDlg.U_VehcleNo;
                    //txtDriverCnic.Text = oDlg.DriverCnic;
                    //txtDriverName.Text = oDlg.U_Driver;

                    txtItemCode.Text = oDlg.ItemCode;
                    txtItemName.Text = oDlg.Dscription;
                    txtItemGrpName.Text = oDlg.ItmsGrpNam;
                    ItemGroupCode = oDlg.ItmsGrpCod;
                   

                    txtPathBrand.Text = oDlg.Image;
                }
                oDlg.Dispose();

            }
            catch (Exception Ex)
            {
                //Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        public frmDispatchReturn()
        {
            InitializeComponent();
            //txtCWeight.Visible = false;
            txtCWeight.Enabled = false;
            comportDispatchR.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived); ;
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            try
            {
                string critaria = txtVoucherNo.Text;
                string PldFor = Program.Screen;
                string Fwmnt = txt1WeightKg.Text;
                if (!string.IsNullOrEmpty(Fwmnt))
                {
                    if (lbl2.Text == "True")
                    {
                        Program.OpenLayout(PldFor, critaria, PldFor + " " + txtVoucherNo.Text);
                        Program.OpenLayout("WayBridgeDelivery", critaria, "WayBridgeDelivery " + txtVoucherNo.Text);
                    }
                    else
                    {
                        Program.OpenLayout(PldFor, critaria, PldFor + " " + txtVoucherNo.Text);
                    }
                }
                else
                {
                    Program.ExceptionMsg("Kindly select any document first");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open for Driver ID Picture";

            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                txtPathCnicDriver.Text = theDialog.FileName.ToString();
            }
        }

        private void txt2WeightKG_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "&Update";
        }

        private void cmbTransportCode_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                string[] CardName = cmbTransportCode.Text.Split(':');
                string CArdName1 = CardName[1];
                DataTable val = new DataTable();
                string strQuery = @"select CardName from ocrd Where GroupCode = 117 and CardCode ='" + CArdName1 + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                val = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

                txtTransportName.Text = val.Rows[0][0].ToString();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private string GetDocNumber()
        {
            string retValue = "";
            try
            {
                int value = (from a in oDB.TrnsDispatchReturn select a.DocNum).Count();

                retValue = (value + 1).ToString();

            }
            catch (Exception Ex)
            {
                retValue = "-1";
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            return retValue;
        }

        private void btnGetWeight_Click_1(object sender, EventArgs e)
        {
            try
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Getweight " + txtCWeight.Text);
                string Cval = txtCWeight.Text;
                // txtFullText.Text = Cval;
                Program.oErrMgn.LogEntry(Program.ANV, "Gotweight " + Cval);
                lblWeight.Text = Cval;// txtFullText.Text;
                if (flgSetValues)
                {
                    txt2WeightKG.Text = lblWeight.Text;
                }
                else if (string.IsNullOrEmpty(txt2WeightKG.Text))
                {
                    txt1WeightKg.Text = lblWeight.Text;
                }

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ConnectPort()
        {
            if (comportDispatchR.IsOpen) comportDispatchR.Close();
            bool tmrc = tmrCamFront.Enabled;
            try
            {
                // Set the port's settings
                //  tmrCamFront.Enabled = false;
                comportDispatchR.BaudRate = Convert.ToInt32(BaudRate);
                comportDispatchR.DataBits = Convert.ToInt32(DataBits);
                comportDispatchR.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBit);
                comportDispatchR.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                comportDispatchR.PortName = ComPort;
                comportDispatchR.ReadTimeout = 5000;

                Program.WarningMsg("Trying to connect.");
                comportDispatchR.Open();
                Program.SuccesesMsg("Connected.");
                //  tmrCamFront.Enabled = tmrc;

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

        private void setValues(int doc)
        {
            flgSetValues = true;
            try
            {
                Program.NoMsg("");
                btnSubmit.Text = "&Add";

                TrnsDispatchReturn trm = (from a in oDB.TrnsDispatchReturn where a.DocNum == doc select a).FirstOrDefault();

                txtVoucherNo.Text = Convert.ToString(trm.DocNum);

                string[] date = trm.FDocDate.ToString().Split(' ');
                txtDate.Text = date[0];
                lbl2.Text = Convert.ToString(trm.FlgPosted);
                txtDONum.Text = trm.DONum;
                txtCusCode.Text = trm.CustomerCode;
                txtItemCode.Text = trm.ItemCode;
                txtDOQuantity.Text = Convert.ToString(trm.DOQuantity);
                txtItemGrpName.Text = trm.ItemName;
                txtVehicleRegNo.Text = trm.VehicleNum;
                txtDriverCnic.Text = trm.DriverCNIC;
                string[] Fdate = trm.FWeighmentDate.ToString().Split(' ');
                txt1WeightDate.Text = Fdate[0];
                txt1WeightKg.Text = Convert.ToString(trm.FWeighmentKG);
                //cmbTransportCode.SelectedValue = trm.TransportCode;
                //cmbTransportType.SelectedValue = Convert.ToString(trm.TransportID);

                string tCodeName = @"select  CardName +':'+ CardCode as CardCode from ocrd Where GroupCode = 117 and CardCode ='" + trm.TransportCode + "'";
                string tCode = mFm.ExecuteQueryScaler(tCodeName, Program.ConStrSAP);
                cmbTransportCode.Text = tCode;
                int ttype = Convert.ToInt32(trm.TransportID);
                string ttypeName = (from a in oDB.MstTransportType where a.ID == ttype select a.Name).FirstOrDefault();
                cmbTransportType.Text = ttypeName;

                txt2WeightKG.Text = Convert.ToString(trm.SWeighmentKG);
                txtNetWeightKG.Text = Convert.ToString(trm.NetWeightKG);
                string shiftname = (from a in oDB.MstShift where a.ID == Convert.ToInt32(trm.FShift) select a.Name).FirstOrDefault();
                txtShift.Text = shiftname;
                // txtShift.Text = Convert.ToString(trm.FShift);
                txtTime.Text = trm.FTime;
                txtCusName.Text = trm.CustomerName;

                string[] DOdate = trm.DODate.ToString().Split(' ');
                txtDODATE1.Text = DOdate[0];
                txtItemName.Text = trm.ItemName;
                txtBalQuan.Text = Convert.ToString(trm.BalanceQuantity);
                // txtDaySeries.Text = trm.
                //cmbPacker.Text = Convert.ToString(trm.PackerId);
                txtDriverName.Text = trm.DriverName;
                txtPathCnicDriver.Text = trm.DriverDocument;
                txt1WeightTime.Text = trm.FWeighmentTime;
                txt1WeightTons.Text = Convert.ToString(trm.FWeighmentTon);
                txtTransportName.Text = trm.TransportName;
                //txtCNICPath.Text =
                //txtBrandPath.Text = 
                if (!string.IsNullOrEmpty(Convert.ToString(trm.SWeighmentDate)))
                {
                    string[] Sdate = trm.SWeighmentDate.ToString().Split(' ');
                    txt2WeightDate.Text = Sdate[0];
                }
                else
                {
                    txt2WeightDate.Text = DateTime.Now.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(trm.SWeighmentTime))
                {
                    txt2WeightTime.Text = trm.SWeighmentTime;
                }
                else
                {
                    txt2WeightTime.Text = DateTime.Now.ToShortTimeString();
                }
                txt2WeightKG.Text = Convert.ToString(trm.SWeighmentKG);
                txtDaySerial.Text = Convert.ToString(trm.DaySeries);
                txtReturnQuan.Text = Convert.ToString(trm.ReturnQuantity);
                txt2WeightTons.Text = Convert.ToString(trm.SWeighmentTon);
                txtNetWeightTons.Text = Convert.ToString(trm.NetWeightTon);
                if (flgSetValues == true)
                {
                    lblWeight.Text = "";
                }



                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    txtVoucherNo.Enabled = true;
                    txtDONum.Enabled = true;
                    txtDODATE1.Enabled = true;
                    txtShift.Enabled = true;
                    txtCusName.Enabled = true;
                    txtCusCode.Enabled = true;
                    txtItemCode.Enabled = true;
                    txtItemGrpName.Enabled = true;
                    txtItemName.Enabled = true;
                    txtBalQuan.Enabled = true;
                    txtDOQuantity.Enabled = true;
                    txtDate.Enabled = true;
                    txtTime.Enabled = true;
                    txtDaySerial.Enabled = true;

                    txt1WeightDate.Enabled = true;
                    txt1WeightKg.Enabled = true;
                    txt1WeightTime.Enabled = true;
                    txt1WeightTons.Enabled = true;
                    txt2WeightDate.Enabled = true;
                    txt2WeightKG.Enabled = true;
                    txt2WeightTime.Enabled = true;
                    txt2WeightTons.Enabled = true;
                    txtNetWeightKG.Enabled = true;
                    txtNetWeightTons.Enabled = true;
                    txtDifferenceNWTons.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Set Values " + ex);
            }
        }

        private void FillTransporttype()
        {
            try
            {
                var oDoc = (from a in oDB.MstTransportType select a).ToList();

                cmbTransportType.DataSource = oDoc;
                cmbTransportType.DisplayMember = "Name";
                cmbTransportType.ValueMember = "ID";

                //foreach (var data in oDoc)
                //{
                //    cmbTransportType.Items.Add(data.Name);

                //}
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void FillTransportCode()
        {
            try
            {
                DataTable dt = new DataTable();

                string strQuery = @"select CardCode,CardName +':'+ CardCode as CardCodeName from ocrd Where GroupCode = 117";

                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dt = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

                cmbTransportCode.DataSource = dt;
                cmbTransportCode.DisplayMember = "CardCodeName";
                cmbTransportCode.ValueMember = "CardCode";
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    cmbTransportCode.Items.Add(dt.Rows[i][0].ToString());

                //}
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlBulkSealer()
        {
            try
            {
                Program.FormDocNum = Convert.ToInt32(txtVoucherNo.Text);
                Program.FormItemCode = txtItemCode.Text;
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oBulkSealer";
                Program.FormName = this.Name;
                //oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtDONum.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                //if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                //{
                //    txtBalQuan.Text = oDlg.SelectedObjectID;
                //    txtDODATE1.Text = oDlg.SelectedObjectIDComplex;
                //}
                oDlg.Dispose();
            }
            catch (Exception Ex)
            {
                //Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void frmDispatchReturn_Load(object sender, EventArgs e)
        {
            try
            {
                shiftName = this.GetShift();
                shiftid = this.shiftidParent;
                InitiallizeForm();
            }
            catch (Exception Ex)
            {
            }
        }

        private void CallSafeCWeight(string pValue)
        {
            if (txtCWeight.InvokeRequired)
            {
                delCWeight DelCall = new delCWeight(CallSafeCWeight);
                this.txtCWeight.Invoke(DelCall, new object[] { pValue });
            }
            else
            {
                this.txtCWeight.Text = pValue;
            }
        }

        private void HandleDialogSearch()
        {
            frmOpenDlg oDlg = new frmOpenDlg();
            oDlg.OpenFor = "trnsDispatchReturn";
            oDlg.SourceDocNum = txtDONum.Text;
            oDlg.ShowDialog();
            oDlg.Dispose();
        }

        private void DisplayData(string data)
        {
            lblWeight.Invoke(new EventHandler(delegate { lblWeight.Text = data; }));
        }

        private void LoadGrid()
        {
            try
            {
                dt.Clear();
                CreateDt1();
                IEnumerable<TrnsDispatchReturn> getData = from a in oDB.TrnsDispatchReturn where a.FlgSecondWeight == false select a;

                foreach (TrnsDispatchReturn item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["Wmnt#"] = item.DocNum;
                    dtrow["Vehicle#"] = item.VehicleNum;
                    dtrow["DONum"] = item.DONum;
                    dtrow["ItemName"] = item.ItemName;
                    dtrow["First Weight"] = item.FWeighmentTon;
                    string[] date = Convert.ToString(item.FDocDate).Split(' ');
                    dtrow["DocDate"] = date[0];
                    dt.Rows.Add(dtrow);
                }
                grdDetails.DataSource = dt;
                grdDetails.EnableFiltering = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void CreateDt1()
        {
            try
            {
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Wmnt#");
                    dt.Columns.Add("Vehicle#");
                    dt.Columns.Add("DONum");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("First Weight");
                    dt.Columns.Add("DocDate");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        public int daySeries()
        {
            int dSeries = 0;
            try
            {
                if (!string.IsNullOrEmpty(txtItemGrpName.Text))
                {
                    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    int fSeries = (from a in oDB.TrnsDispatchReturn
                                   where a.ItemGroupName == txtItemGrpName.Text && Convert.ToString(a.FDocDate) == todayDate
                                   select a).Count();
                    dSeries = fSeries + 1;
                }
                return dSeries;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ChkTolerence()
        {
            decimal exceed = 0;
            string itemGrp = txtItemGrpName.Text;
            decimal Tolerence = Convert.ToDecimal(oDB.MstTolerance.Where(x => x.TName == itemGrp).FirstOrDefault().TRate);
            if (Tolerence > 0)
            {
                val1 = (Convert.ToDecimal(txtDOQuantity.Text) * (Tolerence / 100)) + (Convert.ToDecimal(txtDOQuantity.Text));
                val2 = ((Convert.ToDecimal(txtDOQuantity.Text) - Convert.ToDecimal(txtDOQuantity.Text) * (Tolerence / 100)));

                if (val1 > Convert.ToDecimal(txtNetWeightTons.Text))
                {
                    if (Convert.ToDecimal(txtNetWeightTons.Text) > val2)
                    {

                        return true;

                    }
                    else
                    {
                        exceed = val2 - Convert.ToDecimal(txtNetWeightTons.Text);
                        Program.WarningMsg("tolerence decreases" + exceed + " Ton");
                        return false;
                    }
                }
                else
                {
                    exceed = Convert.ToDecimal(txtNetWeightTons.Text) - val1;
                    Program.WarningMsg("tolerence increases" + exceed + " Ton");
                    return false;
                }
            }
            else
            if (!itemGrp.ToUpper().Contains("BAG"))
            {
                if (Convert.ToDecimal(txtNetWeightTons.Text) < Convert.ToDecimal(txtBalQuan.Text))
                {
                    return true;
                }
                else
                {
                    Program.WarningMsg("Net Weight cannot b greater than Balance Quantity");
                    return false;
                }
            }
            else
            {
                return true;
            }




            //return false;

        }

        [STAThread]
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (IndicatorType.ToString() == "RM-OUT" || IndicatorType.ToString() == "DSP-IN" || IndicatorType.ToString() == "DSP-OUT" || IndicatorType.ToString() == "SILO-1")
            {
                Indicator01();
            }
            else if (IndicatorType.ToString() == "RM-IN")
            {
                Indicator02();
            }
        }

        private void Indicator01()
        {

            if (!alreadyReading)
            {
                try
                {
                    alreadyReading = true;
                    Thread.Sleep(500);
                    string data = comportDispatchR.ReadExisting();
                    //Program.oErrMgn.LogEntry(Program.ANV, " readed data :" + data);
                    //Program.oErrMgn.LogEntry(Program.ANV, " readed data ki lenght :" + data.Length.ToString());
                    if (data.Length < 9)
                    {
                        //Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value 9 se kam hai");
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
                        // txtFullText.Text = data;
                        // Program.oErrMgn.LogEntry(Program.ANV, "data k bad " + Convert.ToString(data));
                        // if (flgAscii) charindex = data.IndexOf((char)Convert.ToInt32(StartChar));
                        //Program.oErrMgn.LogEntry(Program.ANV, "ascii k baad");
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

        }

        private void Indicator02()
        {
            if (!alreadyReading)
            {
                alreadyReading = true;
                try
                {
                    //Program.oErrMgn.LogEntry(Program.ANV, "port fucntion start");
                    //Thread.Sleep(100);
                    string data = comportDispatchR.ReadLine();
                    //Program.oErrMgn.LogEntry(Program.ANV, " readed data :" + data);
                    //Program.oErrMgn.LogEntry(Program.ANV, " readed data ki lenght :" + data.Length.ToString());
                    //if (data.Length < 10)
                    //{
                    //    Program.oErrMgn.LogEntry(Program.ANV, "data lenght ki value 10 se kam hai");
                    //    alreadyReading = false;
                    //    return;
                    //}
                    txtFullText.Invoke(new EventHandler(delegate
                    {
                        long currentWeight = 0;
                        long.TryParse(data, out currentWeight);
                        CallSafeCWeight(currentWeight.ToString());
                        //Program.oErrMgn.LogEntry(Program.ANV, " Currentwt :" + currentWeight.ToString());
                    }));
                }
                catch (Exception ex)
                {
                    alreadyReading = false;
                    Program.oErrMgn.LogException(Program.ANV, ex);
                }
                alreadyReading = false;
            }

        }

        #endregion

    }
}
