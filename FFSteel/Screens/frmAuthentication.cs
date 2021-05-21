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
using mfmFFSDB;
using mfmFFS.Dialog;


namespace mfmFFS.Screens
{
    public partial class frmAuthentication : frmBaseForm
    {

        #region Variable

        dbFFS oDIAPI = null;
        DataTable dtGrid;
        Int32 OpenObjectID = 0;
        // dbFFS oDB;
        Hashtable CodeIndex;
        Int32 CurrentRecord = 0, TotalRecords = 0;

        #endregion

        #region Function

        private void InitializeForm()
        {
            try
            {
                oDIAPI = new dbFFS(Program.ConStrApp);
                CreateGrid();
                SetComboBox();
                LoadCurrentMenuSystem();
                CodeIndex = new Hashtable();
                GetAllObjects();
                byte Auth = Convert.ToByte(oDIAPI.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "Authentication").FirstOrDefault().GivenRight);
                if (Auth == 4)
                {
                    btnSubmit.Enabled = false;
                }
                bool super = Convert.ToBoolean(oDIAPI.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                if (super == true)
                {
                    btnSubmit.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("InitializeForm Exception Error : " + Ex.Message);
            }
        }

        private void LoadCurrentMenuSystem()
        {
            try
            {
                IEnumerable<CnfMenues> oCollection = from a in oDIAPI.CnfMenues
                                                     select a;
                foreach (CnfMenues One in oCollection)
                {
                    CnfMenues pOne = (from a in oDIAPI.CnfMenues
                                      where a.ID == One.MenuParent
                                      select a).FirstOrDefault();
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["ID"] = One.ID;
                    dtRow["MenuName"] = One.MenuName;
                    if (pOne != null)
                        dtRow["MenuParent"] = pOne.MenuName;
                    else
                        dtRow["MenuParent"] = "Module Name";
                    dtRow["Authorization"] = "5";
                    if (pOne != null)
                        dtGrid.Rows.Add(dtRow);
                }
                grdRoleDetail.DataSource = dtGrid;
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void CreateGrid()
        {
            try
            {
                dtGrid = new DataTable();
                dtGrid.Columns.Add("ID");
                dtGrid.Columns.Add("MenuName");
                dtGrid.Columns.Add("MenuParent");
                dtGrid.Columns.Add("Authorization");
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void SetComboBox()
        {
            try
            {
                GridViewComboBoxColumn oAuthorization = (GridViewComboBoxColumn)grdRoleDetail.Columns["Authorization"];


                var oRoles = (from a in oDIAPI.MstLov where a.Type == "Roles" select a);

                oAuthorization.DataSource = oRoles;
                oAuthorization.ValueMember = "ID";
                oAuthorization.DisplayMember = "Value";

                oAuthorization.ReadOnly = false;
                grdRoleDetail.Columns.Add(oAuthorization);
            }
            catch (Exception Ex)
            {

            }
        }

        private void FillCombo(int id)
        {
            try
            {
                GridViewComboBoxColumn oAuthorization = (GridViewComboBoxColumn)grdRoleDetail.Columns["Authorization"];

                var oRoles = (from a in oDIAPI.MstLov where a.ID == id select a);

                oAuthorization.DataSource = oRoles;
                oAuthorization.ValueMember = "ID";
                oAuthorization.DisplayMember = "Value";

                oAuthorization.ReadOnly = false;
                grdRoleDetail.Columns.Add(oAuthorization);
            }
            catch (Exception Ex)
            {

            }
        }
        private void SubmitRecord(String pType)
        {
            try
            {
                CnfRoles oNew = null;
                if (pType == "Add")
                {
                    oNew = new CnfRoles();
                    oNew.RoleName = txtRoleName.Text;
                }
                else
                    oNew = (from a in Program.oDI.CnfRoles
                            where a.ID == OpenObjectID
                            select a).FirstOrDefault();
                oNew.CreatedBy = Program.oCurrentUser.UserCode;
                oNew.UpdatedBy = Program.oCurrentUser.UserCode;
                oNew.CreateDt = DateTime.Now;
                oNew.UpdateDt = DateTime.Now;
                for (Int32 i = 0; i < grdRoleDetail.RowCount; i++)
                {
                    GridViewRowInfo CurrentRow = grdRoleDetail.Rows[i];
                    GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                    GridViewCellInfo cellName = CurrentRow.Cells["MenuName"];
                    GridViewCellInfo cellAuthorization = CurrentRow.Cells["Authorization"];
                    GridViewCellInfo cellMenuParent = CurrentRow.Cells["MenuParent"];
                    Int32 MenuId = Convert.ToInt32(cellID.Value);

                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private Boolean AddRecord()
        {
            Boolean retValue = true;

            try
            {
                CnfRoles oNew = new CnfRoles();
                oNew.CreatedBy = Program.oCurrentUser.UserCode;
                oNew.CreateDt = DateTime.Now;
                oNew.RoleName = txtRoleName.Text;
                oNew.UpdatedBy = Program.oCurrentUser.UserCode;
                oNew.UpdateDt = DateTime.Now;


                string RoleName = txtRoleName.Text;
                var Count = (from a in oDIAPI.CnfRoles where a.RoleName == RoleName select a);

                if (Count.Count() > 0)
                {
                    for (Int32 i = 0; i < grdRoleDetail.RowCount; i++)
                    {
                        GridViewRowInfo CurrentRow = grdRoleDetail.Rows[i];
                        GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                        GridViewCellInfo cellName = CurrentRow.Cells["MenuName"];
                        GridViewCellInfo cellAuthorization = CurrentRow.Cells["Authorization"];
                        GridViewCellInfo cellMenuParent = CurrentRow.Cells["MenuParent"];
                        Int32 MenuId = Convert.ToInt32(cellID.Value);
                       // String MenuName = Convert.ToString(cellName.Value);
                        int Authorization = Convert.ToInt32(cellAuthorization.Value);
                        CnfMenues oMenu = (from a in oDIAPI.CnfMenues where a.MenuName == Convert.ToString(cellName.Value) select a).FirstOrDefault();
                        CnfRolesDetail oDetail = oDIAPI.CnfRolesDetail.Where(x=> x.RoleID == Count.FirstOrDefault().ID && x.MenuID == oMenu.ID).FirstOrDefault();
                      

                        //oDetail.CnfMenues = oMenu;
                        //oDetail.MenuName = MenuName;
                        oDetail.GivenRight = Convert.ToByte(Authorization);
                      //  oDetail.CreatedBy = Program.oCurrentUser.UserCode;
                      //  oDetail.UpdatedBy = Program.oCurrentUser.UserCode;
                        //oDetail.CreateDt = DateTime.Now;
                        //oDetail.UpdateDt = DateTime.Now;
                        oDetail.UpdatedBy = Program.oCurrentUser.UserCode;
                        oDetail.UpdateDt = DateTime.Now;
                        //oNew.CnfRolesDetail.Add(oDetail);
                        oDIAPI.SubmitChanges();
                        Program.SuccesesMsg("Record Successfully Updated.");
                        
                       
                    }

                }
                else
                {   
                    for (Int32 i = 0; i < grdRoleDetail.RowCount; i++)
                    {
                        GridViewRowInfo CurrentRow = grdRoleDetail.Rows[i];
                        GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                        GridViewCellInfo cellName = CurrentRow.Cells["MenuName"];
                        GridViewCellInfo cellAuthorization = CurrentRow.Cells["Authorization"];
                        GridViewCellInfo cellMenuParent = CurrentRow.Cells["MenuParent"];
                        Int32 MenuId = Convert.ToInt32(cellID.Value);
                        String MenuName = Convert.ToString(cellName.Value);
                        int Authorization = Convert.ToInt32(cellAuthorization.Value);

                        CnfRolesDetail oDetail = new CnfRolesDetail();
                        CnfMenues oMenu = (from a in oDIAPI.CnfMenues where a.ID == MenuId select a).FirstOrDefault();
                        oDetail.CnfMenues = oMenu;
                        oDetail.MenuName = MenuName;
                        oDetail.GivenRight = Convert.ToByte(Authorization);
                        oDetail.CreatedBy = Program.oCurrentUser.UserCode;
                        oDetail.UpdatedBy = Program.oCurrentUser.UserCode;
                        oDetail.CreateDt = DateTime.Now;
                        oDetail.UpdateDt = DateTime.Now;
                        oNew.CnfRolesDetail.Add(oDetail);
                    }
                    oDIAPI.CnfRoles.InsertOnSubmit(oNew);
                    oDIAPI.SubmitChanges();
                    Program.SuccesesMsg("Record Successfully Added.");
                }


            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg("AddRecord Exception Error : " + Ex.Message);
                retValue = false;
            }
            return retValue;
        }

        //private Boolean UpdateRecord()
        //{
        //    try
        //    {
        //        //CnfRoles oNew = (from a in oDIAPI.CnfRoles where a.ID == OpenObjectID select a).FirstOrDefault();
        //        //oNew.RoleName = txtRoleName.Text;
        //        //oNew.UpdatedBy = Program.oCurrentUser.UserCode;
        //        //oNew.UpdateDt = DateTime.Now;
        //        for (Int32 i = 0; i < grdRoleDetail.RowCount; i++)
        //        {
        //            GridViewRowInfo CurrentRow = grdRoleDetail.Rows[i];
        //            GridViewCellInfo cellID = CurrentRow.Cells["ID"];
        //            GridViewCellInfo cellName = CurrentRow.Cells["MenuName"];
        //            GridViewCellInfo cellAuthorization = CurrentRow.Cells["Authorization"];
        //            GridViewCellInfo cellMenuParent = CurrentRow.Cells["MenuParent"];
        //            Int32 MenuId = Convert.ToInt32(cellID.Value);
        //            String MenuName = Convert.ToString(cellName.Value);
        //            Byte Authorization = Convert.ToByte(cellAuthorization.Value);

        //            CnfRolesDetail oDetail = (from a in oDIAPI.CnfRolesDetail
        //                                      where a.CnfRoles.ID == OpenObjectID
        //                                      && a.CnfMenues.ID == MenuId
        //                                      select a).FirstOrDefault();
        //            CnfMenues oMenu = (from a in oDIAPI.CnfMenues where a.ID == MenuId select a).FirstOrDefault();
        //            if (oDetail != null)
        //            {
        //                oDetail.GivenRight = Authorization;
        //                oDetail.UpdatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.UpdateDt = DateTime.Now;
        //            }
        //            else
        //            {
        //                oDetail = new CnfRolesDetail();
        //                oDetail.CnfMenues = oMenu;
        //                oDetail.MenuName = MenuName;
        //                oDetail.GivenRight = Authorization;
        //                oDetail.CreatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.UpdatedBy = Program.oCurrentUser.UserCode;
        //                oDetail.CreateDt = DateTime.Now;
        //                oDetail.UpdateDt = DateTime.Now;
        //                oNew.CnfRolesDetail.Add(oDetail);
        //            }
        //        }
        //        oDIAPI.SubmitChanges();
        //        Program.SuccesesMsg("Record Successfully Updated.");
        //        return true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        Program.ExceptionMsg(Ex.Message);
        //        return false;
        //    }
        //}

        private void FillRecord()
        {
            try
            {
                CnfRoles oOpen = (from a in oDIAPI.CnfRoles where a.ID == OpenObjectID select a).FirstOrDefault();
                if (oOpen == null) return;
                txtRoleName.Text = oOpen.RoleName;

                for (Int32 i = 0; i < grdRoleDetail.RowCount; i++)
                {
                    GridViewRowInfo CurrentRow = grdRoleDetail.Rows[i];
                    GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                    GridViewCellInfo cellAuthorization = CurrentRow.Cells["Authorization"];
                    Int32 MenuId = Convert.ToInt32(cellID.Value);
                    Byte Authorization = Convert.ToByte(cellAuthorization.Value);

                    CnfRolesDetail oDetail = (from a in oDIAPI.CnfRolesDetail
                                              where a.CnfRoles.ID == oOpen.ID
                                              && a.CnfMenues.ID == MenuId
                                              select a).FirstOrDefault();
                    if (oDetail == null) continue;
                    cellAuthorization.Value = oDetail.GivenRight.ToString();
                }

                btnSubmit.Text = "Update";
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void ClearControls()
        {
            try
            {
                txtRoleName.Text = "";
                for (Int32 i = 0; i < grdRoleDetail.RowCount; i++)
                {
                    GridViewRowInfo CurrentRow = grdRoleDetail.Rows[i];
                    GridViewCellInfo cellAuthorization = CurrentRow.Cells["Authorization"];
                    cellAuthorization.Value = "1";
                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private void HandleDialogControl()
        {
            try
            {
                frmOpenDlg oDlg = new frmOpenDlg();
                oDlg.OpenFor = "Roles";
                oDlg.ShowDialog();
                if (oDlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    OpenObjectID = Convert.ToInt32(oDlg.SelectedObjectID);
                }
                if (OpenObjectID > 0)
                {
                    //FillRecord();
                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        private Boolean ValidateRecord()
        {
            Boolean retValue = true;
            try
            {
                if (string.IsNullOrEmpty(txtRoleName.Text))
                {
                    erp.SetError(txtRoleName, "Field is empty.");
                    return false;
                }
                else
                {
                    erp.Clear();
                }


            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
                retValue = false;
            }
            return retValue;
        }

        private void GetAllObjects()
        {
            try
            {
                IEnumerable<CnfRoles> oCollection = from a in oDIAPI.CnfRoles select a;
                int i = 0;
                foreach (CnfRoles One in oCollection)
                {
                    CodeIndex.Add(i, One.ID);
                    i++;
                }
                TotalRecords = i;
            }
            catch (Exception Ex)
            {
                Program.SuccesesMsg("GetAllObjects Exceptions : " + Ex.Message);
            }

        }

        #endregion

        #region FormEvents

        public frmAuthentication()
        {
            InitializeComponent();
        }

        private void frmAuthentication_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (btnSubmit.Text == "&Add")
            {
                if (ValidateRecord())
                {
                    if (AddRecord())
                    {
                        ClearControls();
                    }
                }
            }
            else
            {
                if (ValidateRecord())
                {
                    if (AddRecord())
                    {
                        btnSubmit.Text = "&Add";
                        ClearControls();
                    }
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult oResult;
            if (btnSubmit.Text != "Add")
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
            btnSubmit.Text = "&Add";
            txtRoleID.Text = "0";
            txtRoleName.Text = "";
            InitializeForm();
            //dtGrid.Clear();
            //ClearControls();
        }

        private void btnFirstRecord_Click(object sender, EventArgs e)
        {
            try
            {

                var GetRoles = (from a in oDIAPI.CnfRoles orderby a.ID ascending select a);
                int count = Convert.ToInt32(GetRoles.FirstOrDefault().ID);
                //var MaxRecord = (from a in oDIAPI.CnfRolesDetail where a.ID == GetRoles select a.ID).Max();

                if (count != 0)
                {
                    txtRoleID.Text = Convert.ToString(GetRoles.FirstOrDefault().ID);
                    txtRoleName.Text = GetRoles.FirstOrDefault().RoleName;
                    int DocNum = Convert.ToInt32(txtRoleID.Text);
                    GetRec(DocNum);
                }
                //GetnPopulateDisptachData(DocNum);
                Program.WarningMsg("Reached To First Record");

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
                int count = Convert.ToInt32(txtRoleID.Text);
                int NextRec = count - 1;
                var GetRoles = (from a in oDIAPI.CnfRoles where a.ID == NextRec select a);

                if (GetRoles.Count() > 0)
                {
                    txtRoleID.Text = Convert.ToString(GetRoles.FirstOrDefault().ID);
                    txtRoleName.Text = GetRoles.FirstOrDefault().RoleName;
                    //int DocNum = Convert.ToInt32(txtRoleID.Text);
                    GetRec(NextRec);
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
                int count = Convert.ToInt32(txtRoleID.Text);
                int NextRec = count + 1;
                var GetRoles = (from a in oDIAPI.CnfRoles where a.ID == NextRec select a);

                if (GetRoles.Count() > 0)
                {
                    txtRoleID.Text = Convert.ToString(GetRoles.FirstOrDefault().ID);
                    txtRoleName.Text = GetRoles.FirstOrDefault().RoleName;
                    //int DocNum = Convert.ToInt32(txtRoleID.Text);
                    GetRec(NextRec);
                }
                else
                {
                    Program.WarningMsg("Reached To Last Record");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void GetnPopulateDisptachData(int id)
        {
            try
            {
                IEnumerable<CnfRolesDetail> Getdata = (from a in oDIAPI.CnfRolesDetail
                                                       where a.RoleID == id
                                                       select a);
                Int32 Serial = 1;
                dtGrid.Clear();
                foreach (var item in Getdata)
                {
                    // SetComboBox();
                    DataRow dtRow = dtGrid.NewRow();
                    //GridViewComboBoxColumn cmb = (GridViewComboBoxColumn)grdRoleDetail.Columns["Authorization"];
                    // Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn1 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
                    //cmb = Convert.ToByte(item.GivenRight);

                    dtRow["MenuName"] = item.MenuName;

                    dtRow["Authorization"] = Convert.ToString(item.GivenRight);
                    dtRow["ID"] = item.ID;

                    //FillCombo(Convert.ToInt32(item.GivenRight));
                    dtGrid.Rows.Add(dtRow);
                }
                grdRoleDetail.DataSource = dtGrid;
                grdRoleDetail.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdRoleDetail.ShowGroupPanel = false;
                grdRoleDetail.ReadOnly = true;
                grdRoleDetail.EnableFiltering = true;
                grdRoleDetail.ShowFilteringRow = true;
            }
            catch (Exception Ex)
            {
                Program.oErrMgn.LogException(Program.ANV, Ex);
            }
        }
        private void GetRec(int DocNum)
        {
            try
            {
                Program.NoMsg("");
                btnSubmit.Text = "&Update";
                var RoleDetail = (from a in oDIAPI.CnfRolesDetail
                                  join b in oDIAPI.CnfMenues on a.MenuID equals b.ID
                                  where a.RoleID == DocNum
                                  select new { a, b });

                dtGrid.Clear();
                string menu = "";
                int menuID = 0;
                foreach (var item in RoleDetail)
                {
                    menu = Convert.ToString(item.b.MenuParent);
                    menuID = Convert.ToInt32(item.b.MenuParent);
                    var MenuName = (from b in oDIAPI.CnfMenues where b.ID == menuID select b.MenuName).FirstOrDefault();
                    DataRow dtRow = dtGrid.NewRow();
                    dtRow["ID"] = Convert.ToString(item.a.ID);
                    dtRow["MenuName"] = Convert.ToString(item.a.MenuName);
                    dtRow["Authorization"] = Convert.ToString(item.a.GivenRight);
                    dtRow["MenuParent"] = Convert.ToString(MenuName);
                    dtGrid.Rows.Add(dtRow);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void btnLastRecord_Click(object sender, EventArgs e)
        {
            try
            {

                var GetRoles = (from a in oDIAPI.CnfRoles orderby a.ID descending select a);
                int count = Convert.ToInt32(GetRoles.FirstOrDefault().ID);
                //var MaxRecord = (from a in oDIAPI.CnfRolesDetail where a.ID == GetRoles select a.ID).Max();

                if (count != 0)
                {
                    txtRoleID.Text = Convert.ToString(GetRoles.FirstOrDefault().ID);
                    txtRoleName.Text = GetRoles.FirstOrDefault().RoleName;
                    int DocNum = Convert.ToInt32(txtRoleID.Text);
                    GetRec(DocNum);
                }
                //GetnPopulateDisptachData(DocNum);
                Program.WarningMsg("Reached To Last Record");

            }
            catch (Exception ex)
            {

                throw;
            }
            //try
            //{
            //    CurrentRecord = TotalRecords - 1;
            //    OpenObjectID = Convert.ToInt32(CodeIndex[CurrentRecord].ToString());
            //   // FillRecord();
            //}
            //catch (Exception Ex)
            //{
            //    Program.ExceptionMsg("btnFirstRecord Exception Error : " + Ex.Message);
            //}
        }

        #endregion

    }
}
