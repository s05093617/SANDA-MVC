using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Music_Station0730.Helpers
{
    public class ExcelFactory
    {
        public void ExportExcel(string fileName, XSSFWorkbook workbook, HttpResponseBase response)
        {
            response.Clear();
            // 產生 Excel 資料流
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            // 設定強制下載標頭
            //response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileName + ".xlsx"));
            response.AddHeader("Content-Disposition",
                string.Format("attachment; filename=" +
                              System.Web.HttpUtility.UrlEncode("" + fileName + "" + ".xlsx",
                                  System.Text.Encoding.UTF8)));
            //response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            // 輸出檔案
            response.BinaryWrite(ms.ToArray());
            workbook.Close();
            ms.Close();
            ms.Dispose();
            response.End();
        }

        //1.匯出檔名 2.DataTable 3.樣版路徑 4.Response
        public void ExportExcel(string fileName, DataTable dt, string path, HttpResponseBase response)
        {
            FileStream file = new FileStream(HostingEnvironment.MapPath(path), FileMode.Open, FileAccess.Read);
            XSSFWorkbook workbook = DataTableToWorkbook(new XSSFWorkbook(file), dt);

            response.Clear();
            // 產生 Excel 資料流
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            // 設定強制下載標頭
            //response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileName + ".xlsx"));
            response.AddHeader("Content-Disposition",
                string.Format("attachment; filename=" +
                              System.Web.HttpUtility.UrlEncode("" + fileName + "" + ".xlsx",
                                  System.Text.Encoding.UTF8)));
            //response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            // 輸出檔案
            response.BinaryWrite(ms.ToArray());
            workbook.Close();
            ms.Close();
            ms.Dispose();
            response.End();
        }

        public XSSFWorkbook DataTableToWorkbook(XSSFWorkbook Tmp, DataTable SourceTable)
        {
            XSSFWorkbook workbook = Tmp;

            //XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet();
            //XSSFSheet sheet = (XSSFSheet)workbook.GetSheet(SheetName);
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
            //XSSFRow headerRow = (XSSFRow)sheet.CreateRow(0);

            XSSFCellStyle cs = (XSSFCellStyle)workbook.CreateCellStyle();
            cs.WrapText = true; // 設定換行
            cs.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top; //文字對齊方式
            int rowIndex = 1;
            char[] ch = new char[] { '\n' };
            foreach (DataRow row in SourceTable.Rows)
            {
                XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    string[] str = row[column].ToString().Split(ch); //取得行數
                    if (row[column].ToString().Contains("\n")) //檢查是否有換行符號
                    {
                        dataRow.Cells[column.Ordinal].CellStyle = cs;
                        dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20; //計算Cell的高度
                    }

                    sheet.AutoSizeColumn(column.Ordinal); //自動調整欄寬
                }

                rowIndex++;
            }

            return workbook;
        }

        //1.匯出檔名 2.DataTable 3.樣版路徑 4.Response
        public void ExportLecturerInfoExcel(string fileName, DataTable dt, DataTable dt2, DataTable dt3, DataTable dt4,
            DataTable dt5, DataTable dt6, DataTable dt7, string path, HttpResponseBase response, int sheetCount)
        {
            FileStream file = new FileStream(HostingEnvironment.MapPath(path), FileMode.Open, FileAccess.Read);
            XSSFWorkbook workbook =
                DataTableLecturerInfoToWorkbook(new XSSFWorkbook(file), dt, dt2, dt3, dt4, dt5, dt6, dt7, 7);

            response.Clear();
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);

            response.AddHeader("Content-Disposition",
                string.Format("attachment; filename=" +
                              System.Web.HttpUtility.UrlEncode("" + fileName + "" + ".xlsx",
                                  System.Text.Encoding.UTF8)));
            response.BinaryWrite(ms.ToArray());
            workbook.Close();
            ms.Close();
            ms.Dispose();
        }

        public XSSFWorkbook DataTableLecturerInfoToWorkbook(XSSFWorkbook Tmp, DataTable SourceTable,
            DataTable SourceTable2, DataTable SourceTable3, DataTable SourceTable4, DataTable SourceTable5,
            DataTable SourceTable6, DataTable SourceTable7, int sheetCount)
        {
            XSSFWorkbook workbook = Tmp;

            for (int i = 0; i < sheetCount; i++)
            {
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(i);
                XSSFCellStyle cs = (XSSFCellStyle)workbook.CreateCellStyle();
                cs.WrapText = true;
                cs.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
                int rowIndex = 1;
                char[] ch = new char[] { '\n' };

                switch (i)
                {
                    #region "LecturerInfo"

                    case 0:
                        foreach (DataRow row in SourceTable.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                    #endregion

                    #region "License"

                    case 1:
                        foreach (DataRow row in SourceTable2.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable2.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                    #endregion

                    #region "WorkExperiences"

                    case 2:
                        foreach (DataRow row in SourceTable3.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable3.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                    #endregion

                    #region "AvailableCourses"

                    case 3:
                        foreach (DataRow row in SourceTable4.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable4.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                    #endregion

                    #region "CourseLecturers"

                    case 4:
                        foreach (DataRow row in SourceTable5.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable5.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                    #endregion

                    #region "LecturerTrainnings"

                    case 5:
                        foreach (DataRow row in SourceTable6.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable6.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                    #endregion

                    #region "LecturerTrainnings"

                    case 6:
                        foreach (DataRow row in SourceTable7.Rows)
                        {
                            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in SourceTable7.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                string[] str = row[column].ToString().Split(ch);
                                if (row[column].ToString().Contains("\n"))
                                {
                                    dataRow.Cells[column.Ordinal].CellStyle = cs;
                                    dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                                }

                                sheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex++;
                        }

                        break;

                        #endregion

                }
            }

            return workbook;
        }

        //Excel匯出  通式
        public void ExportExcelMutiple(string fileName, List<DataTable> dtList, string path, HttpResponseBase response,
            int sheetCount)
        {
            FileStream file = new FileStream(HostingEnvironment.MapPath(path), FileMode.Open, FileAccess.Read);
            XSSFWorkbook workbook = DataTableMutipleToWorkbook(new XSSFWorkbook(file), dtList);

            response.Clear();
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);

            response.AddHeader("Content-Disposition",
                string.Format("attachment; filename=" +
                              System.Web.HttpUtility.UrlEncode("" + fileName + "" + ".xlsx",
                                  System.Text.Encoding.UTF8)));
            response.BinaryWrite(ms.ToArray());
            workbook.Close();
            ms.Close();
            ms.Dispose();
        }



        public XSSFWorkbook DataTableMutipleToWorkbook(XSSFWorkbook Tmp, List<DataTable> dtList)
        {
            XSSFWorkbook workbook = Tmp;
            int idx = 0;
            //dt List集合
            foreach (var dt in dtList)
            {
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(idx);
                XSSFCellStyle cs = (XSSFCellStyle)workbook.CreateCellStyle();
                cs.WrapText = true;
                cs.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
                int rowIndex = 1;
                char[] ch = new char[] { '\n' };
                //key欄位
                foreach (DataRow row in dt.Rows)
                {
                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        string[] str = row[column].ToString().Split(ch);
                        if (row[column].ToString().Contains("\n"))
                        {
                            dataRow.Cells[column.Ordinal].CellStyle = cs;
                            dataRow.HeightInPoints = str.Length * sheet.DefaultRowHeight / 20;
                        }

                        sheet.AutoSizeColumn(column.Ordinal);
                    }

                    rowIndex++;
                }

                idx = idx + 1;
            }

            return workbook;
        }



    }
}