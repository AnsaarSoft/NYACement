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


namespace mfmFFS.Screens
{
    public partial class frmRawMaterialReturnPost : frmBaseForm
    {

        #region Variable
        DataTable dt = new DataTable();
        string OpenObjectID = "";
        dbFFS oDB = null;
        string ActiveSeries = "";
        string ComPort, BaudRate, Parity, StopBit, StartChar, DataBits, DataLenght, FrontCam, BackCam, CamTimer, FCUserName, BCUserName, FCPassword, BCPassword;
        bool flgSboLogin = false;
        
        private void frmDispatchPost_Load(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        Boolean flgAscii = false;
        Boolean flgToggle = false;
        Int64 mostRecentWeight = 0;

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row is GridViewTableHeaderRowInfo)
            {
                if (radGridView1.Rows.Count > 0)
                {
                    if (e.Column.HeaderText == "Check")
                    {
                        GridViewRowInfo CurrentRow = radGridView1.Rows[0];
                        GridViewCellInfo cellCheck = CurrentRow.Cells["Check"];
                        bool Val = Convert.ToBoolean(cellCheck.Value);
                        if (Val == false)
                        {
                            for (int j = 0; j < radGridView1.Rows.Count; j++)
                            {
                                GridViewRowInfo CurrentRow1 = radGridView1.Rows[j];
                                GridViewCellInfo cellCheck1 = CurrentRow1.Cells["Check"];
                                cellCheck1.Value = true;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < radGridView1.Rows.Count; j++)
                            {
                                GridViewRowInfo CurrentRow1 = radGridView1.Rows[j];
                                GridViewCellInfo cellCheck1 = CurrentRow1.Cells["Check"];

                                cellCheck1.Value = false;
                            }
                        }
                    }
                }
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Dispose();
                base.mytabpage.Dispose();
                Program.NoMsg("");
            }
            catch (Exception ex)
            {

            }
        }

        Boolean isLoadingImageFront = false;
        Boolean isLoadingImageBack = false;
        int FormState = 0;
        DataTable dtRows = null;
        private SerialPort comport = new SerialPort();
        Boolean alreadyReading = false;

        #endregion

        #region Events
        private void radButton3_Click(object sender, EventArgs e)
        {
            Post();
        }
        #endregion

        #region Functions

        public frmRawMaterialReturnPost()
        {
            InitializeComponent();
            InitiallizeForm();
        }

