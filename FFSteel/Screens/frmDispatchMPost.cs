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
    public partial class frmDispatchMPost : frmBaseForm
    {

        #region Variable
        DataTable dt = new DataTable();
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        bool flgSboLogin = false;

        List<ConsolidateDataM> oList = new List<ConsolidateDataM>();

        #endregion

        #region Functions
        public frmDispatchMPost()
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
                /*dt.Columns.Add("SBRNum");
                dt.Columns.Add("CustomerName");
                dt.Columns.Add("ItemName");*/
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
                IEnumerable<TrnsDispatchMultiHeader> getData = from a in oDB.TrnsDispatchMultiHeader where a.FlgSecondWeight == true && a.FlgPosted == null select a;

                foreach (TrnsDispatchMultiHeader item in getData)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["ID"] = item.ID;
                    string[] SDate = Convert.ToString(item.SWeighmentDate).Split(' ');
                    string SeconDate = SDate[0];
                    dtrow["SWeightDate"] = SeconDate;
                    dtrow["DocNum"] = item.DocNum;
                    /*dtrow["SBRNum"] = item.SBRNum;
                    dtrow["CustomerName"] = item.CustomerName;
                    dtrow["ItemName"] = item.ItemName;*/
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
                            //GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                            GridViewCellInfo cellDocNum = CurrentRow.Cells["DocNum"];
                            //TrnsDispatchMultiDetail Res = oDB.TrnsDispatchMultiDetail.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();
                            //TrnsDispatchMultiDetail Res = oDB.TrnsDispatchMultiDetail.Where(x => x.DocNum == Convert.ToInt32(cellID.Value)).FirstOrDefault();
                            var odetail = (from a in oDB.TrnsDispatchMultiDetail
                                           where a.DocNum == Convert.ToInt32(cellDocNum.Value)
                                           select a).ToList();
                            /*if (Res.FlgMulti.GetValueOrDefault())
                            {
                                AddtoSBOMulti(Res);
                            }
                            else
                            {
                                AddtoSBO(Res);
                            }*/
                            foreach (var one in odetail)
                            {
                                ConsolidateDataM oItem = new ConsolidateDataM();
                                oItem.Customer = one.CustomerCode;
                                oItem.SBRNo = one.SBRNum;
                                oList.Add(oItem);
                            }
                            //ConsolidateDataM oItem = new ConsolidateDataM();
                            //oItem.Customer = Res.CustomerCode;
                            //oItem.SBRNo = Res.SBRNum;
                            //oList.Add(oItem);
                        }
                    }
                    if (oList.Count > 0)
                    {
                        var GroupByCustomer = (from a in oList
                                           group a by a.Customer into b
                                           select b).ToList();
                        foreach (var CustomerWise in GroupByCustomer)
                        {
                            var GroupBySBRNo = CustomerWise.GroupBy(a => a.SBRNo).ToList();
                            foreach (var SBRNoWise in GroupBySBRNo)
                            {
                                GroupMDLine TheLine = new GroupMDLine();
                                foreach (var SBR in SBRNoWise)
                                {
                                    var DMD = (from a in oDB.TrnsDispatchMultiDetail
                                               where a.SBRNum == SBR.SBRNo
                                                select a).FirstOrDefault();
                                    if (DMD == null) continue;
                                    TheLine.DocNum = (int)DMD.DocNum;
                                    TheLine.SBRNumber = DMD.SBRNum;
                                    //myDoc.SBRDate
                                    TheLine.CardCode = DMD.CustomerCode;
                                    TheLine.CardName = DMD.CustomerName;
                                    TheLine.ItemCode = DMD.ItemCode;
                                    TheLine.ItemName = DMD.ItemName;
                                    TheLine.ItemGroupCode = DMD.ItemGroupCode;
                                    TheLine.ItemGroupName = DMD.ItemGroupName;
                                    TheLine.OrderQty = (decimal)DMD.OrderQuantity;
                                    TheLine.BalQty = (decimal)DMD.BalanceQuantity;

                                    var DMH = (from a in oDB.TrnsDispatchMultiHeader
                                               where a.DocNum == TheLine.DocNum
                                               select a).FirstOrDefault();
                                    if (DMH == null) continue;

                                    GroupMD myDoc = new GroupMD();
                                    //myDoc.DocNum = 
                                    myDoc.PackerId = (int)DMH.PackerId;
                                    myDoc.VehicleNo = DMH.VehicleNum;
                                    myDoc.DCNIC = DMH.DriverCNIC;
                                    myDoc.Driver = DMH.DriverName;
                                    myDoc.FirstWeightKG = (double)DMH.FWeighmentKG;
                                    myDoc.FirstWeightTon = (double)DMH.FWeighmentTon;
                                    myDoc.TransportId = (int)DMH.TransportID;
                                    myDoc.TransporterCode = DMH.TransportCode;
                                    myDoc.SecondWeighKG = (double)DMH.SWeighmentKG;
                                    myDoc.SecondWeighTon = (double)DMH.SWeighmentTon;
                                    myDoc.NetWeightKG = (double)DMH.NetWeightKG;
                                    myDoc.NetWeightTon = (double)DMH.NetWeightTon;
                                    myDoc.sdocdate = Convert.ToDateTime(DMH.SDocDate);
                                    myDoc.oLines.Add(TheLine);
                                }
                                //AddtoSBOGroup(myDoc);
                                //if (myDoc.Status)
                                //{
                                //    foreach (var One in myDoc.oLines)
                                //    {
                                //        var oGRN = (from a in oDB.TrnsRawMaterial
                                //                    where a.ID == One.TransactionId
                                //                    select a).FirstOrDefault();
                                //        if (oGRN == null) continue;
                                //        oGRN.FlgPosted = true;
                                //        oDB.SubmitChanges();
                                //    }
                                //}
                                //else
                                //{

                                //}
                            }
                        }
                    }

                    /*LoadGrid();*/
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

        private void AddtoSBOGroup(GroupMD Doc,GroupMDLine oDetail)
        {
            string retValue = string.Empty, ObjectKey = string.Empty;
            string transportname = "";
            try
            {
                DataTable qdt = new DataTable();
                SAPbobsCOM.Documents oDeliveryH = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                oDeliveryH.CardCode = oDetail.CardCode;
                oDeliveryH.CardName = oDetail.CardName;


                oDeliveryH.DocDate = Doc.sdocdate;
                oDeliveryH.DocDueDate = Doc.sdocdate;
                oDeliveryH.TaxDate = Doc.sdocdate;




                //string Query, Query1, Query2;
                //int BaseLine = 0, BaseEntry = 0, BaseType = 0;
                //if (Doc.flgAPRI)
                //{
                //    Query = "Select x1.LineNum From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                //    BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                //    Query1 = "Select x1.DocEntry From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                //    BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                //    Query2 = "Select x.Objtype From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                //    BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                //}
                //else
                //{
                //    Query = "Select x1.LineNum From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                //    BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                //    Query1 = "Select x1.DocEntry From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                //    BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                //    Query2 = "Select x.Objtype From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                //    BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                //}

                //oPurchase.Lines.BaseLine = BaseLine;
                //oPurchase.Lines.BaseEntry = BaseEntry;
                //oPurchase.Lines.BaseType = BaseType;
                //oPurchase.Lines.ItemCode = Doc.ItemCode;

                ////oPurchase.Lines.UserFields.Fields.Item("U_Yard").Value = Line.Yard;
                ////oPurchase.Lines.UserFields.Fields.Item("U_VehcleNo").Value = Line.VehicleNo;
                ////oPurchase.Lines.UserFields.Fields.Item("U_Dcnic").Value = Line.DCNIC;
                ////oPurchase.Lines.UserFields.Fields.Item("U_Driver").Value = Line.Driver;
                ////oPurchase.Lines.UserFields.Fields.Item("U_WeghRef").Value = Doc.BaseDocCollection;
                //double QuanityValue = 0;
                //if (Doc.ItemGroup.ToUpper().Contains("OIL"))
                //{
                //    QuanityValue = Doc.oLines.Sum(a => a.NetWeightKG);
                //    oPurchase.Lines.Quantity = Doc.oLines.Sum(a => a.NetWeightKG);
                //    //oPurchase.Lines.UserFields.Fields.Item("U_FrstWght").Value = Line.FirstWeight;
                //    //oPurchase.Lines.UserFields.Fields.Item("U_ScndWght").Value = Line.SecondWeigh;
                //}
                //else
                //{
                //    QuanityValue = Doc.oLines.Sum(a => a.NetWeightTon);
                //    //oPurchase.Lines.UserFields.Fields.Item("U_FrstWght").Value = Line.FirstWeight;
                //    //oPurchase.Lines.UserFields.Fields.Item("U_ScndWght").Value = Line.SecondWeigh;
                //}
                //oPurchase.Lines.Quantity = QuanityValue;
                ////if (Line.TransportId != 0)
                ////{
                ////    var record = oDB.MstTransportType.Where(x => x.ID == Line.TransportId);
                ////    if (record.Count() > 0)
                ////    {
                ////        transportname = record.FirstOrDefault().Name;
                ////    }
                ////}
                ////oPurchase.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                ////oPurchase.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = Line.TransporterCode;

                //oPurchase.Lines.Add();
                //if (oPurchase.Add() != 0)
                ////if (true)
                //{
                //    int ErrorCode = 0; string ErrorDesc = string.Empty;
                //    Program.SapCompany.GetLastError(out ErrorCode, out ErrorDesc);
                //    retValue = "Sap B1 Error : " + ErrorCode + " : " + ErrorDesc;
                //    Program.ExceptionMsg(retValue);
                //    Doc.Status = false;
                //    Doc.SapDoc = "";
                //    Doc.Error = retValue;
                //}
                //else
                //{
                //    retValue = Convert.ToString(Program.SapCompany.GetNewObjectKey());
                //    //TrnsRawMaterial RawMaterial = oDB.TrnsRawMaterial.Where(x => x.ID == oVal.ID).FirstOrDefault();
                //    //RawMaterial.FlgPosted = true;
                //    //oDB.SubmitChanges();
                //    Doc.Status = true;
                //    Doc.SapDoc = retValue;
                //    Doc.Error = "";
                //    Program.SuccesesMsg("Posted to SBO");
                //}
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.ToString());
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

        #region Events
        private void radButton3_Click(object sender, EventArgs e)
        {
            Post();
        }

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
        #endregion

        public class ConsolidateDataM
        {
            public string Customer { get; set; }
            public string SBRNo { get; set; }
        }

     

        public class GroupMD
        {
            public int DocNum { get; set; }
            public int PackerId { get; set; }
            public string VehicleNo { get; set; }
            public string DCNIC { get; set; }
            public string Driver { get; set; }
            public double FirstWeightKG { get; set; }
            public double FirstWeightTon { get; set; }
            public int TransportId { get; set; }
            public string TransporterCode { get; set; }
            public double SecondWeighKG { get; set; }
            public double SecondWeighTon { get; set; }
            public double NetWeightKG { get; set; }
            public double NetWeightTon { get; set; }
            public DateTime sdocdate { get; set; }

            public List<GroupMDLine> oLines = new List<GroupMDLine>();
        }

        public class GroupMDLine
        {
            public int DocNum { get; set; }
            public string SBRNumber { get; set; }
            public string SBRDate { get; set; }
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string ItemGroupCode { get; set; }
            public string ItemGroupName { get; set; }
            public decimal OrderQty { get; set; }
            public decimal BalQty { get; set; }
        }
    }
}
