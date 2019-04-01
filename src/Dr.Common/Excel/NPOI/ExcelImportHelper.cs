
#region ExcelImportHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Excel.NPOI
* 类 名 称 ：ExcelImportHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:05:36
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Excel.NPOI
{
    /// <summary>
    /// Excel 导入帮助类
    /// </summary>
    public class ExcelImportHelper
    {
        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="filePath">Excel路径</param>
        /// <param name="headers">表头所在行索引 (sheetIndex,headerIndex) </param>
        /// <returns></returns>
        public static DataSet Import(string filePath, Dictionary<int, int> headers = null)
        {
            DataSet ds = new DataSet();

            IWorkbook workbook;
            var fileExtion = Path.GetExtension(filePath).ToLower();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fileExtion == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileExtion == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    throw new NotImplementedException(fileExtion);
                }
                if (workbook == null)
                {
                    return null;
                }

                int sheetCount = workbook.NumberOfSheets;

                for (int sc = 0; sc < sheetCount; sc++)
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        ISheet sheet = workbook.GetSheetAt(sc);
                        if (sheet == null)
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(sheet.SheetName))
                        {
                            dt.TableName = sheet.SheetName;
                        }

                        var headerIndex = sheet.FirstRowNum;

                        if (headers != null && headers.ContainsKey(sc))
                        {
                            headerIndex = headers[sc];
                        }

                        IRow header = sheet.GetRow(headerIndex);
                        if (header == null)
                        {
                            continue;
                        }
                        List<int> columns = new List<int>();
                        for (int i = 0; i < header.LastCellNum; i++)
                        {
                            var value = header.GetCell(i).GetCellValue();
                            if (string.IsNullOrEmpty(value))
                            {
                                value = "C_" + i.ToString();
                            }
                            dt.Columns.Add(new DataColumn(value.ToString()));
                            columns.Add(i);
                        }

                        for (int i = headerIndex + 1; i <= sheet.LastRowNum; i++)
                        {
                            try
                            {
                                DataRow row = dt.NewRow();
                                var flag = false;
                                foreach (int j in columns)
                                {
                                    try
                                    {
                                        var excelRow = sheet.GetRow(i);
                                        if (excelRow == null)
                                        {
                                            continue;
                                        }
                                        var value = excelRow.GetCell(j).GetCellValue();
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            flag = true;
                                        }
                                        row[j] = value;
                                    }
                                    catch
                                    {
                                        row[j] = string.Empty;
                                    }
                                }
                                if (flag)
                                {
                                    dt.Rows.Add(row);
                                }
                            }
                            catch
                            { }
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            ds.Tables.Add(dt);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return ds;
        }
    }
}
