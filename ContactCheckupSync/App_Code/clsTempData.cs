using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;

/// <summary>
/// Summary description for clsTempData
/// </summary>
public class clsTempData
{
	public clsTempData()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string getDropDownListValue(ComboBox ddlName, String columnName)
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
    public DataTable getPatient(DateTime DOEFrom,DateTime DOETo,string CompanyName)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.No,");
        strSQL.Append("P.rowguid PatientGUID,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.Episode,");
        strSQL.Append("P.Prename,");
        strSQL.Append("P.Name Forename,");
        strSQL.Append("P.LastName Surname,");
        strSQL.Append("P.LabEpisode,");
        strSQL.Append("P.DOB,");
        strSQL.Append("P.EMPID EmployeeID,");
        strSQL.Append("P.DOE,");
        strSQL.Append("P.Payor Company,");
        strSQL.Append("PL.ChildCompany,");
        strSQL.Append("P.ProChkList,");
        strSQL.Append("(SELECT TOP 1 CheckListDesc FROM Checklist WHERE CheckList = P.ProChkList ORDER BY ID DESC)ProChkListDetail,");
        strSQL.Append("P.Age,");
        strSQL.Append("P.Sex,");
        strSQL.Append("P.Address,");
        strSQL.Append("P.Tel,");
        strSQL.Append("P.Email,");
        strSQL.Append("P.Physician,P.RegType,P.Programid,P.DIV DIVI, P.DEP,P.SEC,P.POS,P.LAN,P.NAT,P.CNT_TRY,P.LOC,");
        strSQL.Append("P.Payor,P.Epi_Rowid,P.ORD_STS,P.STS,P.DR_CDE,P.NTE,P.Job,P.BusUnit,P.BusDiv,P.Line,P.Shift,P.Location,");
        strSQL.Append("P.GrpBook,P.HISExist,");
        strSQL.Append("P.SyncStatus,P.SyncWhen,PL.BookCreate,");
        strSQL.Append("(SELECT COUNT(RowID) FROM tblCheckList WITH(NOLOCK) WHERE PatientUID = P.rowguid) ChecklistCount ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("LEFT JOIN tblPatientList PL ON P.rowguid = PL.PatientUID ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '"+DOEFrom.ToString("yyyy-MM-dd HH:mm")+"' AND '"+DOETo.ToString("yyyy-MM-dd HH:mm")+"') ");
        strSQL.Append("AND Payor = '"+CompanyName+"' ");
        strSQL.Append("ORDER BY P.NO;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #endregion
        return dt;
    }
    public DataTable getPatientMobile_OLD(DateTime DOEFrom, DateTime DOETo, string CompanyName)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("CONCAT(P.Forename,' ',P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("P.Payor,");
        //strSQL.Append("P.BookCreate,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND WFID=1 AND ProStatus=3 LIMIT 0,1) DateRegis,");
        strSQL.Append("ProChkListDetail ProgramDetail,");
        strSQL.Append("(SELECT Count(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID) CountChecklistAll,");
        strSQL.Append("(SELECT Count(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus=3) CountChecklistComplete,");
        strSQL.Append("(SELECT Count(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus=4) CountChecklistCancel,");
        strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus<>3) ProgramPending,");
        strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus=4) ProgramCancel ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("AND Company = '" + CompanyName + "';");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #endregion
        return dt;
    }
    public DataTable getPatientMobile(DateTime DOEFrom, DateTime DOETo, string CompanyName)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.PatientGUID,");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("CONCAT(P.Forename,' ',P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("P.Payor,");
        strSQL.Append("P.BookCreate,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND WFID=1 AND ProStatus=3 LIMIT 0,1) DateRegis,");
        strSQL.Append("ProChkListDetail ProgramDetail,");
        strSQL.Append("COUNT(PC.RowID) CountChecklistAll,");
        strSQL.Append("SUM(ProStatus>=3) CountChecklistComplete,");
        strSQL.Append("SUM(ProStatus=4) CountChecklistCancel,");
        //strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus<>3) ProgramPending,");
        //strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus=4) ProgramCancel ");
        strSQL.Append("'' ProgramPending,");
        strSQL.Append("'' ProgramCancel,");
        strSQL.Append("P.SyncStatus,P.SyncWhen ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("INNER JOIN patientchecklist PC ON P.PatientGUID=PC.PatientGUID ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        if (CompanyName != "")
        {
            strSQL.Append("AND Company = '" + CompanyName + "' ");
        }
        strSQL.Append("GROUP BY P.PatientGUID ");
        strSQL.Append("ORDER BY P.Payor,P.No;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        if(dt!=null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["CountChecklistAll"].ToString().Trim() != dt.Rows[i]["CountChecklistComplete"].ToString())
                {
                    dt.Rows[i]["ProgramPending"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus<>3");
                }
                if (dt.Rows[i]["CountChecklistCancel"].ToString().Trim() != "0")
                {
                    //dt.Rows[i]["ProgramCancel"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus=4");
                    dt.Rows[i]["ProgramCancel"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(ProStatusRemark SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus=4");
                }
            }
            dt.AcceptChanges();
        }
        #endregion
        return dt;
    }
    public DataTable getPatientMobileByAll(DateTime DOEFrom, DateTime DOETo)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.PatientGUID,");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("CONCAT(P.Forename,' ',P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("P.Payor,");
        strSQL.Append("P.BookCreate,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND WFID=1 AND ProStatus=3 LIMIT 0,1) DateRegis,");
        strSQL.Append("ProChkListDetail ProgramDetail,");
        strSQL.Append("COUNT(PC.RowID) CountChecklistAll,");
        strSQL.Append("SUM(ProStatus>=3) CountChecklistComplete,");
        strSQL.Append("SUM(ProStatus=4) CountChecklistCancel,");
        //strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus<>3) ProgramPending,");
        //strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus=4) ProgramCancel ");
        strSQL.Append("'' ProgramPending,");
        strSQL.Append("'' ProgramCancel,");
        strSQL.Append("P.SyncStatus,P.SyncWhen ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("INNER JOIN patientchecklist PC ON P.PatientGUID=PC.PatientGUID ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("GROUP BY P.PatientGUID ");
        strSQL.Append("ORDER BY P.Payor,P.No;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["CountChecklistAll"].ToString().Trim() != dt.Rows[i]["CountChecklistComplete"].ToString())
                {
                    dt.Rows[i]["ProgramPending"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus<>3");
                }
                if (dt.Rows[i]["CountChecklistCancel"].ToString().Trim() != "0")
                {
                    //dt.Rows[i]["ProgramCancel"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus=4");
                    dt.Rows[i]["ProgramCancel"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(ProStatusRemark SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus=4");
                }
            }
            dt.AcceptChanges();
        }
        #endregion
        return dt;
    }
    public DataTable getPatientMobileByBookCreate(DateTime DOEFrom, DateTime DOETo, string BookCreate)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.PatientGUID,");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("CONCAT(P.Forename,' ',P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("P.Payor,");
        strSQL.Append("P.BookCreate,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND WFID=1 AND ProStatus=3 LIMIT 0,1) DateRegis,");
        strSQL.Append("ProChkListDetail ProgramDetail,");
        strSQL.Append("COUNT(PC.RowID) CountChecklistAll,");
        strSQL.Append("SUM(ProStatus>=3) CountChecklistComplete,");
        strSQL.Append("SUM(ProStatus=4) CountChecklistCancel,");
        //strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus<>3) ProgramPending,");
        //strSQL.Append("(SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND ProStatus=4) ProgramCancel ");
        strSQL.Append("'' ProgramPending,");
        strSQL.Append("'' ProgramCancel,");
        strSQL.Append("P.SyncStatus,P.SyncWhen ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("INNER JOIN patientchecklist PC ON P.PatientGUID=PC.PatientGUID ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        if (BookCreate != "")
        {
            strSQL.Append("AND BookCreate = '" + BookCreate + "' ");
        }
        strSQL.Append("GROUP BY P.PatientGUID ");
        strSQL.Append("ORDER BY P.BookCreate,P.No;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["CountChecklistAll"].ToString().Trim() != dt.Rows[i]["CountChecklistComplete"].ToString())
                {
                    dt.Rows[i]["ProgramPending"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus<>3");
                }
                if (dt.Rows[i]["CountChecklistCancel"].ToString().Trim() != "0")
                {
                    //dt.Rows[i]["ProgramCancel"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(WorkFlow SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus=4");
                    dt.Rows[i]["ProgramCancel"] = clsSQL.Return("SELECT CONVERT(GROUP_CONCAT(ProStatusRemark SEPARATOR ',') USING 'UTF8') FROM patientchecklist WHERE PatientGUID='" + dt.Rows[i]["PatientGUID"].ToString() + "' AND ProStatus=4");
                }
            }
            dt.AcceptChanges();
        }
        #endregion
        return dt;
    }
    public DataTable getCompany(DateTime DOEFrom,DateTime DOETo)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("DISTINCT P.Payor Company ");
        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("ORDER BY P.Payor;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #endregion
        return dt;
    }
    public DataTable getCompanyMobile(DateTime DOEFrom, DateTime DOETo)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("DISTINCT P.Company ");
        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("ORDER BY P.Company;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #endregion
        return dt;
    }
    public DataTable getBookCreateMobile(DateTime DOEFrom, DateTime DOETo)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("DISTINCT P.BookCreate ");
        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("ORDER BY P.BookCreate;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #endregion
        return dt;
    }
    public DataTable getPatientChecklist(string PatientGUID,string Episode,string HN)
    {
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("RowID,");
        strSQL.Append("PatientUID PatientGUID,");
        strSQL.Append("'"+HN+"' HN,");
        strSQL.Append("EN Episode,");
        strSQL.Append("CheckListID,");
        strSQL.Append("CheckList ProChkList,");
        strSQL.Append("ProID,");
        strSQL.Append("WorkFlow,");
        strSQL.Append("WFID,");
        strSQL.Append("WFSequen,");
        strSQL.Append("ProStatus,");
        strSQL.Append("RegDate,");
        strSQL.Append("ModifyDate,");
        strSQL.Append("SyncWhen ");
        strSQL.Append("FROM tblCheckList ");
        strSQL.Append("WHERE ");
        strSQL.Append("(EN<>'' AND EN='"+Episode+"') OR (PatientUID<>'' AND PatientUID='"+PatientGUID+"') ");
        strSQL.Append("ORDER BY ModifyDate DESC, WFSequen ASC;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #region DuplicateClear
        if(dt!=null && dt.Rows.Count > 0)
        {
            dt = RemoveDuplicateRows(dt, "WFID");
        }
        #endregion
        #endregion
        //if(dt!=null && dt.Rows.Count > 0)
        //{

        //}
        //else
        //{
        //    MessageBox.Show("Test");
        //}
        return dt;
    }
    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
        //And add duplicate item value in arraylist.
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }

        //Removing a list of duplicate items from datatable.
        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);

        //Datatable which contains unique records will be return as output.
        return dTable;
    }
    public DataTable getLabSummary()
    {
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.Company,");
        strSQL.Append("DATE(PL.CWhen) DateAccept,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientlab ");
        //strSQL.Append("INNER JOIN patient ON patientlab.LabEpisode = patient.LabEpisode ");
        //strSQL.Append("WHERE DATE(PL.CWhen) = DATE(patientlab.CWhen) AND Company = P.Company AND WFID = 6");
        //strSQL.Append(") CountBloodComplete,");
        strSQL.Append("SUM(PL.WFID = 6)CountBloodComplete,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientchecklist ");
        //strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
        //strSQL.Append("WHERE Company = P.Company AND WFID = 6");
        //strSQL.Append(") CountBloodAll,");
        strSQL.Append("0 CountBloodAll,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientlab ");
        //strSQL.Append("INNER JOIN patient ON patientlab.LabEpisode = patient.LabEpisode ");
        //strSQL.Append("WHERE DATE(PL.CWhen) = DATE(patientlab.CWhen) AND Company = P.Company AND WFID = 7");
        //strSQL.Append(") CountUrineComplete,");
        strSQL.Append("SUM(PL.WFID=7) CountUrineComplete,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientchecklist ");
        //strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
        //strSQL.Append("WHERE Company = P.Company AND WFID = 7");
        //strSQL.Append(") CountUrineAll,");
        strSQL.Append("0 CountUrineAll,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientlab ");
        //strSQL.Append("INNER JOIN patient ON patientlab.LabEpisode = patient.LabEpisode ");
        //strSQL.Append("WHERE DATE(PL.CWhen) = DATE(patientlab.CWhen) AND Company = P.Company AND WFID = 8");
        //strSQL.Append(") CountStoolComplete,");
        strSQL.Append("SUM(PL.WFID=8) CountStoolComplete,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientchecklist ");
        //strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
        //strSQL.Append("WHERE Company = P.Company AND WFID = 8");
        //strSQL.Append(") CountStoolAll,");
        strSQL.Append("0 CountStoolAll,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientlab ");
        //strSQL.Append("INNER JOIN patient ON patientlab.LabEpisode = patient.LabEpisode ");
        //strSQL.Append("WHERE DATE(PL.CWhen) = DATE(patientlab.CWhen) AND Company = P.Company AND WFID = 9");
        //strSQL.Append(") CountHeavyMetalComplete,");
        strSQL.Append("SUM(PL.WFID=9) CountHeavyMetalComplete,");
        //strSQL.Append("(");
        //strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
        //strSQL.Append("FROM patientchecklist ");
        //strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
        //strSQL.Append("WHERE Company = P.Company AND WFID = 9");
        //strSQL.Append(") CountHeavyMetalAll ");
        strSQL.Append("0 CountHeavyMetalAll ");
        strSQL.Append("FROM ");
        strSQL.Append("patient P ");
        strSQL.Append("INNER JOIN patientlab PL ON P.LabEpisode = PL.LabEpisode AND PL.StatusFlag = 'A' ");
        strSQL.Append("GROUP BY P.Company,DATE(PL.CWhen);");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        if(dt!=null && dt.Rows.Count > 0)
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                strSQL.Length = 0;strSQL.Capacity = 0;
                strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
                strSQL.Append("FROM patientchecklist ");
                strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
                strSQL.Append("WHERE Company = '"+dt.Rows[i]["Company"].ToString()+"' AND WFID = 6");
                dt.Rows[i]["CountBloodAll"] = clsSQL.Return(strSQL.ToString());

                strSQL.Length = 0; strSQL.Capacity = 0;
                strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
                strSQL.Append("FROM patientchecklist ");
                strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
                strSQL.Append("WHERE Company = '" + dt.Rows[i]["Company"].ToString() + "' AND WFID = 7");
                dt.Rows[i]["CountUrineAll"] = clsSQL.Return(strSQL.ToString());

                strSQL.Length = 0; strSQL.Capacity = 0;
                strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
                strSQL.Append("FROM patientchecklist ");
                strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
                strSQL.Append("WHERE Company = '" + dt.Rows[i]["Company"].ToString() + "' AND WFID = 8");
                dt.Rows[i]["CountStoolAll"] = clsSQL.Return(strSQL.ToString());

                strSQL.Length = 0; strSQL.Capacity = 0;
                strSQL.Append("SELECT COUNT(patient.LabEpisode) ");
                strSQL.Append("FROM patientchecklist ");
                strSQL.Append("INNER JOIN patient ON patientchecklist.PatientGUID = patient.PatientGUID ");
                strSQL.Append("WHERE Company = '" + dt.Rows[i]["Company"].ToString() + "' AND WFID = 9");
                dt.Rows[i]["CountHeavyMetalAll"] = clsSQL.Return(strSQL.ToString());
            }
            dt.AcceptChanges();
        }
        #endregion
        return dt;
    }
    public DataTable getLabDetail(DateTime DOEFrom, DateTime DOETo, string CompanyName)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        /*
        strSQL.Append("SELECT ");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("P.LabEpisode,");
        strSQL.Append("CONCAT(P.Forename,' ',P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=6 LIMIT 0,1) AcceptDateBlood,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=7 LIMIT 0,1) AcceptDateUrine,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=8 LIMIT 0,1) AcceptDateStool,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=9 LIMIT 0,1) AcceptDateHeavyMetal,");
        strSQL.Append("((SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND (WFID=6 OR WFID=7 OR WFID=8 OR WFID=9))-(SELECT COUNT(LabEpisode) FROM patientlab WHERE LabEpisode=P.LabEpisode AND (WFID=6 OR WFID=7 OR WFID=8 OR WFID=9))) CountLabPending,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND WFID=1) RegisterDate,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID AND WFID = 6) CountChecklistBlood,");
strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID AND WFID = 7) CountChecklistUrine,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID AND WFID = 8) CountChecklistStool,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID AND WFID = 9) CountChecklistHeavyMetal ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("AND Company = '" + CompanyName + "';");
        */
        strSQL.Append("SELECT ");
        strSQL.Append("P.PatientGUID,P.No OrderNo, P.HN,P.EmployeeID,P.LabEpisode,CONCAT(P.Forename, ' ', P.Surname) Name,P.POS Position, P.DEP Department, P.DIVI Division, P.SEC Section, P.Line,P.Shift,P.Location Site,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode = P.LabEpisode AND WFID = 6 LIMIT 0, 1) AcceptDateBlood,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode = P.LabEpisode AND WFID = 7 LIMIT 0,1) AcceptDateUrine,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode = P.LabEpisode AND WFID = 8 LIMIT 0,1) AcceptDateStool,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode = P.LabEpisode AND WFID = 9 LIMIT 0,1) AcceptDateHeavyMetal,");
        strSQL.Append("(SUM(WFID = 6 OR WFID = 7 OR WFID = 8 OR WFID = 9) - (SELECT COUNT(LabEpisode) FROM patientlab WHERE LabEpisode = P.LabEpisode AND WFID IN(6, 7, 8, 9))) CountLabPending,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID = P.PatientGUID AND WFID = 1 LIMIT 0,1) RegisterDate,");
        strSQL.Append("SUM(WFID = 6) CountChecklistBlood,");
        strSQL.Append("SUM(WFID = 7) CountChecklistUrine,");
        strSQL.Append("SUM(WFID = 8) CountChecklistStool,");
        strSQL.Append("SUM(WFID = 9) CountChecklistHeavyMetal ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("INNER JOIN patientchecklist PL ON P.PatientGUID = PL.PatientGUID ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("AND Company = '" + CompanyName + "' ");
        strSQL.Append("GROUP BY P.PatientGUID;");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        if(dt!=null && dt.Rows.Count > 0)
        {
            dt.Columns.Remove("PatientGUID");
            dt.AcceptChanges();
        }
        #endregion
        return dt;
    }
    public DataTable getLabDetail(DateTime DOEFrom, DateTime DOETo)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        /*
        strSQL.Append("SELECT ");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("P.LabEpisode,");
        strSQL.Append("CONCAT(P.Forename,' ',P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("P.Payor,");
        //strSQL.Append("P.BookCreate,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=6 LIMIT 0,1) Blood,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=7 LIMIT 0,1) Urine,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=8 LIMIT 0,1) Stool,");
        strSQL.Append("(SELECT CWhen FROM patientlab WHERE LabEpisode=P.LabEpisode AND WFID=9 LIMIT 0,1) HeavyMetal,");
        strSQL.Append("(SELECT MWhen FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND WFID=1 AND ProStatus>=2) RegisterDate ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "');");
        */
        strSQL.Append("SELECT ");
        strSQL.Append("P.No OrderNo,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.EmployeeID,");
        strSQL.Append("P.LabEpisode,");
        strSQL.Append("CONCAT(P.Forename, ' ', P.Surname) Name,");
        strSQL.Append("P.POS Position,");
        strSQL.Append("P.DEP Department,");
        strSQL.Append("P.DIVI Division,");
        strSQL.Append("P.SEC Section,");
        strSQL.Append("P.Line,");
        strSQL.Append("P.Shift,");
        strSQL.Append("P.Location Site,");
        strSQL.Append("P.Payor,");
        strSQL.Append("PLBlood.CWhen Blood,");
        strSQL.Append("PLUrine.CWhen Urine,");
        strSQL.Append("PLStool.CWhen Stool,");
        strSQL.Append("PLHeavyMetal.CWhen HeavyMetal,");
        strSQL.Append("PC.MWhen RegisterDate ");
        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("LEFT JOIN PatientChecklist PC ON P.PatientGUID = PC.PatientGUID AND PC.WFID = 1 AND PC.ProStatus >= 2 ");
        strSQL.Append("LEFT JOIN PatientLab PLBlood ON P.LabEpisode = PLBlood.LabEpisode AND PLBlood.StatusFlag = 'A' AND PLBlood.WFID = 6 ");
        strSQL.Append("LEFT JOIN PatientLab PLUrine ON P.LabEpisode = PLUrine.LabEpisode AND PLUrine.StatusFlag = 'A' AND PLUrine.WFID = 7 ");
        strSQL.Append("LEFT JOIN PatientLab PLStool ON P.LabEpisode = PLStool.LabEpisode AND PLStool.StatusFlag = 'A' AND PLStool.WFID = 8 ");
        strSQL.Append("LEFT JOIN PatientLab PLHeavyMetal ON P.LabEpisode = PLHeavyMetal.LabEpisode AND PLHeavyMetal.StatusFlag = 'A' AND PLHeavyMetal.WFID = 9 ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE >= '" + DOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND P.DOE <= '" + DOETo.ToString("yyyy-MM-dd HH:mm") + "')");


        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        #endregion
        return dt;
    }
    public string getPatientNotHadChecklist()
    {
        #region Variable
        var result = new StringBuilder();
        var strSQL = new StringBuilder();
        var dt = new DataTable();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.HN,P.Episode,P.Forename,P.Surname,P.ProChkList,P.Company ");
        strSQL.Append("FROM patient P ");
        //strSQL.Append("WHERE(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID) = 0;");
        strSQL.Append("WHERE NOT P.PatientGUID IN (SELECT DISTINCT PatientGUID FROM patientchecklist);");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
        if(dt!=null && dt.Rows.Count > 0)
        {
            result.Append(Environment.NewLine + Environment.NewLine);
            result.Append("## รายชื่อที่ไม่มี Checklist ##");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Append(Environment.NewLine);
                result.Append("HN : "+dt.Rows[i]["HN"].ToString()+" ");
                result.Append("EN : "+dt.Rows[i]["Episode"].ToString()+" ");
                result.Append("Name : "+dt.Rows[i]["Forename"].ToString()+" "+ dt.Rows[i]["Surname"].ToString()+" ");
                result.Append("Company : " + dt.Rows[i]["Company"].ToString() + " ");
            }
        }
        else
        {
            result.Append(Environment.NewLine + Environment.NewLine);
            result.Append("## รายชื่อที่ไม่มี Checklist ##");
            result.Append(Environment.NewLine);
            result.Append("- ไม่พบข้อมูล -");
        }
        #endregion
        return result.ToString();
    }
    public string getPatientCountAll(DateTime dtDOEFrom,DateTime dtDOETo,string Company)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var result = "";
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        strSQL.Append("SELECT COUNT(PatientGUID) FROM patient WHERE (DOE BETWEEN '" + dtDOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + dtDOETo.ToString("yyyy-MM-dd HH:mm") + "') AND Company='"+Company+"'");
        result = clsSQL.Return(strSQL.ToString());
        #endregion
        return result;
    }
    public string getPatientCountPending(DateTime dtDOEFrom, DateTime dtDOETo, string Company)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var result = "";
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        strSQL.Append("SELECT ");
        strSQL.Append("COUNT(P.PatientGUID)CountNotRegister ");
        strSQL.Append("FROM ");
        strSQL.Append("patient P ");
        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + dtDOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + dtDOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("AND P.Company = '" + Company + "' ");
        strSQL.Append("AND(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID AND patientchecklist.RegDate IS NULL) > 0;");
        result = clsSQL.Return(strSQL.ToString());
        #endregion
        return result;
    }
    public DataTable getPatientChecklistCountByProStatus(DateTime dtDOEFrom, DateTime dtDOETo, string Company,string RegisterDate)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var result = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("P.HN,");
        strSQL.Append("P.Forename,");
        strSQL.Append("P.Surname,");
        strSQL.Append("P.PatientGUID,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND DATE(patientchecklist.RegDate)='"+ RegisterDate + "')CountChecklistAll,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND DATE(patientchecklist.RegDate)='" + RegisterDate + "' AND ProStatus=3)CountChecklistComplete,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND DATE(patientchecklist.RegDate)='" + RegisterDate + "' AND ProStatus=2)CountChecklistDocumentPending,");
        strSQL.Append("(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID=P.PatientGUID AND DATE(patientchecklist.RegDate)='" + RegisterDate + "' AND ProStatus=4)CountChecklistCancel ");
        strSQL.Append("FROM patient P ");
        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '" + dtDOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '" + dtDOETo.ToString("yyyy-MM-dd HH:mm") + "') ");
        strSQL.Append("AND P.Company = '"+Company+"';");
        #endregion
        result = clsSQL.Bind(strSQL.ToString());
        #endregion
        return result;
    }
    public DataTable getPatientChecklistGroupByRegisterDate(DateTime dtDOEFrom, DateTime dtDOETo, string Company)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        #region Variable
        var result = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("DATE(RegDate)RegisterDate ");
        strSQL.Append("FROM patientchecklist PL ");
        strSQL.Append("INNER JOIN patient P ON PL.PatientGUID = P.PatientGUID ");
        strSQL.Append("WHERE PL.WFID = 1 AND NOT PL.RegDate IS NULL ");
        strSQL.Append("AND(P.DOE BETWEEN '"+ dtDOEFrom.ToString("yyyy-MM-dd HH:mm") + "' AND '"+dtDOETo.ToString("yyyy-MM-dd HH:mm")+"') ");
        strSQL.Append("AND P.Company = '"+Company+"' ");
        strSQL.Append("GROUP BY DATE(RegDate) ");
        strSQL.Append("ORDER BY DATE(RegDate);");
        #endregion
        result = clsSQL.Bind(strSQL.ToString());
        #endregion
        return result;
    }
    /// <summary>
    /// ดึงข้อมูล PatientChecklist ทั้งหมดในระบบ Mobile ที่มี ProStatus>1 เพื่อนำไปอัพเดทข้อมูลที่ Main Server
    /// </summary>
    /// <returns></returns>
    public DataTable getPatientChecklistMobile()
    {
        #region Variable
        //var hourAddSync = System.Configuration.ConfigurationManager.AppSettings["hourAddSync"];
        var hourAddSync = clsGlobal.AddHours;
        var result = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("RowID,PatientGUID,HN,WorkFlow,WFID,ProStatus,IFNULL(ProStatusRemark,'')ProStatusRemark,RegDate,ModifyDate,MWhen,MUser ");
        strSQL.Append("FROM ");
        strSQL.Append("patientchecklist ");
        strSQL.Append("WHERE ");
        strSQL.Append("ProStatus>1 ");
        strSQL.Append("AND MWhen>=DATE_ADD(NOW(),INTERVAL "+ hourAddSync + " HOUR);");
        #endregion
        result = clsSQL.Bind(strSQL.ToString());
        #endregion
        return result;
    }
    public DataTable getPatientChecklistMain(string RowID)
    {
        #region Variable
        var result = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("SELECT ");
        strSQL.Append("RowID,PatientUID,WFID,ProStatus,ProStatusRemark,RegDate,ModifyDate ");
        strSQL.Append("FROM ");
        strSQL.Append("tblCheckList ");
        strSQL.Append("WHERE ");
        strSQL.Append("RowID="+RowID.ToString()+";");
        #endregion
        result = clsSQL.Bind(strSQL.ToString());
        #endregion
        return result;
    }
    public DataTable getReportType()
    {
        var dt = new DataTable();

        dt.Columns.Add("UID", typeof(int));
        dt.Columns.Add("Name", typeof(string));

        dt.Rows.Add(1, "All");
        dt.Rows.Add(2, "Payor");
        dt.Rows.Add(3, "Book");

        dt.AcceptChanges();
        return dt;
    }
}
