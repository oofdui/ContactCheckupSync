using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;

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
        strSQL.Append("P.rowguid PatientGUID,");
        strSQL.Append("P.HN,");
        strSQL.Append("P.Episode,");
        strSQL.Append("P.Prename,");
        strSQL.Append("P.Name Forename,");
        strSQL.Append("P.LastName Surname,");
        strSQL.Append("P.LabEpisode,");
        strSQL.Append("P.DOB,");
        strSQL.Append("P.No,");
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
        strSQL.Append("P.GrpBook,P.HISExist ");

        strSQL.Append("FROM ");
        strSQL.Append("Patient P ");
        strSQL.Append("LEFT JOIN tblPatientList PL ON P.rowguid = PL.PatientUID ");

        strSQL.Append("WHERE ");
        strSQL.Append("(P.DOE BETWEEN '"+DOEFrom.ToString("yyyy-MM-dd HH:mm")+"' AND '"+DOETo.ToString("yyyy-MM-dd HH:mm")+"') ");
        strSQL.Append("AND Payor = '"+CompanyName+"';");
        #endregion
        dt = clsSQL.Bind(strSQL.ToString());
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
    public DataTable getPatientChecklist(string PatientGUID,string Episode,string HN)
    {
        #region Variable
        var dt = new DataTable();
        var strSQL = new StringBuilder();
        var clsSQL = new clsSQL(clsGlobal.dbTypeMain, clsGlobal.csMain);
        #endregion
        #region Procedure
        #region SQLQuery
        strSQL.Append("");
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
        strSQL.Append("ModifyDate ");
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
        strSQL.Append("WHERE(SELECT COUNT(RowID) FROM patientchecklist WHERE PatientGUID = P.PatientGUID) = 0;");
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
            result.Append("- ไม่พบข้อมูล -");
        }
        #endregion
        return result.ToString();
    }
}
