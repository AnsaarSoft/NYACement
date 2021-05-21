using System;
using System.Collections;
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
using mfmFFS.Dialog;
using mfmFFSDB;

namespace mfmFFS.Screens
{
    public partial class frmUser : frmBaseForm
    {

        #region Variable

        String OpenObjectID = "";
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        int GhostId = 1;
        Hashtable CodeIndex = null;
        Int32 CurrentRecord = -1, TotalRecords = 0;
        List<MstUsers> oCollection;

        #endregion

        #region Functions

        private void Initiallization()
        {
            try
            {
                btnSubmit.Text = "&Add";

                //GhostId = Convert.ToInt32(oDB.MstUsers.FirstOrDefault());
                txtID.Text = "0";//Convert.ToString(GhostId + 1);
                oDB = new dbFFS(Program.ConStrApp);
                FillRolesCombo();
                cmbRole.Text = "";
                //byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "Users").FirstOrDefault().GivenRight);
                //if (Auth == 4)
                //{
                //    btnSubmit.Enabled = false;
                //}
                //bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                //if (super == true)
                //{
                //    btnSubmit.Enabled = true;
                //}
                oCollection = (from a in oDB.MstUsers
                               select a).ToList();
                TotalRecords = oCollection.Count;
            }
            catch (Exception ex)
            {
                //program.exceptionmsg(ex.message);
                Program.oErrMgn.LogException(Program.ANV, ex);
            }
        }

        private Boolean AddUpdateRecord()
        {
            try
            {
                MstUsers oNew = new MstUsers();
                oNew.CreatedBy = Program.oCurrentUser.UserCode;
                oNew.CreateDt = DateTime.Now;

                int id = Convert.ToInt32(txtID.Text);
                var oldData = (oDB.MstUsers.Where(x => x.ID == id));

                if (oldData.Count() > 0)
                {
                    var oldData1 = oldData.FirstOrDefault();
                    //oNew.UserCode = txtUserCode.Text;
                    oldData1.UserName = txtUserName.Text;
                    oldData1.Password = txtPassword.Text;
                    oldData1.FlgActive = chkActiveUser.Checked;
                    oldData1.FlgSuper = chkSuperUser.Checked;
                    oldData1.FlgSpecial = chkSpecial.Checked;
                    oldData1.Email = txtEmail.Text;

                    Int64 RoleID = Convert.ToInt64(cmbRole.SelectedValue);
                    CnfRoles oRec = (from a in oDB.CnfRoles where a.ID == RoleID select a).FirstOrDefault();
                    if (oRec != null)
                    {
                        oldData1.RoleID = Convert.ToInt32(oRec.ID);
                    }

                    oldData1.UpdatedBy = Program.oCurrentUser.UserCode;
                    oldData1.UpdateDt = DateTime.Now;
                    oDB.SubmitChanges();
                    Program.SuccesesMsg("Record Successfully Updated");
                }
                else
                {
                    oNew.UserCode = txtUserCode.Text;
                    oNew.UserName = txtUserName.Text;
                    oNew.Password = txtPassword.Text;
                    oNew.FlgActive = chkActiveUser.Checked;
                    oNew.FlgSuper = chkSuperUser.Checked;
                    oNew.FlgSpecial = chkSpecial.Checked;
                    oNew.Email = txtEmail.Text;

                    Int64 RoleID = Convert.ToInt64(cmbRole.SelectedValue);
                    CnfRoles oRec = (from a in oDB.CnfRoles where a.ID == RoleID select a).FirstOrDefault();
                    if (oRec != null)
                    {
                        oNew.RoleID = Convert.ToInt32(oRec.ID);
                    }

                    oNew.CreatedBy = Program.oCurrentUser.UserCode;
                    oNew.CreateDt = DateTime.Now;
                    oNew.UpdatedBy = Program.oCurrentUser.UserCode;
                    oNew.UpdateDt = DateTime.Now;
                    oDB.MstUsers.InsertOnSubmit(oNew);
                    oDB.SubmitChanges();
                    Program.SuccesesMsg("Record Successfully Added");

                }
                return true;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("Add/UpdateRecord Function Exception Error : Contact AbacusConsultings.");
                Program.oErrMgn.LogException(Program.ANV, Ex);
                return false;
            }
        }

