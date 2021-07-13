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
using System.Threading;
using System.Net;
using System.Data.Linq;

namespace mfmFFS.Screens
{
    public partial class frmDispatchM : frmBaseForm
    {

        #region Variable
        //dbFFS oDB = new dbFFS(Program.ConStrApp);
        /*string OpenObjectID = "";*/
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        string ActiveSeries = "";
        string ComPort, BaudRate, Parity, StopBit, StartChar, DataBits, DataLenght, IndicatorType;
        /*Boolean flgAscii = false;
        Boolean flgToggle = false;
        Int64 mostRecentWeight = 0;
        Boolean isLoadingImageFront = false;
        Boolean isLoadingImageBack = false;
        int FormState = 0;*/
        DataTable dt = new DataTable();
        DataTable dtM = new DataTable();
        private SerialPort comportDispatch = new SerialPort();
        // private SerialPort comport = new SerialPort();
        Boolean alreadyReading = false;
        decimal val1 = 0;
        decimal val2 = 0;
        string ItemGroupCode = "";
        long shiftid = 0;
        string shiftName = "";
        public int DispatchDocNum;
        public string DispatchItemCode = "";
        bool flgSetValues = false;
        /*int ZeroHit = 0;*/
        long currentwt = 0;
        delegate void delCWeight(string value);
        bool flgMultiSales = false;
        bool flgCustChange = false, flgItemChange = false, flgReselect = false;
        decimal totalLoadingQty = 0;

        string ToleranceApprovedBy = "";
        bool flgChkStatus = false;
        //bool flgcehckexcep = false;
        public List<ItemCheckList> ItemList = new List<ItemCheckList>();

        #endregion

        #region Functions

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

        public frmDispatchM()
        {
            InitializeComponent();
            txt1WeightDate.Text = DateTime.Now.ToString();
            txt2WeightDate.Text = DateTime.Now.ToString();
            /*txtCWeight.Visible = true;
            txtCWeight.Enabled = false;*/
            //Program.comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            /*comportDispatch.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);*/
        }

