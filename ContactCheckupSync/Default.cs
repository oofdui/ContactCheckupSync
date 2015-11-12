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
    public partial class Default: Form
    {
        #region GlobalVariable
        ToolTip tt = new ToolTip();
        #endregion
        public Default()
        {
            InitializeComponent();
        }
        private void Default_Load(object sender, EventArgs e)
        {
            setDefault();
        }
        private void mnToMain_Click(object sender, EventArgs e)
        {
            setSyncToMain();
        }
        private void mnSyncToMobile_Click(object sender, EventArgs e)
        {
            setSyncToMobile();
        }
        private void mnReport_Click(object sender, EventArgs e)
        {
            setReport();
        }
        private void btCreateTable_Click(object sender, EventArgs e)
        {
            #region Variable
            var strSQL = new string[]
            {
                "CREATE TABLE Patient(PatientGUID VARCHAR(50) NOT NULL,HN VARCHAR(12) NOT NULL,Episode VARCHAR(13),LabEpisode VARCHAR(100),DOB DATETIME,No INT,EmployeeID VARCHAR(20),DOE DATETIME,Company VARCHAR(200),ChildCompany VARCHAR(200),ProChkList VARCHAR(50),ProChkListDetail VARCHAR(200),Prename VARCHAR(50),Forename VARCHAR(50),Surname VARCHAR(50),Age VARCHAR(50),Sex VARCHAR(50),Address VARCHAR(300),Tel VARCHAR(200),Email VARCHAR(50),Physician VARCHAR(50),RegType VARCHAR(10),Programid INT(4),DIVI VARCHAR(80),DEP VARCHAR(80),SEC VARCHAR(80),POS VARCHAR(99),LAN INT,CNT_TRY INT,LOC VARCHAR(50),Payor VARCHAR(100),Epi_Rowid DECIMAL(10, 0),ORD_STS VARCHAR(1),STS VARCHAR(1),DR_CDE VARCHAR(15),NTE VARCHAR(200),BusUnit VARCHAR(80),BusDiv VARCHAR(80),Line VARCHAR(80),Location VARCHAR(80),GrpBook VARCHAR(100),HISExist CHAR(1),SyncStatus CHAR(1) NOT NULL DEFAULT '0',SyncWhen DATETIME,CWhen TIMESTAMP DEFAULT CURRENT_TIMESTAMP,CUser VARCHAR(10) NOT NULL DEFAULT '',StatusFlag CHAR(1),PRIMARY KEY(PatientGUID));",
                "CREATE TABLE PatientChecklist(RowID INT NOT NULL,PatientGUID VARCHAR(50) NOT NULL,HNEpisode VARCHAR(13),CheckListID INT,ProChkList VARCHAR(200),ProID INT,WorkFlow VARCHAR(200),WFID INT,WFSequen INT,ProStatus FLOAT,ProStatusRemark VARCHAR(200),RegDate DATETIME,ModifyDate DATETIME,SyncStatus CHAR(1) NOT NULL DEFAULT '0',SyncWhen DATETIME,CWhen TIMESTAMP DEFAULT CURRENT_TIMESTAMP,CUser VARCHAR(10) DEFAULT '',MWhen DATETIME,MUser VARCHAR(10) DEFAULT '',PRIMARY KEY(RowID));",
                "CREATE TABLE ProStatusDetail(Code FLOAT NOT NULL,Detail VARCHAR(100) NOT NULL,PRIMARY KEY(Code));",
                "INSERT INTO ProStatusDetail(Code,Detail)VALUES('1','ปริ้นเอกสารแล้ว (สถานะเริ่มต้น)');",
                "INSERT INTO ProStatusDetail(Code,Detail)VALUES('2','ลงทะเบียนรับเอกสารแล้ว');",
                "INSERT INTO ProStatusDetail(Code,Detail)VALUES('3','ดำเนินการแล้ว');",
                "INSERT INTO ProStatusDetail(Code,Detail)VALUES('4','ยกเลิกการตรวจ');",
                "CREATE TABLE PatientLab(LabEpisode VARCHAR(100) NOT NULL,WFID INT(11) NOT NULL,CWhen DATETIME NOT NULL,StatusFlag CHAR(1) NOT NULL DEFAULT 'A',PRIMARY KEY(LabEpisode,WFID));"
            };
            var outMessage = "";
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            #endregion
            #region Procedure
            if (clsSQL.IsConnected())
            {
                for(int i = 0; i < strSQL.Length; i++)
                {
                    if (!clsSQL.Execute(strSQL[i],out outMessage))
                    {
                        MessageBox.Show(outMessage, "Error on CreateTable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                MessageBox.Show("ดำเนินการเสร็จสมบูรณ์", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูล Mobile ได้", "Database Connection Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }
        private void btClearData_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ข้อมูลทั้งหมดในระบบ Mobile จะถูกลบทิ้ง ยืนยันการลบข้อมูล ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No)
            {
                return;
            }
            #region Variable
            var strSQL = new string[]
            {
                "DELETE FROM patient;",
                "DELETE FROM patientchecklist;",
                "DELETE FROM patientlab;"
            };
            var outMessage = "";
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            #endregion
            #region Procedure
            if (clsSQL.IsConnected())
            {
                for (int i = 0; i < strSQL.Length; i++)
                {
                    if (!clsSQL.Execute(strSQL[i], out outMessage))
                    {
                        MessageBox.Show(outMessage, "Error on ClearData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                MessageBox.Show("ดำเนินการเสร็จสมบูรณ์", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูล Mobile ได้", "Database Connection Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }
        #region Default
        private void setDefault()
        {
            #region Tooltip
            tt.SetToolTip(btClose, "ปิดโปรแกรม");
            tt.SetToolTip(btMinimize, "ย่อหน้าต่าง");
            tt.SetToolTip(btMaximize, "ขยาย/ลดขนาดหน้าต่าง");
            tt.SetToolTip(btMove, "ย้ายหน้าต่าง");
            tt.SetToolTip(mnSyncToMain, "ซิงค์ข้อมูลจากระบบ Mobile กลับสู่ระบบหลัก");
            tt.SetToolTip(mnSyncToMobile, "ซิงค์ข้อมูลจากระบบหลักสู่ระบบ Mobile");
            #endregion
            setSyncToMain();

            lblHeader.Text = string.Format("{0} v.{1}", clsGlobal.ApplicationName, clsGlobal.ApplicationVersion());
            lblCredit.Text = "©2015 All rights reserved.  Powered by nithi.re";
            lblFooter.Text = string.Format("MobileServer : {0}"+Environment.NewLine+"MainServer : {1}",
                System.Configuration.ConfigurationManager.AppSettings["cs"].Split(new string[] { "uid=" },StringSplitOptions.None)[0],
                System.Configuration.ConfigurationManager.AppSettings["csMain"].Split(new string[] { "uid=" }, StringSplitOptions.None)[0]);
        }
        #endregion
        #region CommondEvent
        private void btClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }
        #endregion
        #region MoveEvent
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private void btMove_MouseDown(object sender, MouseEventArgs e)
        {
            eventMouseDown();
        }
        private void btMove_MouseMove(object sender, MouseEventArgs e)
        {
            eventMouseMove();
        }
        private void btMove_MouseUp(object sender, MouseEventArgs e)
        {
            eventMouseUp();
        }
        private void tbMenuTop_MouseDown(object sender, MouseEventArgs e)
        {
            eventMouseDown();
        }
        private void tbMenuTop_MouseMove(object sender, MouseEventArgs e)
        {
            eventMouseMove();
        }
        private void tbMenuTop_MouseUp(object sender, MouseEventArgs e)
        {
            eventMouseUp();
        }
        private void eventMouseDown()
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }
        private void eventMouseMove()
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }
        private void eventMouseUp()
        {
            dragging = false;
        }
        #endregion
        #region Function
        private Color getColorSoft(Color color, double amount)
        {
            byte r = (byte)((color.R) + (amount * 3));
            byte g = (byte)((color.G) + amount);
            //byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            byte b = (byte)color.B;
            return Color.FromArgb(r, g, b);
        }
        private IEnumerable<Control> getAllControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(
                ctrl => getAllControls(ctrl, type))
                .Concat(controls)
                .Where(c => c.GetType() == type);
        }
        private void setMenuActive(string Name)
        {
            var controls = getAllControls(this, typeof(Button));
            foreach (var control in controls)
            {
                var mn = (Button)control;
                if (mn.Name.Contains("mn"))
                {
                    if (mn.Name == Name)
                    {
                        mn.BackColor = ColorTranslator.FromHtml("#FFF");
                        mn.ForeColor = ColorTranslator.FromHtml("#000");
                    }
                    else
                    {
                        mn.BackColor = getColorSoft(ColorTranslator.FromHtml("#00A2E8"), 50);
                        mn.ForeColor = ColorTranslator.FromHtml("#484848");
                    }
                }
            }
        }
        private string getDropDownListValue(ComboBox ddlName, String columnName)
        {
            #region Variable
            var result = "";
            #endregion
            #region Procedure
            try
            {
                var drv = (DataRowView)ddlName.SelectedItem;
                var dr = drv.Row;
                result = dr[columnName].ToString();
            }
            catch (Exception) { }
            #endregion
            return result;
        }
        #endregion
        #region SyncToMain
        private void setSyncToMain()
        {
            setMenuActive("mnSyncToMain");
            tbContent.RowStyles[1].Height = 0;
            tbSyncToMobile.Visible = false;
            tbSyncToMain.Visible = true;
        }
        #endregion
        #region SyncToMobile
        private void setSyncToMobile()
        {
            setMenuActive("mnSyncToMobile");
            tbContent.RowStyles[0].Height = 0;
            tbSyncToMobile.Visible = true;
            tbSyncToMain.Visible = false;
            var clsSQLMain = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
            var clsSQLMobile = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            if (!clsSQLMain.IsConnected() && !clsSQLMobile.IsConnected())
            {
                btSearch.Enabled = false; //btSync.Enabled = false;
                dtDOEFrom.Enabled = false;dtDOETo.Enabled = false;
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
        private void ddlCompany_Click(object sender, EventArgs e)
        {
            
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
            #region Variable
            var dt = new DataTable();
            var clsTempData = new clsTempData();
            #endregion
            #region Procedure
            dt = clsTempData.getPatient(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany,"Company"));
            if(dt!=null && dt.Rows.Count > 0)
            {
                clsGlobal.dtPatient = dt.Copy();btSync.Enabled = true;
                lblSyncToMobile.Text = "";
                #region RemoveColumn
                string[] columns = { "PatientGUID", "LabEpisode", "Address", "Tel", "Email", "Physician", "RegType", "Programid", "DIVI", "DEP", "SEC", "POS", "LAN", "NAT", "CNT_TRY", "LOC", "Payor", "Epi_Rowid", "ORD_STS", "STS", "DR_CDE", "NTE", "Job", "BusUnit", "BusDiv", "Line", "Shift", "Location", "GrpBook", "HISExist" };
                for(int i = 0; i < columns.Length; i++)
                {
                    dt.Columns.Remove(columns[i]);
                }
                dt.AcceptChanges();
                #endregion
                gvSyncToMobile.DataSource = dt;
                lblSyncToMobile.Text = string.Format("พบข้อมูลทั้งหมด {0} รายการ", dt.Rows.Count.ToString());
            }
            else
            {
                clsGlobal.dtPatient = null; btSync.Enabled = false;
                lblSyncToMobile.Text = "- ไม่พบข้อมูลที่ต้องการ -";
            }
            #endregion
        }
        private void btSync_Click(object sender, EventArgs e)
        {
            backgroundWorkerSyncToMobile.RunWorkerAsync();
        }
        public void setProgressBarSyncToMobile(int value,int maximumValue)
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
        private void SyncToMobile()
        {
            if (clsGlobal.dtPatient != null && clsGlobal.dtPatient.Rows.Count > 0)
            {
                var countSuccess = 0; var countFail = 0; var countExist = 0;
                var countChecklistSuccess = 0; var countChecklistFail = 0; var countChecklistExist = 0;
                var outMessage = "";
                if (setPatientToMobile(clsGlobal.dtPatient, out countSuccess, out countFail, out countExist, out countChecklistSuccess, out countChecklistFail, out countChecklistExist, out outMessage))
                {
                    if (lblSyncToMobile.InvokeRequired)
                    {
                        lblSyncToMobile.Invoke(new MethodInvoker(delegate
                        {
                            lblSyncToMobile.Text = string.Format(
                                "Sync ข้อมูลสู่ระบบ Mobile เสร็จสิ้น" + Environment.NewLine + Environment.NewLine +
                                "    @Patient Success : {0} , Exist : {1} , Fail : {2}" + Environment.NewLine +
                                "    @Checklist Success: {3} , Exist : {4} , Fail : {5}",
                                countSuccess.ToString(),
                                countExist.ToString(),
                                countFail.ToString(),
                                countChecklistSuccess.ToString(),
                                countChecklistExist.ToString(),
                                countChecklistFail.ToString()
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
                        lblSyncToMobile.Text+=clsTempData.getPatientNotHadChecklist();
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
            SyncToMobile();
            if (btSync.InvokeRequired)
            {
                btSync.Invoke(new MethodInvoker(delegate
                {
                    btSync.Enabled = true;
                }));
            }
        }
        public bool setPatientToMobile(DataTable dt, out int countSuccess, out int countFail, out int countExist, out int countChecklistSuccess, out int countChecklistFail, out int countChecklistExist, out string outMessage)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            #region Variable
            var result = true;
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            var strSQL = "";
            countSuccess = 0; countFail = 0; countExist = 0;
            countChecklistSuccess = 0; countChecklistFail = 0; countChecklistExist = 0;
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
                            {"Age","'"+dt.Rows[i]["Age"].ToString()+"'" },
                            {"Sex","'"+dt.Rows[i]["Sex"].ToString()+"'" },
                            {"Address","'"+dt.Rows[i]["Address"].ToString().SQLQueryFilter()+"'" },
                            {"Tel","'"+dt.Rows[i]["Tel"].ToString().SQLQueryFilter()+"'" },
                            {"Email","'"+dt.Rows[i]["Email"].ToString()+"'" },
                            {"Physician","'"+dt.Rows[i]["Physician"].ToString()+"'" },
                            {"RegType","'"+dt.Rows[i]["RegType"].ToString()+"'" },
                            {"Programid","'"+dt.Rows[i]["Programid"].ToString()+"'" },
                            {"DIVI","'"+dt.Rows[i]["DIVI"].ToString()+"'" },
                            {"DEP","'"+dt.Rows[i]["DEP"].ToString()+"'" },
                            {"SEC","'"+dt.Rows[i]["SEC"].ToString()+"'" },
                            {"POS","'"+dt.Rows[i]["POS"].ToString()+"'" },
                            {"LAN","'"+dt.Rows[i]["LAN"].ToString()+"'" },
                            {"NAT","'"+dt.Rows[i]["NAT"].ToString()+"'" },
                            {"CNT_TRY","'"+dt.Rows[i]["CNT_TRY"].ToString()+"'" },
                            {"LOC","'"+dt.Rows[i]["LOC"].ToString()+"'" },
                            {"Payor","'"+dt.Rows[i]["Payor"].ToString()+"'" },
                            {"Epi_Rowid","'"+dt.Rows[i]["Epi_Rowid"].ToString()+"'" },
                            {"ORD_STS","'"+dt.Rows[i]["ORD_STS"].ToString()+"'" },
                            {"STS","'"+dt.Rows[i]["STS"].ToString()+"'" },
                            {"DR_CDE","'"+dt.Rows[i]["DR_CDE"].ToString()+"'" },
                            {"NTE","'"+dt.Rows[i]["NTE"].ToString()+"'" },
                            {"Job","'"+dt.Rows[i]["Job"].ToString()+"'" },
                            {"BusUnit","'"+dt.Rows[i]["BusUnit"].ToString()+"'" },
                            {"BusDiv","'"+dt.Rows[i]["BusDiv"].ToString()+"'" },
                            {"Line","'"+dt.Rows[i]["Line"].ToString()+"'" },
                            {"Shift","'"+dt.Rows[i]["Shift"].ToString()+"'" },
                            {"Location","'"+dt.Rows[i]["Location"].ToString()+"'" },
                            {"GrpBook","'"+dt.Rows[i]["GrpBook"].ToString()+"'" },
                            {"HISExist","'"+dt.Rows[i]["HISExist"].ToString()+"'" },
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
                        var setPatientChecklistToMobileStatus = setPatientChecklistToMobile(
                            dt.Rows[i]["PatientGUID"].ToString(),
                            dt.Rows[i]["Episode"].ToString(),
                            dt.Rows[i]["HN"].ToString(),
                            out countChecklistSuccessTemp, out countChecklistExistTemp, out countChecklistFailTemp);
                        countChecklistSuccess += countChecklistSuccessTemp;
                        countChecklistExist += countChecklistExistTemp;
                        countChecklistFail += countChecklistFailTemp;
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
        public string setPatientChecklistToMobile(string PatientGUID, string Episode, string HN, out int countChecklistSuccess, out int countChecklistExist, out int countChecklistFail)
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
            countChecklistSuccess = 0; countChecklistFail = 0; countChecklistExist = 0;
            #endregion
            #region Procedure
            dt = clsTempData.getPatientChecklist(PatientGUID, Episode, HN);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSQL = "SELECT COUNT(RowID) FROM patientchecklist WHERE RowID=" + dt.Rows[i]["RowID"].ToString() + ";";
                    var count = clsSQLMobile.Return(strSQL);
                    if (count != "0")
                    {
                        result = "E";
                        countChecklistExist += 1;
                    }
                    else
                    {
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
                            {"RegDate","'"+DateTime.Parse(dt.Rows[i]["RegDate"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'" },
                            {"ModifyDate","'"+DateTime.Parse(dt.Rows[i]["ModifyDate"].ToString()).ToString("yyyy-MM-dd HH:mm")+"'" },
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
                    }
                }
            }
            #endregion
            return result;
        }
        #endregion
        #region Report
        private void setReport()
        {
            setMenuActive("mnReport");
            tbContent.RowStyles[0].Height = 0;
            tbContent.RowStyles[1].Height = 0;
            tbSyncToMobile.Visible = false;
            tbSyncToMain.Visible = false;
        }
        #endregion
    }
}
