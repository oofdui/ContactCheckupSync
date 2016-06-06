using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ContactCheckupSyncConsole
{
    class Program
    {
        #region GlobalVariable
        static string pathSync = System.Configuration.ConfigurationManager.AppSettings["pathSync"];
        #endregion
        static void Main(string[] args)
        {
            setUsageLog();
            try
            {
                #region ListAllFile
                var files = Directory.GetFiles(pathSync,"*.xml");
                if (files.Length > 0)
                {
                    for(int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            Sync(files[i]);
                        }
                        catch(Exception exSync)
                        {
                            Console.WriteLine("Error on Sync : " + exSync.Message);
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                }
                #endregion
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error on ListAllFile : " + ex.Message);
                System.Threading.Thread.Sleep(5000);
                return;
                wsDefault.ServiceSoapClient wsDefault = new wsDefault.ServiceSoapClient();
                if (wsDefault.MailSend(
                    System.Configuration.ConfigurationManager.AppSettings["mailTo"],
                    System.Configuration.ConfigurationManager.AppSettings["site"] + " : " + clsGlobal.ApplicationName + " Console Sync",
                    "<h1>" + System.Configuration.ConfigurationManager.AppSettings["site"] + " : " + clsGlobal.ApplicationName + " Console Sync" + "</h1><h3><span style='color:#red;'>Error</span></h3><hr/>" + ex.Message,
                    "AutoSystem@glsict.com",
                    System.Configuration.ConfigurationManager.AppSettings["site"] + " : " + clsGlobal.ApplicationName,
                    "", "", "<b>ServerIP</b> : " + clsGlobal.IPAddress() + "<br/><b>ExecutePath</b> : " + clsGlobal.ExecutePathBuilder() + "<br/><b>Application Version</b> : "+clsGlobal.ApplicationVersion(), false))
                {
                    Console.WriteLine(string.Format("MailSend : {0}", "Success"));
                }
                else
                {
                    Console.WriteLine(string.Format("MailSend : {0}", "Fail"));
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }
        static private void Sync(string fileFullName)
        {
            #region Variable
            var fi = new FileInfo(fileFullName);
            var dt = new DataTable();
            var dtMain = new DataTable();
            var tblPatientStatusOnMobile = "";
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            var countSuccess = 0;
            var countFail = 0;
            var countSuccessMobileStatus = 0;
            var mailMessage = new StringBuilder();
            var outSQL = "";
            var outMessage = "";
            #endregion
            #region Procedure
            if (fi.Exists)
            {
                if (!IsFileLocked(fi))
                {
                    #region FileExist
                    Console.WriteLine(string.Format("Find file : {0} ({1})", "Found", fi.FullName));
                    Console.WriteLine(string.Format("Read file : {0}", "Processing..."));
                    dt = XMLSelecter(fi.FullName);
                    Console.WriteLine(string.Format("Read file : {0}", "Completed"));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine(string.Format("Read DataTable : {0} ({1} Rows)", "Found", dt.Rows.Count.ToString()));
                        #region UpdateToDatabase
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["PatientGUID"].ToString().Trim().ToLower().Contains("new-regis"))
                            {
                                continue;
                            }
                            #region Update tblPatientListSTS & tblPatientStatusOnMobile
                            if (dt.Rows[i]["WFID"].ToString().Trim() == "1" && float.Parse(dt.Rows[i]["ProStatus"].ToString().Trim()) >= 2)
                            {
                                tblPatientStatusOnMobile = clsSQL.Return("SELECT StatusOnMobile FROM Patient WHERE rowguid='" + dt.Rows[i]["PatientGUID"].ToString().Trim() + "';");
                                if (tblPatientStatusOnMobile != "R")
                                {
                                    if (clsSQL.Execute(
                                        "UPDATE tblPatientList SET STS='R',SyncWhen=GETDATE() WHERE PatientUID='" + dt.Rows[i]["PatientGUID"].ToString().Trim() + "';" +
                                        "UPDATE Patient SET SyncStatus='1',SyncWhen=GETDATE(),StatusOnMobile='R' WHERE rowguid='" + dt.Rows[i]["PatientGUID"].ToString().Trim() + "';", out outMessage))
                                    {
                                        countSuccessMobileStatus += 1;
                                        Console.WriteLine(string.Format("{0} : Update MobileStatus : {1} ({2})", dt.Rows[i]["HN"].ToString(), "Success", dt.Rows[i]["HN"].ToString().Trim()));
                                        mailMessage.Append(string.Format("{0} : Update MobileStatus : {1} ({2})<br/>", dt.Rows[i]["HN"].ToString(), "<span style='color:green;'>Success</span>", dt.Rows[i]["HN"].ToString().Trim()));
                                    }
                                    else
                                    {
                                        countFail += 1;
                                        Console.WriteLine(string.Format("{0} : Update MobileStatus : {1} ({2}) : {3}", dt.Rows[i]["HN"].ToString(), "Fail", dt.Rows[i]["HN"].ToString().Trim(), outMessage));
                                        mailMessage.Append(string.Format("{0} : Update MobileStatus : {1} ({2} : {3})<br/>", dt.Rows[i]["HN"].ToString(), "<span style='color:red;'>Fail</span>", dt.Rows[i]["HN"].ToString().Trim(), outMessage));
                                    }
                                }
                            }
                            #endregion
                            #region ChecklistUpdate
                            dtMain = getPatientChecklistMain(dt.Rows[i]["RowID"].ToString());
                            if (dtMain != null && dtMain.Rows.Count > 0)
                            {
                                if (dt.Rows[i]["ProStatus"].ToString().Trim() != dtMain.Rows[0]["ProStatus"].ToString().Trim() ||
                                    dt.Rows[i]["ProStatusRemark"].ToString().Trim() != dtMain.Rows[0]["ProStatusRemark"].ToString().Trim()/*||
                            dtMobile.Rows[i]["RegDate"].ToString().Trim() != dtMain.Rows[0]["RegDate"].ToString().Trim() ||
                            dtMobile.Rows[i]["ModifyDate"].ToString().Trim() != dtMain.Rows[0]["ModifyDate"].ToString().Trim()*/)
                                {
                                    #region UpdateChecklist
                                    if (!clsSQL.Update(
                                        "tblCheckList",
                                        new string[,]
                                        {
                                    {"ProStatus",dt.Rows[i]["ProStatus"].ToString().Trim() },
                                    {"ProStatusRemark","'"+dt.Rows[i]["ProStatusRemark"].ToString().SQLQueryFilter()+"'" },
                                    {"RegDate",(dt.Rows[i]["RegDate"].ToString()!=""?"'"+DateTime.Parse(dt.Rows[i]["RegDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")+"'":"NULL") },
                                    {"SyncWhen","GETDATE()"}
                                        },
                                        new string[,] { { } },
                                        "RowID=" + dt.Rows[i]["RowID"].ToString(), out outSQL, out outMessage, true))
                                    {
                                        countFail += 1;
                                        Console.WriteLine(string.Format("{0} : Update tblChecklist : {1} ({2})", dt.Rows[i]["HN"].ToString(), "Fail", outMessage));
                                        mailMessage.Append(string.Format("{0} : Update tblChecklist : {1} ({2})<br/>", dt.Rows[i]["HN"].ToString(), "<span style='color:red;'>Fail</span>", outMessage));
                                    }
                                    else
                                    {
                                        #region LogUpdate
                                        countSuccess += 1;
                                        Console.WriteLine(string.Format("{0} : Update tblChecklist : {1} ({2})", dt.Rows[i]["HN"].ToString(), "Success", dtMain.Rows[0]["ProStatus"].ToString().Trim() + "->" + dt.Rows[i]["ProStatus"].ToString().Trim()));
                                        mailMessage.Append(string.Format("{0} : Update tblChecklist : {1} ({2})<br/>", dt.Rows[i]["HN"].ToString(), "<span style='color:green;'>Success</span>", dtMain.Rows[0]["ProStatus"].ToString().Trim() + "->" + dt.Rows[i]["ProStatus"].ToString().Trim()));
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                mailMessage.Append("ไม่พบข้อมูลในไฟล์");
                            }
                            #endregion
                        }
                        #endregion
                        Console.WriteLine(string.Format("Summary : Success {0} Fail {1}", countSuccess.ToString(), countFail.ToString()));
                        Console.WriteLine(string.Format("MailSend : {0}", "Processing..."));
                        try
                        {
                            if (countSuccessMobileStatus > 0 || countSuccess > 0 || countFail > 0)
                            {
                                wsDefault.ServiceSoapClient wsDefault = new wsDefault.ServiceSoapClient();
                                if (wsDefault.MailSend(
                                    System.Configuration.ConfigurationManager.AppSettings["mailTo"],
                                    System.Configuration.ConfigurationManager.AppSettings["site"] + " : " + clsGlobal.ApplicationName + " Console Sync",
                                    "<h1>" + System.Configuration.ConfigurationManager.AppSettings["site"] + " : " + clsGlobal.ApplicationName + " Console Sync" + "</h1><h3><span style='color:#238DBB;'>StatusUpdateSuccess : " + countSuccessMobileStatus.ToString() + "</span> , <span style='color:green;'>Success : " + countSuccess.ToString() + "</span> , <span style='color:red;'>Fail : " + countFail.ToString() + "</span></h3><h4>FileName : " + fileFullName + "</h4><hr/>" + mailMessage.ToString(),
                                    "AutoSystem@glsict.com",
                                    System.Configuration.ConfigurationManager.AppSettings["site"] + " : " + clsGlobal.ApplicationName,
                                    "", "", "<b>ServerIP</b> : " + clsGlobal.IPAddress() + "<br/><b>ExecutePath</b> : " + clsGlobal.ExecutePathBuilder() + "<br/><b>Application Version</b> : " + clsGlobal.ApplicationVersion(), false))
                                {
                                    Console.WriteLine(string.Format("MailSend : {0}", "Success"));
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("MailSend : {0}", "Fail"));
                                }
                            }
                        }
                        catch (Exception exMail)
                        {
                            Console.WriteLine(string.Format("MailSend : {0}", "Fail : " + exMail.Message));
                        }
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Read DataTable : {0}", "No Data"));
                    }
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception exDelete)
                    {
                        Console.WriteLine(string.Format("Delete file : {0} ({1})", "Fail", exDelete.Message));
                    }
                    #endregion
                }
                else
                {
                    Console.WriteLine(string.Format("Read File : {0} ({1})", "Fail", "FileIsLocked"));
                }
            }
            else
            {
                //ไม่เจอไฟล์ก็จบไป
            }
            #endregion
        }
        static protected bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        static private void setUsageLog()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["enableUsageLog"].Trim().ToLower() == "true")
            {
                try
                {
                    wsCenter.ServiceSoapClient wsCenter = new wsCenter.ServiceSoapClient();
                    wsCenter.InsertLogApplicationBySite(
                        clsGlobal.ApplicationName,
                        "Console : " + clsGlobal.ApplicationVersion(),
                        System.Configuration.ConfigurationManager.AppSettings["site"],
                        clsGlobal.WindowsLogon(),
                        clsGlobal.IPAddress(),
                        clsGlobal.ComputerName());
                }
                catch (Exception ex)
                {

                }
            }
        }
        static DataTable XMLSelecter(string PathFile)
        {
            #region Variable
            var result = new DataTable();
            var ds = new DataSet();
            #endregion
            #region Procedure
            try
            {
                if (!string.IsNullOrEmpty(PathFile))
                {
                    if (PathFile.Contains(".xml") || PathFile.Contains(".XML"))
                    {
                        if (new FileInfo(PathFile).Exists)
                        {
                            ds.ReadXml(PathFile);
                            result = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception) { }
            #endregion
            return result;
        }
        static DataTable getPatientChecklistMain(string RowID)
        {
            #region Variable
            var result = new DataTable();
            var strSQL = new StringBuilder();
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            #endregion
            #region Procedure
            #region SQLQuery
            strSQL.Append("SELECT ");
            strSQL.Append("RowID,PatientUID,WFID,ProStatus,ProStatusRemark,RegDate,ModifyDate ");
            strSQL.Append("FROM ");
            strSQL.Append("tblCheckList ");
            strSQL.Append("WHERE ");
            strSQL.Append("RowID=" + RowID.ToString() + ";");
            #endregion
            result = clsSQL.Bind(strSQL.ToString());
            #endregion
            return result;
        }
    }
}