        private void InitiallizeForm()
        {
            try
            {
                txtFullText.Visible = false;
                Program.flgIndicator = true;
                Program.NoMsg("");
                dt.Clear();
                dtM.Clear();
                /*GetMachineSetting();*/
                CreateDt();
                CreateMultipleDispatchDt();
                FillPacker();
                FillTransportCode();
                FillTransporttype();

                InitiallizeDocument();
                LoadGrid();
                txt2WeightDate.Text = "";
                txt2WeightKG.Text = "";
                txt2WeightTime.Text = "";
                txt2WeightTon.Text = "";
                txtNetWeightKG.Text = "";
                txtNetWeightTon.Text = "";
                txtDifferenceWeight.Text = "";

                ApplyAuthorization();

                /*tmrAlreadyReading.Interval = 1000;*/
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        public void FillTransporttype()
        {
            try
            {
                var oDoc = (from a in oDB.MstTransportType select a).ToList();

                cmbTransportType.DataSource = oDoc;
                cmbTransportType.DisplayMember = "Name";
                cmbTransportType.ValueMember = "ID";
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

                string strQuery = @"select CardCode, CardName +':'+ CardCode as CardCodeName from ocrd Where GroupCode = 108";

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

        private double TotalLoadingQty()
        {
            double totalLoadingQty = 0;
            if (grdWmntDetails.Rows.Count > 0)
            {
                for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                {
                    totalLoadingQty += Convert.ToDouble(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                }
            }
            else
            {
                txtTotalLoadingQty.Text = string.Empty;
            }
            return totalLoadingQty;
        }

        public void FillPacker()
        {
            try
            {
                var oDoc = (from a in oDB.MstPacker select a).ToList();


                cmbPacker.DataSource = oDoc;
                cmbPacker.DisplayMember = "Name";
                cmbPacker.ValueMember = "ID";
                //foreach (var data in oDoc)
                //{
                //    cmbPacker.Items.Add(data.Name);

                //}
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        public void setValues(int doc)
        {
            flgSetValues = true;
            try
            {
                Program.NoMsg("");
                btnSubmit.Text = "&Add";
                TrnsDispatchMultiHeader trm = (from a in oDB.TrnsDispatchMultiHeader where a.DocNum == doc select a).FirstOrDefault();
                //Program.FormDocNum = Convert.ToInt32(trm.DocNum);
                //txtDocNo.Text = Convert.ToString(Program.FormDocNum);
                txtDocNo.Text = Convert.ToString(trm.DocNum);
                flgMultiSales = trm.FlgMulti.GetValueOrDefault();
                string[] date = trm.FDocDate.ToString().Split(' ');
                txtCurrDate.Text = date[0];
                /*txtSBRNum.Text = trm.SBRNum;
                txtCustomerCode.Text = trm.CustomerCode;*/
                //Program.FormItemCode = trm.ItemCode;
                //txtItemCod.Text = Program.FormItemCode;
                /*txtItemCod.Text = trm.ItemCode;
                txtOrderQuantity.Text = Convert.ToString(trm.OrderQuantity).Trim();
                txtItemGroupName.Text = trm.ItemGroupName;*/
                txtVehicleNum.Text = trm.VehicleNum;
                txtDriverCNIC.Text = trm.DriverCNIC;

                string[] fdate = trm.FWeighmentDate.ToString().Split(' ');
                txt1WeightDate.Text = fdate[0];
                txt1WeightKG.Text = Convert.ToString(trm.FWeighmentKG);


                string tCodeName = @"select  CardName +':'+ CardCode as CardCode from ocrd Where GroupCode = 108 and CardCode ='" + trm.TransportCode + "'";

                string tCode = mFm.ExecuteQueryScaler(tCodeName, Program.ConStrSAP);

                //cmbTransportCode.Text = tCode;
                cmbTransportCode.SelectedIndex = (cmbTransportCode.Items.IndexOf(tCode));
                int ttype = Convert.ToInt32(trm.TransportID);
                string ttypeName = (from a in oDB.MstTransportType where a.ID == ttype select a.Name).FirstOrDefault();
                //cmbTransportType.Text = ttypeName;
                cmbTransportType.SelectedIndex = (cmbTransportType.Items.IndexOf(ttypeName));
                int pckr = Convert.ToInt32(trm.PackerId);
                string pckName = (from a in oDB.MstPacker where a.ID == pckr select a.Name).FirstOrDefault();
                //cmbPacker.Text = pckName;
                cmbPacker.SelectedIndex = (cmbPacker.Items.IndexOf(pckName));


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
                txt2WeightKG.Text = Convert.ToString(trm.SWeighmentKG);


                txt2WeightTon.Text = Convert.ToString(trm.SWeighmentTon);

                txtNetWeightKG.Text = Convert.ToString(trm.NetWeightKG);

                string shiftname = (from a in oDB.MstShift where a.ID == Convert.ToInt32(trm.FShift) select a.Name).FirstOrDefault();
                txtShift.Text = shiftname;
                txtCurrTime.Text = trm.FTime;
                /*txtCustomerName.Text = trm.CustomerName;*/

                /*string[] sbrdate = trm.SBRDate.ToString().Split(' ');
                txtSBRDate.Text = sbrdate[0];
                txtItemNam.Text = trm.ItemName;
                txtBalanceQuantity.Text = Convert.ToString(trm.BalanceQuantity);*/
                txtDaySeries.Text = Convert.ToString(trm.DaySeries);
                //cmbPacker.SelectedValue = Convert.ToString(trm.PackerId);
                txtCNICPath.Text = trm.DriverDocument;
                txtDriverName.Text = trm.DriverName;
                txt1WeightTime.Text = trm.FWeighmentTime;
                txt1WeightTon.Text = Convert.ToString(trm.FWeighmentTon);
                TxtTransportName.Text = trm.TransportName;
                lbl2.Text = Convert.ToString(trm.FlgSecondWeight);
                //txtCNICPath.Text =
                //txtBrandPath.Text = 

                txtNetWeightTon.Text = Convert.ToString(trm.NetWeightTon);
                if (flgSetValues == true)
                {
                    lblWeight.Text = "";
                }

                #region Load Multi SO
                /* FirstWeightSOList.Clear();
                 var olist = (from a in oDB.TrnsDispatchMulti
                              where a.DocNum == doc
                              select a).ToList();
                 foreach (var One in olist)
                 {
                     SaleOrderDataM oSO = new SaleOrderDataM();
                     oSO.SBRNum = One.SBRNum;
                     oSO.Balance = One.BalanceQty.GetValueOrDefault();
                     oSO.Order = One.OrderQty.GetValueOrDefault();
                     FirstWeightSOList.Add(oSO);
                 }*/

                LoadMultipleDispatchGrid(Convert.ToInt32(txtDocNo.Text));


                #endregion

                //ApplyAuthorization();

                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                //if (super == true)
                //{
                //    txtDocNo.Enabled = true;
                //    txtSBRNum.Enabled = true;
                //    txtSBRDate.Enabled = true;
                //    txtShift.Enabled = true;
                //    txtCustomerName.Enabled = true;
                //    txtCustomerCode.Enabled = true;
                //    txtItemCod.Enabled = true;
                //    txtItemGroupName.Enabled = true;
                //    txtItemNam.Enabled = true;
                //    txtOrderQuantity.Enabled = true;
                //    txtBalanceQuantity.Enabled = true;
                //    txtCurrDate.Enabled = true;
                //    txtCurrTime.Enabled = true;
                //    txtDaySeries.Enabled = true;

                //    txt1WeightDate.Enabled = true;
                //    txt1WeightKG.Enabled = true;
                //    txt1WeightTime.Enabled = true;
                //    txt1WeightTon.Enabled = true;
                //    txt2WeightDate.Enabled = true;
                //    txt2WeightKG.Enabled = true;
                //    txt2WeightTime.Enabled = true;
                //    txt2WeightTon.Enabled = true;
                //    txtNetWeightKG.Enabled = true;
                //    txtNetWeightTon.Enabled = true;
                //    txtDifferenceWeight.Enabled = true;
                //}
                //else
                //{
                //    txtOrderQuantity.Enabled = false;
                //}


                btnAdd.Enabled = false;

            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Set Values " + ex);
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
                    //dt.Columns.Add("SBRNum");
                    //dt.Columns.Add("ItemName");
                    dt.Columns.Add("First Weight");
                    dt.Columns.Add("DocDate");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void CreateMultipleDispatchDt()
        {
            try
            {
                if (dtM.Columns.Count == 0)
                {
                    dtM.Columns.Add("Customer Code");
                    dtM.Columns.Add("Customer");
                    dtM.Columns.Add("SBR#");
                    dtM.Columns.Add("SBR Date");
                    dtM.Columns.Add("Item Group Code");
                    dtM.Columns.Add("Item Group Name");
                    dtM.Columns.Add("Item Code");
                    dtM.Columns.Add("Item Name");
                    dtM.Columns.Add("Loading Qty");
                    dtM.Columns.Add("Balance Qty");
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
                Program.DocNum = string.Empty;
                btnGetWeight.Visible = true;
                //btnSubmit.Text = "&Add";
                txtDocNo.Text = GetDocNumber();
                //txtDaySeries.Text = 
                txtCurrDate.Text = DateTime.Now.Date.ToShortDateString();
                txtSBRNum.Text = "";
                txtItemGroupName.Text = "";
                txtDaySeries.Text = "";
                txtVehicleNum.Text = "";
                txtDriverCNIC.Text = "";
                txtDriverName.Text = "";
                txt1WeightKG.Text = "";
                txt1WeightTon.Text = "";
                TxtTransportName.Text = "";
                cmbTransportCode.Text = "";
                cmbTransportType.Text = "";
                txtBrandPath.Text = "";
                txtCNICPath.Text = "";
                txt2WeightDate.Text = "";
                txt2WeightTime.Text = "";
                txt2WeightKG.Text = "";
                txt2WeightTon.Text = "";
                txtCurrTime.Text = DateTime.Now.ToShortTimeString();
                txtItemCod.Text = "";
                txtItemNam.Text = "";
                txt1WeightDate.Text = DateTime.Now.ToShortDateString();
                txt1WeightTime.Text = DateTime.Now.ToShortTimeString();
                txt2WeightDate.Text = DateTime.Now.ToShortDateString();
                txt2WeightTime.Text = DateTime.Now.ToShortTimeString();
                //txtCurrDate.Text = "";
                // txtDaySeries.Text = GetDaySeries();
                // cmbSourceDoc.SelectedIndex = 0;
                txtCustomerCode.Text = "";
                txtCustomerName.Text = "";
                txtShift.Text = "";
                // txtCurrTime.Text = "";
                // cmbEngineStatus.SelectedIndex = 0;
                // cmbPacker.SelectedValue = "";
                cmbPacker.Text = "";
                txtBalanceQuantity.Text = "";
                txtSBRDate.Text = "";
                txtOrderQuantity.Text = "";
                txtShift.Text = shiftName;
                txt1WeightTime.Text = Convert.ToString(DateTime.Now.ToShortTimeString());

                flgSetValues = false;
                txtOrderQuantity.Enabled = true;

                flgMultiSales = true;

                flgCustChange = false;
                flgItemChange = false;
                flgReselect = false;
                /*SaleOrderList.Clear();
                FirstWeightSOList.Clear();*/
                //grdDetails.Rows.Clear();
                //dt.Rows.Clear();
                txtToleranceLimit.Text = "";
                txtToleranceLimit.Visible = false;
                lblToleranceLimit.Visible = false;
                chkAllowTolerance.Checked = false;
                if (btnSubmit.Text == "&Add")
                {
                    grdWmntDetails.Rows.Clear();
                }
                else
                {
                    grdWmntDetails.DataSource = null;
                }
                btnSubmit.Text = "&Add";
                btnAdd.Enabled = true;
                ItemList.Clear();
                txtTotalLoadingQty.Text = "";
                //if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                //{
                //    grdWmntDetails.Columns["Loading Qty"].ReadOnly = fasle;
                //}
                if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    grdWmntDetails.Columns["Loading Qty"].ReadOnly = false;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogSearch()
        {
            frmOpenDlg oDlg = new frmOpenDlg();
            oDlg.OpenFor = "trnsDispatchMultiHeader";
            oDlg.SourceDocNum = txtSBRNum.Text;

            oDlg.ShowDialog();
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

        /*private Boolean AddRecord()
        {
            Boolean retValue = true;
            try
            {
                using (dbFFS oDBPrivate = new dbFFS(Program.ConStrApp))
                {

                    string value = txtDocNo.Text;
                    TrnsDispatch oDoc = new TrnsDispatch();
                    //Program.FormDocNum = Convert.ToInt32(txtDocNo.Text);
                    //oDoc.DocNum = Convert.ToInt32(Program.FormDocNum);//

                    oDoc.FUpdateDate = DateTime.Now;
                    oDoc.FUpdateBy = Program.oCurrentUser.UserCode + " : " + Program.AppVersion;
                    oDoc.FCreateDate = DateTime.Now;
                    oDoc.FCreateBy = Program.oCurrentUser.UserCode + " : " + Program.AppVersion;

                    //oDoc.SCreateBy = null;
                    //oDoc.SCreateDate = null;
                    //oDoc.SUpdateBy = null;
                    //oDoc.SUpdateDate = null;



                    var getTrnsDispatch = oDBPrivate.TrnsDispatch.Where(x => x.DocNum == Convert.ToInt32(txtDocNo.Text));

                    #region Update Document
                    if (getTrnsDispatch.Count() > 0)
                    {
                        var oOld = getTrnsDispatch.FirstOrDefault();
                        //oOld.DocNum = Convert.ToInt32(txtDocNo.Text);
                        if (!string.IsNullOrEmpty(txtNetWeightKG.Text))
                        {
                            oOld.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("SecondWeighment KG can not be Empty");
                            return false;
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

                        if (Convert.ToDecimal(txt2WeightKG.Text) > 0)
                        {
                            oOld.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                            oOld.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("Second Weight can not be 0");
                            return false;
                        }
                        //oOld.FDocDate = Convert.ToDateTime(txtCurrDate.Text);
                        string CurrTime = txtCurrTime.Text;
                        oOld.SBRNum = txtSBRNum.Text;
                        //oOld.FlgMulti = flgMultiSales;
                        oOld.Flg2Rpt = false;
                        oOld.FlgWBrpt = false;
                        oOld.SBRDate = Convert.ToDateTime(txtSBRDate.Text);
                        oOld.CustomerCode = txtCustomerCode.Text;
                        oOld.CustomerName = txtCustomerName.Text;
                        //Program.FormItemCode= txtItemCod.Text;
                        //oDoc.ItemCode = Program.FormItemCode;
                        if (!ChkTolerence())
                        {
                            return false;
                        }
                        oOld.ItemCode = txtItemCod.Text;
                        oOld.ItemName = txtItemNam.Text;
                        oOld.ItemGroupName = txtItemGroupName.Text;



                        if (txtItemNam.Text.ToLower().Contains("bulk"))
                        {
                            oOld.OrderQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                        }


                        else if (string.IsNullOrEmpty(txtOrderQuantity.Text) || Convert.ToDecimal(txtOrderQuantity.Text) == 0)
                        {
                            Program.WarningMsg("Order Quantity cannot b empty or 0");
                            return false;
                        }

                        else
                        {
                            if (Convert.ToDecimal(txtOrderQuantity.Text) > Convert.ToDecimal(txtBalanceQuantity.Text))
                            {
                                Program.WarningMsg("Order Quantity cannot b greater than Balance quantity");
                                return false;
                            }
                            else
                            {
                                oOld.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                            }

                        }

                        oOld.BalanceQuantity = Convert.ToDecimal(txtBalanceQuantity.Text);
                        if (!string.IsNullOrEmpty(Convert.ToString(cmbPacker.SelectedValue)))
                        {
                            oOld.PackerId = Convert.ToInt32(cmbPacker.SelectedValue);
                        }

                        oOld.VehicleNum = txtVehicleNum.Text;
                        oOld.DriverCNIC = txtDriverCNIC.Text;
                        oOld.DriverName = txtDriverName.Text;
                        oOld.DriverDocument = txtCNICPath.Text;
                        //if (!string.IsNullOrEmpty(txt1WeightKG.Text))
                        //{
                        //    oOld.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);

                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("FirstWeighment KG can not be Empty");
                        //    return false;
                        //}

                        //if (!string.IsNullOrEmpty(txt1WeightDate.Text))
                        //{
                        //    oOld.FWeighmentDate = Convert.ToDateTime(txt1WeightDate.Text);

                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("FirstWeighment Date can not be Empty");
                        //    return false;
                        //}

                        //oOld.FWeighmentTime = a;

                        //if (!string.IsNullOrEmpty(txt1WeightTon.Text))
                        //{
                        //    oOld.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);

                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("FirstWeighment Ton can not be Empty");
                        //    return false;
                        //}
                        if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportType.SelectedValue)))
                        {
                            oOld.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.SelectedValue)))
                        {
                            oOld.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                        }

                        oOld.TransportName = TxtTransportName.Text;
                        oOld.FUpdateDate = DateTime.Now;
                        oOld.FUpdateBy = Program.oCurrentUser.UserCode;

                        if (!string.IsNullOrEmpty(txt2WeightKG.Text))
                        {
                            oOld.FlgSecondWeight = true;
                        }
                        if (!string.IsNullOrEmpty(oOld.SWeighmentKG.ToString()))
                        {

                            oOld.SDocDate = DateTime.Now;
                            if (!string.IsNullOrEmpty(txtShift.Text))
                            {
                                oOld.SShift = Convert.ToInt32(shiftidParent);
                            }
                            else
                            {
                                Program.ExceptionMsg("Second Shift can not be Empty");
                                return false;
                            }

                            oOld.STime = DateTime.Now.ToShortTimeString();

                            if (!string.IsNullOrEmpty(txt2WeightDate.Text))
                            {
                                oOld.SWeighmentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                            }
                            else
                            {
                                Program.ExceptionMsg("SecondWeighment Date can not be Empty");
                                return false;
                            }

                            if (!string.IsNullOrEmpty(txt2WeightTime.Text))
                            {
                                oOld.SWeighmentTime = DateTime.Now.ToShortTimeString();
                            }
                            else
                            {
                                Program.ExceptionMsg("SecondWeighment Time can not be Empty");
                                return false;
                            }

                        }
                        if (!string.IsNullOrEmpty(txt2WeightKG.Text))
                        {
                            oOld.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("SecondWeighment KG can not be Empty");
                            return false;
                        }

                        if (!string.IsNullOrEmpty(txt2WeightTon.Text))
                        {
                            oOld.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("SecondWeighment Ton can not be Empty");
                            return false;
                        }
                        if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
                        {
                            oOld.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                            oOld.DispatchQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("SecondWeighment Ton can not be Empty");
                            return false;
                        }
                        if (string.IsNullOrEmpty(Convert.ToString(oOld.SCreateDate)) && string.IsNullOrEmpty((oOld.SCreateBy)))
                        {
                            oOld.SCreateBy = Program.oCurrentUser.UserCode + " : " + Program.AppVersion;
                            oOld.SCreateDate = Convert.ToDateTime(DateTime.Now);
                        }
                        oOld.SUpdateBy = Program.oCurrentUser.UserCode + " : " + Program.AppVersion;
                        oOld.SUpdateDate = Convert.ToDateTime(DateTime.Now);
                        Program.SuccesesMsg("Record Updated Successfully");
                        if (flgMultiSales)
                        {
                            decimal CurOrderQty = oOld.OrderQuantity.GetValueOrDefault();
                            decimal valCurOrderQty = CurOrderQty;
                            if (!flgCustChange)
                            {
                                if (!flgItemChange)
                                {
                                    if (!flgReselect)
                                    {
                                        //oDBPrivate.RemoveAllDetailsDispatch(oOld.DocNum);
                                        Program.oErrMgn.LogEntry(Program.ANV, "1st weigh queue count: " + FirstWeightSOList.Count.ToString());
                                        string solist = string.Empty;
                                        foreach (var Previous in FirstWeightSOList)
                                        {
                                            var oCheck = (from a in SaleOrderList
                                                          where a.SBRNum == Previous.SBRNum
                                                          select a).Count();
                                            if (oCheck == 0)
                                            {
                                                SaleOrderData onew = new SaleOrderData();
                                                onew.SBRNum = Previous.SBRNum;
                                                onew.Balance = Previous.Balance;
                                                onew.Order = Previous.Order;
                                                SaleOrderList.Add(onew);
                                                solist += " " + Previous.SBRNum;
                                            }
                                        }
                                        Program.oErrMgn.LogEntry(Program.ANV, "1st weigh sbr list: " + solist);
                                    }
                                }
                            }
                            Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oOld.DocNum.ToString() + " queue count: " + SaleOrderList.Count.ToString());
                            Program.oErrMgn.LogEntry(Program.ANV, "order qty: " + CurOrderQty.ToString());
                                        List<string> ValidSaleOrder = new List<string>();
                                        foreach (var One in SaleOrderList)
                                        {
                                            if (CurOrderQty == 0)
                                            {
                                                One.Order = 0;
                                                continue;
                                            }
                                            TrnsDispatchMulti oDetail = new TrnsDispatchMulti();
                                            oDetail.SBRNum = One.SBRNum;
                                            ValidSaleOrder.Add(One.SBRNum);
                                            oDetail.CardCode = oOld.CustomerCode;
                                            oDetail.ItemCode = oOld.ItemCode;
                                            oDetail.DocNum = Convert.ToInt32(txtDocNo.Text);
                                            oDetail.BalanceQty = One.Balance;
                                            decimal? CurOpenQty = 0;
                                            oDBPrivate.GetSumOpenValueSOValidation(One.SBRNum, oOld.CustomerCode, oOld.ItemCode, oOld.DocNum, ref CurOpenQty);
                                            decimal LiveBalanceQty = 0;
                                            LiveBalanceQty = CurOpenQty.GetValueOrDefault();
                                            Program.oErrMgn.LogEntry(Program.ANV, "SBRNum qty: " + One.SBRNum.ToString());
                                            Program.oErrMgn.LogEntry(Program.ANV, "Live qty: " + LiveBalanceQty.ToString());
                                            if (CurOrderQty >= LiveBalanceQty)
                                            {
                                                oDetail.OrderQty = LiveBalanceQty;
                                                CurOrderQty -= LiveBalanceQty;
                                                One.Order = Convert.ToDecimal(oDetail.OrderQty);
                                                Program.oErrMgn.LogEntry(Program.ANV, "so: " + One.SBRNum + " qty: " + oDetail.OrderQty.ToString());
                                            }
                                            else if (CurOrderQty < LiveBalanceQty)
                                            {
                                                oDetail.OrderQty = CurOrderQty;
                                                One.Order = Convert.ToDecimal(oDetail.OrderQty);
                                                Program.oErrMgn.LogEntry(Program.ANV, "so: " + One.SBRNum + " qty: " + oDetail.OrderQty.ToString());
                                                CurOrderQty = 0;
                                            }

                                            oDBPrivate.TrnsDispatchMulti.InsertOnSubmit(oDetail);
                                        }
                                        oOld.SBRNum = string.Join(",", ValidSaleOrder);
                                        if (CurOrderQty > 0)
                                        {
                                            Program.WarningMsg("Order quantity exceeds available open quantity.");
                                            Program.oErrMgn.LogEntry(Program.ANV, "unsettle qty: " + CurOrderQty.ToString());
                                            return false;
                                        }
                                         foreach (var One in SaleOrderList)
                                         {
                                             if (One.Order == 0)
                                             {
                                                 continue;
                                             }
                                             decimal? finaldoc1 = 0;
                                             decimal AvailableValue = 0;
                                             oDBPrivate.GetSumOpenValueSOValidation(One.SBRNum, oOld.CustomerCode, oOld.ItemCode, oOld.DocNum, ref finaldoc1);
                                             AvailableValue = finaldoc1.GetValueOrDefault();
                                             if (One.Order > AvailableValue)
                                             {
                                                 Program.WarningMsg("Order quantity exceeds available open quantity. ==> " + AvailableValue.ToString());
                                                 Program.oErrMgn.LogEntry(Program.ANV, "order qty: " + One.Order.ToString());
                                                 Program.oErrMgn.LogEntry(Program.ANV, "available qty: " + AvailableValue.ToString());
                                                 return false;
                                             }
                                         }
                                        if (flgCustChange)
                            {
                                oDBPrivate.RemoveAllDetailsDispatch(oOld.DocNum);
                            }
                            else if (flgItemChange)
                            {
                                oDBPrivate.RemoveAllDetailsDispatch(oOld.DocNum);
                            }
                            else if (flgReselect)
                            {
                                oDBPrivate.RemoveAllDetailsDispatch(oOld.DocNum);
                            }
                            else
                            {
                                oDBPrivate.RemoveAllDetailsDispatch(oOld.DocNum);
                            }
                        }

                    }
                    #endregion

                    #region Add Document
                    else
                    {
                        oDoc.DocNum = Convert.ToInt32(txtDocNo.Text);
                        //string[] FwTime = (txt1WeightDate.Text).Split(' ');
                        //string a = FwTime[1];
                        oDoc.Flg1Rpt = false;
                        oDoc.FlgMulti = flgMultiSales;
                        oDoc.FDocDate = Convert.ToDateTime(txtCurrDate.Text);
                        string CurrTime = txtCurrTime.Text;
                        oDoc.SBRNum = txtSBRNum.Text;
                        if (!string.IsNullOrEmpty(txtShift.Text))
                        {
                            oDoc.FShift = Convert.ToInt32(shiftid);
                        }
                        else
                        {
                            Program.ExceptionMsg("Shift can not be Empty");
                            return false;
                        }
                        oDoc.FTime = txt1WeightTime.Text;
                        if (!string.IsNullOrEmpty(txtSBRDate.Text))
                        {
                            oDoc.SBRDate = Convert.ToDateTime(txtSBRDate.Text);
                        }
                        else
                        {
                            Program.WarningMsg("Sbr date is not selected");
                            return false;
                        }
                        oDoc.CustomerCode = txtCustomerCode.Text;
                        oDoc.CustomerName = txtCustomerName.Text;
                        //Program.FormItemCode= txtItemCod.Text;
                        //oDoc.ItemCode = Program.FormItemCode;
                        oDoc.ItemCode = txtItemCod.Text;
                        oDoc.DaySeries = Convert.ToInt32(txtDaySeries.Text);
                        oDoc.ItemName = txtItemNam.Text;
                        oDoc.ItemGroupName = txtItemGroupName.Text;
                        oDoc.ItemGroupCode = ItemGroupCode;
                        if (string.IsNullOrEmpty(txtOrderQuantity.Text) || Convert.ToDecimal(txtOrderQuantity.Text) == 0)
                        {
                            Program.WarningMsg("Order Quantity cannot b empty or 0");
                            return false;
                        }
                        else
                        {
                            if (Convert.ToDecimal(txtOrderQuantity.Text) > Convert.ToDecimal(txtBalanceQuantity.Text))
                            {
                                Program.WarningMsg("Order Quantity cannot b greater than Balance quantity");
                                return false;
                            }
                            else
                            {
                                oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                            }

                        }

                        if (!string.IsNullOrEmpty(txtBalanceQuantity.Text))
                        {
                            oDoc.BalanceQuantity = Convert.ToDecimal(txtBalanceQuantity.Text);
                        }
                        else
                        {
                            oDoc.BalanceQuantity = 0;
                        }

                        oDoc.PackerId = Convert.ToInt32(cmbPacker.SelectedValue);
                        oDoc.VehicleNum = txtVehicleNum.Text;
                        oDoc.DriverCNIC = txtDriverCNIC.Text;
                        oDoc.DriverName = txtDriverName.Text;
                        oDoc.DriverDocument = txtCNICPath.Text;
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

                        if (!string.IsNullOrEmpty(txt1WeightDate.Text))
                        {
                            oDoc.FWeighmentDate = Convert.ToDateTime(txt1WeightDate.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("FirstWeighment Date can not be Empty");
                            return false;
                        }

                        oDoc.FWeighmentTime = txt1WeightTime.Text;

                        if (!string.IsNullOrEmpty(txt1WeightTon.Text))
                        {
                            oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                        }
                        else
                        {
                            Program.ExceptionMsg("FirstWeighment Ton can not be Empty");
                            return false;
                        }

                        oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);// 
                        oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                        oDoc.TransportName = TxtTransportName.Text;

                        oDoc.FlgSecondWeight = false;
                        // oDoc.SDocDate = DateTime.Now;


                        if (!string.IsNullOrEmpty(txtShift.Text))
                        {
                            oDoc.SShift = Convert.ToInt32(shiftid);
                        }
                        else
                        {
                            Program.ExceptionMsg("Second Shift can not be Empty");
                            return false;
                        }

                        //oDoc.STime = a;

                        //if (!string.IsNullOrEmpty(txt2WeightDate.Text))
                        //{
                        //    string[] SwTime = txt2WeightDate.Text.Split(' ');
                        //    string b = SwTime[1];
                        //    oDoc.SWeighmentTime = b;
                        //}
                        //else
                        //{
                        //    txt2WeightDate = null;
                        //}


                        //if (!string.IsNullOrEmpty(txt2WeightKG.Text))
                        //{
                        //    oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("SecondWeighment KG can not be Empty");
                        //    return false;
                        //}

                        //if (!string.IsNullOrEmpty(txt2WeightTon.Text))
                        //{
                        //    oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("SecondWeighment Ton can not be Empty");
                        //    return false;
                        //}

                        //if (!string.IsNullOrEmpty(txtNetWeightKG.Text))
                        //{
                        //    oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("SecondWeighment KG can not be Empty");
                        //    return false;
                        //}

                        //if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
                        //{
                        //    oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                        //}
                        //else
                        //{
                        //    Program.ExceptionMsg("SecondWeighment Ton can not be Empty");
                        //    return false;
                        //}

                        if (!string.IsNullOrEmpty(txtBalanceQuantity.Text))
                        {
                            oDoc.BalanceQuantity = Convert.ToDecimal(txtBalanceQuantity.Text);
                        }
                        else
                        {
                            oDoc.BalanceQuantity = 0;
                        }
                        //oDoc.DispatchQuantity = 0;

                        //if (string.IsNullOrEmpty(txtOrderQuantity.Text) || Convert.ToDecimal(txtOrderQuantity.Text) == 0)
                        //{
                        //    Program.WarningMsg("Order Quantity cannot b empty or 0");
                        //    return false;
                        //}
                        //else
                        //{
                        //    oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                        //}
                        oDBPrivate.TrnsDispatch.InsertOnSubmit(oDoc);
                        if (flgMultiSales)
                        {
                            decimal CurOrderQty = oDoc.OrderQuantity.GetValueOrDefault();
                            List<string> ValidSaleOrder = new List<string>();
                            foreach (var One in SaleOrderList)
                            {
                                if (CurOrderQty == 0) break;
                                TrnsDispatchMulti oDetail = new TrnsDispatchMulti();
                                oDetail.SBRNum = One.SBRNum;
                                ValidSaleOrder.Add(One.SBRNum);
                                oDetail.CardCode = oDoc.CustomerCode;
                                oDetail.ItemCode = oDoc.ItemCode;
                                oDetail.DocNum = Convert.ToInt32(txtDocNo.Text);
                                oDetail.BalanceQty = One.Balance;
                                if (CurOrderQty >= One.Balance)
                                {
                                    oDetail.OrderQty = One.Balance;
                                    CurOrderQty -= One.Balance;
                                }
                                else if (CurOrderQty < One.Balance)
                                {
                                    oDetail.OrderQty = CurOrderQty;
                                    CurOrderQty = 0;
                                }
                                oDBPrivate.TrnsDispatchMulti.InsertOnSubmit(oDetail);
                            }
                            oDoc.SBRNum = string.Join(",", ValidSaleOrder);
                            Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oDoc.DocNum.ToString() + " sbrlist: " + string.Join(",", ValidSaleOrder));
                            Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oDoc.DocNum.ToString() + " soorderlist: " + SaleOrderList.Count.ToString());

                        }
                        Program.SuccesesMsg("Record Added Successfully");
                    }

                    #endregion
                    oDBPrivate.SubmitChanges();
                }
            }
            catch (Exception Ex)
            {
                retValue = false;
                Program.oErrMgn.LogException(Program.ANV, Ex);
                Program.ExceptionMsg("Values are Missing or Incorrect.");
            }

            return retValue;
        }*/

        private void HandleDialogControlSaleOrder()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oSaleOrderMD"; //osaleorder multiple dispatch
                //oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtSBRNum.Text;
                oDlg.Text = "Order Selector";
                // oDlg.flgAll = chkAllItem.Checked;

                /**added by talha**/
                oDlg.CustomerCode = txtCustomerCode.Text;
                /****/
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtSBRDate.Text = oDlg.DocDate;
                    txtSBRNum.Text = oDlg.DocNum;
                    Program.DocNum = txtSBRNum.Text;
                    txtCustomerCode.Text = oDlg.CustomerCode;
                    txtCustomerName.Text = oDlg.CustomerName;
                    txtBrandPath.Text = oDlg.Image;
                }

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
                Program.FormDocNum = Convert.ToInt32(txtDocNo.Text);
                Program.FormItemCode = txtItemCod.Text;
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oBulkSealer";
                Program.FormName = this.Name;
                //oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtSBRNum.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                //if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                //{
                //    txtBalanceQuantity.Text = oDlg.SelectedObjectID;
                //    txtSBRDate.Text = oDlg.SelectedObjectIDComplex;
                //}

            }
            catch (Exception Ex)
            {
                //Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlSaleOrderItem()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oItem";
                oDlg.Text = "Item Selector";
                //oDlg.DocType = cmbSourceDoc.SelectedItem.Text;
                oDlg.SourceDocNum = txtSBRNum.Text;
                // oDlg.flgAll = chkAllItem.Checked;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    // txtSBRDate.Text = oDlg.DocDate;
                    //txtOrderQuantity.Text = oDlg.Quantity;
                    // txtSBRNum.Text = oDlg.DocNum;
                    txtItemCod.Text = oDlg.ItemCode;
                    txtItemNam.Text = oDlg.Dscription;
                    txtItemGroupName.Text = oDlg.ItmsGrpNam;
                    ItemGroupCode = oDlg.ItmsGrpCod;
                    //ItemGroupCode = oDlg.ItmsGrpCod;
                    txtBalanceQuantity.Text = oDlg.Balance;
                    //txtDriverName.Text = oDlg.U_Driver;
                    //txtVehicleNum.Text = oDlg.U_VehcleNo;
                    //txtDriverCNIC.Text = oDlg.DriverCnic;

                    txtBrandPath.Text = oDlg.Image;
                }

            }
            catch (Exception Ex)
            {
                //Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlCustomer()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oBP";
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(txtCustomerCode.Text))
                    {
                        if (txtCustomerCode.Text != oDlg.CustomerCode)
                        {
                            flgCustChange = true;
                        }
                    }
                    txtCustomerCode.Text = oDlg.CustomerCode;
                    txtCustomerName.Text = oDlg.CustomerName;

                    flgMultiSales = true;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlSaleOrderItemNewLogic()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "oItemM";
                oDlg.Text = "Item Selector";
                oDlg.oItemList = ItemList;
                oDlg.CustomerCode = txtCustomerCode.Text;
                oDlg.flgMultiSelect = true;
                oDlg.SourceDocNum = txtSBRNum.Text;
                //oDlg.oDataItem.Add(ItemList);
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(txtItemCod.Text))
                    {
                        if (txtItemCod.Text != oDlg.ItemCode)
                        {
                            flgItemChange = true;
                        }
                    }

                    /*txtSBRDate.Text = oDlg.DocDate;*/
                    //txtOrderQuantity.Text = oDlg.Quantity;
                    /*txtSBRNum.Text = oDlg.DocNum;*/
                    //txtDriverCNIC.Text = oDlg.DriverCnic;
                    //txtDriverName.Text = oDlg.U_Driver;
                    txtItemCod.Text = oDlg.ItemCode;
                    txtItemNam.Text = oDlg.Dscription;
                    txtItemGroupName.Text = oDlg.ItmsGrpNam;
                    txtBalanceQuantity.Text = oDlg.Balance;
                    ItemGroupCode = oDlg.ItmsGrpCod;
                    //ItemGroupCode = oDlg.ItmsGrpCod;
                    //txtVehicleNum.Text = oDlg.U_VehcleNo;
                    txtBrandPath.Text = oDlg.Image;
                }

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void HandleDialogControlSaleOrderMulti()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();

                //oDlg.OpenFor = "oSaleOrderM";
                oDlg.OpenFor = "oSaleOrderMD";
                oDlg.Text = "Order Selector";
                oDlg.CustomerCode = txtCustomerCode.Text;
                //oDlg.ItemCode = txtItemCod.Text;
                oDlg.flgMultiSelect = flgMultiSales;
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    /*SaleOrderList.Clear();*/
                    txtSBRDate.Text = oDlg.DocDate;
                    txtSBRNum.Text = oDlg.DocNum;
                    Program.DocNum = txtSBRNum.Text;
                    //SaleOrderList = oDlg.oItems;
                    //LoadedQty = oDlg.oQuantity;
                    //BalanceQty = oDlg.oBalance;
                    /*decimal loadedqty = 0, balanceqty = 0;
                    List<string> sbrnum = new List<string>();
                    if (!flgCustChange)
                    {
                        if (!flgItemChange)
                        {
                            foreach (var two in FirstWeightSOList)
                            {
                                loadedqty += two.Order;
                                balanceqty += two.Balance;
                                sbrnum.Add(two.SBRNum);
                                SaleOrderDataM onew = new SaleOrderDataM();
                                onew.SBRNum = two.SBRNum;
                                onew.Balance = two.Balance;
                                onew.Order = two.Order;
                                SaleOrderList.Add(onew);
                                flgReselect = true;
                            }
                        }
                    }*/
                    /*foreach (var one in oDlg.oData)
                    {
                        if (sbrnum.Contains(one.SBRNum)) continue;
                        loadedqty += one.Order;
                        balanceqty += one.Balance;
                        sbrnum.Add(one.SBRNum);
                        SaleOrderDataM onew = new SaleOrderDataM();
                        onew.SBRNum = one.SBRNum;
                        onew.Balance = one.Balance;
                        onew.Order = one.Order;
                        SaleOrderList.Add(onew);
                        Program.DocNum = one.SBRNum;

                    }
                    txtSBRNum.Text = string.Join(",", sbrnum);*/
                    //foreach(var One in LoadedQty)
                    //{
                    //    loadedqty += Convert.ToDecimal(One);
                    //}
                    //foreach(var One in BalanceQty)
                    //{
                    //    balanceqty += Convert.ToDecimal(One);
                    //}
                    //txtOrderQuantity.Text = loadedqty.ToString();
                    /*txtBalanceQuantity.Text = balanceqty.ToString();*/
                }

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
                Int64 value = Convert.ToInt64((from a in oDB.TrnsDispatchMultiHeader where a.DocNum == Convert.ToInt32(ActiveSeries) select a.DocNum).Max());
                if (value != 0)
                {
                    retValue = ActiveSeries + "-" + (++value).ToString();
                }
                else
                {
                    retValue = ActiveSeries + "-" + "1";
                }
            }
            catch (Exception Ex)
            {
                retValue = ActiveSeries + "-" + "1";
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            return retValue;
        }

        private string GetDocNumber()
        {
            string retValue = "";
            try
            {
                int value = Convert.ToInt32((from a in oDB.TrnsDispatchMultiHeader select a.DocNum).Max());

                retValue = (value + 1).ToString();

            }
            catch (Exception Ex)
            {
                retValue = "-1";
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            return retValue;
        }

        private void ConnectPort()
        {
            if (comportDispatch.IsOpen) comportDispatch.Close();
            //  bool tmrc = tmrCamFront.Enabled;
            try
            {
                // Set the port's settings
                //tmrCamFront.Enabled = false;
                comportDispatch.BaudRate = Convert.ToInt32(BaudRate);
                comportDispatch.DataBits = Convert.ToInt32(DataBits);
                comportDispatch.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBit);
                comportDispatch.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                comportDispatch.PortName = ComPort;
                comportDispatch.ReadTimeout = 5000;

                // Program.WarningMsg("Trying to connect.");
                comportDispatch.Open();
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
                IEnumerable<TrnsDispatchMultiHeader> getData = from a in oDB.TrnsDispatchMultiHeader where a.FlgSecondWeight == false select a;

                foreach (TrnsDispatchMultiHeader item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["Wmnt#"] = item.DocNum;
                    dtrow["Vehicle#"] = item.VehicleNum;
                    //dtrow["SBRNum"] = item.SBRNum;
                    //dtrow["ItemName"] = item.ItemName;
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

            }
        }

        /* private bool CheckLoadingQty()
         {
             Boolean retValue = true;
             if (!string.IsNullOrEmpty(txtNetWeightKG.Text))
             {
                 if (grdWmntDetails.Rows.Count > 0)
                 {
                     totalLoadingQty = 0;
                     for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                     {
                         totalLoadingQty += Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                     }
                     if (totalLoadingQty > Convert.ToDecimal(txtNetWeightKG.Text))
                     {
                         Program.WarningMsg("Loading quantity cannot b greater than Net weight.");
                         retValue = false;
                     }
                 }
             }
             return retValue;

         }*/

        private void LoadMultipleDispatchGrid(int DocNum)
        {
            try
            {
                dtM.Clear();
                CreateMultipleDispatchDt();
                IEnumerable<TrnsDispatchMultiDetail> getData = from a in oDB.TrnsDispatchMultiDetail where a.DocNum == DocNum select a;
                double LoadingQty = 0;
                foreach (TrnsDispatchMultiDetail item in getData)
                {
                    DataRow dtrow = dtM.NewRow();
                    dtrow["Customer Code"] = item.CustomerCode;
                    dtrow["Customer"] = item.CustomerName;
                    dtrow["SBR#"] = item.SBRNum;
                    dtrow["SBR Date"] = item.SBRDate;
                    dtrow["Item Group Code"] = item.ItemGroupCode;
                    dtrow["Item Group Name"] = item.ItemGroupName;
                    dtrow["Item Code"] = item.ItemCode;
                    dtrow["Item Name"] = item.ItemName;
                    dtrow["Loading Qty"] = item.OrderQuantity;
                    dtrow["Balance Qty"] = item.BalanceQuantity;
                    LoadingQty += Convert.ToDouble(item.OrderQuantity);
                    dtM.Rows.Add(dtrow);
                }
                txtTotalLoadingQty.Text = LoadingQty.ToString();
                grdWmntDetails.DataSource = dtM;
                grdWmntDetails.EnableFiltering = true;
            }
            catch (Exception ex)
            {

            }
        }

        public int daySeries()
        {
            int dSeries = 0;
            try
            {
                /*if (!string.IsNullOrEmpty(txtItemGroupName.Text))*/
                if (!string.IsNullOrEmpty(txtDocNo.Text))
                {
                    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    /*int fSeries = (from a in oDB.TrnsDispatch
                                   where a.ItemGroupName == txtItemGroupName.Text && Convert.ToString(a.FDocDate) == todayDate
                                   select a).Count();*/
                    int fSeries = (from a in oDB.TrnsDispatchMultiHeader
                                   where Convert.ToString(a.FDocDate) == todayDate
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
            decimal exceed = 0, Tolerence = 0;
            if (chkAllowTolerance.Checked)
            {
                if (flgChkStatus == true)
                {
                    Tolerence = Convert.ToDecimal(txtToleranceLimit.Text);
                }
                else if (flgChkStatus == false)
                {
                    Program.WarningMsg("You cannot authorized user to allow extra tolerance.");
                    return false;
                }
            }
            else
            {
                Tolerence = Convert.ToDecimal(oDB.MstTolerance.Where(x => x.TName == "Company").FirstOrDefault().TRate);
                //Tolerence = Convert.ToDecimal(oDB.MstTolerance.FirstOrDefault().TRate);
            }
            if (Tolerence > 0)
            {
                val1 = (Convert.ToDecimal(txtTotalLoadingQty.Text) * (Tolerence / 100)) + Convert.ToDecimal(txtTotalLoadingQty.Text);
                val2 = Convert.ToDecimal(txtTotalLoadingQty.Text) - (Convert.ToDecimal(txtTotalLoadingQty.Text) * (Tolerence / 100));

                if (val1 > Convert.ToDecimal(txtNetWeightTon.Text))
                {
                    if (Convert.ToDecimal(txtNetWeightTon.Text) > val2)
                    {
                        return true;
                    }
                    else
                    {
                        exceed = val2 - Convert.ToDecimal(txtNetWeightTon.Text);
                        Program.WarningMsg("tolerence decreases" + exceed + " Ton");
                        return false;
                    }
                }
                else
                {
                    exceed = Convert.ToDecimal(txtNetWeightTon.Text) - val1;
                    Program.WarningMsg("tolerence increases" + exceed + " Ton");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /*public bool ChkTolerenceOld()
        {
            decimal exceed = 0, Tolerence;
            string itemGrp = txtItemGroupName.Text;
            if (chkAllowTolerance.Checked)
            {

                Tolerence = Convert.ToDecimal(txtToleranceLimit.Text);
            }
            else
            {
                //Tolerence = Convert.ToDecimal(oDB.MstTolerance.Where(x => x.TName == itemGrp).FirstOrDefault().TRate);
                Tolerence = Convert.ToDecimal(oDB.MstTolerance.FirstOrDefault().TRate);
            }
            if (Tolerence > 0)
            {
                val1 = (Convert.ToDecimal(txtTotalLoadingQty.Text) * (Tolerence / 100)) + (Convert.ToDecimal(txtTotalLoadingQty.Text));
                val2 = ((Convert.ToDecimal(txtTotalLoadingQty.Text) - Convert.ToDecimal(txtTotalLoadingQty.Text) * (Tolerence / 100)));

                if (val1 > Convert.ToDecimal(txtNetWeightTon.Text))
                {
                    if (Convert.ToDecimal(txtNetWeightTon.Text) > val2)
                    {

                        return true;

                    }
                    else
                    {
                        exceed = val2 - Convert.ToDecimal(txtNetWeightTon.Text);
                        Program.WarningMsg("tolerence decreases" + exceed + " Ton");
                        return false;
                    }
                }
                else
                {
                    exceed = Convert.ToDecimal(txtNetWeightTon.Text) - val1;
                    Program.WarningMsg("tolerence increases" + exceed + " Ton");
                    return false;
                }
            }
            else
            if (!itemGrp.ToUpper().Contains("BAG"))
            {
                if (Convert.ToDecimal(txtNetWeightTon.Text) <= Convert.ToDecimal(txtBalanceQuantity.Text))
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
        }*/

        public bool AddRecordNew()
        {
            try
            {
                using (dbFFS oDBAdd = new dbFFS(Program.ConStrApp))
                {
                    TrnsDispatchMultiHeader oDoc = new TrnsDispatchMultiHeader();
                    //oDoc.DocNum = Convert.ToInt32(txtDocNo.Text);
                    int LatestDocNum = ReturnDocNum();
                    oDoc.DocNum = LatestDocNum;
                    oDoc.Flg1Rpt = false;
                    oDoc.FlgMulti = flgMultiSales;
                    oDoc.FDocDate = DateTime.Now;
                    oDoc.FTime = DateTime.Now.ToShortTimeString();
                    oDoc.FShift = Convert.ToInt32(shiftid);
                    /*oDoc.SBRNum = txtSBRNum.Text;
                    oDoc.SBRDate = Convert.ToDateTime(txtSBRDate.Text);
                    oDoc.CustomerCode = txtCustomerCode.Text;
                    oDoc.CustomerName = txtCustomerName.Text;
                    oDoc.ItemCode = txtItemCod.Text;
                    oDoc.ItemName = txtItemNam.Text;
                    oDoc.ItemGroupCode = ItemGroupCode;
                    oDoc.ItemGroupName = txtItemGroupName.Text;*/
                    oDoc.DaySeries = Convert.ToInt32(txtDaySeries.Text);
                    /*oDoc.OrderQuantity = Convert.ToDecimal(txtOrderQuantity.Text);
                    oDoc.BalanceQuantity = Convert.ToDecimal(txtBalanceQuantity.Text);*/
                    if (!string.IsNullOrEmpty(cmbPacker.Text))
                    {
                        oDoc.PackerId = Convert.ToInt32(cmbPacker.SelectedValue);
                    }
                    oDoc.VehicleNum = txtVehicleNum.Text;
                    oDoc.DriverCNIC = txtDriverCNIC.Text;
                    oDoc.DriverName = txtDriverName.Text;
                    oDoc.DriverDocument = txtCNICPath.Text;
                    oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
                    oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
                    oDoc.FWeighmentDate = DateTime.Now;
                    oDoc.FWeighmentTime = DateTime.Now.ToShortTimeString();
                    if (!string.IsNullOrEmpty(cmbTransportType.Text))
                    {
                        oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(cmbTransportCode.Text))
                    {
                        oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                    }
                    else
                    {
                        oDoc.TransportCode = "";
                    }
                    oDoc.TransportName = TxtTransportName.Text;
                    oDoc.FlgSecondWeight = false;

                    /*if (flgMultiSales)
                    {
                        decimal CurOrderQty = oDoc.OrderQuantity.GetValueOrDefault();
                        List<string> ValidSaleOrder = new List<string>();
                        foreach (var One in SaleOrderList)
                        {
                            if (CurOrderQty == 0) break;
                            TrnsDispatchMulti oDetail = new TrnsDispatchMulti();
                            oDetail.SBRNum = One.SBRNum;
                            ValidSaleOrder.Add(One.SBRNum);
                            oDetail.CardCode = oDoc.CustomerCode;
                            oDetail.ItemCode = oDoc.ItemCode;
                            oDetail.DocNum = Convert.ToInt32(txtDocNo.Text);
                            oDetail.BalanceQty = One.Balance;
                            if (CurOrderQty >= One.Balance)
                            {
                                oDetail.OrderQty = One.Balance;
                                CurOrderQty -= One.Balance;
                            }
                            else if (CurOrderQty < One.Balance)
                            {
                                oDetail.OrderQty = CurOrderQty;
                                CurOrderQty = 0;
                            }
                            oDB.TrnsDispatchMulti.InsertOnSubmit(oDetail);
                        }
                        oDoc.SBRNum = string.Join(",", ValidSaleOrder);
                        Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oDoc.DocNum.ToString() + " sbrlist: " + string.Join(",", ValidSaleOrder));
                        Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oDoc.DocNum.ToString() + " soorderlist: " + SaleOrderList.Count.ToString());
                    }*/

                    /**********************************/
                   
                    for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                    {
                        TrnsDispatchMultiDetail oDetail = new TrnsDispatchMultiDetail();
                        //oDetail.DocNum = Convert.ToInt32(txtDocNo.Text);
                        oDetail.DocNum = LatestDocNum;
                        oDetail.SBRNum = grdWmntDetails.Rows[i].Cells["SBR#"].Value.ToString();
                        oDetail.SBRDate = Convert.ToDateTime(grdWmntDetails.Rows[i].Cells["SBR Date"].Value.ToString());
                        oDetail.CustomerCode = grdWmntDetails.Rows[i].Cells["Customer Code"].Value.ToString();
                        oDetail.CustomerName = grdWmntDetails.Rows[i].Cells["Customer"].Value.ToString();
                        oDetail.ItemCode = grdWmntDetails.Rows[i].Cells["Item Code"].Value.ToString();
                        oDetail.ItemName = grdWmntDetails.Rows[i].Cells["Item Name"].Value.ToString();
                        oDetail.ItemGroupCode = grdWmntDetails.Rows[i].Cells["Item Group Code"].Value.ToString();
                        oDetail.ItemGroupName = grdWmntDetails.Rows[i].Cells["Item Group Name"].Value.ToString();
                        oDetail.OrderQuantity = Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                        oDetail.BalanceQuantity = Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Balance Qty"].Value.ToString());
                        oDBAdd.TrnsDispatchMultiDetail.InsertOnSubmit(oDetail);
                    }
                    /**************************************************/

                    oDoc.FCreateBy = Program.oCurrentUser.UserCode + " @ " + Program.ANV;
                    oDoc.FCreateDate = DateTime.Now;
                    oDoc.FUpdateBy = Program.oCurrentUser.UserCode + " @ " + Program.ANV;
                    oDoc.FUpdateDate = DateTime.Now;
                    oDBAdd.TrnsDispatchMultiHeader.InsertOnSubmit(oDoc);
                    //if(!flgcehckexcep)
                    //{
                    //    flgcehckexcep = true;
                    //    throw new Exception("kaam k bech mai exception.");
                    //}
                    oDBAdd.SubmitChanges();
                    Program.SuccesesMsg("Record Successfully Added.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                Program.ExceptionMsg("Something went wrong, Contact Abacus Support.");
                return false;
            }
        }

        public bool UpdateRecordNew()
        {
            try
            {
                TrnsDispatchMultiHeader oDoc = (from a in oDB.TrnsDispatchMultiHeader
                                                where a.DocNum.ToString() == txtDocNo.Text
                                                select a).FirstOrDefault();

                oDoc.Flg2Rpt = false;
                oDoc.FlgWBrpt = false;
                oDoc.SDocDate = DateTime.Now;
                oDoc.STime = DateTime.Now.ToShortTimeString();
                oDoc.SShift = Convert.ToInt32(shiftid);
                /*oDoc.SBRNum = txtSBRNum.Text;
                oDoc.SBRDate = Convert.ToDateTime(txtSBRDate.Text);
                oDoc.CustomerCode = txtCustomerCode.Text;
                oDoc.CustomerName = txtCustomerName.Text;
                oDoc.ItemCode = txtItemCod.Text;
                oDoc.ItemName = txtItemNam.Text;
                oDoc.ItemGroupCode = ItemGroupCode;
                oDoc.ItemGroupName = txtItemGroupName.Text;*/
                /*if (txtItemNam.Text.ToLower().Contains("bulk"))
                {
                    oDoc.OrderQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
                }*/
                if (!string.IsNullOrEmpty(cmbPacker.Text))
                {
                    oDoc.PackerId = Convert.ToInt32(cmbPacker.SelectedValue);
                }
                /*oDoc.BalanceQuantity = Convert.ToDecimal(txtBalanceQuantity.Text);*/
                oDoc.VehicleNum = txtVehicleNum.Text;
                oDoc.DriverCNIC = txtDriverCNIC.Text;
                oDoc.DriverName = txtDriverName.Text;
                oDoc.DriverDocument = txtCNICPath.Text;
                if (!string.IsNullOrEmpty(cmbTransportType.Text))
                {
                    oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
                }
                if (!string.IsNullOrEmpty(cmbTransportCode.Text))
                {
                    oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
                }
                else
                {
                    oDoc.TransportCode = "";
                }
                oDoc.TransportName = TxtTransportName.Text;
                oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
                oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
                oDoc.SWeighmentDate = DateTime.Now;
                oDoc.SWeighmentTime = DateTime.Now.ToShortTimeString();
                oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
                oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
                /*oDoc.DispatchQuantity = Convert.ToDecimal(txtNetWeightTon.Text);*/
                oDoc.FlgSecondWeight = true;

                /*if (flgMultiSales)
                {
                    decimal CurOrderQty = oDoc.OrderQuantity.GetValueOrDefault();
                    decimal valCurOrderQty = CurOrderQty;
                    if (!flgCustChange)
                    {
                        if (!flgItemChange)
                        {
                            if (!flgReselect)
                            {
                                Program.oErrMgn.LogEntry(Program.ANV, "1st weigh queue count: " + FirstWeightSOList.Count.ToString());
                                string solist = string.Empty;
                                foreach (var Previous in FirstWeightSOList)
                                {
                                    var oCheck = (from a in SaleOrderList
                                                  where a.SBRNum == Previous.SBRNum
                                                  select a).Count();
                                    if (oCheck == 0)
                                    {
                                        SaleOrderData onew = new SaleOrderData();
                                        onew.SBRNum = Previous.SBRNum;
                                        onew.Balance = Previous.Balance;
                                        onew.Order = Previous.Order;
                                        SaleOrderList.Add(onew);
                                        solist += " " + Previous.SBRNum;
                                    }
                                }
                                Program.oErrMgn.LogEntry(Program.ANV, "1st weigh sbr list: " + solist);
                            }
                        }
                    }
                    Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oDoc.DocNum.ToString() + " queue count: " + SaleOrderList.Count.ToString());
                    Program.oErrMgn.LogEntry(Program.ANV, "order qty: " + CurOrderQty.ToString());
                    List<string> ValidSaleOrder = new List<string>();
                    foreach (var One in SaleOrderList)
                    {
                        if (CurOrderQty == 0)
                        {
                            One.Order = 0;
                            continue;
                        }
                        TrnsDispatchMulti oDetail = new TrnsDispatchMulti();
                        oDetail.SBRNum = One.SBRNum;
                        ValidSaleOrder.Add(One.SBRNum);
                        oDetail.CardCode = oDoc.CustomerCode;
                        oDetail.ItemCode = oDoc.ItemCode;
                        oDetail.DocNum = Convert.ToInt32(txtDocNo.Text);
                        oDetail.BalanceQty = One.Balance;
                        decimal? CurOpenQty = 0;
                        oDB.GetSumOpenValueSOIteration(One.SBRNum, oDoc.CustomerCode, oDoc.ItemCode, oDoc.DocNum, ref CurOpenQty);
                        decimal LiveBalanceQty = 0;
                        LiveBalanceQty = CurOpenQty.GetValueOrDefault();
                        Program.oErrMgn.LogEntry(Program.ANV, "SBRNum qty: " + One.SBRNum.ToString());
                        Program.oErrMgn.LogEntry(Program.ANV, "Live qty: " + LiveBalanceQty.ToString());
                        if (CurOrderQty >= LiveBalanceQty)
                        {
                            oDetail.OrderQty = LiveBalanceQty;
                            CurOrderQty -= LiveBalanceQty;
                            One.Order = Convert.ToDecimal(oDetail.OrderQty);
                            Program.oErrMgn.LogEntry(Program.ANV, "so: " + One.SBRNum + " qty: " + oDetail.OrderQty.ToString());
                        }
                        else if (CurOrderQty < LiveBalanceQty)
                        {
                            oDetail.OrderQty = CurOrderQty;
                            One.Order = Convert.ToDecimal(oDetail.OrderQty);
                            Program.oErrMgn.LogEntry(Program.ANV, "so: " + One.SBRNum + " qty: " + oDetail.OrderQty.ToString());
                            CurOrderQty = 0;
                        }

                        oDB.TrnsDispatchMulti.InsertOnSubmit(oDetail);
                    }
                    oDoc.SBRNum = string.Join(",", ValidSaleOrder);
                    if (CurOrderQty > 0)
                    {
                        Program.WarningMsg("Order quantity exceeds available open quantity.");
                        Program.oErrMgn.LogEntry(Program.ANV, "unsettle qty: " + CurOrderQty.ToString());
                        return false;
                    }
                    foreach (var One in SaleOrderList)
                    {
                        if (One.Order == 0)
                        {
                            continue;
                        }
                        decimal? finaldoc1 = 0;
                        decimal AvailableValue = 0;
                        oDB.GetSumOpenValueSOValidation(One.SBRNum, oDoc.CustomerCode, oDoc.ItemCode, oDoc.DocNum, ref finaldoc1);
                        AvailableValue = finaldoc1.GetValueOrDefault();
                        if (One.Order > AvailableValue)
                        {
                            Program.WarningMsg("Order quantity exceeds available open quantity. ==> " + AvailableValue.ToString());
                            Program.oErrMgn.LogEntry(Program.ANV, "order qty: " + One.Order.ToString());
                            Program.oErrMgn.LogEntry(Program.ANV, "available qty: " + AvailableValue.ToString());
                            return false;
                        }
                    }
                    if (flgCustChange)
                    {
                        oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
                    }
                    else if (flgItemChange)
                    {
                        oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
                    }
                    else if (flgReselect)
                    {
                        oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
                    }
                    else
                    {
                        oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
                    }
                }*/

                //TrnsDispatchMultiDetail oDetail = (from a in oDB.TrnsDispatchMultiDetail
                //                                   where a.DocNum.ToString() == txtDocNo.Text
                //                                   select a).FirstOrDefault();
                //for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                //{
                //    oDetail.OrderQuantity = Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                //}


                for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                {

                    TrnsDispatchMultiDetail oDetail = new TrnsDispatchMultiDetail();

                    oDetail = (from a in oDB.TrnsDispatchMultiDetail
                               where a.DocNum.ToString() == txtDocNo.Text && a.CustomerCode == grdWmntDetails.Rows[i].Cells["Customer Code"].Value.ToString()
                               && a.SBRNum == grdWmntDetails.Rows[i].Cells["SBR#"].Value.ToString() && a.ItemCode == grdWmntDetails.Rows[i].Cells["Item Code"].Value.ToString()
                               select a).FirstOrDefault();
                    oDetail.OrderQuantity = Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                }


                oDoc.SCreateBy = Program.oCurrentUser.UserCode + " @ " + Program.ANV;
                oDoc.SCreateDate = DateTime.Now;
                oDoc.SUpdateBy = Program.oCurrentUser.UserCode + " @ " + Program.ANV;
                oDoc.SUpdateDate = DateTime.Now;
                if (chkAllowTolerance.Checked)
                {
                    oDoc.FlgTolerance = chkAllowTolerance.Checked;
                    oDoc.ToleranceLimit = Convert.ToDecimal(txtToleranceLimit.Text);
                    oDoc.ToleranceLimitABy = ToleranceApprovedBy;
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

        public bool UpdateRecordSpecial()
        {
            try
            {
                var odetail = (from a in oDB.TrnsDispatchMultiDetail
                            where a.DocNum.ToString() == txtDocNo.Text
                            select a).ToList();
                if (odetail == null) return false;
                foreach (var one in odetail)
                {
                    for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                    {
                        string grdSBRNo = grdWmntDetails.Rows[i].Cells["SBR#"].Value.ToString();
                        string grdCustomerCode = grdWmntDetails.Rows[i].Cells["Customer Code"].Value.ToString();
                        string grdItemCode = grdWmntDetails.Rows[i].Cells["Item Code"].Value.ToString();
                        decimal grdLoadingQty = Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                        decimal grdBalanceQty = Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Balance Qty"].Value.ToString());
                        if (one.SBRNum == grdSBRNo && one.CustomerCode == grdCustomerCode && one.ItemCode == grdItemCode && one.OrderQuantity != grdLoadingQty)
                        {
                            if (grdLoadingQty > grdBalanceQty)
                            {
                                if (grdItemCode == "FG001" || grdItemCode == "FG003" || grdItemCode == "FG005")
                                {
                                    Double BalQty = ((Convert.ToDouble(grdBalanceQty) * 0.05) + Convert.ToDouble(grdBalanceQty));
                                    if (Convert.ToDouble(grdLoadingQty) > BalQty)
                                    {
                                        Program.WarningMsg("Loading Quantity cannot be greater than Balance quantity.");
                                        return false;
                                    }
                                }
                                else
                                {
                                    Program.WarningMsg("Loading quantity cannot b greater than balance quantity.");
                                    return false;
                                }
                            }
                            one.OrderQuantity = grdLoadingQty;                           
                        }
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

        //public bool UpdateRecordSpecial()
        //{
        //    try
        //    {
        //        bool flgDetailReset = false;
        //        var oDoc = (from a in oDB.TrnsDispatchMultiHeader
        //                    where a.DocNum.ToString() == txtDocNo.Text
        //                    select a).FirstOrDefault();
        //        if (oDoc == null) return false;
        //        if (oDoc.FlgPosted.GetValueOrDefault())
        //        {
        //            Program.WarningMsg("You can't update posted documents.");
        //            return false;
        //        }
        //        //Valid Condition for Reset
        //        /*if (oDoc.OrderQuantity != Convert.ToDecimal(txtOrderQuantity.Text))
        //        {
        //            flgDetailReset = true;
        //        }
        //        if (oDoc.CustomerCode != txtCustomerCode.Text)
        //        {
        //            flgDetailReset = true;
        //        }
        //        if (oDoc.ItemCode != txtItemCod.Text)
        //        {
        //            flgDetailReset = true;
        //        }
        //        if (oDoc.SBRNum != txtSBRNum.Text)
        //        {
        //            flgDetailReset = true;
        //        }
        //        //
        //        oDoc.SBRNum = txtSBRNum.Text;
        //        oDoc.SBRDate = Convert.ToDateTime(txtSBRDate.Text);
        //        oDoc.CustomerCode = txtCustomerCode.Text;
        //        oDoc.CustomerName = txtCustomerName.Text;
        //        oDoc.ItemCode = txtItemCod.Text;
        //        oDoc.ItemName = txtItemNam.Text;
        //        oDoc.ItemGroupCode = ItemGroupCode;
        //        oDoc.ItemGroupName = txtItemGroupName.Text;*/
        //        /*if (!string.IsNullOrEmpty(txtOrderQuantity.Text))
        //        {
        //            if (oDoc.FWeighmentKG.GetValueOrDefault() > 0 && oDoc.SWeighmentKG.GetValueOrDefault() == 0)
        //            {
        //                decimal orderqty = Convert.ToDecimal(txtOrderQuantity.Text);
        //                decimal balanceqty = Convert.ToDecimal(txtBalanceQuantity.Text);
        //                if (orderqty <= balanceqty)
        //                {
        //                    oDoc.OrderQuantity = orderqty;
        //                }
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(txtBalanceQuantity.Text))
        //        {
        //            oDoc.BalanceQuantity = Convert.ToDecimal(txtBalanceQuantity.Text);
        //        }*/
        //        if (!string.IsNullOrEmpty(cmbPacker.Text))
        //        {
        //            oDoc.PackerId = Convert.ToInt32(cmbPacker.SelectedValue);
        //        }
        //        oDoc.VehicleNum = txtVehicleNum.Text;
        //        oDoc.DriverCNIC = txtDriverCNIC.Text;
        //        oDoc.DriverName = txtDriverName.Text;
        //        if (!string.IsNullOrEmpty(txtCNICPath.Text))
        //        {
        //            oDoc.DriverDocument = txtCNICPath.Text;
        //        }
        //        if (!string.IsNullOrEmpty(cmbTransportType.Text))
        //        {
        //            oDoc.TransportID = Convert.ToInt32(cmbTransportType.SelectedValue);
        //        }
        //        if (!string.IsNullOrEmpty(cmbTransportCode.Text))
        //        {
        //            oDoc.TransportCode = Convert.ToString(cmbTransportCode.SelectedValue);
        //        }
        //        if (!string.IsNullOrEmpty(TxtTransportName.Text))
        //        {
        //            oDoc.TransportName = TxtTransportName.Text;
        //        }

        //        if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
        //        {
        //            if (!string.IsNullOrEmpty(txt1WeightKG.Text))
        //            {
        //                if (oDoc.FWeighmentKG != Convert.ToDecimal(txt1WeightKG.Text))
        //                {
        //                    flgDetailReset = true;
        //                }
        //                oDoc.FWeighmentKG = Convert.ToDecimal(txt1WeightKG.Text);
        //            }
        //            if (!string.IsNullOrEmpty(txt1WeightTon.Text))
        //            {
        //                oDoc.FWeighmentTon = Convert.ToDecimal(txt1WeightTon.Text);
        //            }
        //            if (!string.IsNullOrEmpty(txt2WeightKG.Text))
        //            {
        //                if (oDoc.SWeighmentKG != Convert.ToDecimal(txt2WeightKG.Text))
        //                {
        //                    flgDetailReset = true;
        //                }
        //                oDoc.SWeighmentKG = Convert.ToDecimal(txt2WeightKG.Text);
        //            }
        //            if (!string.IsNullOrEmpty(txt2WeightTon.Text))
        //            {
        //                oDoc.SWeighmentTon = Convert.ToDecimal(txt2WeightTon.Text);
        //            }
        //            if (!string.IsNullOrEmpty(txtNetWeightKG.Text))
        //            {
        //                oDoc.NetWeightKG = Convert.ToDecimal(txtNetWeightKG.Text);
        //                if (oDoc.NetWeightKG != Convert.ToDecimal(txtNetWeightKG.Text))
        //                {
        //                    flgDetailReset = true;
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
        //            {
        //                oDoc.NetWeightTon = Convert.ToDecimal(txtNetWeightTon.Text);
        //            }
        //            /*if (txtItemNam.Text.ToLower().Contains("bulk") && !string.IsNullOrEmpty(txtNetWeightTon.Text))
        //            {
        //                oDoc.OrderQuantity = Convert.ToDecimal(txtNetWeightTon.Text);
        //            }
        //            if (!string.IsNullOrEmpty(txtNetWeightTon.Text) && !string.IsNullOrEmpty(txtBalanceQuantity.Text))
        //            {
        //                decimal netweight = Convert.ToDecimal(txtNetWeightTon.Text);
        //                decimal balanceqty = Convert.ToDecimal(txtBalanceQuantity.Text);
        //                if (netweight > balanceqty)
        //                {
        //                    Program.WarningMsg("Net weight cannot be greater than Balance quantity.");
        //                    return false;
        //                }
        //            }*/
        //        }

        //        /*if (flgMultiSales && flgDetailReset)
        //        {
        //            decimal CurOrderQty = oDoc.OrderQuantity.GetValueOrDefault();
        //            decimal valCurOrderQty = CurOrderQty;
        //            if (!flgCustChange)
        //            {
        //                if (!flgItemChange)
        //                {
        //                    if (!flgReselect)
        //                    {
        //                        Program.oErrMgn.LogEntry(Program.ANV, "1st weigh queue count: " + FirstWeightSOList.Count.ToString());
        //                        string solist = string.Empty;
        //                        foreach (var Previous in FirstWeightSOList)
        //                        {
        //                            var oCheck = (from a in SaleOrderList
        //                                          where a.SBRNum == Previous.SBRNum
        //                                          select a).Count();
        //                            if (oCheck == 0)
        //                            {
        //                                SaleOrderData onew = new SaleOrderData();
        //                                onew.SBRNum = Previous.SBRNum;
        //                                onew.Balance = Previous.Balance;
        //                                onew.Order = Previous.Order;
        //                                SaleOrderList.Add(onew);
        //                                solist += " " + Previous.SBRNum;
        //                            }
        //                        }
        //                        Program.oErrMgn.LogEntry(Program.ANV, "1st weigh sbr list: " + solist);
        //                    }
        //                }
        //            }
        //            Program.oErrMgn.LogEntry(Program.ANV, "docnum: " + oDoc.DocNum.ToString() + " queue count: " + SaleOrderList.Count.ToString());
        //            Program.oErrMgn.LogEntry(Program.ANV, "order qty: " + CurOrderQty.ToString());
        //            List<string> ValidSaleOrder = new List<string>();
        //            foreach (var One in SaleOrderList)
        //            {
        //                if (CurOrderQty == 0)
        //                {
        //                    One.Order = 0;
        //                    continue;
        //                }
        //                TrnsDispatchMulti oDetail = new TrnsDispatchMulti();
        //                oDetail.SBRNum = One.SBRNum;
        //                ValidSaleOrder.Add(One.SBRNum);
        //                oDetail.CardCode = oDoc.CustomerCode;
        //                oDetail.ItemCode = oDoc.ItemCode;
        //                oDetail.DocNum = Convert.ToInt32(txtDocNo.Text);
        //                oDetail.BalanceQty = One.Balance;
        //                decimal? CurOpenQty = 0;
        //                oDB.GetSumOpenValueSOValidation(One.SBRNum, oDoc.CustomerCode, oDoc.ItemCode, oDoc.DocNum, ref CurOpenQty);
        //                decimal LiveBalanceQty = 0;
        //                LiveBalanceQty = CurOpenQty.GetValueOrDefault();
        //                Program.oErrMgn.LogEntry(Program.ANV, "SBRNum qty: " + One.SBRNum.ToString());
        //                Program.oErrMgn.LogEntry(Program.ANV, "Live qty: " + LiveBalanceQty.ToString());
        //                if (CurOrderQty >= LiveBalanceQty)
        //                {
        //                    oDetail.OrderQty = LiveBalanceQty;
        //                    CurOrderQty -= LiveBalanceQty;
        //                    One.Order = Convert.ToDecimal(oDetail.OrderQty);
        //                    Program.oErrMgn.LogEntry(Program.ANV, "so: " + One.SBRNum + " qty: " + oDetail.OrderQty.ToString());
        //                }
        //                else if (CurOrderQty < LiveBalanceQty)
        //                {
        //                    oDetail.OrderQty = CurOrderQty;
        //                    One.Order = Convert.ToDecimal(oDetail.OrderQty);
        //                    Program.oErrMgn.LogEntry(Program.ANV, "so: " + One.SBRNum + " qty: " + oDetail.OrderQty.ToString());
        //                    CurOrderQty = 0;
        //                }

        //                oDB.TrnsDispatchMulti.InsertOnSubmit(oDetail);
        //            }
        //            oDoc.SBRNum = string.Join(",", ValidSaleOrder);
        //            if (CurOrderQty > 0)
        //            {
        //                Program.WarningMsg("Order quantity exceeds available open quantity.");
        //                Program.oErrMgn.LogEntry(Program.ANV, "unsettle qty: " + CurOrderQty.ToString());
        //                return false;
        //            }
        //            foreach (var One in SaleOrderList)
        //            {
        //                if (One.Order == 0)
        //                {
        //                    continue;
        //                }
        //                decimal? finaldoc1 = 0;
        //                decimal AvailableValue = 0;
        //                oDB.GetSumOpenValueSOValidation(One.SBRNum, oDoc.CustomerCode, oDoc.ItemCode, oDoc.DocNum, ref finaldoc1);
        //                AvailableValue = finaldoc1.GetValueOrDefault();
        //                if (One.Order > AvailableValue)
        //                {
        //                    Program.WarningMsg("Order quantity exceeds available open quantity. ==> " + AvailableValue.ToString());
        //                    Program.oErrMgn.LogEntry(Program.ANV, "order qty: " + One.Order.ToString());
        //                    Program.oErrMgn.LogEntry(Program.ANV, "available qty: " + AvailableValue.ToString());
        //                    return false;
        //                }
        //            }
        //            if (flgCustChange)
        //            {
        //                oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
        //            }
        //            else if (flgItemChange)
        //            {
        //                oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
        //            }
        //            else if (flgReselect)
        //            {
        //                oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
        //            }
        //            else
        //            {
        //                oDB.RemoveAllDetailsDispatch(oDoc.DocNum);
        //            }
        //        }*/

        //        oDB.SubmitChanges();
        //        Program.SuccesesMsg("Record Successfully Updated.");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, ex);
        //        Program.ExceptionMsg("Something went wrong, Contact Abacus Support.");
        //        return false;
        //    }
        //}

        public bool AddValidate()
        {
            try
            {
                if (grdWmntDetails.Rows.Count == 0)
                {
                    Program.WarningMsg("mandatory.");
                    return false;
                }
                if (grdWmntDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString()) > Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Balance Qty"].Value.ToString()))
                        {
                            if (grdWmntDetails.Rows[i].Cells["Item Code"].Value.ToString() == "FG001" || grdWmntDetails.Rows[i].Cells["Item Code"].Value.ToString() == "FG003" || grdWmntDetails.Rows[i].Cells["Item Code"].Value.ToString() == "FG005")
                            {
                                Double BalQty = ((Convert.ToDouble(grdWmntDetails.Rows[i].Cells["Balance Qty"].Value.ToString()) * 0.05) + Convert.ToDouble(grdWmntDetails.Rows[i].Cells["Balance Qty"].Value.ToString()));
                                if (Convert.ToDouble(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString()) > BalQty)
                                {
                                    Program.WarningMsg("Loading Quantity cannot be greater than Balance quantity.");
                                    return false;
                                }
                            }
                            else
                            {
                                Program.WarningMsg("Loading quantity cannot b greater than balance quantity.");
                                return false;
                            }
                        }
                    }
                }
                /*if (string.IsNullOrEmpty(txtCustomerCode.Text))
                {
                    Program.WarningMsg("Customer is mandatory.");
                    return false;
                }
                if (string.IsNullOrEmpty(txtItemCod.Text))
                {
                    Program.WarningMsg("Item code is mandatory.");
                    return false;
                }
                if (string.IsNullOrEmpty(txtSBRNum.Text))
                {
                    Program.WarningMsg("SBR number is mandatory.");
                    return false;
                }*/
                if (string.IsNullOrEmpty(txtShift.Text))
                {
                    Program.WarningMsg("Shift can't be empty.");
                    return false;
                }
                /*if (string.IsNullOrEmpty(txtOrderQuantity.Text))
                {
                    Program.WarningMsg("Order quantity can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txtOrderQuantity.Text) <= 0)
                    {
                        Program.WarningMsg("Order quantity can't be zero.");
                        return false;
                    }
                }
                if (string.IsNullOrEmpty(txtBalanceQuantity.Text))
                {
                    Program.WarningMsg("Balance quantity can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txtOrderQuantity.Text) > Convert.ToDecimal(txtBalanceQuantity.Text))
                    {
                        Program.WarningMsg("Order Quantity cannot b greater than Balance quantity.");
                        return false;
                    }
                }*/
                //if (string.IsNullOrEmpty(Convert.ToString(cmbPacker.SelectedValue)))
                //{
                //    Program.WarningMsg("Packer should be selected.");
                //    return false;
                //}
                //if (string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.SelectedValue)))
                //{
                //    Program.WarningMsg("Transporter code is mandatory.");
                //    return false;
                //}
                //if (string.IsNullOrEmpty(Convert.ToString(cmbTransportType.SelectedValue)))
                //{
                //    Program.WarningMsg("Transporter type is mandatory");
                //    return false;
                //}
                if (string.IsNullOrEmpty(txt1WeightKG.Text))
                {
                    Program.WarningMsg("First weight can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txt1WeightKG.Text) == 0)
                    {
                        Program.WarningMsg("First weight can't be zero.");
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(txt2WeightKG.Text))
                {
                    Program.WarningMsg("Second weight can't be set at that time.");
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

        public bool UpdateValidate()
        {
            try
            {
                if (grdWmntDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString()) > Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Balance Qty"].Value.ToString()))
                        {
                            Program.WarningMsg("Loading quantity cannot b greater than balance quantity.");
                        }
                    }
                }
                /*if (string.IsNullOrEmpty(txtOrderQuantity.Text))
                {
                    Program.WarningMsg("Order quantity can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txtOrderQuantity.Text) <= 0)
                    {
                        Program.WarningMsg("Order quantity can't be zero.");
                        return false;
                    }
                    if (txtItemNam.Text.ToLower().Contains("bulk"))
                    {
                        txtOrderQuantity.Text = txtNetWeightTon.Text;
                    }
                }
                if (string.IsNullOrEmpty(txtBalanceQuantity.Text))
                {
                    Program.WarningMsg("Balance quantity can't be empty.");
                    return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(txtOrderQuantity.Text))
                    {
                        Program.WarningMsg("Order Quantity can't be zero or nothing.");
                        return false;
                    }
                    if (string.IsNullOrEmpty(txtBalanceQuantity.Text))
                    {
                        Program.WarningMsg("Balance Quantity can't be zero or nothing.");
                        return false;
                    }
                    if (chkAllowTolerance.Checked)
                    {
                        if (string.IsNullOrEmpty(txtToleranceLimit.Text))
                        {
                            Program.WarningMsg("Tolerance limit can't be empty.");
                            return false;
                        }
                    }
                    if (!ChkTolerence())
                    {
                        return false;
                    }*/
                //if (Convert.ToDecimal(txtOrderQuantity.Text) > Convert.ToDecimal(txtBalanceQuantity.Text))
                //{
                //    Program.WarningMsg("Order Quantity cannot b greater than Balance quantity.");
                //    return false;
                //}
                //if (Convert.ToDecimal(txtNetWeightTon.Text) > Convert.ToDecimal(txtBalanceQuantity.Text))
                //{
                //    Program.WarningMsg("Net weight cannot b greater than Balance quantity.");
                //    return false;
                //}
                /*}*/
                //if (string.IsNullOrEmpty(Convert.ToString(cmbPacker.Text)))
                //{
                //    Program.WarningMsg("Packer should be selected.");
                //    return false;
                //}
                //if (string.IsNullOrEmpty(Convert.ToString(cmbTransportCode.Text)))
                //{
                //    Program.WarningMsg("Transporter code is mandatory.");
                //    return false;
                //}
                //if (string.IsNullOrEmpty(Convert.ToString(cmbTransportType.Text)))
                //{
                //    Program.WarningMsg("Transporter type is mandatory");
                //    return false;
                //}
                if (string.IsNullOrEmpty(txt1WeightKG.Text))
                {
                    Program.WarningMsg("First weight can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txt1WeightKG.Text) == 0)
                    {
                        Program.WarningMsg("First weight can't be zero.");
                        return false;
                    }
                }
                if (string.IsNullOrEmpty(txt2WeightKG.Text))
                {
                    Program.WarningMsg("Second weight can't be empty.");
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(txt2WeightKG.Text) == 0)
                    {
                        Program.WarningMsg("Second weight can't be zero.");
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(txtTotalLoadingQty.Text))
                {
                    Program.WarningMsg("Total Loading Qty can't be empty.");
                    return false;
                }
                else if (Convert.ToDecimal(txtTotalLoadingQty.Text) == 0)
                {
                    Program.WarningMsg("Total Loading Qty can't be zero.");
                    return false;
                }

                if (chkAllowTolerance.Checked)
                {
                    if (string.IsNullOrEmpty(txtToleranceLimit.Text))
                    {
                        Program.WarningMsg("Tolerance limit can't be empty.");
                        return false;
                    }
                }
                if (!ChkTolerence())
                {
                    return false;
                }


                /*if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
                {
                    if (grdWmntDetails.Rows.Count > 0)
                    {
                        totalLoadingQty = 0;
                        for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                        {
                            totalLoadingQty += Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                        }
                        if (totalLoadingQty > Convert.ToDecimal(txtNetWeightTon.Text))
                        {
                            Program.WarningMsg("Loading quantity cannot b greater than Net weight.");
                            return false;
                        }
                        else if (totalLoadingQty != Convert.ToDecimal(txtNetWeightTon.Text))
                        {
                            Program.WarningMsg("Loading quantity is not equal to Net weight.");
                            return false;
                        }
                    }
                }*/


                return true;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                return false;
            }
        }

        public void AddDocument()
        {
            try
            {
                if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    if (UpdateRecordSpecial())
                    {
                        frmOpenDlg AddSeals = new frmOpenDlg();
                        if (AddSeals.AddSealToDb())
                        {
                        }
                        AddSeals.Dispose();
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
                                frmOpenDlg AddSeals = new frmOpenDlg();
                                if (AddSeals.AddSealToDb())
                                {
                                    var oDoc = (from a in oDB.TrnsDispatchMultiHeader
                                                where a.DocNum.ToString() == txtDocNo.Text
                                                select a).FirstOrDefault();
                                    if (oDoc != null)
                                    {
                                        if (Program.OpenLayout(Program.Screen, txtDocNo.Text, Program.Screen + " " + txtDocNo.Text))
                                        {
                                            oDoc.Flg1Rpt = true;
                                            oDB.SubmitChanges();
                                        }
                                    }
                                }
                                AddSeals.Dispose();
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
                                frmOpenDlg AddSeal = new frmOpenDlg();
                                if (AddSeal.AddSealToDb())
                                {
                                    var oDoc = (from a in oDB.TrnsDispatchMultiHeader
                                                where a.DocNum.ToString() == txtDocNo.Text
                                                select a).FirstOrDefault();
                                    if (oDoc != null)
                                    {
                                        if (oDoc.FlgSecondWeight.GetValueOrDefault())
                                        {
                                            if (Program.OpenLayout(Program.Screen, txtDocNo.Text, Program.Screen + " " + txtDocNo.Text))
                                            {
                                                oDoc.Flg2Rpt = true;
                                            }
                                            if (Program.OpenLayout("WayBridgeDelivery", txtDocNo.Text, "WayBridgeDelivery " + txtDocNo.Text))
                                            {
                                                oDoc.FlgWBrpt = true;
                                            }
                                            oDB.SubmitChanges();
                                        }
                                    }
                                }
                                AddSeal.Dispose();
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

        public void ApplyAuthorization()
        {
            try
            {
                if (Program.oCurrentUser.UserCode.ToLower() == "manager")
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtCustomerCode.Enabled = false;
                    txtItemCod.Enabled = false;
                    txtSBRNum.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNum.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = true;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG.Enabled = true;
                    txtNetWeightKG.Enabled = false;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtCustomerName.Enabled = false;
                    txtItemNam.Enabled = false;
                    txtSBRDate.Enabled = false;
                    txtBalanceQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txtDriverName.Enabled = true;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    TxtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeight.Enabled = false;
                    txtTotalLoadingQty.Enabled = false;

                }
                else if (Program.oCurrentUser.FlgSuper.GetValueOrDefault() && !Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtCustomerCode.Enabled = false;
                    txtItemCod.Enabled = false;
                    txtSBRNum.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNum.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = false;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG.Enabled = false;
                    txtNetWeightKG.Enabled = false;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtCustomerName.Enabled = false;
                    txtItemNam.Enabled = false;
                    txtSBRDate.Enabled = false;
                    txtBalanceQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txtDriverName.Enabled = true;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    TxtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeight.Enabled = false;
                    txtTotalLoadingQty.Enabled = false;
                }
                else if (Program.oCurrentUser.FlgSuper.GetValueOrDefault() && Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtCustomerCode.Enabled = false;
                    txtItemCod.Enabled = false;
                    txtSBRNum.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNum.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = true;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG.Enabled = true;
                    txtNetWeightKG.Enabled = false;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtCustomerName.Enabled = false;
                    txtItemNam.Enabled = false;
                    txtSBRDate.Enabled = false;
                    txtBalanceQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txtDriverName.Enabled = true;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    TxtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeight.Enabled = false;
                    txtTotalLoadingQty.Enabled = false;
                }
                else
                {
                    txtDocNo.Enabled = false;
                    txtCurrDate.Enabled = false;
                    txtCustomerCode.Enabled = false;
                    txtItemCod.Enabled = false;
                    txtSBRNum.Enabled = false;
                    txtOrderQuantity.Enabled = true;
                    txtItemGroupName.Enabled = false;
                    txtVehicleNum.Enabled = true;
                    txtDriverCNIC.Enabled = true;
                    txt1WeightDate.Enabled = false;
                    txt1WeightKG.Enabled = false;
                    txt2WeightDate.Enabled = false;
                    txt2WeightKG.Enabled = false;
                    txtNetWeightKG.Enabled = false;

                    txtShift.Enabled = false;
                    txtCurrTime.Enabled = false;
                    txtCustomerName.Enabled = false;
                    txtItemNam.Enabled = false;
                    txtSBRDate.Enabled = false;
                    txtBalanceQuantity.Enabled = false;
                    txtDaySeries.Enabled = false;
                    txtDriverName.Enabled = true;
                    txt1WeightTime.Enabled = false;
                    txt1WeightTon.Enabled = false;
                    TxtTransportName.Enabled = false;
                    txtCNICPath.Enabled = false;
                    txtBrandPath.Enabled = false;
                    txt2WeightTime.Enabled = false;
                    txt2WeightTon.Enabled = false;
                    txtNetWeightTon.Enabled = false;
                    txtDifferenceWeight.Enabled = false;
                    txtTotalLoadingQty.Enabled = false;
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
                    int value = (from a in oprivate.TrnsDispatchMultiHeader
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
                    string data = comportDispatch.ReadExisting();
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
                    string data = comportDispatch.ReadLine();
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

        private void HandleDialogControlCancelAuthorizedUser()
        {
            lblToleranceLimit.Visible = false;
            txtToleranceLimit.Visible = false;
            txtToleranceLimit.Text = string.Empty;
            flgChkStatus = false;
            chkAllowTolerance.Checked = false;
        }
        #endregion

        #region Events
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void tmrAlreadyReading_Tick(object sender, EventArgs e)
        {
            alreadyReading = false;
        }

        private void btnGetWeight_Click(object sender, EventArgs e)
        {
            try
            {
                //Program.oErrMgn.LogEntry(Program.ANV, "Getweight " + txtCWeight.Text);
                //string Cval = txtCWeight.Text;
                //// txtFullText.Text = Cval;
                //Program.oErrMgn.LogEntry(Program.ANV, "Gotweight " + Cval);
                //lblWeight.Text = Cval;// txtFullText.Text;
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
                    txt2WeightKG.Text = lblWeight.Text;
                }
                else if (string.IsNullOrEmpty(txt2WeightKG.Text))
                {
                    txt1WeightKG.Text = lblWeight.Text;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string critaria = txtDocNo.Text;
                string PldFor = Program.Screen.Trim();
                string Fwmnt = txt1WeightKG.Text;
                if (!string.IsNullOrEmpty(Fwmnt))
                {
                    if (lbl2.Text == "True")
                    {
                        Program.OpenLayout(PldFor, critaria, PldFor + " " + txtDocNo.Text);
                        Program.OpenLayout("WayBridgeDelivery", critaria, "WayBridgeDelivery " + txtDocNo.Text);
                    }
                    else
                    {
                        Program.OpenLayout(PldFor, critaria, PldFor + " " + txtDocNo.Text);
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

        private void grdDetails_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                int DocNum = 0;
                DocNum = Convert.ToInt32(e.Row.Cells["Wmnt#"].Value);
                Program.DocNum = DocNum.ToString();
                setValues(DocNum);
                ApplyAuthorization();
                txtCWeight.Enabled = false;
                btnSubmit.Text = "&Update";
                if (btnSubmit.Text == "&Update")
                {
                    btnAdd.Enabled = false;
                    //if (Program.oCurrentUser.FlgSpecial.GetValueOrDefault())
                    //{
                    //    grdWmntDetails.Columns["Loading Qty"].ReadOnly = true;
                    //}
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                //txtFullText.Text = "";
                //txtFullText.Text= mostRecentWeight.ToString();
                InitiallizeDocument();
                btnSubmit.Text = "&Add";
                BtnDocSelect.Enabled = true;
                BtnItemSelect.Enabled = true;
                btnSubmit.Enabled = true;
                txtDriverCNIC.Enabled = true;
                txtDriverName.Enabled = true;
                TxtTransportName.Enabled = false;
                txtVehicleNum.Enabled = true;
                cmbPacker.Enabled = true;
                cmbTransportType.Enabled = true;
                cmbTransportCode.Enabled = true;
                txtOrderQuantity.Enabled = true;
                if (Program.oCurrentUser.UserCode.ToLower() == "manager")
                {
                    txt1WeightKG.Enabled = true;
                    txt2WeightKG.Enabled = true;
                }
                //grdWmntDetails.Columns["Loading Qty"].ReadOnly = false;
                // txtOrderQuantity.Enabled = true;
                flgSetValues = false;
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                //if (super == false)
                //{
                //    txtDocNo.Enabled = false;
                //    txtSBRDate.Enabled = false;
                //    txtSBRNum.Enabled = false;
                //    txtShift.Enabled = false;
                //    txtCustomerName.Enabled = false;
                //    txtCustomerCode.Enabled = false;
                //    txtItemCod.Enabled = false;
                //    txtItemGroupName.Enabled = false;
                //    txtItemNam.Enabled = false;
                //    txtOrderQuantity.Enabled = true;
                //    txtBalanceQuantity.Enabled = false;
                //    txtCurrDate.Enabled = false;
                //    txtCurrTime.Enabled = false;
                //    txtDaySeries.Enabled = false;
                //    TxtTransportName.Enabled = false;
                //    txt1WeightDate.Enabled = false;
                //    txt1WeightKG.Enabled = false;
                //    txt1WeightTime.Enabled = false;
                //    txt1WeightTon.Enabled = false;
                //    txt2WeightDate.Enabled = false;
                //    txt2WeightKG.Enabled = false;
                //    txt2WeightTime.Enabled = false;
                //    txt2WeightTon.Enabled = false;
                //    txtNetWeightKG.Enabled = false;
                //    txtNetWeightTon.Enabled = false;
                //    txtDifferenceWeight.Enabled = false;
                //}
                txtCWeight.Enabled = false;
                flgMultiSales = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void txtDocNo_TextChanged(object sender, EventArgs e)
        {
            if (Program.sealdt != null)
            {
                Program.sealdt.Clear();
            }

        }

        private void txtFullText_TextChanged(object sender, EventArgs e)
        {
            //lblWeight.Text = txtFullText.Text;
            ////if (string.IsNullOrEmpty(txt1WeightKG.Text) || txt1WeightKG.Text == "0")
            ////{

            ////}
            //else
            if (flgSetValues)
            {
                txt2WeightKG.Text = txtFullText.Text;
            }
            else if (string.IsNullOrEmpty(txt2WeightKG.Text))
            {
                txt1WeightKG.Text = txtFullText.Text;
            }
            lblWeight.Text = txtFullText.Text;
        }

        private void btnNextRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                int DocNum = Convert.ToInt32(txtDocNo.Text);
                int previusDocNum = DocNum + 1;
                int checkDoc = 0;
                checkDoc = (from a in oDB.TrnsDispatchMultiHeader where a.DocNum == previusDocNum select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    ApplyAuthorization();
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //TxtTransportName.Enabled = false;
                    //txtVehicleNum.Enabled = false;
                    //cmbPacker.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                    //txtOrderQuantity.Enabled = false;
                    btnSubmit.Text = "&Update";
                    //flgSetValues = false;
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
                    //    BtnDocSelect.Enabled = false;
                    //    BtnItemSelect.Enabled = false;
                    //    btnSubmit.Enabled = false;
                    //}
                    //txtCWeight.Enabled = false;
                }
                else
                {
                    Program.WarningMsg("Reached To Last Record");
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
                int DocNum = Convert.ToInt32(txtDocNo.Text);
                int previusDocNum = DocNum - 1;
                int checkDoc = 0;
                checkDoc = (from a in oDB.TrnsDispatchMultiHeader where a.DocNum == previusDocNum select a).Count();

                if (checkDoc > 0)
                {
                    setValues(previusDocNum);
                    ApplyAuthorization();
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //TxtTransportName.Enabled = false;
                    //txtVehicleNum.Enabled = false;
                    //cmbPacker.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                    //txtOrderQuantity.Enabled = false;
                    btnSubmit.Text = "&Update";
                    //flgSetValues = false;
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
                    //    BtnDocSelect.Enabled = false;
                    //    BtnItemSelect.Enabled = false;
                    //    btnSubmit.Enabled = false;
                    //}
                    //txtCWeight.Enabled = false;
                }
                else
                {
                    Program.WarningMsg("Reached To First Record");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnLastRecord_Click_1(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDB.TrnsDispatchMultiHeader select a.DocNum).Max();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    ApplyAuthorization();
                    btnSubmit.Text = "&Update";
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //TxtTransportName.Enabled = false;
                    //txtVehicleNum.Enabled = false;
                    //cmbPacker.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                    //txtOrderQuantity.Enabled = false;
                    //flgSetValues = false;
                    //Program.WarningMsg("Reached To Last Record");
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
                    //    BtnDocSelect.Enabled = false;
                    //    BtnItemSelect.Enabled = false;
                    //    btnSubmit.Enabled = false;
                    //}
                    //txtCWeight.Enabled = false;
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
                var MaxRecord = (from a in oDB.TrnsDispatchMultiHeader select a.DocNum).First();

                if (MaxRecord != null)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    ApplyAuthorization();
                    btnSubmit.Text = "&Update";
                    //txtDriverCNIC.Enabled = false;
                    //txtDriverName.Enabled = false;
                    //TxtTransportName.Enabled = false;
                    //txtVehicleNum.Enabled = false;
                    //cmbPacker.Enabled = false;
                    //cmbTransportType.Enabled = false;
                    //cmbTransportCode.Enabled = false;
                    //txtOrderQuantity.Enabled = false;
                    //flgSetValues = false;
                    //Program.WarningMsg("Reached To First Record");
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
                    //    BtnDocSelect.Enabled = false;
                    //    BtnItemSelect.Enabled = false;
                    //    btnSubmit.Enabled = false;
                    //}
                    //txtCWeight.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Program.DocNum))
                {
                    Program.DocNum = null;
                    HandleDialogSearch();
                    if (Program.DocNum != null)
                    {
                        setValues(Convert.ToInt32(Program.DocNum));
                        btnSubmit.Text = "&Update";
                    }
                    flgSetValues = false;
                    txtCWeight.Enabled = false;
                }
                else
                {
                    Program.WarningMsg("You can't search document already selected.");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void txt1WeightKG_TextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txt2WeightKG.Text) && !string.IsNullOrEmpty(txt1WeightKG.Text))
            //{
            //    txtNetWeightKG.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightKG.Text) - Convert.ToDecimal(txt2WeightKG.Text)));
            //    txt1WeightTon.Text = Convert.ToString(Convert.ToDecimal(txt1WeightKG.Text) / 1000);
            //    txt2WeightTon.Text = Convert.ToString(Convert.ToDecimal(txt2WeightKG.Text) / 1000);
            //}
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

        private void txt2WeightKG_TextChanged(object sender, EventArgs e)
        {

            //Program.NoMsg("");
            //int n;
            //bool isNumeric = int.TryParse(Convert.ToString(txt2WeightKG.Text), out n);

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
                    //if (isNumeric)
                    //{


                    txt2WeightTon.Text = Convert.ToString(Convert.ToDecimal(txt2WeightKG.Text) / 1000);
                    txtNetWeightKG.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightKG.Text) - Convert.ToDecimal(txt2WeightKG.Text)));
                    txtNetWeightTon.Text = Convert.ToString(System.Math.Abs(Convert.ToDecimal(txt1WeightTon.Text) - Convert.ToDecimal(txt2WeightTon.Text)));
                    /*if (!string.IsNullOrEmpty(txtOrderQuantity.Text))
                    {
                        txtDifferenceWeight.Text = Convert.ToString(Convert.ToDecimal(txtNetWeightTon.Text) - Convert.ToDecimal(txtOrderQuantity.Text));
                    }*/
                    if (grdWmntDetails.Rows.Count > 0)
                    {

                        totalLoadingQty = 0;
                        for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                        {
                            totalLoadingQty += Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                        }
                        txtDifferenceWeight.Text = Convert.ToString(totalLoadingQty - Convert.ToDecimal(txtNetWeightTon.Text));
                    }
                    //}
                    //else
                    //{
                    //    Program.ExceptionMsg("Values can only be Numeric");
                    //}

                }
            }
            else
            {
                txt2WeightTon.Text = null;
                txtNetWeightKG.Text = null;
                txtNetWeightTon.Text = null;
                txtDifferenceWeight.Text = null;
            }
        }

        private void cmbTransportCode_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbTransportCode.Text))
                {
                    string[] CardName = cmbTransportCode.Text.Split(':');
                    string CArdName1 = CardName[1];
                    DataTable val = new DataTable();
                    string strQuery = @"select CardName from ocrd Where GroupCode = 108 and CardCode ='" + CArdName1 + "'";
                    //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                    val = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);
                    TxtTransportName.Text = val.Rows[0][0].ToString();
                }
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
                AddDocument();
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
            //if (btnCancel.Text == "Close")
            //{
            //    oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
            //}
            //if (oResult == System.Windows.Forms.DialogResult.Cancel)
            //{
            //    //Program.comport.Close();
            //    this.Dispose();
            //    base.mytabpage.Dispose();
            //}
            try
            {
                if (comportDispatch.IsOpen)
                {
                    comportDispatch.DiscardInBuffer();
                    comportDispatch.DiscardOutBuffer();
                    comportDispatch.Close();
                }
                Program.flgIndicator = false;
                this.Dispose();
                base.mytabpage.Dispose();
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogEntry(Program.ANV, "Form Close Port Exception Dispatch.");
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (flgMultiSales)
                {
                    HandleDialogControlSaleOrderMulti();
                }
                else
                {
                    HandleDialogControlSaleOrder();
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void frmDispatch_Load(object sender, EventArgs e)
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

        private void grdWmntDetails_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (e.Column.Name == "Loading Qty")
                {
                    if (Convert.ToDecimal(grdWmntDetails.Rows[e.RowIndex].Cells["Loading Qty"].Value.ToString()) > Convert.ToDecimal(grdWmntDetails.Rows[e.RowIndex].Cells["Balance Qty"].Value.ToString()))
                    {
                        if (grdWmntDetails.Rows[e.RowIndex].Cells["Item Code"].Value.ToString() == "FG001" || grdWmntDetails.Rows[e.RowIndex].Cells["Item Code"].Value.ToString() == "FG003" || grdWmntDetails.Rows[e.RowIndex].Cells["Item Code"].Value.ToString() == "FG005")
                        {
                            Double BalQty = ((Convert.ToDouble(grdWmntDetails.Rows[e.RowIndex].Cells["Balance Qty"].Value.ToString()) * 0.05) + Convert.ToDouble(grdWmntDetails.Rows[e.RowIndex].Cells["Balance Qty"].Value.ToString()));
                            if (Convert.ToDouble(grdWmntDetails.Rows[e.RowIndex].Cells["Loading Qty"].Value.ToString()) > BalQty)
                            {
                                Program.WarningMsg("Loading Quantity cannot be greater than Balance quantity.");
                            }
                        }
                        else
                        {
                            Program.WarningMsg("Loading Quantity cannot be greater than Balance quantity.");
                        }
                    }
                    /*if (!string.IsNullOrEmpty(txtNetWeightTon.Text))
                    {
                        if (grdWmntDetails.Rows.Count > 0)
                        {
                            totalLoadingQty = 0;
                            for (int i = 0; i < grdWmntDetails.Rows.Count; i++)
                            {
                                totalLoadingQty += Convert.ToDecimal(grdWmntDetails.Rows[i].Cells["Loading Qty"].Value.ToString());
                            }
                            if (totalLoadingQty > Convert.ToDecimal(txtNetWeightTon.Text))
                            {
                                Program.WarningMsg("Loading quantity cannot b greater than Net weight.");
                            }
                        }

                    }*/
                    txtTotalLoadingQty.Text = TotalLoadingQty().ToString();
                    txt2WeightKG.Text = "";
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
                txtCNICPath.Text = theDialog.FileName.ToString();
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



        private void BtnItemSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!flgMultiSales)
                {
                    HandleDialogControlSaleOrderItem();
                    txtDaySeries.Text = Convert.ToString(daySeries());
                }
                else
                {

                    HandleDialogControlSaleOrderItemNewLogic();
                    txtDaySeries.Text = Convert.ToString(daySeries());
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnCust_Click(object sender, EventArgs e)
        {
            try
            {
                HandleDialogControlCustomer();
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
                    DialogResult oResult = RadMessageBox.Show("You will need to require administrator permission to allow extra tolerance.", "Confirmation.", MessageBoxButtons.OKCancel);
                    if (oResult == System.Windows.Forms.DialogResult.OK)
                    {
                        //pnlLogin.Visible = true;
                        frmLoginDlg oDlg = new frmLoginDlg();
                        oDlg.ShowDialog();
                        if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            ToleranceApprovedBy = oDlg.ToleranceApprovedBy;
                            lblToleranceLimit.Visible = oDlg.lblToleranceLimit;
                            txtToleranceLimit.Visible = oDlg.txtToleranceLimitVisible;
                            flgChkStatus = oDlg.flgChkStatus;
                        }
                        else if (oDlg.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                        {
                            HandleDialogControlCancelAuthorizedUser();
                        }
                    }
                    else if (oResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        HandleDialogControlCancelAuthorizedUser();
                    }
                }
                else
                {
                    HandleDialogControlCancelAuthorizedUser();
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtSBRNum.Text = string.Empty;
                txtSBRDate.Text = string.Empty;
                txtItemCod.Text = string.Empty;
                txtItemNam.Text = string.Empty;
                txtOrderQuantity.Text = string.Empty;
                txtBalanceQuantity.Text = string.Empty;
                txtItemGroupName.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }

        }

        private void txtSBRNum_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtItemCod.Text = string.Empty;
                txtItemNam.Text = string.Empty;
                txtOrderQuantity.Text = string.Empty;
                txtBalanceQuantity.Text = string.Empty;
                txtItemGroupName.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void txtItemCod_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtOrderQuantity.Text = string.Empty;
                txtBalanceQuantity.Text = string.Empty;
                txtItemGroupName.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCustomerCode.Text))
                {
                    Program.WarningMsg("Customer should be selected");
                    return;
                }
                else if (string.IsNullOrEmpty(txtSBRNum.Text))
                {
                    Program.WarningMsg("SBR number should be selected.");
                    return;
                }
                else if (string.IsNullOrEmpty(txtItemCod.Text))
                {
                    Program.WarningMsg("Item should be selected.");
                    return;
                }
                else if (string.IsNullOrEmpty(txtOrderQuantity.Text))
                {
                    Program.WarningMsg("Loading Quantity can't be empty.");
                    return;
                }
                else if (Convert.ToDecimal(txtOrderQuantity.Text) <= 0)
                {
                    Program.WarningMsg("Order quantity can't be zero.");
                    return;
                }
                else if (Convert.ToDecimal(txtOrderQuantity.Text) > Convert.ToDecimal(txtBalanceQuantity.Text))
                {
                    if (txtItemCod.Text == "FG001" || txtItemCod.Text == "FG003" || txtItemCod.Text == "FG005")
                    {
                        Double BalQty = ((Convert.ToDouble(txtBalanceQuantity.Text) * 0.05) + Convert.ToDouble(txtBalanceQuantity.Text));
                        if (Convert.ToDouble(txtOrderQuantity.Text) > BalQty)
                        {
                            Program.WarningMsg("Order Quantity cannot be greater than Balance quantity.");
                            return;
                        }
                    }
                    else
                    {
                        Program.WarningMsg("Order Quantity cannot be greater than Balance quantity.");
                        return;
                    }
                }
                //else
                //{
                    grdWmntDetails.Rows.Add(txtCustomerCode.Text, txtCustomerName.Text, txtSBRNum.Text, txtSBRDate.Text, ItemGroupCode, txtItemGroupName.Text, txtItemCod.Text, txtItemNam.Text, txtOrderQuantity.Text, txtBalanceQuantity.Text);

                    ItemCheckList odata = new ItemCheckList();
                    odata.SBRNum = Convert.ToInt32(txtSBRNum.Text);
                    odata.ItemCode = txtItemCod.Text;
                    ItemList.Add(odata);

                    txtTotalLoadingQty.Text = TotalLoadingQty().ToString();
                    txtSBRNum.Text = string.Empty;
                    txtSBRDate.Text = string.Empty;
                    txtItemCod.Text = string.Empty;
                    txtItemNam.Text = string.Empty;
                    txtOrderQuantity.Text = string.Empty;
                    txtBalanceQuantity.Text = string.Empty;
                    txtItemGroupName.Text = string.Empty;
                //}
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


        private void grdWmntDetails_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            try
            {
                int value = e.Rows.FirstOrDefault().Index;
                string SONo = grdWmntDetails.Rows[value].Cells["SBR#"].Value.ToString();
                string itemCode = grdWmntDetails.Rows[value].Cells["Item Code"].Value.ToString();
                int count = 0, itemAt = 0;
                foreach (var t in ItemList)
                {
                    if (Convert.ToInt32(SONo) == t.SBRNum && itemCode == t.ItemCode)
                    {
                        itemAt = count;
                    }
                    count++;
                }
                ItemList.RemoveAt(itemAt);
            }
            catch (Exception ex)
            {

            }
        }

        private void grdWmntDetails_UserDeletedRow(object sender, GridViewRowEventArgs e)
        {
            try
            {
                txtTotalLoadingQty.Text = TotalLoadingQty().ToString();
            }
            catch (Exception ex)
            {

            }

        }

        #endregion
    }


}

public class ItemCheckList
{
    public int SBRNum { get; set; }
    public string ItemCode { get; set; }

}


