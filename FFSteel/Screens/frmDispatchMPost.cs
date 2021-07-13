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
        List<ChkDocHeaderStatus> oListHeader = new List<ChkDocHeaderStatus>();

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
                    dt.Rows.Add(dtrow);
                }
                radGridView1.DataSource = dt;
                radGridView1.EnableFiltering = true;
                oList.Clear();
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
                   flgSboLogin = frmMain.LoginSBO();
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
                            GridViewCellInfo cellDocNum = CurrentRow.Cells["DocNum"];
                            var odetail = (from a in oDB.TrnsDispatchMultiDetail
                                           where a.DocNum == Convert.ToInt32(cellDocNum.Value) && (a.FlgPosted == null ? false : a.FlgPosted) == false
                                           select a).ToList();
                            foreach (var one in odetail)
                            {
                                ConsolidateDataM oItem = new ConsolidateDataM();
                                oItem.Customer = one.CustomerCode;
                                oItem.SBRNo = one.SBRNum;
                                oItem.DocNum = one.DocNum.ToString();
                                oItem.ItemCode = one.ItemCode;
                                oList.Add(oItem);
                            }
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
                                
                                foreach (var SBR in SBRNoWise)
                                {
                                    GroupMD myDoc = new GroupMD();
                                    GroupMDLine TheLine = new GroupMDLine();
                                    var DMD = (from a in oDB.TrnsDispatchMultiDetail
                                               where a.SBRNum == SBR.SBRNo && a.DocNum == Convert.ToInt32(SBR.DocNum)
                                               && a.ItemCode == SBR.ItemCode
                                               select a).FirstOrDefault();
                                    if (DMD == null) continue;
                                    TheLine.DocNum = (int)DMD.DocNum;
                                    myDoc.SBRNumber = DMD.SBRNum;
                                    myDoc.CardCode = DMD.CustomerCode;
                                    myDoc.CardName = DMD.CustomerName;
                                    myDoc.DocNum = (int)DMD.DocNum;
                                    myDoc.LineId = DMD.ID;
                                    TheLine.ItemCode = DMD.ItemCode;
                                    TheLine.ItemName = DMD.ItemName;
                                    TheLine.ItemGroupCode = DMD.ItemGroupCode;
                                    TheLine.OrderQty = (decimal)DMD.OrderQuantity;

                                    var DMH = (from a in oDB.TrnsDispatchMultiHeader
                                               where a.DocNum == TheLine.DocNum
                                               select a).FirstOrDefault();
                                    if (DMH == null) continue;

                                    myDoc.Status = false;
                                    myDoc.PackerId = DMH.PackerId != null ? (int)DMH.PackerId : 0;
                                    myDoc.VehicleNo = DMH.VehicleNum;
                                    myDoc.DCNIC = DMH.DriverCNIC;
                                    myDoc.Driver = DMH.DriverName;
                                    myDoc.FirstWeightKG = (double)DMH.FWeighmentKG;
                                    myDoc.FirstWeightTon = (double)DMH.FWeighmentTon;
                                    myDoc.TransportId = DMH.TransportID != null ? (int)DMH.TransportID : 0;
                                    myDoc.TransporterCode = DMH.TransportCode != null ? DMH.TransportCode : "0";
                                    myDoc.SecondWeighKG = (double)DMH.SWeighmentKG;
                                    myDoc.SecondWeighTon = (double)DMH.SWeighmentTon;
                                    myDoc.NetWeightKG = (double)DMH.NetWeightKG;
                                    myDoc.NetWeightTon = (double)DMH.NetWeightTon;
                                    myDoc.sdocdate = Convert.ToDateTime(DMH.SDocDate);
                                    myDoc.oLines.Add(TheLine);
                                    AddtoSBOGroup(myDoc);
                                    if (myDoc.Status)
                                    {
                                        var oSD = (from a in oDB.TrnsDispatchMultiDetail
                                                   where a.DocNum == myDoc.DocNum && a.SBRNum == myDoc.SBRNumber && a.ItemCode == TheLine.ItemCode
                                                   select a).FirstOrDefault();
                                        if (oSD == null) continue;
                                        oSD.FlgPosted = true;
                                        oDB.SubmitChanges();
                                    }
                                    ChkDocHeaderStatus oItem1 = new ChkDocHeaderStatus();
                                    oItem1.DocNum = myDoc.DocNum;
                                    oItem1.LineId = myDoc.LineId;
                                    oItem1.flgStatus = myDoc.Status;
                                    oListHeader.Add(oItem1);
                                }
                                //if(Program.SapCompany.Connected)
                                //{
                                //    MessageBox.Show("connected.");
                                //}
                                //else
                                //{
                                //    MessageBox.Show("disconn");
                                //}
                            }
                        }
                        oListHeader = oListHeader.OrderBy(m => m.DocNum).ToList();
                        int docNumCompTrue = 0;
                        if (oListHeader.Count > 0)
                        {
                            foreach (var Single in oListHeader)
                            {
                                if (docNumCompTrue != Single.DocNum)
                                {
                                    docNumCompTrue = 0;

                                    int docLines = (from a in oDB.TrnsDispatchMultiDetail
                                                    where a.DocNum == Single.DocNum
                                                    select a).Count();

                                    int docTLines = (from a in oDB.TrnsDispatchMultiDetail
                                                     where a.DocNum == Single.DocNum && (a.FlgPosted == null ? false : a.FlgPosted) == true
                                                     select a).Count();
                                    if (docLines == docTLines)
                                    {
                                        if (docNumCompTrue == 0)
                                        {
                                            var oSO = (from a in oDB.TrnsDispatchMultiHeader
                                            where a.DocNum == Single.DocNum
                                            select a).FirstOrDefault();
                                            if (oSO == null) continue;
                                            oSO.FlgPosted = true;
                                            oDB.SubmitChanges();
                                            docNumCompTrue = Single.DocNum;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    oListHeader.Clear();
                    if (flgSboLogin)
                    {
                        flgSboLogin = false;
                        Program.isSapConnected = false;
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

        private void AddtoSBOGroup(GroupMD myDoc)
        {
            string transportname = "";
            string retValue = string.Empty, ObjectKey = string.Empty;
            DataTable qdt = new DataTable();
            try
            {

                SAPbobsCOM.Documents oDeliveryH = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);


                oDeliveryH.CardCode = myDoc.CardCode;
                oDeliveryH.CardName = myDoc.CardName;

                oDeliveryH.DocDate = Convert.ToDateTime(myDoc.sdocdate);
                oDeliveryH.DocDueDate = Convert.ToDateTime(myDoc.sdocdate);
                oDeliveryH.TaxDate = Convert.ToDateTime(myDoc.sdocdate);
                //oDeliveryH.UserFields.Fields.Item("U_ToWeight").Value = Convert.ToDouble((myDoc.NetWeightTon));

                oDeliveryH.UserFields.Fields.Item("U_ToWeight").Value = Convert.ToDouble((myDoc.FirstWeightTon));
                oDeliveryH.UserFields.Fields.Item("U_ToWeight2").Value = Convert.ToDouble((myDoc.SecondWeighTon));

                var odetail = (from a in oDB.TrnsDispatchMultiDetail
                               where a.DocNum == myDoc.DocNum
                               select a).ToList();
                foreach (var One in myDoc.oLines)
                {
                    //string Querydt = "Select A.ExpnsCode, (A.LineTotal/(Select Y.DocRate from ORDR Y Where y.DocNum =" + myDoc.SBRNumber + "))  * (" + One.OrderQty + " / (Select SUM(y.Quantity) From RDR1 y Where y.DocEntry = A.DocEntry)) as LineGross from RDR3 A Where A.DocEntry = (Select x.DocEntry from ORDR x Where x.DocNum = " + myDoc.SBRNumber + ")";
                    //qdt = mFm.ExecuteQueryDt(Querydt, Program.ConStrSAP);
                    //for (int i = 0; i < qdt.Rows.Count; i++)
                    //{
                    //    oDeliveryH.Expenses.ExpenseCode = Convert.ToInt32(qdt.Rows[i][0]);
                    //    oDeliveryH.Expenses.LineTotal = Convert.ToDouble(qdt.Rows[i][1]);
                    //    oDeliveryH.Expenses.Add();
                    //}
                    oDeliveryH.Lines.UserFields.Fields.Item("U_WeghRef").Value = myDoc.DocNum.ToString();

                    string Query = "Select x1.LineNum From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + myDoc.SBRNumber + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    int BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                    string Query1 = "Select x1.DocEntry From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + myDoc.SBRNumber + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    int BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                    string Query2 = "Select x.Objtype From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + myDoc.SBRNumber + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    int BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                    oDeliveryH.Lines.BaseLine = BaseLine;
                    oDeliveryH.Lines.BaseEntry = BaseEntry;
                    oDeliveryH.Lines.BaseType = BaseType;
                    //oDeliveryH.Lines.BaseLine = 0;
                    oDeliveryH.Lines.ItemCode = One.ItemCode;
                    //if (oVal.ItemGroupName.ToUpper().Contains("BAG"))
                    //{
                    oDeliveryH.Lines.Quantity = Convert.ToDouble(One.OrderQty);
                    //}
                    //else
                    //{
                    //    oDeliveryH.Lines.Quantity = Convert.ToDouble(One.OrderQty);
                    //}
                    
                    #region Freight 1

                    string Query3 = "Select isnull(x1.U_FreightTon,0) As U_FreightTon From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + myDoc.SBRNumber + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    double U_FreightTon = Convert.ToDouble(mFm.ExecuteQueryScaler(Query3, Program.ConStrSAP));
                    if (U_FreightTon > 0)
                    {
                        double Freight1LC = Math.Round(Convert.ToDouble(One.OrderQty) * U_FreightTon,2);
                        oDeliveryH.Lines.Expenses.ExpenseCode = 4;
                        oDeliveryH.Lines.Expenses.LineTotal = Freight1LC;
                        oDeliveryH.Lines.Expenses.GroupCode = 0;
                        oDeliveryH.Lines.Expenses.BaseGroup = 0;
                        //oDeliveryH.Lines.Expenses.SetCurrentLine = 
                        oDeliveryH.Lines.Expenses.Add();
                    }
                    #endregion

                    #region Freight 2 FPI
                    double Freight2LC = 0;
                    if (One.ItemCode == "FG001" || One.ItemCode == "FG002")
                    {
                        Freight2LC = Math.Round(Convert.ToDouble(One.OrderQty) * 1.89,2);
                    }
                    else if (One.ItemCode == "FG003" || One.ItemCode == "FG004")
                    {
                        Freight2LC = Math.Round(Convert.ToDouble(One.OrderQty) * 2.05,2);
                    }
                    else if (One.ItemCode == "FG005" || One.ItemCode == "FG006")
                    {
                        Freight2LC = Math.Round(Convert.ToDouble(One.OrderQty) * 3.57,2);
                    }
                    else
                    {

                    }
                    if (Freight2LC > 0)
                    {
                        oDeliveryH.Lines.Expenses.ExpenseCode = 2;
                        oDeliveryH.Lines.Expenses.LineTotal = Freight2LC;
                        oDeliveryH.Lines.Expenses.GroupCode = 1;
                        oDeliveryH.Lines.Expenses.BaseGroup = 1;
                        oDeliveryH.Lines.Expenses.Add();
                    }
                    #endregion

                    #region Freight 3 OCC047

                    string Query4 = "Select x1.Price From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + myDoc.SBRNumber + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    double Price = Convert.ToDouble(mFm.ExecuteQueryScaler(Query4, Program.ConStrSAP));
                    if (Price > 0)
                    {
                        double Freight3LC = Math.Round(Price * Convert.ToDouble(One.OrderQty) * 0.0047,2);
                        oDeliveryH.Lines.Expenses.ExpenseCode = 1;
                        oDeliveryH.Lines.Expenses.LineTotal = Freight3LC;
                        oDeliveryH.Lines.Expenses.GroupCode = 2;
                        oDeliveryH.Lines.Expenses.BaseGroup = 2;
                        oDeliveryH.Lines.Expenses.Add();
                    }
                    #endregion

                    #region Destination

                    string Query5 = "Select x1.U_Bin As U_FreightTon From ORDR x Inner Join RDR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = '" + myDoc.SBRNumber + "' and x1.ItemCode = '" + One.ItemCode + "'";
                    string destination = mFm.ExecuteQueryScaler(Query5, Program.ConStrSAP);
                    if (destination != null)
                    {
                        oDeliveryH.Lines.UserFields.Fields.Item("U_Bin").Value = destination;
                    }

                    #endregion

                    oDeliveryH.Lines.UserFields.Fields.Item("U_Packer").Value = myDoc.PackerId.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_VehcleNo").Value = myDoc.VehicleNo.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Dcnic").Value = myDoc.DCNIC.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Driver").Value = myDoc.Driver.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_FrstWght").Value = Convert.ToDouble((myDoc.FirstWeightTon));
                    //oDeliveryH.Lines.UserFields.Fields.Item("U_ToWeight").Value = Convert.ToDouble((myDoc.NetWeightTon));

                    if (myDoc.TransportId != 0)
                    {
                        var record = oDB.MstTransportType.Where(x => x.ID == myDoc.TransportId);
                        if (record.Count() > 0)
                        {
                            transportname = record.FirstOrDefault().Name;
                        }
                    }
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                    oDeliveryH.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = myDoc.TransporterCode.ToString();
                    oDeliveryH.Lines.UserFields.Fields.Item("U_ScndWght").Value = Convert.ToDouble((myDoc.SecondWeighTon));
                    oDeliveryH.Lines.Add();
                }
                if (oDeliveryH.Add() != 0)
                {
                    int ErrorCode = 0; string ErrorDesc = string.Empty;
                    Program.SapCompany.GetLastError(out ErrorCode, out ErrorDesc);
                    retValue = "DocNum : " + myDoc.DocNum.ToString() + " Sap B1 Error and desc : " + ErrorCode + " : " + ErrorDesc;
                    Program.ExceptionMsg(retValue.ToString());
                    Program.oErrMgn.LogEntry(Program.ANV, retValue);
                    myDoc.Status = false;
                }
                else
                {
                    retValue = Convert.ToString(Program.SapCompany.GetNewObjectKey());
                    //TrnsDispatchMultiHeader Dispatch = oDB.TrnsDispatchMultiHeader.Where(x => x.DocNum == myDoc.DocNum).FirstOrDefault();
                    //Dispatch.FlgPosted = true;
                    //oDB.SubmitChanges();
                    myDoc.Status = true;
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
            public string DocNum { get; set; }
            public string ItemCode { get; set; }
        }

        public class GroupMD
        {
            public bool Status { get; set; }
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string SBRNumber { get; set; }
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

            public int LineId { get; set; }

            public List<GroupMDLine> oLines = new List<GroupMDLine>();
        }

        public class GroupMDLine
        {
            public int DocNum { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string ItemGroupCode { get; set; }
            public decimal OrderQty { get; set; }
        }

        public class ChkDocHeaderStatus
        {
            public int DocNum { get; set; }
            public int LineId { get; set; }
            public bool flgStatus { get; set; }
        }
    }
}