        private void InitiallizeForm()
        {
            try
            {
                oDB = new dbFFS(Program.ConStrApp);

                CreateDt();
                LoadGrid();
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "RawMaterialReturn Post").FirstOrDefault().GivenRight);
                if (Auth == 4)
                {
                    radButton3.Enabled = false;
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                if (super == true)
                {
                    radButton3.Enabled = true;
                }
                // AuthorizationScheme();
                //tmrCamFront.Interval = (Convert.ToInt32(CamTimer) * 1000);
                //tmrCamBack.Interval = (Convert.ToInt32(CamTimer) * 1000);
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
                dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("SWeighDate");
                dt.Columns.Add("DocNum");
                dt.Columns.Add("GRPONum");
                dt.Columns.Add("VendorName");
                dt.Columns.Add("Transporter");

                dt.Columns.Add("ItemName");
                dt.Columns.Add("Barrage");
                dt.Columns.Add("FirstWeight KG");
                dt.Columns.Add("SecondWeight KG");
                dt.Columns.Add("NetWeight KG");
                dt.Columns.Add("NetWeight Ton");
                dt.Columns.Add("VehicleNum");
                dt.Columns.Add("DriverCNIC");
                dt.Columns.Add("DriverName");
                dt.Columns.Add("Status");

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        private void LoadGrid()
        {
            try
            {
                dt.Clear();
                CreateDt();
                IEnumerable<TrnsRawMaterialReturn> getData = from a in oDB.TrnsRawMaterialReturn where a.FlgSecondWeight == true && a.FlgPosted == null select a;

                foreach (TrnsRawMaterialReturn item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    string transportname = "";
                    dtrow["ID"] = item.ID;
                    string[] SDate = Convert.ToString(item.SWeighmentDate).Split(' ');
                    string SeconDate = SDate[0];
                    dtrow["SWeighDate"] = SeconDate;
                    //dtrow["SWeightDate"] = item.SWeighmentDate;
                    dtrow["DocNum"] = item.DocNum;
                    dtrow["GRPONum"] = item.GRPONum;
                    dtrow["VendorName"] = item.VendorName;
                    dtrow["Transporter"] = item.TransportName;
                    dtrow["ItemName"] = item.ItemName;
                    if (item.TransportID != 0)
                    {
                        var record = oDB.MstTransportType.Where(x => x.ID == item.TransportID);
                        if (record.Count() > 0)
                        {
                            transportname = record.FirstOrDefault().Name;
                        }
                    }
                    dtrow["Barrage"] = transportname;
                    dtrow["FirstWeight KG"] = item.FWeighmentKG;
                    dtrow["SecondWeight KG"] = item.SWeighmentKG;
                    dtrow["NetWeight KG"] = item.NetWeightKG;
                    dtrow["NetWeight Ton"] = item.NetWeightTon;
                    dtrow["VehicleNum"] = item.VehicleNum;
                    dtrow["DriverCNIC"] = item.DriverCNIC;
                    dtrow["DriverName"] = item.DriverName;
                    string Post = "";
                    if (item.FlgPosted == true)
                    {
                        Post = "Posted";
                    }
                    else
                    {
                        Post = "NotPosted";
                    }
                    dtrow["Status"] = Post;
                    //string[] date = Convert.ToString(item.FDocDate).Split(' ');
                    //dtrow["DocDate"] = date[0];
                    dt.Rows.Add(dtrow);

                }
                radGridView1.DataSource = dt;
                radGridView1.EnableFiltering = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void Post()
        {
            try
            {
                if (flgSboLogin == false)
                {
                    frmMain.LoginSBO();
                    flgSboLogin = true;
                }
                if (flgSboLogin)
                {
                    for (int i = 0; i < radGridView1.RowCount; i++)
                    {
                        GridViewRowInfo CurrentRow = radGridView1.Rows[i];
                        GridViewCellInfo cellCheck = CurrentRow.Cells["Check"];
                        bool Val = Convert.ToBoolean(cellCheck.Value);
                        if (Val == true)
                        {
                            GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                            TrnsRawMaterialReturn Res = oDB.TrnsRawMaterialReturn.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();
                            AddtoSBO(Res);
                        }
                    }
                    LoadGrid();
                }
                else
                {
                    Program.NoMsg("Company not connected");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void AddtoSBO(TrnsRawMaterialReturn oVal)
        {
            string retValue = string.Empty, ObjectKey = string.Empty;
            string transportname = "";
            try
            {
                DataTable qdt = new DataTable();

                SAPbobsCOM.Documents oPurchaseR = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns);

                oPurchaseR.CardCode = oVal.VendorCode;
                oPurchaseR.CardName = oVal.VendorName;
                oPurchaseR.DocDate = Convert.ToDateTime(oVal.SDocDate);
                oPurchaseR.DocDueDate = Convert.ToDateTime(oVal.SDocDate);
                oPurchaseR.TaxDate = Convert.ToDateTime(oVal.SDocDate);
                string Querydt = "Select A.ExpnsCode, (A.LineTotal/(Select Y.DocRate from OPDN Y Where y.DocNum =" + oVal.GRPONum + "))  * (" + oVal.NetWeightTon + " / (Select SUM(y.Quantity) From PDN1 y Where y.DocEntry = A.DocEntry)) as LineGross from PDN3 A Where A.DocEntry = (Select x.DocEntry from OPDN x Where x.DocNum = " + oVal.GRPONum + ")";
                qdt = mFm.ExecuteQueryDt(Querydt, Program.ConStrSAP);
                for (int i = 0; i < qdt.Rows.Count; i++)
                {
                    oPurchaseR.Expenses.ExpenseCode = Convert.ToInt32(qdt.Rows[i][0]);
                    oPurchaseR.Expenses.LineTotal = Convert.ToDouble(qdt.Rows[i][1]);
                    oPurchaseR.Expenses.Add();
                }
                oPurchaseR.Lines.UserFields.Fields.Item("U_WeghRef").Value = oVal.DocNum.ToString();
                string Query = "Select x1.LineNum From OPDN x Inner Join PDN1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.GRPONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                int BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                string Query1 = "Select x1.DocEntry From OPDN x Inner Join PDN1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.GRPONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                int BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                string Query2 = "Select x.Objtype From OPDN x Inner Join PDN1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.GRPONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                int BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                oPurchaseR.Lines.BaseLine = BaseLine;
                oPurchaseR.Lines.BaseEntry = BaseEntry;
                oPurchaseR.Lines.BaseType = BaseType;
                oPurchaseR.Lines.ItemCode = oVal.ItemCode;
                oPurchaseR.Lines.Quantity = Convert.ToDouble(oVal.NetWeightTon);
                oPurchaseR.Lines.UserFields.Fields.Item("U_Yard").Value = oVal.YardId.ToString();
                oPurchaseR.Lines.UserFields.Fields.Item("U_VehcleNo").Value = oVal.VehicleNum;
                oPurchaseR.Lines.UserFields.Fields.Item("U_Dcnic").Value = oVal.DriverCNIC;
                oPurchaseR.Lines.UserFields.Fields.Item("U_Driver").Value = oVal.DriverName;
                oPurchaseR.Lines.UserFields.Fields.Item("U_FrstWght").Value = Convert.ToDouble((oVal.FWeighmentTon));
                if (oVal.TransportID != 0)
                {
                    var record = oDB.MstTransportType.Where(x => x.ID == oVal.TransportID);
                    if (record.Count() > 0)
                    {
                        transportname = record.FirstOrDefault().Name;
                    }
                }
                oPurchaseR.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                oPurchaseR.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = oVal.TransportCode.ToString();
                oPurchaseR.Lines.UserFields.Fields.Item("U_ScndWght").Value = Convert.ToDouble((oVal.SWeighmentTon));
                oPurchaseR.Lines.Add();
                if (oPurchaseR.Add() != 0)
                //if (true)
                {
                    int ErrorCode = 0; string ErrorDesc = string.Empty;
                    Program.SapCompany.GetLastError(out ErrorCode, out ErrorDesc);
                    retValue = "Sap B1 Error : " + ErrorCode + " : " + ErrorDesc;
                    Program.ExceptionMsg(retValue.ToString());
                }
                else
                {
                    retValue = Convert.ToString(Program.SapCompany.GetNewObjectKey());
                    TrnsRawMaterialReturn RawMaterialReturn = oDB.TrnsRawMaterialReturn.Where(x => x.ID == oVal.ID).FirstOrDefault();
                    RawMaterialReturn.FlgPosted = true;
                    oDB.SubmitChanges();

                    Program.SuccesesMsg("Posted to SBO");
                }





            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.ToString());
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        //    try
        //    {
        //        string strQuerySAP = "INSERT INTO RPD1 ( U_Weghref," +
        //                                                          @"BaseRef,
        //                                                          BaseLine,
        //                                                          BaseType,
        //                                                          ItemCode,
        //                                                          Dscription,
        //                                                          U_Yard,
        //                                                          U_Vehcle, 
        //                                                          U_DCNIC,
        //                                                          U_Driver,
        //                                                          U_frstwght, 
        //                                                          U_trnstyp,
        //                                                          U_trnsprtr,
        //                                                          U_ScndWght,
        //                                                          Quantity" +



        //                                         @"VALUES  ( N'" + Convert.ToString(oVal.DocNum) + @"' , -- Code - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.GRPONum) + @"' , -- Name - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.GRPONum) + @"' , -- U_VoucherNo - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.GRPONum) + @"' , -- U_GatePass - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.ItemCode) + @"' , -- U_SourceType - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.ItemName) + @"' , -- U_UDODocNo - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.YardId) + @"' , -- U_BPCode - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.VehicleNum) + @"' , -- U_BPName - nvarchar(100)
        //                                                  N'" + Convert.ToString(oVal.DriverCNIC) + @"' , -- U_ItemCode - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.DriverName) + @"' , -- U_ItemName - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.FWeighmentTon) + @"' , -- U_Driver - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.TransportID) + @"' , -- U_VehicleReg - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.TransportCode) + @"' , -- U_EngineStatus - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.SWeighmentTon) + @"' , -- U_DriverStatus - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.NetWeightTon) + @"' , -- U_FirstWeight - nvarchar(30)

        //                                                )
        //                                        ";
        //        string strQuery = "INSERT INTO ORPD (             CardCode," +
        //                                                          @"CardName,
        //                                                          DocDate,
        //                                                          DocDueDate,
        //                                                          TaxDate" +



        //                                         @"VALUES  ( N'" + Convert.ToString(oVal.VendorCode) + @"' , -- Code - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.VendorName) + @"' , -- Name - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.SDocDate) + @"' , -- U_VoucherNo - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.SDocDate) + @"' , -- U_GatePass - nvarchar(30)
        //                                                  N'" + Convert.ToString(oVal.SDocDate) + @"' , -- U_SourceType - nvarchar(30)


        //                                                )
        //                                        ";
        //        Program.oErrMgn.LogEntry(Program.ANV, strQuerySAP);
        //        Program.oErrMgn.LogEntry(Program.ANV, strQuery);
        //        Program.oErrMgn.LogEntry(Program.ANV, Program.ConStrSAP);
        //        mFm.ExecuteQuery(strQuerySAP, Program.ConStrSAP);
        //        mFm.ExecuteQuery(strQuery, Program.ConStrSAP);


        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.oErrMgn.LogException(Program.ANV, Ex);
        //    }
        //}
        #endregion

    }
}

