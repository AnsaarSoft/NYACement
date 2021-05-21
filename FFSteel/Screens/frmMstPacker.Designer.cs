namespace mfmFFS.Screens
{
    partial class frmMstPacker
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
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
            this.radButton2.TabIndex = 50;
            this.radButton2.Text = "Close";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(27, 313);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(134, 24);
            this.radButton1.TabIndex = 49;
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
            this.pnlControls.TabIndex = 48;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(233, 4);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(156, 33);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Packer Master";
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
            gridViewTextBoxColumn1.FieldName = "Code";
            gridViewTextBoxColumn1.HeaderText = "Code";
            gridViewTextBoxColumn1.MaxWidth = 150;
            gridViewTextBoxColumn1.MinWidth = 150;
            gridViewTextBoxColumn1.Name = "Code";
            gridViewTextBoxColumn1.Width = 150;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "Name";
            gridViewTextBoxColumn2.HeaderText = "Name";
            gridViewTextBoxColumn2.MaxWidth = 200;
            gridViewTextBoxColumn2.MinWidth = 200;
            gridViewTextBoxColumn2.Name = "Name";
            gridViewTextBoxColumn2.Width = 200;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Remarks";
            gridViewTextBoxColumn3.HeaderText = "Remarks";
            gridViewTextBoxColumn3.MaxWidth = 240;
            gridViewTextBoxColumn3.MinWidth = 240;
            gridViewTextBoxColumn3.Name = "Remarks";
            gridViewTextBoxColumn3.Width = 240;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "ID";
            gridViewTextBoxColumn4.HeaderText = "ID";
            gridViewTextBoxColumn4.IsVisible = false;
            gridViewTextBoxColumn4.Name = "ID";
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "Isnew";
            gridViewTextBoxColumn5.HeaderText = "Isnew";
            gridViewTextBoxColumn5.IsVisible = false;
            gridViewTextBoxColumn5.Name = "Isnew";
            this.grdYardMaster.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.grdYardMaster.MasterTemplate.EnableGrouping = false;
            this.grdYardMaster.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.grdYardMaster.Name = "grdYardMaster";
            this.grdYardMaster.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grdYardMaster.Size = new System.Drawing.Size(609, 238);
            this.grdYardMaster.TabIndex = 47;
            this.grdYardMaster.ThemeName = "ControlDefault";
            this.grdYardMaster.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.grdYardMaster_UserAddedRow);
            this.grdYardMaster.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grdYardMaster_CellClick);
            this.grdYardMaster.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grdYardMaster_CellValueChanged);
            // 
            // frmMstPacker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 360);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.grdYardMaster);
            this.Name = "frmMstPacker";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmMstPacker";
            this.Load += new System.EventHandler(this.frmMstPacker_Load);
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
        private Telerik.WinControls.UI.RadPanel pnlControls;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadGridView grdYardMaster;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
    }
}