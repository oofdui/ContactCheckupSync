using OfficeOpenXml;
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
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }
        private void Report_Load(object sender, EventArgs e)
        {
            var clsSQL = new clsSQL(clsGlobal.dbType, clsGlobal.cs);
            if (!clsSQL.IsConnected())
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูล Mobile ได้", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtDOEFrom.Enabled = false;dtDOETo.Enabled = false;btSearch.Enabled = false;btExport.Enabled = false;ddlCompany.Enabled = false;
                return;
            }
            setType();
        }
        private void dtDOEFrom_ValueChanged(object sender, EventArgs e)
        {
            //setCompany();
            setDropDownList();
        }
        private void dtDOETo_ValueChanged(object sender, EventArgs e)
        {
            //setCompany();
            setDropDownList();
        }
        private void btSearch_Click(object sender, EventArgs e)
        {
            wbSearch.RunWorkerAsync();
        }
        private void btExport_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        private void btLabExport_Click(object sender, EventArgs e)
        {
            ExportLab();
        }
        private void setCompany()
        {
            #region Variable
            var dt = new DataTable();
            var clsTempData = new clsTempData();
            #endregion
            #region Procedure
            dt = clsTempData.getCompanyMobile(dtDOEFrom.Value, dtDOETo.Value);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.NewRow();
                dr[0] = "- ทั้งหมด -";
                dt.Rows.InsertAt(dr, 0);

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
        private void setBookCreate()
        {
            #region Variable
            var dt = new DataTable();
            var clsTempData = new clsTempData();
            #endregion
            #region Procedure
            dt = clsTempData.getBookCreateMobile(dtDOEFrom.Value, dtDOETo.Value);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.NewRow();
                dr[0] = "- ทั้งหมด -";
                dt.Rows.InsertAt(dr, 0);

                ddlCompany.DataSource = dt;
                ddlCompany.DisplayMember = "BookCreate";
                ddlCompany.ValueMember = "BookCreate";
            }
            else
            {
                ddlCompany.DataSource = null;
            }
            #endregion
        }
        private void Export()
        {
            try
            {
                if (pbDefault.InvokeRequired)
                {
                    pbDefault.Invoke(new MethodInvoker(delegate
                    {
                        pbDefault.Maximum = 5;
                        pbDefault.Value = 0;
                    }));
                }
                var clsTempData = new clsTempData();
                var reportType = "";
                #region ReportType
                var type = getDropDownListValue(ddlType, "Name");
                switch (type)
                {
                    case "All":
                        reportType = "ALL";
                        break;
                    case "Payor":
                        reportType = "PAYOR";
                        if(getDropDownListValue(ddlCompany, "Payor")!="- ทั้งหมด -")
                        {
                            reportType += " - " + getDropDownListValue(ddlCompany, "Payor");
                        }
                        break;
                    case "Book":
                        reportType = "BOOK";
                        if (getDropDownListValue(ddlCompany, "BookCreate") != "- ทั้งหมด -")
                        {
                            reportType += " - " + getDropDownListValue(ddlCompany, "BookCreate");
                        }
                        break;
                    default:
                        break;
                }
                #endregion
                var FileName = clsGlobal.ExecutePathBuilder() + @"Export\" + reportType + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".xlsx";
                var dt = new DataTable();
                dt = clsGlobal.dtPatient;
                FileInfo newFile = new FileInfo(FileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(FileName);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    var rows = 1;
                    #region Summary
                    /*
                    try
                    {
                        ExcelWorksheet worksheetSummary = package.Workbook.Worksheets.Add("Summary");
                        #region HeaderBuilder
                        var headers = new string[] { "", "จำนวน", "%" };
                        var iHeader = 0;
                        for (iHeader = 0; iHeader < headers.Length; iHeader++)
                        {
                            worksheetSummary.Cells[rows, iHeader + 1].Value = headers[iHeader];

                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Bold = true;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Name = "Tahoma";
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Size = 12;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Color.SetColor(Color.White);
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                            worksheetSummary.Cells[rows, iHeader + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        var dtRegisterDate = new DataTable();
                        dtRegisterDate = clsTempData.getPatientChecklistGroupByRegisterDate(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"));
                        if (dtRegisterDate != null && dtRegisterDate.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtRegisterDate.Rows.Count; i++)
                            {
                                iHeader += 1;
                                worksheetSummary.Cells[rows, iHeader].Value = DateTime.Parse(dtRegisterDate.Rows[i]["RegisterDate"].ToString()).ToString("dd/MM/yyyy");

                                worksheetSummary.Cells[rows, iHeader].Style.Font.Bold = true;
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma";
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 12;
                                worksheetSummary.Cells[rows, iHeader].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Color.SetColor(Color.White);
                                worksheetSummary.Cells[rows, iHeader].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                worksheetSummary.Cells[rows, iHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }
                        #endregion
                        #region RowsBuilder
                        rows += 1;
                        #region Summary
                        iHeader = 1;
                        worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheetSummary.Cells[rows, iHeader].Value = "จำนวนพนักงานทั้งหมด"; iHeader += 1;
                        worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheetSummary.Cells[rows, iHeader].Value = clsTempData.getPatientCountAll(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company")); iHeader += 1;
                        worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheetSummary.Cells[rows, iHeader].Value = "100";
                        #endregion
                        #region Detail
                        if (dtRegisterDate != null && dtRegisterDate.Rows.Count > 0)
                        {
                            var countSummary = 0;
                            #region เข้ารับการตรวจ
                            iHeader = 1; rows += 1;
                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheetSummary.Cells[rows, iHeader].Value = "เข้ารับการตรวจ";
                            iHeader += 3;
                            for (int i = 0; i < dtRegisterDate.Rows.Count; i++)
                            {
                                var dtRegisterDateDetail = new DataTable();
                                var count = 0;
                                dtRegisterDateDetail = clsTempData.getPatientChecklistCountByProStatus(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"), DateTime.Parse(dtRegisterDate.Rows[i]["RegisterDate"].ToString()).ToString("yyyy-MM-dd"));
                                if (dtRegisterDateDetail != null && dtRegisterDateDetail.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtRegisterDateDetail.Rows.Count; j++)
                                    {
                                        if (dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() != "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetSummary.Cells[rows, iHeader].Value = count;
                                countSummary += count;
                                iHeader += 1;
                            }
                            worksheetSummary.Cells[rows, 2].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, 2].Style.Font.Size = 11; worksheetSummary.Cells[rows, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheetSummary.Cells[rows, 2].Value = countSummary;
                            #endregion
                            #region ตรวจครบทุกรายการ
                            iHeader = 1; rows += 1; countSummary = 0;
                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheetSummary.Cells[rows, iHeader].Value = " - ตรวจครบทุกรายการ";
                            iHeader += 3;
                            for (int i = 0; i < dtRegisterDate.Rows.Count; i++)
                            {
                                var dtRegisterDateDetail = new DataTable();
                                var count = 0;
                                dtRegisterDateDetail = clsTempData.getPatientChecklistCountByProStatus(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"), DateTime.Parse(dtRegisterDate.Rows[i]["RegisterDate"].ToString()).ToString("yyyy-MM-dd"));
                                if (dtRegisterDateDetail != null && dtRegisterDateDetail.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtRegisterDateDetail.Rows.Count; j++)
                                    {
                                        if (dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() != "0" && dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() == dtRegisterDateDetail.Rows[j]["CountChecklistComplete"].ToString())
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetSummary.Cells[rows, iHeader].Value = count;
                                countSummary += count;
                                iHeader += 1;
                            }
                            worksheetSummary.Cells[rows, 2].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, 2].Style.Font.Size = 11; worksheetSummary.Cells[rows, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheetSummary.Cells[rows, 2].Value = countSummary;
                            #endregion
                            #region ค้างตรวจ
                            iHeader = 1; rows += 1; countSummary = 0;
                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheetSummary.Cells[rows, iHeader].Value = " - ค้างตรวจ";
                            iHeader += 3;
                            for (int i = 0; i < dtRegisterDate.Rows.Count; i++)
                            {
                                var dtRegisterDateDetail = new DataTable();
                                var count = 0;
                                dtRegisterDateDetail = clsTempData.getPatientChecklistCountByProStatus(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"), DateTime.Parse(dtRegisterDate.Rows[i]["RegisterDate"].ToString()).ToString("yyyy-MM-dd"));
                                if (dtRegisterDateDetail != null && dtRegisterDateDetail.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtRegisterDateDetail.Rows.Count; j++)
                                    {
                                        if (dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() != "0" && dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() != dtRegisterDateDetail.Rows[j]["CountChecklistComplete"].ToString())
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetSummary.Cells[rows, iHeader].Value = count;
                                countSummary += count;
                                iHeader += 1;
                            }
                            worksheetSummary.Cells[rows, 2].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, 2].Style.Font.Size = 11; worksheetSummary.Cells[rows, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheetSummary.Cells[rows, 2].Value = countSummary;
                            #endregion
                            #region ค้างคืนเอกสาร
                            iHeader = 1; rows += 1; countSummary = 0;
                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheetSummary.Cells[rows, iHeader].Value = " - ค้างคืนเอกสาร";
                            iHeader += 3;
                            for (int i = 0; i < dtRegisterDate.Rows.Count; i++)
                            {
                                var dtRegisterDateDetail = new DataTable();
                                var count = 0;
                                dtRegisterDateDetail = clsTempData.getPatientChecklistCountByProStatus(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"), DateTime.Parse(dtRegisterDate.Rows[i]["RegisterDate"].ToString()).ToString("yyyy-MM-dd"));
                                if (dtRegisterDateDetail != null && dtRegisterDateDetail.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtRegisterDateDetail.Rows.Count; j++)
                                    {
                                        if (dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() != "0" && dtRegisterDateDetail.Rows[j]["CountChecklistDocumentPending"].ToString() != "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetSummary.Cells[rows, iHeader].Value = count;
                                countSummary += count;
                                iHeader += 1;
                            }
                            worksheetSummary.Cells[rows, 2].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, 2].Style.Font.Size = 11; worksheetSummary.Cells[rows, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheetSummary.Cells[rows, 2].Value = countSummary;
                            #endregion
                            #region งดการตรวจ
                            iHeader = 1; rows += 1; countSummary = 0;
                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheetSummary.Cells[rows, iHeader].Value = " - งดการตรวจ";
                            iHeader += 3;
                            for (int i = 0; i < dtRegisterDate.Rows.Count; i++)
                            {
                                var dtRegisterDateDetail = new DataTable();
                                var count = 0;
                                dtRegisterDateDetail = clsTempData.getPatientChecklistCountByProStatus(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"), DateTime.Parse(dtRegisterDate.Rows[i]["RegisterDate"].ToString()).ToString("yyyy-MM-dd"));
                                if (dtRegisterDateDetail != null && dtRegisterDateDetail.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtRegisterDateDetail.Rows.Count; j++)
                                    {
                                        if (dtRegisterDateDetail.Rows[j]["CountChecklistAll"].ToString() != "0" && dtRegisterDateDetail.Rows[j]["CountChecklistCancel"].ToString() != "0")
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                                worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetSummary.Cells[rows, iHeader].Value = count;
                                countSummary += count;
                                iHeader += 1;
                            }
                            worksheetSummary.Cells[rows, 2].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, 2].Style.Font.Size = 11; worksheetSummary.Cells[rows, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheetSummary.Cells[rows, 2].Value = countSummary;
                            #endregion
                            #region ยังไม่ได้เข้ารับการตรวจ
                            iHeader = 1; rows += 1; countSummary = 0;
                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheetSummary.Cells[rows, iHeader].Value = "ยังไม่ได้เข้ารับการตรวจ";
                            iHeader += 1;
                            var countNotRegister = clsTempData.getPatientCountPending(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"));

                            worksheetSummary.Cells[rows, iHeader].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheetSummary.Cells[rows, iHeader].Value = countNotRegister;
                            #endregion
                        }
                        #endregion
                        #endregion
                        #region คำนวนเปอร์เซ็นต์
                        //เข้ารับการตรวจ
                        worksheetSummary.Cells[3, 3].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheetSummary.Cells[3, 3].Value = 100 * (double.Parse(worksheetSummary.Cells[3, 2].Value.ToString()) / double.Parse(worksheetSummary.Cells[2, 2].Value.ToString()));
                        //ยังไม่ได้เข้ารับการตรวจ
                        worksheetSummary.Cells[8, 3].Style.Font.Name = "Tahoma"; worksheetSummary.Cells[rows, iHeader].Style.Font.Size = 11; worksheetSummary.Cells[rows, iHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheetSummary.Cells[8, 3].Value = 100 * (double.Parse(worksheetSummary.Cells[8, 2].Value.ToString()) / double.Parse(worksheetSummary.Cells[2, 2].Value.ToString()));
                        #endregion
                        #region ResizeColumn
                        for (int i = 0; i < worksheetSummary.Dimension.End.Column; i++)
                        {
                            worksheetSummary.Column(i + 1).AutoFit();
                        }
                        #endregion
                    }
                    catch (Exception exDetail)
                    {
                        MessageBox.Show(exDetail.Message, "Summary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    */
                    #endregion
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value = 1;
                        }));
                    }
                    #region Detail
                    try
                    {
                        #region CreateGroupSheet
                        var dtDetailHeader = new DataTable();
                        if (type == "All")
                        {
                            dtDetailHeader.Columns.Add("All");
                            dtDetailHeader.Rows.Add("All");
                        }
                        else if (type == "Payor")
                        {
                            dtDetailHeader = dt.DefaultView.ToTable(true, "Payor");
                        }
                        else if (type == "Book")
                        {
                            dtDetailHeader = dt.DefaultView.ToTable(true, "BookCreate");
                        }
                        #endregion
                        #region CreateSheetDetail
                        for (int i = 0; i < dtDetailHeader.Rows.Count; i++)
                        {
                            #region DataFilter
                            var sheetName = "";
                            var dtDetailDetail = new DataTable();
                            var dvDetailDetail = new DataView(dt);
                            if (type == "All")
                            {
                                dtDetailDetail = dt;
                                sheetName = "Detail - All";
                            }
                            else if (type == "Payor")
                            {
                                sheetName = "Detail - " + dtDetailHeader.Rows[i][0].ToString();
                                dvDetailDetail.RowFilter = "Payor='" + dtDetailHeader.Rows[i][0].ToString() + "'";
                                dtDetailDetail = dvDetailDetail.ToTable();
                            }
                            else if (type == "Book")
                            {
                                sheetName = "Detail - " + dtDetailHeader.Rows[i][0].ToString();
                                dvDetailDetail.RowFilter = "BookCreate='" + dtDetailHeader.Rows[i][0].ToString() + "'";
                                dtDetailDetail = dvDetailDetail.ToTable();
                            }
                            #endregion
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                            rows = 1;
                            #region HeaderBuilder
                            for (int c = 0; c <= dtDetailDetail.Columns.Count - 1; c++)
                            {
                                worksheet.Cells[rows, c + 1].Value = dtDetailDetail.Columns[c].ColumnName;

                                worksheet.Cells[rows, c + 1].Style.Font.Bold = true;
                                worksheet.Cells[rows, c + 1].Style.Font.Name = "Tahoma";
                                worksheet.Cells[rows, c + 1].Style.Font.Size = 12;
                                worksheet.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                worksheet.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                worksheet.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            #endregion
                            #region RowsBuilder
                            for (int r = 0; r < dtDetailDetail.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtDetailDetail.Columns.Count; c++)
                                {
                                    //กรณีฟิลด์เป็นข้อมูลวันที่ ให้คงรูปแบบไว้เพื่อง่ายในการฟิลด์เตอร์
                                    if (dtDetailDetail.Columns[c].ColumnName.Contains("Date") && dtDetailDetail.Rows[r][c].ToString()!="" && dtDetailDetail.Rows[r][c]!=DBNull.Value)
                                    {
                                        try
                                        {
                                            worksheet.Cells[rows + r + 1, c + 1].Value = DateTime.Parse(dtDetailDetail.Rows[r][c].ToString());
                                            worksheet.Cells[rows + r + 1, c + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                                        }
                                        catch (Exception)
                                        {
                                            worksheet.Cells[rows + r + 1, c + 1].Value = dtDetailDetail.Rows[r][c].ToString();
                                        }
                                    }
                                    else
                                    {
                                        worksheet.Cells[rows + r + 1, c + 1].Value = dtDetailDetail.Rows[r][c].ToString();
                                    }
                                    worksheet.Cells[rows + r + 1, c + 1].Style.Font.Name = "Tahoma";
                                    worksheet.Cells[rows + r + 1, c + 1].Style.Font.Size = 11;
                                }
                            }

                            #endregion
                            #region ResizeColumn
                            for (int c = 0; c < dtDetailDetail.Columns.Count; c++)
                            {
                                worksheet.Column(c + 1).AutoFit();
                            }
                            #endregion
                        }
                        #endregion
                    }
                    catch (Exception exDetail)
                    {
                        MessageBox.Show(exDetail.Message, "Detail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #endregion
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value = 2;
                        }));
                    }
                    #region LabSummary
                    try
                    {
                        ExcelWorksheet worksheetLabSummary = package.Workbook.Worksheets.Add("LabSummary");
                        dt = null; rows = 1;
                        dt = clsTempData.getLabSummary();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region HeaderBuilder
                            worksheetLabSummary.Cells[rows, 1].Value = "Company";
                            worksheetLabSummary.Cells[rows, 2].Value = "DateAccept";
                            worksheetLabSummary.Cells[rows, 3].Value = "Blood";
                            worksheetLabSummary.Cells[rows, 4].Value = "Urine";
                            worksheetLabSummary.Cells[rows, 5].Value = "Stool";
                            worksheetLabSummary.Cells[rows, 6].Value = "HeavyMetal";
                            for (int c = 0; c < 6; c++)
                            {
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Bold = true;
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Size = 12;
                                worksheetLabSummary.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                worksheetLabSummary.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                worksheetLabSummary.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheetLabSummary.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            #endregion
                            #region RowsBuilder
                            for (int r = 0; r < dt.Rows.Count; r++)
                            {
                                worksheetLabSummary.Cells[rows + r + 1, 1].Value = dt.Rows[r]["Company"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 1].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows + r + 1, 1].Style.Font.Size = 11;
                                worksheetLabSummary.Cells[rows + r + 1, 2].Value = DateTime.Parse(dt.Rows[r]["DateAccept"].ToString()).ToString("dd/MM/yyyy");
                                worksheetLabSummary.Cells[rows + r + 1, 2].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows + r + 1, 2].Style.Font.Size = 11;
                                worksheetLabSummary.Cells[rows + r + 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 3].Value = dt.Rows[r]["CountBloodComplete"].ToString() + " of " + dt.Rows[r]["CountBloodAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 3].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows + r + 1, 3].Style.Font.Size = 11;
                                worksheetLabSummary.Cells[rows + r + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 4].Value = dt.Rows[r]["CountUrineComplete"].ToString() + " of " + dt.Rows[r]["CountUrineAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 4].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows + r + 1, 4].Style.Font.Size = 11;
                                worksheetLabSummary.Cells[rows + r + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 5].Value = dt.Rows[r]["CountStoolComplete"].ToString() + " of " + dt.Rows[r]["CountStoolAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 5].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows + r + 1, 5].Style.Font.Size = 11;
                                worksheetLabSummary.Cells[rows + r + 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 6].Value = dt.Rows[r]["CountHeavyMetalComplete"].ToString() + " of " + dt.Rows[r]["CountHeavyMetalAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 6].Style.Font.Name = "Tahoma";
                                worksheetLabSummary.Cells[rows + r + 1, 6].Style.Font.Size = 11;
                                worksheetLabSummary.Cells[rows + r + 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            #endregion
                            #region ResizeColumn
                            for (int c = 0; c < 6; c++)
                            {
                                worksheetLabSummary.Column(c + 1).AutoFit();
                            }
                            #endregion
                        }
                    }
                    catch (Exception exDetail)
                    {
                        MessageBox.Show(exDetail.Message, "LabSummary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #endregion
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value = 3;
                        }));
                    }
                    #region LabDetail
                    try
                    {
                        ExcelWorksheet worksheetLabDetail = package.Workbook.Worksheets.Add("LabDetail");
                        dt = null; rows = 1;
                        dt = clsTempData.getLabDetail(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"));
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region HeaderBuilder
                            int c;
                            for (c = 0; c <= dt.Columns.Count - 1; c++)
                            {
                                if (dt.Columns[c].ColumnName == "AcceptDateBlood" ||
                                        dt.Columns[c].ColumnName == "AcceptDateUrine" ||
                                        dt.Columns[c].ColumnName == "AcceptDateStool" ||
                                        dt.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                                {
                                    worksheetLabDetail.Cells[rows, c + 1].Value = dt.Columns[c].ColumnName.Replace("AcceptDate", "");
                                }
                                else if (dt.Columns[c].ColumnName == "CountLabPending")
                                {
                                    worksheetLabDetail.Cells[rows, c + 1].Value = "Remark";
                                }
                                else
                                {
                                    worksheetLabDetail.Cells[rows, c + 1].Value = dt.Columns[c].ColumnName;
                                }
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Bold = true;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Name = "Tahoma";
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Size = 12;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                worksheetLabDetail.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                worksheetLabDetail.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheetLabDetail.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            #endregion
                            #region RowsBuilder
                            for (int r = 0; r < dt.Rows.Count; r++)
                            {
                                for (c = 0; c < dt.Columns.Count; c++)
                                {
                                    if (dt.Columns[c].ColumnName == "AcceptDateBlood" ||
                                        dt.Columns[c].ColumnName == "AcceptDateUrine" ||
                                        dt.Columns[c].ColumnName == "AcceptDateStool" ||
                                        dt.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                                    {
                                        if (dt.Columns[c].ColumnName == "AcceptDateBlood")
                                        {
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dt.Rows[r][c].ToString() == "" ? (dt.Rows[r]["CountChecklistBlood"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dt.Rows[r][c].ToString());
                                        }
                                        else if (dt.Columns[c].ColumnName == "AcceptDateUrine")
                                        {
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dt.Rows[r][c].ToString() == "" ? (dt.Rows[r]["CountChecklistUrine"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dt.Rows[r][c].ToString());
                                        }
                                        else if (dt.Columns[c].ColumnName == "AcceptDateStool")
                                        {
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dt.Rows[r][c].ToString() == "" ? (dt.Rows[r]["CountChecklistStool"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dt.Rows[r][c].ToString());
                                        }
                                        else if (dt.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                                        {
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dt.Rows[r][c].ToString() == "" ? (dt.Rows[r]["CountChecklistHeavyMetal"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dt.Rows[r][c].ToString());
                                        }
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Tahoma";
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 11;
                                    }
                                    else if (dt.Columns[c].ColumnName == "CountLabPending")
                                    {
                                        var remark = "";
                                        if (dt.Rows[r]["CountLabPending"].ToString() != "0" &&
                                            (
                                                dt.Rows[r]["AcceptDateBlood"].ToString() != "" ||
                                                dt.Rows[r]["AcceptDateUrine"].ToString() != "" ||
                                                dt.Rows[r]["AcceptDateStool"].ToString() != "" ||
                                                dt.Rows[r]["AcceptDateHeavyMetal"].ToString() != ""
                                            ))
                                        {
                                            remark = "Reg.Date:" + dt.Rows[r]["RegisterDate"].ToString();
                                        }
                                        else if (dt.Rows[r]["CountLabPending"].ToString() != "0" &&
                                            (
                                                dt.Rows[r]["AcceptDateBlood"].ToString() == "" &&
                                                dt.Rows[r]["AcceptDateUrine"].ToString() == "" &&
                                                dt.Rows[r]["AcceptDateStool"].ToString() == "" &&
                                                dt.Rows[r]["AcceptDateHeavyMetal"].ToString() == ""
                                            ))
                                        {
                                            remark = "ยังไม่ได้เข้ารับการตรวจ";
                                        }

                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = remark;
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Tahoma";
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 11;
                                    }
                                    else
                                    {
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = dt.Rows[r][c].ToString();
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Tahoma";
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 11;
                                    }
                                }
                            }
                            #endregion
                            #region ResizeColumn
                            for (c = 0; c < dt.Columns.Count; c++)
                            {
                                worksheetLabDetail.Column(c + 1).AutoFit();
                            }
                            #endregion
                            #region HiddenColumn
                            int totalRows = worksheetLabDetail.Dimension.End.Row;
                            int totalCols = worksheetLabDetail.Dimension.End.Column;
                            var range = worksheetLabDetail.Cells[1, 1, 1, totalCols];
                            for (int i = 1; i <= totalCols; i++)
                            {
                                if (range[1, i].Address != "" && range[1, i].Value != null && range[1, i].Value.ToString() == "RegisterDate")
                                {
                                    worksheetLabDetail.Column(i).Hidden = true;
                                }
                                else if (range[1, i].Address != "" && range[1, i].Value != null && range[1, i].Value.ToString() == "CountChecklistBlood")
                                {
                                    worksheetLabDetail.Column(i).Hidden = true;
                                }
                                else if (range[1, i].Address != "" && range[1, i].Value != null && range[1, i].Value.ToString() == "CountChecklistUrine")
                                {
                                    worksheetLabDetail.Column(i).Hidden = true;
                                }
                                else if (range[1, i].Address != "" && range[1, i].Value != null && range[1, i].Value.ToString() == "CountChecklistStool")
                                {
                                    worksheetLabDetail.Column(i).Hidden = true;
                                }
                                else if (range[1, i].Address != "" && range[1, i].Value != null && range[1, i].Value.ToString() == "CountChecklistHeavyMetal")
                                {
                                    worksheetLabDetail.Column(i).Hidden = true;
                                }
                            }
                            #endregion
                        }
                    }
                    catch (Exception exDetail)
                    {
                        MessageBox.Show(exDetail.Message, "LabDetail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #endregion
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value = 4;
                        }));
                    }
                    package.Save();
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value = 5;
                        }));
                    }
                    DialogResult dr = MessageBox.Show("Export Seccessful!" + Environment.NewLine + Environment.NewLine + FileName + Environment.NewLine + Environment.NewLine + "ต้องการดูไฟล์คลิก Yes", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(Path.GetDirectoryName(FileName));
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportLab()
        {
            try
            {
                var clsTempData = new clsTempData();
                var FileName = clsGlobal.ExecutePathBuilder() + @"Export\" + "LabAll" + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".xlsx";
                var dt = new DataTable();
                FileInfo newFile = new FileInfo(FileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(FileName);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    var rows = 1;
                    #region LabDetail
                    try
                    {
                        ExcelWorksheet worksheetLabDetail = package.Workbook.Worksheets.Add("LabAll");
                        dt = null; rows = 1;
                        dt = clsTempData.getLabDetail(dtDOEFrom.Value, dtDOETo.Value);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region HeaderBuilder
                            int c;
                            for (c = 0; c <= dt.Columns.Count - 1; c++)
                            {
                                worksheetLabDetail.Cells[rows, c + 1].Value = dt.Columns[c].ColumnName;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Bold = true;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Name = "Tahoma";
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Size = 12;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheetLabDetail.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                worksheetLabDetail.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                worksheetLabDetail.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheetLabDetail.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            #endregion
                            #region RowsBuilder
                            for (int r = 0; r < dt.Rows.Count; r++)
                            {
                                for (c = 0; c < dt.Columns.Count; c++)
                                {
                                    worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = dt.Rows[r][c].ToString();
                                    worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Tahoma";
                                    worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 11;
                                }
                            }
                            #endregion
                            #region ResizeColumn
                            for (c = 0; c < dt.Columns.Count; c++)
                            {
                                worksheetLabDetail.Column(c + 1).AutoFit();
                            }
                            #endregion
                        }
                    }
                    catch (Exception exDetail)
                    {
                        MessageBox.Show(exDetail.Message, "LabDetail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #endregion
                    package.Save();
                    DialogResult dr = MessageBox.Show("Export Seccessful!" + Environment.NewLine + Environment.NewLine + FileName + Environment.NewLine + Environment.NewLine + "ต้องการดูไฟล์คลิก Yes", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(Path.GetDirectoryName(FileName));
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExportLab Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Search()
        {
            #region Variable
            var dt = new DataTable();
            var clsTempData = new clsTempData();
            var SyncStatus = "";
            #endregion
            #region Procedure
            var type = getDropDownListValue(ddlType, "Name");
            switch (type)
            {
                case "All":
                    dt = clsTempData.getPatientMobileByAll(dtDOEFrom.Value, dtDOETo.Value);
                    break;
                case "Payor":
                    dt = clsTempData.getPatientMobile(dtDOEFrom.Value, dtDOETo.Value, (getDropDownListValue(ddlCompany, "Company")!="- ทั้งหมด -"? getDropDownListValue(ddlCompany, "Company"):""));
                    break;
                case "Book":
                    dt = clsTempData.getPatientMobileByBookCreate(dtDOEFrom.Value, dtDOETo.Value, (getDropDownListValue(ddlCompany, "BookCreate")!="- ทั้งหมด -"? getDropDownListValue(ddlCompany, "BookCreate"):""));
                    break;
                default:
                    break;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Remove("PatientGUID");
                if (btExport.InvokeRequired)
                {
                    btExport.Invoke(new MethodInvoker(delegate
                    {
                        btExport.Enabled = true;
                    }));
                }
                if (lblDefault.InvokeRequired)
                {
                    lblDefault.Invoke(new MethodInvoker(delegate
                    {
                        lblDefault.Text = "";
                    }));
                }
                #region AddColumns
                dt.Columns.Add("Summary", typeof(string));
                dt.Columns.Add("Remark", typeof(string));
                dt.Columns.Add("RemarkCancel", typeof(string));
                #endregion
                #region FillData
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["CountChecklistAll"].ToString() != "0" && dt.Rows[i]["CountChecklistAll"].ToString() == dt.Rows[i]["CountChecklistComplete"].ToString())
                    {
                        dt.Rows[i]["Summary"] = "ตรวจแล้ว";
                        dt.Rows[i]["Remark"] = "ตรวจครบทุกรายการ";
                    }
                    else if (dt.Rows[i]["CountChecklistComplete"].ToString() != "0" && dt.Rows[i]["CountChecklistAll"].ToString() != dt.Rows[i]["CountChecklistComplete"].ToString())
                    {
                        dt.Rows[i]["Summary"] = "ตรวจแล้ว-มีค้างตรวจ";
                        dt.Rows[i]["Remark"] = dt.Rows[i]["ProgramPending"];
                    }
                    else if (dt.Rows[i]["CountChecklistComplete"].ToString() == "0" && dt.Rows[i]["CountChecklistAll"].ToString() != "0")
                    {
                        dt.Rows[i]["Summary"] = "ยังไม่ได้เข้ารับการตรวจ";
                        dt.Rows[i]["Remark"] = "";
                    }
                    else
                    {
                        dt.Rows[i]["Summary"] = "ยังไม่ได้เข้ารับการตรวจ";
                        dt.Rows[i]["Remark"] = "";
                    }
                    if (dt.Rows[i]["ProgramCancel"].ToString() != "")
                    {
                        dt.Rows[i]["RemarkCancel"] = dt.Rows[i]["ProgramCancel"].ToString();
                    }
                }
                dt.AcceptChanges();
                #endregion
                #region RemoveColumn
                string[] columns = { "CountChecklistAll", "CountChecklistComplete", "CountChecklistCancel", "ProgramPending", "ProgramCancel" };
                for (int i = 0; i < columns.Length; i++)
                {
                    dt.Columns.Remove(columns[i]);
                }
                dt.AcceptChanges();
                #endregion
                clsGlobal.dtPatient = dt.Copy();
                clsGlobal.dtPatient.Columns.Remove("SyncWhen");
                clsGlobal.dtPatient.Columns.Remove("SyncStatus");
                if (gvDefault.InvokeRequired)
                {
                    gvDefault.Invoke(new MethodInvoker(delegate
                    {
                        gvDefault.DataSource = dt;
                        #region HighlightSync
                        for (int i = 0; i < gvDefault.Rows.Count; i++)
                        { 
                            if(gvDefault.Rows[i].Cells["SyncStatus"].Value!= null)
                            { 
                                SyncStatus = gvDefault.Rows[i].Cells["SyncStatus"].Value.ToString().Trim();
                            }
                            else { SyncStatus = "0"; }
                            if (SyncStatus == "1")
                            {
                                gvDefault.Rows[i].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFDD68");
                            }
                        }
                        #endregion
                    }));
                }
                if (lblDefault.InvokeRequired)
                {
                    lblDefault.Invoke(new MethodInvoker(delegate
                    {
                        lblDefault.Text = string.Format("พบข้อมูลทั้งหมด {0} รายการ", dt.Rows.Count.ToString());
                    }));
                }
            }
            else
            {
                clsGlobal.dtPatient = null;
                if (gvDefault.InvokeRequired)
                {
                    gvDefault.Invoke(new MethodInvoker(delegate
                    {
                        gvDefault.DataSource = null;
                    }));
                }
                if (btExport.InvokeRequired)
                {
                    btExport.Invoke(new MethodInvoker(delegate
                    {
                        btExport.Enabled = false;
                    }));
                }
                if (lblDefault.InvokeRequired)
                {
                    lblDefault.Invoke(new MethodInvoker(delegate
                    {
                        lblDefault.Text = "- ไม่พบข้อมูลที่ต้องการ -";
                    }));
                }
            }
            #endregion
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
                        try
                        {
                            var drv = (DataRowView)ddlName.SelectedItem;
                            var dr = drv.Row;
                            try
                            {
                                result = dr[columnName].ToString();
                            }
                            catch (Exception)
                            {
                                result = dr[0].ToString();
                            }
                        }
                        catch (Exception)
                        {
                            result = "";
                        }
                    }));
                }
                else
                {
                    try
                    {
                        var drv = (DataRowView)ddlName.SelectedItem;
                        var dr = drv.Row;
                        try
                        {
                            result = dr[columnName].ToString();
                        }
                        catch (Exception)
                        {
                            result = dr[0].ToString();
                        }
                    }
                    catch (Exception)
                    {
                        result = "";
                    }
                }
            }
            catch (Exception) { }
            #endregion
            return result;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = "กำลังสร้างรายงาน...";
                }));
            }
            if (pbDefault.InvokeRequired)
            {
                pbDefault.Invoke(new MethodInvoker(delegate
                {
                    pbDefault.Visible = true;
                }));
            }
            Export();
            if (pbDefault.InvokeRequired)
            {
                pbDefault.Invoke(new MethodInvoker(delegate
                {
                    pbDefault.Visible = false;
                }));
            }
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = "สร้างรายงานเสร็จสิ้น";
                }));
            }
        }
        private void setType()
        {
            #region Variable
            var clsTempData = new clsTempData();
            var dt = new DataTable();
            #endregion
            #region Procedure
            dt = clsTempData.getReportType();
            if(dt!=null && dt.Rows.Count > 0)
            {
                ddlType.DataSource = dt;
                ddlType.DisplayMember = "Name";
                ddlType.ValueMember = "UID";

                ddlType.SelectedIndex = 0;

                ddlCompany.Enabled = false;
            }
            #endregion
        }
        private void wsSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            if (btSearch.InvokeRequired)
            {
                btSearch.Invoke(new MethodInvoker(delegate
                {
                    btSearch.Enabled = false;
                }));
            }
            if (btExport.InvokeRequired)
            {
                btExport.Invoke(new MethodInvoker(delegate
                {
                    btExport.Enabled = false;
                }));
            }
            if (btLabExport.InvokeRequired)
            {
                btLabExport.Invoke(new MethodInvoker(delegate
                {
                    btLabExport.Enabled = false;
                }));
            }
            if (anWaiting.InvokeRequired)
            {
                anWaiting.Invoke(new MethodInvoker(delegate
                {
                    anWaiting.Visible = true;
                }));
            }
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = "กำลังทำการค้นหา...";
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
            if (anWaiting.InvokeRequired)
            {
                anWaiting.Invoke(new MethodInvoker(delegate
                {
                    anWaiting.Visible = false;
                }));
            }
            if (btExport.InvokeRequired)
            {
                btExport.Invoke(new MethodInvoker(delegate
                {
                    btExport.Enabled = true;
                }));
            }
            if (btLabExport.InvokeRequired)
            {
                btLabExport.Invoke(new MethodInvoker(delegate
                {
                    btLabExport.Enabled = true;
                }));
            }
        }
        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDropDownList();
        }
        private void setDropDownList()
        {
            var type = getDropDownListValue(ddlType, "Name");
            switch (type)
            {
                case "All":
                    ddlCompany.Enabled = false;
                    break;
                case "Payor":
                    ddlCompany.Enabled = true;
                    setCompany();
                    break;
                case "Book":
                    ddlCompany.Enabled = true;
                    setBookCreate();
                    break;
                default:
                    break;
            }
        }
    }
}