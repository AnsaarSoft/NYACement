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
using mfmWeighment;
using mfmFFSDB;
using UFFU;

namespace mfmFFS.Dialog
{
    public partial class frmOpenDlg : RadForm
    {

        #region Variable
        public int Dispatchcode = 0;
        public string SealNumber = "";
        public string FormItemCode = "";
        public int FormDocNum = 0;
        public string FormName = "";
        public String OpenFor = "";
        public String SelectedObjectID = "-1";
        public String SelectedObjectIDComplex = "-1";
        public String DocNum = "";
        public String DocDate = "";

        public String CustomerCode = "";
        public String CustomerName = "";

        public String VendorCode = "";
        public String VendorName = "";

        public String ItemCode = "";
        public String Dscription = "";
        public String ItmsGrpNam = "";
        public String ItmsGrpCod = "";
        public String Image = "";

        public String Quantity = "";
        public String Balance = "";
        public String ItemName = "";
        public String unitMsr = "";
        public String Price = "";
        public String LineTotal = "";
        public String U_VehcleNo = "";
        public String U_Driver = "";
        public String DriverCnic = "";
        public string UsrPrecision = "{0:F2}";
        public string DocType = "";
        public Boolean flgAll = false;
        public Boolean flgToggle = false;
        public Boolean flgXSlip = false;
        public string SourceDocNum = "";
        public Boolean flgMultiSelect = false;
        //public List<string> oItems = new List<string>();
        //public List<string> oQuantity = new List<string>();
        //public List<string> oBalance = new List<string>();
        public List<SaleOrderData> oData = new List<SaleOrderData>();

        private dbFFS oDB = null;

        DataTable dtGrid;
        DataTable dtItems;
        DataTable dtSOrder;
        DataTable dtPOrder;
        DataTable dtDoOrder;
        DataTable dtDOItems;
        DataTable dtGrpoItems;
        DataTable dtGproOrder;
        DataTable ForRemainingQuantity;

        Screens.frmDispatch frmdispatch = new Screens.frmDispatch();

        #endregion 

        #region Functions

