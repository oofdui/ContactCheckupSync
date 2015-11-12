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
            dt = clsTempData.getPatientMobile(dtDOEFrom.Value, dtDOETo.Value, clsTempData.getDropDownListValue(ddlCompany, "Company"));
            if (dt != null && dt.Rows.Count > 0)
            {
                btExport.Enabled = true;
                lblDefault.Text = "";
                #region AddColumns
                dt.Columns.Add("Summary", typeof(string));
                dt.Columns.Add("Remark", typeof(string));
                dt.Columns.Add("RemarkCancel", typeof(string));
                #endregion
                #region FillData
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["CountChecklistAll"].ToString()!="0" && dt.Rows[i]["CountChecklistAll"].ToString() == dt.Rows[i]["CountChecklistComplete"].ToString())
                    {
                        dt.Rows[i]["Summary"] = "ตรวจแล้ว";
                        dt.Rows[i]["Remark"] = "ตรวจครบทุกรายการ";
                    }
                    else if (dt.Rows[i]["CountChecklistComplete"].ToString()!="0" && dt.Rows[i]["CountChecklistAll"].ToString()!= dt.Rows[i]["CountChecklistComplete"].ToString())
                    {
                        dt.Rows[i]["Summary"] = "ตรวจแล้ว-มีค้างตรวจ";
                        dt.Rows[i]["Remark"] = dt.Rows[i]["ProgramPending"];
                    }
                    else if (dt.Rows[i]["CountChecklistComplete"].ToString() == "0" && dt.Rows[i]["CountChecklistAll"].ToString()!="0")
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
                string[] columns = { "CountChecklistAll","CountChecklistComplete","CountChecklistCancel","ProgramPending","ProgramCancel"};
                for (int i = 0; i < columns.Length; i++)
                {
                    dt.Columns.Remove(columns[i]);
                }
                dt.AcceptChanges();
                #endregion
                clsGlobal.dtPatient = dt.Copy();
                gvDefault.DataSource = dt;
                lblDefault.Text = string.Format("พบข้อมูลทั้งหมด {0} รายการ", dt.Rows.Count.ToString());
            }
            else
            {
                clsGlobal.dtPatient = null; btExport.Enabled = false;
                lblDefault.Text = "- ไม่พบข้อมูลที่ต้องการ -";
            }
            #endregion
        }
        private void btExport_Click(object sender, EventArgs e)
        {
            var clsTempData = new clsTempData();
            var FileName = clsGlobal.ExecutePathBuilder() + @"Export\"+clsTempData.getDropDownListValue(ddlCompany,"Company")+"_"+DateTime.Now.ToString("dd-MM-yyyy-HH-mm")+".xlsx";
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
                #region Detail
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Detail");
                var rows = 1;
                #region HeaderBuilder
                for (int c = 0; c <= dt.Columns.Count - 1; c++)
                {
                    worksheet.Cells[rows, c + 1].Value = dt.Columns[c].ColumnName;

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
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        worksheet.Cells[rows + r + 1, c + 1].Value = dt.Rows[r][c].ToString();
                        worksheet.Cells[rows + r + 1, c + 1].Style.Font.Name = "Tahoma";
                        worksheet.Cells[rows + r + 1, c + 1].Style.Font.Size = 11;
                    }
                }

                #endregion
                #region ResizeColumn
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    worksheet.Column(c + 1).AutoFit();
                }
                #endregion
                #endregion
                #region LabSummary
                ExcelWorksheet worksheetLabSummary = package.Workbook.Worksheets.Add("LabSummary");
                dt = null; rows = 1;
                 dt = clsTempData.getLabSummary();
                if(dt!=null && dt.Rows.Count > 0)
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
                #endregion
                #region LabDetail
                ExcelWorksheet worksheetLabDetail = package.Workbook.Worksheets.Add("LabDetail");
                dt = null; rows = 1;
                dt = clsTempData.getLabDetail(dtDOEFrom.Value,dtDOETo.Value,clsTempData.getDropDownListValue(ddlCompany,"Company"));
                if(dt!=null && dt.Rows.Count > 0)
                {
                    #region HeaderBuilder
                    for (int c = 0; c <= dt.Columns.Count - 1; c++)
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
                        for (int c = 0; c < dt.Columns.Count; c++)
                        {
                            if(dt.Columns[c].ColumnName=="AcceptDateBlood" ||
                                dt.Columns[c].ColumnName == "AcceptDateUrine" ||
                                dt.Columns[c].ColumnName == "AcceptDateStool" ||
                                dt.Columns[c].ColumnName == "AcceptDateHeavyMetal")
                            {
                                worksheetLabDetail.Cells[rows + r + 1, c + 1].Value = (dt.Rows[r][c].ToString()==""?"ยังไม่เก็บ Specimens": dt.Rows[r][c].ToString());
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
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        worksheet.Column(c + 1).AutoFit();
                    }
                    #endregion
                }
                #endregion
                package.Save();
                DialogResult dr = MessageBox.Show("Export Seccessful!" + Environment.NewLine+ Environment.NewLine + FileName+ Environment.NewLine + Environment.NewLine + "ต้องการดูไฟล์คลิก Yes", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
    }
}
