namespace mfmFFS.Screens
{
    partial class frmMstTolerance
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.pnlControls = new Telerik.WinControls.UI.RadPanel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.grdYardMaster = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).BeginInit();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdYardMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdYardMaster.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(167, 313);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(134, 24);
            this.radButton2.TabIndex = 54;
            this.radButton2.Text = "Close";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(27, 313);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(134, 24);
            this.radButton1.TabIndex = 53;
            this.radButton1.Text = "Add";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pnlControls.Controls.Add(this.radLabel1);
            this.pnlControls.Location = new System.Drawing.Point(27, 12);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(609, 40);
            this.pnlControls.TabIndex = 52;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(202, 4);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(188, 33);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Tolerance Master";
            // 
            // grdYardMaster
            // 
            this.grdYardMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.grdYardMaster.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdYardMaster.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.grdYardMaster.ForeColor = System.Drawing.Color.Black;
            this.grdYardMaster.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.grdYardMaster.Location = new System.Drawing.Point(27, 69);
            // 
            // 
            // 
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "ID";
            gridViewTextBoxColumn1.HeaderText = "ID";
            gridViewTextBoxColumn1.IsPinned = true;
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "ID";
            gridViewTextBoxColumn1.PinPosition = Telerik.WinControls.UI.PinnedColumnPosition.Right;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "ProductName";
            gridViewTextBoxColumn2.HeaderText = "ProductName";
            gridViewTextBoxColumn2.IsPinned = true;
            gridViewTextBoxColumn2.MaxWidth = 400;
            gridViewTextBoxColumn2.MinWidth = 400;
            gridViewTextBoxColumn2.Name = "ProductName";
            gridViewTextBoxColumn2.PinPosition = Telerik.WinControls.UI.PinnedColumnPosition.Left;
            gridViewTextBoxColumn2.Width = 400;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "ToleranceRate";
            gridViewTextBoxColumn3.HeaderText = "ToleranceRate";
            gridViewTextBoxColumn3.IsPinned = true;
            gridViewTextBoxColumn3.MaxWidth = 190;
            gridViewTextBoxColumn3.MinWidth = 240;
            gridViewTextBoxColumn3.Name = "ToleranceRate";
            gridViewTextBoxColumn3.PinPosition = Telerik.WinControls.UI.PinnedColumnPosition.Left;
            gridViewTextBoxColumn3.Width = 190;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "Isnew";
            gridViewTextBoxColumn4.HeaderText = "Isnew";
            gridViewTextBoxColumn4.IsVisible = false;
            gridViewTextBoxColumn4.Name = "Isnew";
            this.grdYardMaster.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.grdYardMaster.MasterTemplate.EnableGrouping = false;
            this.grdYardMaster.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.grdYardMaster.Name = "grdYardMaster";
            this.grdYardMaster.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grdYardMaster.Size = new System.Drawing.Size(609, 238);
            this.grdYardMaster.TabIndex = 51;
            this.grdYardMaster.ThemeName = "ControlDefault";
            this.grdYardMaster.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.grdYardMaster_UserAddedRow);
            this.grdYardMaster.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grdYardMaster_CellClick_1);
            this.grdYardMaster.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grdYardMaster_CellValueChanged_1);
            // 
            // frmMstTolerance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 360);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.grdYardMaster);
            this.Name = "frmMstTolerance";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmMstTolerance";
            this.Load += new System.EventHandler(this.frmMstTolerance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).EndInit();
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdYardMaster.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdYardMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadPanel pnlControls;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadGridView grdYardMaster;
    }
}