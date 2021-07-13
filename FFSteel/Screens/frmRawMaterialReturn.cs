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
    public partial class frmRawMaterialReturn : frmBaseForm
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
        private SerialPort comportRawR = new SerialPort();
        Boolean alreadyReading = false;
        DataTable dt = new DataTable();
        long shiftid = 0;
        string shiftName = "";
        string ItemGroupCode = "";
        string returnQuantity = "";
        bool flgSetValues = false;
        decimal val1;
        int ZeroHit = 0;
        long currentwt = 0;
        delegate void delCWeight(string value);
        string CardName = "";
        #endregion

        #region Functions
        public frmRawMaterialReturn()
        {
            InitializeComponent();
            //txtCWeight.Visible = false;
           /* txtCWeight.Enabled = false;
            comportRawR.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);*/
        }
        
        private void InitiallizeForm()
        {
            try
            {
                Program.NoMsg("");
                Program.flgIndicator = true;
                dt.Clear();
                oDB = new dbFFS(Program.ConStrApp);
                /*GetMachineSetting();*/
                FillTransportCode();
                FillTransporttype();
                FillYard();
                CreateDt();
                InitiallizeDocument();
                //AuthorizationScheme();
                LoadGrid();
                txt2WeighDate.Text = "";
                txt2WeighKG.Text = "";
                txt2WeighTime.Text = "";
                txt2WeighTon.Text = "";
                txtNetWeighKG.Text = "";
                txtNetWeighTon.Text = "";
                txtDiffGRPONtWt.Text = "";

                txtVoucherNo.Enabled = false;
                txtGRPONum.Enabled = false;
                txtGRPODate.Enabled = false;
                txtShift.Enabled = false;
                txtVendorName.Enabled = false;
                txtVendorCode.Enabled = false;
                txtItemCode.Enabled = false;
                txtItemGrpName.Enabled = false;
                txtItemName.Enabled = false;
                txtGRPOQuanTon.Enabled = false;
                txtRemainingQuant.Enabled = false;
                txtDate.Enabled = false;
                txtTime.Enabled = false;
                txtDaySerial.Enabled = false;

                txt1WeighDate.Enabled = false;
                txt1WeighKG.Enabled = false;
                txt1WeighTime.Enabled = false;
                txt1WeighTon.Enabled = false;
                txt2WeighDate.Enabled = false;
                txt2WeighKG.Enabled = false;
                txt2WeighTime.Enabled = false;
                txt2WeighTon.Enabled = false;
                txtNetWeighKG.Enabled = false;
                txtNetWeighTon.Enabled = false;
                txtDiffGRPONtWt.Enabled = false;
                txtFullText.Visible = false;
                /*tmrAlreadyReading.Interval = 1000;*/
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "RawMaterialReturn").FirstOrDefault().GivenRight);
                if (Auth == 4)
                {
                    btnSubmit.Enabled = false;
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
                txtCWeight.Enabled = false;
                txtPathCnic.Enabled = false;
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
                    dt.Columns.Add("Wmnt#");
                    dt.Columns.Add("Vehicle#");
                    dt.Columns.Add("GRPO#");
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

        private void InitiallizeDocument()
        {
            try
            {
                txtVoucherNo.Text = GetVoucherNumber();
                txtShift.Text = shiftName;
                txtDate.Text = DateTime.Now.ToShortDateString();
                txtTime.Text = DateTime.Now.ToShortTimeString();
                txtVendorCode.Text = "";
                txtVendorName.Text = "";
                txtGRPODate.Text = "";
                txtGRPONum.Text = "";
                txtItemCode.Text = "";
                txtItemName.Text = "";
                txtRemainingQuant.Text = "";
                txtGRPOQuanTon.Text = "";
                txtVehicleNUm.Text = "";
                txtDaySerial.Text = "";
                txtDriverCNIC.Text = "";
                txtDriverName.Text = "";
                txt1WeighDate.Text = DateTime.Now.ToShortDateString();
                txt1WeighTime.Text = DateTime.Now.ToShortTimeString();
                txt1WeighKG.Text = "";
                txt1WeighTon.Text = "";
                cmbTransportCode.Text = "";
                cmbTransportType.Text = "";
                cmbYardType.Text = "";
                txtPathBrand.Text = "";
                txtPathCnic.Text = "";
                txt2WeighDate.Text = DateTime.Now.ToShortDateString();
                txt2WeighTime.Text = DateTime.Now.ToShortTimeString();
                txtNetWeighKG.Text = "";
                txtNetWeighTon.Text = "";
                txtReturnQuant.Text = "";
                flgSetValues = false;
                txtToleranceLimit.Text = "";
                txtToleranceLimit.Visible = false;
                chkAllowTolerance.Checked = false;
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
                    //FrontCam = oDoc.FrontCameraLOC;
                    //BackCam = oDoc.BackCameraLOC;
                    //FCUserName = oDoc.UserNameFC;
                    //FCPassword = oDoc.PasswordFC;
                    //BCUserName = oDoc.UserNameBC;
                    //BCPassword = oDoc.PasswordBC;
                    //ActiveSeries = oDoc.Series;
                    //CamTimer = Convert.ToString(oDoc.CamTimer);
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
                TrnsRawMaterialReturn oDoc = new TrnsRawMaterialReturn();

                oDoc.FCreateDate = DateTime.Now;
                oDoc.FCreateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                oDoc.FUpdateDate = DateTime.Now;

                var getTrnsDispatch = oDB.TrnsRawMaterialReturn.Where(x => x.DocNum == Convert.ToInt32(txtVoucherNo.Text));

                if (getTrnsDispatch.Count() > 0)
                {

                    var oOld = getTrnsDispatch.FirstOrDefault();
                   
                    oOld.Flg2Rpt = false;
                    oOld.FlgWBrpt = false;
                    // oOld.FDocDate = Convert.ToDateTime(txtDate.Text);
                    // oOld.FShift = Convert.ToInt32(shiftid);
                    // oOld.FTime = txtTime.Text;
                    oOld.GRPONum = txtGRPONum.Text;
                    oOld.GRPODate = Convert.ToDateTime(txtGRPODate.Text);
                    oOld.VendorCode = txtVendorCode.Text;
                    oOld.VendorName = txtVendorName.Text;
                    oOld.ItemCode = txtItemCode.Text;
                    oOld.ItemName = txtItemName.Text;
                    oOld.GRPOQuantity = Convert.ToDecimal(txtGRPOQuanTon.Text);
                    oOld.VehicleNum = txtVehicleNUm.Text;
                    oOld.DriverCNIC = txtDriverCNIC.Text;
                    oOld.DriverName = txtDriverName.Text;
                    oOld.DriverPath = txtPathCnic.Text;
                    //oOld.FWeighmentDate = Convert.ToDateTime(txt1WeighDate.Text);
                    // oOld.FWeighmentTime = txt1WeighTime.Text;
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportType.SelectedValue)))
                    {
                        oOld.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.SelectedValue)))
                    {
                        oOld.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(cmbYardType.SelectedValue)))
                    {
                        oOld.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                    }

                    oOld.TransportName = txtTransportName.Text;
                    // oOld.FCreateDate = DateTime.Now;
                    // oOld.FCreateBy = Program.oCurrentUser.UserCode;
                    //  oOld.FUpdateBy = Program.oCurrentUser.UserCode;
                    //  oOld.FUpdateDate = DateTime.Now;
            
                    if (Convert.ToDecimal(txt1WeighKG.Text) > 0)

                    {
                        if (!string.IsNullOrEmpty(txt1WeighKG.Text))
                        {
                            oOld.FWeighmentTon = Convert.ToDecimal(txt1WeighKG.Text);
                            oOld.FWeighmentKG = Convert.ToDecimal(txt1WeighKG.Text);
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
      
                    if (Convert.ToDecimal(txt2WeighKG.Text) > 0)
                    {
                        oOld.SWeighmentKG = Convert.ToDecimal(txt2WeighKG.Text);
                        oOld.SWeighmentTon = Convert.ToDecimal(txt2WeighTon.Text);
                    }
                    else
                    {
                        Program.ExceptionMsg("Second Weight can not be 0");
                        return false;
                    }
                    if (string.IsNullOrEmpty(oOld.SWeighmentKG.ToString()))
                    {

                        oOld.SWeighmentDate = Convert.ToDateTime(txt2WeighDate.Text);
                        oOld.SWeighmentTime = txt2WeighTime.Text;
                        oOld.SDocDate = DateTime.Now;
                        oOld.SShift = Convert.ToInt32(shiftid);
                        oOld.STime = txt2WeighTime.Text;
                    }
                    oOld.NetWeightKG = Convert.ToDecimal(txtNetWeighKG.Text);
                    oOld.NetWeightTon = Convert.ToDecimal(txtNetWeighTon.Text);
                    // oOld.ReturnQuantity = Convert.ToDecimal(txtReturnQuant.Text);
                    if (chkAllowTolerance.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtToleranceLimit.Text))
                        {
                            oOld.FlgTolerance = chkAllowTolerance.Checked;
                            oOld.ToleranceLimit = Convert.ToDecimal(txtToleranceLimit.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("Tolerance Limit can not be Empty");
                            return false;
                        }
                    }
                    if (!ChkTolerence())
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(oOld.SCreateBy))
                    {
                        oOld.SCreateBy = Program.oCurrentUser.UserCode;
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(oOld.SCreateDate)))
                    {
                        oOld.SCreateDate = (DateTime.Now);
                    }
                    oOld.SUpdateBy = Program.oCurrentUser.UserCode;
                    oOld.SUpdateDate = DateTime.Now;

                    if (string.IsNullOrEmpty(oOld.SCreateBy))
                    {
                        oOld.SCreateBy = Program.oCurrentUser.UserCode;
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(oOld.SCreateDate)))
                    {
                        oOld.SCreateDate = (DateTime.Now);
                    }

                    oOld.ItemGroupCode = ItemGroupCode;
                    oOld.ItemGroupName = txtItemGrpName.Text;
                    oOld.RemainingQuantity = Convert.ToDecimal(txtRemainingQuant.Text);
                    if (!string.IsNullOrEmpty(txt2WeighKG.Text))
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
                    oDoc.YardId = Convert.ToInt32(cmbYardType.SelectedValue);
                    oDoc.Flg1Rpt = false;
                    oDoc.FTime = txtTime.Text;
                    oDoc.DaySeries = Convert.ToInt32(txtDaySerial.Text);
                    oDoc.GRPONum = txtGRPONum.Text;
                    oDoc.GRPODate = Convert.ToDateTime(txtGRPODate.Text);
                    oDoc.VendorCode = txtVendorCode.Text;
                    oDoc.VendorName = txtVendorName.Text;
                    oDoc.ItemCode = txtItemCode.Text;
                    oDoc.ItemName = txtItemName.Text;
                    oDoc.GRPOQuantity = Convert.ToDecimal(txtGRPOQuanTon.Text);
                    oDoc.VehicleNum = txtVehicleNUm.Text;
                    oDoc.DriverCNIC = txtDriverCNIC.Text;
                    oDoc.DriverName = txtDriverName.Text;
                    oDoc.DriverPath = txtPathCnic.Text;
                    oDoc.FWeighmentDate = Convert.ToDateTime(txt1WeighDate.Text);
                    oDoc.FWeighmentTime = txt1WeighTime.Text;

                   
                    if (Convert.ToDecimal(txt1WeighKG.Text) > 0)

                    {
                        if (!string.IsNullOrEmpty(txt1WeighKG.Text))
                        {
                            oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeighKG.Text);
                            oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeighKG.Text);
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
                    oDoc.FCreateDate = DateTime.Now;
                    oDoc.FCreateBy = Program.oCurrentUser.UserCode;
                    oDoc.FUpdateBy = Program.oCurrentUser.UserCode;
                    oDoc.FUpdateDate = DateTime.Now;
                    oDoc.FlgSecondWeight = false;
                    //if (!string.IsNullOrEmpty(txt2WeighKG.Text))
                    //{
                    //    oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeighKG.Text);
                    //}
                    //else
                    //{
                    //    oDoc.SWeighmentKG = 0;
                    //}

                    //if (!string.IsNullOrEmpty(txt2WeighTon.Text))
                    //{
                    //    oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeighTon.Text);
                    //}
                    //else
                    //{
                    //    oDoc.SWeighmentTon = 0;
                    //}
                    //oDoc.SWeighmentDate = DateTime.Now;
                    //oDoc.SWeighmentTime = txt2WeighTime.Text;
                    //oDoc.SDocDate = DateTime.Now;
                    //oDoc.SShift = Convert.ToInt32(shiftid);
                    //oDoc.STime = txt2WeighTime.Text;

                    //if (!string.IsNullOrEmpty(txtNetWeighKG.Text))
                    //{
                    //    oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeighKG.Text);
                    //}
                    //else
                    //{
                    //    oDoc.NetWeightKG = 0;
                    //}

                    //if (!string.IsNullOrEmpty(txtNetWeighTon.Text))
                    //{
                    //    oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeighTon.Text);
                    //}
                    //else
                    //{
                    //    oDoc.NetWeightTon = 0;
                    //}

                    if (!string.IsNullOrEmpty(txtReturnQuant.Text))
                    {
                        oDoc.ReturnQuantity = Convert.ToDecimal(txtReturnQuant.Text);
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

                    if (!string.IsNullOrEmpty(txtRemainingQuant.Text))
                    {
                        oDoc.RemainingQuantity = Convert.ToDecimal(txtRemainingQuant.Text);
                    }
                    else
                    {
                        oDoc.RemainingQuantity = 0;
                    }
                    oDB.TrnsRawMaterialReturn.InsertOnSubmit(oDoc);
                }
                oDB.SubmitChanges();
                Program.SuccesesMsg("Record Successfully Added.");

            }
            catch (Exception Ex)
            {
                retValue = false;
                Program.oErrMgn.LogException(Program.ANV, Ex);
                Program.ExceptionMsg("Values are Missing or Incorrect.");
            }


            return retValue;
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

                string strQuery = @"select CardCode,CardName +':'+ CardCode as CardCodeName from ocrd Where GroupCode = 108";

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

        //private Boolean UpdateRecord()
        //{
        //    Boolean retValue = true;
        //    try
        //    {
        //        string value = txtVoucherNo.Text;
        //        string purpose = cmbSourceDoc.SelectedItem.Text;
        //        TrnsWeighment oDoc = (from a in oDB.TrnsWeighment where a.VoucherNo == value select a).FirstOrDefault();

        //        oDoc.BPCode = txtBPCode.Text;
        //        oDoc.BPName = txtBPName.Text;
        //        oDoc.DriverName = txtDriverName.Text;
        //        oDoc.VehicleRegNo = txtVehicleRegNo.Text;
        //        oDoc.EngineStatus = cmbEngineStatus.SelectedItem.Text;
        //        oDoc.DriverStatus = cmbDriverStatus.SelectedItem.Text;
        //        oDoc.Remarks = txtRemarks.Text;


        //        oDoc.TransactionState = FormState;

        //        oDoc.UpdateDt = DateTime.Now;
        //        oDoc.UpdatedBy = Program.oCurrentUser.UserCode;

        //        //Add Item Detail   
        //        if (FormState % 2 == 0)
        //        {

        //            TrnsWeighmentDetails oDetail = oDoc.TrnsWeighmentDetails[Convert.ToInt32(oDoc.ItemCount) - 1];
        //            oDetail.ItemCode = txtItemCode.Text;
        //            oDetail.ItemName = txtItemName.Text;
        //            if (purpose == "Sales Order" || purpose == "Purchase Return" || purpose == "AP Credit Memo")
        //            {
        //                oDetail.FirstWeight = Convert.ToDecimal(lblWeight.Text);
        //                TrnsImages oImg = new TrnsImages();
        //                oImg.FrontImage = ImageToByteArray(getCamBmp(FrontCam, FCUserName, FCPassword));
        //                oImg.BackImage = ImageToByteArray(getCamBmp(BackCam, BCUserName, BCPassword));
        //                oImg.CreateDt = DateTime.Now;
        //                oImg.UpdateDt = DateTime.Now;
        //                oImg.CreatedBy = Program.oCurrentUser.UserCode;
        //                oImg.UpdatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.TrnsImages = oImg;
        //            }
        //            else if (purpose == "Purchase Order" || purpose == "Sales Return" || purpose == "AR Credit Memo")
        //            {
        //                oDetail.SecondWeight = Convert.ToDecimal(lblWeight.Text);
        //                TrnsImages oImg = new TrnsImages();
        //                oImg.FrontImage = ImageToByteArray(getCamBmp(FrontCam, FCUserName, FCPassword));
        //                oImg.BackImage = ImageToByteArray(getCamBmp(BackCam, BCUserName, BCPassword));
        //                oImg.CreateDt = DateTime.Now;
        //                oImg.UpdateDt = DateTime.Now;
        //                oImg.CreatedBy = Program.oCurrentUser.UserCode;
        //                oImg.UpdatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.TrnsImages = oImg;
        //            }
        //            if (Convert.ToBoolean(Program.oCurrentUser.FlgSuper))
        //            {
        //                oDetail.Status = "Manual";
        //            }
        //            else
        //            {
        //                oDetail.Status = "Auto";
        //            }

        //            oDetail.UpdateDt = DateTime.Now;
        //        }
        //        else
        //        {
        //            TrnsWeighmentDetails oDetail = new TrnsWeighmentDetails();
        //            oDoc.TrnsWeighmentDetails.Add(oDetail);
        //            oDoc.TransactionState = FormState + 1;
        //            oDetail.ItemCode = txtItemCode.Text;
        //            oDetail.ItemName = txtItemName.Text;
        //            if (purpose == "Sales Order" || purpose == "Purchase Return" || purpose == "AP Credit Memo")
        //            {
        //                oDetail.SecondWeight = oDoc.TrnsWeighmentDetails[Convert.ToInt32(oDoc.ItemCount) - 1].FirstWeight;
        //                oDetail.FirstWeight = Convert.ToDecimal(lblWeight.Text);
        //                TrnsImages oImg = new TrnsImages();
        //                oImg.FrontImage = ImageToByteArray(getCamBmp(FrontCam, FCUserName, FCPassword));
        //                oImg.BackImage = ImageToByteArray(getCamBmp(BackCam, BCUserName, BCPassword));
        //                oImg.CreateDt = DateTime.Now;
        //                oImg.UpdateDt = DateTime.Now;
        //                oImg.CreatedBy = Program.oCurrentUser.UserCode;
        //                oImg.UpdatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.TrnsImages = oImg;
        //            }
        //            else if (purpose == "Purchase Order" || purpose == "Sales Return" || purpose == "AR Credit Memo")
        //            {
        //                oDetail.SecondWeight = oDoc.TrnsWeighmentDetails[Convert.ToInt32(oDoc.ItemCount) - 1].FirstWeight;
        //                oDetail.FirstWeight = Convert.ToDecimal(lblWeight.Text);
        //                TrnsImages oImg = new TrnsImages();
        //                oImg.FrontImage = ImageToByteArray(getCamBmp(FrontCam, FCUserName, FCPassword));
        //                oImg.BackImage = ImageToByteArray(getCamBmp(BackCam, BCUserName, BCPassword));
        //                oImg.CreateDt = DateTime.Now;
        //                oImg.UpdateDt = DateTime.Now;
        //                oImg.CreatedBy = Program.oCurrentUser.UserCode;
        //                oImg.UpdatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.TrnsImages = oImg;
        //            }
        //            oDoc.ItemCount = Convert.ToInt32(oDoc.ItemCount) + 1;
        //            if (Convert.ToBoolean(Program.oCurrentUser.FlgSuper))
        //            {
        //                oDetail.Status = "Manual";
        //            }
        //            else
        //            {
        //                oDetail.Status = "Auto";
        //            }
        //            oDetail.CreateDt = DateTime.Now;
        //            oDetail.UpdateDt = DateTime.Now;
        //        }
        //        oDB.SubmitChanges();
        //        Program.SuccesesMsg("Record Successfully Updated.");
        //    }
        //    catch (Exception Ex)
        //    {
        //        retValue = false;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //    return retValue;
        //}

        //private Boolean ValidateAdd()
        //{
        //    Boolean retValue = true;
        //    try
        //    {

        //    }
        //    catch (Exception Ex)
        //    {
        //        retValue = false;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //    return retValue;
        //}

        //private void FillRecord()
        //{
        //    try
        //    {
        //        SboOGIP oDoc = (from a in oDB.SboOGIP where a.ID.ToString() == OpenObjectID select a).FirstOrDefault();
        //        if (oDoc != null)
        //        {
        //            txtBPCode.Text = oDoc.CardCode;
        //            txtBPName.Text = oDoc.CardName;
        //            txtSourceDocNo.Text = oDoc.BaseDoc;
        //            txtGatePass.Text = oDoc.DocEntry;
        //            txtDriverName.Text = oDoc.Driver;
        //            txtVehicleRegNo.Text = oDoc.VehicleReg;
        //            cmbEngineStatus.SelectedIndex = 1;
        //            cmbDriverStatus.SelectedIndex = 2;
        //            FormState = 1;
        //            TrnsWeighment oWB = (from a in oDB.TrnsWeighment where a.GatePass == oDoc.DocEntry select a).FirstOrDefault();
        //            if (oWB == null)
        //            {
        //                btnSubmit.Text = "&Add";
        //                txtVoucherNo.Text = GetVoucherNumber();

        //            }
        //            else
        //            {
        //                btnSubmit.Text = "&Update";
        //                txtVoucherNo.Text = Convert.ToString(oWB.VoucherNo);
        //                picFront.Image = ByteArrayToImage(GetBinaryArrayFront(oWB.TrnsImages.ID));
        //                picBack.Image = ByteArrayToImage(GetBinaryArrayBack(oWB.TrnsImages.ID));
        //                FormState = Convert.ToInt32(oWB.TransactionState) + 1;
        //                cmbSourceDoc.SelectedValue = Convert.ToString(oWB.Purpose);
        //                string purpose = Convert.ToString(oWB.Purpose);
        //                txtRemarks.Text = oWB.Remarks;
        //                if (FormState % 2 == 0)
        //                {
        //                    txtItemCode.Text = oWB.TrnsWeighmentDetails[Convert.ToInt32(oWB.ItemCount) - 1].ItemCode;
        //                    txtItemName.Text = oWB.TrnsWeighmentDetails[Convert.ToInt32(oWB.ItemCount) - 1].ItemName;
        //                    if (purpose == "Sales Order" || purpose == "Purchase Return" || purpose == "AP Credit Memo")
        //                    {
        //                        txtTareWeight.Text = Convert.ToString(oWB.TrnsWeighmentDetails[Convert.ToInt32(oWB.ItemCount) - 1].SecondWeight);
        //                    }
        //                    else if (purpose == "Purchase Order" || purpose == "Sales Return" || purpose == "AR Credit Memo")
        //                    {
        //                        txtGrossWeight.Text = Convert.ToString(oWB.TrnsWeighmentDetails[Convert.ToInt32(oWB.ItemCount) - 1].FirstWeight);
        //                    }
        //                    Int64 imageid = Convert.ToInt64(oWB.TrnsWeighmentDetails[Convert.ToInt32(oWB.ItemCount) - 1].ImgID);
        //                    picLastFront.Image = ByteArrayToImage(GetBinaryArrayFront(imageid));
        //                    picLastBack.Image = ByteArrayToImage(GetBinaryArrayBack(imageid));
        //                }
        //                else
        //                {
        //                    Int64 imageid = Convert.ToInt64(oWB.TrnsWeighmentDetails[Convert.ToInt32(oWB.ItemCount) - 1].ImgID);
        //                    picLastFront.Image = ByteArrayToImage(GetBinaryArrayFront(imageid));
        //                    picLastBack.Image = ByteArrayToImage(GetBinaryArrayBack(imageid));
        //                }
        //                FillGrid(oWB);
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //private void FillGrid(TrnsWeighment oDoc)
        //{
        //    try
        //    {
        //        dtRows.Rows.Clear();
        //        int serial = 1;
        //        foreach (TrnsWeighmentDetails oDetail in oDoc.TrnsWeighmentDetails)
        //        {
        //            DataRow drRow = dtRows.NewRow();
        //            drRow["ID"] = Convert.ToString(oDetail.ID);
        //            drRow["Serial"] = Convert.ToString(serial);
        //            drRow["ItemCode"] = Convert.ToString(oDetail.ItemCode);
        //            drRow["ItemName"] = Convert.ToString(oDetail.ItemName);
        //            if (oDetail.FirstWeight != null)
        //            {
        //                drRow["Gross"] = mFm.ApplyFormat(Convert.ToString(oDetail.FirstWeight), "D");
        //            }
        //            else
        //            {
        //                drRow["Gross"] = Convert.ToString("0.00");
        //            }
        //            if (oDetail.SecondWeight != null)
        //            {
        //                drRow["Tare"] = mFm.ApplyFormat(Convert.ToString(oDetail.SecondWeight),"D");
        //            }
        //            else
        //            {
        //                drRow["Tare"] = Convert.ToString("0.00");
        //            }
        //            drRow["Status"] = Convert.ToString(oDetail.Status);
        //            dtRows.Rows.Add(drRow);
        //            serial++;
        //        }
        //        grdDetails.DataSource = dtRows;
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //private void HandleDialogControl()
        //{
        //    try
        //    {
        //        frmOpenDlg oDlg = new frmOpenDlg();
        //        oDlg.OpenFor = "sboOGIP";
        //        oDlg.flgToggle = flgToggle;
        //        oDlg.ShowDialog();
        //        if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
        //        {
        //            OpenObjectID = oDlg.SelectedObjectID;
        //        }
        //        if (!String.IsNullOrEmpty(OpenObjectID))
        //        {
        //            FillRecord();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        //Program.ExceptionMsg(Ex.Message);
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        private void HandleDialogControlItems()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oGRPOItems";
                oDlg.Text = "Item Selector";
                oDlg.SourceDocNum = txtVendorCode.Text;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtRemainingQuant.Text = oDlg.Balance;
                    // txtSBRDate.Text = oDlg.DocDate;
                    txtGRPOQuanTon.Text = Math.Round(Convert.ToDecimal(oDlg.Quantity), 3).ToString();
                    // txtSBRNum.Text = oDlg.DocNum;

                    //Code By : BILAL AHMED
                    //Chaneged By : Ghulam
                    //Date : 04-10-2019
                    //txtDriverCNIC.Text = oDlg.DriverCnic;
                    //txtDriverName.Text = oDlg.U_Driver;
                    //txtVehicleNUm.Text = oDlg.U_VehcleNo;


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

        private string GetVoucherNumber()
        {
            string retValue = "";
            try
            {
                int value = (from a in oDB.TrnsRawMaterialReturn select a.DocNum).Count();

                retValue = (value + 1).ToString();
            }
            catch (Exception Ex)
            {
                retValue = "-1";
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            return retValue;
        }

        //private void LoadItemsCombo()
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(txtSourceDocNo.Text))
        //        {
        //            string SourceDocument = cmbSourceDoc.SelectedText;
        //            switch (SourceDocument)
        //            {
        //                case "Sales Order":
        //                    //SalesOrderItem();
        //                    break;
        //                case "Sales Return":
        //                    //SalesReturnItem();
        //                    break;
        //                case "AR Credit Memo":
        //                    //ARCreditMemoItem();
        //                    break;
        //                case "Purchase Order":
        //                    //PurchaseOrderItem();
        //                    break;
        //                case "Purchase Return":
        //                    //PurchaseReturnItem();
        //                    break;
        //                case "AP Credit Memo":
        //                    //APCreditMemoItem();
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //public Image ByteArrayToImage(byte[] byteArrayIn)
        //{
        //    Image returnImage = null;
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream(byteArrayIn))
        //        {
        //            returnImage = Image.FromStream(ms);
        //            return returnImage;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        returnImage = (Bitmap) mfmFFS.Properties.Resources.ResourceManager.GetObject("NoImage.bmp");
        //        return returnImage;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        //{
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (imageIn != null)
        //            {
        //                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            }
        //            return ms.ToArray();

        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        return null;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        private void ConnectPort()
        {
            if (comportRawR.IsOpen) comportRawR.Close();
            bool tmrc = tmrCamFront.Enabled;
            try
            {
                // Set the port's settings
                // tmrCamFront.Enabled = false;

                comportRawR.BaudRate = Convert.ToInt32(BaudRate);
                comportRawR.DataBits = Convert.ToInt32(DataBits);
                comportRawR.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBit);
                comportRawR.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                comportRawR.PortName = ComPort;
                comportRawR.ReadTimeout = 5000;

                Program.WarningMsg("Trying to connect.");
                comportRawR.Open();
                Program.SuccesesMsg("Connected.");
                // tmrCamFront.Enabled = tmrc;

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

        //[STAThread]
        //private void DisplayData(string data)
        //{
        //    txtFullData.Invoke(new EventHandler(delegate { txtFullData.Text = data; }));
        //}

        //private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    Program.oErrMgn.LogEntry(Program.ANV, "port fucntion start");
        //    if (!alreadyReading)
        //    {
        //        alreadyReading = true;
        //        try
        //        {
        //            string data = comport.ReadExisting();
        //            Program.oErrMgn.LogEntry(Program.ANV, " readed data " + data);
        //            if (data.Length <= Convert.ToInt32(DataLenght) + 2)
        //            {
        //                Program.oErrMgn.LogEntry(Program.ANV, "crap data lenght ki value kam hai");
        //                return;
        //            }

        //            txtFullData.Invoke(new EventHandler(delegate
        //            {
        //                int charindex = data.IndexOf(StartChar);
        //                Program.oErrMgn.LogEntry(Program.ANV, "charindex ki value " + Convert.ToString(charindex) );
        //                txtFullData.Text = data;
        //                if (flgAscii) charindex = data.IndexOf((char)Convert.ToInt32(StartChar));
        //                long currentWeight = 0;
        //                if (charindex > 0)
        //                {
        //                    Program.oErrMgn.LogEntry(Program.ANV, "char index ki bari value " + Convert.ToString(charindex + Convert.ToInt32(DataLenght)));
        //                    if (data.Length > charindex + Convert.ToInt32(DataLenght))
        //                    {
        //                        currentWeight = Convert.ToInt64(data.Substring(charindex + 2, Convert.ToInt16(DataLenght)));
        //                    }
        //                }
        //                if (currentWeight == mostRecentWeight)
        //                {
        //                    txtFullData.Text = currentWeight.ToString();
        //                }
        //                Program.oErrMgn.LogEntry(Program.ANV, "most recent ki value " + Convert.ToString(mostRecentWeight));
        //                mostRecentWeight = currentWeight;
        //                Program.oErrMgn.LogEntry(Program.ANV, "current weight ki value " + Convert.ToString(currentWeight));
        //            }));

        //        }
        //        catch (Exception Ex)
        //        {
        //            Program.ExceptionMsg(" in reading:" + Ex.Message + ":Port closed");
        //            Program.oErrMgn.LogException(Program.ANV, Ex);
        //        }
        //        Program.oErrMgn.LogEntry(Program.ANV, "port fucntion end");
        //    }
        //}

        //private void getImageFront(string strUrl)
        //{
        //    try
        //    {
        //        System.Threading.Thread myThread = new System.Threading.Thread(delegate()
        //        {
        //            //Your code here
        //            if (!isLoadingImageFront)
        //            {
        //                isLoadingImageFront = true;
        //                picPreFront.Invoke(new EventHandler(delegate
        //                {
        //                    picPreFront.Image = getCamBmp(strUrl, FCUserName, FCPassword);
        //                }));
        //                isLoadingImageFront = false;
        //            }
        //        });
        //        myThread.IsBackground = true;
        //        myThread.Start();
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //private void getImageBack(string strUrl)
        //{
        //    try
        //    {
        //        System.Threading.Thread myThread = new System.Threading.Thread(delegate()
        //        {
        //            //Your code here
        //            if (!isLoadingImageBack)
        //            {
        //                isLoadingImageBack = true;
        //                picPreBack.Invoke(new EventHandler(delegate
        //                {
        //                    picPreBack.Image = getCamBmpBack(strUrl, BCUserName, BCPassword);

        //                }));


        //                isLoadingImageBack = false;
        //            }
        //        });
        //        myThread.IsBackground = true;
        //        myThread.Start();
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //private Image getCamBmp(string strUrl,string pUserName,string pPassword)
        //{

        //    Image bmp = picNoCam.Image;
        //    if (Convert.ToInt16(CamTimer) == 0) return bmp;
        //    try
        //    {
        //        string sourceURL = strUrl;
        //        byte[] buffer = new byte[1000000];
        //        int read, total = 0;
        //        // create HTTP request
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
        //        req.Credentials = new NetworkCredential(pUserName, pPassword);
        //        WebResponse resp = req.GetResponse();
        //        Stream stream = resp.GetResponseStream();
        //        while ((read = stream.Read(buffer, total, 1000)) != 0)
        //        {
        //            total += read;
        //        }
        //        bmp = (Image)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
        //    }
        //    catch(Exception Ex)
        //    {
        //        //CamTimer = "0";
        //        tmrCamFront.Enabled = false;
        //        bmp = picNoCam.Image; 
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //    return bmp;

        //}

        //private Image getCamBmpBack(string strUrl, string pUserName, string pPassword)
        //{

        //    Image bmp = picNoCam1.Image;
        //    if (Convert.ToInt16(CamTimer) == 0) return bmp;
        //    try
        //    {
        //        string sourceURL = strUrl;
        //        byte[] buffer = new byte[1000000];
        //        int read, total = 0;
        //        // create HTTP request
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
        //        req.Credentials = new NetworkCredential(pUserName, pPassword);
        //        WebResponse resp = req.GetResponse();
        //        Stream stream = resp.GetResponseStream();
        //        while ((read = stream.Read(buffer, total, 1000)) != 0)
        //        {
        //            total += read;
        //        }
        //        bmp = (Image)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
        //    }
        //    catch (Exception Ex)
        //    {
        //        //CamTimer = "0";
        //        tmrCamBack.Enabled = false;
        //        bmp = picNoCam1.Image;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //    return bmp;

        //}

        //public byte[] GetBinaryArrayFront(Int64 binaryId)
        //{
        //    try
        //    {
        //        TrnsImages img = (from p in oDB.TrnsImages where p.ID == binaryId select p).FirstOrDefault();
        //        byte[] outResult = img.FrontImage.ToArray();
        //        return outResult;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return null;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //public byte[] GetBinaryArrayBack(Int64 binaryId)
        //{
        //    try
        //    {
        //        TrnsImages img = (from p in oDB.TrnsImages where p.ID == binaryId select p).FirstOrDefault();
        //        byte[] outResult = img.BackImage.ToArray();
        //        return outResult;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return null;
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        private void AuthorizationScheme()
        {
            try
            {
                if (Convert.ToBoolean(Program.oCurrentUser.FlgSuper))
                {
                    txtGRPOQuanTon.Enabled = true;
                }
                else
                {
                    txtGRPOQuanTon.Enabled = false;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        //#endregion

        //#region FormEvents

        private void setValues(int doc)
        {
            try
            {


                flgSetValues = true;
                Program.NoMsg("");
                btnSubmit.Text = "&Add";
                TrnsRawMaterialReturn trm = (from a in oDB.TrnsRawMaterialReturn where a.DocNum == doc select a).FirstOrDefault();

                txtVoucherNo.Text = Convert.ToString(trm.DocNum);

                string[] date = trm.FDocDate.ToString().Split(' ');
                txtDate.Text = date[0];
                txtGRPONum.Text = trm.GRPONum;
                txtVendorCode.Text = trm.VendorCode;
                txtItemCode.Text = trm.ItemCode;
                txtGRPOQuanTon.Text = Convert.ToString(trm.GRPOQuantity);
                txtItemGrpName.Text = trm.ItemGroupName;
                txtVehicleNUm.Text = trm.VehicleNum;
                txtDriverCNIC.Text = trm.DriverCNIC;
                txtPathCnic.Text = trm.DriverPath;
                string tCodeName = @"select  CardName +':'+ CardCode as CardCode from ocrd Where GroupCode = 108 and CardCode ='" + trm.TransportCode + "'";
                string tCode = mFm.ExecuteQueryScaler(tCodeName, Program.ConStrSAP);
                cmbTransportCode.Text = tCode;
                int Yard = Convert.ToInt32(trm.YardId);
                string YardName = (from a in oDB.MstYardType where a.ID == Yard select a.Name).FirstOrDefault();
                cmbYardType.Text = YardName;
                int ttype = Convert.ToInt32(trm.TransportID);
                string ttypeName = (from a in oDB.MstTransportType where a.ID == ttype select a.Name).FirstOrDefault();
                cmbTransportType.Text = ttypeName;

                string[] fdate = trm.FWeighmentDate.ToString().Split(' ');
                txt1WeighDate.Text = fdate[0];
                txt1WeighKG.Text = Convert.ToString(trm.FWeighmentKG);
                //cmbTransportCode.Text = trm.TransportCode;
                //cmbTransportType.Text = Convert.ToString(trm.TransportID);

                txtDaySerial.Text = Convert.ToString(trm.DaySeries);
                if (!string.IsNullOrEmpty(Convert.ToString(trm.SWeighmentDate)))
                {
                    string[] sdate = trm.SWeighmentDate.ToString().Split(' ');
                    txt2WeighDate.Text = sdate[0];
                }
                else
                {
                    txt2WeighDate.Text = DateTime.Now.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(trm.SWeighmentTime))
                {
                    txt2WeighTime.Text = trm.SWeighmentTime;
                }
                else
                {
                    txt2WeighTime.Text = DateTime.Now.ToShortTimeString();
                }


                txt2WeighKG.Text = Convert.ToString(trm.SWeighmentKG);
                txtNetWeighKG.Text = Convert.ToString(trm.NetWeightKG);
                long shift = Convert.ToInt32(trm.FShift);
                string shiftName = Convert.ToString((from a in oDB.MstShift where a.ID == shift select a.Name).FirstOrDefault());
                txtShift.Text = shiftName;
                txtTime.Text = trm.FTime;
                txtVendorName.Text = trm.VendorName;

                string[] podate = trm.GRPODate.ToString().Split(' ');
                txtGRPODate.Text = podate[0];
                txtItemName.Text = trm.ItemName;

                txtRemainingQuant.Text = Convert.ToString(trm.RemainingQuantity);
                // txtDaySeries.Text = trm.
                //cmbPacker.Text = Convert.ToString(trm.PackerId);
                txtDriverName.Text = trm.DriverName;
                txt1WeighTime.Text = trm.FWeighmentTime;
                txt1WeighTon.Text = Convert.ToString(trm.FWeighmentTon);
                txtTransportName.Text = trm.TransportName;
                //txtCNICPath.Text =
                //txtBrandPath.Text = 
                // txt2WeighTime.Text = trm.SWeighmentTime;
                txt2WeighTon.Text = Convert.ToString(trm.SWeighmentTon);
                txtNetWeighTon.Text = Convert.ToString(trm.NetWeightTon);
                if (flgSetValues == true)
                {
                    lblWeight.Text = "";
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);
                if (super == true)
                {
                    txtVoucherNo.Enabled = true;
                    txtGRPONum.Enabled = true;
                    txtGRPODate.Enabled = true;
                    txtShift.Enabled = true;
                    txtVendorName.Enabled = true;
                    txtVendorCode.Enabled = true;
                    txtItemCode.Enabled = true;
                    txtItemGrpName.Enabled = true;
                    txtItemName.Enabled = true;
                    txtGRPOQuanTon.Enabled = true;
                    txtRemainingQuant.Enabled = true;
                    txtDate.Enabled = true;
                    txtTime.Enabled = true;
                    txtDaySerial.Enabled = true;

                    txt1WeighDate.Enabled = true;
                    txt1WeighKG.Enabled = true;
                    txt1WeighTime.Enabled = true;
                    txt1WeighTon.Enabled = true;
                    txt2WeighDate.Enabled = true;
                    txt2WeighKG.Enabled = true;
                    txt2WeighTime.Enabled = true;
                    txt2WeighTon.Enabled = true;
                    txtNetWeighKG.Enabled = true;
                    txtNetWeighTon.Enabled = true;
                    txtDiffGRPONtWt.Enabled = true;
                }
                txtCWeight.Enabled = false;


            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Set Values " + ex);
            }

        }
        
        private void HandleDialogControlGRPO()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oDoGPRO";
                oDlg.Text = "Order Selector";
                // oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtVendorCode.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtRemainingQuant.Text = oDlg.Balance;
                    // txtSBRDate.Text = oDlg.DocDate;
                    txtGRPONum.Text = oDlg.DocNum;
                    Program.DocNum = txtGRPONum.Text;
                    txtGRPOQuanTon.Text = oDlg.Quantity;
                    // txtSBRNum.Text = oDlg.DocNum;
                    txtDriverCNIC.Text = oDlg.DriverCnic;
                    txtDriverName.Text = oDlg.U_Driver;
                    txtItemCode.Text = oDlg.ItemCode;
                    txtItemName.Text = oDlg.Dscription;
                    txtItemGrpName.Text = oDlg.ItmsGrpNam;
                    txtVehicleNUm.Text = oDlg.U_VehcleNo;
                    txtVendorCode.Text = oDlg.VendorCode;
                    txtVendorName.Text = oDlg.VendorName;
                    txtGRPODate.Text = oDlg.DocDate;

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
                oDlg.SourceDocNum = txtGRPONum.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                //if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                //{
                //    txtRemainingQuant.Text = oDlg.SelectedObjectID;
                //    txtGRPODate.Text = oDlg.SelectedObjectIDComplex;
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
                oDlg.OpenFor = "trnsRawMaterialReturn";
                oDlg.SourceDocNum = txtGRPONum.Text;

                oDlg.ShowDialog();
                oDlg.Dispose();
            }
            catch (Exception ex)
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
        
        //private void btnInOut_ToggleStateChanged(object sender, StateChangedEventArgs args)
        //{
        //    try
        //    {
        //        if (btnInOut.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
        //        {
        //            btnInOut.Text = "In";
        //            flgToggle = false;
        //        }
        //        if (btnInOut.ToggleState == Telerik.WinControls.Enumerations.ToggleState.Off)
        //        {
        //            btnInOut.Text = "Out";
        //            flgToggle = true;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //    }
        //}

        private void DisplayData(string data)
        {
            lblWeight.Invoke(new EventHandler(delegate { lblWeight.Text = data; }));
        }
        
        private void LoadGrid()
        {
            try
            {
                dt.Clear();
                CreateDt();
                IEnumerable<TrnsRawMaterialReturn> getData = from a in oDB.TrnsRawMaterialReturn where a.FlgSecondWeight == false select a;

                foreach (TrnsRawMaterialReturn item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["Wmnt#"] = item.DocNum;
                    dtrow["Vehicle#"] = item.ItemCode;
                    dtrow["GRPO#"] = item.GRPONum;
                    dtrow["ItemName"] = item.ItemName;
                    dtrow["First Weight"] = item.FWeighmentKG;
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

        public int daySeries()
        {
            int dSeries = 0;
            try
            {
                if (!string.IsNullOrEmpty(txtItemGrpName.Text))
                {
                    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    int fSeries = (from a in oDB.TrnsRawMaterialReturn
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
            decimal exceed = 0, Tolerence;
            string itemGrp = txtItemGrpName.Text;
            if (chkAllowTolerance.Checked)
            {
                Tolerence = Convert.ToDecimal(txtToleranceLimit.Text);
            }
            else
            {
                Tolerence = Convert.ToDecimal(oDB.MstTolerance.Where(x => x.TName == itemGrp).FirstOrDefault().TRate);
            }
            
            if (Convert.ToDecimal(txtNetWeighTon.Text) < Convert.ToDecimal(txtRemainingQuant.Text))
            {
                return true;
            }
            else
            {
                Program.WarningMsg("Net Weight cannot b greater than Balance Quantity");
                return false;
            }

            if (Tolerence > 0)
            {
                val1 = (Convert.ToDecimal(txtRemainingQuant.Text) * (Tolerence / 100)) + (Convert.ToDecimal(txtRemainingQuant.Text));
                // val2 = ((Convert.ToDecimal(txtDOQuantity.Text) - Convert.ToDecimal(txtDOQuantity.Text) * (Tolerence / 100)));

                if (val1 > Convert.ToDecimal(txtNetWeighTon.Text))
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
                    exceed = Convert.ToDecimal(txtNetWeighTon.Text) - val1;
                    Program.WarningMsg("tolerence increases" + exceed + " Ton");
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




            //return false;

        }

        private void Indicator01()
        {

            if (!alreadyReading)
            {
                try
                {
                    alreadyReading = true;
                    Thread.Sleep(500);
                    string data = comportRawR.ReadExisting();
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
                    string data = comportRawR.ReadLine();
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

        #region Events

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                oDB = new dbFFS(Program.ConStrApp);
                frmOpenDlg od = new frmOpenDlg();
                string critaria = txtVoucherNo.Text;
                string PldFor = Program.Screen;
                var Doc = oDB.TrnsRawMaterialReturn.Where(x => x.DocNum == Convert.ToInt32(critaria)).FirstOrDefault();


                if (btnSubmit.Text == "&Add")
                {
                    if (AddRecord())
                    {
                        var RMRadd = oDB.TrnsRawMaterialReturn.Where(x => x.DocNum == Convert.ToInt32(critaria)).FirstOrDefault();
                        if (od.AddSealToDb())
                        {
                            if (Program.OpenLayout(PldFor, critaria, PldFor + " " + critaria))
                            {
                                try
                                {
                                    RMRadd.Flg1Rpt = true;

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

        private void radButton1_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
                if (btnCancel.Text == "Close")
                {
                    oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
                }
                if (oResult == System.Windows.Forms.DialogResult.Cancel)
                {

                   // comportRawR.Close();
                    if (comportRawR.IsOpen)
                    {
                        comportRawR.DiscardInBuffer();
                        comportRawR.DiscardOutBuffer();
                        comportRawR.Close();
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

        private void grdDetails_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            int DocNum = 0;
            DocNum = Convert.ToInt32(e.Row.Cells["Wmnt#"].Value);
            setValues(DocNum);
            txtDriverCNIC.Enabled = true;
            txtDriverName.Enabled = true;
            txtTransportName.Enabled = false;
            txtVehicleNUm.Enabled = true;
            cmbYardType.Enabled = true;
            cmbTransportType.Enabled = true;
            cmbTransportCode.Enabled = true;
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
            // comportRawR.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            btnSubmit.Text = "&Update";
            //flgSetValues = false;

        }

        private void radButton3_Click(object sender, EventArgs e)
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

        private void frmRawMaterialReturn_Load(object sender, EventArgs e)
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

        private void btnAddNew_Click_1(object sender, EventArgs e)
        {
            InitiallizeDocument();
            btnSubmit.Text = "&Add";
            flgSetValues = false;
            btnItem.Enabled = true;
            radButton1.Enabled = true;
            btnSubmit.Enabled = true;
            txtDriverCNIC.Enabled = true;
            txtDriverName.Enabled = true;
            txtTransportName.Enabled = false;
            txtVehicleNUm.Enabled = true;
            cmbYardType.Enabled = true;
            cmbTransportType.Enabled = true;
            cmbTransportCode.Enabled = true;
        }

        private void txt1WeighKG_TextChanged(object sender, EventArgs e)
        {
            if (flgSetValues == true)
            {
                lblWeight.Text = "";
            }
            else
            {
                lblWeight.Text = txt1WeighKG.Text;
            }

            if (!string.IsNullOrEmpty(txt1WeighKG.Text))
            {
                txt1WeighTon.Text = Convert.ToString(Convert.ToDecimal(txt1WeighKG.Text) / 1000);

            }

            else
            {
                txt1WeighTon.Text = null;
            }
        }

        private void txt2WeighKG_TextChanged(object sender, EventArgs e)
        {
            if (flgSetValues == false)
            {
                lblWeight.Text = "";
            }
            else
            {
                lblWeight.Text = txt2WeighKG.Text;
            }

            if (!string.IsNullOrWhiteSpace(txt2WeighKG.Text))
            {
                if (!string.IsNullOrEmpty(txt2WeighKG.Text))
                {
                    txt2WeighTon.Text = Convert.ToString(Convert.ToDecimal(txt2WeighKG.Text) / 1000);
                    txtNetWeighKG.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeighKG.Text) - Convert.ToDecimal(txt2WeighKG.Text)));
                    txtNetWeighTon.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeighTon.Text) - Convert.ToDecimal(txt2WeighTon.Text)));
                    txtDiffGRPONtWt.Text = Convert.ToString(Convert.ToDecimal(txtGRPOQuanTon.Text) - Convert.ToDecimal(txtNetWeighTon.Text));
                }
            }
            else
            {
                txt2WeighTon.Text = null;
                txtNetWeighKG.Text = null;
                txtNetWeighTon.Text = null;
                txtDiffGRPONtWt.Text = null;
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

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                Program.DocNum = null;
                HandleDialogSearch();
                if (Program.DocNum != null)
                {
                    setValues(Convert.ToInt32(Program.DocNum));
                    txtDriverCNIC.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleNUm.Enabled = false;
                    cmbYardType.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
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
                    radButton1.Enabled = false;
                    btnSubmit.Enabled = false;
                }
                txtCWeight.Enabled = false;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void txtVoucherNo_TextChanged(object sender, EventArgs e)
        {
            if (Program.sealdt != null)
            {
                Program.sealdt.Clear();
            }
        }

        private void btnLastRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDB.TrnsRawMaterialReturn where a.FlgSecondWeight == true && a.FlgPosted == null select a.DocNum).Max();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    txtDriverCNIC.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleNUm.Enabled = false;
                    cmbYardType.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
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
                    radButton1.Enabled = false;
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
                checkDoc = (from a in oDB.TrnsRawMaterialReturn where a.DocNum == previusDocNum && a.FlgSecondWeight == true && a.FlgPosted == null select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    txtDriverCNIC.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleNUm.Enabled = false;
                    cmbYardType.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
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
                    radButton1.Enabled = false;
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
                checkDoc = (from a in oDB.TrnsRawMaterialReturn where a.DocNum == previusDocNum && a.FlgSecondWeight == true && a.FlgPosted == null select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    txtDriverCNIC.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleNUm.Enabled = false;
                    cmbYardType.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
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
                    radButton1.Enabled = false;
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
                var MaxRecord = (from a in oDB.TrnsRawMaterialReturn where a.FlgSecondWeight == true && a.FlgPosted == null select a.DocNum).First();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    txtDriverCNIC.Enabled = false;
                    txtDriverName.Enabled = false;
                    txtTransportName.Enabled = false;
                    txtVehicleNUm.Enabled = false;
                    cmbYardType.Enabled = false;
                    cmbTransportType.Enabled = false;
                    cmbTransportCode.Enabled = false;
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
                    radButton1.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void txtFullText_TextChanged(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txt1WeighKG.Text) || txt1WeighKG.Text == "0")
            //{
            //    txt1WeighKG.Text = txtFullText.Text;
            //}
            //else if (flgSetValues)
            //{
            //    txt2WeighKG.Text = txtFullText.Text;
            //}
            if (flgSetValues)
            {
                txt2WeighKG.Text = txtFullText.Text;
            }
            else if (string.IsNullOrEmpty(txt2WeighKG.Text))
            {
                txt1WeighKG.Text = txtFullText.Text;
            }
            lblWeight.Text = txtFullText.Text;
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
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnPreviosRecord_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnNextRecord_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnLastRecord_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception Ex)
            {
            }
        }

        //private void cmbSourceDoc_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        //{
        //    try
        //    {
        //        //LoadItemsCombo();
        //        tmrCamFront.Enabled = true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        //private void txtRTWeight_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string value = txtRTWeight.Text;
        //        //Int64 value1 = Convert.ToInt64(value);
        //        //lblWeight.Text = Convert.ToString((value1 / 10) * 10);
        //        lblWeight.Text = txtGRPOQuanTon.Text;
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        private void btnItem_Click(object sender, EventArgs e)
        {
            try
            {
                HandleDialogControlGRPO();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        
        //private void tmrCamFront_Tick(object sender, EventArgs e)
        //{
        //    if (!isLoadingImageFront)
        //    {
        //        if (FrontCam != "")
        //        {
        //            getImageFront(FrontCam);
        //        }

        //    }
        //}

        //private void tmrCamBack_Tick(object sender, EventArgs e)
        //{
        //    if (!isLoadingImageBack)
        //    {
        //        if (BackCam != "")
        //        {
        //            getImageBack(BackCam);
        //        }
        //    }
        //}

        //private void txtFullData_TextChanged(object sender, EventArgs e)
        //{
        //    txtRTWeight.Text = mostRecentWeight.ToString();
        //}

        private void tmrAlreadyReading_Tick(object sender, EventArgs e)
        {
            alreadyReading = false;
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

        private void radButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open for Driver ID Picture";

            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                txtPathCnic.Text = theDialog.FileName.ToString();
            }
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            try
            {
                string critaria = txtVoucherNo.Text;
                string PldFor = Program.Screen;
                string Fwmnt = txt1WeighKG.Text;
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

        private void txt2WeighKG_Click(object sender, EventArgs e)
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
                string strQuery = @"select CardName from ocrd Where GroupCode = 108 and CardCode ='" + CArdName1 + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                val = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

                txtTransportName.Text = val.Rows[0][0].ToString();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            //txtTransportName.Text = CardName;
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
                    txt2WeighKG.Text = lblWeight.Text;
                }
                else if (string.IsNullOrEmpty(txt2WeighKG.Text))
                {
                    txt1WeighKG.Text = lblWeight.Text;
                }

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
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
