
#region ExcelCommon 声明

/**************************************************************
* 命名空间 ：Dr.Common.Excel.NPOI
* 类 名 称 ：ExcelCommon
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:05:16
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Excel.NPOI
{
    /// <summary>
    /// NPOI Excel 通用帮助类
    /// </summary>
    public static class ExcelCommon
    {
        /// <summary>
        /// 获取单元格数据
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string GetCellValue(this ICell cell)
        {
            var result = string.Empty;

            try
            {
                if (cell == null)
                {
                    return result;
                }

                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            result = cell.DateCellValue.ToString();
                        }
                        else
                        {
                            result = cell.NumericCellValue.ToString();
                        }
                        break;
                    case CellType.String:
                        result = cell.StringCellValue;
                        break;
                    case CellType.Formula:
                        try
                        {
                            HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                            e.EvaluateInCell(cell);
                            result = cell.ToString();
                        }
                        catch
                        {
                            try
                            {
                                result = cell.StringCellValue;
                            }
                            catch
                            { }
                        }
                        break;
                    case CellType.Boolean:
                        result = cell.BooleanCellValue.ToString();
                        break;
                    case CellType.Error:
                        result = cell.ErrorCellValue.ToString();
                        break;
                    case CellType.Blank:
                        result = string.Empty;
                        break;
                    case CellType.Unknown:
                    default:
                        result = cell.ToString();
                        break;
                }

            }
            catch
            { }
            return result;
        }
    }
}
