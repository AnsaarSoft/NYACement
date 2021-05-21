namespace mfmFFS.Screens
{
    partial class frmUser
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUser));
            this.txtID = new Telerik.WinControls.UI.RadTextBox();
            this.txtEmail = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel7 = new Telerik.WinControls.UI.RadLabel();
            this.cmbBridgeName = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.cmbWBSetting = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.pnlControls = new Telerik.WinControls.UI.RadPanel();
            this.btnLastRecord = new Telerik.WinControls.UI.RadButton();
            this.btnNextRecord = new Telerik.WinControls.UI.RadButton();
            this.btnPreviosRecord = new Telerik.WinControls.UI.RadButton();
            this.btnFirstRecord = new Telerik.WinControls.UI.RadButton();
            this.btnAddNew = new Telerik.WinControls.UI.RadButton();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.cmbRole = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.chkActiveUser = new Telerik.WinControls.UI.RadCheckBox();
            this.chkSuperUser = new Telerik.WinControls.UI.RadCheckBox();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnSubmit = new Telerik.WinControls.UI.RadButton();
            this.txtUserCode = new Telerik.WinControls.UI.RadTextBox();
            this.txtPassword = new Telerik.WinControls.UI.RadTextBox();
            this.txtUserName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.erp = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkSpecial = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBridgeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWBSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).BeginInit();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLastRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNextRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPreviosRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFirstRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkActiveUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSuperUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSubmit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSpecial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(575, 212);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(166, 20);
            this.txtID.TabIndex = 1;
            this.txtID.Visible = false;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(180, 147);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(166, 20);
            this.txtEmail.TabIndex = 3;
            this.txtEmail.Click += new System.EventHandler(this.txtEmail_Click);
            // 
            // radLabel7
            // 
            this.radLabel7.Location = new System.Drawing.Point(73, 148);
            this.radLabel7.Name = "radLabel7";
            this.radLabel7.Size = new System.Drawing.Size(33, 18);
            this.radLabel7.TabIndex = 56;
            this.radLabel7.Text = "Email";
            // 
            // cmbBridgeName
            // 
            this.cmbBridgeName.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbBridgeName.Items.Add(radListDataItem1);
            this.cmbBridgeName.Items.Add(radListDataItem2);
            this.cmbBridgeName.Location = new System.Drawing.Point(560, 83);
            this.cmbBridgeName.Name = "cmbBridgeName";
            this.cmbBridgeName.Size = new System.Drawing.Size(166, 20);
            this.cmbBridgeName.TabIndex = 5;
            this.cmbBridgeName.Visible = false;
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(453, 83);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(72, 18);
            this.radLabel6.TabIndex = 54;
            this.radLabel6.Text = "Bridge Name";
            this.radLabel6.Visible = false;
            // 
            // cmbWBSetting
            // 
            this.cmbWBSetting.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbWBSetting.Location = new System.Drawing.Point(560, 105);
            this.cmbWBSetting.Name = "cmbWBSetting";
            this.cmbWBSetting.Size = new System.Drawing.Size(166, 20);
            this.cmbWBSetting.TabIndex = 6;
            this.cmbWBSetting.Visible = false;
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(453, 105);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(62, 18);
            this.radLabel5.TabIndex = 52;
            this.radLabel5.Text = "WB Setting";
            this.radLabel5.Visible = false;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pnlControls.Controls.Add(this.btnLastRecord);
            this.pnlControls.Controls.Add(this.btnNextRecord);
            this.pnlControls.Controls.Add(this.btnPreviosRecord);
            this.pnlControls.Controls.Add(this.btnFirstRecord);
            this.pnlControls.Controls.Add(this.btnAddNew);
            this.pnlControls.Controls.Add(this.btnSearch);
            this.pnlControls.Location = new System.Drawing.Point(42, 5);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(812, 40);
            this.pnlControls.TabIndex = 50;
            // 
            // btnLastRecord
            // 
            this.btnLastRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnLastRecord.Image")));
            this.btnLastRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLastRecord.Location = new System.Drawing.Point(284, 7);
            this.btnLastRecord.Name = "btnLastRecord";
            this.btnLastRecord.Size = new System.Drawing.Size(50, 26);
            this.btnLastRecord.TabIndex = 14;
            this.btnLastRecord.Click += new System.EventHandler(this.btnLastRecord_Click);
            // 
            // btnNextRecord
            // 
            this.btnNextRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnNextRecord.Image")));
            this.btnNextRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNextRecord.Location = new System.Drawing.Point(218, 7);
            this.btnNextRecord.Name = "btnNextRecord";
            this.btnNextRecord.Size = new System.Drawing.Size(50, 26);
            this.btnNextRecord.TabIndex = 13;
            this.btnNextRecord.Click += new System.EventHandler(this.btnNextRecord_Click);
            // 
            // btnPreviosRecord
            // 
            this.btnPreviosRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnPreviosRecord.Image")));
            this.btnPreviosRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPreviosRecord.Location = new System.Drawing.Point(152, 7);
            this.btnPreviosRecord.Name = "btnPreviosRecord";
            this.btnPreviosRecord.Size = new System.Drawing.Size(50, 26);
            this.btnPreviosRecord.TabIndex = 12;
            this.btnPreviosRecord.Click += new System.EventHandler(this.btnPreviosRecord_Click);
            // 
            // btnFirstRecord
            // 
            this.btnFirstRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnFirstRecord.Image")));
            this.btnFirstRecord.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFirstRecord.Location = new System.Drawing.Point(86, 7);
            this.btnFirstRecord.Name = "btnFirstRecord";
            this.btnFirstRecord.Size = new System.Drawing.Size(50, 26);
            this.btnFirstRecord.TabIndex = 11;
            this.btnFirstRecord.Click += new System.EventHandler(this.btnFirstRecord_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddNew.Location = new System.Drawing.Point(20, 7);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(50, 26);
            this.btnAddNew.TabIndex = 10;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(446, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 26);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbRole.Location = new System.Drawing.Point(180, 170);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(166, 20);
            this.cmbRole.TabIndex = 4;
            this.cmbRole.SelectedValueChanged += new System.EventHandler(this.cmbRole_SelectedValueChanged);
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(73, 170);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(99, 18);
            this.radLabel4.TabIndex = 35;
            this.radLabel4.Text = "Authorization Role";
            // 
            // chkActiveUser
            // 
            this.chkActiveUser.Location = new System.Drawing.Point(180, 238);
            this.chkActiveUser.Name = "chkActiveUser";
            this.chkActiveUser.Size = new System.Drawing.Size(76, 18);
            this.chkActiveUser.TabIndex = 7;
            this.chkActiveUser.Text = "Active User";
            this.chkActiveUser.Click += new System.EventHandler(this.chkActiveUser_Click);
            // 
            // chkSuperUser
            // 
            this.chkSuperUser.Location = new System.Drawing.Point(180, 212);
            this.chkSuperUser.Name = "chkSuperUser";
            this.chkSuperUser.Size = new System.Drawing.Size(75, 18);
            this.chkSuperUser.TabIndex = 6;
            this.chkSuperUser.Text = "Super User";
            this.chkSuperUser.Click += new System.EventHandler(this.chkSuperUser_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 481);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(51, 481);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(110, 24);
            this.btnSubmit.TabIndex = 8;
            this.btnSubmit.Text = "&Add";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtUserCode
            // 
            this.txtUserCode.Location = new System.Drawing.Point(180, 81);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.Size = new System.Drawing.Size(166, 20);
            this.txtUserCode.TabIndex = 0;
            this.txtUserCode.Click += new System.EventHandler(this.txtUserCode_Click);
            this.txtUserCode.Leave += new System.EventHandler(this.txtUserCode_Leave);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(180, 125);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(166, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(180, 103);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(166, 20);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Click += new System.EventHandler(this.txtUserName_Click);
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(73, 126);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(53, 18);
            this.radLabel3.TabIndex = 34;
            this.radLabel3.Text = "Password";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(73, 105);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(62, 18);
            this.radLabel2.TabIndex = 35;
            this.radLabel2.Text = "User Name";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(73, 83);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(58, 18);
            this.radLabel1.TabIndex = 33;
            this.radLabel1.Text = "User Code";
            // 
            // erp
            // 
            this.erp.ContainerControl = this;
            // 
            // chkSpecial
            // 
            this.chkSpecial.Location = new System.Drawing.Point(180, 262);
            this.chkSpecial.Name = "chkSpecial";
            this.chkSpecial.Size = new System.Drawing.Size(121, 18);
            this.chkSpecial.TabIndex = 57;
            this.chkSpecial.Text = "Special Weight Alter";
            this.chkSpecial.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.chkSpecial_ToggleStateChanged);
            // 
            // frmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 546);
            this.Controls.Add(this.chkSpecial);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.radLabel7);
            this.Controls.Add(this.cmbBridgeName);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.cmbWBSetting);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.chkActiveUser);
            this.Controls.Add(this.chkSuperUser);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Name = "frmUser";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmUser";
            this.Load += new System.EventHandler(this.frmUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBridgeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWBSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).EndInit();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnLastRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNextRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPreviosRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFirstRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkActiveUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSuperUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSubmit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSpecial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox txtUserCode;
        private Telerik.WinControls.UI.RadTextBox txtPassword;
        private Telerik.WinControls.UI.RadTextBox txtUserName;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnSubmit;
        private Telerik.WinControls.UI.RadCheckBox chkSuperUser;
        private Telerik.WinControls.UI.RadCheckBox chkActiveUser;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadDropDownList cmbRole;
        private Telerik.WinControls.UI.RadPanel pnlControls;
        private Telerik.WinControls.UI.RadButton btnLastRecord;
        private Telerik.WinControls.UI.RadButton btnNextRecord;
        private Telerik.WinControls.UI.RadButton btnPreviosRecord;
        private Telerik.WinControls.UI.RadButton btnFirstRecord;
        private Telerik.WinControls.UI.RadButton btnAddNew;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private System.Windows.Forms.ErrorProvider erp;
        private Telerik.WinControls.UI.RadDropDownList cmbWBSetting;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadDropDownList cmbBridgeName;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadTextBox txtEmail;
        private Telerik.WinControls.UI.RadLabel radLabel7;
        private Telerik.WinControls.UI.RadTextBox txtID;
        private Telerik.WinControls.UI.RadCheckBox chkSpecial;
    }
}