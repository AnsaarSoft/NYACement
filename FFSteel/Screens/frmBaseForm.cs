using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mfmFFSDB;

namespace mfmFFS.Screens
{
    public partial class frmBaseForm : Telerik.WinControls.UI.RadForm
    {

        public DevComponents.DotNetBar.TabControlPanel mytabpage;
        public long DocEntry = 0;
        public frmMain frmParentRef;
        public long shiftidParent = 0;

        public frmBaseForm()
        {
            InitializeComponent();
        }

        public void frmBaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                mytabpage.Dispose();
            }
            catch { }
        }

        public string GetShift()
        {
            try
            {
                dbFFS oDB = new dbFFS(Program.ConStrApp);
                string time = DateTime.Now.ToString("HH:mm tt");

                int CurrentTimeinMins = HourstoMins(time); // time in mins ==> 24 hours

                var GetMstShifts = from a in oDB.MstShift select a;
                string ShiftName = "";
                foreach (var item in GetMstShifts)
                {
                    ShiftName = item.Name;
                    shiftidParent = item.ID;
                    string DurationFrom = item.DurationFrom;
                    int DurationFromInt = HourstoMins(DurationFrom);

                    string DurationTo = item.DurationTo;
                    int DurationToInt = HourstoMins(DurationTo);

                    if (DurationFromInt <= CurrentTimeinMins && DurationToInt >= CurrentTimeinMins)
                    {
                        break;
                    }
                }

                return Convert.ToString(ShiftName).ToString();
            }
            catch (Exception ex)
            {
                Program.oErrMgn.LogException(Program.ANV, ex);
                return "";
            }

        }

        public int HourstoMins(string time)
        {
            try
            {
                string[] DigitalTime = time.Split(' ');
                string[] Mins = DigitalTime[0].Split(':');
                int hoursTomins = Convert.ToInt32(Mins[0]) * 60;
                int totalMins = hoursTomins + Convert.ToInt32(Mins[1]);
                return totalMins;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        //public string Conversion24Hours(string time)
        //{
        //    try
        //    {
        //        string[] TimeCheck = time.Split(' ');
        //        string[] OnlyTime = TimeCheck[0].Split(':');
        //        string Hours = OnlyTime[0];

        //        string TimeChecktt = TimeCheck[1];
        //        if (TimeChecktt.ToLower() == "pm" && Convert.ToInt32(Hours) < 12)
        //        {
        //            int newHours = Convert.ToInt32(OnlyTime[0]) + 12;
        //            string ConvertedTime = newHours + ":" + OnlyTime[1];
        //            return ConvertedTime;
        //        }
        //        else
        //        {
        //            return time;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

    }
}
