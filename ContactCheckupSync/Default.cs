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