        private Boolean UpdateRecord()
        {
            try
            {
                string userCode = txtUserCode.Text;
                int ID = Convert.ToInt32(txtID.Text);
                if (userCode != null)
                {
                    MstUsers oNew = (from a in oDB.MstUsers where a.ID == ID select a).FirstOrDefault();
                    oNew.UserName = txtUserName.Text;
                    oNew.Password = txtPassword.Text;
                    oNew.Email = txtEmail.Text;
                    oNew.FlgActive = chkActiveUser.Checked;
                    oNew.FlgSuper = chkSuperUser.Checked;
                    Int32 RoleID = Convert.ToInt32(cmbRole.SelectedValue);
                    CnfRoles oRec = (from a in oDB.CnfRoles where a.ID == RoleID select a).FirstOrDefault();
                    if (oRec != null)
                    {
                        oNew.RoleID = oRec.ID;
                    }
                    oNew.UpdatedBy = Program.oCurrentUser.UserCode;
                    oNew.UpdateDt = DateTime.Now;
                    oDB.SubmitChanges();
                    Program.SuccesesMsg("Record Successfully Updated");
                }
                return true;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("Update Record Error : Contact AbacusConsultings.");
                Program.oErrMgn.LogException(Program.ANV, Ex);
                return false;
            }
        }

        private Boolean ValidateRecord()
        {
            try
            {
                int ID = Convert.ToInt32(txtID.Text);
                var GetData = (from a in oDB.MstUsers where a.ID == ID select a);
                if (GetData.Count()>0)
                {
                    Program.ExceptionMsg("UserCode Already Exists");
                    return false;
                }
                else
                {

                    if (string.IsNullOrEmpty(txtUserCode.Text))
                    {
                        erp.SetError(txtUserCode, "Field is Empty");
                        return false;
                    }
                    else
                    {
                        erp.Clear();
                    }
                    if (string.IsNullOrEmpty(txtUserName.Text))
                    {
                        erp.SetError(txtUserName, "Field is Empty");
                        return false;
                    }
                    else
                    {
                        erp.Clear();
                    }
                    if (string.IsNullOrEmpty(txtPassword.Text))
                    {
                        erp.SetError(txtPassword, "Field is Empty");
                        return false;
                    }
                    else
                    {
                        erp.Clear();
                    }
                    return true;

                }
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
                return false;

            }
        }

