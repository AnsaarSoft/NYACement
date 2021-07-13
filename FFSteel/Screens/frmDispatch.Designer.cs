using mfmFFSDB;

namespace mfmFFS.Screens
{
    partial class frmDispatch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDispatch));
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.tmrCamFront = new System.Windows.Forms.Timer(this.components);
            this.tmrAlreadyReading = new System.Windows.Forms.Timer(this.components);
            this.tmrCamBack = new System.Windows.Forms.Timer(this.components);
            this.txtFullText = new Telerik.WinControls.UI.RadTextBox();
            this.txtCWeight = new Telerik.WinControls.UI.RadTextBox();
            this.pnlControls = new Telerik.WinControls.UI.RadPanel();
            this.btnPrint = new Telerik.WinControls.UI.RadButton();
            this.lblWeight = new Telerik.WinControls.UI.RadLabel();
            this.btnLastRecord = new Telerik.WinControls.UI.RadButton();
            this.btnNextRecord = new Telerik.WinControls.UI.RadButton();
            this.btnPreviosRecord = new Telerik.WinControls.UI.RadButton();
            this.btnFirstRecord = new Telerik.WinControls.UI.RadButton();
            this.btnAddNew = new Telerik.WinControls.UI.RadButton();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.btnGetWeight = new Telerik.WinControls.UI.RadButton();
            this.BtnItemSelect = new Telerik.WinControls.UI.RadButton();
            this.txtDifferenceWeight = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel36 = new Telerik.WinControls.UI.RadLabel();
            this.txtCustomerName = new Telerik.WinControls.UI.RadTextBox();
            this.btnBulkTable = new Telerik.WinControls.UI.RadButton();
            this.txtBrandPath = new Telerik.WinControls.UI.RadTextBox();
            this.txtDaySeries = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel34 = new Telerik.WinControls.UI.RadLabel();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radLabel35 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel32 = new Telerik.WinControls.UI.RadLabel();
            this.txtCNICPath = new Telerik.WinControls.UI.RadTextBox();
            this.txtItemGroupName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel33 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel30 = new Telerik.WinControls.UI.RadLabel();
            this.txtNetWeightTon = new Telerik.WinControls.UI.RadTextBox();
            this.txt2WeightTon = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel28 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel13 = new Telerik.WinControls.UI.RadLabel();
            this.txtNetWeightKG = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel29 = new Telerik.WinControls.UI.RadLabel();
            this.txt2WeightTime = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel25 = new Telerik.WinControls.UI.RadLabel();
            this.txt2WeightKG = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel26 = new Telerik.WinControls.UI.RadLabel();
            this.txt2WeightDate = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel27 = new Telerik.WinControls.UI.RadLabel();
            this.cmbTransportCode = new Telerik.WinControls.UI.RadDropDownList();
            this.cmbTransportType = new Telerik.WinControls.UI.RadDropDownList();
            this.cmbPacker = new Telerik.WinControls.UI.RadDropDownList();
            this.TxtTransportName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel23 = new Telerik.WinControls.UI.RadLabel();
            this.txt1WeightTon = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel24 = new Telerik.WinControls.UI.RadLabel();
            this.txt1WeightTime = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel21 = new Telerik.WinControls.UI.RadLabel();
            this.txtDriverName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel22 = new Telerik.WinControls.UI.RadLabel();
            this.txt1WeightKG = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel19 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel17 = new Telerik.WinControls.UI.RadLabel();
            this.txtSBRNum = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel20 = new Telerik.WinControls.UI.RadLabel();
            this.txt1WeightDate = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel18 = new Telerik.WinControls.UI.RadLabel();
            this.txtDriverCNIC = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel14 = new Telerik.WinControls.UI.RadLabel();
            this.txtVehicleNum = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel15 = new Telerik.WinControls.UI.RadLabel();
            this.txtBalanceQuantity = new Telerik.WinControls.UI.RadTextBox();
            this.txtSBRDate = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel16 = new Telerik.WinControls.UI.RadLabel();
            this.BtnDocSelect = new Telerik.WinControls.UI.RadButton();
            this.txtOrderQuantity = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.grdDetails = new Telerik.WinControls.UI.RadGridView();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnSubmit = new Telerik.WinControls.UI.RadButton();
            this.radLabel9 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel10 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel11 = new Telerik.WinControls.UI.RadLabel();
            this.txtCurrTime = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel12 = new Telerik.WinControls.UI.RadLabel();
            this.txtItemNam = new Telerik.WinControls.UI.RadTextBox();
            this.txtShift = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtItemCod = new Telerik.WinControls.UI.RadTextBox();
            this.txtCurrDate = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel7 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.txtCustomerCode = new Telerik.WinControls.UI.RadTextBox();
            this.txtDocNo = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel8 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.lbl2 = new System.Windows.Forms.Label();
            this.btnRefresh = new Telerik.WinControls.UI.RadButton();
            this.btnCust = new Telerik.WinControls.UI.RadButton();
            this.radLabel31 = new Telerik.WinControls.UI.RadLabel();
            this.chkAllowTolerance = new Telerik.WinControls.UI.RadCheckBox();
            this.lblToleranceLimit = new Telerik.WinControls.UI.RadLabel();
            this.txtToleranceLimit = new Telerik.WinControls.UI.RadTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbBridge02 = new Telerik.WinControls.UI.RadRadioButton();
            this.rbBridge01 = new Telerik.WinControls.UI.RadRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).BeginInit();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLastRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNextRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPreviosRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFirstRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnItemSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDifferenceWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel36)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBulkTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBrandPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDaySeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel35)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCNICPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemGroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel30)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNetWeightTon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightTon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNetWeightKG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel29)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightKG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTransportCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTransportType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPacker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTransportName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightTon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriverName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightKG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBRNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriverCNIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVehicleNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalanceQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBRDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnDocSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrderQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetails.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSubmit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllowTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToleranceLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToleranceLimit)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbBridge02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbBridge01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrAlreadyReading
            // 
            this.tmrAlreadyReading.Interval = 1000;
            this.tmrAlreadyReading.Tick += new System.EventHandler(this.tmrAlreadyReading_Tick);
            // 
            // txtFullText
            // 
            this.txtFullText.Location = new System.Drawing.Point(1002, 44);
            this.txtFullText.Name = "txtFullText";
            this.txtFullText.Size = new System.Drawing.Size(15, 20);
            this.txtFullText.TabIndex = 133;
            this.txtFullText.Visible = false;
            this.txtFullText.TextChanged += new System.EventHandler(this.txtFullText_TextChanged);
            // 
            // txtCWeight
            // 
            this.txtCWeight.Location = new System.Drawing.Point(1002, 19);
            this.txtCWeight.Name = "txtCWeight";
            this.txtCWeight.Size = new System.Drawing.Size(15, 20);
            this.txtCWeight.TabIndex = 133;
            this.txtCWeight.Visible = false;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pnlControls.Controls.Add(this.btnPrint);
            this.pnlControls.Controls.Add(this.lblWeight);
            this.pnlControls.Controls.Add(this.btnLastRecord);
            this.pnlControls.Controls.Add(this.btnNextRecord);
            this.pnlControls.Controls.Add(this.btnPreviosRecord);
            this.pnlControls.Controls.Add(this.btnFirstRecord);
            this.pnlControls.Controls.Add(this.btnAddNew);
            this.pnlControls.Controls.Add(this.btnSearch);
            this.pnlControls.Location = new System.Drawing.Point(14, 12);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(625, 40);
            this.pnlControls.TabIndex = 132;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrint.Location = new System.Drawing.Point(105, 7);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(50, 26);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = false;
            this.lblWeight.BackColor = System.Drawing.Color.Black;
            this.lblWeight.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeight.ForeColor = System.Drawing.Color.Lime;
            this.lblWeight.Location = new System.Drawing.Point(449, 3);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(163, 32);
            this.lblWeight.TabIndex = 13;
            this.lblWeight.Text = "0";
            this.lblWeight.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLastRecord
            // 
            this.btnLastRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnLastRecord.Image")));
            this.btnLastRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLastRecord.Location = new System.Drawing.Point(306, 7);
            this.btnLastRecord.Name = "btnLastRecord";
            this.btnLastRecord.Size = new System.Drawing.Size(50, 26);
            this.btnLastRecord.TabIndex = 12;
            this.btnLastRecord.Click += new System.EventHandler(this.btnLastRecord_Click_1);
            // 
            // btnNextRecord
            // 
            this.btnNextRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnNextRecord.Image")));
            this.btnNextRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNextRecord.Location = new System.Drawing.Point(256, 7);
            this.btnNextRecord.Name = "btnNextRecord";
            this.btnNextRecord.Size = new System.Drawing.Size(50, 26);
            this.btnNextRecord.TabIndex = 11;
            this.btnNextRecord.Click += new System.EventHandler(this.btnNextRecord_Click_1);
            // 
            // btnPreviosRecord
            // 
            this.btnPreviosRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnPreviosRecord.Image")));
            this.btnPreviosRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPreviosRecord.Location = new System.Drawing.Point(208, 7);
            this.btnPreviosRecord.Name = "btnPreviosRecord";
            this.btnPreviosRecord.Size = new System.Drawing.Size(50, 26);
            this.btnPreviosRecord.TabIndex = 11;
            this.btnPreviosRecord.Click += new System.EventHandler(this.btnPreviosRecord_Click_1);
            // 
            // btnFirstRecord
            // 
            this.btnFirstRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnFirstRecord.Image")));
            this.btnFirstRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFirstRecord.Location = new System.Drawing.Point(156, 7);
            this.btnFirstRecord.Name = "btnFirstRecord";
            this.btnFirstRecord.Size = new System.Drawing.Size(50, 26);
            this.btnFirstRecord.TabIndex = 10;
            this.btnFirstRecord.Click += new System.EventHandler(this.btnFirstRecord_Click_1);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddNew.Location = new System.Drawing.Point(54, 7);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(50, 26);
            this.btnAddNew.TabIndex = 9;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(3, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 26);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // btnGetWeight
            // 
            this.btnGetWeight.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetWeight.Location = new System.Drawing.Point(672, 15);
            this.btnGetWeight.Name = "btnGetWeight";
            this.btnGetWeight.Size = new System.Drawing.Size(150, 26);
            this.btnGetWeight.TabIndex = 10;
            this.btnGetWeight.Text = "&Getweight";
            this.btnGetWeight.Click += new System.EventHandler(this.btnGetWeight_Click);
            // 
            // BtnItemSelect
            // 
            this.BtnItemSelect.Image = global::mfmWeighment.Properties.Resources.Folder16;
            this.BtnItemSelect.Location = new System.Drawing.Point(274, 132);
            this.BtnItemSelect.Name = "BtnItemSelect";
            this.BtnItemSelect.Size = new System.Drawing.Size(20, 20);
            this.BtnItemSelect.TabIndex = 131;
            this.BtnItemSelect.Click += new System.EventHandler(this.BtnItemSelect_Click);
            // 
            // txtDifferenceWeight
            // 
            this.txtDifferenceWeight.Location = new System.Drawing.Point(462, 441);
            this.txtDifferenceWeight.Name = "txtDifferenceWeight";
            this.txtDifferenceWeight.Size = new System.Drawing.Size(159, 20);
            this.txtDifferenceWeight.TabIndex = 129;
            // 
            // radLabel36
            // 
            this.radLabel36.Location = new System.Drawing.Point(299, 441);
            this.radLabel36.Name = "radLabel36";
            this.radLabel36.Size = new System.Drawing.Size(169, 18);
            this.radLabel36.TabIndex = 130;
            this.radLabel36.Text = "Difference SO & Net Weight(Tons)";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(462, 109);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(159, 20);
            this.txtCustomerName.TabIndex = 61;
            // 
            // btnBulkTable
            // 
            this.btnBulkTable.Location = new System.Drawing.Point(119, 350);
            this.btnBulkTable.Name = "btnBulkTable";
            this.btnBulkTable.Size = new System.Drawing.Size(153, 24);
            this.btnBulkTable.TabIndex = 72;
            this.btnBulkTable.Text = "Table";
            this.btnBulkTable.Click += new System.EventHandler(this.btnBulkTable_Click);
            // 
            // txtBrandPath
            // 
            this.txtBrandPath.Location = new System.Drawing.Point(462, 352);
            this.txtBrandPath.Name = "txtBrandPath";
            this.txtBrandPath.Size = new System.Drawing.Size(159, 20);
            this.txtBrandPath.TabIndex = 124;
            this.txtBrandPath.Visible = false;
            // 
            // txtDaySeries
            // 
            this.txtDaySeries.Location = new System.Drawing.Point(462, 199);
            this.txtDaySeries.Name = "txtDaySeries";
            this.txtDaySeries.Size = new System.Drawing.Size(159, 20);
            this.txtDaySeries.TabIndex = 59;
            // 
            // radLabel34
            // 
            this.radLabel34.Location = new System.Drawing.Point(299, 353);
            this.radLabel34.Name = "radLabel34";
            this.radLabel34.Size = new System.Drawing.Size(64, 18);
            this.radLabel34.TabIndex = 125;
            this.radLabel34.Text = "Brand Logo";
            this.radLabel34.Visible = false;
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(623, 328);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(43, 24);
            this.radButton2.TabIndex = 71;
            this.radButton2.Text = "Browse";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radLabel35
            // 
            this.radLabel35.Location = new System.Drawing.Point(11, 353);
            this.radLabel35.Name = "radLabel35";
            this.radLabel35.Size = new System.Drawing.Size(66, 18);
            this.radLabel35.TabIndex = 123;
            this.radLabel35.Text = "Bulker Seals";
            // 
            // radLabel32
            // 
            this.radLabel32.Location = new System.Drawing.Point(299, 200);
            this.radLabel32.Name = "radLabel32";
            this.radLabel32.Size = new System.Drawing.Size(74, 18);
            this.radLabel32.TabIndex = 61;
            this.radLabel32.Text = "Day Serial No";
            // 
            // txtCNICPath
            // 
            this.txtCNICPath.Location = new System.Drawing.Point(462, 330);
            this.txtCNICPath.Name = "txtCNICPath";
            this.txtCNICPath.Size = new System.Drawing.Size(159, 20);
            this.txtCNICPath.TabIndex = 113;
            // 
            // txtItemGroupName
            // 
            this.txtItemGroupName.Location = new System.Drawing.Point(119, 199);
            this.txtItemGroupName.Name = "txtItemGroupName";
            this.txtItemGroupName.Size = new System.Drawing.Size(153, 20);
            this.txtItemGroupName.TabIndex = 58;
            this.txtItemGroupName.TextChanged += new System.EventHandler(this.txtItemGroupName_TextChanged);
            // 
            // radLabel33
            // 
            this.radLabel33.Location = new System.Drawing.Point(11, 200);
            this.radLabel33.Name = "radLabel33";
            this.radLabel33.Size = new System.Drawing.Size(96, 18);
            this.radLabel33.TabIndex = 60;
            this.radLabel33.Text = "Item Group Name";
            // 
            // radLabel30
            // 
            this.radLabel30.Location = new System.Drawing.Point(299, 331);
            this.radLabel30.Name = "radLabel30";
            this.radLabel30.Size = new System.Drawing.Size(143, 18);
            this.radLabel30.TabIndex = 114;
            this.radLabel30.Text = "Scanning of driver’s ID card";
            // 
            // txtNetWeightTon
            // 
            this.txtNetWeightTon.Location = new System.Drawing.Point(462, 419);
            this.txtNetWeightTon.Name = "txtNetWeightTon";
            this.txtNetWeightTon.Size = new System.Drawing.Size(159, 20);
            this.txtNetWeightTon.TabIndex = 126;
            // 
            // txt2WeightTon
            // 
            this.txt2WeightTon.Location = new System.Drawing.Point(462, 396);
            this.txt2WeightTon.Name = "txt2WeightTon";
            this.txt2WeightTon.Size = new System.Drawing.Size(159, 20);
            this.txt2WeightTon.TabIndex = 122;
            // 
            // radLabel28
            // 
            this.radLabel28.Location = new System.Drawing.Point(299, 420);
            this.radLabel28.Name = "radLabel28";
            this.radLabel28.Size = new System.Drawing.Size(94, 18);
            this.radLabel28.TabIndex = 127;
            this.radLabel28.Text = "Net Weight(Tons)";
            // 
            // radLabel13
            // 
            this.radLabel13.Location = new System.Drawing.Point(299, 397);
            this.radLabel13.Name = "radLabel13";
            this.radLabel13.Size = new System.Drawing.Size(113, 18);
            this.radLabel13.TabIndex = 123;
            this.radLabel13.Text = "Second Weight(Tons)";
            // 
            // txtNetWeightKG
            // 
            this.txtNetWeightKG.Location = new System.Drawing.Point(119, 419);
            this.txtNetWeightKG.Name = "txtNetWeightKG";
            this.txtNetWeightKG.Size = new System.Drawing.Size(153, 20);
            this.txtNetWeightKG.TabIndex = 124;
            // 
            // radLabel29
            // 
            this.radLabel29.Location = new System.Drawing.Point(11, 420);
            this.radLabel29.Name = "radLabel29";
            this.radLabel29.Size = new System.Drawing.Size(85, 18);
            this.radLabel29.TabIndex = 125;
            this.radLabel29.Text = "Net Weight(KG)";
            // 
            // txt2WeightTime
            // 
            this.txt2WeightTime.Location = new System.Drawing.Point(462, 374);
            this.txt2WeightTime.Name = "txt2WeightTime";
            this.txt2WeightTime.Size = new System.Drawing.Size(159, 20);
            this.txt2WeightTime.TabIndex = 120;
            // 
            // radLabel25
            // 
            this.radLabel25.Location = new System.Drawing.Point(299, 375);
            this.radLabel25.Name = "radLabel25";
            this.radLabel25.Size = new System.Drawing.Size(110, 18);
            this.radLabel25.TabIndex = 121;
            this.radLabel25.Text = "Second Weight Time";
            // 
            // txt2WeightKG
            // 
            this.txt2WeightKG.Location = new System.Drawing.Point(119, 396);
            this.txt2WeightKG.Name = "txt2WeightKG";
            this.txt2WeightKG.Size = new System.Drawing.Size(153, 20);
            this.txt2WeightKG.TabIndex = 118;
            this.txt2WeightKG.TextChanged += new System.EventHandler(this.txt2WeightKG_TextChanged);
            this.txt2WeightKG.Click += new System.EventHandler(this.txt2WeightKG_Click);
            // 
            // radLabel26
            // 
            this.radLabel26.Location = new System.Drawing.Point(11, 397);
            this.radLabel26.Name = "radLabel26";
            this.radLabel26.Size = new System.Drawing.Size(104, 18);
            this.radLabel26.TabIndex = 119;
            this.radLabel26.Text = "Second Weight(KG)";
            // 
            // txt2WeightDate
            // 
            this.txt2WeightDate.Location = new System.Drawing.Point(119, 374);
            this.txt2WeightDate.Name = "txt2WeightDate";
            this.txt2WeightDate.Size = new System.Drawing.Size(153, 20);
            this.txt2WeightDate.TabIndex = 116;
            // 
            // radLabel27
            // 
            this.radLabel27.Location = new System.Drawing.Point(11, 375);
            this.radLabel27.Name = "radLabel27";
            this.radLabel27.Size = new System.Drawing.Size(109, 18);
            this.radLabel27.TabIndex = 117;
            this.radLabel27.Text = "Second Weight Date";
            // 
            // cmbTransportCode
            // 
            this.cmbTransportCode.Items.Add(radListDataItem1);
            this.cmbTransportCode.Location = new System.Drawing.Point(119, 308);
            this.cmbTransportCode.Name = "cmbTransportCode";
            this.cmbTransportCode.Size = new System.Drawing.Size(153, 20);
            this.cmbTransportCode.TabIndex = 115;
            this.cmbTransportCode.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.cmbTransportCode_SelectedIndexChanged);
            // 
            // cmbTransportType
            // 
            this.cmbTransportType.Location = new System.Drawing.Point(119, 330);
            this.cmbTransportType.Name = "cmbTransportType";
            this.cmbTransportType.Size = new System.Drawing.Size(153, 20);
            this.cmbTransportType.TabIndex = 114;
            // 
            // cmbPacker
            // 
            this.cmbPacker.Location = new System.Drawing.Point(462, 221);
            this.cmbPacker.Name = "cmbPacker";
            this.cmbPacker.Size = new System.Drawing.Size(159, 20);
            this.cmbPacker.TabIndex = 113;
            // 
            // TxtTransportName
            // 
            this.TxtTransportName.Location = new System.Drawing.Point(462, 308);
            this.TxtTransportName.Name = "TxtTransportName";
            this.TxtTransportName.Size = new System.Drawing.Size(159, 20);
            this.TxtTransportName.TabIndex = 111;
            // 
            // radLabel23
            // 
            this.radLabel23.Location = new System.Drawing.Point(299, 309);
            this.radLabel23.Name = "radLabel23";
            this.radLabel23.Size = new System.Drawing.Size(87, 18);
            this.radLabel23.TabIndex = 112;
            this.radLabel23.Text = "Transport Name";
            // 
            // txt1WeightTon
            // 
            this.txt1WeightTon.Location = new System.Drawing.Point(462, 287);
            this.txt1WeightTon.Name = "txt1WeightTon";
            this.txt1WeightTon.Size = new System.Drawing.Size(159, 20);
            this.txt1WeightTon.TabIndex = 109;
            // 
            // radLabel24
            // 
            this.radLabel24.Location = new System.Drawing.Point(299, 288);
            this.radLabel24.Name = "radLabel24";
            this.radLabel24.Size = new System.Drawing.Size(97, 18);
            this.radLabel24.TabIndex = 110;
            this.radLabel24.Text = "First Weight(Tons)";
            // 
            // txt1WeightTime
            // 
            this.txt1WeightTime.Location = new System.Drawing.Point(462, 265);
            this.txt1WeightTime.Name = "txt1WeightTime";
            this.txt1WeightTime.Size = new System.Drawing.Size(159, 20);
            this.txt1WeightTime.TabIndex = 107;
            // 
            // radLabel21
            // 
            this.radLabel21.Location = new System.Drawing.Point(299, 266);
            this.radLabel21.Name = "radLabel21";
            this.radLabel21.Size = new System.Drawing.Size(94, 18);
            this.radLabel21.TabIndex = 108;
            this.radLabel21.Text = "First Weight Time";
            // 
            // txtDriverName
            // 
            this.txtDriverName.Location = new System.Drawing.Point(462, 243);
            this.txtDriverName.Name = "txtDriverName";
            this.txtDriverName.Size = new System.Drawing.Size(159, 20);
            this.txtDriverName.TabIndex = 105;
            // 
            // radLabel22
            // 
            this.radLabel22.Location = new System.Drawing.Point(299, 244);
            this.radLabel22.Name = "radLabel22";
            this.radLabel22.Size = new System.Drawing.Size(69, 18);
            this.radLabel22.TabIndex = 106;
            this.radLabel22.Text = "Driver Name";
            // 
            // txt1WeightKG
            // 
            this.txt1WeightKG.Location = new System.Drawing.Point(119, 287);
            this.txt1WeightKG.Name = "txt1WeightKG";
            this.txt1WeightKG.Size = new System.Drawing.Size(153, 20);
            this.txt1WeightKG.TabIndex = 99;
            this.txt1WeightKG.TextChanged += new System.EventHandler(this.txt1WeightKG_TextChanged);
            // 
            // radLabel19
            // 
            this.radLabel19.Location = new System.Drawing.Point(11, 331);
            this.radLabel19.Name = "radLabel19";
            this.radLabel19.Size = new System.Drawing.Size(81, 18);
            this.radLabel19.TabIndex = 104;
            this.radLabel19.Text = "Transport Type";
            // 
            // radLabel17
            // 
            this.radLabel17.Location = new System.Drawing.Point(11, 288);
            this.radLabel17.Name = "radLabel17";
            this.radLabel17.Size = new System.Drawing.Size(87, 18);
            this.radLabel17.TabIndex = 100;
            this.radLabel17.Text = "First Weight(KG)";
            // 
            // txtSBRNum
            // 
            this.txtSBRNum.Location = new System.Drawing.Point(119, 154);
            this.txtSBRNum.Name = "txtSBRNum";
            this.txtSBRNum.Size = new System.Drawing.Size(153, 20);
            this.txtSBRNum.TabIndex = 101;
            // 
            // radLabel20
            // 
            this.radLabel20.Location = new System.Drawing.Point(11, 309);
            this.radLabel20.Name = "radLabel20";
            this.radLabel20.Size = new System.Drawing.Size(83, 18);
            this.radLabel20.TabIndex = 102;
            this.radLabel20.Text = "Transport Code";
            // 
            // txt1WeightDate
            // 
            this.txt1WeightDate.Location = new System.Drawing.Point(119, 265);
            this.txt1WeightDate.Name = "txt1WeightDate";
            this.txt1WeightDate.Size = new System.Drawing.Size(153, 20);
            this.txt1WeightDate.TabIndex = 97;
            // 
            // radLabel18
            // 
            this.radLabel18.Location = new System.Drawing.Point(11, 266);
            this.radLabel18.Name = "radLabel18";
            this.radLabel18.Size = new System.Drawing.Size(93, 18);
            this.radLabel18.TabIndex = 98;
            this.radLabel18.Text = "First Weight Date";
            // 
            // txtDriverCNIC
            // 
            this.txtDriverCNIC.Location = new System.Drawing.Point(119, 243);
            this.txtDriverCNIC.Name = "txtDriverCNIC";
            this.txtDriverCNIC.Size = new System.Drawing.Size(153, 20);
            this.txtDriverCNIC.TabIndex = 95;
            // 
            // radLabel14
            // 
            this.radLabel14.Location = new System.Drawing.Point(11, 244);
            this.radLabel14.Name = "radLabel14";
            this.radLabel14.Size = new System.Drawing.Size(61, 18);
            this.radLabel14.TabIndex = 96;
            this.radLabel14.Text = "Driver Cnic";
            // 
            // txtVehicleNum
            // 
            this.txtVehicleNum.Location = new System.Drawing.Point(119, 221);
            this.txtVehicleNum.Name = "txtVehicleNum";
            this.txtVehicleNum.Size = new System.Drawing.Size(153, 20);
            this.txtVehicleNum.TabIndex = 93;
            // 
            // radLabel15
            // 
            this.radLabel15.Location = new System.Drawing.Point(11, 222);
            this.radLabel15.Name = "radLabel15";
            this.radLabel15.Size = new System.Drawing.Size(60, 18);
            this.radLabel15.TabIndex = 94;
            this.radLabel15.Text = "Vehicle No";
            // 
            // txtBalanceQuantity
            // 
            this.txtBalanceQuantity.Location = new System.Drawing.Point(462, 177);
            this.txtBalanceQuantity.Name = "txtBalanceQuantity";
            this.txtBalanceQuantity.Size = new System.Drawing.Size(159, 20);
            this.txtBalanceQuantity.TabIndex = 87;
            // 
            // txtSBRDate
            // 
            this.txtSBRDate.Location = new System.Drawing.Point(462, 154);
            this.txtSBRDate.Name = "txtSBRDate";
            this.txtSBRDate.Size = new System.Drawing.Size(159, 20);
            this.txtSBRDate.TabIndex = 86;
            // 
            // radLabel16
            // 
            this.radLabel16.Location = new System.Drawing.Point(299, 222);
            this.radLabel16.Name = "radLabel16";
            this.radLabel16.Size = new System.Drawing.Size(46, 18);
            this.radLabel16.TabIndex = 85;
            this.radLabel16.Text = "Packer#";
            // 
            // BtnDocSelect
            // 
            this.BtnDocSelect.Image = global::mfmWeighment.Properties.Resources.Folder16;
            this.BtnDocSelect.Location = new System.Drawing.Point(274, 154);
            this.BtnDocSelect.Name = "BtnDocSelect";
            this.BtnDocSelect.Size = new System.Drawing.Size(20, 20);
            this.BtnDocSelect.TabIndex = 84;
            this.BtnDocSelect.Click += new System.EventHandler(this.btnItem_Click);
            // 
            // txtOrderQuantity
            // 
            this.txtOrderQuantity.Location = new System.Drawing.Point(119, 177);
            this.txtOrderQuantity.Name = "txtOrderQuantity";
            this.txtOrderQuantity.Size = new System.Drawing.Size(153, 20);
            this.txtOrderQuantity.TabIndex = 76;
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(11, 178);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(93, 18);
            this.radLabel4.TabIndex = 77;
            this.radLabel4.Text = "Loading Quantity";
            // 
            // grdDetails
            // 
            this.grdDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.grdDetails.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.grdDetails.ForeColor = System.Drawing.Color.Black;
            this.grdDetails.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.grdDetails.Location = new System.Drawing.Point(672, 68);
            // 
            // 
            // 
            this.grdDetails.MasterTemplate.AllowAddNewRow = false;
            this.grdDetails.MasterTemplate.AllowDeleteRow = false;
            this.grdDetails.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "ID";
            gridViewTextBoxColumn1.HeaderText = "ID";
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "ID";
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "Wmnt#";
            gridViewTextBoxColumn2.HeaderText = "Wmnt#";
            gridViewTextBoxColumn2.MinWidth = 0;
            gridViewTextBoxColumn2.Name = "Wmnt#";
            gridViewTextBoxColumn2.Width = 41;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Vehicle#";
            gridViewTextBoxColumn3.HeaderText = "Vehicle#";
            gridViewTextBoxColumn3.Name = "Vehicle#";
            gridViewTextBoxColumn3.Width = 62;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "SBRNum";
            gridViewTextBoxColumn4.HeaderText = "SBRNum";
            gridViewTextBoxColumn4.Name = "SBRNum";
            gridViewTextBoxColumn4.Width = 45;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "ItemName";
            gridViewTextBoxColumn5.HeaderText = "ItemName";
            gridViewTextBoxColumn5.Name = "ItemName";
            gridViewTextBoxColumn5.Width = 80;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.FieldName = "First Weight";
            gridViewTextBoxColumn6.HeaderText = "First Weight";
            gridViewTextBoxColumn6.Name = "First Weight";
            gridViewTextBoxColumn6.Width = 52;
            gridViewDateTimeColumn1.EnableExpressionEditor = false;
            gridViewDateTimeColumn1.FieldName = "DocDate";
            gridViewDateTimeColumn1.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            gridViewDateTimeColumn1.HeaderText = "DocDate";
            gridViewDateTimeColumn1.MinWidth = 0;
            gridViewDateTimeColumn1.Name = "DocDate";
            gridViewDateTimeColumn1.Width = 49;
            this.grdDetails.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewDateTimeColumn1});
            this.grdDetails.MasterTemplate.EnableGrouping = false;
            sortDescriptor1.PropertyName = "Weighment#";
            this.grdDetails.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.grdDetails.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.ReadOnly = true;
            this.grdDetails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grdDetails.Size = new System.Drawing.Size(345, 396);
            this.grdDetails.TabIndex = 72;
            this.grdDetails.Text = "radGridView1";
            this.grdDetails.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grdDetails_CellDoubleClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(132, 488);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(16, 488);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(110, 24);
            this.btnSubmit.TabIndex = 70;
            this.btnSubmit.Text = "&Add";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // radLabel9
            // 
            this.radLabel9.Location = new System.Drawing.Point(299, 155);
            this.radLabel9.Name = "radLabel9";
            this.radLabel9.Size = new System.Drawing.Size(47, 18);
            this.radLabel9.TabIndex = 65;
            this.radLabel9.Text = "SO Date";
            // 
            // radLabel10
            // 
            this.radLabel10.Location = new System.Drawing.Point(299, 178);
            this.radLabel10.Name = "radLabel10";
            this.radLabel10.Size = new System.Drawing.Size(91, 18);
            this.radLabel10.TabIndex = 67;
            this.radLabel10.Text = "Balance Quantity";
            // 
            // radLabel11
            // 
            this.radLabel11.Location = new System.Drawing.Point(299, 110);
            this.radLabel11.Name = "radLabel11";
            this.radLabel11.Size = new System.Drawing.Size(88, 18);
            this.radLabel11.TabIndex = 64;
            this.radLabel11.Text = "Customer Name";
            // 
            // txtCurrTime
            // 
            this.txtCurrTime.Location = new System.Drawing.Point(462, 88);
            this.txtCurrTime.Name = "txtCurrTime";
            this.txtCurrTime.Size = new System.Drawing.Size(159, 20);
            this.txtCurrTime.TabIndex = 60;
            // 
            // radLabel12
            // 
            this.radLabel12.Location = new System.Drawing.Point(299, 89);
            this.radLabel12.Name = "radLabel12";
            this.radLabel12.Size = new System.Drawing.Size(31, 18);
            this.radLabel12.TabIndex = 61;
            this.radLabel12.Text = "Time";
            // 
            // txtItemNam
            // 
            this.txtItemNam.Location = new System.Drawing.Point(462, 132);
            this.txtItemNam.Name = "txtItemNam";
            this.txtItemNam.Size = new System.Drawing.Size(159, 20);
            this.txtItemNam.TabIndex = 55;
            // 
            // txtShift
            // 
            this.txtShift.Location = new System.Drawing.Point(462, 66);
            this.txtShift.Name = "txtShift";
            this.txtShift.Size = new System.Drawing.Size(159, 20);
            this.txtShift.TabIndex = 58;
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(299, 133);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(62, 18);
            this.radLabel5.TabIndex = 57;
            this.radLabel5.Text = "Item Name";
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(299, 67);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(29, 18);
            this.radLabel6.TabIndex = 59;
            this.radLabel6.Text = "Shift";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(11, 155);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(27, 18);
            this.radLabel3.TabIndex = 51;
            this.radLabel3.Text = "SO#";
            // 
            // txtItemCod
            // 
            this.txtItemCod.Location = new System.Drawing.Point(119, 132);
            this.txtItemCod.Name = "txtItemCod";
            this.txtItemCod.Size = new System.Drawing.Size(153, 20);
            this.txtItemCod.TabIndex = 54;
            // 
            // txtCurrDate
            // 
            this.txtCurrDate.Location = new System.Drawing.Point(119, 88);
            this.txtCurrDate.Name = "txtCurrDate";
            this.txtCurrDate.Size = new System.Drawing.Size(153, 20);
            this.txtCurrDate.TabIndex = 48;
            // 
            // radLabel7
            // 
            this.radLabel7.Location = new System.Drawing.Point(11, 133);
            this.radLabel7.Name = "radLabel7";
            this.radLabel7.Size = new System.Drawing.Size(58, 18);
            this.radLabel7.TabIndex = 56;
            this.radLabel7.Text = "Item Code";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(11, 89);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(30, 18);
            this.radLabel2.TabIndex = 49;
            this.radLabel2.Text = "Date";
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Location = new System.Drawing.Point(119, 109);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Size = new System.Drawing.Size(153, 20);
            this.txtCustomerCode.TabIndex = 52;
            // 
            // txtDocNo
            // 
            this.txtDocNo.Location = new System.Drawing.Point(119, 66);
            this.txtDocNo.Name = "txtDocNo";
            this.txtDocNo.Size = new System.Drawing.Size(153, 20);
            this.txtDocNo.TabIndex = 46;
            this.txtDocNo.TextChanged += new System.EventHandler(this.txtDocNo_TextChanged);
            // 
            // radLabel8
            // 
            this.radLabel8.Location = new System.Drawing.Point(11, 110);
            this.radLabel8.Name = "radLabel8";
            this.radLabel8.Size = new System.Drawing.Size(84, 18);
            this.radLabel8.TabIndex = 53;
            this.radLabel8.Text = "Customer Code";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(11, 67);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(71, 18);
            this.radLabel1.TabIndex = 47;
            this.radLabel1.Text = "Weighment#";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(994, 4);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(26, 13);
            this.lbl2.TabIndex = 134;
            this.lbl2.Text = "lbl2";
            this.lbl2.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(672, 46);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 16);
            this.btnRefresh.TabIndex = 137;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCust
            // 
            this.btnCust.Image = global::mfmWeighment.Properties.Resources.Folder16;
            this.btnCust.Location = new System.Drawing.Point(274, 109);
            this.btnCust.Name = "btnCust";
            this.btnCust.Size = new System.Drawing.Size(20, 20);
            this.btnCust.TabIndex = 85;
            this.btnCust.Click += new System.EventHandler(this.btnCust_Click);
            // 
            // radLabel31
            // 
            this.radLabel31.Location = new System.Drawing.Point(11, 441);
            this.radLabel31.Name = "radLabel31";
            this.radLabel31.Size = new System.Drawing.Size(85, 18);
            this.radLabel31.TabIndex = 146;
            this.radLabel31.Text = "Allow Tolerance";
            // 
            // chkAllowTolerance
            // 
            this.chkAllowTolerance.Location = new System.Drawing.Point(119, 442);
            this.chkAllowTolerance.Name = "chkAllowTolerance";
            this.chkAllowTolerance.Size = new System.Drawing.Size(15, 15);
            this.chkAllowTolerance.TabIndex = 147;
            this.chkAllowTolerance.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.chkAllowTolerance_ToggleStateChanged);
            // 
            // lblToleranceLimit
            // 
            this.lblToleranceLimit.Location = new System.Drawing.Point(299, 464);
            this.lblToleranceLimit.Name = "lblToleranceLimit";
            this.lblToleranceLimit.Size = new System.Drawing.Size(82, 18);
            this.lblToleranceLimit.TabIndex = 148;
            this.lblToleranceLimit.Text = "Tolerance Limit";
            this.lblToleranceLimit.Visible = false;
            // 
            // txtToleranceLimit
            // 
            this.txtToleranceLimit.Location = new System.Drawing.Point(462, 464);
            this.txtToleranceLimit.Name = "txtToleranceLimit";
            this.txtToleranceLimit.Size = new System.Drawing.Size(159, 20);
            this.txtToleranceLimit.TabIndex = 149;
            this.txtToleranceLimit.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbBridge02);
            this.panel1.Controls.Add(this.rbBridge01);
            this.panel1.Location = new System.Drawing.Point(846, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 26);
            this.panel1.TabIndex = 150;
            // 
            // rbBridge02
            // 
            this.rbBridge02.Location = new System.Drawing.Point(80, 4);
            this.rbBridge02.Name = "rbBridge02";
            this.rbBridge02.Size = new System.Drawing.Size(68, 18);
            this.rbBridge02.TabIndex = 1;
            this.rbBridge02.TabStop = false;
            this.rbBridge02.Text = "Bridge 02";
            // 
            // rbBridge01
            // 
            this.rbBridge01.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbBridge01.Location = new System.Drawing.Point(4, 4);
            this.rbBridge01.Name = "rbBridge01";
            this.rbBridge01.Size = new System.Drawing.Size(68, 18);
            this.rbBridge01.TabIndex = 0;
            this.rbBridge01.Text = "Bridge 01";
            this.rbBridge01.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // frmDispatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 519);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtToleranceLimit);
            this.Controls.Add(this.lblToleranceLimit);
            this.Controls.Add(this.chkAllowTolerance);
            this.Controls.Add(this.radLabel31);
            this.Controls.Add(this.btnCust);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnGetWeight);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.txtFullText);
            this.Controls.Add(this.txtCWeight);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.BtnItemSelect);
            this.Controls.Add(this.txtDifferenceWeight);
            this.Controls.Add(this.radLabel36);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.btnBulkTable);
            this.Controls.Add(this.txtBrandPath);
            this.Controls.Add(this.txtDaySeries);
            this.Controls.Add(this.radLabel34);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radLabel35);
            this.Controls.Add(this.radLabel32);
            this.Controls.Add(this.txtCNICPath);
            this.Controls.Add(this.txtItemGroupName);
            this.Controls.Add(this.radLabel33);
            this.Controls.Add(this.radLabel30);
            this.Controls.Add(this.txtNetWeightTon);
            this.Controls.Add(this.txt2WeightTon);
            this.Controls.Add(this.radLabel28);
            this.Controls.Add(this.radLabel13);
            this.Controls.Add(this.txtNetWeightKG);
            this.Controls.Add(this.radLabel29);
            this.Controls.Add(this.txt2WeightTime);
            this.Controls.Add(this.radLabel25);
            this.Controls.Add(this.txt2WeightKG);
            this.Controls.Add(this.radLabel26);
            this.Controls.Add(this.txt2WeightDate);
            this.Controls.Add(this.radLabel27);
            this.Controls.Add(this.cmbTransportCode);
            this.Controls.Add(this.cmbTransportType);
            this.Controls.Add(this.cmbPacker);
            this.Controls.Add(this.TxtTransportName);
            this.Controls.Add(this.radLabel23);
            this.Controls.Add(this.txt1WeightTon);
            this.Controls.Add(this.radLabel24);
            this.Controls.Add(this.txt1WeightTime);
            this.Controls.Add(this.radLabel21);
            this.Controls.Add(this.txtDriverName);
            this.Controls.Add(this.radLabel22);
            this.Controls.Add(this.txt1WeightKG);
            this.Controls.Add(this.radLabel19);
            this.Controls.Add(this.radLabel17);
            this.Controls.Add(this.txtSBRNum);
            this.Controls.Add(this.radLabel20);
            this.Controls.Add(this.txt1WeightDate);
            this.Controls.Add(this.radLabel18);
            this.Controls.Add(this.txtDriverCNIC);
            this.Controls.Add(this.radLabel14);
            this.Controls.Add(this.txtVehicleNum);
            this.Controls.Add(this.radLabel15);
            this.Controls.Add(this.txtBalanceQuantity);
            this.Controls.Add(this.txtSBRDate);
            this.Controls.Add(this.radLabel16);
            this.Controls.Add(this.BtnDocSelect);
            this.Controls.Add(this.txtOrderQuantity);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.grdDetails);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.radLabel9);
            this.Controls.Add(this.radLabel10);
            this.Controls.Add(this.radLabel11);
            this.Controls.Add(this.txtCurrTime);
            this.Controls.Add(this.radLabel12);
            this.Controls.Add(this.txtItemNam);
            this.Controls.Add(this.txtShift);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.txtItemCod);
            this.Controls.Add(this.txtCurrDate);
            this.Controls.Add(this.radLabel7);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.txtCustomerCode);
            this.Controls.Add(this.txtDocNo);
            this.Controls.Add(this.radLabel8);
            this.Controls.Add(this.radLabel1);
            this.Name = "frmDispatch";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmDispatch";
            this.Load += new System.EventHandler(this.frmDispatch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtFullText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).EndInit();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLastRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNextRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPreviosRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFirstRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnItemSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDifferenceWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel36)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBulkTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBrandPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDaySeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel35)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCNICPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemGroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel30)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNetWeightTon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightTon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNetWeightKG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel29)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightKG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt2WeightDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTransportCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTransportType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPacker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTransportName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightTon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriverName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightKG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBRNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt1WeightDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriverCNIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVehicleNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalanceQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBRDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnDocSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrderQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetails.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSubmit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllowTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToleranceLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToleranceLimit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbBridge02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbBridge01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadTextBox txtDocNo;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtCurrDate;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox txtItemNam;
        private Telerik.WinControls.UI.RadTextBox txtShift;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadTextBox txtItemCod;
        private Telerik.WinControls.UI.RadLabel radLabel7;
        private Telerik.WinControls.UI.RadTextBox txtCustomerCode;
        private Telerik.WinControls.UI.RadLabel radLabel8;
        private Telerik.WinControls.UI.RadLabel radLabel9;
        private Telerik.WinControls.UI.RadLabel radLabel10;
        private Telerik.WinControls.UI.RadLabel radLabel11;
        private Telerik.WinControls.UI.RadTextBox txtCurrTime;
        private Telerik.WinControls.UI.RadLabel radLabel12;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnSubmit;
        private Telerik.WinControls.UI.RadGridView grdDetails;
        private Telerik.WinControls.UI.RadTextBox txtOrderQuantity;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadButton BtnDocSelect;
        private Telerik.WinControls.UI.RadLabel radLabel16;
        private Telerik.WinControls.UI.RadTextBox txtSBRDate;
        private Telerik.WinControls.UI.RadTextBox txtBalanceQuantity;
        private System.Windows.Forms.Timer tmrCamFront;
        private System.Windows.Forms.Timer tmrAlreadyReading;
        private System.Windows.Forms.Timer tmrCamBack;
        private Telerik.WinControls.UI.RadTextBox txtDriverCNIC;
        private Telerik.WinControls.UI.RadLabel radLabel14;
        private Telerik.WinControls.UI.RadTextBox txtVehicleNum;
        private Telerik.WinControls.UI.RadLabel radLabel15;
        private Telerik.WinControls.UI.RadTextBox txt1WeightKG;
        private Telerik.WinControls.UI.RadLabel radLabel17;
        private Telerik.WinControls.UI.RadTextBox txt1WeightDate;
        private Telerik.WinControls.UI.RadLabel radLabel18;
        private Telerik.WinControls.UI.RadLabel radLabel19;
        private Telerik.WinControls.UI.RadTextBox txtSBRNum;
        private Telerik.WinControls.UI.RadLabel radLabel20;
        private Telerik.WinControls.UI.RadTextBox txt1WeightTime;
        private Telerik.WinControls.UI.RadLabel radLabel21;
        private Telerik.WinControls.UI.RadTextBox txtDriverName;
        private Telerik.WinControls.UI.RadLabel radLabel22;
        private Telerik.WinControls.UI.RadTextBox TxtTransportName;
        private Telerik.WinControls.UI.RadLabel radLabel23;
        private Telerik.WinControls.UI.RadTextBox txt1WeightTon;
        private Telerik.WinControls.UI.RadLabel radLabel24;
        private Telerik.WinControls.UI.RadDropDownList cmbPacker;
        private Telerik.WinControls.UI.RadDropDownList cmbTransportType;
        private Telerik.WinControls.UI.RadDropDownList cmbTransportCode;
        private Telerik.WinControls.UI.RadTextBox txt2WeightTon;
        private Telerik.WinControls.UI.RadLabel radLabel13;
        private Telerik.WinControls.UI.RadTextBox txt2WeightTime;
        private Telerik.WinControls.UI.RadLabel radLabel25;
        private Telerik.WinControls.UI.RadTextBox txt2WeightKG;
        private Telerik.WinControls.UI.RadLabel radLabel26;
        private Telerik.WinControls.UI.RadTextBox txt2WeightDate;
        private Telerik.WinControls.UI.RadLabel radLabel27;
        private Telerik.WinControls.UI.RadTextBox txtNetWeightTon;
        private Telerik.WinControls.UI.RadLabel radLabel28;
        private Telerik.WinControls.UI.RadTextBox txtNetWeightKG;
        private Telerik.WinControls.UI.RadLabel radLabel29;
        private Telerik.WinControls.UI.RadTextBox txtCNICPath;
        private Telerik.WinControls.UI.RadLabel radLabel30;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadTextBox txtDaySeries;
        private Telerik.WinControls.UI.RadLabel radLabel32;
        private Telerik.WinControls.UI.RadTextBox txtItemGroupName;
        private Telerik.WinControls.UI.RadLabel radLabel33;
        private Telerik.WinControls.UI.RadTextBox txtBrandPath;
        private Telerik.WinControls.UI.RadLabel radLabel34;
        private Telerik.WinControls.UI.RadLabel radLabel35;
        private Telerik.WinControls.UI.RadButton btnBulkTable;
        private Telerik.WinControls.UI.RadTextBox txtCustomerName;
        private Telerik.WinControls.UI.RadTextBox txtDifferenceWeight;
        private Telerik.WinControls.UI.RadLabel radLabel36;
        private Telerik.WinControls.UI.RadButton BtnItemSelect;
        private Telerik.WinControls.UI.RadPanel pnlControls;
        private Telerik.WinControls.UI.RadButton btnPrint;
        private Telerik.WinControls.UI.RadButton btnGetWeight;
        private Telerik.WinControls.UI.RadButton btnLastRecord;
        private Telerik.WinControls.UI.RadButton btnNextRecord;
        private Telerik.WinControls.UI.RadButton btnPreviosRecord;
        private Telerik.WinControls.UI.RadButton btnFirstRecord;
        private Telerik.WinControls.UI.RadButton btnAddNew;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadTextBox txtFullText;
        private Telerik.WinControls.UI.RadTextBox txtCWeight;
        private System.Windows.Forms.Label lbl2;
        private Telerik.WinControls.UI.RadButton btnRefresh;
        private Telerik.WinControls.UI.RadButton btnCust;
        private Telerik.WinControls.UI.RadLabel radLabel31;
        private Telerik.WinControls.UI.RadCheckBox chkAllowTolerance;
        private Telerik.WinControls.UI.RadLabel lblToleranceLimit;
        private Telerik.WinControls.UI.RadTextBox txtToleranceLimit;
        private Telerik.WinControls.UI.RadLabel lblWeight;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadRadioButton rbBridge02;
        private Telerik.WinControls.UI.RadRadioButton rbBridge01;
    }
}