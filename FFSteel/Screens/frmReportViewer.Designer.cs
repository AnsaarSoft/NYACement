namespace mfmFFS.Screens
{
    partial class frmReportViewer
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
            this.crvMain = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // crvMain
            // 
            this.crvMain.ActiveViewIndex = -1;
            this.crvMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.crvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvMain.Location = new System.Drawing.Point(0, 0);
            this.crvMain.Name = "crvMain";
            this.crvMain.Size = new System.Drawing.Size(900, 546);
            this.crvMain.TabIndex = 0;
            // 
            // frmReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 546);
            this.Controls.Add(this.crvMain);
            this.Name = "frmReportViewer";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmReportViewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReportViewer_FormClosed);
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crvMain;
    }
}