        private void ClearControl()
        {
            try
            {
                txtUserName.Text = "";
                txtID.Text = "0";
                txtUserCode.Text = "";
                txtPassword.Text = "";
                cmbRole.Text = "";
                chkActiveUser.Checked = false;
                chkSuperUser.Checked = false;
                chkSpecial.Checked = false;
                btnSubmit.Text = "&Add";
                txtEmail.Text = "";

            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
     
        private void FillRolesCombo()
        {
            try
            {
                IEnumerable<CnfRoles> oCollection = from a in oDB.CnfRoles select a;
                cmbRole.DataSource = oCollection;
                cmbRole.ValueMember = "ID";
                cmbRole.DisplayMember = "RoleName";
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);

            }
        }

        private void FillBridgeName()
        {

            try
            {
                IEnumerable<MstLov> oMstLove = from a in oDB.MstLov where a.Type == "WB" select a;
                cmbBridgeName.DataSource = oMstLove;
                cmbBridgeName.ValueMember = "ID";
                cmbBridgeName.DisplayMember = "Value";
            }
            catch (Exception ex)
            {

            }
        }

        private void FillWBSettingCombo()
        {
            try
            {

                IEnumerable<MstWeighBridge> oCollection = from a in oDB.MstWeighBridge select a;
                cmbWBSetting.DataSource = oCollection;
                cmbWBSetting.ValueMember = "ID";
                cmbWBSetting.DisplayMember = "WBCode";
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }

        private void GetAllObjects()
        {
            try
            {
                if (CodeIndex == null)
                {
                    CodeIndex = new Hashtable();
                }
                else
                {
                    CodeIndex.Clear();
                }
                IEnumerable<MstUsers> oCollection = from a in oDB.MstUsers select a;
                int i = 0;
                foreach (MstUsers One in oCollection)
                {
                    CodeIndex.Add(i, One.UserCode);
                    i++;
                }
                TotalRecords = i;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("GetAllObjects Exceptions : " + Ex.Message);
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }

        }

        #endregion

        #region Form Events

        public frmUser()
        {
            InitializeComponent();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            Initiallization();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (btnSubmit.Text == "&Add")
            {
                if (ValidateRecord())
                {
                    if (AddUpdateRecord())
                    {
                        ClearControl();
                    }
                }
            }
            else
            {
                if (AddUpdateRecord())
                {
                    ClearControl();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult oResult;
            if (btnSubmit.Text != "&Add")
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. You lose all entered information.", "Confirmation.", MessageBoxButtons.YesNo);
            }
            else
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
            }
            if (oResult == System.Windows.Forms.DialogResult.Yes)
            {
                this.Dispose();
                base.mytabpage.Dispose();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //   HandleDialogControl();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void btnFirstRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDB.MstUsers select a.ID).First();

                if (MaxRecord != 0)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);

                    Program.WarningMsg("Reached To First Record");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnPreviosRecord_Click(object sender, EventArgs e)
        {
            try
            {
                if (TotalRecords == 0)
                    return;
                CurrentRecord--;
                if (CurrentRecord < 0)
                {
                    CurrentRecord = 0;
                }
                if (CurrentRecord >= 0)
                {
                    setValues(oCollection[CurrentRecord].ID);
                }
                else
                {
                    Program.WarningMsg("Reached To First Record");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnNextRecord_Click(object sender, EventArgs e)
        {
            try
            {
                if (TotalRecords == 0)
                    return;
                CurrentRecord++;
                if (CurrentRecord < TotalRecords)
                {

                }
                else
                {
                    CurrentRecord = 0;
                }   
                if (CurrentRecord >= 0)
                {
                    setValues(oCollection[CurrentRecord].ID);
                }
                else
                {
                    Program.WarningMsg("Reached To Last Record");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnLastRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var MaxRecord = (from a in oDB.MstUsers select a.ID).Max();

                if (MaxRecord != 0)
                {
                    int DocNum = Convert.ToInt32(MaxRecord);
                    setValues(DocNum);
                    Program.WarningMsg("Reached To Last Record");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void txtUserCode_Click(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }

        }

        private bool checkAddUpdate()
        {
            string UCode = txtUserCode.Text;
            int aa = (from a in oDB.MstUsers where a.UserCode == UCode select a).Count();
            if (Convert.ToInt32(aa) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private void txtUserName_Click(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void txtEmail_Click(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void chkSuperUser_Click(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void chkActiveUser_Click(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void cmbRole_SelectedValueChanged(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void txtUserCode_Leave(object sender, EventArgs e)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        private void chkSpecial_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (checkAddUpdate())
            {
                btnSubmit.Text = "&Update";
            }
            else
            {
                btnSubmit.Text = "&Add";
            }
        }

        public void setValues(long doc)
        {

            try
            {
                Program.NoMsg("");
                btnSubmit.Text = "&Add";
                MstUsers trm = (from a in oDB.MstUsers where a.ID == doc select a).FirstOrDefault();

                //Program.FormDocNum = Convert.ToInt32(trm.DocNum);
                //txtDocNo.Text = Convert.ToString(Program.FormDocNum);
                txtUserCode.Text = Convert.ToString(trm.UserCode);

                txtUserName.Text = trm.UserName;
                txtEmail.Text = trm.Email;
                txtPassword.Text = trm.Password;
                cmbRole.Text = "";
                txtID.Text = Convert.ToString(trm.ID);
                int Role = Convert.ToInt32(trm.RoleID);
                string RoleName = (from a in oDB.CnfRoles where a.ID == Role select a.RoleName).FirstOrDefault();
                cmbRole.Text = RoleName;

                if (trm.FlgSuper == true)
                {
                    chkSuperUser.Checked = true;
                }
                else
                {
                    chkSuperUser.Checked = false;
                }
                if (trm.FlgActive == true)
                {
                    chkActiveUser.Checked = true;
                }
                else
                {
                    chkActiveUser.Checked = false;
                }
                chkSpecial.Checked = trm.FlgSpecial.GetValueOrDefault();

            }
            catch (Exception ex)
            {
                Program.ExceptionMsg(ex.ToString());
            }

        }

        #endregion

    }
}
