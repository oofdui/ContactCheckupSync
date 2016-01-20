using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _ContactCheckupSync
{
    public partial class SyncToMobile : Form
    {
        public SyncToMobile()
        {
            InitializeComponent();
        }

        private void SyncToMobile_Load(object sender, EventArgs e)
        {
            var clsSQLMain = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
            var clsSQLMobile = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            dtDOEFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            dtDOETo.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            if (!clsSQLMain.IsConnected() && !clsSQLMobile.IsConnected())
            {
                btSearch.Enabled = false; //btSync.Enabled = false;
                dtDOEFrom.Enabled = false; dtDOETo.Enabled = false;
                ddlCompany.Enabled = false;
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูล (Main,Mobile) ได้", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!clsSQLMain.IsConnected())
            {
                btSearch.Enabled = false; //btSync.Enabled = false;
                dtDOEFrom.Enabled = false; dtDOETo.Enabled = false;
                ddlCompany.Enabled = false;
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูล (Main) ได้", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!clsSQLMobile.IsConnected())
            {
                btSearch.Enabled = false; //btSync.Enabled = false;
                dtDOEFrom.Enabled = false; dtDOETo.Enabled = false;
                ddlCompany.Enabled = false;
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูล (Mobile) ได้", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                btSearch.Enabled = true; //btSync.Enabled = true;
                dtDOEFrom.Enabled = true; dtDOETo.Enabled = true;
                ddlCompany.Enabled = true;
            }
        }
        private void setCompany()
        {
            #region Variable
            var dt = new DataTable();
            var clsTempData = new clsTempData();
            #endregion
            #region Procedure
            dt = clsTempData.getCompany(dtDOEFrom.Value, dtDOETo.Value);
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCompany.DataSource = dt;
                ddlCompany.DisplayMember = "Company";
                ddlCompany.ValueMember = "Company";
            }
            else
            {
                ddlCompany.DataSource = null;
            }
            #endregion
        }
        private void dtDOEFrom_ValueChanged(object sender, EventArgs e)
        {
            setCompany();
        }
        private void dtDOETo_ValueChanged(object sender, EventArgs e)
        {
            setCompany();
        }
        private void btSearch_Click(object sender, EventArgs e)
        {
            wbSearch.RunWorkerAsync();
        }
        private void btSync_Click(object sender, EventArgs e)
        {
            backgroundWorkerSyncToMobile.RunWorkerAsync();
        }
        private void Search()
        {
            #region Variable
            var dt = new DataTable();
            var clsTempData = new clsTempData();
            var countNoChecklist = 0;
            #endregion
            #region Procedure
            dt = clsTempData.getPatient(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"));
            if (dt != null && dt.Rows.Count > 0)
            {
                clsGlobal.dtPatient = dt.Copy();
                if (btSync.InvokeRequired)
                {
                    btSync.Invoke(new MethodInvoker(delegate
                    {
                        btSync.Enabled = true;
                    }));
                }
                //lblSyncToMobile.Text = "";
                #region RemoveColumn
                string[] columns = { "PatientGUID", "LabEpisode", "Address", "Tel", "Email", "Physician", "RegType", "Programid", "DIVI", "DEP", "SEC", "POS", "LAN", "NAT", "CNT_TRY", "LOC", "Payor", "Epi_Rowid", "ORD_STS", "STS", "DR_CDE", "NTE", "Job", "BusUnit", "BusDiv", "Line", "Shift", "Location", "GrpBook", "HISExist" };
                for (int i = 0; i < columns.Length; i++)
                {
                    dt.Columns.Remove(columns[i]);
                }
                dt.AcceptChanges();
                #endregion
                if (gvSyncToMobile.InvokeRequired)
                {
                    gvSyncToMobile.Invoke(new MethodInvoker(delegate
                    {
                        #region SortDatatable
                        if(dt!=null && dt.Rows.Count > 0)
                        {
                            for(int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (int.Parse(dt.Rows[i]["ChecklistCount"].ToString()) > 0)
                                {
                                    dt.Rows[i]["ChecklistCount"] = 1;
                                }
                                else
                                {
                                    dt.Rows[i]["ChecklistCount"] = 0;
                                }
                            }
                            dt.AcceptChanges();
                        }
                        var dv = dt.DefaultView;
                        dv.Sort = "ChecklistCount,No";
                        #endregion
                        gvSyncToMobile.DataSource = dv.ToTable();
                        for(int i = 0; i < gvSyncToMobile.Rows.Count; i++)
                        {
                            var checklistCount = (gvSyncToMobile.Rows[i].Cells["ChecklistCount"].Value!=null?gvSyncToMobile.Rows[i].Cells["ChecklistCount"].Value.ToString():"0");
                            if (checklistCount == "0")
                            {
                                gvSyncToMobile.Rows[i].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FF6262");
                                countNoChecklist += 1;
                            }
                        }
                        gvSyncToMobile.Columns["ChecklistCount"].Visible = false;
                    }));
                }
                if (lblSyncToMobile.InvokeRequired)
                {
                    lblSyncToMobile.Invoke(new MethodInvoker(delegate
                    {
                        lblSyncToMobile.Text = string.Format("พบข้อมูลทั้งหมด {0} รายการ | ไม่มี Checklist {1} รายการ", dt.Rows.Count.ToString(), countNoChecklist.ToString());
                    }));
                }
            }
            else
            {
                clsGlobal.dtPatient = null;
                if (btSync.InvokeRequired)
                {
                    btSync.Invoke(new MethodInvoker(delegate
                    {
                        btSync.Enabled = false;
                    }));
                }
                if (lblSyncToMobile.InvokeRequired)
                {
                    lblSyncToMobile.Invoke(new MethodInvoker(delegate
                    {
                        lblSyncToMobile.Text = "- ไม่พบข้อมูลที่ต้องการ -";
                    }));
                }
            }
            #endregion
        }
        public void setProgressBarSyncToMobile(int value, int maximumValue)
        {
            if (pbSyncToMobile.InvokeRequired)
            {
                pbSyncToMobile.Invoke(new MethodInvoker(delegate
                {
                    pbSyncToMobile.Visible = true;
                    pbSyncToMobile.Maximum = maximumValue;
                    pbSyncToMobile.Value = value;
                }));
            }
            else
            {
                pbSyncToMobile.Visible = true;
                pbSyncToMobile.Maximum = maximumValue;
                pbSyncToMobile.Value = value;
            }
        }
        private void setSyncToMobile()
        {
            if (clsGlobal.dtPatient != null && clsGlobal.dtPatient.Rows.Count > 0)
            {
                var countSuccess = 0; var countFail = 0; var countExist = 0;
                var countChecklistSuccess = 0; var countChecklistFail = 0; var countChecklistExist = 0;var countChecklistUpdate = 0;
                var outMessage = "";
                if (setPatientToMobile(clsGlobal.dtPatient, out countSuccess, out countFail, out countExist, out countChecklistSuccess, out countChecklistFail, out countChecklistExist,out countChecklistUpdate, out outMessage))
                {
                    if (lblSyncToMobile.InvokeRequired)
                    {
                        lblSyncToMobile.Invoke(new MethodInvoker(delegate
                        {
                            lblSyncToMobile.Text = string.Format(
                                "Sync ข้อมูลสู่ระบบ Mobile เสร็จสิ้น" + Environment.NewLine + Environment.NewLine +
                                "    @Patient Success : {0} , Exist : {1} , Fail : {2}" + Environment.NewLine +
                                "    @Checklist Success: {3} , Exist : {4} , Fail : {5} , Update : {6}",
                                countSuccess.ToString(),
                                countExist.ToString(),
                                countFail.ToString(),
                                countChecklistSuccess.ToString(),
                                countChecklistExist.ToString(),
                                countChecklistFail.ToString(),
                                countChecklistUpdate.ToString()
                                );
                        }));
                    }
                    //MessageBox.Show("Sync ข้อมูลสู่ระบบ Mobile เสร็จสิ้น" + Environment.NewLine +
                    //    "@Patient Success : " + countSuccess.ToString() +
                    //    " , Exist : " + countExist.ToString() +
                    //    " , Fail : " + countFail.ToString() + Environment.NewLine +
                    //    "@Checklist Success : " + countChecklistSuccess.ToString() +
                    //    " , Exist : " + countChecklistExist.ToString() +
                    //    " , Fail : " + countChecklistFail.ToString(),
                    //    "SyncComplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (lblSyncToMobile.InvokeRequired)
                    {
                        lblSyncToMobile.Invoke(new MethodInvoker(delegate
                        {
                            lblSyncToMobile.Text = string.Format(
                                "Sync ข้อมูลสู่ระบบ Mobile ผิดพลาด" + Environment.NewLine + Environment.NewLine +
                                "    @Patient Success : {0} , Exist : {1} , Fail : {2}" + Environment.NewLine +
                                "    @Checklist Success: {3} , Exist : {4} , Fail : {5}" + Environment.NewLine +
                                "Message Error : {6}",
                                countSuccess.ToString(),
                                countExist.ToString(),
                                countFail.ToString(),
                                countChecklistSuccess.ToString(),
                                countChecklistExist.ToString(),
                                countChecklistFail.ToString(),
                                outMessage
                                );
                        }));
                    }
                    //MessageBox.Show("Sync ข้อมูลสู่ระบบ Mobile ผิดพลาด" + Environment.NewLine +
                    //    "@Patient Success : " + countSuccess.ToString() +
                    //    " , Exist : " + countExist.ToString() +
                    //    " , Fail : " + countFail.ToString() + Environment.NewLine +
                    //    "@Checklist Success : " + countChecklistSuccess.ToString() +
                    //    " , Exist : " + countChecklistExist.ToString() +
                    //    " , Fail : " + countChecklistFail.ToString() + Environment.NewLine + Environment.NewLine +
                    //    outMessage,
                    //    "SyncComplete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (lblSyncToMobile.InvokeRequired)
                {
                    lblSyncToMobile.Invoke(new MethodInvoker(delegate
                    {
                        var clsTempData = new clsTempData();
                        lblSyncToMobile.Text += clsTempData.getPatientNotHadChecklist();
                    }));
                }
            }
            else
            {
                if (lblSyncToMobile.InvokeRequired)
                {
                    lblSyncToMobile.Invoke(new MethodInvoker(delegate
                    {
                        lblSyncToMobile.Text = "ไม่พบข้อมูลที่ต้องการ Sync";
                    }));
                }
                //MessageBox.Show("ไม่พบข้อมูลที่ต้องการ Sync", "SyncFail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void backgroundWorkerSyncToMobile_DoWork(object sender, DoWorkEventArgs e)
        {
            if (lblSyncToMobile.InvokeRequired)
            {
                lblSyncToMobile.Invoke(new MethodInvoker(delegate
                {
                    lblSyncToMobile.Text = "กำลังซิงค์ข้อมูล...";
                }));
            }
            if (btSync.InvokeRequired)
            {
                btSync.Invoke(new MethodInvoker(delegate
                {
                    btSync.Enabled = false;
                }));
            }
            setSyncToMobile();
            if (btSync.InvokeRequired)
            {
                btSync.Invoke(new MethodInvoker(delegate
                {
                    btSync.Enabled = true;
                }));
            }
            if (pbSyncToMobile.InvokeRequired)
            {
                pbSyncToMobile.Invoke(new MethodInvoker(delegate
                {
                    pbSyncToMobile.Visible = false;
                }));
            }
        }
        public bool setPatientToMobile(DataTable dt, out int countSuccess, out int countFail, out int countExist, out int countChecklistSuccess, out int countChecklistFail, out int countChecklistExist,out int countChecklistUpdate, out string outMessage)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            #region Variable
            var result = true;
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            var strSQL = "";
            countSuccess = 0; countFail = 0; countExist = 0;
            countChecklistSuccess = 0; countChecklistFail = 0; countChecklistExist = 0; countChecklistUpdate = 0;
             outMessage = "";
            #endregion
            #region Procedure
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        setProgressBarSyncToMobile(i + 1, dt.Rows.Count);
                        #region Patient
                        strSQL = "SELECT COUNT(PatientGUID) FROM patient WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "';";
                        var count = clsSQL.Return(strSQL);
                        if (count == "0")
                        {
                            if (!clsSQL.Insert(
                                "patient",
                                new string[,]
                                {
                            {"PatientGUID","'"+dt.Rows[i]["PatientGUID"].ToString()+"'" },
                            {"HN","'"+dt.Rows[i]["HN"].ToString()+"'" },
                            {"Episode","'"+dt.Rows[i]["Episode"].ToString()+"'" },
                            {"LabEpisode","'"+dt.Rows[i]["LabEpisode"].ToString()+"'" },
                            {"DOB","'"+DateTime.Parse(dt.Rows[i]["DOB"].ToString()).ToString("yyyy-MM-dd")+"'" },
                            {"No","'"+dt.Rows[i]["No"].ToString()+"'" },
                            {"EmployeeID","'"+dt.Rows[i]["EmployeeID"].ToString()+"'" },
                            {"DOE","'"+DateTime.Parse(dt.Rows[i]["DOE"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'" },
                            {"Company","'"+dt.Rows[i]["Company"].ToString().SQLQueryFilter()+"'" },
                            {"ChildCompany","'"+dt.Rows[i]["ChildCompany"].ToString().SQLQueryFilter()+"'" },
                            {"ProChkList","'"+dt.Rows[i]["ProChkList"].ToString()+"'" },
                            {"ProChkListDetail","'"+dt.Rows[i]["ProChkListDetail"].ToString().SQLQueryFilter()+"'" },
                            {"Prename","'"+dt.Rows[i]["Prename"].ToString().SQLQueryFilter()+"'" },
                            {"Forename","'"+dt.Rows[i]["Forename"].ToString().SQLQueryFilter()+"'" },
                            {"Surname","'"+dt.Rows[i]["Surname"].ToString().SQLQueryFilter()+"'" },
                            {"Age","'"+dt.Rows[i]["Age"].ToString().SQLQueryFilter()+"'" },
                            {"Sex","'"+dt.Rows[i]["Sex"].ToString().Trim().SQLQueryFilter()+"'" },
                            {"Address","'"+dt.Rows[i]["Address"].ToString().SQLQueryFilter()+"'" },
                            {"Tel","'"+dt.Rows[i]["Tel"].ToString().SQLQueryFilter()+"'" },
                            {"Email","'"+dt.Rows[i]["Email"].ToString().SQLQueryFilter()+"'" },
                            {"Physician","'"+dt.Rows[i]["Physician"].ToString().SQLQueryFilter()+"'" },
                            {"RegType","'"+dt.Rows[i]["RegType"].ToString().SQLQueryFilter()+"'" },
                            {"Programid",""+(dt.Rows[i]["Programid"].ToString()==""?"NULL":dt.Rows[i]["Programid"].ToString())+"" },
                            {"DIVI","'"+dt.Rows[i]["DIVI"].ToString().SQLQueryFilter()+"'" },
                            {"DEP","'"+dt.Rows[i]["DEP"].ToString().SQLQueryFilter()+"'" },
                            {"SEC","'"+dt.Rows[i]["SEC"].ToString().SQLQueryFilter()+"'" },
                            {"POS","'"+dt.Rows[i]["POS"].ToString().SQLQueryFilter()+"'" },
                            {"LAN","'"+dt.Rows[i]["LAN"].ToString().SQLQueryFilter()+"'" },
                            {"NAT","'"+dt.Rows[i]["NAT"].ToString().SQLQueryFilter()+"'" },
                            {"CNT_TRY","'"+dt.Rows[i]["CNT_TRY"].ToString().SQLQueryFilter()+"'" },
                            {"LOC","'"+dt.Rows[i]["LOC"].ToString().SQLQueryFilter()+"'" },
                            {"Payor","'"+dt.Rows[i]["Payor"].ToString().SQLQueryFilter()+"'" },
                            {"Epi_Rowid",""+(dt.Rows[i]["Epi_Rowid"].ToString().Trim()==""?"NULL":dt.Rows[i]["Epi_Rowid"].ToString())+"" },
                            {"ORD_STS","'"+dt.Rows[i]["ORD_STS"].ToString().SQLQueryFilter()+"'" },
                            {"STS","'"+dt.Rows[i]["STS"].ToString().SQLQueryFilter()+"'" },
                            {"DR_CDE","'"+dt.Rows[i]["DR_CDE"].ToString().SQLQueryFilter()+"'" },
                            {"NTE","'"+dt.Rows[i]["NTE"].ToString().SQLQueryFilter()+"'" },
                            {"Job","'"+dt.Rows[i]["Job"].ToString().SQLQueryFilter()+"'" },
                            {"BusUnit","'"+dt.Rows[i]["BusUnit"].ToString().SQLQueryFilter()+"'" },
                            {"BusDiv","'"+dt.Rows[i]["BusDiv"].ToString().SQLQueryFilter()+"'" },
                            {"Line","'"+dt.Rows[i]["Line"].ToString().SQLQueryFilter()+"'" },
                            {"Shift","'"+dt.Rows[i]["Shift"].ToString().SQLQueryFilter()+"'" },
                            {"Location","'"+dt.Rows[i]["Location"].ToString().SQLQueryFilter()+"'" },
                            {"GrpBook","'"+dt.Rows[i]["GrpBook"].ToString().SQLQueryFilter()+"'" },
                            {"BookCreate","'"+dt.Rows[i]["BookCreate"].ToString().SQLQueryFilter()+"'" },
                            {"HISExist","'"+dt.Rows[i]["HISExist"].ToString().SQLQueryFilter()+"'" },
                            {"SyncStatus",(dt.Rows[i]["SyncStatus"]!=DBNull.Value?"'"+dt.Rows[i]["SyncStatus"].ToString()+"'":"'0'") },
                            {"SyncWhen",(dt.Rows[i]["SyncWhen"]!=DBNull.Value?"'"+DateTime.Parse(dt.Rows[i]["SyncWhen"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'":"NULL") },
                            {"CUser","'"+clsGlobal.WindowsLogon()+"'" },
                            {"StatusFlag","'A'" }
                                },
                                new string[,] { { } },
                                out strSQL,
                                true))
                            {
                                countFail += 1;
                                result = false;
                            }
                            else
                            {
                                countSuccess += 1;
                            }
                        }
                        else
                        {
                            var tempHN = dt.Rows[i]["HN"].ToString();
                            countExist += 1;
                        }
                        #endregion
                        #region PatientChecklist
                        var countChecklistSuccessTemp = 0;
                        var countChecklistExistTemp = 0;
                        var countChecklistFailTemp = 0;
                        var countChecklistUpdateTemp = 0;
                        var setPatientChecklistToMobileStatus = setPatientChecklistToMobile(
                            dt.Rows[i]["PatientGUID"].ToString(),
                            dt.Rows[i]["Episode"].ToString(),
                            dt.Rows[i]["HN"].ToString(),
                            out countChecklistSuccessTemp, out countChecklistExistTemp, out countChecklistFailTemp,out countChecklistUpdateTemp);
                        countChecklistSuccess += countChecklistSuccessTemp;
                        countChecklistExist += countChecklistExistTemp;
                        countChecklistFail += countChecklistFailTemp;
                        countChecklistUpdate += countChecklistUpdateTemp;
                        #endregion
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                outMessage = ex.Message;
            }
            #endregion
            return result;
        }
        public string setPatientChecklistToMobile(string PatientGUID, string Episode, string HN, out int countChecklistSuccess, out int countChecklistExist, out int countChecklistFail, out int countChecklistUpdate)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            #region Variable
            var result = "F";//F=False , S=Success , E=Exist
            var dt = new DataTable();
            var outSQL = "";
            var clsSQLMain = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
            var clsSQLMobile = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            var clsTempData = new clsTempData();
            var strSQL = "";
            countChecklistSuccess = 0; countChecklistFail = 0; countChecklistExist = 0; countChecklistUpdate = 0;
            #endregion
            #region Procedure
            dt = clsTempData.getPatientChecklist(PatientGUID, Episode, HN);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var proStatusMobile = "";
                    strSQL = "SELECT IFNULL(ProStatus,0)ProStatus FROM patientchecklist WHERE RowID=" + dt.Rows[i]["RowID"].ToString() + ";";
                    proStatusMobile = clsSQLMobile.Return(strSQL);
                    if (proStatusMobile == "")
                    {
                        #region Insert
                        if (!clsSQLMobile.Insert(
                            "patientchecklist",
                            new string[,]
                            {
                                {"RowID",dt.Rows[i]["RowID"].ToString() },
                                {"PatientGUID","'"+PatientGUID+"'" },
                                {"HN","'"+HN+"'" },
                                {"Episode","'"+Episode+"'" },
                                {"CheckListID",dt.Rows[i]["CheckListID"].ToString() },
                                {"ProChkList","'"+dt.Rows[i]["ProChkList"].ToString()+"'" },
                                {"ProID",dt.Rows[i]["ProID"].ToString() },
                                {"WorkFlow","'"+dt.Rows[i]["WorkFlow"].ToString()+"'" },
                                {"WFID",dt.Rows[i]["WFID"].ToString() },
                                {"WFSequen",dt.Rows[i]["WFSequen"].ToString() },
                                {"ProStatus",dt.Rows[i]["ProStatus"].ToString() },
                                {"RegDate","NULL" },
                                {"ModifyDate","NULL" },
                                {"SyncStatus",(dt.Rows[i]["SyncWhen"]!=DBNull.Value?"'1'":"'0'") },
                                {"SyncWhen",(dt.Rows[i]["SyncWhen"]!=DBNull.Value?"'"+DateTime.Parse(dt.Rows[i]["SyncWhen"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'":"NULL") },
                                {"CUser","'"+clsGlobal.WindowsLogon()+"'" },
                                {"MWhen","NOW()" },
                                {"MUser","'"+clsGlobal.WindowsLogon()+"'" },
                            },
                            new string[,] { { } },
                            out outSQL, true
                            ))
                        {
                            result = "F";
                            countChecklistFail += 1;
                        }
                        else
                        {
                            result = "S";
                            countChecklistSuccess += 1;
                        }
                        #endregion
                    }
                    else if (int.Parse(dt.Rows[i]["ProStatus"].ToString()) > int.Parse(proStatusMobile))
                    {
                        //Update : ProStatus changed from main
                        #region Update
                        if (!clsSQLMobile.Update(
                            "patientchecklist",
                            new string[,]
                            {
                                {"ProStatus",dt.Rows[i]["ProStatus"].ToString() },
                                {"RegDate",(dt.Rows[i]["RegDate"]!=DBNull.Value?"'"+DateTime.Parse(dt.Rows[i]["RegDate"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'":"NULL") },
                                {"ModifyDate",(dt.Rows[i]["ModifyDate"]!=DBNull.Value?"'"+DateTime.Parse(dt.Rows[i]["ModifyDate"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'":"NULL") },
                                {"SyncStatus","'0'" },
                                {"SyncWhen","NOW()" },
                                {"CUser","'"+clsGlobal.WindowsLogon()+"'" },
                                {"MWhen","NOW()" },
                                {"MUser","'"+clsGlobal.WindowsLogon()+"'" },
                            },
                            new string[,] { { } },
                            "RowID="+ dt.Rows[i]["RowID"].ToString(),
                            out outSQL, true
                            ))
                        {
                            result = "F";
                            countChecklistFail += 1;
                        }
                        else
                        {
                            result = "U";
                            countChecklistUpdate += 1;
                        }
                        #endregion
                    }
                    else
                    {
                        result = "E";
                        countChecklistExist += 1;
                    }
                }
            }
            #endregion
            return result;
        }
        private string getDropDownListValue(ComboBox ddlName, String columnName)
        {
            #region Variable
            var result = "";
            #endregion
            #region Procedure
            try
            {
                if (ddlName.InvokeRequired)
                {
                    ddlName.Invoke(new MethodInvoker(delegate
                    {
                        var drv = (DataRowView)ddlName.SelectedItem;
                        var dr = drv.Row;
                        result = dr[columnName].ToString();
                    }));
                }
                else
                {
                    var drv = (DataRowView)ddlName.SelectedItem;
                    var dr = drv.Row;
                    result = dr[columnName].ToString();
                }
            }
            catch (Exception) { }
            #endregion
            return result;
        }
        private void wbSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            if (btSearch.InvokeRequired)
            {
                btSearch.Invoke(new MethodInvoker(delegate
                {
                    btSearch.Enabled = false;
                }));
            }
            if (btSync.InvokeRequired)
            {
                btSync.Invoke(new MethodInvoker(delegate
                {
                    btSync.Enabled = false;
                }));
            }
            if (anWaiting.InvokeRequired)
            {
                anWaiting.Invoke(new MethodInvoker(delegate
                {
                    anWaiting.Visible = true;
                }));
            }
            if (lblSyncToMobile.InvokeRequired)
            {
                lblSyncToMobile.Invoke(new MethodInvoker(delegate
                {
                    lblSyncToMobile.Text = "กำลังค้นหาข้อมูล...";
                }));
            }
            Search();
            if (btSearch.InvokeRequired)
            {
                btSearch.Invoke(new MethodInvoker(delegate
                {
                    btSearch.Enabled = true;
                }));
            }
            if (btSync.InvokeRequired)
            {
                btSync.Invoke(new MethodInvoker(delegate
                {
                    btSync.Enabled = true;
                }));
            }
            if (anWaiting.InvokeRequired)
            {
                anWaiting.Invoke(new MethodInvoker(delegate
                {
                    anWaiting.Visible = false;
                }));
            }
        }
    }
}