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
using mfmFFSDB;
using mfmFFS.Dialog;


namespace mfmFFS.Screens
{
    public partial class frmCRpt : frmBaseForm
    {
        #region Variables
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        //int OpenObjectID = 1;
        //string Table = "";
        string Customer = "";
        string Customer2 = "";
        string Transporter = "";
        string Transporter2 = "";
        string Supplier = "";
        string Supplier2 = "";
        string DtFrom = "";
        string DtTo = "";
        string ItemCode = "";
        string ItemCode2 = "";
        string RptCode = "";
        string critaria = "";
        #endregion
        private void InitializeForm()
        {
            try
            {
                RBCustomer.Enabled = false;
                RBSupplier.Enabled = false;

                showDateOnly();

                FillRpt();

                FillCustomer();
                FillCustomer2();
                FillSupplier();
                FillSupplier2();
                FillTransport();
                FillTransport2();
                //FillItemCode();
                //FillItemCode2();
                DateEmpty();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public frmCRpt()
        {
            InitializeComponent();
        }
        private void showDateOnly()
        {
            //lblFromDate.Visible = true;
            //lblToDate.Visible = true;
            //lblCustomer.Visible = false;
            //lblsup.Visible = false;
            // lblTransporter.Visible = false;
            cmbCustomer.Enabled = false;
            cmbSupplier.Enabled = false;
            cmbTransporter.Enabled = false;
            cmbItemCode.Enabled = false;
            cmbCustomer2.Enabled = false;
            cmbSupplier2.Enabled = false;
            cmbTransporter2.Enabled = false;
            cmbItemCode2.Enabled = false;
        }
        private void DateEmpty()
        {
            try
            {
                //this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                //this.dtpFrom.CustomFormat = " ";
                //this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                //this.dtpTo.CustomFormat = " ";
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                Customer = "";
                Transporter = "";
                Supplier = "";
                DtFrom = "";
                DtTo = "";
                critaria = "";
                cmbItemCode.Text = null;
                cmbCustomer.Text = null;
                cmbSupplier.Text = null;
                cmbTransporter.Text = null;
                cmbItemCode2.Text = null;
                cmbCustomer2.Text = null;
                cmbSupplier2.Text = null;
                cmbTransporter2.Text = null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void FillRpt()
        {
            try
            {
                var oDoc = (from a in oDB.MstReports where a.FlgReport == true select a.RptCode);
                //var oDoc = oDB.MstReports.Where(x => x.FlgReport == true);
                cmbReportsFor.DataSource = oDoc;
                cmbReportsFor.DisplayMember = "RptCode";
                cmbReportsFor.ValueMember = "ID";

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
            FillItemCode();
            FillItemCode2();
        }
        public void FillTransport()
        {
            try
            {
                //var oDoc = (from a in oDB.MstTransportType select a.Name.Distinct()).ToList();
                var oDoc = oDB.TrnsDispatch.Select(x => x.TransportCode).Distinct();

                cmbTransporter.DataSource = oDoc;
                cmbTransporter.DisplayMember = "TransportCode";
                cmbTransporter.ValueMember = "ID";
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        public void FillTransport2()
        {
            try
            {
                //var oDoc = (from a in oDB.MstTransportType select a.Name.Distinct()).ToList();
                var oDoc = oDB.TrnsDispatch.Select(x => x.TransportCode).Distinct();
                cmbTransporter2.DataSource = oDoc;
                cmbTransporter2.DisplayMember = "TransportCode";
                cmbTransporter2.ValueMember = "ID";
                              
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        public void FillCustomer()
        {
            try
            {
                //var oDoc = (from a in oDB.TrnsDispatch select a.CustomerName.Distinct()).ToList();
                var oDoc = oDB.TrnsDispatch.Select(x => x.CustomerCode).Distinct();
                cmbCustomer.DataSource = oDoc;
                cmbCustomer.DisplayMember = "CustomerCode";
                cmbCustomer.ValueMember = "ID";

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        private void FillCustomer2()
        {
            try
            {
                var oDoc = oDB.TrnsDispatch.Select(x => x.CustomerCode).Distinct();
                cmbCustomer2.DataSource = oDoc;
                cmbCustomer2.DisplayMember = "CustomerCode";
                cmbCustomer2.ValueMember = "ID";
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void FillItemCode()
        {
            try
            {
                cmbItemCode.DataSource = null;
                //if (RptCode == "TrnsDispatch" || RptCode == "TrnsDispatchReturn")
                if (RptCode == "Dispatch" )//|| RptCode == "TrnsDispatchReturn")
                {
                    var oDoc = oDB.TrnsDispatch.Select(x => x.ItemCode).Distinct();
                    cmbItemCode.DataSource = oDoc;
                    cmbItemCode.DisplayMember = "ItemCode";
                    cmbItemCode.ValueMember = "ID";
                  
                }
                //else if (RptCode == "TrnsRawMaterial" || RptCode == "TrnsRawMaterialReturn")
                else if (RptCode == "RawMaterial" )//|| RptCode == "TrnsRawMaterialReturn")
                {
                    var oDoc = oDB.TrnsRawMaterial.Select(x => x.ItemCode).Distinct();
                    cmbItemCode.DataSource = oDoc;
                    cmbItemCode.DisplayMember = "ItemCode";
                    cmbItemCode.ValueMember = "ID";
                    
                }

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        public void FillItemCode2()
        {
            try
            {
                cmbItemCode2.DataSource = null;
                // if (RptCode == "TrnsDispatch" || RptCode == "TrnsDispatchReturn")
                if (RptCode == "Dispatch")
                {
                    var oDoc = oDB.TrnsDispatch.Select(x => x.ItemCode).Distinct();
                    cmbItemCode2.DataSource = oDoc;
                    cmbItemCode2.DisplayMember = "ItemCode";
                    cmbItemCode2.ValueMember = "ID";
                }
                // else if (RptCode == "TrnsRawMaterial" || RptCode == "TrnsRawMaterialReturn")
                else if (RptCode == "RawMaterial")
                {
                    var oDoc = oDB.TrnsRawMaterial.Select(x => x.ItemCode).Distinct();
                    cmbItemCode2.DataSource = oDoc;
                    cmbItemCode2.DisplayMember = "ItemCode";
                    cmbItemCode2.ValueMember = "ID";
                }

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        public void FillSupplier()
        {
            try
            {
                // var oDoc = (from a in oDB.TrnsRawMaterial select a.VendorName.Distinct()).ToList();
                var oDoc = oDB.TrnsRawMaterial.Select(x => x.VendorCode).Distinct();
                cmbSupplier.DataSource = oDoc;
                cmbSupplier.DisplayMember = "VendorCode";
                cmbSupplier.ValueMember = "ID";

            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        public void FillSupplier2()
        {
            try
            {
                // var oDoc = (from a in oDB.TrnsRawMaterial select a.VendorName.Distinct()).ToList();
                var oDoc = oDB.TrnsRawMaterial.Select(x => x.VendorCode).Distinct();
           
                cmbSupplier2.DataSource = oDoc;
                cmbSupplier2.DisplayMember = "VendorCode";
                cmbSupplier2.ValueMember = "ID";
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void RBTransporter_Click(object sender, EventArgs e)
        {
            DateEmpty();
            try
            {
                cmbCustomer.Enabled = false;
                cmbSupplier.Enabled = false;
                cmbTransporter.Enabled = true;
                cmbItemCode.Enabled = false;
                cmbCustomer2.Enabled = false;
                cmbSupplier2.Enabled = false;
                cmbTransporter2.Enabled = true;
                cmbItemCode2.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RBSupplier_Click(object sender, EventArgs e)
        {
            DateEmpty();
            try
            {
                cmbCustomer.Enabled = false;
                cmbSupplier.Enabled = true;
                cmbTransporter.Enabled = false;
                cmbItemCode.Enabled = false;
                cmbCustomer2.Enabled = false;
                cmbSupplier2.Enabled = true;
                cmbTransporter2.Enabled = false;
                cmbItemCode2.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RBCustomer_Click(object sender, EventArgs e)
        {
            DateEmpty();
            try
            {
                cmbCustomer.Enabled = true;
                cmbSupplier.Enabled = false;
                cmbItemCode.Enabled = false;
                cmbTransporter.Enabled = false;
                cmbCustomer2.Enabled = true;
                cmbSupplier2.Enabled = false;
                cmbItemCode2.Enabled = false;
                cmbTransporter2.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RBItemCode_Click(object sender, EventArgs e)
        {
            try
            {
                FillItemCode();
                FillItemCode2();
                DateEmpty();
                cmbCustomer.Enabled = false;
                cmbSupplier.Enabled = false;
                cmbTransporter.Enabled = false;
                cmbItemCode.Enabled = true;
                cmbCustomer2.Enabled = false;
                cmbSupplier2.Enabled = false;
                cmbTransporter2.Enabled = false;
                cmbItemCode2.Enabled = true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dtpFrom_Enter(object sender, EventArgs e)
        {
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.CustomFormat = "yyyy/MM/dd";
        }

        private void dtpTo_Enter(object sender, EventArgs e)
        {
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.CustomFormat = "yyyy/MM/dd";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string RptFor = cmbReportsFor.Text;
                frmReportViewer fpv = new frmReportViewer();
                Program.rpt = RptFor;
                string PldFor = "";
     
                switch (RptCode)
                {
                    case "Dispatch":
                        if (rptDispatch())
                        {
                            fpv.OpenReport(PldFor, critaria, PldFor + " " + RptFor, DtTo, DtFrom, Customer, Customer2, Transporter, Transporter2, Supplier, Supplier2, ItemCode, ItemCode2);
                            
                        }
                        break;
              
                    case "RawMaterial":
                        if (rptRawMaterial())
                        {
                            fpv.OpenReport(PldFor, critaria, PldFor + " " + RptFor, DtTo, DtFrom, Customer, Customer2, Transporter, Transporter2, Supplier, Supplier2, ItemCode, ItemCode2);
                        }
                        break;
                 
                    default:
                        break;
                }
                //frmReportViewer.rptDocument.Close();
                //frmReportViewer.rptDocument.Dispose();
                //frmReportViewer.rptDocument = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void frmCRpt_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeForm();
            }
            catch (Exception Ex)
            {
            }
        }
        private bool rptDispatch()
        {
            try
            {
                critaria = "";
                if (RBCustomer.IsChecked == true)
                {
                    Customer = cmbCustomer.Text;
                    Customer2 = cmbCustomer2.Text;
                }
                else if (RBTransporter.IsChecked == true)
                {
                    Transporter = cmbTransporter.Text;
                    Transporter2 = cmbTransporter2.Text;
                }
                else if (RBItemCode.IsChecked == true)
                {
                    ItemCode = cmbItemCode.Text;
                    ItemCode2 = cmbItemCode2.Text;
                }
                if (dtpFrom.Text != " " && dtpTo.Text != " ")
                {
                    DtFrom = dtpFrom.Value.ToString("yyyy/MM/dd");
                    DtTo = dtpTo.Value.ToString("yyyy/MM/dd");
                }
                if (dtpFrom.Text == " " && dtpTo.Text != " " || dtpFrom.Text != " " && dtpTo.Text == " ")
                {
                    Program.ExceptionMsg("");
                    return false;
                }

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
        //private bool rptDispatchReturn()
        //{
        //    try
        //    {
        //        if (RBCustomer.IsChecked == true)
        //        {
        //            Customer = cmbCustomer.Text;
        //        }
        //        else if (RBTransporter.IsChecked == true)
        //        {
        //            Transporter = cmbTransporter.Text;
        //        }
        //        if (dtpFrom.Text != " " && dtpTo.Text != " ")
        //        {
        //            DtFrom = dtpFrom.Text;
        //            DtTo = dtpTo.Text;
        //        }
        //        if (dtpFrom.Text == " " && dtpTo.Text != " " || dtpFrom.Text != " " && dtpTo.Text == " ")
        //        {
        //            Program.ExceptionMsg("");
        //            return false;
        //        }

        //        return true;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        private bool rptRawMaterial()
        {
            try
            {
                if (RBSupplier.IsChecked == true)
                {
                    Supplier = cmbSupplier.Text;
                    Supplier2 = cmbSupplier2.Text;
                }
                else if (RBTransporter.IsChecked == true)
                {
                    Transporter = cmbTransporter.Text;
                    Transporter2 = cmbTransporter2.Text;
                }
                else if (RBItemCode.IsChecked == true)
                {
                    ItemCode = cmbItemCode.Text;
                    ItemCode2 = cmbItemCode2.Text;
                }
                if (dtpFrom.Text != " " && dtpTo.Text != " ")
                {
                    DtFrom = dtpFrom.Text;
                    DtTo = dtpTo.Text;
                }
                if (dtpFrom.Text == " " && dtpTo.Text != " " || dtpFrom.Text != " " && dtpTo.Text == " ")
                {
                    Program.ExceptionMsg("");
                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //private void rptRawMaterialReturn()
        //{
        //    try
        //    {
        //        if (RBCustomer.IsChecked == true)
        //        {
        //            Supplier = cmbSupplier.Text;
        //        }
        //        else if (RBTransporter.IsChecked == true)
        //        {
        //            Transporter = cmbTransporter.Text;
        //        }
        //        if (dtpFrom.Text != " " && dtpTo.Text != " ")
        //        {
        //            DtFrom = dtpFrom.Text;
        //            DtTo = dtpTo.Text;
        //        }
        //        //else if (dtpFrom.Text != "" || dtpTo.Text != "")
        //        //{
        //        //    Program.WarningMsg("Kindly select Both Dates");
        //        //}

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        private void cmbReportsFor_TextChanged(object sender, EventArgs e)
        {
            DateEmpty();
            try
            {
                RptCode = (from a in oDB.MstReports where a.RptCode == cmbReportsFor.Text select a.RptName).FirstOrDefault();
                // RptCode = oDB.MstReports.Where(x => x.RptName == RptFor).FirstOrDefault().RptCode;
                //if (RptCode == "TrnsDispatch" || RptCode == "TrnsDispatchReturn")
                if (RptCode == "Dispatch" )//|| RptCode == "TrnsDispatchReturn")
                {
                    RBCustomer.Enabled = true;
                    //cmbCustomer.Enabled = true;
                    RBSupplier.Enabled = false;
                    cmbSupplier.Enabled = false;
                    cmbSupplier2.Enabled = false;

                }

                //                else if (RptCode == "TrnsRawMaterial" || RptCode == "TrnsRawMaterialReturn")
                else if (RptCode == "RawMaterial" )//|| RptCode == "TrnsRawMaterialReturn")
                {
                    //cmbSupplier.Enabled = true;
                    RBSupplier.Enabled = true;
                    cmbCustomer.Enabled = false;
                    cmbCustomer2.Enabled = false;
                    RBCustomer.Enabled = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RBDate_Click(object sender, EventArgs e)
        {
            try
            {
                DateEmpty();
                cmbCustomer.Enabled = false;
                cmbSupplier.Enabled = false;
                cmbItemCode.Enabled = false;
                cmbTransporter.Enabled = false;
                cmbCustomer2.Enabled = false;
                cmbSupplier2.Enabled = false;
                cmbItemCode2.Enabled = false;
                cmbTransporter2.Enabled = false;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.mytabpage.Dispose();
        }
    }
}
