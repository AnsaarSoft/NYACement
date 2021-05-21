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
using DevComponents.DotNetBar;

namespace mfmFFS.Screens
{
    public partial class frmMstPacker : frmBaseForm
    {
        #region Variables
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        DataTable dt = new DataTable();
        string GridCode = "";
        string GridName = "";
        string header = "";

        #endregion

        #region Events
        public frmMstPacker()
        {
            InitializeComponent();
        }

        private void frmMstPacker_Load(object sender, EventArgs e)
        {
            LoadCurrentMenuSystem();
            Program.NoMsg("");
        }
        private void grdYardMaster_CellClick(object sender, GridViewCellEventArgs e)
        {
            string code = Convert.ToString(e.Row.Cells["Code"].Value);
            if (!string.IsNullOrEmpty(code))
            {
                radButton1.Text = "Update";
            }
            else
            {
                radButton1.Text = "Add";
            }
        }
        private void grdYardMaster_CellValueChanged(object sender, GridViewCellEventArgs e)
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
                    Program.ExceptionMsg("Duplication with Code - " + GridCode);
                }
            }
            catch (Exception ex)
            {
                Program.ExceptionMsg(ex.ToString());
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


        #endregion

        #region Functions
        private Boolean AddRecord()
        {
            try
            {
                MstPacker NewMP = new MstPacker();
                NewMP.CreateBy = Program.oCurrentUser.UserCode;
                NewMP.UpdateBy = Program.oCurrentUser.UserCode;
                NewMP.CreateDt = DateTime.Now;
                NewMP.UpdateDt = DateTime.Now;

                for (int i = 0; i < grdYardMaster.RowCount; i++)
                {
                    GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
                    GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                    GridViewCellInfo cellCode = CurrentRow.Cells["Code"];
                    if (string.IsNullOrEmpty(cellCode.Value.ToString()))
                    {
                        Program.ExceptionMsg("Code Can't be Empty.");
                        break;
                    }
                    GridViewCellInfo cellName = CurrentRow.Cells["Name"];
                    if (string.IsNullOrEmpty(cellName.Value.ToString()))
                    {
                        Program.ExceptionMsg("Name Can't be Empty.");
                        break;
                    }

                    GridViewCellInfo cellDurationFrom = CurrentRow.Cells["DurationFrom"];
                    GridViewCellInfo cellDurationTo = CurrentRow.Cells["DurationTo"];
                    GridViewCellInfo cellRemarks = CurrentRow.Cells["Remarks"];
                    GridViewCellInfo Isnew = CurrentRow.Cells["Isnew"];
                    #region Update
                    if (Isnew.Value.ToString() == "0")
                    {
                        MstPacker oldMP = oDB.MstPacker.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();

                        if (oldMP.Code != Convert.ToString(cellCode.Value) || oldMP.Name != Convert.ToString(cellName.Value) || oldMP.Remarks != Convert.ToString(cellRemarks.Value))
                        {
                            oldMP.Code = Convert.ToString(cellCode.Value);
                            oldMP.Name = Convert.ToString(cellName.Value);
                            oldMP.Remarks = Convert.ToString(cellRemarks.Value);
                            oldMP.UpdateBy = Program.oCurrentUser.UserCode;
                            oldMP.UpdateDt = DateTime.Now;
                            Program.SuccesesMsg("Record Successfully Updated");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    #endregion
                    else
                    #region AddNew
                    {
                        NewMP.Code = Convert.ToString(cellCode.Value);
                        NewMP.Name = Convert.ToString(cellName.Value);
                        NewMP.Remarks = Convert.ToString(cellRemarks.Value);

                        oDB.MstPacker.InsertOnSubmit(NewMP);
                        Program.SuccesesMsg("Record Successfully Added");
                    }
                    #endregion
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

        //private Boolean ValidateRecord()
        //{
        //    Boolean retValue = true;
        //    try
        //    {
        //        var GetShiftData = from a in oDB.MstPacker select a;

        //        foreach (MstPacker data in GetShiftData)
        //        {
        //            int i = 0;
        //            if (header == "Code" || header == "Name" || header == "DurationFrom" || header == "DurationTo" || header == "Remarks")
        //            {
        //                if (GridCode != "")
        //                {
        //                    if (data.Code == GridCode)
        //                    {
        //                        retValue = false;
        //                        LoadCurrentMenuSystem();
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}
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
                IEnumerable<MstPacker> oCollection = from a in oDB.MstPacker
                                                     select a;
                foreach (MstPacker One in oCollection)
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
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "Packer Master").FirstOrDefault().GivenRight);
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
