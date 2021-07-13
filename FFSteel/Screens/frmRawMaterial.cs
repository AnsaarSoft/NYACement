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
using Telerik;
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
    public partial class frmRawMaterial : frmBaseForm
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
        DataTable dt = new DataTable();
        private SerialPort Rawcomport = new SerialPort();
        Boolean alreadyReading = false;
        int DocNum;
        string header = "";
        string ItemGroupCode = "";
        long shiftid = 0;
        string shiftName = "";
        bool flgSetValues = false;
        decimal val1 = 0;
        int ZeroHit = 0;
        long currentwt = 0;
        decimal RemainQ = 0;
        delegate void delCWeight(string value);
        delCWeight DelCall;
        string ItemCode;
        frmOpenDlg oDlg;//= new frmOpenDlg();

        #endregion

        #region Functions

        private void InitiallizeForm()
        {
            try
            {

                Program.NoMsg("");
                Program.flgIndicator = true;
                dt.Clear();
                oDB = new dbFFS(Program.ConStrApp);
                /*GetMachineSetting();*/
                CreateDt();
                FillYard();
                FillTransporttype();
                cmbTransportCode.Text = "";
                FillTransportCode();
                InitiallizeDocument();
                //AuthorizationScheme();
                tmrAlreadyReading.Interval = 100;
                LoadGrid();
                txt2WeightDate.Text = "";
                txt2WeightKG2.Text = "";
                txt2WeightTime.Text = "";
                txt2WeightTon.Text = "";
                txtNetWeightKG.Text = "";
                txtNetWeightTon.Text = "";
                txtDifferenceWeightTon.Text = "";

                txtFullText.Visible = false;
                /*tmrAlreadyReading.Interval = 1000;*/

                ApplyAuthorization();

                btnSubmit.Text = "&Add";
                DelCall = new delCWeight(CallSafeCWeight);
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
                Program.oErrMgn.LogEntry(Program.ANV, "CreateDt function start");
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Wmnt#");
                    dt.Columns.Add("Vehicle#");
                    dt.Columns.Add("PONum");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("First Weight");
                    dt.Columns.Add("DocDate");
                }
                Program.oErrMgn.LogEntry(Program.ANV, "CreateDt function End");
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "CreateDt function Exception: " + Ex);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void InitiallizeDocument()
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "InitiallizeDocument function start");
                Program.DocNum = string.Empty;
                txtDocNo.Text = GetDocNumber();
                txtShift.Text = shiftName;
                txtCurrDate.Text = DateTime.Now.ToShortDateString();
                txtCurrTime.Text = DateTime.Now.ToShortTimeString();
                txtPODate.Text = "";
                txtPONo.Text = "";
                txtVendorCode.Text = "";
                txtVendorName.Text = "";
                txtItemName.Text = "";
                txtItemCode.Text = "";
                txtDaySeries.Text = "";
                txtOrderQuantity.Text = "";
                txtRemainQuantity.Text = "";
                txtItemGroupName.Text = "";
                txtVehicleNo.Text = "";
                cmbYardType.Text = "";
                txtDriverCNIC.Text = "";
                txtDriverName.Text = "";
                txt1WeightKG.Text = "";
                txt1WeightTon.Text = "";
                txt1WeightDate.Text = DateTime.Now.ToShortDateString();
                txt1WeightTime.Text = DateTime.Now.ToShortTimeString();
                txtTransportName.Text = "";
                cmbTransportCode.Text = "";
                cmbTransportType.Text = "";
                txtBrandPath.Text = "";
                txtCNICPath.Text = "";
                //txt2WeightKG22.Text = "";
                txt2WeightKG2.Text = "";
                txt2WeightTon.Text = "";
                txt2WeightDate.Text = DateTime.Now.ToShortDateString();
                txt2WeightTime.Text = DateTime.Now.ToShortTimeString();
                txtNetWeightKG.Text = "";
                txtNetWeightTon.Text = "";
                txtPartyWeight.Text = "";
                flgSetValues = false;
                btnSubmit.Text = "&Add";
                //txtOrderQuantity.Enabled = true;
                txtTransportName.Enabled = false;
                tgsDocType.Enabled = true;
                //Program.oErrMgn.LogEntry(Program.ANV, "InitiallizeDocument function End");
                txtToleranceLimit.Text = "";
                txtToleranceLimit.Visible = false;
                chkAllowTolerance.Checked = false;
                txtIGPNo.Text = "";
            }
            catch (Exception Ex)
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "InitiallizeDocument function Exception: " + Ex);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private string GetDocNumber()
        {
            string retValue = "";
            try
            {
                int value = (from a in oDB.TrnsRawMaterial select a.DocNum).Count();

                retValue = (value + 1).ToString();

            }
            catch (Exception Ex)
            {
                retValue = "-1";
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            return retValue;
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

        private Boolean AddRecord()
        {
            Program.oErrMgn.LogEntry(Program.ANV, "AddRecord function start");
            Boolean retValue = true;
            try
            {
                //   string value = txtVoucherNo.Text;
                //  string purpose = cmbSourceDoc.SelectedItem.Text;
                TrnsRawMaterial oDoc = new TrnsRawMaterial();

                oDoc.FCreateDate = DateTime.Now;
                oDoc.FCreateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateDate = DateTime.Now;

                var getTrnsDispatch = oDB.TrnsRawMaterial.Where(x => x.DocNum == Convert.ToInt32(txtDocNo.Text));

                if (getTrnsDispatch.Count() > 0)
                {
                    var oOld = getTrnsDispatch.FirstOrDefault();
                    // oOld.DocNum = Convert.ToInt32(txtDocNo.Text);
                    //oOld.FDocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //oOld.FShift = Convert.ToInt32(shiftid);
                    //oOld.FTime = DateTime.Now.ToShortTimeString();
                    oOld.PONum = txtPONo.Text;
                    oOld.PODate = Convert.ToDateTime(txtPODate.Text);

                    //getTrnsDispatch.FirstOrDefault().Flg2Rpt
                    if (getTrnsDispatch.FirstOrDefault().Flg2Rpt == null)
                    {
                        oOld.Flg2Rpt = false;
                    }

                    oOld.FlgWBrpt = false;
                    oOld.VendorCode = txtVendorCode.Text;
                    oOld.VendorName = txtVendorName.Text;
                    oOld.ItemCode = txtItemCode.Text;
                    oOld.ItemName = txtItemName.Text;
                    oOld.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                    //oOld.RemainingQuantity = Convert.ToDecimal(txtRemainQuantity.Text);
                    //updated by: dawer
                    //request by: Ghulam mustafa
                    //date: 22/3/2019
                    oOld.RemainingQuantity = Convert.ToDecimal(txtRemainQuantity.Text);
                    if (!ChkTolerence())
                    {
                        return false;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbYardType.SelectedValue)))
                    {
                        oOld.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                    }
                    oOld.VehicleNum = txtVehicleNo.Text;
                    oOld.DriverCNIC = txtDriverCNIC.Text;
                    oOld.DriverName = txtDriverName.Text;
                    oOld.DriverDocument = txtCNICPath.Text;
                    oOld.PartyWeight = txtPartyWeight.Text;
                    //  oOld.FWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    // oOld.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
                    //     oOld.FWeighmentTime = DateTime.Now.ToShortTimeString();
                    // oOld.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportType.SelectedValue)))
                    {
                        oOld.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.SelectedValue)))
                    {
                        oOld.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                    }

                    oOld.TransportName = txtTransportName.Text;
                    //     oOld.FCreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //     oOld.FUpdateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //      oOld.FCreateBy = Program.oCurrentUser.UserCode;
                    //   oOld.FUpdateBy = Program.oCurrentUser.UserCode;
                    if (string.IsNullOrEmpty(oOld.SWeighmentKG.ToString()))
                    {
                        oOld.SDocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOld.SShift = Convert.ToInt32(shiftid);
                        oOld.STime = (DateTime.Now.ToShortTimeString());
                        oOld.SWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOld.SWeighmentTime = DateTime.Now.ToShortTimeString();
                    }

                    if (Convert.ToDecimal(txt1WeightKG.Text) > 0)

                    {
                        if (!string.IsNullOrEmpty(txt1WeightKG.Text))
                        {
                            oOld.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                            oOld.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
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

                    if (Convert.ToDecimal(txt2WeightKG2.Text) > 0)
                    {
                        oOld.SWeighmentKG = Convert.ToDecimal(txt2WeightKG2.Text);
                        oOld.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                    }
                    else
                    {
                        Program.ExceptionMsg("Second Weight can not be 0");
                        return false;
                    }
                    //updated by: dawer
                    //request by: Ghulam mustafa
                    //date: 4/2/2020
                    //updated by: dawer
                    //request by: Ghulam mustafa
                    //date: 17/6/2019
                    //if (Convert.ToDecimal(txtRemainQuantity.Text) < Convert.ToDecimal(txtNetWeightTon.Text))
                    //{
                    //    Program.ExceptionMsg("NetWeight is Greater than Remaining Quantity");
                    //    return false;
                    //}
                    //updated by: dawer
                    //request by: Ghulam mustafa
                    //date: 4/2/2020
                    if (Convert.ToDecimal(txt1WeightTon.Text) < Convert.ToDecimal(txt2WeightTon.Text))
                    {
                        Program.ExceptionMsg("First Weight should be Greater than Second Weight");
                        return false;
                    }
                    oOld.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                    oOld.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                    oOld.ReveivedQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                    if (string.IsNullOrEmpty(oOld.SCreateBy))
                    {
                        oOld.SCreateBy = Program.oCurrentUser.UserCode;
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(oOld.SCreateDate)))
                    {
                        oOld.SCreateDate = (DateTime.Now);
                    }

                    oOld.FUpdateBy = Program.oCurrentUser.UserCode;
                    oOld.FUpdateDate = (DateTime.Now);
                    //oOld.ItemGroupCode = ItemGroupCode;
                    //oOld.ItemGroupName = txtItemGroupName.Text;
                    if (!string.IsNullOrEmpty(txt2WeightKG2.Text))
                    {
                        oOld.FlgSecondWeight = true;
                    }
                    else
                    {
                        Program.SuccesesMsg("Second Weight can not be empty");
                        return false;
                    }
                }
                else
                {
                    oDoc.DocNum = Convert.ToInt32(txtDocNo.Text);
                    oDoc.FDocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    oDoc.Flg1Rpt = false;
                    if (!string.IsNullOrEmpty(txtShift.Text))
                    {
                        oDoc.FShift = Convert.ToInt32(shiftid);

                    }
                    else
                    {
                        Program.ExceptionMsg("Shift can not be Empty");
                        return false;
                    }
                    oDoc.FTime = DateTime.Now.ToShortTimeString();
                    oDoc.PONum = txtPONo.Text;
                    oDoc.PODate = Convert.ToDateTime(txtPODate.Text);
                    oDoc.VendorCode = txtVendorCode.Text;
                    oDoc.VendorName = txtVendorName.Text;
                    oDoc.ItemCode = txtItemCode.Text;
                    oDoc.ItemName = txtItemName.Text;
                    oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                    if (!string.IsNullOrEmpty(txtRemainQuantity.Text))
                    {
                        oDoc.RemainingQuantity = Convert.ToDecimal(txtRemainQuantity.Text);
                    }
                    else
                    {
                        oDoc.RemainingQuantity = 0;
                    }
                    oDoc.DaySeries = Convert.ToInt32(txtDaySeries.Text);
                    oDoc.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                    oDoc.VehicleNum = txtVehicleNo.Text;
                    oDoc.DriverCNIC = txtDriverCNIC.Text;
                    oDoc.DriverName = txtDriverName.Text;
                    oDoc.DriverDocument = txtCNICPath.Text;
                    oDoc.PartyWeight = txtPartyWeight.Text;
                    oDoc.FWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    // oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
                    oDoc.FWeighmentTime = DateTime.Now.ToShortTimeString();
                    // oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                    if (Convert.ToDecimal(txt1WeightKG.Text) > 0)
                    {
                        if (!string.IsNullOrEmpty(txt1WeightKG.Text))
                        {
                            oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                            oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
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
                    // oDoc.FCreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    // oDoc.FUpdateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    // oDoc.FCreateBy = Program.oCurrentUser.UserCode;
                    //  oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                    oDoc.FlgSecondWeight = false;


                    //oDoc.SDocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //oDoc.SShift = Convert.ToInt32(txtShift.Text);
                    //oDoc.STime = (DateTime.Now.ToShortTimeString());
                    //oDoc.SWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //oDoc.SWeighmentTime = DateTime.Now.ToShortTimeString();
                    //oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG2.Text);
                    //oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                    //oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                    //oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                    // oDoc.ReveivedQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                    //oDoc.SCreateBy = Program.oCurrentUser.UserCode;
                    //oDoc.SCreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                    oDoc.FUpdateDate = (DateTime.Now);
                    oDoc.ItemGroupCode = ItemGroupCode;
                    oDoc.ItemGroupName = txtItemGroupName.Text;

                    oDB.TrnsRawMaterial.InsertOnSubmit(oDoc);
                }
                oDB.SubmitChanges();

                Program.SuccesesMsg("Record Successfully Added.");
                Program.oErrMgn.LogEntry(Program.ANV, "AddRecord function End");

            }
            catch (Exception Ex)
            {
                retValue = false;
                Program.oErrMgn.LogEntry(Program.ANV, "Add function Exception");
                Program.oErrMgn.LogException(Program.ANV, Ex);
                Program.ExceptionMsg("Values are Missing or Incorrect.");

            }
            return retValue;
        }

        private bool AddRecordNew()
        {
            try
            {
                TrnsRawMaterial oDoc = new TrnsRawMaterial();
                //oDoc.DocNum = Convert.ToInt32(txtDocNo.Text);
                oDoc.DocNum = ReturnDocNum();
                oDoc.FDocDate = DateTime.Now;
                oDoc.FShift = Convert.ToInt32(shiftid);
                oDoc.FTime = DateTime.Now.ToShortTimeString();
                oDoc.FlgAPRI = !tgsDocType.Value;
                oDoc.PONum = txtPONo.Text;
                oDoc.PODate = Convert.ToDateTime(txtPODate.Text);
                oDoc.VendorCode = txtVendorCode.Text;
                oDoc.VendorName = txtVendorName.Text;
                oDoc.ItemCode = txtItemCode.Text;
                oDoc.ItemName = txtItemName.Text;
                oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                if (!string.IsNullOrEmpty(txtRemainQuantity.Text))
                {
                    oDoc.RemainingQuantity = Convert.ToDecimal(txtRemainQuantity.Text);
                }
                else
                {
                    oDoc.RemainingQuantity = 0;
                }
                oDoc.DaySeries = Convert.ToInt32(txtDaySeries.Text);
                oDoc.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                oDoc.VehicleNum = txtVehicleNo.Text;
                oDoc.DriverCNIC = txtDriverCNIC.Text;
                oDoc.DriverName = txtDriverName.Text;
                oDoc.DriverDocument = txtCNICPath.Text;
                oDoc.PartyWeight = txtPartyWeight.Text;
                oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                oDoc.TransportName = txtTransportName.Text;
                oDoc.ItemGroupCode = ItemGroupCode;
                oDoc.ItemGroupName = txtItemGroupName.Text;
                if (Convert.ToDecimal(txt1WeightKG.Text) > 0)
                {
                    oDoc.FWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    oDoc.FWeighmentTime = DateTime.Now.ToShortTimeString();
                    oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
                    oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                    oDoc.FlgSecondWeight = false;
                }
                oDoc.Flg1Rpt = false;
                oDoc.FCreateBy = Program.oCurrentUser.UserCode;
                oDoc.FCreateDate = DateTime.Now;
                oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateDate = DateTime.Now;
                oDoc.IGPNo = txtIGPNo.Text;
                oDB.TrnsRawMaterial.InsertOnSubmit(oDoc);
                oDB.SubmitChanges();
                Program.SuccesesMsg("Record Successfully Added.");
                return true;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                Program.ExceptionMsg("Something went wrong, Contact Abacus Support.");
                return false;
            }
        }

        private bool UpdateRecordNew()
        {
            try
            {
                TrnsRawMaterial oDoc = (from a in oDB.TrnsRawMaterial
                                        where a.DocNum == Convert.ToInt32(txtDocNo.Text)
                                        select a).FirstOrDefault();

                oDoc.SDocDate = DateTime.Now;
                oDoc.SShift = Convert.ToInt32(shiftid); ;
                oDoc.STime = DateTime.Now.ToShortTimeString();
                oDoc.PONum = txtPONo.Text;
                oDoc.PODate = Convert.ToDateTime(txtPODate.Text);
                oDoc.VendorCode = txtVendorCode.Text;
                oDoc.VendorName = txtVendorName.Text;
                oDoc.ItemCode = txtItemCode.Text;
                oDoc.ItemName = txtItemName.Text;
                oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                oDoc.RemainingQuantity = Convert.ToDecimal(txtRemainQuantity.Text);
                oDoc.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                oDoc.VehicleNum = txtVehicleNo.Text;
                oDoc.DriverCNIC = txtDriverCNIC.Text;
                oDoc.DriverName = txtDriverName.Text;
                oDoc.DriverDocument = txtCNICPath.Text;
                oDoc.PartyWeight = txtPartyWeight.Text;
                oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                oDoc.TransportName = txtTransportName.Text;

                if (Convert.ToDecimal(txt2WeightKG2.Text) > 0)
                {
                    oDoc.SWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    oDoc.SWeighmentTime = DateTime.Now.ToShortTimeString();
                    oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG2.Text);
                    oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                    oDoc.FlgSecondWeight = true;
                }
                oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                oDoc.ReveivedQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                oDoc.Flg2Rpt = false;
                oDoc.FlgWBrpt = false;
                oDoc.SCreateBy = Program.oCurrentUser.UserCode;
                oDoc.SCreateDate = DateTime.Now;
                oDoc.SUpdateBy = Program.oCurrentUser.UserCode;
                oDoc.SUpdateDate = DateTime.Now;
                oDoc.IGPNo = txtIGPNo.Text;
                if (chkAllowTolerance.Checked)
                {
                    oDoc.FlgTolerance = chkAllowTolerance.Checked;
                    oDoc.ToleranceLimit = Convert.ToDecimal(txtToleranceLimit.Text);
                }
                oDB.SubmitChanges();
                Program.SuccesesMsg("Record Successfully Added.");
                return true;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                Program.ExceptionMsg("Something went wrong, Contact Abacus Support.");
                return false;
            }
        }

        private void HandleDialogVendor()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();

                oDlg.OpenFor = "oPOVendor";
                oDlg.flgToggle = flgToggle;
                oDlg.Text = "Vendor Selector";
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtVendorCode.Text = oDlg.VendorCode;
                    txtVendorName.Text = oDlg.VendorName;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogPurchaseOrder()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oPO";
                oDlg.flgToggle = flgToggle;
                oDlg.Text = "Order Selector";
                oDlg.VendorCode = txtVendorCode.Text;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtPODate.Text = oDlg.DocDate;
                    txtPONo.Text = oDlg.DocNum;
                    Program.DocNum = txtPONo.Text;
                    txtBrandPath.Text = oDlg.Image;
                }
                oDlg.Dispose();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogAPReservedInvoice()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oAPRI";
                oDlg.flgToggle = flgToggle;
                oDlg.Text = "Order Selector";
                oDlg.VendorCode = txtVendorCode.Text;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtPODate.Text = oDlg.DocDate;
                    txtPONo.Text = oDlg.DocNum;
                    Program.DocNum = txtPONo.Text;
                    txtBrandPath.Text = oDlg.Image;
                }
                oDlg.Dispose();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlItems()
        {
            try
            {
                // frmOpenDlg oDlg = new frmOpenDlg();
                oDlg = new frmOpenDlg();
                if (tgsDocType.Value)
                {
                    oDlg.OpenFor = "oPOItems";
                }
                else
                {
                    oDlg.OpenFor = "oAPRIItems";
                }
                oDlg.Text = "Item Selector";


                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //nEW Addition By Ghulam
                    //Date 13 Jan 21
                    if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
                    {
                        decimal netweight = Convert.ToDecimal(txtNetWeightTon.Text);
                        if (netweight > Convert.ToDecimal(oDlg.Balance))
                        {
                            Program.WarningMsg("You can't select this item and PO has less balance.");
                            return;
                        }
                    }
                    //Code By : BILAL AHMED
                    //Chaneged By : Ghulam
                    //Date : 04-10-2019

                    if (string.IsNullOrEmpty(txt2WeightKG2.Text))
                    {
                        txtRemainQuantity.Text = oDlg.Balance;
                    }
                    //Code By : Syed Dawer Badr
                    //Chaneged By : Ghulam
                    //Date : 06-21-2019
                    bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                    if (super == true)
                    {
                        txtRemainQuantity.Text = oDlg.Balance;
                    }
                    // txtSBRDate.Text = oDlg.DocDate;
                    txtOrderQuantity.Text = Math.Round(Convert.ToDecimal(oDlg.Quantity), 3).ToString();
                    // txtSBRNum.Text = oDlg.DocNum;

                    txtItemCode.Text = oDlg.ItemCode;
                    txtItemName.Text = oDlg.Dscription;
                    txtItemGroupName.Text = oDlg.ItmsGrpNam;
                    ItemGroupCode = oDlg.ItmsGrpCod;
                    if (!string.IsNullOrEmpty(txtItemName.Text))
                    {
                        switch (txtItemName.Text)
                        {
                            case "Clinker SRC":
                                cmbYardType.SelectedIndex = 0;
                                break;
                            case "Clinker OPC":
                                cmbYardType.SelectedIndex = 1;
                                break;
                            case "Gypsum":
                                cmbYardType.SelectedIndex = 2;
                                break;
                            case "Limestone":
                                cmbYardType.SelectedIndex = 3;
                                break;
                            default:
                                break;
                        }
                    }

                    //Code By : BILAL AHMED
                    //Chaneged By : Ghulam
                    //Date : 04-10-2019
                    //txtDriverCNIC.Text = oDlg.DriverCnic;
                    //txtDriverName.Text = oDlg.U_Driver;
                    //txtVehicleNo.Text = oDlg.U_VehcleNo;

                    txtBrandPath.Text = oDlg.Image;

                }
                oDlg.Dispose();

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ForRemaningQuantity()
        {
            try
            {
                DataTable dt = new DataTable();
                Program.DocNum = txtPONo.Text;
                frmOpenDlg oDlg = new frmOpenDlg();

                oDlg.OpenFor = "oPOItems";
                oDlg.Text = "Item Selector";
                dt = oDlg.RemainingQuan();
                DataTable dt2 = dt.AsEnumerable().Where(x => x.Field<string>("ItemCode") == ItemCode).CopyToDataTable();
                string RemQuan = Convert.ToString(dt.Rows[0][6]);
                txtRemainQuantity.Text = RemQuan;

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void FillTransporttype()
        {
            try
            {
                var oDoc = (from a in oDB.MstTransportType orderby a.ID descending select a).ToList();

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

        private void FillYard()
        {
            try
            {
                var oDoc = (from a in oDB.MstYardType select a).ToList();

                cmbYardType.DataSource = oDoc;
                cmbYardType.DisplayMember = "Name";
                cmbYardType.ValueMember = "ID";

                //foreach (var data in oDoc)
                //{
                //    cmbYardType.Items.Add(data.Name);

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

                string strQuery = @"select CardCode,CardName +':'+ CardCode as CardCodeName from ocrd Where GroupCode = 108 and validFor = 'Y'";

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

        public frmRawMaterial()
        {
            try
            {
                //Rawcomport.Close();
                InitializeComponent();
                Program.oErrMgn.LogEntry(Program.ANV, "Raw material Screen Start");
                /*Rawcomport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                txtCWeight.Visible = true;
                txtCWeight.Enabled = false;*/
                // comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            }
            catch (Exception)
            {

                throw;
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

        private void setValues(int doc)
        {
            try
            {
                flgSetValues = true;
                Program.NoMsg("");
                btnSubmit.Text = "&Add";

                TrnsRawMaterial trm = (from a in oDB.TrnsRawMaterial where a.DocNum == doc select a).FirstOrDefault();

                txtDocNo.Text = Convert.ToString(trm.DocNum);
                if(trm.FlgAPRI.GetValueOrDefault())
                {
                    lblDocType.Text = "";
                }
                else
                {
                    lblDocType.Text = "";
                }

                txtPONo.Text = trm.PONum;
                lbl2.Text = Convert.ToString(trm.FlgPosted);
                txtVendorCode.Text = trm.VendorCode;
                txtItemCode.Text = trm.ItemCode;
                ItemCode = trm.ItemCode;
                txtOrderQuantity.Text = Convert.ToString(trm.OrderQuantity);
                txtItemGroupName.Text = trm.ItemGroupName;
                txtVehicleNo.Text = trm.VehicleNum;
                txtDriverCNIC.Text = trm.DriverCNIC;
                txtCNICPath.Text = trm.DriverDocument;
                txtPartyWeight.Text = trm.PartyWeight;

                txt1WeightKG.Text = Convert.ToString(trm.FWeighmentKG);
                //cmbTransportCode.SelectedValue = (trm.TransportCode);
                //cmbTransportCode.Text = tCode;
                if (!string.IsNullOrEmpty(trm.TransportCode))
                {
                    string tCodeName = @"select  CardName +':'+ CardCode as CardCode from ocrd Where GroupCode = 108 and CardCode ='" + trm.TransportCode + "'";
                    string tCode = mFm.ExecuteQueryScaler(tCodeName, Program.ConStrSAP);
                    //string tcode = trm.TransportCode.ToString();
                    //int value = (cmbTransportCode.Items.IndexOf(tcode));
                    cmbTransportCode.SelectedIndex = (cmbTransportCode.Items.IndexOf(tCode));
                }
                if (trm.YardId != null)
                {
                    int Yard = Convert.ToInt32(trm.YardId);
                    string YardName = (from a in oDB.MstYardType where a.ID == Yard select a.Name).FirstOrDefault();
                    //cmbYardType.Text = YardName;
                    cmbYardType.SelectedIndex = (cmbYardType.Items.IndexOf(YardName));
                }
                if (trm.TransportID != null)
                {
                    int ttype = Convert.ToInt32(trm.TransportID);
                    string ttypeName = (from a in oDB.MstTransportType where a.ID == ttype select a.Name).FirstOrDefault();
                    //cmbTransportType.Text = ttypeName;
                    cmbTransportType.SelectedIndex = (cmbTransportType.Items.IndexOf(ttypeName));
                }

                // cmbTransportCode.SelectedValue = trm.TransportCode;
                // cmbTransportType.SelectedValue = Convert.ToString(trm.TransportID);

                //txt2WeightKG22.Text = Convert.ToString(trm.SWeighmentKG);
                txtRemainQuantity.Text = Convert.ToString(trm.RemainingQuantity);
                txt2WeightKG2.Text = Convert.ToString(trm.SWeighmentKG);
                txtNetWeightKG.Text = Convert.ToString(trm.NetWeightKG);
                txtDaySeries.Text = Convert.ToString(trm.DaySeries);
                string shiftname = (from a in oDB.MstShift where a.ID == Convert.ToInt32(trm.FShift) select a.Name).FirstOrDefault();
                txtShift.Text = shiftname;
                txtCurrTime.Text = trm.FTime;
                txtVendorName.Text = trm.VendorName;

                txtItemName.Text = trm.ItemName;

                // txtDaySeries.Text = trm.

                txtDriverName.Text = trm.DriverName;
                txt1WeightTime.Text = trm.FWeighmentTime;
                txt1WeightTon.Text = Convert.ToString(trm.FWeighmentTon);
                txtTransportName.Text = trm.TransportName;
                //txtCNICPath.Text =
                //txtBrandPath.Text = 

                txt2WeightTon.Text = Convert.ToString(trm.SWeighmentTon);

                txtNetWeightTon.Text = Convert.ToString(trm.NetWeightTon);

                txtIGPNo.Text = trm.IGPNo;
                //* Update By: BILAL
                //Date : 3-27-2019
                if (string.IsNullOrEmpty(txtNetWeightTon.Text))
                {
                    txtNetWeightTon.Text = "0";
                }
                //* 
                //updated by: dawer
                //request by: Ghulam mustafa
                //date: 22/3/2019
                txtDifferenceWeightTon.Text = Convert.ToString(Convert.ToDecimal(txtRemainQuantity.Text) - Convert.ToDecimal(txtNetWeightTon.Text));
                if (flgSetValues == true)
                {
                    lblWeight.Text = "";
                }
                string[] Podate = trm.PODate.ToString().Split(' ');
                txtPODate.Text = Podate[0];
                string[] date = trm.FDocDate.ToString().Split(' ');
                txtCurrDate.Text = date[0];
                string[] fdate = trm.FWeighmentDate.ToString().Split(' ');
                txt1WeightDate.Text = fdate[0];
                if (!string.IsNullOrEmpty(Convert.ToString(trm.SWeighmentDate)))
                {
                    string[] sdate = trm.SWeighmentDate.ToString().Split(' ');
                    txt2WeightDate.Text = sdate[0];
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
                tgsDocType.Enabled = false;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Set Values " + ex);
            }
        }

        private void HandleDialogControlBulkSealer()
        {
            try
            {
                Program.FormDocNum = Convert.ToInt32(txtDocNo.Text);
                Program.FormItemCode = txtItemCode.Text;
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oBulkSealer";
                Program.FormName = this.Name;
                //oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtPONo.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                //if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                //{
                //    txtRemainQuantity.Text = oDlg.SelectedObjectID;
                //    txtPODate.Text = oDlg.SelectedObjectIDComplex;
                //}
                oDlg.Dispose();
            }
            catch (Exception Ex)
            {
                //Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogSearch()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "trnsRawMaterial";
                oDlg.SourceDocNum = txtPONo.Text;

                oDlg.ShowDialog();
                oDlg.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DisplayData(string data)
        {
            try
            {
                lblWeight.Invoke(new EventHandler(delegate { lblWeight.Text = data; }));
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void LoadGrid()
        {
            try
            {
                Program.oErrMgn.LogEntry(Program.ANV, "LoadGrid function start");
                dt.Clear();
                CreateDt();
                IEnumerable<TrnsRawMaterial> getData = from a in oDB.TrnsRawMaterial where a.FlgSecondWeight == false select a;

                foreach (TrnsRawMaterial item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["Wmnt#"] = item.DocNum;
                    dtrow["Vehicle#"] = item.VehicleNum;
                    dtrow["PONum"] = item.PONum;
                    dtrow["ItemName"] = item.ItemName;
                    dtrow["First Weight"] = item.FWeighmentTon;
                    string[] date = Convert.ToString(item.FDocDate).Split(' ');
                    dtrow["DocDate"] = date[0];
                    dt.Rows.Add(dtrow);
                }
                grdDetails.DataSource = dt;
                grdDetails.EnableFiltering = true;
                Program.oErrMgn.LogEntry(Program.ANV, "LoadGrid function End");
                dt.Dispose();
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "LoadGrid function Exception: " + ex);
                throw;
            }
        }

        public int daySeries()
        {
            int dSeries = 0;
            try
            {
                if (!string.IsNullOrEmpty(txtItemGroupName.Text))
                {
                    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    int fSeries = (from a in oDB.TrnsRawMaterial
                                   where a.ItemGroupName == txtItemGroupName.Text && Convert.ToString(a.FDocDate) == todayDate
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
            try
            {
                decimal exceed = 0, Tolerence;
                string itemGrp = txtItemGroupName.Text;
                //decimal value = oDB.MstTolerance.Where(x => x.TName == itemGrp).FirstOrDefault().TRate.GetValueOrDefault();
                if (chkAllowTolerance.Checked)
                {
                    Tolerence = Convert.ToDecimal(txtToleranceLimit.Text);
                }
                else
                {
                    decimal value = (from a in oDB.MstTolerance
                                     where a.TName == itemGrp
                                     select a.TRate).FirstOrDefault().GetValueOrDefault();
                    Tolerence = Convert.ToDecimal(value);
                }
                if (Tolerence > 0)
                {
                    // val1 = (Convert.ToDecimal(txtRemainQuantity.Text) * (Tolerence / 100)) + (Convert.ToDecimal(txtRemainQuantity.Text));
                    // val2 = ((Convert.ToDecimal(txtDOQuantity.Text) - Convert.ToDecimal(txtDOQuantity.Text) * (Tolerence / 100)));

                    //updated by: dawer
                    //request by: Ghulam mustafa
                    //date: 16/12/2019

                    val1 = Convert.ToDecimal(RemainQ) + Tolerence;
                    if (val1 >= Convert.ToDecimal(txtNetWeightTon.Text))
                    {
                        //if (Convert.ToDecimal(txtNetWeightTons.Text) > val2)
                        //{

                        return true;

                        //}
                        //else
                        //{
                        //    exceed = val2 - Convert.ToDecimal(txtNetWeightTons.Text);
                        //    Program.WarningMsg("tolerence decreases" + exceed + " Ton");
                        //    return false;
                        //}
                    }
                    else
                    {
                        exceed = Convert.ToDecimal(txtNetWeightTon.Text) - val1;
                        Program.WarningMsg("tolerence increases " + exceed + " Ton, Allow Quantity is " + val1);
                        return false;
                    }
                }
                //else
                //if (!itemGrp.ToUpper().Contains("BAG"))
                //{
                //    if (Convert.ToDecimal(txtNetWeightTon.Text) < Convert.ToDecimal(txtRemainQuantity.Text))
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        Program.WarningMsg("Net Weight cannot b greater than Balance Quantity");
                //        return false;
                //    }
                //}
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }




            //return false;

        }

        public void AddDocument()
        {
            try
            {
                if (Program.oCurrentUser.FlgSuper.GetValueOrDefault())
                {
                    if (UpdateRecordSpecial())
                    {
                        InitiallizeDocument();
                        LoadGrid();
                    }
                }
                else
                {
                    if (btnSubmit.Text == "&Add")
                    {
                        if (AddValidate())
                        {
                            if (AddRecordNew())
                            {
                                frmOpenDlg SealDialog = new frmOpenDlg();
                                if (SealDialog.AddSealToDb())
                                {
                                    var oDoc = (from a in oDB.TrnsRawMaterial
                                                where a.DocNum.ToString() == txtDocNo.Text
                                                select a).FirstOrDefault();
                                    if (oDoc != null)
                                    {
                                        if (Program.OpenLayout(Program.Screen, txtDocNo.Text, Program.Screen + " " + txtDocNo.Text))
                                        {
                                            oDoc.Flg1Rpt = true;
                                        }
                                    }
                                    oDB.SubmitChanges();
                                }
                                SealDialog.Dispose();
                                InitiallizeDocument();
                                LoadGrid();
                            }
                        }
                    }
                    else if (btnSubmit.Text == "&Update")
                    {
                        if (UpdateValidate())
                        {
                            if (UpdateRecordNew())
                            {
                                frmOpenDlg SealDialog = new frmOpenDlg();
                                if (SealDialog.AddSealToDb())
                                {
                                    var oDoc = (from a in oDB.TrnsRawMaterial
                                                where a.DocNum.ToString() == txtDocNo.Text
                                                select a).FirstOrDefault();
                                    if (oDoc != null)
                                    {
                                        if (Program.OpenLayout(Program.Screen, txtDocNo.Text, Program.Screen + " " + txtDocNo.Text))
                                        {
                                            oDoc.Flg2Rpt = true;
                                        }
                                    }
                                    oDB.SubmitChanges();
                                }
                                SealDialog.Dispose();
                                InitiallizeDocument();
                                LoadGrid();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        public bool AddValidate()
        {
            try
            {
                if (shiftid == 0)
                {
                    Program.WarningMsg("Shift can't be empty.");
                    return false;
                }
                if (!string.IsNullOrEmpty(txt1WeightKG.Text))
                {
                    if (Convert.ToDecimal(txt1WeightKG.Text) == 0)
                    {
                        Program.WarningMsg("First Weight can not be 0");
                        return false;
                    }
                }
                else
                {
                    Program.WarningMsg("First Weight can not be Empty");
                    return false;
                }
                if (string.IsNullOrEmpty(txtVehicleNo.Text))
                {
                    Program.WarningMsg("Vehicle number is mandatory.");
                    return false;
                }
                if (!string.IsNullOrEmpty(txtItemGroupName.Text) && txtItemGroupName.Text.ToLower() == "clinker")
                {
                    //if (string.IsNullOrEmpty(Convert.ToString(cmbYardType.Text)))
                    //{
                    //    Program.WarningMsg("Yard type must be selected.");
                    //    return false;
                    //}
                    if (string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.SelectedValue)))
                    {
                        Program.WarningMsg("Transport code is mandatory.");
                        return false;
                    }
                    if (string.IsNullOrEmpty(Convert.ToString(cmbTransportType.SelectedValue)))
                    {
                        Program.WarningMsg("Transportation type is mandatory.");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                return false;
            }
        }

        public bool UpdateValidate()
        {
            try
            {
                if (chkAllowTolerance.Checked)
                {
                    if (string.IsNullOrEmpty(txtToleranceLimit.Text))
                    {
                        Program.WarningMsg("Tolerance Limit can not be Empty");
                        return false;
                    }
                }
                if (!ChkTolerence())
                {
                    Program.WarningMsg("Tolerance didn't met.");
                    return false;
                }
                if (!string.IsNullOrEmpty(txtItemGroupName.Text) && txtItemGroupName.Text.ToLower() == "clinker")
                {
                    if (string.IsNullOrEmpty(Convert.ToString(cmbYardType.Text)))
                    {
                        Program.WarningMsg("Yard type must be selected.");
                        return false;
                    }
                    if (string.IsNullOrEmpty(cmbTransportType.Text))
                    {
                        Program.WarningMsg("Transportation type is mandatory.");
                        return false;
                    }
                }
                if (string.IsNullOrEmpty(txt2WeightKG2.Text))
                {
                    Program.WarningMsg("Second weight can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txt2WeightKG2.Text) == 0)
                    {
                        Program.WarningMsg("Second weight can't be zero.");
                        return false;
                    }
                }
                if (Convert.ToDecimal(txt1WeightTon.Text) < Convert.ToDecimal(txt2WeightTon.Text))
                {
                    Program.WarningMsg("First Weight should be Greater than Second Weight");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                return false;
            }
        }

        public bool UpdateRecordSpecial()
        {
            try
            {
                TrnsRawMaterial oDoc = (from a in oDB.TrnsRawMaterial
                                        where a.DocNum == Convert.ToInt32(txtDocNo.Text)
                                        select a).FirstOrDefault();
                if (oDoc.FlgPosted.GetValueOrDefault())
                {
                    Program.WarningMsg("You can't update posted documents.");
                    return false;
                }

                oDoc.PONum = txtPONo.Text;
                oDoc.PODate = Convert.ToDateTime(txtPODate.Text);
                oDoc.VendorCode = txtVendorCode.Text;
                oDoc.VendorName = txtVendorName.Text;
                oDoc.ItemCode = txtItemCode.Text;
                oDoc.ItemName = txtItemName.Text;
                oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                oDoc.RemainingQuantity = Convert.ToDecimal(txtRemainQuantity.Text);
                oDoc.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                oDoc.VehicleNum = txtVehicleNo.Text;
                oDoc.DriverCNIC = txtDriverCNIC.Text;
                oDoc.DriverName = txtDriverName.Text;
                oDoc.DriverDocument = txtCNICPath.Text;
                oDoc.PartyWeight = txtPartyWeight.Text;
                oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                oDoc.TransportName = txtTransportName.Text;
                if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    if (!string.IsNullOrEmpty(txt1WeightKG.Text))
                    {
                        if (Convert.ToDecimal(txt1WeightKG.Text) > 0)
                        {
                            oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
                            oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                            oDoc.FUpdateBy = Program.oCurrentUser.UserCode + " " + Program.ANV;
                            oDoc.FUpdateDate = DateTime.Now;
                        }
                    }
                    if (!string.IsNullOrEmpty(txt2WeightKG2.Text))
                    {
                        if (Convert.ToDecimal(txt2WeightKG2.Text) > 0)
                        {
                            oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG2.Text);
                            oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                            oDoc.SUpdateBy = Program.oCurrentUser.UserCode + " " + Program.ANV;
                            oDoc.SUpdateDate = DateTime.Now;
                        }
                    }
                    if (!string.IsNullOrEmpty(txtNetWeightKG.Text))
                    {
                        oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                    }
                    if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
                    {
                        decimal remainingqty = Convert.ToDecimal(txtRemainQuantity.Text);
                        decimal netweightqty = Convert.ToDecimal(txtNetWeightTon.Text);
                        if (netweightqty > remainingqty)
                        {
                            Program.WarningMsg("Net quantity can't exceed remaining quantity.");
                            return false;
                        }
                        oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                        oDoc.ReveivedQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                    }
                }
                if (chkAllowTolerance.Checked)
                {
                    if (string.IsNullOrEmpty(txtToleranceLimit.Text))
                    {
                        Program.WarningMsg("Tolerance limit can't be empty.");
                        return false;
                    }
                    else
                    {
                        oDoc.FlgTolerance = chkAllowTolerance.Checked;
                        oDoc.ToleranceLimit = Convert.ToDecimal(txtToleranceLimit.Text);
                    }
                }
                oDB.SubmitChanges();
                Program.SuccesesMsg("Record Successfully Updated.");
                return true;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                Program.ExceptionMsg("Something went wrong, Contact Abacus Support.");
                return false;
            }
        }

        private void ApplyAuthorization()
        {
            try
            {
                if (Program.oCurrentUser.UserCode.ToLower() == "manager")
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = true;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = true;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = true;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;

                    txtCWeight.Enabled = false;
                }
                else if (Program.oCurrentUser.FlgSuper.GetValueOrDefault() && !Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = false;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = false;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = false;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = true;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;

                    txtCWeight.Enabled = false;
                }
                else if (Program.oCurrentUser.FlgSuper.GetValueOrDefault() && Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = false;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = true;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = true;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = true;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;
                }
                else
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = false;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = false;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = false;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;

                    txtCWeight.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private int ReturnDocNum()
        {
            int DocNum = 1;
            try
            {
                using (dbFFS oprivate = new dbFFS(Program.ConStrApp))
                {
                    int value = (from a in oprivate.TrnsRawMaterial
                                 select a.DocNum).Max() ?? 0;
                    DocNum = value + 1;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                DocNum = 1;
            }
            return DocNum;
        }

        private void Indicator01()
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

        }

        private void Indicator02()
        {
            if (!alreadyReading)
            {
                alreadyReading = true;
                try
                {
                    //Thread.Sleep(100);
                    //Program.oErrMgn.LogEntry(Program.ANV, "indicator 02");
                    string data = Rawcomport.ReadLine();

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
                }
                alreadyReading = false;
            }

        }

        #endregion

        #region Events

        private void btnVendor_Click(object sender, EventArgs e)
        {
            try
            {
                HandleDialogVendor();
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tgsDocType.Value)
                {
                    HandleDialogPurchaseOrder();
                    tgsDocType.Enabled = false;
                }
                else
                {
                    HandleDialogAPReservedInvoice();
                    tgsDocType.Enabled = false;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnPOItem_Click(object sender, EventArgs e)
        {
            try
            {
                HandleDialogControlItems();
                txtDaySeries.Text = Convert.ToString(daySeries());
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void cmbTransportCode_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                string[] CardName = cmbTransportCode.Text.Split(':');
                if (CardName[0] != "")
                {
                    string CArdName1 = CardName[1];
                    DataTable val = new DataTable();
                    string strQuery = @"select CardName from ocrd Where GroupCode = 108 and validFor = 'Y' and CardCode ='" + CArdName1 + "'";
                    //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                    val = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

                    txtTransportName.Text = val.Rows[0][0].ToString();
                }
            }
            catch (Exception Ex)
            {

            }
        }

        private void btnBulkTable_Click(object sender, EventArgs e)
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

            try
            {
                InitiallizeDocument();
                flgSetValues = false;
                btnSubmit.Text = "&Add";
                btnPOItem.Enabled = true;
                btnDocNum.Enabled = true;
                btnSubmit.Enabled = true;
                txtDriverCNIC.Enabled = true;
                txtDriverName.Enabled = true;
                txtTransportName.Enabled = false;
                txtVehicleNo.Enabled = true;
                cmbYardType.Enabled = true;
                cmbTransportType.Enabled = true;
                cmbTransportCode.Enabled = true;
                if (Program.oCurrentUser.UserCode.ToString() == "manager")
                {
                    txt1WeightKG.Enabled = true;
                    txt2WeightKG2.Enabled = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {


                //DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
                //if (btnCancel.Text == "Close")
                //{
                //    oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
                //}
                //if (oResult == System.Windows.Forms.DialogResult.Cancel)
                //{
                //Rawcomport.DtrEnable = false;
                //Rawcomport.RtsEnable = false;
                //Rawcomport.DiscardInBuffer();
                //Rawcomport.DiscardOutBuffer();
                //20/3/2019
                if (Rawcomport.IsOpen)
                {
                    Rawcomport.DiscardInBuffer();
                    Rawcomport.DiscardOutBuffer();
                    Rawcomport.Close();
                    // Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 103");
                }
                Program.flgIndicator = false;
                this.Dispose();
                //Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 104");
                base.mytabpage.Dispose();
                //Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 105");
                //20/3/2019
                //}
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Port Ex 101");
                Program.oErrMgn.LogEntry(Program.ANV, ex.Message.ToString());
            }
        }

        private void txt1WeightKG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (flgSetValues == true)
                {
                    lblWeight.Text = "";
                }
                else
                {
                    lblWeight.Text = txt1WeightKG.Text;
                }

                if (!string.IsNullOrEmpty(txt1WeightKG.Text))
                {
                    txt1WeightTon.Text = Convert.ToString(Convert.ToDecimal(txt1WeightKG.Text) / 1000);
                }
                else
                {
                    txt1WeightTon.Text = null;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "txt1WeightKG_TextChanged function start" + ex);
                throw;
            }
        }

        private void txt2WeightKG2_TextChanged(object sender, EventArgs e)
        {

            //Program.NoMsg("");
            //int n;
            //bool isNumeric = int.TryParse(Convert.ToString(txt2WeightKG2.Text), out n);

            //if (flgSetValues == false)

            //{
            //    lblWeight.Text = "";
            //}
            //else
            //{
            //    lblWeight.Text = txt2WeightKG2.Text;
            //}

            //if (!string.IsNullOrWhiteSpace(txt2WeightKG2.Text))
            //{
            //    if (!string.IsNullOrEmpty(txt2WeightKG2.Text))
            //    {
            //        txt2WeightTon.Text = Convert.ToString(Convert.ToDecimal(txt2WeightKG2.Text) / 1000);
            //        txtNetWeightKG.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightKG.Text) - Convert.ToDecimal(txt2WeightKG2.Text)));
            //        txtNetWeightTon.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightTon.Text) - Convert.ToDecimal(txt2WeightTon.Text)));

            //        txtDifferenceWeightTon.Text = Convert.ToString(Convert.ToDecimal(txtRemainQuantity.Text) - Convert.ToDecimal(txtNetWeightTon.Text));
            //        txtRemainQuantity.Text = Convert.ToString(Convert.ToDecimal(txtRemainQuantity.Text) - Convert.ToDecimal(txtNetWeightTon.Text));
            //    }
            //}
            //else
            //{
            //    txt2WeightTon.Text = null;
            //    txtNetWeightKG.Text = null;
            //    txtNetWeightTon.Text = null;
            //    txtDifferenceWeightTon.Text = null;
            //}
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Program.DocNum))
                {
                    HandleDialogSearch();
                    if (Program.DocNum != null)
                    {
                        setValues(Convert.ToInt32(Program.DocNum));
                        txtDriverCNIC.Enabled = false;
                        txtDriverName.Enabled = false;
                        txtTransportName.Enabled = false;
                        txtVehicleNo.Enabled = false;
                        cmbYardType.Enabled = false;
                        cmbTransportType.Enabled = false;
                        cmbTransportCode.Enabled = false;
                        flgSetValues = false;
                        btnSubmit.Text = "&Update";
                    }
                }
                else
                {
                    Program.WarningMsg("Document already selected you can't search.");
                }
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                //if (super == true)
                //{
                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = true;

                //    }
                //}
                //else
                //{
                //    btnPOItem.Enabled = false;
                //    btnDocNum.Enabled = false;
                //    btnSubmit.Enabled = false;
                //}
                //txt1WeightDate.Enabled = false;
                //txt1WeightKG.Enabled = false;
                //txt1WeightTime.Enabled = false;
                //txt1WeightTon.Enabled = false;
                //txt2WeightDate.Enabled = false;
                //txt2WeightKG2.Enabled = false;
                //txt2WeightTime.Enabled = false;
                //txt2WeightTon.Enabled = false;
                //txtNetWeightKG.Enabled = false;
                //txtNetWeightTon.Enabled = false;
                //txtDifferenceWeightTon.Enabled = false;
                //txtCWeight.Enabled = false;
                //lblWeight.Text = "";

                //txtDocNo.Enabled = false;
                //txtShift.Enabled = false;
                //txtCurrDate.Enabled = false;
                //txtCurrTime.Enabled = false;
                //txtPONo.Enabled = false;
                //txtVendorCode.Enabled = false;
                //txtVendorName.Enabled = false;
                //txtItemCode.Enabled = false;
                //txtItemName.Enabled = false;
                //txtOrderQuantity.Enabled = false;
                //txtRemainQuantity.Enabled = false;
                //txtItemGroupName.Enabled = false;
                //txtDaySeries.Enabled = false;
                //txtCNICPath.Enabled = false;
                //txtPODate.Enabled = false;

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
                //var MaxRecord = (from a in oDB.TrnsRawMaterial where a.FlgSecondWeight == true && a.FlgPosted == null select a.DocNum).Max();
                var MaxRecord = (from a in oDB.TrnsRawMaterial select a.DocNum).Max();
                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    flgSetValues = false;
                    btnSubmit.Text = "&Update";
                    ApplyAuthorization();
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //txtTransportName.Enabled = false;
                    //txtVehicleNo.Enabled = false;
                    //cmbYardType.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;

                    //txtCWeight.Enabled = false;
                    lblWeight.Text = "";
                    Program.WarningMsg("Reached To Last Record");
                }
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                //if (super == true)
                //{
                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = true;

                //    }
                //}
                //else
                //{
                //    btnPOItem.Enabled = false;
                //    btnDocNum.Enabled = false;
                //    btnSubmit.Enabled = false;
                //}
                //txt1WeightDate.Enabled = false;
                //txt1WeightKG.Enabled = false;
                //txt1WeightTime.Enabled = false;
                //txt1WeightTon.Enabled = false;
                //txt2WeightDate.Enabled = false;
                //txt2WeightKG2.Enabled = false;
                //txt2WeightTime.Enabled = false;
                //txt2WeightTon.Enabled = false;
                //txtNetWeightKG.Enabled = false;
                //txtNetWeightTon.Enabled = false;
                //txtDifferenceWeightTon.Enabled = false;

                //txtDocNo.Enabled = false;
                //txtShift.Enabled = false;
                //txtCurrDate.Enabled = false;
                //txtCurrTime.Enabled = false;
                //txtPONo.Enabled = false;
                //txtVendorCode.Enabled = false;
                //txtVendorName.Enabled = false;
                //txtItemCode.Enabled = false;
                //txtItemName.Enabled = false;
                //txtOrderQuantity.Enabled = false;
                //txtRemainQuantity.Enabled = false;
                //txtItemGroupName.Enabled = false;
                //txtDaySeries.Enabled = false;
                //txtCNICPath.Enabled = false;
                //txtPODate.Enabled = false;
            }
            catch (Exception ex)
            {
                //btnPOItem.Enabled = false;
                //btnDocNum.Enabled = false;
                //btnSubmit.Enabled = false;
                //Program.WarningMsg("Reached To Last Record, Something went wrong.");
            }
        }

        private void btnNextRecord_Click_1(object sender, EventArgs e)
        {
            try
            {

                int DocNum = Convert.ToInt32(txtDocNo.Text);
                int previusDocNum = DocNum + 1;
                int checkDoc = 0;
                //checkDoc = (from a in oDB.TrnsRawMaterial where a.DocNum == previusDocNum && a.FlgSecondWeight == true && a.FlgPosted == null select a).Count();
                checkDoc = (from a in oDB.TrnsRawMaterial where a.DocNum == previusDocNum select a).Count();
                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    btnSubmit.Text = "&Update";
                    ApplyAuthorization();
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //txtTransportName.Enabled = false;
                    //txtVehicleNo.Enabled = false;
                    //cmbYardType.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                    //flgSetValues = false;

                    //txtCWeight.Enabled = false;
                    lblWeight.Text = "";
                }
                else
                {
                    Program.WarningMsg("Reached To Last Record");
                }

                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                //if (super == true)
                //{
                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = true;

                //    }
                //}
                //else
                //{
                //    btnPOItem.Enabled = false;
                //    btnDocNum.Enabled = false;
                //    btnSubmit.Enabled = false;
                //}
                //txt1WeightDate.Enabled = false;
                //txt1WeightKG.Enabled = false;
                //txt1WeightTime.Enabled = false;
                //txt1WeightTon.Enabled = false;
                //txt2WeightDate.Enabled = false;
                //txt2WeightKG2.Enabled = false;
                //txt2WeightTime.Enabled = false;
                //txt2WeightTon.Enabled = false;
                //txtNetWeightKG.Enabled = false;
                //txtNetWeightTon.Enabled = false;
                //txtDifferenceWeightTon.Enabled = false;

                //txtDocNo.Enabled = false;
                //txtShift.Enabled = false;
                //txtCurrDate.Enabled = false;
                //txtCurrTime.Enabled = false;
                //txtPONo.Enabled = false;
                //txtVendorCode.Enabled = false;
                //txtVendorName.Enabled = false;
                //txtItemCode.Enabled = false;
                //txtItemName.Enabled = false;
                //txtOrderQuantity.Enabled = false;
                //txtRemainQuantity.Enabled = false;
                //txtItemGroupName.Enabled = false;
                //txtDaySeries.Enabled = false;
                //txtCNICPath.Enabled = false;
                //txtPODate.Enabled = false;
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
                int DocNum = Convert.ToInt32(txtDocNo.Text);
                int previusDocNum = DocNum - 1;
                int checkDoc = 0;
                //checkDoc = (from a in oDB.TrnsRawMaterial where a.DocNum == previusDocNum && a.FlgSecondWeight == true && a.FlgPosted == null select a).Count();
                checkDoc = (from a in oDB.TrnsRawMaterial where a.DocNum == previusDocNum select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    btnSubmit.Text = "&Update";
                    
                    flgSetValues = false;
                    ApplyAuthorization();
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //txtTransportName.Enabled = false;
                    //txtVehicleNo.Enabled = false;
                    //cmbYardType.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                }
                else
                {
                    Program.WarningMsg("Reached To First Record");
                }
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                //if (super == true)
                //{
                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = true;

                //    }
                //}
                //else
                //{
                //    btnPOItem.Enabled = false;
                //    btnDocNum.Enabled = false;
                //    btnSubmit.Enabled = false;
                //}
                //txt1WeightDate.Enabled = false;
                //txt1WeightKG.Enabled = false;
                //txt1WeightTime.Enabled = false;
                //txt1WeightTon.Enabled = false;
                //txt2WeightDate.Enabled = false;
                //txt2WeightKG2.Enabled = false;
                //txt2WeightTime.Enabled = false;
                //txt2WeightTon.Enabled = false;
                //txtNetWeightKG.Enabled = false;
                //txtNetWeightTon.Enabled = false;
                //txtDifferenceWeightTon.Enabled = false;
                //txtCWeight.Enabled = false;

                //txtDocNo.Enabled = false;
                //txtShift.Enabled = false;
                //txtCurrDate.Enabled = false;
                //txtCurrTime.Enabled = false;
                //txtPONo.Enabled = false;
                //txtVendorCode.Enabled = false;
                //txtVendorName.Enabled = false;
                //txtItemCode.Enabled = false;
                //txtItemName.Enabled = false;
                //txtOrderQuantity.Enabled = false;
                //txtRemainQuantity.Enabled = false;
                //txtItemGroupName.Enabled = false;
                //txtDaySeries.Enabled = false;
                //txtCNICPath.Enabled = false;
                //txtPODate.Enabled = false;

                lblWeight.Text = "";
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
                //var MaxRecord = (from a in oDB.TrnsRawMaterial where a.FlgSecondWeight == true && a.FlgPosted == null select a.DocNum).First();
                var MaxRecord = (from a in oDB.TrnsRawMaterial select a.DocNum).First();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    btnSubmit.Text = "&Update";
                    flgSetValues = false;
                    ApplyAuthorization();
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //txtTransportName.Enabled = false;
                    //txtVehicleNo.Enabled = false;
                    //cmbYardType.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                    Program.WarningMsg("Reached To First Record");
                }
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                //if (super == true)
                //{
                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = true;

                //    }
                //}
                //else
                //{
                //    btnPOItem.Enabled = false;
                //    btnDocNum.Enabled = false;
                //    btnSubmit.Enabled = false;
                //}
                //txt1WeightDate.Enabled = false;
                //txt1WeightKG.Enabled = false;
                //txt1WeightTime.Enabled = false;
                //txt1WeightTon.Enabled = false;
                //txt2WeightDate.Enabled = false;
                //txt2WeightKG2.Enabled = false;
                //txt2WeightTime.Enabled = false;
                //txt2WeightTon.Enabled = false;
                //txtNetWeightKG.Enabled = false;
                //txtNetWeightTon.Enabled = false;
                //txtDifferenceWeightTon.Enabled = false;
                //txtCWeight.Enabled = false;

                //txtDocNo.Enabled = false;
                //txtShift.Enabled = false;
                //txtCurrDate.Enabled = false;
                //txtCurrTime.Enabled = false;
                //txtPONo.Enabled = false;
                //txtVendorCode.Enabled = false;
                //txtVendorName.Enabled = false;
                //txtItemCode.Enabled = false;
                //txtItemName.Enabled = false;
                //txtOrderQuantity.Enabled = false;
                //txtRemainQuantity.Enabled = false;
                //txtItemGroupName.Enabled = false;
                //txtDaySeries.Enabled = false;
                //txtCNICPath.Enabled = false;
                //txtPODate.Enabled = false;


                lblWeight.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        private void txtDocNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Program.sealdt != null)
                {
                    Program.sealdt.Clear();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void txtFullText_TextChanged(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txt1WeightKG.Text) || txt1WeightKG.Text == "0")
            //{
            //    txt1WeightKG.Text = txtFullText.Text;
            //}
            //else if (flgSetValues)
            //{
            //    txt2WeightKG2.Text = txtFullText.Text;
            //}
            //if (flgSetValues)
            //{
            //    txt2WeightKG2.Text = txtFullText.Text;
            //}
            //else if (string.IsNullOrEmpty(txt2WeightKG2.Text))
            //{
            //    txt1WeightKG.Text = txtFullText.Text;
            //}
            //lblWeight.Text = txtFullText.Text;
        }

        private void tmrAlreadyReading_Tick(object sender, EventArgs e)
        {
            try
            {
                alreadyReading = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open for Driver ID Picture";

                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCNICPath.Text = theDialog.FileName.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            AddDocument();
        }

        private void grdDetails_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                DocNum = 0;
                DocNum = Convert.ToInt32(e.Row.Cells["Wmnt#"].Value);
                setValues(DocNum);
                ForRemaningQuantity();
                btnSubmit.Text = "&Update";
                tgsDocType.Enabled = false;
                if (Program.oCurrentUser.UserCode.ToLower() == "manager" || Program.oCurrentUser.FlgSuper.GetValueOrDefault())
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = false;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = false;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = true;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = true;
                    txtCNICPath.Enabled = true;
                    txtBrandPath.Enabled = true;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;

                    txtCWeight.Enabled = false;
                }
                else if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = false;
                    txtDriverCNIC.Enabled = false;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = true;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = true;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;

                    txtCWeight.Enabled = false;
                }
                else
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtPONo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    txtItemCode.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNo.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = false;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG2.Enabled = false;
                    txtNetWeightKG.Enabled = false;
                    txtPartyWeight.Enabled = true;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtPODate.Enabled = false;
                    txtItemName.Enabled = false;
                    txtRemainQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeightTon.Enabled = false;

                    txtCWeight.Enabled = false;
                }
                // frmOpenDlg od = new frmOpenDlg();
                //od.
                //txtRemainQuantity.Text = 
                //txtDriverCNIC.Enabled = true;
                //txtDriverName.Enabled = true;
                //txtTransportName.Enabled = false;
                //txtVehicleNo.Enabled = true;
                //cmbYardType.Enabled = true;
                //cmbTransportType.Enabled = true;
                //cmbTransportCode.Enabled = true;
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                //if (super == true)
                //{
                //    foreach (Control o in this.Controls)
                //    {
                //        // special handling for the menu
                //        o.Enabled = true;

                //    }
                //}
                //else
                //{
                //    //btnPOItem.Enabled = true;
                //    //btnDocNum.Enabled = true;
                //    btnSubmit.Enabled = true;
                //}
                //txt1WeightDate.Enabled = false;
                //txt1WeightKG.Enabled = false;
                //txt1WeightTime.Enabled = false;
                //txt1WeightTon.Enabled = false;
                //txt2WeightDate.Enabled = false;
                //txt2WeightKG2.Enabled = false;
                //txt2WeightTime.Enabled = false;
                //txt2WeightTon.Enabled = false;
                //txtNetWeightKG.Enabled = false;
                //txtNetWeightTon.Enabled = false;
                //txtDifferenceWeightTon.Enabled = false;

                //txtDocNo.Enabled = false;
                //txtShift.Enabled = false;
                //txtCurrDate.Enabled = false;
                //txtCurrTime.Enabled = false;
                //txtPONo.Enabled = false;
                //txtVendorCode.Enabled = false;
                //txtVendorName.Enabled = false;
                //txtItemCode.Enabled = false;
                //txtItemName.Enabled = false;
                //txtOrderQuantity.Enabled = false;
                //txtRemainQuantity.Enabled = false;
                //txtItemGroupName.Enabled = false;
                //txtDaySeries.Enabled = false;
                //txtCNICPath.Enabled = false;
                //txtPODate.Enabled = false;


                //txtCWeight.Enabled = false;



            }
            catch (Exception)
            {

                throw;
            }


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string critaria = txtDocNo.Text;
                string PldFor = Program.Screen;
                string Fwmnt = txt1WeightKG.Text;
                if (!string.IsNullOrEmpty(Fwmnt))
                {
                    if (lbl2.Text == "True")
                    {
                        Program.OpenLayout(PldFor.Trim(), critaria, PldFor.Trim() + " " + txtDocNo.Text);
                        //Program.OpenLayout("WayBridgeDelivery", critaria, "WayBridgeDelivery " + txtDocNo.Text);
                    }
                    else
                    {
                        Program.OpenLayout(PldFor.Trim(), critaria, PldFor.Trim() + " " + txtDocNo.Text);
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

        private void txt2WeightKG2_Click(object sender, EventArgs e)
        {
            //btnSubmit.Text = "&Update";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGrid();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void chkAllowTolerance_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            try
            {
                if (chkAllowTolerance.Checked)
                {
                    lblToleranceLimit.Visible = true;
                    txtToleranceLimit.Visible = true;
                }
                else
                {
                    lblToleranceLimit.Visible = false;
                    txtToleranceLimit.Visible = false;
                    txtToleranceLimit.Text = string.Empty;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        
        private void btnGetWeight_Click(object sender, EventArgs e)
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "Getweight " + txtCWeight.Text);
                //string Cval = txtCWeight.Text;
                //// txtFullText.Text = Cval;
                string Cval = "";
                if (rbBridge01.IsChecked)
                {
                    Program.oErrMgn.LogEntry(Program.ANV, "Getweight1 " + Program.Bridge01Value);
                    Cval = Program.Bridge01Value;
                }
                else
                {
                    Program.oErrMgn.LogEntry(Program.ANV, "Getweight2 " + Program.Bridge02Value);
                    Cval = Program.Bridge02Value;
                }
                Program.oErrMgn.LogEntry(Program.ANV, "Gotweight " + Cval);
                lblWeight.Text = Cval;// txtFullText.Text;
                if (flgSetValues)
                {
                    txt2WeightKG2.Text = lblWeight.Text;
                }
                else if (string.IsNullOrEmpty(txt2WeightKG2.Text))
                {
                    txt1WeightKG.Text = lblWeight.Text;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void txt2WeightKG2_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                Program.oErrMgn.LogEntry(Program.ANV, "txt2WeightKG2_TextChanged_1 function start");
                if (flgSetValues == false)

                {
                    lblWeight.Text = "";
                }
                else
                {
                    lblWeight.Text = txt2WeightKG2.Text;
                }

                if (!string.IsNullOrWhiteSpace(txt2WeightKG2.Text))
                {
                    if (!string.IsNullOrEmpty(txt2WeightKG2.Text))
                    {
                        txt2WeightTon.Text = Convert.ToString(Convert.ToDecimal(txt2WeightKG2.Text) / 1000);
                        txtNetWeightKG.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightKG.Text) - Convert.ToDecimal(txt2WeightKG2.Text)));
                        txtNetWeightTon.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightTon.Text) - Convert.ToDecimal(txt2WeightTon.Text)));

                        RemainQ = Convert.ToDecimal(txtRemainQuantity.Text);
                        txtDifferenceWeightTon.Text = Convert.ToString(Convert.ToDecimal(txtRemainQuantity.Text) - Convert.ToDecimal(txtNetWeightTon.Text));

                        // resource: BIlal Ahmed
                        // Date: 3/28/2019
                        // Moiefied by: Ghulam





                        //txtRemainQuantity.Text = Convert.ToString(Convert.ToDecimal(txtRemainQuantity.Text) - Convert.ToDecimal(txtNetWeightTon.Text));
                    }
                }
                else
                {
                    txt2WeightTon.Text = null;
                    txtNetWeightKG.Text = null;
                    txtNetWeightTon.Text = null;
                    txtDifferenceWeightTon.Text = null;
                }
                Program.oErrMgn.LogEntry(Program.ANV, "txt2WeightKG2_TextChanged_1 function End");
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private void tgsDocType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (tgsDocType.Value == true)
                {
                    lblDocType.Text = "PO #";
                }
                else
                {
                    lblDocType.Text = "AP RI #";
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private void frmRawMaterial_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'alMabrookaDataSet.TrnsRawMaterial' table. You can move, or remove it, as needed.
            //this.trnsRawMaterialTableAdapter.Fill(this.alMabrookaDataSet.TrnsRawMaterial);
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

        #endregion
    }
}
