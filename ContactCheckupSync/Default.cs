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
            setUsageLog();
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
                "CREATE TABLE patient (PatientGUID varchar(50) NOT NULL,HN varchar(12) NOT NULL,Episode varchar(13) default NULL,LabEpisode varchar(100) default NULL,DOB datetime default NULL,No int(11) default NULL,EmployeeID varchar(20) default NULL,DOE datetime default NULL,Company varchar(200) default NULL,ChildCompany varchar(200) default NULL,ProChkList varchar(50) default NULL,ProChkListDetail varchar(200) default NULL,Prename varchar(50) default NULL,Forename varchar(50) default NULL,Surname varchar(50) default NULL,Age varchar(50) default NULL,Sex varchar(50) default NULL,Address varchar(300) default NULL,Tel varchar(200) default NULL,Email varchar(50) default NULL,Physician varchar(50) default NULL,RegType varchar(10) default NULL,Programid int(4) default NULL,DIVI varchar(80) default NULL,DEP varchar(80) default NULL,SEC varchar(80) default NULL,POS varchar(99) default NULL,LAN int(11) default NULL,NAT int(11) default NULL,CNT_TRY int(11) default NULL,LOC varchar(50) default NULL,Payor varchar(100) default NULL,Epi_Rowid decimal(10,0) default NULL,ORD_STS varchar(1) default NULL,STS varchar(1) default NULL,DR_CDE varchar(15) default NULL,NTE varchar(200) default NULL,Job varchar(80) default NULL,BusUnit varchar(80) default NULL,BusDiv varchar(80) default NULL,Line varchar(80) default NULL,Shift varchar(300) default NULL,Location varchar(80) default NULL,GrpBook varchar(100) default NULL,HISExist char(1) default NULL,SyncStatus char(1) NOT NULL default '0',SyncWhen datetime default NULL,CWhen timestamp NOT NULL default CURRENT_TIMESTAMP,CUser varchar(10) NOT NULL default '',StatusFlag char(1) default NULL,PRIMARY KEY(PatientGUID));",
                "CREATE TABLE patientchecklist (RowID int(11) NOT NULL,PatientGUID varchar(50) NOT NULL,HN varchar(12) default NULL,Episode varchar(13) default NULL,CheckListID int(11) default NULL,ProChkList varchar(200) default NULL,ProID int(11) default NULL,WorkFlow varchar(200) default NULL,WFID int(11) default NULL,WFSequen int(11) default NULL,ProStatus float default NULL,ProStatusRemark varchar(200) default NULL,RegDate datetime default NULL,ModifyDate datetime default NULL,SyncStatus char(1) NOT NULL default '0',SyncWhen datetime default NULL,CWhen timestamp NOT NULL default CURRENT_TIMESTAMP,CUser varchar(10) default '',MWhen datetime default NULL,MUser varchar(10) default '',PRIMARY KEY(RowID));",
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
        private void setUsageLog()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["enableUsageLog"].Trim().ToLower() == "true")
            {
                try
                {
                    wsCenter.ServiceSoapClient wsCenter = new wsCenter.ServiceSoapClient();
                    wsCenter.InsertLogApplicationBySite(
                        clsGlobal.ApplicationName,
                        clsGlobal.ApplicationVersion(),
                        System.Configuration.ConfigurationManager.AppSettings["site"],
                        clsGlobal.WindowsLogon(),
                        clsGlobal.IPAddress(),
                        clsGlobal.ComputerName());
                }
                catch(Exception ex)
                {

                }
            }
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
        public void DisposeAllButThis()
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.Close();
            }
        }
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

            DisposeAllButThis();

            var childForm = new SyncToMain();
            childForm.MdiParent = this;
            childForm.Parent = this.pnDefault;
            childForm.Text = "SyncToMain";
            childForm.WindowState = FormWindowState.Maximized;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Show();
        }
        #endregion
        #region SyncToMobile
        private void setSyncToMobile()
        {
            setMenuActive("mnSyncToMobile");
            DisposeAllButThis();

            var childForm = new SyncToMobile();
            childForm.MdiParent = this;
            childForm.Parent = this.pnDefault;
            childForm.Text = "SyncToMobile";
            childForm.WindowState = FormWindowState.Maximized;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Show();
        }
        #endregion
        #region Report
        private void setReport()
        {
            setMenuActive("mnReport");
            DisposeAllButThis();

            var childForm = new Report();
            childForm.MdiParent = this;
            childForm.Parent = this.pnDefault;
            childForm.Text = "Report";
            childForm.WindowState = FormWindowState.Maximized;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Show();
        }
        #endregion
    }
}