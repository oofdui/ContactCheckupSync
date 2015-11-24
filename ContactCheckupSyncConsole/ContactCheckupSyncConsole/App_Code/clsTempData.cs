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
    public static DataTable getPatientChecklistMain(string RowID)
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
        strSQL.Append("RowID="+RowID.ToString()+";");
        #endregion
        result = clsSQL.Bind(strSQL.ToString());
        #endregion
        return result;
    }
}
