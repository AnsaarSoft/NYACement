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
    public partial class frmDispatchPost : frmBaseForm
    {

        #region Variable
        DataTable dt = new DataTable();
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        bool flgSboLogin = false;

        #endregion


        #region Events
        private void radButton3_Click(object sender, EventArgs e)
        {
            Post();
        }
        #endregion


        #region Functions
        public frmDispatchPost()
        {
            InitializeComponent();
            CreateDt();
            LoadGrid();
        }

        private void CreateDt()
        {
            try
            {
                dt = new DataTable();

                dt.Columns.Add("ID");
                dt.Columns.Add("SWeightDate");
                dt.Columns.Add("DocNum");
                dt.Columns.Add("SBRNum");
                dt.Columns.Add("CustomerName");
                dt.Columns.Add("ItemName");
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
                IEnumerable<TrnsDispatch> getData = from a in oDB.TrnsDispatch where a.FlgSecondWeight == true && a.FlgPosted == null select a;

                foreach (TrnsDispatch item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["ID"] = item.ID;
                    string[] SDate = Convert.ToString(item.SWeighmentDate).Split(' ');
                    string SeconDate = SDate[0];
                    dtrow["SWeightDate"] = SeconDate;
                    dtrow["DocNum"] = item.DocNum;
                    dtrow["SBRNum"] = item.SBRNum;
                    dtrow["CustomerName"] = item.CustomerName;
                    dtrow["ItemName"] = item.ItemName;
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
                            TrnsDispatch Res = oDB.TrnsDispatch.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();
                            if (Res.FlgMulti.GetValueOrDefault())
                            {
                                AddtoSBOMulti(Res);
                            }
                            else
                            {
                                AddtoSBO(Res);
                            }
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

        private void AddtoSBO(TrnsDispatch oVal)
        {

            string transportname = "";
            string retValue = string.Empty, ObjectKey = string.Empty;
            DataTable qdt = new DataTable();
            try
            {

                SAPbobsCOM.Documents oDeliveryH = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);

                oDeliveryH.CardCode = oVal.CustomerCode;
                oDeliveryH.CardName = oVal.CustomerName;

                oDeliveryH.DocDate = Convert.ToDateTime(oVal.SDocDate);
                oDeliveryH.DocDueDate = Convert.ToDateTime(oVal.SDocDate);
                oDeliveryH.TaxDate = Convert.ToDateTime(oVal.SDocDate);
                string Querydt = "Select A.ExpnsCode, (A.LineTotal/(Select Y.DocRate from ORDR Y Where y.DocNum =" + oVal.SBRNum + "))  * (" + oVal.OrderQuantity + " / (Select SUM(y.Quantity) From RDR1 y Where y.DocEntry = A.DocEntry)) as LineGross from RDR3 A Where A.DocEntry = (Select x.DocEntry from ORDR x Where x.DocNum = " + oVal.SBRNum + ")";
                qdt = mFm.ExecuteQueryDt(Querydt, Program.ConStrSAP);
                for (int i = 0; i < qdt.Rows.Count; i++)
                {
                    oDeliveryH.Expenses.ExpenseCode = Convert.ToInt32(qdt.Rows[i][0]);
                    oDeliveryH.Expenses.LineTotal = Convert.ToDouble(qdt.Rows[i][1]);
                    oDeliveryH.Expenses.Add();
                }
                oDeliveryH.Lines.UserFields.Fields.Item("U_WeghRef").Value = oVal.DocNum.ToString();

                string Query = "Select x1.LineNum From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + oVal.SBRNum + "' and x1.ItemCode = '" + oVal.ItemCode + "'";
                int BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                string Query1 = "Select x1.DocEntry From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + oVal.SBRNum + "' and x1.ItemCode = '" + oVal.ItemCode + "'";
                int BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                string Query2 = "Select x.Objtype From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + oVal.SBRNum + "' and x1.ItemCode = '" + oVal.ItemCode + "'";
                int BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                oDeliveryH.Lines.BaseLine = BaseLine;
                oDeliveryH.Lines.BaseEntry = BaseEntry;
                oDeliveryH.Lines.BaseType = BaseType;
                oDeliveryH.Lines.ItemCode = oVal.ItemCode;
                if (oVal.ItemGroupName.ToUpper().Contains("BAG"))
                {
                    oDeliveryH.Lines.Quantity = Convert.ToDouble(oVal.OrderQuantity);
                }
                else
                {
                    oDeliveryH.Lines.Quantity = Convert.ToDouble(oVal.NetWeightTon);
                }
                oDeliveryH.Lines.UserFields.Fields.Item("U_Packer").Value = oVal.PackerId.ToString();
                oDeliveryH.Lines.UserFields.Fields.Item("U_VehcleNo").Value = oVal.VehicleNum.ToString();
                oDeliveryH.Lines.UserFields.Fields.Item("U_Dcnic").Value = oVal.DriverCNIC.ToString();
                oDeliveryH.Lines.UserFields.Fields.Item("U_Driver").Value = oVal.DriverName.ToString();
                oDeliveryH.Lines.UserFields.Fields.Item("U_FrstWght").Value = Convert.ToDouble((oVal.FWeighmentTon));
                if (oVal.TransportID != 0)
                {
                    var record = oDB.MstTransportType.Where(x => x.ID == oVal.TransportID);
                    if (record.Count() > 0)
                    {
                        transportname = record.FirstOrDefault().Name;
                    }
                }
                oDeliveryH.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                oDeliveryH.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = oVal.TransportCode.ToString();
                oDeliveryH.Lines.UserFields.Fields.Item("U_ScndWght").Value = Convert.ToDouble((oVal.SWeighmentTon));
                oDeliveryH.Lines.Add();
                if (oDeliveryH.Add() != 0)
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
                    TrnsDispatch Dispatch = oDB.TrnsDispatch.Where(x => x.ID == oVal.ID).FirstOrDefault();
                    Dispatch.FlgPosted = true;
                    oDB.SubmitChanges();
                    //LoadGrid();
                    Program.SuccesesMsg("Posted to SBO");
                }


                //string strQuerySAP = "INSERT INTO DLN1 ( U_Weghref,"+
                //                                                  @"BaseRef,
                //                                                  BaseLine,
                //                                                  BaseType,
                //                                                  ItemCode,
                //                                                  Dscription,
                //                                                  U_packer,
                //                                                  U_Vehcle, 
                //                                                  U_DCNIC,
                //                                                  U_Driver,
                //                                                  U_frstwght, 
                //                                                  U_trnstyp,
                //                                                  U_trnsprtr,
                //                                                  U_ScndWght,
                //                                                  Quantity"+



                //                                 @"VALUES  ( N'" + Convert.ToString(oVal.DocNum) + @"' , -- Code - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SBRNum) + @"' , -- Name - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SBRNum) + @"' , -- U_VoucherNo - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SBRNum) + @"' , -- U_GatePass - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.ItemCode) + @"' , -- U_SourceType - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.ItemName) + @"' , -- U_UDODocNo - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.PackerId) + @"' , -- U_BPCode - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.VehicleNum) + @"' , -- U_BPName - nvarchar(100)
                //                                          N'" + Convert.ToString(oVal.DriverCNIC) + @"' , -- U_ItemCode - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.DriverName) + @"' , -- U_ItemName - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.FWeighmentTon) + @"' , -- U_Driver - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.TransportID) + @"' , -- U_VehicleReg - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.TransportCode) + @"' , -- U_EngineStatus - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SWeighmentTon) + @"' , -- U_DriverStatus - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.NetWeightTon) + @"' , -- U_FirstWeight - nvarchar(30)

                //                                        )
                //                                ";
                //string strQuery = "INSERT INTO ODLN (             CardCode," +
                //                                                  @"CardName,
                //                                                  DocDate,
                //                                                  DocDueDate,
                //                                                  TaxDate" +



                //                                 @"VALUES  ( N'" + Convert.ToString(oVal.CustomerCode) + @"' , -- Code - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.CustomerName) + @"' , -- Name - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SDocDate) + @"' , -- U_VoucherNo - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SDocDate) + @"' , -- U_GatePass - nvarchar(30)
                //                                          N'" + Convert.ToString(oVal.SDocDate) + @"' , -- U_SourceType - nvarchar(30)


                //                                        )
                //                                ";
                //Program.oErrMgn.LogEntry(Program.ANV, strQuerySAP);
                //Program.oErrMgn.LogEntry(Program.ANV, strQuery);
                //Program.oErrMgn.LogEntry(Program.ANV, Program.ConStrSAP);
                // mFm.ExecuteQuery(strQuerySAP, Program.ConStrSAP);
                //mFm.ExecuteQuery(strQuery, Program.ConStrSAP);


            }
            catch (Exception Ex)
            {
                //Program.WarningMsg(Ex);
                Program.ExceptionMsg(Ex.ToString());
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void AddtoSBOMulti(TrnsDispatch oVal)
        {
            string transportname = "";
            string retValue = string.Empty, ObjectKey = string.Empty;
            DataTable qdt = new DataTable();
            try
            {

                SAPbobsCOM.Documents oDeliveryH = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);

                oDeliveryH.CardCode = oVal.CustomerCode;
                oDeliveryH.CardName = oVal.CustomerName;

                oDeliveryH.DocDate = Convert.ToDateTime(oVal.SDocDate);
                oDeliveryH.DocDueDate = Convert.ToDateTime(oVal.SDocDate);
                oDeliveryH.TaxDate = Convert.ToDateTime(oVal.SDocDate);
                var odetail = (from a in oDB.TrnsDispatchMulti
                               where a.DocNum == oVal.DocNum
                               select a).ToList();
                foreach (var One in odetail)
                {
                    string Querydt = "Select A.ExpnsCode, (A.LineTotal/(Select Y.DocRate from ORDR Y Where y.DocNum =" + One.SBRNum + "))  * (" + One.OrderQty + " / (Select SUM(y.Quantity) From RDR1 y Where y.DocEntry = A.DocEntry)) as LineGross from RDR3 A Where A.DocEntry = (Select x.DocEntry from ORDR x Where x.DocNum = " + One.SBRNum + ")";
                    qdt = mFm.ExecuteQueryDt(Querydt, Program.ConStrSAP);
                    for (int i = 0; i < qdt.Rows.Count; i++)
                    {
                        oDeliveryH.Expenses.ExpenseCode = Convert.ToInt32(qdt.Rows[i][0]);
                        oDeliveryH.Expenses.LineTotal = Convert.ToDouble(qdt.Rows[i][1]);
                        oDeliveryH.Expenses.Add();
                    }
                    oDeliveryH.Lines.UserFields.Fields.Item("U_WeghRef").Value = oVal.DocNum.ToString();

                    string Query = "Select x1.LineNum From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + One.SBRNum + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    int BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                    string Query1 = "Select x1.DocEntry From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + One.SBRNum + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    int BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                    string Query2 = "Select x.Objtype From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + One.SBRNum + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    int BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                    oDeliveryH.Lines.BaseLine = BaseLine;
                    oDeliveryH.Lines.BaseEntry = BaseEntry;
                    oDeliveryH.Lines.BaseType = BaseType;
                    //oDeliveryH.Lines.BaseLine = 0;
                    oDeliveryH.Lines.ItemCode = oVal.ItemCode;
                    if (oVal.ItemGroupName.ToUpper().Contains("BAG"))
                    {
                        oDeliveryH.Lines.Quantity = Convert.ToDouble(One.OrderQty);
                    }
                    else
                    {
                        oDeliveryH.Lines.Quantity = Convert.ToDouble(One.OrderQty);
                    }
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Packer").Value = oVal.PackerId.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_VehcleNo").Value = oVal.VehicleNum.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Dcnic").Value = oVal.DriverCNIC.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Driver").Value = oVal.DriverName.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_FrstWght").Value = Convert.ToDouble((oVal.FWeighmentTon));
                    if (oVal.TransportID != 0)
                    {
                        var record = oDB.MstTransportType.Where(x => x.ID == oVal.TransportID);
                        if (record.Count() > 0)
                        {
                            transportname = record.FirstOrDefault().Name;
                        }
                    }
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = oVal.TransportCode.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_ScndWght").Value = Convert.ToDouble((oVal.SWeighmentTon));
                    oDeliveryH.Lines.Add();
                }
                if (oDeliveryH.Add() != 0)
                {
                    int ErrorCode = 0; string ErrorDesc = string.Empty;
                    Program.SapCompany.GetLastError(out ErrorCode, out ErrorDesc);
                    retValue = "DocNum : " + oVal.DocNum.ToString() + " Sap B1 Error and desc : " + ErrorCode + " : " + ErrorDesc;
                    Program.ExceptionMsg(retValue.ToString());
                    Program.oErrMgn.LogEntry(Program.ANV, retValue);
                }
                else
                {
                    retValue = Convert.ToString(Program.SapCompany.GetNewObjectKey());
                    TrnsDispatch Dispatch = oDB.TrnsDispatch.Where(x => x.ID == oVal.ID).FirstOrDefault();
                    Dispatch.FlgPosted = true;
                    oDB.SubmitChanges();
                    Program.SuccesesMsg("Posted to SBO");
                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        #endregion

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
                            if (radGridView1.MasterTemplate.ChildRows.Count > 0)
                            {
                                for (int j = 0; j < radGridView1.MasterTemplate.ChildRows.Count; j++)
                                {
                                    GridViewRowInfo CurrentRow1 = radGridView1.MasterTemplate.ChildRows[j];
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
                                    cellCheck1.Value = true;
                                }
                            }
                        }
                        else
                        {
                            if (radGridView1.MasterTemplate.ChildRows.Count > 0)
                            {
                                for (int j = 0; j < radGridView1.MasterTemplate.ChildRows.Count; j++)
                                {
                                    GridViewRowInfo CurrentRow1 = radGridView1.MasterTemplate.ChildRows[j];
                                    GridViewCellInfo cellCheck1 = CurrentRow1.Cells["Check"];
                                    cellCheck1.Value = false;
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
    }
}
