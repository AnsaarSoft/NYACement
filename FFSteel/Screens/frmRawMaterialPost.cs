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
    public partial class frmRawMaterialPost : frmBaseForm
    {

        #region Variable

        string OpenObjectID = "";
        dbFFS oDB = null;
        string ActiveSeries = "";
        string ComPort, BaudRate, Parity, StopBit, StartChar, DataBits, DataLenght, FrontCam, BackCam, CamTimer, FCUserName, BCUserName, FCPassword, BCPassword;

        Boolean flgAscii = false;

        Boolean flgToggle = false;
        Int64 mostRecentWeight = 0;
        Boolean isLoadingImageFront = false;
        Boolean isLoadingImageBack = false;
        string SNO1, DocID1;

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
                                for (int j = 0; j < radGridView1.ChildRows.Count; j++)
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
                            if(radGridView1.MasterTemplate.ChildRows.Count > 0)
                            {
                                for (int j = 0; j < radGridView1.ChildRows.Count; j++)
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

        int FormState = 0;

        private void MasterTemplate_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        DataTable dtRows = null;
        private SerialPort comport = new SerialPort();
        Boolean alreadyReading = false;
        bool flgSboLogin = false;

        DataTable dt = new DataTable();

        private void radButton3_Click(object sender, EventArgs e)
        {
            Post();
        }

        List<ConsolidateData> oList = new List<ConsolidateData>();

        #endregion

        #region Functions

        public frmRawMaterialPost()
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
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "RawMaterial Post").FirstOrDefault().GivenRight);
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
                dt.Columns.Add("SWeightDate");
                dt.Columns.Add("DocNum");
                dt.Columns.Add("PONum");
                dt.Columns.Add("VendorName");
                dt.Columns.Add("Transporter");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Barrage");
                dt.Columns.Add("FirstWeight KG");
                dt.Columns.Add("SecondWeight KG");
                dt.Columns.Add("NetWeight KG");
                dt.Columns.Add("NetWeight Ton");
                dt.Columns.Add("VehicleNum");
                dt.Columns.Add("SNO");
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
                IEnumerable<TrnsRawMaterial> getData = from a in oDB.TrnsRawMaterial where a.FlgSecondWeight == true && a.FlgPosted == null select a;

                foreach (TrnsRawMaterial item in getData)
                {
                    string transportname = "";
                    DataRow dtrow = dt.NewRow();
                    dtrow["ID"] = item.ID;
                    string[] SDate = Convert.ToString(item.SWeighmentDate).Split(' ');
                    string SeconDate = SDate[0];
                    dtrow["SWeightDate"] = SeconDate;
                    dtrow["DocNum"] = item.DocNum;
                    dtrow["PONum"] = item.PONum;

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
                    dtrow["SNO"] = item.SNO;
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
            catch (Exception Ex)
            {

                Program.ExceptionMsg(Ex.ToString());
                Program.oErrMgn.LogException(Program.ANV, Ex);
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
                    if (optSingle.IsChecked)
                    {
                        //Program.oErrMgn.LogEntry(Program.ANV, "simple.");
                        #region Single
                        for (int i = 0; i < radGridView1.RowCount; i++)
                        {
                            GridViewRowInfo CurrentRow = radGridView1.Rows[i];
                            GridViewCellInfo cellCheck = CurrentRow.Cells["Check"];
                            bool Val = Convert.ToBoolean(cellCheck.Value);
                            //Program.oErrMgn.LogEntry(Program.ANV, $"loop count. {i}");
                            if (Val == true)
                            {
                                GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                                TrnsRawMaterial Res = oDB.TrnsRawMaterial.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();
                                Res.SNO = Res.DocNum.ToString();
                                oDB.SubmitChanges();
                                AddtoSBO(Res);
                            }

                        }
                        #endregion
                    }
                    else if (optConsolidated.IsChecked)
                    {
                        Program.oErrMgn.LogEntry(Program.ANV, "group.");
                        #region Consolidation
                        for (int i = 0; i < radGridView1.RowCount; i++)
                        {
                            GridViewRowInfo CurrentRow = radGridView1.Rows[i];
                            GridViewCellInfo cellCheck = CurrentRow.Cells["Check"];
                            bool Val = Convert.ToBoolean(cellCheck.Value);
                            if (Val == true)
                            {
                                GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                                //GridViewCellInfo SNO = CurrentRow.Cells["SNO"];
                                TrnsRawMaterial Res = oDB.TrnsRawMaterial.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();
                                ConsolidateData oItem = new ConsolidateData();
                                oItem.Vendor = Res.VendorCode;
                                oItem.PONumber = Res.PONum;
                                oItem.DocumentDate = Res.SWeighmentDate.GetValueOrDefault();
                                oItem.TransactionId = Res.ID;
                                oList.Add(oItem);
                                //if (!string.IsNullOrEmpty(Convert.ToString(cellID.Value)))
                                //{
                                //}
                                //else
                                //{
                                //    Program.ExceptionMsg("SNO is Mandatory.");
                                //}
                            }
                        }
                        if (oList.Count > 0)
                        {
                            var GroupByDate = (from a in oList
                                               group a by a.DocumentDate into b
                                               select b).ToList();
                            //Program.oErrMgn.LogEntry(Program.ANV, "date count: " + GroupByDate.Count.ToString());
                            foreach (var Datewise in GroupByDate)
                            {
                                //stage two grouping.
                                var GroupByVendor = Datewise.GroupBy(a => a.Vendor).ToList();
                                //Program.oErrMgn.LogEntry(Program.ANV, "vendor count: " + GroupByVendor.Count.ToString());
                                foreach (var VendorWise in GroupByVendor)
                                {
                                    var GroupByPO = VendorWise.GroupBy(a => a.PONumber).ToList();
                                    //Program.oErrMgn.LogEntry(Program.ANV, "po count: " + GroupByPO.Count.ToString());
                                    foreach (var POWise in GroupByPO)
                                    {
                                        GroupGRN myDoc = new GroupGRN();
                                        foreach (var PO in POWise)
                                        {
                                            var oGrn = (from a in oDB.TrnsRawMaterial
                                                        where a.ID == PO.TransactionId
                                                        select a).FirstOrDefault();
                                            if (oGrn == null) continue;
                                            myDoc.CardCode = oGrn.VendorCode;
                                            myDoc.CardName = oGrn.VendorName;
                                            myDoc.SecondWeightDate = Convert.ToDateTime(oGrn.SWeighmentDate);
                                            myDoc.BaseDocCollection += oGrn.DocNum.ToString() + ", ";
                                            myDoc.Status = false;
                                            myDoc.SapDoc = "";
                                            myDoc.Error = "";
                                            myDoc.PONumber = oGrn.PONum;
                                            myDoc.ItemCode = oGrn.ItemCode;
                                            myDoc.ItemGroup = oGrn.ItemGroupName;
                                            myDoc.flgAPRI = oGrn.FlgAPRI.GetValueOrDefault();

                                            GroupGRNLine TheLine = new GroupGRNLine();
                                            TheLine.TransactionId = PO.TransactionId;
                                            TheLine.Yard = oGrn.YardId.ToString();
                                            TheLine.VehicleNo = oGrn.VehicleNum;
                                            TheLine.DCNIC = oGrn.DriverCNIC;
                                            TheLine.Driver = oGrn.DriverName;
                                            TheLine.DocRef = oGrn.DocNum.ToString();
                                            TheLine.NetWeightKG = (double)oGrn.NetWeightKG;
                                            TheLine.NetWeightTon = (double)oGrn.NetWeightTon;
                                            TheLine.FirstWeight = (double)oGrn.FWeighmentKG;
                                            TheLine.SecondWeigh = (double)oGrn.SWeighmentKG;
                                            TheLine.TransportId = oGrn.TransportID.GetValueOrDefault();
                                            TheLine.TransporterCode = oGrn.TransportCode;
                                            myDoc.oLines.Add(TheLine);
                                        }
                                        AddtoSBOGroup(myDoc);
                                        if (myDoc.Status)
                                        {
                                            foreach (var One in myDoc.oLines)
                                            {
                                                var oGRN = (from a in oDB.TrnsRawMaterial
                                                            where a.ID == One.TransactionId
                                                            select a).FirstOrDefault();
                                                if (oGRN == null) continue;
                                                oGRN.FlgPosted = true;
                                                oDB.SubmitChanges();
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                        }
                        #endregion
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

        private void AddtoSBO(TrnsRawMaterial oVal)
        {
            string retValue = string.Empty, ObjectKey = string.Empty;
            string transportname = "";
            try
            {
                Program.oErrMgn.LogEntry(Program.ANV, "started");
                DataTable qdt = new DataTable();
                SAPbobsCOM.Documents oPurchase = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                Program.oErrMgn.LogEntry(Program.ANV, "started1");
                oPurchase.CardCode = oVal.VendorCode;
                oPurchase.CardName = oVal.VendorName;
                oPurchase.DocDate = Convert.ToDateTime(oVal.SDocDate);
                oPurchase.DocDueDate = Convert.ToDateTime(oVal.SDocDate);
                oPurchase.TaxDate = Convert.ToDateTime(oVal.SDocDate);
                //string Querydt = "Select A.ExpnsCode, (A.LineTotal/(Select Y.DocRate from OPOR Y Where y.DocNum =" + oVal.PONum + "))  * (" + oVal.NetWeightTon + " / (Select SUM(y.Quantity) From POR1 y Where y.DocEntry = A.DocEntry)) as LineGross from POR3 A Where A.DocEntry = (Select x.DocEntry from OPOR x Where x.DocNum = " + oVal.PONum + ")";
                //qdt = mFm.ExecuteQueryDt(Querydt, Program.ConStrSAP);
                //Program.oErrMgn.LogEntry(Program.ANV, "started3");
                //for (int i = 0; i < qdt.Rows.Count; i++)
                //{
                //    oPurchase.Expenses.ExpenseCode = Convert.ToInt32(qdt.Rows[i][0]);
                //    oPurchase.Expenses.LineTotal = Convert.ToDouble(qdt.Rows[i][1]);
                //    oPurchase.Expenses.Add();
                //}

                oPurchase.Lines.UserFields.Fields.Item("U_WeghRef").Value = oVal.DocNum.ToString();
                Program.oErrMgn.LogEntry(Program.ANV, "started4");
                string Query, Query1, Query2;
                int BaseLine = 0, BaseEntry = 0, BaseType = 0;
                if(oVal.FlgAPRI.GetValueOrDefault())
                {
                    Query = "Select x1.LineNum From OPCH x Inner Join PCH1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.PONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                    Program.oErrMgn.LogEntry(Program.ANV, $"pehli query {Query}");
                    BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                    Program.oErrMgn.LogEntry(Program.ANV, $"pehli query ki value {Query}");
                    Query1 = "Select x1.DocEntry From OPCH x Inner Join PCH1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.PONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                    Program.oErrMgn.LogEntry(Program.ANV, $"second query {Query}");
                    BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                    Program.oErrMgn.LogEntry(Program.ANV, $"second query ki value {Query}");
                    Query2 = "Select x.Objtype From OPCH x Inner Join PCH1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.PONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                    Program.oErrMgn.LogEntry(Program.ANV, $"third query {Query}");
                    BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                    Program.oErrMgn.LogEntry(Program.ANV, $"third query ki value {Query}");
                }
                else
                {
                    Query = "Select x1.LineNum From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.PONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                    BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                    Query1 = "Select x1.DocEntry From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.PONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                    BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                    Query2 = "Select x.Objtype From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + oVal.PONum + " and x1.ItemCode = '" + oVal.ItemCode + "'";
                    BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                }

                Program.oErrMgn.LogEntry(Program.ANV, "started5");
                oPurchase.Lines.BaseLine = BaseLine;
                oPurchase.Lines.BaseEntry = BaseEntry;
                oPurchase.Lines.BaseType = BaseType;
                oPurchase.Lines.ItemCode = oVal.ItemCode;

                oPurchase.Lines.UserFields.Fields.Item("U_Yard").Value = oVal.YardId.ToString();
                oPurchase.Lines.UserFields.Fields.Item("U_VehcleNo").Value = oVal.VehicleNum;
                oPurchase.Lines.UserFields.Fields.Item("U_Dcnic").Value = oVal.DriverCNIC;
                oPurchase.Lines.UserFields.Fields.Item("U_Driver").Value = oVal.DriverName;
                if (oVal.ItemGroupName.ToUpper().Contains("OIL"))
                {
                    oPurchase.Lines.Quantity = Convert.ToDouble(oVal.NetWeightKG);
                    oPurchase.Lines.UserFields.Fields.Item("U_FrstWght").Value = Convert.ToDouble((oVal.FWeighmentKG));
                    oPurchase.Lines.UserFields.Fields.Item("U_ScndWght").Value = Convert.ToDouble((oVal.SWeighmentKG));
                }
                else
                {
                    oPurchase.Lines.Quantity = Convert.ToDouble(oVal.NetWeightTon);
                    oPurchase.Lines.UserFields.Fields.Item("U_FrstWght").Value = Convert.ToDouble((oVal.FWeighmentTon));
                    oPurchase.Lines.UserFields.Fields.Item("U_ScndWght").Value = Convert.ToDouble((oVal.SWeighmentTon));
                }
                if (oVal.TransportID != 0)
                {
                    var record = oDB.MstTransportType.Where(x => x.ID == oVal.TransportID);
                    if (record.Count() > 0)
                    {
                        transportname = record.FirstOrDefault().Name;
                    }
                }
                oPurchase.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                oPurchase.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = oVal.TransportCode;
                Program.oErrMgn.LogEntry(Program.ANV, "started6");
                oPurchase.Lines.Add();
                if (oPurchase.Add() != 0)
                //if (true)
                {
                    int ErrorCode = 0; string ErrorDesc = string.Empty;
                    Program.SapCompany.GetLastError(out ErrorCode, out ErrorDesc);
                    retValue = "Sap B1 Error : " + ErrorCode + " : " + ErrorDesc;
                    Program.ExceptionMsg(retValue);
                    Program.oErrMgn.LogEntry(Program.ANV, "" + retValue);
                }
                else
                {
                    retValue = Convert.ToString(Program.SapCompany.GetNewObjectKey());
                    TrnsRawMaterial RawMaterial = oDB.TrnsRawMaterial.Where(x => x.ID == oVal.ID).FirstOrDefault();
                    RawMaterial.FlgPosted = true;
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

        private void AddtoSBOGroup(GroupGRN Doc)
        {
            string retValue = string.Empty, ObjectKey = string.Empty;
            string transportname = "";
            try
            {
                DataTable qdt = new DataTable();
                SAPbobsCOM.Documents oPurchase = Program.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                oPurchase.CardCode = Doc.CardCode;
                oPurchase.CardName = Doc.CardName;
                oPurchase.DocDate = Doc.SecondWeightDate;
                oPurchase.DocDueDate = Doc.SecondWeightDate;
                oPurchase.TaxDate = Doc.SecondWeightDate;
                //string Querydt = "Select A.ExpnsCode, (A.LineTotal/(Select Y.DocRate from OPOR Y Where y.DocNum =" + oVal.PONum + "))  * (" + oVal.NetWeightTon + " / (Select SUM(y.Quantity) From POR1 y Where y.DocEntry = A.DocEntry)) as LineGross from POR3 A Where A.DocEntry = (Select x.DocEntry from OPOR x Where x.DocNum = " + oVal.PONum + ")";
                //qdt = mFm.ExecuteQueryDt(Querydt, Program.ConStrSAP);
                //for (int i = 0; i < qdt.Rows.Count; i++)
                //{
                //    oPurchase.Expenses.ExpenseCode = Convert.ToInt32(qdt.Rows[i][0]);
                //    oPurchase.Expenses.LineTotal = Convert.ToDouble(qdt.Rows[i][1]);
                //    oPurchase.Expenses.Add();
                //}

                string Query, Query1, Query2;
                int BaseLine = 0, BaseEntry = 0, BaseType = 0;
                if(Doc.flgAPRI)
                {
                    Query = "Select x1.LineNum From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                    BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                    Query1 = "Select x1.DocEntry From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                    BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                    Query2 = "Select x.Objtype From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                    BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                }
                else
                {
                    Query = "Select x1.LineNum From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                    BaseLine = Convert.ToInt32(mFm.ExecuteQueryScaler(Query, Program.ConStrSAP));
                    Query1 = "Select x1.DocEntry From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                    BaseEntry = Convert.ToInt32(mFm.ExecuteQueryScaler(Query1, Program.ConStrSAP));
                    Query2 = "Select x.Objtype From OPOR x Inner Join POR1 x1 on x.DocEntry = x1.DocEntry Where x.DocNum = " + Doc.PONumber + " and x1.ItemCode = '" + Doc.ItemCode + "'";
                    BaseType = Convert.ToInt32(mFm.ExecuteQueryScaler(Query2, Program.ConStrSAP));
                }

                oPurchase.Lines.BaseLine = BaseLine;
                oPurchase.Lines.BaseEntry = BaseEntry;
                oPurchase.Lines.BaseType = BaseType;
                oPurchase.Lines.ItemCode = Doc.ItemCode;

                //oPurchase.Lines.UserFields.Fields.Item("U_Yard").Value = Line.Yard;
                //oPurchase.Lines.UserFields.Fields.Item("U_VehcleNo").Value = Line.VehicleNo;
                //oPurchase.Lines.UserFields.Fields.Item("U_Dcnic").Value = Line.DCNIC;
                //oPurchase.Lines.UserFields.Fields.Item("U_Driver").Value = Line.Driver;
                //oPurchase.Lines.UserFields.Fields.Item("U_WeghRef").Value = Doc.BaseDocCollection;
                double QuanityValue = 0;
                if (Doc.ItemGroup.ToUpper().Contains("OIL"))
                {
                    QuanityValue = Doc.oLines.Sum(a => a.NetWeightKG);
                    oPurchase.Lines.Quantity = Doc.oLines.Sum(a => a.NetWeightKG);
                    //oPurchase.Lines.UserFields.Fields.Item("U_FrstWght").Value = Line.FirstWeight;
                    //oPurchase.Lines.UserFields.Fields.Item("U_ScndWght").Value = Line.SecondWeigh;
                }
                else
                {
                    QuanityValue = Doc.oLines.Sum(a => a.NetWeightTon);
                    //oPurchase.Lines.UserFields.Fields.Item("U_FrstWght").Value = Line.FirstWeight;
                    //oPurchase.Lines.UserFields.Fields.Item("U_ScndWght").Value = Line.SecondWeigh;
                }
                oPurchase.Lines.Quantity = QuanityValue;
                //if (Line.TransportId != 0)
                //{
                //    var record = oDB.MstTransportType.Where(x => x.ID == Line.TransportId);
                //    if (record.Count() > 0)
                //    {
                //        transportname = record.FirstOrDefault().Name;
                //    }
                //}
                //oPurchase.Lines.UserFields.Fields.Item("U_Trnstyp").Value = transportname;
                //oPurchase.Lines.UserFields.Fields.Item("U_Trnsprtr").Value = Line.TransporterCode;

                oPurchase.Lines.Add();
                if (oPurchase.Add() != 0)
                //if (true)
                {
                    int ErrorCode = 0; string ErrorDesc = string.Empty;
                    Program.SapCompany.GetLastError(out ErrorCode, out ErrorDesc);
                    retValue = "Sap B1 Error : " + ErrorCode + " : " + ErrorDesc;
                    Program.ExceptionMsg(retValue);
                    Doc.Status = false;
                    Doc.SapDoc = "";
                    Doc.Error = retValue;
                }
                else
                {
                    retValue = Convert.ToString(Program.SapCompany.GetNewObjectKey());
                    //TrnsRawMaterial RawMaterial = oDB.TrnsRawMaterial.Where(x => x.ID == oVal.ID).FirstOrDefault();
                    //RawMaterial.FlgPosted = true;
                    //oDB.SubmitChanges();
                    Doc.Status = true;
                    Doc.SapDoc = retValue;
                    Doc.Error = "";
                    Program.SuccesesMsg("Posted to SBO");
                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.ToString());
                Program.oErrMgn.LogException(Program.ANV, Ex);
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

    }

    public class ConsolidateData
    {
        public string Vendor { get; set; }
        public string PONumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public int TransactionId { get; set; }
    }

    public class GroupGRN
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime SecondWeightDate { get; set; }
        public string BaseDocCollection { get; set; }
        public string PONumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemGroup { get; set; }

        public bool Status { get; set; }
        public bool flgAPRI { get; set; }
        public string SapDoc { get; set; }
        public string Error { get; set; }

        public List<GroupGRNLine> oLines = new List<GroupGRNLine>();
    }

    public class GroupGRNLine
    {
        public int TransactionId { get; set; }
        public string Yard { get; set; }
        public string VehicleNo { get; set; }
        public string DCNIC { get; set; }
        public string Driver { get; set; }
        public string DocRef { get; set; }

        public double NetWeightKG { get; set; }
        public double NetWeightTon { get; set; }
        public double FirstWeight { get; set; }
        public double SecondWeigh { get; set; }
        public int TransportId { get; set; }
        public string TransporterCode { get; set; }
    }
}

