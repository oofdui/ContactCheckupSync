using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _ContactCheckupSync
{
    public partial class SyncToMain : Form
    {
        #region GlobalVariable
        int syncTimerSecond = int.Parse(System.Configuration.ConfigurationManager.AppSettings["syncTimerSecond"]);
        int syncTimerTryAgainSecond = int.Parse(System.Configuration.ConfigurationManager.AppSettings["syncTimerTryAgainSecond"]);
        int syncTimerSecondCount = 0;
        string pathSync = System.Configuration.ConfigurationManager.AppSettings["pathSync"];
        string pathSyncLocal=clsGlobal.ExecutePathBuilder()+@"Sync\";
        #endregion
        public SyncToMain()
        {
            InitializeComponent();
        }
        private void SyncToMain_Load(object sender, EventArgs e)
        {
            txtAddHours.Text = System.Configuration.ConfigurationManager.AppSettings["hourAddSync"];
            lblSyncPathMobile.Text = pathSyncLocal;
            lblSyncPathMain.Text = pathSync;
            lblDefault.Text = "";
            tmDefault.Enabled = true;
            tmDefault.Start();
        }
        private void btStart_Click(object sender, EventArgs e)
        {
            #region ConnectionChecker
            /*
            var message = "";
            var clsSQLMain = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
            var clsSQLMobile = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            if(!clsSQLMain.IsConnected() || !clsSQLMobile.IsConnected())
            {
                message = "ไม่สามารถเชื่อมต่อฐานข้อมูล ";
                if (!clsSQLMain.IsConnected())
                {
                    message += " Main ";
                }
                if (!clsSQLMobile.IsConnected())
                {
                    message += " Mobile ";
                }
                message += "ได้";
                MessageBox.Show(message,"ConnectionError",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            */
            #endregion            
            tmDefault.Enabled = true;
            tmDefault.Start();
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            bwDefault.CancelAsync();
            bwTimer.CancelAsync();
            tmDefault.Stop();
            tmDefault.Enabled = false;
            syncTimerSecondCount = 0;
            #region ProgressBar
            if (pbDefault.InvokeRequired)
            {
                pbDefault.Invoke(new MethodInvoker(delegate
                {
                    pbDefault.Visible = false;
                    pbDefault.Value = 0;
                }));
            }
            #endregion
            #region btStart
            if (btStart.InvokeRequired)
            {
                btStart.Invoke(new MethodInvoker(delegate
                {
                    btStart.Enabled = true;
                }));
            }
            else
            {
                btStart.Enabled = true;
            }
            #endregion
            #region btStop
            if (btStop.InvokeRequired)
            {
                btStop.Invoke(new MethodInvoker(delegate
                {
                    btStop.Enabled = false;
                }));
            }
            else
            {
                btStop.Enabled = false;
            }
            #endregion
            #region Animation
            if (anLoading.InvokeRequired)
            {
                anLoading.Invoke(new MethodInvoker(delegate
                {
                    anLoading.Visible = false;
                }));
            }
            else
            {
                anLoading.Visible = false;
            }
            #endregion
            #region Label
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = "";
                }));
            }
            else
            {
                lblDefault.Text = "";
            }
            #endregion
        }
        private void bwDefault_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Control
            if (btStart.InvokeRequired)
            {
                btStart.Invoke(new MethodInvoker(delegate
                {
                    btStart.Enabled = false;
                }));
            }
            else
            {
                btStart.Enabled = false;
            }
            if (btStop.InvokeRequired)
            {
                btStop.Invoke(new MethodInvoker(delegate
                {
                    btStop.Enabled = true;
                }));
            }
            else
            {
                btStop.Enabled = true;
            }
            if (anLoading.InvokeRequired)
            {
                anLoading.Invoke(new MethodInvoker(delegate
                {
                    anLoading.Visible = true;
                }));
            }
            #endregion
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            #region Variable
            var clsTempData = new clsTempData();
            var clsSQLMain = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
            var clsSQLMobile = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            var dtMobile = new DataTable();
            var dtMain = new DataTable();
            var tblPatientListSTS = "";
            var tblPatientStatusOnMobile = "";
            var outSQL = "";
            var outMessage = "";
            var countSuccess = 0; var countFail = 0; var countDuplicate = 0;
            var clsInvoker = new clsInvoker();
            var fileName = "SYNC_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".xml";
            #endregion
            #region Procedure
            try
            {
                clsGlobal.AddHours = txtAddHours.Text.Trim();
                countSuccess = 0; countFail = 0; countDuplicate = 0;
                dtMobile = clsTempData.getPatientChecklistMobile();
                clsInvoker.setListView(
                    lvDefault,
                    Color.Green,
                    99,
                    new string[] {
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Start", "", "เริ่มขั้นตอนการ Sync" }
                );
                if (dtMobile != null && dtMobile.Rows.Count > 0)
                {
                    clsInvoker.setListView(
                        lvDefault,
                        Color.Green,
                        99,
                        new string[] {
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Syncing", "", "เตรียมดำเนินการสร้างไฟล์ทั้งหมด "+dtMobile.Rows.Count.ToString()+" รายการ" }
                    );
                    DirectoryInfo di = new DirectoryInfo(pathSyncLocal);
                    if (!di.Exists)
                    {
                        clsInvoker.setListView(
                                lvDefault,
                                Color.Red,
                                99,
                                new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", "Folder's not exist. : "+di.FullName }
                                );
                        di.Create();
                        clsInvoker.setListView(
                                lvDefault,
                                Color.Green,
                                99,
                                new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", "", "สร้างโฟล์เดอร์เรียบร้อย : "+di.FullName }
                                );
                    }
                    FileInfo fi = new FileInfo(string.Format("{0}{1}", di.FullName, fileName));
                    if (fi.Exists) { fi.Delete(); }
                    if (XMLCreator(dtMobile, fi.FullName))
                    {
                        clsInvoker.setListView(
                                lvDefault,
                                Color.Green,
                                99,
                                new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", "", "Export File Success "+dtMobile.Rows.Count.ToString()+" records. : "+fi.FullName }
                                );
                        try
                        {
                            #region CopyToServer
                            FileInfo fiServer = new FileInfo(string.Format("{0}{1}", pathSync, fileName));
                            if (fiServer.Exists)
                            {
                                clsInvoker.setListView(
                                    lvDefault,
                                    Color.Orange,
                                    99,
                                    new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Exist", "", "File Exist : "+fiServer.FullName }
                                    );
                                try
                                {
                                    fiServer.Delete();
                                    clsInvoker.setListView(
                                        lvDefault,
                                        Color.Green,
                                        99,
                                        new string[] {
                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", "", "Delete Exist" }
                                        );
                                }
                                catch (Exception exDelete)
                                {
                                    clsInvoker.setListView(
                                        lvDefault,
                                        Color.Red,
                                        99,
                                        new string[] {
                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", "Delete Exist : "+exDelete.Message }
                                        );
                                }
                            }
                            try
                            {
                                clsInvoker.setListView(
                                        lvDefault,
                                        Color.Orange,
                                        99,
                                        new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Processing...", "", "Copy file to server : "+fiServer.FullName }
                                        );
                                fi.CopyTo(fiServer.FullName);
                                clsInvoker.setListView(
                                        lvDefault,
                                        Color.Green,
                                        99,
                                        new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", "", "Copy file to server" }
                                        );
                                fi.Delete();
                            }
                            catch (Exception exCopy)
                            {
                                clsInvoker.setListView(
                                        lvDefault,
                                        Color.Red,
                                        99,
                                        new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", "Copy file to server : "+exCopy.Message }
                                        );
                                fi.Delete();
                            }
                            #endregion
                        }
                        catch(Exception exCopyFile)
                        {
                            clsInvoker.setListView(
                                    lvDefault,
                                    Color.Red,
                                    99,
                                    new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", "Copy file to server : "+pathSync + fileName+" : "+exCopyFile.Message }
                                    );
                        }
                    }
                    else
                    {
                        clsInvoker.setListView(
                                lvDefault,
                                Color.Red,
                                99,
                                new string[] {
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", "Export File Fail. : "+fi.FullName }
                                );
                    }
                    return;
                    //clsInvoker.setProgressBar(pbDefault, dtMobile.Rows.Count, 0);
                    //for (int i = 0; i < dtMobile.Rows.Count; i++)
                    //{
                    //    if (bwDefault.CancellationPending)
                    //    {
                    //        clsInvoker.setListView(
                    //            lvDefault, 
                    //            Color.Green, 
                    //            99,
                    //            new string[] {
                    //                DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Cancel", "", "Cancel by user." }
                    //            );
                    //        e.Cancel = true;
                    //        syncTimerSecondCount = 0;
                    //        clsInvoker.setProgressBar(pbDefault, dtMobile.Rows.Count, 0);
                    //        return;
                    //    }
                    //    if (!clsSQLMain.IsConnected() || !clsSQLMobile.IsConnected())
                    //    {
                    //        ListViewBuilder(lvDefault, Color.Red, 99,
                    //            new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", "Cannot connect database." });
                    //        continue;
                    //    }
                    //    #region Update tblPatientListSTS & tblPatientStatusOnMobile
                    //    if (dtMobile.Rows[i]["WFID"].ToString().Trim() == "1" && float.Parse(dtMobile.Rows[i]["ProStatus"].ToString().Trim()) >= 2)
                    //    {
                    //        tblPatientListSTS = clsSQLMain.Return("SELECT STS FROM tblPatientList WHERE PatientUID='" + dtMobile.Rows[i]["PatientGUID"].ToString().Trim() + "';");
                    //        tblPatientStatusOnMobile = clsSQLMain.Return("SELECT StatusOnMobile FROM Patient WHERE rowguid='" + dtMobile.Rows[i]["PatientGUID"].ToString().Trim() + "';");
                    //        if (tblPatientListSTS != "R" || tblPatientStatusOnMobile != "R")
                    //        {
                    //            if (clsSQLMain.Execute("UPDATE tblPatientList SET STS='R',SyncWhen=GETDATE() WHERE PatientUID='" + dtMobile.Rows[i]["PatientGUID"].ToString().Trim() + "';UPDATE Patient SET SyncStatus='1',SyncWhen=GETDATE(),StatusOnMobile='R' WHERE rowguid='" + dtMobile.Rows[i]["PatientGUID"].ToString().Trim() + "';"))
                    //            {
                    //                countSuccess += 1;
                    //                ListViewBuilder(lvDefault, Color.Green, 99,
                    //                    new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", dtMobile.Rows[i]["HN"].ToString(), "Update tblPatientList.STS,Patient.StatusOnMobile Complete." });
                    //                #region UpdateSyncStatus
                    //                if (clsSQLMobile.Update(
                    //                    "patient",
                    //                    new string[,]
                    //                    {
                    //                        {"SyncStatus","'1'" },
                    //                        {"SyncWhen","SYSDATE()" }
                    //                    },
                    //                    new string[,] { { } },
                    //                    "PatientGUID='" + dtMobile.Rows[i]["PatientGUID"].ToString() + "'",
                    //                    out outSQL, true
                    //                    ))
                    //                {
                    //                    ListViewBuilder(lvDefault, Color.Green, 99,
                    //                        new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", dtMobile.Rows[i]["HN"].ToString(), "Update SyncStatus Complete." });
                    //                }
                    //                else
                    //                {
                    //                    ListViewBuilder(lvDefault, Color.Red, 99,
                    //                        new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", dtMobile.Rows[i]["HN"].ToString(), "Update SyncStatus Fail." });
                    //                }
                    //                #endregion
                    //            }
                    //            else
                    //            {
                    //                countFail += 1;
                    //                ListViewBuilder(lvDefault, Color.Red, 99,
                    //                    new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", dtMobile.Rows[i]["HN"].ToString(), "Update tblPatientList.STS Fail." });
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //    dtMain = clsTempData.getPatientChecklistMain(dtMobile.Rows[i]["RowID"].ToString());
                    //    if (dtMain != null && dtMain.Rows.Count > 0)
                    //    {
                    //        if (dtMobile.Rows[i]["ProStatus"].ToString().Trim() != dtMain.Rows[0]["ProStatus"].ToString().Trim() ||
                    //            dtMobile.Rows[i]["ProStatusRemark"].ToString().Trim() != dtMain.Rows[0]["ProStatusRemark"].ToString().Trim()/* ||
                    //        dtMobile.Rows[i]["RegDate"].ToString().Trim() != dtMain.Rows[0]["RegDate"].ToString().Trim() ||
                    //        dtMobile.Rows[i]["ModifyDate"].ToString().Trim() != dtMain.Rows[0]["ModifyDate"].ToString().Trim()*/)
                    //        {
                    //            #region Update
                    //            if (!clsSQLMain.Update(
                    //                "tblCheckList",
                    //                new string[,]
                    //                {
                    //                {"ProStatus",dtMobile.Rows[i]["ProStatus"].ToString().Trim() },
                    //                {"ProStatusRemark","'"+dtMobile.Rows[i]["ProStatusRemark"].ToString().SQLQueryFilter()+"'" },
                    //                {"RegDate",(dtMobile.Rows[i]["RegDate"].ToString()!=""?"'"+DateTime.Parse(dtMobile.Rows[i]["RegDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")+"'":"NULL") },
                    //                {"ModifyDate",(dtMobile.Rows[i]["ModifyDate"].ToString()!=""?"'"+DateTime.Parse(dtMobile.Rows[i]["ModifyDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")+"'":"NULL") },
                    //                {"SyncWhen","GETDATE()"}
                    //                },
                    //                new string[,] { { } },
                    //                "RowID=" + dtMobile.Rows[i]["RowID"].ToString(), out outSQL, out outMessage, true))
                    //            {
                    //                #region LogUpdate
                    //                countFail += 1;
                    //                ListViewBuilder(lvDefault, Color.Red, 99,
                    //                    new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", dtMobile.Rows[i]["HN"].ToString(), dtMobile.Rows[i]["WorkFlow"].ToString() + " : " + dtMobile.Rows[i]["ProStatus"].ToString().Trim() + "->" + dtMain.Rows[0]["ProStatus"].ToString().Trim() + Environment.NewLine + outMessage });
                    //                #endregion
                    //            }
                    //            else
                    //            {
                    //                #region LogUpdate
                    //                countSuccess += 1;
                    //                ListViewBuilder(lvDefault, Color.Green, 99,
                    //                    new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Success", dtMobile.Rows[i]["HN"].ToString(), dtMobile.Rows[i]["WorkFlow"].ToString() + " : " + dtMobile.Rows[i]["ProStatus"].ToString().Trim() + "->" + dtMain.Rows[0]["ProStatus"].ToString().Trim() });
                    //                #endregion
                    //            }
                    //            #endregion
                    //        }
                    //    }
                    //    else
                    //    {
                    //        #region LogUpdate
                    //        //countDuplicate += 1;
                    //        //ListViewBuilder(lvDefault, Color.Blue, 99,
                    //        //            new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "NoChange", dtMobile.Rows[i]["HN"].ToString(), dtMobile.Rows[i]["WorkFlow"].ToString() + " : " + dtMobile.Rows[i]["ProStatus"].ToString().Trim() + "->" + dtMain.Rows[0]["ProStatus"].ToString().Trim() });
                    //        #endregion
                    //    }
                    //    #region ProgressBar
                    //    if (pbDefault.InvokeRequired)
                    //    {
                    //        pbDefault.Invoke(new MethodInvoker(delegate
                    //        {
                    //            pbDefault.Value += 1;
                    //        }));
                    //    }
                    //    #endregion
                    //}
                    //#region ProgressBar
                    //if (pbDefault.InvokeRequired)
                    //{
                    //    pbDefault.Invoke(new MethodInvoker(delegate
                    //    {
                    //        pbDefault.Visible = false;
                    //    }));
                    //}
                    //#endregion
                    //#region LogUpdate
                    //ListViewBuilder(lvDefault, Color.Green, 99,
                    //    new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Summary", "", "SyncSuccess : " + countSuccess.ToString() + " | Fail : " + countFail.ToString() + " | NoChange : " + countDuplicate.ToString() + " " });
                    //#endregion
                }
                else
                {
                    clsInvoker.setListView(
                        lvDefault,
                        Color.Orange,
                        99,
                        new string[] {
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Warn", "", "ไม่พบข้อมูลสำหรับการ Sync" }
                    );
                }
            }
            catch (Exception exMain)
            {
                ListViewBuilder(lvDefault, Color.Red, 99,
                    new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "", exMain.Message });
            }
            #endregion
        }
        private void ListViewBuilderInvoke(ListView listView, Color? color, int columnFullWidth, params string[] value)
        {
            listView.Items.Add(new ListViewItem(value));
            if (color != null)
            {
                listView.Items[listView.Items.Count - 1].ForeColor = (Color)color;
            }
            listView.EnsureVisible(listView.Items.Count - 1);
            ListViewResizeColumn(listView, columnFullWidth);
        }
        public void ListViewBuilder(ListView listView, Color? color = null, int columnFullWidth = 99, params string[] value)
        {
            if (listView.InvokeRequired)
            {
                listView.Invoke(new MethodInvoker(delegate
                {
                    ListViewBuilderInvoke(listView, color, columnFullWidth, value);
                }));
            }
            else
            {
                ListViewBuilderInvoke(listView, color, columnFullWidth, value);
            }
        }
        private void ListViewResizeColumnInvoke(ListView listView, int column)
        {
            var totalColumnWidth = 0;
            var calculateColumnWidth = 0;
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                if (column == 99)
                {
                    if (i < listView.Columns.Count - 1)
                    {
                        listView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }
                else
                {
                    if (column != i)
                    {
                        listView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                        totalColumnWidth += listView.Columns[i].Width;
                    }
                }
            }
            #region FullFill
            if (column == 99)
            {
                listView.Columns[listView.Columns.Count - 1].Width = -2;
            }
            else
            {
                calculateColumnWidth = listView.Width - totalColumnWidth - (listView.Width / 10);//ลบด้วย 10% ของความกว้างทั้งหมดอีกรอบกันเลย
                listView.Columns[column].Width = calculateColumnWidth;
                listView.Columns[listView.Columns.Count - 1].Width = -2;
            }
            #endregion
        }
        public void ListViewResizeColumn(ListView listView, int column = 99)
        {
            if (listView.InvokeRequired)
            {
                #region Invoke
                listView.Invoke(new MethodInvoker(delegate
                {
                    ListViewResizeColumnInvoke(listView, column);
                }));
                #endregion
            }
            else
            {
                ListViewResizeColumnInvoke(listView, column);
            }
        }
        private void tmDefault_Tick(object sender, EventArgs e)
        {
            var clsSQLMobile = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            var clsSQLMain = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);

            if (syncTimerSecondCount>=syncTimerSecond)
            {
                syncTimerSecondCount = 0;
            }
            if (syncTimerSecondCount == 0)
            {
                if(clsSQLMobile.IsConnected() && clsSQLMain.IsConnected())
                {
                    if (!bwDefault.IsBusy)
                    {
                        bwDefault.RunWorkerAsync();
                    }
                    else
                    {
                        syncTimerSecondCount = syncTimerSecond - syncTimerTryAgainSecond;
                    }
                }
                else
                {
                    if (btStart.InvokeRequired)
                    {
                        btStart.Invoke(new MethodInvoker(delegate
                        {
                            btStart.Enabled = false;
                        }));
                    }
                    else
                    {
                        btStart.Enabled = false;
                    }
                    if (btStop.InvokeRequired)
                    {
                        btStop.Invoke(new MethodInvoker(delegate
                        {
                            btStop.Enabled = true;
                        }));
                    }
                    else
                    {
                        btStop.Enabled = true;
                    }
                    if (anLoading.InvokeRequired)
                    {
                        anLoading.Invoke(new MethodInvoker(delegate
                        {
                            anLoading.Visible = true;
                        }));
                    }
                    else
                    {
                        anLoading.Visible = true;
                    }
                    syncTimerSecondCount = syncTimerSecond-syncTimerTryAgainSecond;
                    ListViewBuilder(lvDefault, Color.Red, 99,
                        new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Fail", "","Cannot connect database."});
                }
            }
            syncTimerSecondCount += 1;
            if (!bwTimer.IsBusy)
            {
                bwTimer.RunWorkerAsync();
            }
        }
        private void bwTimer_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Timer
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = string.Format("ระบบจะทำงานทุกๆ {0} วินาที ขณะนี้ตัวนับอยู่ที่ {1} วินาที", syncTimerSecond.ToString(),syncTimerSecondCount.ToString());
                }));
            }
            #endregion
        }
        public bool XMLCreator(DataTable dt, string PathFile)
        {
            #region Variable
            var result = false;
            var ds = new DataSet();
            #endregion
            #region Procedure
            try
            {
                if (!string.IsNullOrEmpty(PathFile))
                {
                    if (PathFile.Contains(".xml") || PathFile.Contains(".XML"))
                    {
                        ds.Tables.Add(dt);
                        ds.WriteXml(PathFile);
                        result = true;
                    }
                }
            }
            catch (Exception) { }
            #endregion
            return result;
        }
        private void btSyncPathMobile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(lblSyncPathMobile.Text);
        }
        private void btSyncPathMain_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(lblSyncPathMain.Text);
        }
    }
}
