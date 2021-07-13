namespace mfmFFS.Dialog
{
    partial class frmLoginDlg
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
            this.radLabel38 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel39 = new Telerik.WinControls.UI.RadLabel();
            this.btnCancel2 = new Telerik.WinControls.UI.RadButton();
            this.btnAuthenticate = new Telerik.WinControls.UI.RadButton();
            this.txtUserID = new Telerik.WinControls.UI.RadTextBox();
            this.txtPassword = new Telerik.WinControls.UI.RadTextBox();
            this.lblLoginStatus = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel39)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAuthenticate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel38
            // 
            this.radLabel38.Location = new System.Drawing.Point(6, 14);
            this.radLabel38.Name = "radLabel38";
            this.radLabel38.Size = new System.Drawing.Size(43, 18);
            this.radLabel38.TabIndex = 6;
            this.radLabel38.Text = "User ID";
            // 
            // radLabel39
            // 
            this.radLabel39.Location = new System.Drawing.Point(6, 38);
            this.radLabel39.Name = "radLabel39";
            this.radLabel39.Size = new System.Drawing.Size(53, 18);
            this.radLabel39.TabIndex = 8;
            this.radLabel39.Text = "Password";
            // 
            // btnCancel2
            // 
            this.btnCancel2.Location = new System.Drawing.Point(132, 64);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(110, 24);
            this.btnCancel2.TabIndex = 10;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(6, 64);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(110, 24);
            this.btnAuthenticate.TabIndex = 9;
            this.btnAuthenticate.Text = "Approve";
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(68, 12);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(175, 20);
            this.txtUserID.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(68, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(175, 20);
            this.txtPassword.TabIndex = 7;
            // 
            // lblLoginStatus
            // 
            this.lblLoginStatus.ForeColor = System.Drawing.Color.Red;
            this.lblLoginStatus.Location = new System.Drawing.Point(12, 102);
            this.lblLoginStatus.Name = "lblLoginStatus";
            this.lblLoginStatus.Size = new System.Drawing.Size(2, 2);
            this.lblLoginStatus.TabIndex = 20;
            // 
            // frmLoginDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 116);
            this.Controls.Add(this.lblLoginStatus);
            this.Controls.Add(this.radLabel38);
            this.Controls.Add(this.radLabel39);
            this.Controls.Add(this.btnCancel2);
            this.Controls.Add(this.btnAuthenticate);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.txtPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLoginDlg";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrator User Login";
            this.Load += new System.EventHandler(this.frmSimpleDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel39)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAuthenticate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel38;
        private Telerik.WinControls.UI.RadLabel radLabel39;
        private Telerik.WinControls.UI.RadButton btnCancel2;
        private Telerik.WinControls.UI.RadButton btnAuthenticate;
        private Telerik.WinControls.UI.RadTextBox txtUserID;
        private Telerik.WinControls.UI.RadTextBox txtPassword;
        private Telerik.WinControls.UI.RadLabel lblLoginStatus;
    }
}