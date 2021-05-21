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
    public partial class frmMstTolerance : frmBaseForm
    {
        #region Variables
        dbFFS oDB = new dbFFS(Program.ConStrApp);
        DataTable dt = new DataTable();
        string ProductName = "";
        string GridName = "";
        string header = "";

        #endregion

        #region Events
        public frmMstTolerance()
        {
            InitializeComponent();
        }
        private void frmMstTolerance_Load(object sender, EventArgs e)
        {
            LoadCurrentMenuSystem();
            Program.NoMsg("");
        }
        private void grdYardMaster_CellClick_1(object sender, GridViewCellEventArgs e)
        {
            string Name = Convert.ToString(e.Row.Cells["ProductName"].Value);
            if (!string.IsNullOrEmpty(Name))
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
            GridName = "";
            GridName = Convert.ToString(e.Row.Cells["ProductName"].Value);
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
                    Program.ExceptionMsg("Duplication with Product Name - " + ProductName);
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
                MstTolerance NewMT = new MstTolerance();
                NewMT.CreatedBy = Program.oCurrentUser.UserCode;
                NewMT.UpdatedBy = Program.oCurrentUser.UserCode;
                NewMT.CreateDT = DateTime.Now;
                NewMT.UpdateDt = DateTime.Now;

                for (int i = 0; i < grdYardMaster.RowCount; i++)
                {
                    GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
                    GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                    GridViewCellInfo cellName = CurrentRow.Cells["ProductName"];
                    if (string.IsNullOrEmpty(cellName.Value.ToString()))
                    {
                        Program.ExceptionMsg("Name Can't be Empty.");
                        //flgadd = false;
                        break;
                    }

                    GridViewCellInfo cellToleranceRate = CurrentRow.Cells["ToleranceRate"];
                    if (string.IsNullOrEmpty(Convert.ToString(cellToleranceRate.Value)))
                    {
                        cellToleranceRate.Value = 0;
                    }
                    GridViewCellInfo Isnew = CurrentRow.Cells["Isnew"];

                    if (Isnew.Value.ToString() == "0")
                    {
                        MstTolerance oldMT = oDB.MstTolerance.Where(x => x.ID == Convert.ToInt32(cellID.Value)).FirstOrDefault();

                        if (oldMT.TName != Convert.ToString(cellName.Value) || oldMT.TRate != Convert.ToDecimal(cellToleranceRate.Value))
                        {
                            oldMT.TName = Convert.ToString(cellName.Value);
                            oldMT.TRate = Convert.ToDecimal(cellToleranceRate.Value);
                            oldMT.UpdatedBy = Program.oCurrentUser.UserCode;
                            oldMT.UpdateDt = DateTime.Now;
                            Program.SuccesesMsg("Record Successfully Updated");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        NewMT.TName = Convert.ToString(cellName.Value);
                        try
                        {
                            NewMT.TRate = Convert.ToDecimal(cellToleranceRate.Value);
                        }
                        catch (Exception ex)
                        {
                            Program.ExceptionMsg("DurationFrom Value is not a number.");
                            continue;
                        }


                        oDB.MstTolerance.InsertOnSubmit(NewMT);
                        Program.SuccesesMsg("Record Successfully Added");
                    }

                }
                oDB.SubmitChanges();
                LoadCurrentMenuSystem();
                return true;
            }

            catch (Exception Ex)
            {
                if (Ex.Message == "Input string was not in a correct format.")
                {
                    Program.ExceptionMsg("DurationFrom/DurationTo Value is not a valid number.");
                }
                else
                {
                    Program.ExceptionMsg("AddRecord Function Exception Error : Contact AbacusConsultings.");
                    Program.oErrMgn.LogException(Program.ANV, Ex);
                }
                LoadCurrentMenuSystem();
                return false;
            }

        }

        //private Boolean ValidateRecord()
        //{
        //    Boolean retValue = true;
        //    try
        //    {
        //        var GetShiftData = from a in oDB.MstTolerance select a;

        //        foreach (MstTolerance data in GetShiftData)
        //        {
        //            if (header == "ProductName")
        //            {
        //                if (GridName != "")
        //                {
        //                    if (data.TName == GridName)
        //                    {
        //                        retValue = false;
        //                        LoadCurrentMenuSystem();
        //                        continue;
        //                    }
        //                }
        //            }
        //            if (radButton1.Text== "Update")
        //            {
        //                if (header != "ProductName")
        //                {
        //                    for (int i = 0; i < grdYardMaster.RowCount; i++)
        //                    {
        //                        GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
        //                        GridViewCellInfo cellProductName = CurrentRow.Cells["ProductName"];
        //                        if (GridName == cellProductName.Value.ToString())
        //                        {
        //                            if (i > 1)
        //                            {
        //                                retValue = false;
        //                                LoadCurrentMenuSystem();
        //                                continue;
        //                            }
        //                        }
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
                                GridViewCellInfo cellProductName = CurrentRow.Cells["ProductName"];
                                ProductName = cellProductName.Value.ToString();
                                for (int h = 0; h < grdYardMaster.RowCount; h++)// Iterating 1 Grid value to All grid Values.
                                {
                                    GridViewRowInfo Row = grdYardMaster.Rows[h];
                                    GridViewCellInfo PName = Row.Cells["ProductName"];

                                    if (PName.Value.ToString() == cellProductName.Value.ToString()) // Checking duplicate grid values
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
                                if (cellProductName.Value.ToString() == data.TName) // if record exist in DB more than one
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
                    dt.Columns.Add("ProductName");
                    dt.Columns.Add("ToleranceRate");
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
                IEnumerable<MstTolerance> oCollection = from a in oDB.MstTolerance
                                                        orderby a.TName
                                                        select a;
                foreach (MstTolerance One in oCollection)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["ID"] = One.ID;
                    dtRow["ProductName"] = One.TName;
                    dtRow["ToleranceRate"] = One.TRate;
                    dtRow["Isnew"] = "0";
                    dt.Rows.Add(dtRow);
                }
                grdYardMaster.DataSource = dt;
                byte Auth = Convert.ToByte(oDB.CnfRolesDetail.Where(x => x.RoleID == Program.oCurrentUser.RoleID && x.MenuName == "Tolerance Master").FirstOrDefault().GivenRight);
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
        private void ValidateSavenUpdate()
        {
            // Boolean retValue = true;
            try
            {
                var GetShiftData = from a in oDB.MstTolerance select a;
                switch (radButton1.Text)
                {
                    case "Add":

                        foreach (MstTolerance data in GetShiftData)
                        {
                            #region Validate

                            if (GridName != "")
                            {
                                if (data.TName == GridName)
                                {
                                    // retValue = false;
                                    Program.ExceptionMsg("Record Already Exist");
                                    // LoadCurrentMenuSystem();
                                    break;
                                }

                            }
                            else
                            {
                                Program.ExceptionMsg("Name can not be empty");
                                break;
                            }

                            #endregion
                            #region Add
                            MstTolerance NewMT = new MstTolerance();
                            NewMT.CreatedBy = Program.oCurrentUser.UserCode;
                            NewMT.UpdatedBy = Program.oCurrentUser.UserCode;
                            NewMT.CreateDT = DateTime.Now;
                            NewMT.UpdateDt = DateTime.Now;

                            for (int i = 0; i < grdYardMaster.RowCount; i++)
                            {
                                GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
                                GridViewCellInfo cellID = CurrentRow.Cells["ID"];
                                GridViewCellInfo cellName = CurrentRow.Cells["ProductName"];
                                if (string.IsNullOrEmpty(cellName.Value.ToString()))
                                {
                                    Program.ExceptionMsg("Name Can't be Empty.");
                                    //flgadd = false;
                                    break;
                                }

                                GridViewCellInfo cellToleranceRate = CurrentRow.Cells["ToleranceRate"];
                                if (string.IsNullOrEmpty(Convert.ToString(cellToleranceRate.Value)))
                                {
                                    cellToleranceRate.Value = 0;
                                }
                                GridViewCellInfo Isnew = CurrentRow.Cells["Isnew"];


                                NewMT.TName = Convert.ToString(cellName.Value);
                                try
                                {
                                    NewMT.TRate = Convert.ToDecimal(cellToleranceRate.Value);
                                }
                                catch (Exception ex)
                                {
                                    Program.ExceptionMsg("DurationFrom Value is not a number.");
                                    continue;
                                }


                                oDB.MstTolerance.InsertOnSubmit(NewMT);
                                Program.SuccesesMsg("Record Successfully Added");

                                oDB.SubmitChanges();
                            }
                            #endregion
                        }

                        break;

                    case "Update":


                        foreach (MstTolerance data in GetShiftData)
                        {
                            #region Validate
                            if (header == "ProductName")
                            {
                                if (GridName != "")
                                {
                                    if (data.TName == GridName)
                                    {
                                        //retValue = false;
                                        //LoadCurrentMenuSystem();
                                        break;
                                    }
                                }
                            }

                            if (header != "ProductName")
                            {
                                for (int i = 0; i < grdYardMaster.RowCount; i++)
                                {
                                    GridViewRowInfo CurrentRow = grdYardMaster.Rows[i];
                                    GridViewCellInfo cellProductName = CurrentRow.Cells["ProductName"];
                                    if (GridName == cellProductName.Value.ToString())
                                    {
                                        if (i > 1)
                                        {
                                            //  retValue = false;
                                            // LoadCurrentMenuSystem();
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Update

                            #endregion
                        }

                        break;


                    default:
                        break;
                }
                if (radButton1.Text == "Add")
                {



                }


                LoadCurrentMenuSystem();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion
    }
}
