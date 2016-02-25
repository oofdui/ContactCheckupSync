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
                        pbDefault.Maximum = 6;
                        pbDefault.Value = 1;
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
                    var columnSummaryComplete = 0;
                    var columnSummaryInComplete = 0;
                    List<int> summaryCompletes = new List<int>();
                    List<int> summaryInCompletes = new List<int>();
                    #region FindSummaryColumn
                    columnSummaryComplete = clsTempData.getMaxDateRegis(dtDOEFrom.Value, dtDOETo.Value);
                    if (columnSummaryComplete > 0)
                    {
                        columnSummaryComplete += 3;
                        columnSummaryInComplete = columnSummaryComplete + 1;
                    }
                    #endregion
                    var dtSummary = clsTempData.getPatientMobileByAll(dtDOEFrom.Value, dtDOETo.Value);
                    if(dtSummary!=null && dtSummary.Rows.Count > 0)
                    {
                        #region Replace DateTime with Date
                        for(int i = 0; i < dtSummary.Rows.Count; i++)
                        {
                            if(dtSummary.Rows[i]["DateRegis"]!=DBNull.Value && dtSummary.Rows[i]["DateRegis"].ToString() != "")
                            {
                                dtSummary.Rows[i]["DateRegis"] = DateTime.Parse(dtSummary.Rows[i]["DateRegis"].ToString()).ToString("yyyy-MM-dd");
                            }
                        }
                        dtSummary.AcceptChanges();
                        #endregion
                        ExcelWorksheet worksheetSummary = package.Workbook.Worksheets.Add("Summary");
                        #region Header
                        var headers = new string[] { "Payor", "จำนวนทั้งหมด"};
                        var iHeader = 0;
                        for (iHeader = 0; iHeader < headers.Length; iHeader++)
                        {
                            worksheetSummary.Cells[rows, iHeader + 1].Value = headers[iHeader];

                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Bold = true;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Name = "Angsana New";
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Size = 14;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Font.Color.SetColor(Color.White);
                            worksheetSummary.Cells[rows, iHeader + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                            worksheetSummary.Cells[rows, iHeader + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheetSummary.Cells[rows, iHeader + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        #region HeaderSummary
                        worksheetSummary.Cells[rows, columnSummaryComplete].Value = "รวมตรวจเสร็จ";

                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.Font.Bold = true;
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.Font.Name = "Angsana New";
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.Font.Size = 14;
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.Font.Color.SetColor(Color.White);
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheetSummary.Cells[rows, columnSummaryComplete].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        worksheetSummary.Cells[rows, columnSummaryInComplete].Value = "รวมค้างตรวจ";

                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Font.Bold = true;
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Font.Name = "Angsana New";
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Font.Size = 14;
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Font.Color.SetColor(Color.White);
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheetSummary.Cells[rows, columnSummaryInComplete].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        #endregion
                        #endregion
                        var dtSummaryPayor = dtSummary.DefaultView.ToTable(true, "Payor");
                        #region LoopByPayor
                        for (int i = 0; i < dtSummaryPayor.Rows.Count; i++)
                        {
                            var dvSummaryPayorDetail = new DataView(dtSummary);
                            var dtSummaryPayorDetail = new DataTable();

                            dvSummaryPayorDetail.RowFilter = "Payor='"+dtSummaryPayor.Rows[i][0].ToString()+"'";
                            dvSummaryPayorDetail.Sort = "DateRegis";
                            dtSummaryPayorDetail = dvSummaryPayorDetail.ToTable();
                            if(dtSummaryPayorDetail != null && dtSummaryPayorDetail.Rows.Count > 0)
                            {
                                rows += 1;
                                #region Payor
                                worksheetSummary.Cells[rows, 1].Value = dtSummaryPayor.Rows[i]["Payor"].ToString();
                                worksheetSummary.Cells[rows, 1].Style.Font.Name = "Angsana New";
                                worksheetSummary.Cells[rows, 1].Style.Font.Size = 14;
                                #endregion
                                #region CountAll
                                worksheetSummary.Cells[rows, 2].Value = dtSummaryPayorDetail.Rows.Count;
                                worksheetSummary.Cells[rows, 2].Style.Font.Name = "Angsana New";
                                worksheetSummary.Cells[rows, 2].Style.Font.Size = 14;
                                worksheetSummary.Cells[rows, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                #endregion
                                #region DistinctDateRegis
                                var dtSummaryDateRegis = dtSummaryPayorDetail.DefaultView.ToTable(true, "DateRegis");
                                var summaryComplete = 0;
                                if(dtSummaryDateRegis != null && dtSummaryDateRegis.Rows.Count > 0)
                                {
                                    #region LoopByRegisDateDetail
                                    var columnTemp = 3;//Start 3
                                    for(int r = 0; r < dtSummaryDateRegis.Rows.Count; r++)
                                    {
                                        if (dtSummaryDateRegis.Rows[r]["DateRegis"].ToString() != "")
                                        {
                                            var dvSummaryDateRegisDetail = new DataView(dtSummaryPayorDetail);
                                            var dtSummaryDateRegisDetail = new DataTable();
                                            var sumByDay = 0;

                                            dvSummaryDateRegisDetail.RowFilter = "DateRegis='"+ DateTime.Parse(dtSummaryDateRegis.Rows[r]["DateRegis"].ToString()).ToString("yyyy-MM-dd HH:mm") + "'";
                                            dtSummaryDateRegisDetail = dvSummaryDateRegisDetail.ToTable();
                                            #region CompleteSumByDay
                                            for(int s = 0; s < dtSummaryDateRegisDetail.Rows.Count; s++)
                                            {
                                                if (int.Parse(dtSummaryDateRegisDetail.Rows[s]["CountChecklistAll"].ToString()) == 
                                                    int.Parse(dtSummaryDateRegisDetail.Rows[s]["CountChecklistComplete"].ToString()))
                                                {
                                                    sumByDay += 1;
                                                }
                                            }
                                            #endregion
                                            #region SetHeader
                                            worksheetSummary.Cells[1, columnTemp].Value = "Day "+(r).ToString();

                                            worksheetSummary.Cells[1, columnTemp].Style.Font.Bold = true;
                                            worksheetSummary.Cells[1, columnTemp].Style.Font.Name = "Angsana New";
                                            worksheetSummary.Cells[1, columnTemp].Style.Font.Size = 14;
                                            worksheetSummary.Cells[1, columnTemp].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheetSummary.Cells[1, columnTemp].Style.Font.Color.SetColor(Color.White);
                                            worksheetSummary.Cells[1, columnTemp].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#66C2DC"));
                                            worksheetSummary.Cells[1, columnTemp].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                            worksheetSummary.Cells[1, columnTemp].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                            #endregion
                                            #region SetValue
                                            summaryComplete += sumByDay;
                                            worksheetSummary.Cells[rows, columnTemp].Value = sumByDay;
                                            worksheetSummary.Cells[rows, columnTemp].AddComment(DateTime.Parse(dtSummaryDateRegis.Rows[r]["DateRegis"].ToString()).ToString("dd/MM/yyyy HH:mm"), "วันที่");
                                            worksheetSummary.Cells[rows, columnTemp].Style.Font.Name = "Angsana New";
                                            worksheetSummary.Cells[rows, columnTemp].Style.Font.Size = 14;
                                            worksheetSummary.Cells[rows, columnTemp].Style.WrapText = true;
                                            worksheetSummary.Cells[rows, columnTemp].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                            columnTemp += 1;
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                #region SummaryComplete
                                worksheetSummary.Cells[rows, columnSummaryComplete].Value = summaryComplete;
                                worksheetSummary.Cells[rows, columnSummaryComplete].Style.Font.Name = "Angsana New";
                                worksheetSummary.Cells[rows, columnSummaryComplete].Style.Font.Size = 14;
                                worksheetSummary.Cells[rows, columnSummaryComplete].Style.WrapText = true;
                                worksheetSummary.Cells[rows, columnSummaryComplete].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                #endregion
                                #region SummaryInComplete
                                worksheetSummary.Cells[rows, columnSummaryInComplete].Value = dtSummaryPayorDetail.Rows.Count-summaryComplete;
                                worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Font.Name = "Angsana New";
                                worksheetSummary.Cells[rows, columnSummaryInComplete].Style.Font.Size = 14;
                                worksheetSummary.Cells[rows, columnSummaryInComplete].Style.WrapText = true;
                                worksheetSummary.Cells[rows, columnSummaryInComplete].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                #endregion
                                #endregion
                            }
                        }
                        #endregion
                        #region ResizeColumn
                        for (int resizeC = 0; resizeC < 50; resizeC++)
                        {
                            worksheetSummary.Column(resizeC + 1).AutoFit();
                        }
                        #endregion
                    }
                    #endregion
                    rows = 1;
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value += 1;
                        }));
                    }
                    #region LabSummary
                    try
                    {
                        ExcelWorksheet worksheetLabSummary = package.Workbook.Worksheets.Add("LabSummary");
                        var dtLabSummary = new DataTable(); rows = 1;
                        dtLabSummary = clsTempData.getLabSummary();
                        if (dtLabSummary != null && dtLabSummary.Rows.Count > 0)
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
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Size = 14;
                                worksheetLabSummary.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheetLabSummary.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                worksheetLabSummary.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                worksheetLabSummary.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheetLabSummary.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            #endregion
                            #region RowsBuilder
                            for (int r = 0; r < dtLabSummary.Rows.Count; r++)
                            {
                                worksheetLabSummary.Cells[rows + r + 1, 1].Value = dtLabSummary.Rows[r]["Company"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 1].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows + r + 1, 1].Style.Font.Size = 14;
                                worksheetLabSummary.Cells[rows + r + 1, 2].Value = DateTime.Parse(dtLabSummary.Rows[r]["DateAccept"].ToString()).ToString("dd/MM/yyyy");
                                worksheetLabSummary.Cells[rows + r + 1, 2].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows + r + 1, 2].Style.Font.Size = 14;
                                worksheetLabSummary.Cells[rows + r + 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 3].Value = dtLabSummary.Rows[r]["CountBloodComplete"].ToString() + " of " + dtLabSummary.Rows[r]["CountBloodAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 3].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows + r + 1, 3].Style.Font.Size = 14;
                                worksheetLabSummary.Cells[rows + r + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 4].Value = dtLabSummary.Rows[r]["CountUrineComplete"].ToString() + " of " + dtLabSummary.Rows[r]["CountUrineAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 4].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows + r + 1, 4].Style.Font.Size = 14;
                                worksheetLabSummary.Cells[rows + r + 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 5].Value = dtLabSummary.Rows[r]["CountStoolComplete"].ToString() + " of " + dtLabSummary.Rows[r]["CountStoolAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 5].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows + r + 1, 5].Style.Font.Size = 14;
                                worksheetLabSummary.Cells[rows + r + 1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheetLabSummary.Cells[rows + r + 1, 6].Value = dtLabSummary.Rows[r]["CountHeavyMetalComplete"].ToString() + " of " + dtLabSummary.Rows[r]["CountHeavyMetalAll"].ToString();
                                worksheetLabSummary.Cells[rows + r + 1, 6].Style.Font.Name = "Angsana New";
                                worksheetLabSummary.Cells[rows + r + 1, 6].Style.Font.Size = 14;
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
                            pbDefault.Value += 1;
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
                            #region TrimValue
                            //for(int t = 0; t < dt.Rows.Count; t++)
                            //{
                            //    dt.Rows[t]["BookCreate"] = dt.Rows[t]["BookCreate"].ToString().Trim();
                            //}
                            //dt.AcceptChanges();
                            #endregion
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
                            ExcelWorksheet worksheet=null;
                            try
                            {
                                worksheet = package.Workbook.Worksheets.Add(sheetName);
                            }
                            catch (Exception)
                            {
                                worksheet = package.Workbook.Worksheets.Add(sheetName+" ("+i.ToString()+")");
                            }
                            worksheet.TabColor = ColorTranslator.FromHtml("#FFC90E");
                            //worksheet.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFC90E"));

                            rows = 1;
                            #region HeaderBuilder
                            for (int c = 0; c <= dtDetailDetail.Columns.Count - 1; c++)
                            {
                                worksheet.Cells[rows, c + 1].Value = dtDetailDetail.Columns[c].ColumnName;

                                worksheet.Cells[rows, c + 1].Style.Font.Bold = true;
                                worksheet.Cells[rows, c + 1].Style.Font.Name = "Angsana New";
                                worksheet.Cells[rows, c + 1].Style.Font.Size = 14;
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
                                    worksheet.Cells[rows + r + 1, c + 1].Style.Font.Name = "Angsana New";
                                    worksheet.Cells[rows + r + 1, c + 1].Style.Font.Size = 14;
                                }
                            }
                            #endregion
                            #region ResizeColumn
                            //for (int c = 0; c < dtDetailDetail.Columns.Count; c++)
                            //{
                            //    if (!dtDetailDetail.Columns[c+1].ColumnName.ToLower().Trim().Contains("remark"))
                            //    {
                            //        worksheet.Column(c + 1).AutoFit();
                            //    }
                            //}
                            for(int c=1;c< 200; c++)
                            {
                                if(worksheet.Cells[1, c].Value!= null)
                                {
                                    if (!worksheet.Cells[1, c].Value.ToString().Trim().ToLower().Contains("remark") &&
                                        !worksheet.Cells[1, c].Value.ToString().Trim().ToLower().Contains("programdetail") &&
                                        worksheet.Cells[1, c].Value.ToString().Trim() != "")
                                    {
                                        worksheet.Column(c).AutoFit();
                                    }
                                }
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
                            pbDefault.Value += 1;
                        }));
                    }
                    #region LabDetail
                    try
                    {
                        dt = null; rows = 1;
                        #region CreateGroupSheet
                        var dtLabHeader = new DataTable();
                        if (type == "All")
                        {
                            dt = clsTempData.getLabDetailByAll(dtDOEFrom.Value, dtDOETo.Value);
                            dtLabHeader.Columns.Add("All");
                            dtLabHeader.Rows.Add("All");
                        }
                        else if (type == "Payor")
                        {
                            dt = clsTempData.getLabDetail(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "Company"));
                            dtLabHeader = dt.DefaultView.ToTable(true, "Payor");
                        }
                        else if (type == "Book")
                        {
                            dt = clsTempData.getLabDetailByBook(dtDOEFrom.Value, dtDOETo.Value, getDropDownListValue(ddlCompany, "BookCreate"));
                            #region TrimValue
                            //for (int t = 0; t < dt.Rows.Count; t++)
                            //{
                            //    dt.Rows[t]["BookCreate"] = dt.Rows[t]["BookCreate"].ToString().Trim();
                            //}
                            //dt.AcceptChanges();
                            #endregion
                            dtLabHeader = dt.DefaultView.ToTable(true, "BookCreate");
                        }
                        #endregion
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for(int i = 0; i < dtLabHeader.Rows.Count; i++)
                            {
                                #region DataFilter
                                var sheetName = "";
                                var dtLabDetail = new DataTable();
                                var dvLabDetail = new DataView(dt);
                                if (type == "All")
                                {
                                    dtLabDetail = dt;
                                    sheetName = "Lab - All";
                                }
                                else if (type == "Payor")
                                {
                                    sheetName = "Lab - " + dtLabHeader.Rows[i][0].ToString();
                                    dvLabDetail.RowFilter = "Payor='" + dtLabHeader.Rows[i][0].ToString() + "'";
                                    dtLabDetail = dvLabDetail.ToTable();
                                }
                                else if (type == "Book")
                                {
                                    sheetName = "Lab - " + dtLabHeader.Rows[i][0].ToString();
                                    dvLabDetail.RowFilter = "BookCreate='" + dtLabHeader.Rows[i][0].ToString() + "'";
                                    dtLabDetail = dvLabDetail.ToTable();
                                }
                                #endregion
                                ExcelWorksheet worksheetLabDetail = package.Workbook.Worksheets.Add(sheetName);
                                worksheetLabDetail.TabColor = ColorTranslator.FromHtml("#FF7F27");
                                #region HeaderBuilder
                                int c;
                                for (c = 0; c <= dtLabDetail.Columns.Count - 1; c++)
                                {
                                    if (dtLabDetail.Columns[c].ColumnName == "AcceptDateBlood" ||
                                            dtLabDetail.Columns[c].ColumnName == "AcceptDateUrine" ||
                                            dtLabDetail.Columns[c].ColumnName == "AcceptDateStool" ||
                                            dtLabDetail.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                                    {
                                        worksheetLabDetail.Cells[rows, c + 1].Value = dtLabDetail.Columns[c].ColumnName.Replace("AcceptDate", "");
                                    }
                                    else if (dtLabDetail.Columns[c].ColumnName == "CountLabPending")
                                    {
                                        worksheetLabDetail.Cells[rows, c + 1].Value = "Remark";
                                    }
                                    else
                                    {
                                        worksheetLabDetail.Cells[rows, c + 1].Value = dtLabDetail.Columns[c].ColumnName;
                                    }
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Bold = true;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Name = "Angsana New";
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Size = 14;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                    worksheetLabDetail.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }
                                #endregion
                                #region RowsBuilder
                                for (int r = 0; r < dtLabDetail.Rows.Count; r++)
                                {
                                    for (c = 0; c < dtLabDetail.Columns.Count; c++)
                                    {
                                        if (dtLabDetail.Columns[c].ColumnName == "AcceptDateBlood" ||
                                            dtLabDetail.Columns[c].ColumnName == "AcceptDateUrine" ||
                                            dtLabDetail.Columns[c].ColumnName == "AcceptDateStool" ||
                                            dtLabDetail.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                                        {
                                            if (dtLabDetail.Columns[c].ColumnName == "AcceptDateBlood")
                                            {
                                                worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dtLabDetail.Rows[r][c].ToString() == "" ? (dtLabDetail.Rows[r]["CountChecklistBlood"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dtLabDetail.Rows[r][c].ToString());
                                            }
                                            else if (dtLabDetail.Columns[c].ColumnName == "AcceptDateUrine")
                                            {
                                                worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dtLabDetail.Rows[r][c].ToString() == "" ? (dtLabDetail.Rows[r]["CountChecklistUrine"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dtLabDetail.Rows[r][c].ToString());
                                            }
                                            else if (dtLabDetail.Columns[c].ColumnName == "AcceptDateStool")
                                            {
                                                worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dtLabDetail.Rows[r][c].ToString() == "" ? (dtLabDetail.Rows[r]["CountChecklistStool"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dtLabDetail.Rows[r][c].ToString());
                                            }
                                            else if (dtLabDetail.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                                            {
                                                worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dtLabDetail.Rows[r][c].ToString() == "" ? (dtLabDetail.Rows[r]["CountChecklistHeavyMetal"].ToString() == "0" ? "" : "ยังไม่เก็บ Specimen") : dtLabDetail.Rows[r][c].ToString());
                                            }
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Angsana New";
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 14;
                                        }
                                        else if (dtLabDetail.Columns[c].ColumnName == "CountLabPending")
                                        {
                                            var remark = "";
                                            if (dtLabDetail.Rows[r]["CountLabPending"].ToString() != "0" &&
                                                (
                                                    dtLabDetail.Rows[r]["AcceptDateBlood"].ToString() != "" ||
                                                    dtLabDetail.Rows[r]["AcceptDateUrine"].ToString() != "" ||
                                                    dtLabDetail.Rows[r]["AcceptDateStool"].ToString() != "" ||
                                                    dtLabDetail.Rows[r]["AcceptDateHeavyMetal"].ToString() != ""
                                                ))
                                            {
                                                remark = "Reg.Date:" + dtLabDetail.Rows[r]["RegisterDate"].ToString();
                                            }
                                            else if (dtLabDetail.Rows[r]["CountLabPending"].ToString() != "0" &&
                                                (
                                                    dtLabDetail.Rows[r]["AcceptDateBlood"].ToString() == "" &&
                                                    dtLabDetail.Rows[r]["AcceptDateUrine"].ToString() == "" &&
                                                    dtLabDetail.Rows[r]["AcceptDateStool"].ToString() == "" &&
                                                    dtLabDetail.Rows[r]["AcceptDateHeavyMetal"].ToString() == ""
                                                ))
                                            {
                                                remark = "ยังไม่ได้เข้ารับการตรวจ";
                                            }

                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = remark;
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Angsana New";
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 14;
                                        }
                                        else
                                        {
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = dtLabDetail.Rows[r][c].ToString();
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Angsana New";
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 14;
                                        }
                                    }
                                }
                                #endregion
                                #region ResizeColumn
                                for (c = 0; c < dtLabDetail.Columns.Count; c++)
                                {
                                    //if (!dtLabDetail.Columns[c + 1].ColumnName.ToLower().Trim().Contains("remark"))
                                    //{
                                        worksheetLabDetail.Column(c + 1).AutoFit();
                                    //}
                                }
                                #endregion
                                #region HiddenColumn
                                int totalRows = worksheetLabDetail.Dimension.End.Row;
                                int totalCols = worksheetLabDetail.Dimension.End.Column;
                                var range = worksheetLabDetail.Cells[1, 1, 1, totalCols];
                                for (int ii = 1; ii <= totalCols; ii++)
                                {
                                    if (range[1, ii].Address != "" && range[1, ii].Value != null && range[1, ii].Value.ToString() == "RegisterDate")
                                    {
                                        worksheetLabDetail.Column(ii).Hidden = true;
                                    }
                                    else if (range[1, ii].Address != "" && range[1, ii].Value != null && range[1, ii].Value.ToString() == "CountChecklistBlood")
                                    {
                                        worksheetLabDetail.Column(ii).Hidden = true;
                                    }
                                    else if (range[1, ii].Address != "" && range[1, ii].Value != null && range[1, ii].Value.ToString() == "CountChecklistUrine")
                                    {
                                        worksheetLabDetail.Column(ii).Hidden = true;
                                    }
                                    else if (range[1, ii].Address != "" && range[1, ii].Value != null && range[1, ii].Value.ToString() == "CountChecklistStool")
                                    {
                                        worksheetLabDetail.Column(ii).Hidden = true;
                                    }
                                    else if (range[1, ii].Address != "" && range[1, ii].Value != null && range[1, ii].Value.ToString() == "CountChecklistHeavyMetal")
                                    {
                                        worksheetLabDetail.Column(ii).Hidden = true;
                                    }
                                }
                                #endregion
                            }
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
                            pbDefault.Value += 1;
                        }));
                    }
                    package.Save();
                    if (pbDefault.InvokeRequired)
                    {
                        pbDefault.Invoke(new MethodInvoker(delegate
                        {
                            pbDefault.Value += 1;
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
                #region ReportType
                var type = getDropDownListValue(ddlType, "Name");
                var reportType = "";
                switch (type)
                {
                    case "All":
                        reportType = "ALL";
                        break;
                    case "Payor":
                        reportType = "PAYOR";
                        if (getDropDownListValue(ddlCompany, "Payor") != "- ทั้งหมด -")
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

                var clsTempData = new clsTempData();
                var FileName = clsGlobal.ExecutePathBuilder() + @"Export\" + "LAB-"+ reportType + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".xlsx";
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
                        dt = null; rows = 1;
                        dt = clsTempData.getLabDetail(dtDOEFrom.Value, dtDOETo.Value);
                        #region CreateGroupSheet
                        var dtSheet = new DataTable();
                        if (type == "All")
                        {
                            dtSheet.Columns.Add("All");
                            dtSheet.Rows.Add("All");
                        }
                        else if (type == "Payor")
                        {
                            dtSheet = dt.DefaultView.ToTable(true, "Payor");
                        }
                        else if (type == "Book")
                        {
                            dtSheet = dt.DefaultView.ToTable(true, "BookCreate");
                        }
                        #endregion
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for(int ii = 0; ii < dtSheet.Rows.Count; ii++)
                            {
                                #region DataFilter
                                var sheetName = "";
                                var dtDetail = new DataTable();
                                var dvDetail = new DataView(dt);
                                if (type == "All")
                                {
                                    dtDetail = dt;
                                    sheetName = "All";
                                }
                                else if (type == "Payor")
                                {
                                    sheetName = dtSheet.Rows[ii][0].ToString();
                                    dvDetail.RowFilter = "Payor='" + dtSheet.Rows[ii][0].ToString() + "'";
                                    dtDetail = dvDetail.ToTable();
                                }
                                else if (type == "Book")
                                {
                                    sheetName = dtSheet.Rows[ii][0].ToString();
                                    dvDetail.RowFilter = "BookCreate='" + dtSheet.Rows[ii][0].ToString() + "'";
                                    dtDetail = dvDetail.ToTable();
                                }
                                #endregion
                                ExcelWorksheet worksheetLabDetail = package.Workbook.Worksheets.Add(sheetName);
                                worksheetLabDetail.TabColor = ColorTranslator.FromHtml("#B5E61D");

                                #region HeaderBuilder
                                int c;
                                for (c = 0; c <= dtDetail.Columns.Count - 1; c++)
                                {
                                    worksheetLabDetail.Cells[rows, c + 1].Value = dtDetail.Columns[c].ColumnName;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Bold = true;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Name = "Angsana New";
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Size = 14;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Font.Color.SetColor(Color.White);
                                    worksheetLabDetail.Cells[rows, c + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#31b0d3"));
                                    worksheetLabDetail.Cells[rows, c + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheetLabDetail.Cells[rows, c + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                }
                                #endregion
                                #region RowsBuilder
                                DateTime outDateTime;
                                for (int r = 0; r < dtDetail.Rows.Count; r++)
                                {
                                    for (c = 0; c < dtDetail.Columns.Count; c++)
                                    {
                                        if(DateTime.TryParse(dtDetail.Rows[r][c].ToString(),out outDateTime))
                                        {
                                            worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                                        }
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = dtDetail.Rows[r][c];
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Name = "Angsana New";
                                        worksheetLabDetail.Cells[rows + r + 1, c + 1].Style.Font.Size = 14;
                                    }
                                }
                                #endregion
                                #region ResizeColumn
                                for (c = 0; c < dtDetail.Columns.Count; c++)
                                {
                                    worksheetLabDetail.Column(c + 1).AutoFit();
                                }
                                #endregion
                            }
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
                    dt = clsTempData.getPatientMobileByAll(dtDOEFrom.Value, dtDOETo.Value,(cbHeavyMetal.Checked? false : true));
                    break;
                case "Payor":
                    dt = clsTempData.getPatientMobile(dtDOEFrom.Value, dtDOETo.Value, (getDropDownListValue(ddlCompany, "Company")!="- ทั้งหมด -"? getDropDownListValue(ddlCompany, "Company"):""), (cbHeavyMetal.Checked ? false : true));
                    break;
                case "Book":
                    dt = clsTempData.getPatientMobileByBookCreate(dtDOEFrom.Value, dtDOETo.Value, (getDropDownListValue(ddlCompany, "BookCreate")!="- ทั้งหมด -"? getDropDownListValue(ddlCompany, "BookCreate"):""), (cbHeavyMetal.Checked ? false : true));
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
                        if (dt.Rows[i]["DateRegis"] != DBNull.Value)
                        {
                            dt.Rows[i]["Summary"] = "ค้างคืนเอกสาร";
                            dt.Rows[i]["Remark"] = "";
                        }
                        else
                        {
                            dt.Rows[i]["Summary"] = "ตรวจแล้ว-มีค้างตรวจ";
                            dt.Rows[i]["Remark"] = dt.Rows[i]["ProgramPending"];
                        }
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