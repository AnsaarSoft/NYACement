namespace mfmFFS.Screens
{
    partial class frmAuthentication
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn1 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.GroupDescriptor groupDescriptor1 = new Telerik.WinControls.Data.GroupDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAuthentication));
            this.txtRoleName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnSubmit = new Telerik.WinControls.UI.RadButton();
            this.grdRoleDetail = new Telerik.WinControls.UI.RadGridView();
            this.pnlControls = new Telerik.WinControls.UI.RadPanel();
            this.btnLastRecord = new Telerik.WinControls.UI.RadButton();
            this.btnNextRecord = new Telerik.WinControls.UI.RadButton();
            this.btnPreviosRecord = new Telerik.WinControls.UI.RadButton();
            this.btnFirstRecord = new Telerik.WinControls.UI.RadButton();
            this.btnAddNew = new Telerik.WinControls.UI.RadButton();
            this.erp = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtRoleID = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSubmit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRoleDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRoleDetail.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).BeginInit();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLastRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNextRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPreviosRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFirstRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(120, 69);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(166, 20);
            this.txtRoleName.TabIndex = 36;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 71);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(61, 18);
            this.radLabel2.TabIndex = 35;
            this.radLabel2.Text = "Role Name";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(166, 493);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(37, 493);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(110, 24);
            this.btnSubmit.TabIndex = 39;
            this.btnSubmit.Text = "&Add";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // grdRoleDetail
            // 
            this.grdRoleDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRoleDetail.GroupExpandAnimationType = Telerik.WinControls.UI.GridExpandAnimationType.Slide;
            this.grdRoleDetail.Location = new System.Drawing.Point(42, 96);
            // 
            // 
            // 
            this.grdRoleDetail.MasterTemplate.AddNewRowPosition = Telerik.WinControls.UI.SystemRowPosition.Bottom;
            this.grdRoleDetail.MasterTemplate.AllowAddNewRow = false;
            this.grdRoleDetail.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "MenuName";
            gridViewTextBoxColumn1.HeaderText = "MenuName";
            gridViewTextBoxColumn1.Name = "MenuName";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 458;
            gridViewComboBoxColumn1.FieldName = "Authorization";
            gridViewComboBoxColumn1.HeaderText = "Authorization";
            gridViewComboBoxColumn1.Name = "Authorization";
            gridViewComboBoxColumn1.Width = 639;
            gridViewTextBoxColumn2.FieldName = "ID";
            gridViewTextBoxColumn2.HeaderText = "ID";
            gridViewTextBoxColumn2.IsVisible = false;
            gridViewTextBoxColumn2.Name = "ID";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 10;
            gridViewTextBoxColumn3.FieldName = "MenuParent";
            gridViewTextBoxColumn3.HeaderText = "MenuParent";
            gridViewTextBoxColumn3.Name = "MenuParent";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.Width = 300;
            this.grdRoleDetail.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewComboBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            sortDescriptor1.PropertyName = "MenuParent";
            groupDescriptor1.GroupNames.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.grdRoleDetail.MasterTemplate.GroupDescriptors.AddRange(new Telerik.WinControls.Data.GroupDescriptor[] {
            groupDescriptor1});
            this.grdRoleDetail.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.grdRoleDetail.Name = "grdRoleDetail";
            this.grdRoleDetail.Size = new System.Drawing.Size(1138, 358);
            this.grdRoleDetail.TabIndex = 41;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pnlControls.Controls.Add(this.btnLastRecord);
            this.pnlControls.Controls.Add(this.btnNextRecord);
            this.pnlControls.Controls.Add(this.btnPreviosRecord);
            this.pnlControls.Controls.Add(this.btnFirstRecord);
            this.pnlControls.Controls.Add(this.btnAddNew);
            this.pnlControls.Location = new System.Drawing.Point(42, 5);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(812, 40);
            this.pnlControls.TabIndex = 43;
            // 
            // btnLastRecord
            // 
            this.btnLastRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnLastRecord.Image")));
            this.btnLastRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLastRecord.Location = new System.Drawing.Point(278, 7);
            this.btnLastRecord.Name = "btnLastRecord";
            this.btnLastRecord.Size = new System.Drawing.Size(50, 26);
            this.btnLastRecord.TabIndex = 12;
            this.btnLastRecord.Click += new System.EventHandler(this.btnLastRecord_Click);
            // 
            // btnNextRecord
            // 
            this.btnNextRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnNextRecord.Image")));
            this.btnNextRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNextRecord.Location = new System.Drawing.Point(212, 7);
            this.btnNextRecord.Name = "btnNextRecord";
            this.btnNextRecord.Size = new System.Drawing.Size(50, 26);
            this.btnNextRecord.TabIndex = 11;
            this.btnNextRecord.Click += new System.EventHandler(this.btnNextRecord_Click);
            // 
            // btnPreviosRecord
            // 
            this.btnPreviosRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnPreviosRecord.Image")));
            this.btnPreviosRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPreviosRecord.Location = new System.Drawing.Point(146, 7);
            this.btnPreviosRecord.Name = "btnPreviosRecord";
            this.btnPreviosRecord.Size = new System.Drawing.Size(50, 26);
            this.btnPreviosRecord.TabIndex = 11;
            this.btnPreviosRecord.Click += new System.EventHandler(this.btnPreviosRecord_Click);
            // 
            // btnFirstRecord
            // 
            this.btnFirstRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnFirstRecord.Image")));
            this.btnFirstRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFirstRecord.Location = new System.Drawing.Point(80, 7);
            this.btnFirstRecord.Name = "btnFirstRecord";
            this.btnFirstRecord.Size = new System.Drawing.Size(50, 26);
            this.btnFirstRecord.TabIndex = 10;
            this.btnFirstRecord.Click += new System.EventHandler(this.btnFirstRecord_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddNew.Location = new System.Drawing.Point(14, 7);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(50, 26);
            this.btnAddNew.TabIndex = 9;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // erp
            // 
            this.erp.ContainerControl = this;
            // 
            // txtRoleID
            // 
            this.txtRoleID.Location = new System.Drawing.Point(462, 71);
            this.txtRoleID.Name = "txtRoleID";
            this.txtRoleID.Size = new System.Drawing.Size(63, 20);
            this.txtRoleID.TabIndex = 37;
            this.txtRoleID.Text = "0";
            this.txtRoleID.Visible = false;
            // 
            // frmAuthentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 546);
            this.Controls.Add(this.txtRoleID);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.grdRoleDetail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtRoleName);
            this.Controls.Add(this.radLabel2);
            this.Name = "frmAuthentication";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmAuthentication";
            this.Load += new System.EventHandler(this.frmAuthentication_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSubmit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRoleDetail.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRoleDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).EndInit();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnLastRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNextRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPreviosRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFirstRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox txtRoleName;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnSubmit;
        private Telerik.WinControls.UI.RadGridView grdRoleDetail;
        private Telerik.WinControls.UI.RadPanel pnlControls;
        private Telerik.WinControls.UI.RadButton btnLastRecord;
        private Telerik.WinControls.UI.RadButton btnNextRecord;
        private Telerik.WinControls.UI.RadButton btnPreviosRecord;
        private Telerik.WinControls.UI.RadButton btnFirstRecord;
        private Telerik.WinControls.UI.RadButton btnAddNew;
        private System.Windows.Forms.ErrorProvider erp;
        private Telerik.WinControls.UI.RadTextBox txtRoleID;
    }
}