        private void InitiallizeSetting()
        {
            try
            {
                Program.NoMsg("");
                oDB = new dbFFS(Program.ConStrApp);

                SetTheGrid();
                LoadTheGrid();
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void SetTheGrid()
        {
            try
            {
                switch (OpenFor)
                {
                    //case "Roles":
                    //    dtGrid = new DataTable();
                    //    dtGrid.Columns.Add("ID");
                    //    dtGrid.Columns.Add("Serial");
                    //    dtGrid.Columns.Add("RolesName");
                    //    break;
                    //case "Users":
                    //    dtGrid = new DataTable();
                    //    dtGrid.Columns.Add("ID");
                    //    dtGrid.Columns.Add("Serial");
                    //    dtGrid.Columns.Add("UserName");
                    //    dtGrid.Columns.Add("UserCode");
                    //    break;
                    //case "WBSetting":
                    //    dtGrid = new DataTable();
                    //    dtGrid.Columns.Add("ID");
                    //    dtGrid.Columns.Add("Serial");
                    //    dtGrid.Columns.Add("WBCode");
                    //    dtGrid.Columns.Add("MachineIP");
                    //    break;
                    //case "sboOGIP":
                    //    dtGrid = new DataTable();
                    //    dtGrid.Columns.Add("ID");
                    //    dtGrid.Columns.Add("Serial");
                    //    dtGrid.Columns.Add("DocEntry");
                    //    dtGrid.Columns.Add("DocNumber");
                    //    dtGrid.Columns.Add("CardCode");
                    //    dtGrid.Columns.Add("Driver");
                    //    dtGrid.Columns.Add("VehicleReg");
                    //    dtGrid.Columns.Add("VoucherNo");
                    //    break;
                    //case "sboRoshan":
                    //    dtGrid = new DataTable();
                    //    dtGrid.Columns.Add("ID");
                    //    dtGrid.Columns.Add("Serial");
                    //    dtGrid.Columns.Add("DocEntry");
                    //    dtGrid.Columns.Add("DocNumber");
                    //    dtGrid.Columns.Add("PONumber");
                    //    dtGrid.Columns.Add("CardCode");
                    //    dtGrid.Columns.Add("CardName");
                    //    dtGrid.Columns.Add("ItemCode");
                    //    dtGrid.Columns.Add("ItemName");
                    //    dtGrid.Columns.Add("VoucherNo");
                    //    dtGrid.Columns.Add("VehicleNo");
                    //    dtGrid.Columns.Add("flgXSlip", typeof(Boolean));
                    //    break;
                    case "oSaleOrder":
                    case "oSaleOrderM":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        if (flgMultiSelect)
                        {
                            dtGrid.Columns.Add("Select", typeof(bool));
                        }
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("CustomerCode");
                        dtGrid.Columns.Add("CustomerName");
                        dtGrid.Columns.Add("ItemName");
                        dtGrid.Columns.Add("OrderQty");
                        dtGrid.Columns.Add("Balance");
                        //dtGrid.Columns.Add("ItemCode");
                        //dtGrid.Columns.Add("Dscription");
                        //dtGrid.Columns.Add("ItmsGrpNam");
                        //dtGrid.Columns.Add("ItmsGrpCod");
                        //dtGrid.Columns.Add("Image");
                        //dtGrid.Columns.Add("Balance");
                        //dtGrid.Columns.Add("U_VehcleNo");
                        //dtGrid.Columns.Add("Quantity");


                        //dtGrid.Columns.Add("U_Driver");
                        //dtGrid.Columns.Add("DriverCnic");
                        break;

                    case "oDoOrder":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("CustomerCode");
                        dtGrid.Columns.Add("CustomerName");
                        //dtGrid.Columns.Add("ItemCode");
                        //dtGrid.Columns.Add("Dscription");
                        //dtGrid.Columns.Add("ItmsGrpNam");
                        //dtGrid.Columns.Add("ItmsGrpCod");
                        //dtGrid.Columns.Add("Image");
                        //dtGrid.Columns.Add("Balance");
                        //dtGrid.Columns.Add("U_VehcleNo");
                        //dtGrid.Columns.Add("Quantity");
                        //dtGrid.Columns.Add("U_Driver");
                        //dtGrid.Columns.Add("DriverCnic");
                        break;

                    case "oDoGPRO":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        //dtGrid.Columns.Add("CustomerCode");
                        //dtGrid.Columns.Add("CustomerName");
                        dtGrid.Columns.Add("VendorCode");
                        dtGrid.Columns.Add("VendorName");
                        break;
                    case "oDoItems":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");

                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("Dscription");
                        dtGrid.Columns.Add("ItmsGrpNam");
                        dtGrid.Columns.Add("ItmsGrpCod");
                        dtGrid.Columns.Add("Image");
                        dtGrid.Columns.Add("Balance");
                        dtGrid.Columns.Add("U_VehcleNo");

                        dtGrid.Columns.Add("DocEntry");
                        dtGrid.Columns.Add("LineNum");
                        dtGrid.Columns.Add("Objtype");

                        dtGrid.Columns.Add("Quantity");
                        dtGrid.Columns.Add("U_Driver");
                        dtGrid.Columns.Add("DriverCnic");
                        break;

                    case "oGRPOItems":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");

                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("Dscription");
                        dtGrid.Columns.Add("ItmsGrpNam");
                        dtGrid.Columns.Add("ItmsGrpCod");
                        dtGrid.Columns.Add("Image");
                        dtGrid.Columns.Add("Balance");
                        dtGrid.Columns.Add("U_VehcleNo");
                        dtGrid.Columns.Add("DocEntry");
                        dtGrid.Columns.Add("LineNum");
                        dtGrid.Columns.Add("Objtype");
                        dtGrid.Columns.Add("Quantity");
                        dtGrid.Columns.Add("U_Driver");
                        dtGrid.Columns.Add("DriverCnic");
                        break;


                    case "oItem":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");

                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("Dscription");
                        dtGrid.Columns.Add("ItmsGrpNam");
                        dtGrid.Columns.Add("ItmsGrpCod");
                        dtGrid.Columns.Add("Image");
                        dtGrid.Columns.Add("Quantity");
                        dtGrid.Columns.Add("Balance");
                        dtGrid.Columns.Add("U_VehcleNo");
                        dtGrid.Columns.Add("DocEntry");
                        dtGrid.Columns.Add("LineNum");
                        dtGrid.Columns.Add("Objtype");
                        dtGrid.Columns.Add("U_Driver");
                        dtGrid.Columns.Add("DriverCnic");
                        break;
                    case "oBulkSealer":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("ID");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("SealNumber");
                        dtGrid.Columns.Add("FormName");
                        dtGrid.Columns.Add("CreatedBy");
                        dtGrid.Columns.Add("CreatedDate");
                        dtGrid.Columns.Add("UpdatedBy");
                        dtGrid.Columns.Add("UpdateDate");
                        dtGrid.Columns.Add("Isnew");
                        //dtGrid.Columns.Add("Delete");
                        break;

                    case "oPO":
                    case "oAPRI":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("VendorCode");
                        dtGrid.Columns.Add("VendorName");
                        dtGrid.Columns.Add("CustomerName");
                        dtGrid.Columns.Add("ItemName");
                        dtGrid.Columns.Add("OrderQty");
                        dtGrid.Columns.Add("Balance");
                        break;

                    case "oPOItems":
                    case "oAPRIItems":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("Dscription");
                        dtGrid.Columns.Add("ItmsGrpNam");
                        dtGrid.Columns.Add("ItmsGrpCod");
                        dtGrid.Columns.Add("Image");
                        dtGrid.Columns.Add("Balance");
                        dtGrid.Columns.Add("U_VehcleNo");
                        dtGrid.Columns.Add("Quantity");
                        dtGrid.Columns.Add("DocEntry");
                        dtGrid.Columns.Add("LineNum");
                        dtGrid.Columns.Add("Objtype");
                        dtGrid.Columns.Add("U_Driver");
                        dtGrid.Columns.Add("DriverCnic");
                        break;
                    case "trnsDispatch":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("CustomerCode");
                        dtGrid.Columns.Add("CustomerName");
                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("ItemName");
                        dtGrid.Columns.Add("Vehicle#");
                        dtGrid.Columns.Add("DriverCNIC");
                        dtGrid.Columns.Add("DriverName");
                        dtGrid.Columns.Add("Status");

                        break;
                    case "trnsDispatchReturn":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("CustomerCode");
                        dtGrid.Columns.Add("CustomerName");
                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("ItemName");
                        dtGrid.Columns.Add("Vehicle#");
                        dtGrid.Columns.Add("DriverCNIC");
                        dtGrid.Columns.Add("DriverName");
                        dtGrid.Columns.Add("Status");
                        break;
                    case "trnsRawMaterial":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("VendorCode");
                        dtGrid.Columns.Add("VendorName");
                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("ItemName");
                        dtGrid.Columns.Add("Vehicle#");
                        dtGrid.Columns.Add("DriverCNIC");
                        dtGrid.Columns.Add("DriverName");
                        dtGrid.Columns.Add("Status");

                        break;

                    case "trnsRawMaterialReturn":
                        dtGrid = new DataTable();
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("DocNum");
                        dtGrid.Columns.Add("DocDate");
                        dtGrid.Columns.Add("VendorCode");
                        dtGrid.Columns.Add("VendorName");
                        dtGrid.Columns.Add("ItemCode");
                        dtGrid.Columns.Add("ItemName");
                        dtGrid.Columns.Add("Vehicle#");
                        dtGrid.Columns.Add("DriverCNIC");
                        dtGrid.Columns.Add("DriverName");
                        dtGrid.Columns.Add("Status");
                        break;

                    case "oBP":
                        dtGrid = new DataTable();
                        //dtGrid.Columns.Add("ID");
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("CardCode");
                        dtGrid.Columns.Add("CardName");
                        break;

                    case "oPOVendor":
                        dtGrid = new DataTable();
                        //dtGrid.Columns.Add("ID");
                        dtGrid.Columns.Add("Serial");
                        dtGrid.Columns.Add("CardCode");
                        dtGrid.Columns.Add("CardName");
                        break;
                        //case "HistoryPrint":
                        //    dtGrid = new DataTable();
                        //    dtGrid.Columns.Add("ID");
                        //    dtGrid.Columns.Add("Serial");
                        //    dtGrid.Columns.Add("VoucherNo");
                        //    dtGrid.Columns.Add("VehicleNo");
                        //    dtGrid.Columns.Add("BuiltyNo");
                        //    dtGrid.Columns.Add("CardCode");
                        //    dtGrid.Columns.Add("CardName");
                        //    break;
                }

            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void LoadTheGrid()
        {
            try
            {
                switch (OpenFor)
                {
                    case "Roles":
                        Roles();
                        break;
                    case "Users":
                        Users();
                        break;
                    case "WBSetting":
                        WBSetting();
                        break;
                    case "sboOGIP":

                        break;
                    case "oItem":
                        if (!flgMultiSelect)
                        {
                            SalesOrderItem();
                            Item();
                        }
                        else
                        {
                            SaleOrderItemMulti2();
                        }
                        break;
                    case "oSaleOrder":
                        SalesOrder();
                        SaleOrder();
                        break;
                    case "oSaleOrderM":
                        SalesOrderM2();
                        //SaleOrder();
                        break;
                    case "oDoOrder":

                        DOOrder();
                        DoOrder();
                        //SelectedValues();
                        break;

                    case "oDoGPRO":

                        GRPOOrder();
                        oGRPOItem();
                        //SelectedValues();
                        break;

                    case "oBulkSealer":

                        LoadSeal();
                        break;

                    case "oPO":

                        PurchaseOrder();
                        POrder();
                        // SelectedValues();
                        break;
                    case "oAPRI":
                        APReservedInvoice();
                        POrder();
                        break;
                    case "oPOItems":
                        PurchaseOrderItem();
                        POItem();
                        break;
                    case "oAPRIItems":
                        APReservedInvoiceItems();
                        POItem();
                        break;
                    case "oDoItems":
                        DOGetItems();
                        DOLoadItems();
                        break;

                    case "oGRPOItems":
                        GRPOGetItems();
                        GRPOLoadItems();
                        break;
                    case "trnsDispatch":
                        GetnPopulateDisptachData();
                        break;
                    case "trnsDispatchReturn":
                        GetnPopulateDisptachReturnData();
                        break;
                    case "trnsRawMaterial":
                        GetnPopulateRawMaterialData();
                        break;
                    case "trnsRawMaterialReturn":
                        GetnPopulateRawMaterialReturnData();
                        break;
                    case "HistoryPrint":
                        //LoadCompletedRecords();
                        break;
                    case "oBP":
                        BPCustomers();
                        break;
                    case "oPOVendor":
                        BPVendors();
                        break;
                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void GetnPopulateDisptachData()
        {
            try
            {
                IEnumerable<TrnsDispatch> Getdata = (from a in oDB.TrnsDispatch
                                                     where a.FlgSecondWeight == true
                                                     && a.FlgPosted == null
                                                     select a);
                Int32 Serial = 1;
                foreach (var item in Getdata)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = item.DocNum;
                    dtRow["DocDate"] = item.FDocDate;
                    dtRow["CustomerCode"] = item.CustomerCode;
                    dtRow["CustomerName"] = item.CustomerName;
                    dtRow["ItemCode"] = item.ItemCode;
                    dtRow["ItemName"] = item.ItemName;
                    dtRow["Vehicle#"] = item.VehicleNum;
                    dtRow["DriverCNIC"] = item.DriverCNIC;
                    dtRow["DriverName"] = item.DriverName;
                    if (item.FlgPosted == true)
                    {
                        dtRow["Status"] = "Posted";
                    }
                    else
                    {
                        dtRow["Status"] = "Unposted";
                    }

                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GetnPopulateDisptachReturnData()
        {
            try
            {
                IEnumerable<TrnsDispatchReturn> Getdata = (from a in oDB.TrnsDispatchReturn
                                                           where a.FlgSecondWeight == true
                                                            && a.FlgPosted == null
                                                           select a);
                Int32 Serial = 1;
                foreach (var item in Getdata)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = item.DocNum;
                    dtRow["DocDate"] = item.FDocDate;
                    dtRow["CustomerCode"] = item.CustomerCode;
                    dtRow["CustomerName"] = item.CustomerName;
                    dtRow["ItemCode"] = item.ItemCode;
                    dtRow["ItemName"] = item.ItemName;
                    dtRow["Vehicle#"] = item.VehicleNum;
                    dtRow["DriverCNIC"] = item.DriverCNIC;
                    dtRow["DriverName"] = item.DriverName;
                    if (item.FlgPosted == true)
                    {
                        dtRow["Status"] = "Posted";
                    }
                    else
                    {
                        dtRow["Status"] = "Unposted";
                    }

                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GetnPopulateRawMaterialData()
        {
            try
            {
                IEnumerable<TrnsRawMaterial> Getdata = (from a in oDB.TrnsRawMaterial
                                                        where a.FlgSecondWeight == true
                                                         && a.FlgPosted == null
                                                        select a);
                Int32 Serial = 1;
                foreach (var item in Getdata)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = item.DocNum;
                    dtRow["DocDate"] = item.FDocDate;
                    dtRow["VendorCode"] = item.VendorCode;
                    dtRow["VendorName"] = item.VendorName;
                    dtRow["ItemCode"] = item.ItemCode;
                    dtRow["ItemName"] = item.ItemName;
                    dtRow["Vehicle#"] = item.VehicleNum;
                    dtRow["DriverCNIC"] = item.DriverCNIC;
                    dtRow["DriverName"] = item.DriverName;
                    if (item.FlgPosted == true)
                    {
                        dtRow["Status"] = "Posted";
                    }
                    else
                    {
                        dtRow["Status"] = "Unposted";
                    }

                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GetnPopulateRawMaterialReturnData()
        {
            try
            {
                IEnumerable<TrnsRawMaterialReturn> Getdata = (from a in oDB.TrnsRawMaterialReturn
                                                              where a.FlgSecondWeight == true
                                                               && a.FlgPosted != true || a.FlgPosted == null
                                                              select a);
                Int32 Serial = 1;
                foreach (var item in Getdata)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = item.DocNum;
                    dtRow["DocDate"] = item.FDocDate;
                    dtRow["VendorCode"] = item.VendorCode;
                    dtRow["VendorName"] = item.VendorName;
                    dtRow["ItemCode"] = item.ItemCode;
                    dtRow["ItemName"] = item.ItemName;
                    dtRow["Vehicle#"] = item.VehicleNum;
                    dtRow["DriverCNIC"] = item.DriverCNIC;
                    dtRow["DriverName"] = item.DriverName;
                    if (item.FlgPosted == true)
                    {
                        dtRow["Status"] = "Posted";
                    }
                    else
                    {
                        dtRow["Status"] = "Unposted";
                    }

                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void DOGetItems()
        {
            try
            {
                string strQuery = @"Select 
				
                A1.LineNum,--baseline
                A1.DocEntry,--baseentry
                A.Objtype,-- basetype
				A1.ItemCode,
				A1.Dscription,
				A3.ItmsGrpNam,
		        A3.ItmsGrpCod,
				ISNULL((Select Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + T0.PicturName as 'Picture' from OITM T0
						Where T0.ItemCode = A1.ItemCode),Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + 'NA.png') as 'Image',

				A1.Quantity,
				CASE WHEN A1.Dscription Like '%Bulk%'
				THEN
				(A1.OpenQty - (
								ISNULL((Select SUM(x.DOQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatchReturn] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.DONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatchReturn] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.DONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				WHEN A1.Dscription Like '%Bag%'
				THEN
				(A1.OpenQty - (
								ISNULL((Select SUM(x.DOQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatchReturn] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.DONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.DOQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatchReturn] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.DONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0))) 
				END as 'Balance',
				A1.unitMsr,
				A1.Price,
				A1.LineTotal,
				A1.U_VehcleNo,
				A1.U_Driver,
				A1.U_Dcnic as 'DriverCnic'

from 
				ODLN A
				Inner Join DLN1 A1 on A.DocEntry = A1.DocEntry
				Inner Join OITM A2 on A2.ItemCode = A1.ItemCode
				Inner Join OITB A3 on A3.ItmsGrpCod = A2.ItmsGrpCod
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
				and A.DocNum = '" + Program.DocNum + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtDOItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void DOLoadItems()
        {
            try
            {


                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtDOItems.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;

                    dtRow["ItemCode"] = dtDetail["ItemCode"];
                    dtRow["Dscription"] = dtDetail["Dscription"];
                    dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    dtRow["Image"] = dtDetail["Image"];
                    dtRow["Balance"] = dtDetail["Balance"];
                    dtRow["Quantity"] = dtDetail["Quantity"];
                    dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    dtRow["U_Driver"] = dtDetail["U_Driver"];
                    dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                grdOpen.Columns["ItmsGrpNam"].IsVisible = false;
                grdOpen.Columns["ItmsGrpCod"].IsVisible = false;
                grdOpen.Columns["Image"].IsVisible = false;
                grdOpen.Columns["DocEntry"].IsVisible = false;
                grdOpen.Columns["LineNum"].IsVisible = false;
                grdOpen.Columns["Objtype"].IsVisible = false;


            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GRPOGetItems()
        {
            try
            {
                string strQuery = @"Select 
				
                A1.LineNum,--baseline
                A1.DocEntry,--baseentry
                A.Objtype,-- basetype
				A1.ItemCode,
				A1.Dscription,
				A3.ItmsGrpNam,
		        A3.ItmsGrpCod,
				ISNULL((Select Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + T0.PicturName as 'Picture' from OITM T0
						Where T0.ItemCode = A1.ItemCode),Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + 'NA.png') as 'Image',

				A1.Quantity,
				(A1.OpenQty - (ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsRawMaterialReturn] x Where x.VendorCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.GRPONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				as 'Balance',
				A1.unitMsr,
				A1.Price,
				A1.LineTotal,
				A1.U_VehcleNo,
				A1.U_Driver,
				A1.U_Dcnic as 'DriverCnic'

from 
				OPDN A
				Inner Join PDN1 A1 on A.DocEntry = A1.DocEntry
				Inner Join OITM A2 on A2.ItemCode = A1.ItemCode
				Inner Join OITB A3 on A3.ItmsGrpCod = A2.ItmsGrpCod
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
				and A.DocNum = '" + Program.DocNum + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtGrpoItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GRPOLoadItems()
        {
            try
            {


                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtGrpoItems.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;

                    dtRow["ItemCode"] = dtDetail["ItemCode"];
                    dtRow["Dscription"] = dtDetail["Dscription"];
                    dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    dtRow["Image"] = dtDetail["Image"];
                    dtRow["Balance"] = dtDetail["Balance"];
                    dtRow["Quantity"] = dtDetail["Quantity"];
                    dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    dtRow["U_Driver"] = dtDetail["U_Driver"];
                    dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                grdOpen.Columns["ItmsGrpNam"].IsVisible = false;
                grdOpen.Columns["ItmsGrpCod"].IsVisible = false;
                grdOpen.Columns["Image"].IsVisible = false;
                grdOpen.Columns["DocEntry"].IsVisible = false;
                grdOpen.Columns["LineNum"].IsVisible = false;
                grdOpen.Columns["Objtype"].IsVisible = false;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SelectedValues()
        {
            try
            {
                switch (OpenFor)
                {
                    case "Roles":
                    case "Users":
                    case "WBSetting":
                    case "sboOGIP":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            SelectedObjectID = grdOpen.MasterTemplate.SelectedRows[0].Cells["ID"].Value.ToString();
                            //SelectedObjectIDComplex = grdOpen.MasterTemplate.SelectedRows[0].Cells["VoucherNo"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "sboRoshan":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            SelectedObjectID = grdOpen.MasterTemplate.SelectedRows[0].Cells["ID"].Value.ToString();
                            SelectedObjectIDComplex = grdOpen.MasterTemplate.SelectedRows[0].Cells["VoucherNo"].Value.ToString();
                            flgXSlip = Convert.ToBoolean(grdOpen.MasterTemplate.SelectedRows[0].Cells["flgXSlip"].Value);
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oSaleOrder":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            DocNum = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocNum"].Value.ToString();
                            DocDate = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocDate"].Value.ToString();
                            //ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            //ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            //ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            //Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            CustomerCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerCode"].Value.ToString();
                            CustomerName = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerName"].Value.ToString();
                            ItemName = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemName"].Value.ToString();
                            Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["OrderQty"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oSaleOrderM":
                        if (grdOpen.Rows.Count > 0)
                        {
                            for (int i = 0; i < grdOpen.Rows.Count; i++)
                            {
                                bool Selected = Convert.ToBoolean(grdOpen.Rows[i].Cells["Select"].Value);
                                if (Selected)
                                {
                                    SaleOrderData oNew = new SaleOrderData();
                                    DocDate = grdOpen.Rows[i].Cells["DocDate"].Value.ToString();
                                    //ItmsGrpCod = grdOpen.Rows[i].Cells["ItmsGrpCod"].Value.ToString();
                                    //ItmsGrpNam = grdOpen.Rows[i].Cells["ItmsGrpNam"].Value.ToString();
                                    oNew.SBRNum = grdOpen.Rows[i].Cells["DocNum"].Value.ToString();
                                    oNew.Order = Convert.ToDecimal(grdOpen.Rows[i].Cells["OrderQty"].Value);
                                    oNew.Balance = Convert.ToDecimal(grdOpen.Rows[i].Cells["Balance"].Value);
                                    oData.Add(oNew);
                                }
                            }

                            //ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            //Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            //DocDate = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocDate"].Value.ToString();
                            //CustomerCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerCode"].Value.ToString();
                            //CustomerName = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerName"].Value.ToString();
                            //ItemName = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemName"].Value.ToString();
                            //Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["OrderQty"].Value.ToString();
                            //Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();

                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oPO":
                    case "oAPRI":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            DocNum = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocNum"].Value.ToString();

                            DocDate = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocDate"].Value.ToString();
                            //ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            //ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            //ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            //Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            VendorCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["VendorCode"].Value.ToString();
                            VendorName = grdOpen.MasterTemplate.SelectedRows[0].Cells["VendorName"].Value.ToString();
                            ItemName = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemName"].Value.ToString();
                            Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["OrderQty"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            //Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            //U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            //DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            //U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            //Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            //Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                        }



                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;


                    case "oDoGPRO":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            DocNum = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocNum"].Value.ToString();

                            DocDate = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocDate"].Value.ToString();
                            //ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            //ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            //ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            //Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            VendorCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["VendorCode"].Value.ToString();
                            VendorName = grdOpen.MasterTemplate.SelectedRows[0].Cells["VendorName"].Value.ToString();
                            //Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            //U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            //DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            //U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            //Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            //Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                        }



                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oDoOrder":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            DocNum = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocNum"].Value.ToString();

                            DocDate = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocDate"].Value.ToString();
                            //ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            //ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            //ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            //Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            CustomerCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerCode"].Value.ToString();
                            CustomerName = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerName"].Value.ToString();
                            //Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            //U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            //DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            //U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            //Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            //Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                        }



                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;

                    //case "oDoItems":
                    //    dtGrid = new DataTable();
                    //    dtGrid.Columns.Add("Serial");

                    //    dtGrid.Columns.Add("ItemCode");
                    //    dtGrid.Columns.Add("Dscription");
                    //    dtGrid.Columns.Add("ItmsGrpNam");
                    //    dtGrid.Columns.Add("ItmsGrpCod");
                    //    dtGrid.Columns.Add("Image");
                    //    dtGrid.Columns.Add("Balance");
                    //    dtGrid.Columns.Add("U_VehcleNo");
                    //    dtGrid.Columns.Add("Quantity");
                    //    dtGrid.Columns.Add("U_Driver");
                    //    dtGrid.Columns.Add("DriverCnic");
                    //    break;
                    //case "oDoItems":
                    //    if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                    //    {
                    //        DocNum = grdOpen.MasterTemplate.SelectedRows[0].Cells["Serial"].Value.ToString();

                    //        DocDate = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocDate"].Value.ToString();
                    //        //ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                    //        //ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                    //        //ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                    //        //Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                    //        CustomerCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerCode"].Value.ToString();
                    //        CustomerName = grdOpen.MasterTemplate.SelectedRows[0].Cells["CustomerName"].Value.ToString();
                    //        //Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                    //        //U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                    //        //DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                    //        //U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                    //        //Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                    //        //Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                    //    }

                    case "oGRPOItems":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                            Program.PRBaseEntry = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocEntry"].Value.ToString();
                            Program.PRBaseLine = grdOpen.MasterTemplate.SelectedRows[0].Cells["LineNum"].Value.ToString();
                            Program.PRBaseType = grdOpen.MasterTemplate.SelectedRows[0].Cells["Objtype"].Value.ToString();

                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oDoItems":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                            Program.DRBaseEntry = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocEntry"].Value.ToString();
                            Program.DRBaseLine = grdOpen.MasterTemplate.SelectedRows[0].Cells["LineNum"].Value.ToString();
                            Program.DRBaseType = grdOpen.MasterTemplate.SelectedRows[0].Cells["Objtype"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oItem":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                            Program.DBaseEntry = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocEntry"].Value.ToString();
                            Program.DBaseLine = grdOpen.MasterTemplate.SelectedRows[0].Cells["LineNum"].Value.ToString();
                            Program.DBaseType = grdOpen.MasterTemplate.SelectedRows[0].Cells["Objtype"].Value.ToString();
                        }

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        //frmdispatch.daySeries(); 
                        break;

                    case "oPOItems":
                    case "oAPRIItems":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {

                            ItemCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItemCode"].Value.ToString();
                            ItmsGrpCod = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpCod"].Value.ToString();
                            ItmsGrpNam = grdOpen.MasterTemplate.SelectedRows[0].Cells["ItmsGrpNam"].Value.ToString();
                            Dscription = grdOpen.MasterTemplate.SelectedRows[0].Cells["Dscription"].Value.ToString();
                            Quantity = grdOpen.MasterTemplate.SelectedRows[0].Cells["Quantity"].Value.ToString();
                            U_VehcleNo = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_VehcleNo"].Value.ToString();
                            DriverCnic = grdOpen.MasterTemplate.SelectedRows[0].Cells["DriverCnic"].Value.ToString();
                            U_Driver = grdOpen.MasterTemplate.SelectedRows[0].Cells["U_Driver"].Value.ToString();
                            Balance = grdOpen.MasterTemplate.SelectedRows[0].Cells["Balance"].Value.ToString();
                            Image = grdOpen.MasterTemplate.SelectedRows[0].Cells["Image"].Value.ToString();
                            Program.PBaseEntry = grdOpen.MasterTemplate.SelectedRows[0].Cells["DocEntry"].Value.ToString();
                            Program.PBaseLine = grdOpen.MasterTemplate.SelectedRows[0].Cells["LineNum"].Value.ToString();
                            Program.PBaseType = grdOpen.MasterTemplate.SelectedRows[0].Cells["Objtype"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oBulkSealer":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            SelectedObjectID = grdOpen.MasterTemplate.SelectedRows[0].Cells["CardCode"].Value.ToString();
                            SelectedObjectIDComplex = grdOpen.MasterTemplate.SelectedRows[0].Cells["CardName"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "HistoryPrint":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            SourceDocNum = grdOpen.MasterTemplate.SelectedRows[0].Cells["VoucherNo"].Value.ToString();
                            string critaria = " WHERE VoucherNo IN ('" + SourceDocNum + "') ";
                            string PldFor = flgToggle == false ? "INWARD" : "OUTWARD";
                            Program.OpenLayout(PldFor, critaria, PldFor + " " + SourceDocNum);
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oBP":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            CustomerCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["CardCode"].Value.ToString();
                            CustomerName = grdOpen.MasterTemplate.SelectedRows[0].Cells["CardName"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case "oPOVendor":
                        if (grdOpen.MasterTemplate.SelectedRows.Count > 0)
                        {
                            VendorCode = grdOpen.MasterTemplate.SelectedRows[0].Cells["CardCode"].Value.ToString();
                            VendorName = grdOpen.MasterTemplate.SelectedRows[0].Cells["CardName"].Value.ToString();
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private void Roles()
        {
            try
            {
                IEnumerable<CnfRoles> oCollection = from a in oDB.CnfRoles select a;
                Int32 Serial = 1;
                foreach (CnfRoles Row in oCollection)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["ID"] = Row.ID;
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["RolesName"] = Row.RoleName;
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.EnableFiltering = true;
                grdOpen.ReadOnly = true;
                grdOpen.MasterTemplate.Columns["ID"].IsVisible = false;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void Users()
        {
            try
            {
                IEnumerable<MstUsers> oCollection = from a in oDB.MstUsers select a;
                Int32 Serial = 1;
                foreach (MstUsers Row in oCollection)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["ID"] = Row.UserCode;
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["UserName"] = Row.UserCode;
                    dtRow["UserCode"] = Row.UserName;
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.MasterTemplate.Columns["ID"].IsVisible = false;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void WBSetting()
        {
            try
            {
                IEnumerable<MstWeighBridge> oCollection = from a in oDB.MstWeighBridge select a;
                Int32 Serial = 1;
                foreach (MstWeighBridge Row in oCollection)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["ID"] = Row.ID;
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["WBCode"] = Row.WBCode;
                    dtRow["MachineIP"] = Row.MachineIP;
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.MasterTemplate.Columns["ID"].IsVisible = false;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void Item()
        {
            try
            {
                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtSOrder.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;

                    dtRow["ItemCode"] = dtDetail["ItemCode"];
                    dtRow["Dscription"] = dtDetail["Dscription"];
                    dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    dtRow["Image"] = dtDetail["Image"];
                    dtRow["Balance"] = dtDetail["Balance"];
                    dtRow["Quantity"] = dtDetail["Quantity"];
                    dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];
                    dtRow["DocEntry"] = dtDetail["DocEntry"].ToString();
                    dtRow["LineNum"] = dtDetail["LineNum"].ToString();
                    dtRow["Objtype"] = dtDetail["Objtype"].ToString();
                    dtRow["U_Driver"] = dtDetail["U_Driver"];
                    dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                grdOpen.Columns["ItmsGrpNam"].IsVisible = false;
                grdOpen.Columns["ItmsGrpCod"].IsVisible = false;
                grdOpen.Columns["Image"].IsVisible = false;
                grdOpen.Columns["DocEntry"].IsVisible = false;
                grdOpen.Columns["LineNum"].IsVisible = false;
                grdOpen.Columns["Objtype"].IsVisible = false;

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void POItem()
        {
            try
            {


                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtPOrder.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;

                    dtRow["ItemCode"] = dtDetail["ItemCode"];
                    dtRow["Dscription"] = dtDetail["Dscription"];
                    dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    dtRow["Image"] = dtDetail["Image"];
                    dtRow["Balance"] = dtDetail["Balance"];
                    dtRow["Quantity"] = dtDetail["Quantity"];
                    dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    dtRow["U_Driver"] = dtDetail["U_Driver"];
                    dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                grdOpen.Columns["ItmsGrpNam"].IsVisible = false;
                grdOpen.Columns["ItmsGrpCod"].IsVisible = false;
                grdOpen.Columns["Image"].IsVisible = false;
                grdOpen.Columns["DocEntry"].IsVisible = false;
                grdOpen.Columns["LineNum"].IsVisible = false;
                grdOpen.Columns["Objtype"].IsVisible = false;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SaleOrder()
        {
            try
            {
                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtSOrder.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    if (flgMultiSelect)
                    {
                        dtRow["Select"] = false;
                    }
                    dtRow["DocNum"] = dtDetail["DocNum"];
                    dtRow["DocDate"] = dtDetail["DocDate"];
                    dtRow["CustomerCode"] = dtDetail["CustomerCode"];
                    dtRow["CustomerName"] = dtDetail["CustomerName"];
                    dtRow["ItemName"] = dtDetail["ItemName"];
                    dtRow["OrderQty"] = dtDetail["OrderQty"];
                    dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["ItemCode"] = dtDetail["ItemCode"];
                    //dtRow["Dscription"] = dtDetail["Dscription"];
                    //dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    //dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    //dtRow["Image"] = dtDetail["Image"];
                    //dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["Quantity"] = dtDetail["Quantity"];
                    //dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    //dtRow["U_Driver"] = dtDetail["U_Driver"];
                    //dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                if (flgMultiSelect)
                {
                    grdOpen.MultiSelect = true;
                    grdOpen.ReadOnly = false;
                    grdOpen.MasterTemplate.Columns["Serial"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["DocNum"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["DocDate"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["CustomerCode"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["CustomerName"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["ItemName"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["OrderQty"].ReadOnly = true;
                    grdOpen.MasterTemplate.Columns["Balance"].ReadOnly = true;
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void DoOrder()
        {
            try
            {


                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtDoOrder.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = dtDetail["DocNum"];
                    dtRow["DocDate"] = dtDetail["DocDate"];
                    dtRow["CustomerCode"] = dtDetail["CustomerCode"];
                    dtRow["CustomerName"] = dtDetail["CustomerName"];
                    //dtRow["ItemCode"] = dtDetail["ItemCode"];
                    //dtRow["Dscription"] = dtDetail["Dscription"];
                    //dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    //dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    //dtRow["Image"] = dtDetail["Image"];
                    //dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["Quantity"] = dtDetail["Quantity"];
                    //dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    //dtRow["U_Driver"] = dtDetail["U_Driver"];
                    //dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void oGRPOItem()
        {
            try
            {


                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtGproOrder.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = dtDetail["DocNum"];
                    dtRow["DocDate"] = dtDetail["DocDate"];
                    dtRow["VendorCode"] = dtDetail["VendorCode"];
                    dtRow["VendorName"] = dtDetail["VendorName"];
                    //dtRow["ItemCode"] = dtDetail["ItemCode"];
                    //dtRow["Dscription"] = dtDetail["Dscription"];
                    //dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    //dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    //dtRow["Image"] = dtDetail["Image"];
                    //dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["Quantity"] = dtDetail["Quantity"];
                    //dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    //dtRow["U_Driver"] = dtDetail["U_Driver"];
                    //dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void POrder()
        {
            try
            {


                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtPOrder.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["DocNum"] = dtDetail["DocNum"];
                    dtRow["DocDate"] = dtDetail["DocDate"];
                    dtRow["VendorCode"] = dtDetail["VendorCode"];
                    dtRow["VendorName"] = dtDetail["VendorName"];
                    dtRow["ItemName"] = dtDetail["ItemName"];
                    dtRow["OrderQty"] = dtDetail["OrderQty"];
                    dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["ItemCode"] = dtDetail["ItemCode"];
                    //dtRow["Dscription"] = dtDetail["Dscription"];
                    //dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    //dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    //dtRow["Image"] = dtDetail["Image"];
                    //dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["Quantity"] = dtDetail["Quantity"];
                    //dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    //dtRow["U_Driver"] = dtDetail["U_Driver"];
                    //dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void BPCustomers()
        {
            try
            {
                string strQuery = string.Empty;
                strQuery = @"SELECT * FROM [dbo].[OCRD] WHERE [CardType] = 'C'";
                DataTable dttemp = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);
                Int32 Serial = 1;
                foreach (DataRow dr in dttemp.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["CardCode"] = Convert.ToString(dr["CardCode"]);
                    dtRow["CardName"] = Convert.ToString(dr["CardName"]);
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void BPVendors()
        {
            try
            {
                string strQuery = string.Empty;
                strQuery = @"
                SELECT DISTINCT a.[CardCode] , a.[CardName]
                FROM 
	                OPOR a INNER JOIN POR1 b ON a.[DocEntry] = b.[DocEntry]
	                INNER JOIN OITM c ON b.[ItemCode] = c.[ItemCode]
                WHERE
	                a.[DocStatus] = 'O'
	                AND a.[CANCELED] = 'N'
	                AND b.[OpenQty] > 0
	                AND c.[QryGroup1] = 'Y'


                Union 

                SELECT DISTINCT a.[CardCode] , a.[CardName]
                FROM 
	                OPCH a INNER JOIN PCH1 b ON a.[DocEntry] = b.[DocEntry]
	                INNER JOIN OITM c ON b.[ItemCode] = c.[ItemCode]
                WHERE
	                a.[DocStatus] = 'O'
	                AND a.[CANCELED] = 'N'
	                AND b.[OpenQty] > 0
	                AND c.[QryGroup3] = 'Y'
                ORDER BY
	                a.[CardCode] 
                                ";
                DataTable dttemp = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);
                Int32 Serial = 1;
                foreach (DataRow dr in dttemp.Rows)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["CardCode"] = Convert.ToString(dr["CardCode"]);
                    dtRow["CardName"] = Convert.ToString(dr["CardName"]);
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SalesOrder()
        {
            try
            {
                string strQuery = @"Select 
				A.DocNum,
				A.DocDate,

				A.CardCode as CustomerCode,
				A.CardName as CustomerName,
                B.Dscription as ItemName,
				B.Quantity as OrderQty,
				CASE WHEN B.Dscription Like '%Bulk%'
				THEN
				(B.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				WHEN B.Dscription Like '%Bag%'
				THEN
				(B.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0))) 
				END as 'Balance'

				

from 
				ORDR A
			    Inner join RDR1 B On A.DocEntry = B.DocEntry
				Inner Join OITM C On B.ItemCode = C.ItemCode
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
                and QryGroup1 = 'Y'
                and B.OpenQty > 0";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtSOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SalesOrderM()
        {
            try
            {
                string strQuery = @"Select 
				A.DocNum,
				A.DocDate,

				A.CardCode as CustomerCode,
				A.CardName as CustomerName,
                B.Dscription as ItemName,
				B.Quantity as OrderQty,
				CASE WHEN B.Dscription Like '%Bulk%'
				THEN
				(B.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				WHEN B.Dscription Like '%Bag%'
				THEN
				(B.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0))) 
				END as 'Balance'

				

from 
				ORDR A
			    Inner join RDR1 B On A.DocEntry = B.DocEntry
				Inner Join OITM C On B.ItemCode = C.ItemCode
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
                and QryGroup1 = 'Y'
                and B.OpenQty > 0
                and A.CardCode = '" + CustomerCode + @"'
                and B.ItemCode = '" + ItemCode + @"'
";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtSOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SalesOrderM2()
        {
            try
            {
                string strQuery = @"
				SELECT  A.DocNum ,
		                A.DocDate,
		                A.CardCode CustomerCode,
		                A.CardName CustomerName,
                        B.ItemCode ,
                        B.Dscription ItemName,
                        B.OpenQty ,
                        B.Quantity OrderQty
		
                FROM    dbo.ORDR A
                        INNER JOIN dbo.RDR1 B ON A.DocEntry = B.DocEntry
                        INNER JOIN dbo.OITM C ON B.ItemCode = C.ItemCode

                WHERE   A.CardCode = '" + CustomerCode + @"'
                        AND B.ItemCode = '" + ItemCode + @"'
                        AND A.CANCELED = 'N'
                        AND A.DocStatus = 'O'
                        AND C.QryGroup1 = 'Y'
                        AND B.OpenQty > 0
                ORDER BY A.DocNum
";
                dtSOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

                foreach (DataRow dr in dtSOrder.Rows)
                {
                    string item, desc, docnum;
                    decimal openqty = 0, middoc = 0, finaldoc = 0;
                    decimal? finaldoc1 = 0;
                    item = Convert.ToString(dr["ItemCode"]);
                    desc = Convert.ToString(dr["ItemName"]);
                    docnum = Convert.ToString(dr["DocNum"]);
                    openqty = Convert.ToDecimal(dr["OpenQty"]);
                    if (desc.Contains("Bulk"))
                    {
                        //middoc = (from a in oDB.TrnsDispatch
                        //          where a.ItemCode == item
                        //          && a.CustomerCode == CustomerCode
                        //          && a.SBRNum.Contains(docnum)
                        //          && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //          && a.FlgSecondWeight == false
                        //          select a.OrderQuantity).Sum().GetValueOrDefault();
                        //finaldoc = (from a in oDB.TrnsDispatch
                        //            where a.ItemCode == item
                        //            && a.CustomerCode == CustomerCode
                        //            && a.SBRNum.Contains(docnum)
                        //            && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //            && a.FlgSecondWeight == true
                        //            select a.NetWeightTon).Sum().GetValueOrDefault();
                        oDB.GetSumOpenValueSO(docnum, CustomerCode, ItemCode, ref finaldoc1);
                        finaldoc = finaldoc1.GetValueOrDefault();
                        dr["OpenQty"] = openqty - (middoc + finaldoc);
                    }
                    else
                    {
                        //middoc = (from a in oDB.TrnsDispatch
                        //          where a.ItemCode == item
                        //          && a.CustomerCode == CustomerCode
                        //          && a.SBRNum.Contains(docnum)
                        //          && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //          && a.FlgSecondWeight == false
                        //          select a.OrderQuantity).Sum().GetValueOrDefault();
                        //finaldoc = (from a in oDB.TrnsDispatch
                        //            where a.ItemCode == item
                        //            && a.CustomerCode == CustomerCode
                        //            && a.SBRNum.Contains(docnum)
                        //            && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //            && a.FlgSecondWeight == true
                        //            select a.OrderQuantity).Sum().GetValueOrDefault();
                        oDB.GetSumOpenValueSO(docnum, CustomerCode, ItemCode, ref finaldoc1);
                        finaldoc = finaldoc1.GetValueOrDefault();
                        dr["OpenQty"] = openqty - (middoc + finaldoc);
                    }
                }

                Int32 Serial = 1;
                foreach (DataRow dtDetail in dtSOrder.Rows)
                {
                    if (Convert.ToDecimal(dtDetail["OpenQty"]) == 0)
                        continue;
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["Select"] = false;
                    dtRow["DocNum"] = dtDetail["DocNum"];
                    dtRow["DocDate"] = dtDetail["DocDate"];
                    dtRow["CustomerCode"] = dtDetail["CustomerCode"];
                    dtRow["CustomerName"] = dtDetail["CustomerName"];
                    dtRow["ItemName"] = dtDetail["ItemName"];
                    dtRow["OrderQty"] = dtDetail["OrderQty"];
                    dtRow["Balance"] = dtDetail["OpenQty"];
                    //dtRow["ItemCode"] = dtDetail["ItemCode"];
                    //dtRow["Dscription"] = dtDetail["Dscription"];
                    //dtRow["ItmsGrpNam"] = dtDetail["ItmsGrpNam"];
                    //dtRow["ItmsGrpCod"] = dtDetail["ItmsGrpCod"];
                    //dtRow["Image"] = dtDetail["Image"];
                    //dtRow["Balance"] = dtDetail["Balance"];
                    //dtRow["Quantity"] = dtDetail["Quantity"];
                    //dtRow["U_VehcleNo"] = dtDetail["U_VehcleNo"];

                    //dtRow["U_Driver"] = dtDetail["U_Driver"];
                    //dtRow["DriverCnic"] = dtDetail["DriverCnic"];
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;

                grdOpen.MultiSelect = true;
                grdOpen.ReadOnly = false;
                grdOpen.AllowAddNewRow = false;
                grdOpen.MasterTemplate.Columns["Serial"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["DocNum"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["DocDate"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["CustomerCode"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["CustomerName"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["ItemName"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["OrderQty"].ReadOnly = true;
                grdOpen.MasterTemplate.Columns["Balance"].ReadOnly = true;

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void DOOrder()
        {
            try
            {
                string strQuery = @"Select 
				A.DocNum,
				A.DocDate,

				A.CardCode as CustomerCode,
				A.CardName as CustomerName,
                B.Dscription as ItemName,
				B.Quantity as OrderQty,
				B.OpenQty as Balance


				

from 
				ODLN A
                Inner join DLN1 B On A.DocEntry = B.DocEntry
				Inner Join OITM C On B.ItemCode = C.ItemCode
			
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
                and QryGroup1 = 'Y'
                and B.OpenQty > 0";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtDoOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GRPOOrder()
        {
            try
            {
                string strQuery = @"Select 
				A.DocNum,
				A.DocDate,

				A.CardCode as VendorCode,
				A.CardName as VendorName,
                B.Dscription as ItemName,
				B.Quantity as OrderQty,
				B.OpenQty as Balance


				

from 
				OPDN A
                Inner join PDN1 B On A.DocEntry = B.DocEntry
				Inner Join OITM C On B.ItemCode = C.ItemCode
			
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
               and QryGroup1 = 'Y'
                and B.OpenQty > 0";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtGproOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SalesOrderItem()
        {
            try
            {
                string strQuery = @"Select 
				
                A1.LineNum,--baseline
                A1.DocEntry,--baseentry
                A.Objtype,-- basetype
				A1.ItemCode,
				A1.Dscription,
				A3.ItmsGrpNam,
		        A3.ItmsGrpCod,
				ISNULL((Select Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + T0.PicturName as 'Picture' from OITM T0
						Where T0.ItemCode = A1.ItemCode),Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + 'NA.png') as 'Image',

				A1.Quantity,
				CASE WHEN A1.Dscription Like '%Bulk%'
				THEN
				(A1.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				WHEN A1.Dscription Like '%Bag%'
				THEN
				(A1.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0))) 
				END as 'Balance',
				A1.unitMsr,
				A1.Price,
				A1.LineTotal,
				A1.U_VehcleNo,
				A1.U_Driver,
				A1.U_Dcnic as 'DriverCnic'

from 
				ORDR A
				Inner Join RDR1 A1 on A.DocEntry = A1.DocEntry
				Inner Join OITM A2 on A2.ItemCode = A1.ItemCode
				Inner Join OITB A3 on A3.ItmsGrpCod = A2.ItmsGrpCod
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
				and A.DocNum = '" + Program.DocNum + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtSOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SalesOrderItemMulti()
        {
            try
            {
                string strQuery = @"Select 
				
                A1.LineNum,--baseline
                A1.DocEntry,--baseentry
                A.Objtype,-- basetype
				A1.ItemCode,
				A1.Dscription,
				A3.ItmsGrpNam,
		        A3.ItmsGrpCod,
				ISNULL((Select Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + T0.PicturName as 'Picture' from OITM T0
						Where T0.ItemCode = A1.ItemCode),Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + 'NA.png') as 'Image',

				A1.Quantity,
				CASE WHEN A1.Dscription Like '%Bulk%'
				THEN
				(A1.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				WHEN A1.Dscription Like '%Bag%'
				THEN
				(A1.OpenQty - (
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '0'),0)
									+ 
								ISNULL((Select SUM(x.OrderQuantity) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsDispatch] x Where x.CustomerCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.SBRNum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0))) 
				END as 'Balance',
				A1.unitMsr,
				A1.Price,
				A1.LineTotal,
				A1.U_VehcleNo,
				A1.U_Driver,
				A1.U_Dcnic as 'DriverCnic'

from 
				ORDR A
				Inner Join RDR1 A1 on A.DocEntry = A1.DocEntry
				Inner Join OITM A2 on A2.ItemCode = A1.ItemCode
				Inner Join OITB A3 on A3.ItmsGrpCod = A2.ItmsGrpCod
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
                and A.CardCode = '" + CustomerCode + @"'";
                dtSOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SaleOrderItemMulti2()
        {
            try
            {
                string Query = "";
                Query = @"
                SELECT 
	                A.DocNum, B.ItemCode,  B.Dscription, B.OpenQty, B.Quantity, Convert(nvarchar(50), D.ItmsGrpCod) ItmsGrpCod, D.ItmsGrpNam
                FROM	
	                dbo.ORDR A
	                INNER JOIN dbo.RDR1 B ON A.DocEntry = B.DocEntry
	                INNER JOIN dbo.OITM C ON B.ItemCode = C.ItemCode
                    INNER JOIN dbo.OITB D ON C.ItmsGrpCod = D.ItmsGrpCod
                WHERE
	                A.CardCode = '" + CustomerCode + @"'
	                AND A.CANCELED = 'N'
	                AND A.DocStatus = 'O'
	                AND C.QryGroup1 = 'Y'
	                AND B.OpenQty > 0
                ORDER BY
	                B.ItemCode
                ";
                dtSOrder = mFm.ExecuteQueryDt(Query, Program.ConStrSAP);
                foreach (DataRow dr in dtSOrder.Rows)
                {
                    string item, desc, docnum;
                    decimal openqty = 0, middoc = 0, finaldoc = 0;
                    decimal? finaldoc1 = 0;
                    item = Convert.ToString(dr["ItemCode"]);
                    desc = Convert.ToString(dr["Dscription"]);
                    docnum = Convert.ToString(dr["DocNum"]);
                    openqty = Convert.ToDecimal(dr["OpenQty"]);
                    if (desc.Contains("Bulk"))
                    {
                        //middoc = (from a in oDB.TrnsDispatch
                        //          where a.ItemCode == item
                        //          && a.CustomerCode == CustomerCode
                        //          && a.SBRNum.Contains(docnum)
                        //          && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //          && a.FlgSecondWeight == false
                        //          select a.OrderQuantity).Sum().GetValueOrDefault();
                        //finaldoc = (from a in oDB.TrnsDispatch
                        //            where a.ItemCode == item
                        //            && a.CustomerCode == CustomerCode
                        //            && a.SBRNum.Contains(docnum)
                        //            && (a.FlgPosted == null ? false : a.FlgPosted ) == false
                        //            && a.FlgSecondWeight == true
                        //            select a.NetWeightTon).Sum().GetValueOrDefault();
                        oDB.GetSumOpenValueSO(docnum, CustomerCode, item, ref finaldoc1);
                        finaldoc = finaldoc1.GetValueOrDefault();
                        dr["OpenQty"] = openqty - (middoc + finaldoc);
                    }
                    else
                    {
                        //middoc = (from a in oDB.TrnsDispatch
                        //          where a.ItemCode == item
                        //          && a.CustomerCode == CustomerCode
                        //          && a.SBRNum.Contains(docnum)
                        //          && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //          && a.FlgSecondWeight == false
                        //          select a.OrderQuantity).Sum().GetValueOrDefault();
                        //finaldoc = (from a in oDB.TrnsDispatch
                        //            where a.ItemCode == item
                        //            && a.CustomerCode == CustomerCode
                        //            && a.SBRNum.Contains(docnum)
                        //            && (a.FlgPosted == null ? false : a.FlgPosted) == false
                        //            && a.FlgSecondWeight == true
                        //            select a.OrderQuantity).Sum().GetValueOrDefault();
                        oDB.GetSumOpenValueSO(docnum, CustomerCode, item, ref finaldoc1);
                        finaldoc = finaldoc1.GetValueOrDefault();
                        dr["OpenQty"] = openqty - (middoc + finaldoc);
                    }

                }
                var oResult = from a in dtSOrder.AsEnumerable()
                              group a by a.Field<string>("ItemCode") into b
                              select new
                              {
                                  ItemCode = b.FirstOrDefault().Field<string>("ItemCode"),
                                  Dscription = b.FirstOrDefault().Field<string>("Dscription"),
                                  ItmsGrpCod = b.FirstOrDefault().Field<string>("ItmsGrpCod"),
                                  ItmsGrpNam = b.FirstOrDefault().Field<string>("ItmsGrpNam"),
                                  OpenQty = b.Sum(c => c.Field<decimal>("OpenQty")),
                                  Quantity = b.Sum(d => d.Field<decimal>("Quantity"))
                              };
                Int32 Serial = 1;
                //foreach (DataRow dtDetail in dtSOrder.Rows)
                //{
                //    DataRow dtRow = dtGrid.NewRow();
                //    dtRow["Serial"] = Serial;
                //    Serial++;
                //    dtRow["ItemCode"] = dtDetail["ItemCode"];
                //    dtRow["Dscription"] = dtDetail["Dscription"];
                //    dtRow["Balance"] = dtDetail["OpenQty"];
                //    dtRow["Quantity"] = dtDetail["Quantity"];
                //    dtGrid.Rows.Add(dtRow);
                //}
                foreach (var One in oResult)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["Serial"] = Serial;
                    Serial++;
                    dtRow["ItemCode"] = One.ItemCode;
                    dtRow["Dscription"] = One.Dscription;
                    dtRow["Quantity"] = One.Quantity;
                    dtRow["Balance"] = One.OpenQty;
                    dtRow["ItmsGrpCod"] = One.ItmsGrpCod;
                    dtRow["ItmsGrpNam"] = One.ItmsGrpNam;
                    dtGrid.Rows.Add(dtRow);
                }
                grdOpen.DataSource = dtGrid;
                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = true;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                grdOpen.Columns["Quantity"].HeaderText = "OrderQty";
                grdOpen.Columns["ItmsGrpNam"].IsVisible = false;
                grdOpen.Columns["ItmsGrpCod"].IsVisible = false;
                grdOpen.Columns["Image"].IsVisible = false;
                grdOpen.Columns["DocEntry"].IsVisible = false;
                grdOpen.Columns["LineNum"].IsVisible = false;
                grdOpen.Columns["Objtype"].IsVisible = false;
                grdOpen.Columns["U_VehcleNo"].IsVisible = false;
                grdOpen.Columns["U_Driver"].IsVisible = false;
                grdOpen.Columns["DriverCnic"].IsVisible = false;
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private void PurchaseOrderItem()
        {
            try
            {
                string strQuery = @"Select 
				
                A1.LineNum,--baseline
                A1.DocEntry,--baseentry
                A.Objtype,-- basetype
				A1.ItemCode,
				A1.Dscription,
				A3.ItmsGrpNam,
		        A3.ItmsGrpCod,
				ISNULL((Select Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + T0.PicturName as 'Picture' from OITM T0
						Where T0.ItemCode = A1.ItemCode),Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + 'NA.png') as 'Image',

				A1.Quantity,
				(A1.OpenQty - (ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsRawMaterial] x Where x.VendorCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.PONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				as 'Balance',
				A1.unitMsr,
				A1.Price,
				A1.LineTotal,
				A1.U_VehcleNo,
				A1.U_Driver,
				A1.U_Dcnic as 'DriverCnic'

from 
				OPOR A
				Inner Join POR1 A1 on A.DocEntry = A1.DocEntry
				Inner Join OITM A2 on A2.ItemCode = A1.ItemCode
				Inner Join OITB A3 on A3.ItmsGrpCod = A2.ItmsGrpCod
Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
				and A.DocNum = '" + Program.DocNum + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtPOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void APReservedInvoiceItems()
        {
            try
            {
                string strQuery = @"Select 
				
                A1.LineNum,--baseline
                A1.DocEntry,--baseentry
                A.Objtype,-- basetype
				A1.ItemCode,
				A1.Dscription,
				A3.ItmsGrpNam,
		        A3.ItmsGrpCod,
				ISNULL((Select Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + T0.PicturName as 'Picture' from OITM T0
						Where T0.ItemCode = A1.ItemCode),Cast((Select x.BitmapPath From OADP x) as nvarchar(max)) + 'NA.png') as 'Image',

				A1.Quantity,
				(A1.OpenQty - (ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsRawMaterial] x Where x.VendorCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = A1.ItemCode and x.PONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				as 'Balance',
				A1.unitMsr,
				A1.Price,
				A1.LineTotal,
				A1.U_VehcleNo,
				A1.U_Driver,
				A1.U_Dcnic as 'DriverCnic'

from 
				OPCH A
				Inner Join PCH1 A1 on A.DocEntry = A1.DocEntry
				Inner Join OITM A2 on A2.ItemCode = A1.ItemCode
				Inner Join OITB A3 on A3.ItmsGrpCod = A2.ItmsGrpCod
Where
				A.CANCELED = 'N'
                AND A.isIns = 'Y'
				AND A.DocStatus = 'O'
				AND A.DocNum = '" + Program.DocNum + "'";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtPOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void PurchaseOrder()
        {
            try
            {
                string strQuery = @"Select 
				A.DocNum,
				A.DocDate,

				A.CardCode as VendorCode,
				A.CardName as VendorName,
                 B.Dscription as ItemName,
                B.Quantity as OrderQty,
				(B.OpenQty - (ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsRawMaterial] x Where x.VendorCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.PONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				as 'Balance'



                from 
				OPOR A
                Inner join POR1 B On A.DocEntry = B.DocEntry
				Inner Join OITM C On B.ItemCode = C.ItemCode
			
                Where
				A.CANCELED = 'N'
				and A.DocStatus = 'O'
                and QryGroup1 = 'Y'
                and B.OpenQty > 0
                and A.CardCode = '" + VendorCode + @"'
                ";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtPOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void APReservedInvoice()
        {
            try
            {
                string strQuery = @"Select 
				A.DocNum,
				A.DocDate,

				A.CardCode as VendorCode,
				A.CardName as VendorName,
                 B.Dscription as ItemName,
                B.Quantity as OrderQty,
				(B.OpenQty - (ISNULL((Select SUM(x.NetWeightTon) From [" + APPSetting.Default.cfgAPPDB + @"].[dbo].[TrnsRawMaterial] x Where x.VendorCode Collate SQL_Latin1_General_CP850_CI_AS = a.CardCode 
									and x.ItemCode Collate SQL_Latin1_General_CP850_CI_AS = B.ItemCode and x.PONum Collate SQL_Latin1_General_CP850_CI_AS = A.DocNum and ISNULL(x.flgPosted,0) = '0' and ISNULL(x.flgSecondWeight,0) = '1'),0)))
				
				as 'Balance'



                FROM 
				OPCH A
                Inner join PCH1 B On A.DocEntry = B.DocEntry
				Inner Join OITM C On B.ItemCode = C.ItemCode
			
                Where
				A.CANCELED = 'N'
				And A.DocStatus = 'O'
                And QryGroup3 = 'Y'
                And B.OpenQty > 0
                AND A.isIns = 'Y'
                And A.CardCode = '" + VendorCode + @"'
                ";
                //  WHERE dbo.ORDR.DocNum = '" + SourceDocNum + "'";
                dtPOrder = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void SalesReturnItem()
        {
            try
            {
                string strQuery = @"SELECT dbo.RDN1.ItemCode, dbo.OITM.ItemName  
                                    FROM dbo.ORDN INNER JOIN dbo.RDN1 ON dbo.ORDN.DocEntry = dbo.RDN1.DocEntry INNER JOIN dbo.OITM ON RDN1.ItemCode = dbo.OITM.ItemCode 
                                    WHERE dbo.ORDN.DocNum = '" + SourceDocNum + "'";
                dtItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void ARCreditMemoItem()
        {
            try
            {
                string strQuery = @"SELECT dbo.RIN1.ItemCode, dbo.OITM.ItemName  
                                    FROM dbo.ORIN INNER JOIN dbo.RIN1 ON dbo.ORIN.DocEntry = dbo.RIN1.DocEntry INNER JOIN dbo.OITM ON dbo.RIN1.ItemCode = dbo.OITM.ItemCode 
                                    WHERE dbo.ORIN.DocNum = '" + SourceDocNum + "'";
                dtItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        //private void PurchaseOrderItem()
        //{
        //    try
        //    {
        //        string strQuery = @"SELECT dbo.POR1.ItemCode, dbo.OITM.ItemName  
        //                            FROM dbo.OPOR INNER JOIN dbo.POR1 ON dbo.OPOR.DocEntry = dbo.POR1.DocEntry INNER JOIN dbo.OITM ON dbo.POR1.ItemCode = dbo.OITM.ItemCode 
        //                            WHERE dbo.OPOR.DocNum = '" + SourceDocNum + "'";
        //        dtItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        private void PurchaseReturnItem()
        {
            try
            {
                string strQuery = @"SELECT dbo.RPD1.ItemCode, dbo.OITM.ItemName  
                                    FROM dbo.ORPD INNER JOIN dbo.RPD1 ON dbo.ORPD.DocEntry = dbo.RPD1.DocEntry INNER JOIN dbo.OITM ON dbo.RPD1.ItemCode = dbo.OITM.ItemCode 
                                    WHERE dbo.ORPD.DocNum = '" + SourceDocNum + "'";
                dtItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void APCreditMemoItem()
        {
            try
            {
                string strQuery = @"SELECT dbo.RPC1.ItemCode, dbo.OITM.ItemName  
                                    FROM dbo.ORPC INNER JOIN dbo.RPC1 ON dbo.ORPC.DocEntry = dbo.RPC1.DocEntry INNER JOIN dbo.OITM ON dbo.RPC1.ItemCode = dbo.OITM.ItemCode 
                                    WHERE dbo.ORPC.DocNum = '" + SourceDocNum + "'";
                dtItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void AllItems()
        {
            try
            {
                string strQuery = "SELECT ItemCode, ItemName FROM dbo.OITM ";
                dtItems = mFm.ExecuteQueryDt(strQuery, Program.ConStrSAP);
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private Boolean ValidateRecord()
        {
            Boolean retValue = true;
            try
            {
                var GetShiftData = from a in oDB.MstSeal where a.DocNum == Program.FormDocNum && a.FormName == Program.FormName select a;

                foreach (MstSeal data in GetShiftData) // iterating DB values.
                {
                    int j = 0;
                    string SealNum = data.SealNum;
                    for (int h = 0; h < grdOpen.RowCount; h++)// Iterating 1 Grid value to All grid Values.
                    {
                        GridViewRowInfo Row = grdOpen.Rows[h];
                        GridViewCellInfo PCode = Row.Cells["SealNumber"];

                        if (PCode.Value.ToString() == SealNum) // Checking duplicate grid values
                        {
                            j++;
                            if (j > 1) // if record exist in grid more than one
                            {
                                retValue = false;
                                LoadSeal();
                                Program.ExceptionMsg("SealNumber can not be dulpicated.");
                                break;
                            }

                        }
                    }

                    //if (retValue != false)
                    //{
                    //    for (int i = 0; i < grdOpen.RowCount; i++) // Iterating 1 Db value to All grid Values.
                    //    {
                    //        if (retValue != false)
                    //        {
                    //            int k = 0;
                    //            GridViewRowInfo CurrentRow = grdOpen.Rows[i];
                    //            GridViewCellInfo cellCode = CurrentRow.Cells["SealNumber"];
                    //            SealNumber = cellCode.Value.ToString();
                    //            for (int h = 0; h < grdOpen.RowCount; h++)// Iterating 1 Grid value to All grid Values.
                    //            {
                    //                GridViewRowInfo Row = grdOpen.Rows[h];
                    //                GridViewCellInfo PCode = Row.Cells["SealNumber"];

                    //                if (PCode.Value.ToString() == SealNumber) // Checking duplicate grid values
                    //                {
                    //                    k++;
                    //                    if (k > 1) // if record exist in grid more than one
                    //                    {
                    //                        retValue = false;
                    //                        LoadSeal();
                    //                        break;
                    //                    }

                    //                }
                    //            }
                    //            if (SealNumber == data.SealNum) // if record exist in DB more than one
                    //            {
                    //                j++;
                    //                if (j > 1)
                    //                {
                    //                    retValue = false;
                    //                    LoadSeal(); 
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    LoadSeal();
                    //}
                }
            }
            catch (Exception ex)
            {
                retValue = false;
            }
            return retValue;
        }

        private void createSealDt()
        {
            // Program.sealdt = new DataTable();
            Program.sealdt = new DataTable();
            if (Program.sealdt.Columns.Count == 0)
            {
                Program.sealdt.Columns.Add("Isnew");
                Program.sealdt.Columns.Add("ID");
                Program.sealdt.Columns.Add("DocNum");
                Program.sealdt.Columns.Add("SealNumber");
                Program.sealdt.Columns.Add("ItemCode");
                Program.sealdt.Columns.Add("FormName");
                Program.sealdt.Columns.Add("CreatedBy");
                Program.sealdt.Columns.Add("CreatedDate");
                Program.sealdt.Columns.Add("UpdatedBy");
                Program.sealdt.Columns.Add("UpdateDate");

            }
        }

        private DataTable AddSealToDt()
        {
            bool flgSealStore = false;
            try
            {

                // dtGrid.Clear();
                //SelectedValues();
                createSealDt();
                //MstSeal NewMP = new MstSeal();

                //NewMP.CreatedBy = Program.oCurrentUser.UserCode;
                //NewMP.UpdatedBy= Program.oCurrentUser.UserCode;
                //NewMP.CreateDT= DateTime.Now;
                //NewMP.UpdateDT = DateTime.Now;

                for (int i = 0; i < grdOpen.RowCount; i++)
                {
                    DataRow dtRow = Program.sealdt.NewRow();
                    GridViewRowInfo CurrentRow = grdOpen.Rows[i];
                    GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                    dtRow["ID"] = cellID.Value;
                    //GridViewCellInfo cellDocNum = CurrentRow.Cells["DocNum"];
                    dtRow["DocNum"] = Program.FormDocNum;
                    //GridViewCellInfo cellItemCode = CurrentRow.Cells["ItemCode"];
                    //cellItemCode.Value = Program.FormItemCode;

                    dtRow["ItemCode"] = Program.FormItemCode;

                    GridViewCellInfo cellSeal = CurrentRow.Cells["SealNumber"];
                    dtRow["SealNumber"] = cellSeal.Value;


                    dtRow["FormName"] = Program.FormName;

                    GridViewCellInfo cellCreatedBy = CurrentRow.Cells["CreatedBy"];
                    cellCreatedBy.Value = Program.oCurrentUser.UserCode;
                    dtRow["CreatedBy"] = cellCreatedBy.Value;

                    //GridViewCellInfo cellCreatedDate = CurrentRow.Cells["CreatedDate"];
                    //cellCreatedDate.Value = DateTime.Now;
                    dtRow["CreatedDate"] = DateTime.Now;

                    GridViewCellInfo cellUpdateBy = CurrentRow.Cells["UpdatedBy"];
                    cellUpdateBy.Value = Program.oCurrentUser.UserCode;
                    dtRow["UpdatedBy"] = cellUpdateBy.Value;

                    //GridViewCellInfo cellUpdateDate = CurrentRow.Cells["UpdateDate"];
                    //cellUpdateDate.Value = Program.oCurrentUser.UserCode;
                    dtRow["UpdateDate"] = DateTime.Now;


                    //dtGrid.Columns.Add("CreatedBy");
                    //dtGrid.Columns.Add("CreatedDate");
                    //dtGrid.Columns.Add("UpdatedBy");
                    //dtGrid.Columns.Add("UpdateDate");

                    if (string.IsNullOrEmpty(cellSeal.Value.ToString()))
                    {
                        Program.ExceptionMsg("Seal Can't be Empty.");
                        //flgadd = false;
                        break;
                    }
                    GridViewCellInfo Isnew = CurrentRow.Cells["Isnew"];
                    dtRow["Isnew"] = Isnew.Value;

                    //if (Isnew.Value.ToString() == "0")
                    //{
                    //    MstSeal oldMP = oDB.MstSeal.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();

                    //    if (oldMP.SealNum != Convert.ToString(cellSeal.Value))
                    //    {
                    //        oldMP.DocNum = Program.FormDocNum;
                    //        oldMP.SealNum = Convert.ToString(cellSeal.Value);
                    //        oldMP.ItemCode = Program.FormItemCode;
                    //        oldMP.FieldName = Program.FormName;
                    //        oldMP.UpdatedBy= Program.oCurrentUser.UserCode;
                    //        oldMP.UpdateDT = DateTime.Now;
                    //        Program.SuccesesMsg("Record Successfully Updated");
                    //        // flgNoupdate = true;
                    //    }
                    //    else
                    //    {
                    //        //flgNoupdate = false;
                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                    //    NewMP.DocNum = Program.FormDocNum;
                    //    NewMP.SealNum = Convert.ToString(cellSeal.Value);
                    //    NewMP.ItemCode = Program.FormItemCode;
                    //    NewMP.FieldName = Program.FormName;

                    //    //oDB.MstSeal.InsertOnSubmit(NewMP);
                    //    Program.SuccesesMsg("Record Successfully Added");
                    //}
                    // oDB.SubmitChanges();

                    //for (int j = 0; j < grdOpen.RowCount; j++)
                    //{
                    //    DataRow dtRow = dtGrid.NewRow();
                    //    dtRow["DocNum"] = Program.FormDocNum;
                    //    dtRow["ItemCode"] = Program.FormItemCode;
                    //    dtRow["SealNumber"] = Convert.ToString(cellSeal.Value);
                    //    dtRow["CreatedBy"] = DateTime.Now;
                    //    dtRow["CreatedDate"] = Program.oCurrentUser.UserCode;
                    //    dtRow["UpdatedBy"] = DateTime.Now;
                    //    dtRow["UpdateDate"] = Program.oCurrentUser.UserCode;
                    //    if (Isnew.Value.ToString() == "0")
                    //    {
                    //        dtRow["Isnew"] = "0";
                    //    }
                    //    else
                    //    {
                    //        dtRow["Isnew"] = "";
                    //    }
                    //    dtGrid.Rows.Add(dtRow);
                    //}

                    Program.sealdt.Rows.Add(dtRow);

                }

                // dtGrid.NewRow();
                //LoadSeal();
            }

            catch (Exception Ex)
            {
                Program.ExceptionMsg("AddRecord Function Exception Error : Contact AbacusConsultings.");
                Program.oErrMgn.LogException(Program.ANV, Ex);

            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            return Program.sealdt;

        }

        public Boolean AddSealToDb()
        {
            Boolean retVal = true;
            oDB = new dbFFS(Program.ConStrApp);
            try
            {
                int ID;
                int DocNum;
                string ItemCode = "";
                string SealNumber = "";
                string FormName = "";
                string CreatedBy = "";
                string CreatedDate = "";
                string UpdatedDate = "";
                string UpdatedBy = "";
                string IsNew = "";

                foreach (DataRow row in Program.sealdt.Rows)
                {

                    DocNum = Convert.ToInt32(row["DocNum"]);
                    ItemCode = row["ItemCode"].ToString();
                    SealNumber = row["SealNumber"].ToString();
                    FormName = row["FormName"].ToString();
                    CreatedBy = row["CreatedBy"].ToString();
                    CreatedDate = row["CreatedDate"].ToString();
                    UpdatedDate = row["UpdateDate"].ToString();
                    UpdatedBy = row["UpdatedBy"].ToString();
                    IsNew = row["IsNew"].ToString();


                    MstSeal NewMP = new MstSeal();
                    NewMP.CreatedBy = CreatedBy;
                    NewMP.UpdatedBy = UpdatedBy;
                    NewMP.CreateDT = Convert.ToDateTime(CreatedDate);
                    NewMP.UpdateDT = Convert.ToDateTime(UpdatedDate);

                    if (IsNew == "0")
                    {
                        ID = Convert.ToInt32(row["ID"]);
                        MstSeal oldMP = oDB.MstSeal.Where(x => x.ID == ID).FirstOrDefault();

                        if (oldMP.SealNum != Convert.ToString(SealNumber))
                        {
                            oldMP.DocNum = DocNum;
                            oldMP.SealNum = SealNumber;
                            oldMP.ItemCode = ItemCode;
                            oldMP.FormName = Program.FormName;
                            oldMP.UpdatedBy = UpdatedBy;
                            oldMP.UpdateDT = Convert.ToDateTime(UpdatedDate);
                            Program.SuccesesMsg("Record Successfully Updated");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {

                        NewMP.DocNum = DocNum;
                        NewMP.SealNum = SealNumber;
                        NewMP.ItemCode = ItemCode;
                        NewMP.FormName = Program.FormName;

                        oDB.MstSeal.InsertOnSubmit(NewMP);
                        Program.SuccesesMsg("Record Successfully Added");
                    }
                    oDB.SubmitChanges();

                }

            }
            catch (Exception ex)
            {
                retVal = false;
                throw;
            }
            return retVal;
        }

        private void LoadSeal()
        {
            try
            {
                // DataTable dt = new DataTable();

                dtGrid.Clear();
                SetTheGrid();
                IEnumerable<MstSeal> oCollection = from a in oDB.MstSeal
                                                   where a.FormName == Program.FormName && a.DocNum == Program.FormDocNum
                                                   select a;

                foreach (MstSeal One in oCollection)
                {
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["ID"] = One.ID;
                    dtRow["DocNum"] = One.DocNum;
                    dtRow["ItemCode"] = One.ItemCode;
                    dtRow["SealNumber"] = One.SealNum;
                    dtRow["CreatedBy"] = One.CreatedBy;
                    dtRow["CreatedDate"] = One.CreateDT;
                    dtRow["UpdatedBy"] = One.UpdatedBy;
                    dtRow["UpdateDate"] = One.UpdateDT;
                    dtRow["Isnew"] = "0";
                    //dtRow["Delete"] = "Delete";
                    dtGrid.Rows.Add(dtRow);


                }

                //if (Program.sealdt != null && oCollection.Count() == 0)

                if (Program.sealdt.Rows.Count > 0)
                {

                    if (oCollection.Count() == 0 || oCollection.Count() != Program.sealdt.Rows.Count)
                    {
                        grdOpen.DataSource = Program.sealdt;
                    }
                    else
                    {
                        grdOpen.DataSource = dtGrid;
                    }
                }
                else
                {
                    grdOpen.DataSource = dtGrid;
                }

                grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdOpen.ShowGroupPanel = false;
                grdOpen.ReadOnly = false;
                grdOpen.EnableFiltering = true;
                grdOpen.ShowFilteringRow = true;
                grdOpen.Columns["Isnew"].IsVisible = false;
                grdOpen.Columns["ID"].IsVisible = false;
                grdOpen.Columns["DocNum"].IsVisible = false;
                grdOpen.Columns["ItemCode"].IsVisible = false;
                grdOpen.Columns["FormName"].IsVisible = false;
                grdOpen.Columns["CreatedBy"].IsVisible = false;
                grdOpen.Columns["CreatedDate"].IsVisible = false;
                grdOpen.Columns["UpdatedBy"].IsVisible = false;
                grdOpen.Columns["UpdateDate"].IsVisible = false;
                //grdOpen.Columns["Delete"].

            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        //private void LoadCompletedRecords()
        //{
        //    try
        //    {
        //        if (!flgToggle)
        //        {
        //            var oCollection = oDB.Abc_mfm_GetHistoryListInwards();
        //            foreach (var Row in oCollection)
        //            {
        //                DataRow dtRow = dtGrid.NewRow();
        //                dtRow["ID"] = Row.ID;
        //                dtRow["Serial"] = Row.Serial;
        //                dtRow["VoucherNo"] = Row.VoucherNo;
        //                dtRow["VehicleNo"] = Row.VehicleNo;
        //                dtRow["CardCode"] = Row.CardCode;
        //                dtRow["CardName"] = Row.CardName;
        //                dtGrid.Rows.Add(dtRow);
        //            }
        //        }
        //        else
        //        {
        //            var oCollection = oDB.Abc_mfm_GetHistoryListOutwards();
        //            foreach (var Row in oCollection)
        //            {
        //                DataRow dtRow = dtGrid.NewRow();
        //                dtRow["ID"] = Row.ID;
        //                dtRow["Serial"] = Row.Serial;
        //                dtRow["VoucherNo"] = Row.VoucherNo;
        //                dtRow["VehicleNo"] = Row.VehicleNo;
        //                dtRow["CardCode"] = Row.CardCode;
        //                dtRow["CardName"] = Row.CardName;
        //                dtGrid.Rows.Add(dtRow);
        //            }
        //        }
        //        grdOpen.DataSource = dtGrid;
        //        grdOpen.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        //        grdOpen.ShowGroupPanel = false;
        //        grdOpen.ReadOnly = true;
        //        grdOpen.EnableFiltering = true;
        //        grdOpen.MasterTemplate.Columns["ID"].IsVisible = false;
        //        grdOpen.ShowFilteringRow = true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}

        public DataTable RemainingQuan()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            if (OpenFor == "oBulkSealer")
                btnSelect.Text = "Post";
            InitiallizeSetting();
            this.MaximizeBox = false;

            return ForRemainingQuantity;
        }

        #endregion

        #region Form Events

        public frmOpenDlg()
        {
            InitializeComponent();
        }

        private void frmOpenDlg_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            if (OpenFor == "oBulkSealer")
                btnSelect.Text = "Post";
            InitiallizeSetting();
            this.MaximizeBox = false;

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenFor == "oBulkSealer")
                {
                    if (ValidateRecord())
                    {
                        AddSealToDt();
                    }
                }
                SelectedValues();
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("btnSelect Click Event Exception Error : " + Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void grdOpen_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                switch (OpenFor)
                {
                    case "oBulkSealer":
                        string code = Convert.ToString(e.Row.Cells["SealNumber"].Value);
                        if (string.IsNullOrEmpty(code))
                        {
                            btnSelect.Text = "Post";
                        }
                        else
                        {
                            btnSelect.Text = "Update";
                        }
                        break;
                    case "trnsDispatch":
                        Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        break;
                    case "trnsDispatchReturn":
                        Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        break;
                    case "trnsRawMaterial":
                        Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        break;
                    case "trnsRawMaterialReturn":
                        Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        break;
                    default:
                        SelectedValues();
                        break;
                }
                //if (OpenFor == "oBulkSealer")
                //{
                //    string code = Convert.ToString(e.Row.Cells["SealNumber"].Value);
                //    if (string.IsNullOrEmpty(code))
                //    {
                //        btnSelect.Text = "Post";
                //    }
                //    else
                //    {
                //        btnSelect.Text = "Update";
                //    }
                //}
                //else if (OpenFor == "trnsDispatch")
                //{
                //    Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                //}
                //else if (OpenFor == "trnsDispatchReturn")
                //{
                //    Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                //}
                //else if (OpenFor == "trnsRawMaterial")
                //{
                //    Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                //}
                //else if (OpenFor == "trnsRawMaterialReturn")
                //{
                //    Program.DocNum = Convert.ToString(e.Row.Cells["DocNum"].Value);
                //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                //}
                //else
                //{
                //    SelectedValues();
                //}
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("grdOpen Cell Click Exception Error : " + Ex.Message);
            }
        }

        private void grdOpen_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            if (OpenFor == "oBulkSealer")
            {
                SealNumber = "";
                SealNumber = Convert.ToString(e.Row.Cells["SealNumber"].Value);
            }

        }

        private void grdOpen_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            try
            {
                DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
                oResult = RadMessageBox.Show("Are you sure you want to delete. Its irreversable process.", "Confirmation.", MessageBoxButtons.YesNo);
                if (oResult == DialogResult.Yes)
                {
                    string lineid = string.Empty;
                    lineid = Convert.ToString(e.Rows[0].Cells["ID"].Value);
                    if (string.IsNullOrEmpty(lineid))
                    {
                        Program.WarningMsg("Line can't be deleted.");
                    }
                    else
                    {
                        var oLine = (from a in oDB.MstSeal
                                     where a.ID.ToString() == lineid
                                     select a).FirstOrDefault();
                        if (oLine != null)
                        {
                            oDB.MstSeal.DeleteOnSubmit(oLine);
                            oDB.SubmitChanges();
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                Program.ExceptionMsg("Something went wrong, Contact Abacus Support.");
            }
        }

        #endregion

    }

    public class SaleOrderData
    {
        public string SBRNum { get; set; }
        public decimal Balance { get; set; }
        public decimal Order { get; set; }
    }
}
