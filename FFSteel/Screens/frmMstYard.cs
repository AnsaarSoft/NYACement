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
    public partial class frmMstYard : frmBaseForm
    {
        #region Variables
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        DataTable dt = new DataTable();
        string GridCode = "";
        string GridName = "";
        string header = "";

        #endregion

        #region Events
        public frmMstYard()
        {
            InitializeComponent();

        }
        private void frmMstYard_Load(object sender, EventArgs e)
        {
            LoadCurrentMenuSystem();
            Program.NoMsg("");
        }
        private void grdYardMaster_CellClick(object sender, GridViewCellEventArgs e)
        {
            string code = Convert.ToString(e.Row.Cells["Code"].Value);
            // dbID= Convert.ToInt32(e.Row.Cells["ID"].Value);
            if (!string.IsNullOrEmpty(code))
            {
                radButton1.Text = "Update";
            }
            else
            {
                radButton1.Text = "Add";
            }

        }
        private void grdYardMaster_CellValueChanged_1(object sender, GridViewCellEventArgs e)
        {
            GridCode = "";
            GridName = "";
            GridCode = Convert.ToString(e.Row.Cells["Code"].Value);

            GridName = Convert.ToString(e.Row.Cells["Name"].Value);
            header = e.Column.HeaderText;

        }
        private void grdYardMaster_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ValidateRecord())
                {
                    AddRecord();
                }
                else
                {
                    Program.ExceptionMsg("Record Already Exist");
                }
            }
            catch (Exception ex)
            {
                Program.ExceptionMsg(ex.ToString());
            }
        }
        private void radButton2_Click(object sender, EventArgs e)
        {
            DialogResult oResult = System.Windows.Forms.DialogResult.Cancel;
            if (radButton2.Text == "Close")
            {
                oResult = RadMessageBox.Show("Are you sure you to close this form. ", "Confirmation.", MessageBoxButtons.YesNo);
            }
            if (oResult == System.Windows.Forms.DialogResult.Yes)
            {
                this.Dispose();
                base.mytabpage.Dispose();
            }
        }
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRecord())
                {
                    AddRecord();

                }
                else
                {
                    Program.ExceptionMsg("Duplication with Code - " + GridCode);
                }
            }
            catch (Exception ex)
            {
                Program.ExceptionMsg(ex.ToString());
            }
        }

        #endregion

        #region Functions
        private Boolean AddRecord()
        {
            try
            {
                MstYardType MYT = new MstYardType();
                MYT.CreateBy = Program.oCurrentUser.UserCode;
                MYT.UpdateBy = Program.oCurrentUser.UserCode;
                MYT.CreateDt = DateTime.Now;
                MYT.UpdateDt = DateTime.Now;

                for (int i = 0; i < grdYardMaster.RowCount; i++)
                {
                    GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
                    GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                    GridViewCellInfo cellCode = CurrentRow.Cells["Code"];
                    if (string.IsNullOrEmpty(cellCode.Value.ToString()))
                    {
                        Program.ExceptionMsg("Code Can't be Empty.");
                        //flgadd = false;
                        break;
                    }
                    GridViewCellInfo cellName = CurrentRow.Cells["Name"];
                    if (string.IsNullOrEmpty(cellName.Value.ToString()))
                    {
                        Program.ExceptionMsg("Name Can't be Empty.");
                        //flgadd = false;
                        break;
                    }
                    GridViewCellInfo cellRemarks = CurrentRow.Cells["Remarks"];
                    GridViewCellInfo Isnew = CurrentRow.Cells["Isnew"];

                    if (Isnew.Value.ToString() == "0")
                    {
                        MstYardType MYT1 = oDB.MstYardType.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();

                        if (MYT1.Code != Convert.ToString(cellCode.Value) || MYT1.Name != Convert.ToString(cellName.Value) || MYT1.Remarks != Convert.ToString(cellRemarks.Value))
                        {
                            MYT1.Code = Convert.ToString(cellCode.Value);
                            MYT1.Name = Convert.ToString(cellName.Value);
                            MYT1.Remarks = Convert.ToString(cellRemarks.Value);
                            MYT1.UpdateBy = Program.oCurrentUser.UserCode;
                            MYT1.UpdateDt = DateTime.Now;
                            Program.SuccesesMsg("Record Successfully Updated");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        MYT.Code = Convert.ToString(cellCode.Value);
                        MYT.Name = Convert.ToString(cellName.Value);
                        MYT.Remarks = Convert.ToString(cellRemarks.Value);
                        oDB.MstYardType.InsertOnSubmit(MYT);
                        Program.SuccesesMsg("Record Successfully Added");
                    }
                    oDB.SubmitChanges();
                }
                LoadCurrentMenuSystem();
                return true;
            }

            catch (Exception Ex)
            {
                Program.ExceptionMsg("AddRecord Function Exception Error : Contact AbacusConsultings.");
                Program.oErrMgn.LogException(Program.ANV, Ex);
                return false;
            }

        }
        //private Boolean ValidateRecord()
        //{
        //    Boolean retValue = true;
        //    try
        //    {
        //        var GetYArdData = from a in oDB.MstYardType select a;

        //        foreach (MstYardType data in GetYArdData)
        //        {

        //            if (header == "Code"|| header == "Name" || header == "Remarks")
        //            {

        //                if (header == "Code")
        //                {
        //                    if (GridCode != "")
        //                    {
        //                        if (data.Code == GridCode)
        //                        {
        //                            retValue = false;
        //                            LoadCurrentMenuSystem();
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (header == "Name")
        //                {
        //                    if (GridName != "")
        //                    {
        //                        do {
        //                            if (data.Name == GridName)
        //                            {
        //                                retValue = false;
        //                                LoadCurrentMenuSystem();
        //                                break;
        //                            }
        //                            //for (int i = 0; i < GetYArdData.Count(); i++)
        //                            //{
        //                            //    if (data.Code == GridCode)
        //                            //    {
        //                            //        break;
        //                            //    }

        //                            //}
        //                        }

        //                        while (data.Code == GridCode);
        //                    }
        //                }


        //                //if (data.Code == GridCode)
        //                //{
        //                //    retValue = false;
        //                //    LoadCurrentMenuSystem();
        //                //    break;
        //                //}
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}
        private Boolean ValidateRecord()
        {
            Boolean retValue = true;
            try
            {
                var GetShiftData = from a in oDB.MstTolerance select a;

                foreach (MstTolerance data in GetShiftData) // iterating DB values.
                {
                    int j = 0;

                    if (retValue != false)
                    {
                        for (int i = 0; i < grdYardMaster.RowCount; i++) // Iterating 1 Db value to All grid Values.
                        {
                            if (retValue != false)
                            {
                                int k = 0;
                                GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
                                GridViewCellInfo cellCode = CurrentRow.Cells["Code"];
                                GridCode = cellCode.Value.ToString();
                                for (int h = 0; h < grdYardMaster.RowCount; h++)// Iterating 1 Grid value to All grid Values.
                                {
                                    GridViewRowInfo Row = grdYardMaster.Rows[h];
                                    GridViewCellInfo PCode = Row.Cells["Code"];

                                    if (PCode.Value.ToString() == GridCode) // Checking duplicate grid values
                                    {
                                        k++;
                                        if (k > 1) // if record exist in grid more than one
                                        {
                                            retValue = false;
                                            LoadCurrentMenuSystem();
                                            break;
                                        }

                                    }
                                }
                                if (GridCode == data.TName) // if record exist in DB more than one
                                {
                                    j++;
                                    if (j > 1)
                                    {
                                        retValue = false;
                                        LoadCurrentMenuSystem();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retValue = false;
            }
            return retValue;
        }
        private void CreateGrid()
        {
            try
            {
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("ID");
                    dt.Columns.Add("Code");
                    dt.Columns.Add("Name");
                    dt.Columns.Add("Remarks");
                    dt.Columns.Add("Isnew");

                }
            }
            catch (Exception Ex)
            {
            }
        }
        private void LoadCurrentMenuSystem()
        {
            try
            {
                dt.Clear();
                CreateGrid();
                dbFFS oDB = new dbFFS(Program.ConStrApp);
                IEnumerable<MstYardType> oCollection = from a in oDB.MstYardType
                                                       select a;
                foreach (MstYardType One in oCollection)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["ID"] = One.ID;
                    dtRow["Code"] = One.Code;
                    dtRow["Name"] = One.Name;
                    dtRow["Remarks"] = One.Remarks;
                    dtRow["Isnew"] = "0";
                    dt.Rows.Add(dtRow);
                }
                grdYardMaster.DataSource = dt;
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "Yard Master").FirstOrDefault().GivenRight);
                if (Auth == 4)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = false;

                    }
                }
                bool super = Convert.ToBoolean(oDB.MstUsers.Where(x => x.UserCode == Program.oCurrentUser.UserCode).FirstOrDefault().FlgSuper);

                if (super == true)
                {
                    foreach (Control o in this.Controls)
                    {
                        // special handling for the menu
                        o.Enabled = true;

                    }
                }
            }
            catch (Exception Ex)
            {
                Program.ExceptionMsg(Ex.Message);
            }
        }

        #endregion
        
    }
}