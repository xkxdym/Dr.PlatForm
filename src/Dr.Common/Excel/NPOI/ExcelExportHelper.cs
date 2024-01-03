
#region ExcelExportHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Excel.NPOI
* 类 名 称 ：ExcelExportHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:05:27
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
using NPOI.HSSF.Util;
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
    /// Excel导出帮助类
    /// </summary>
    public class ExcelExportHelper
    {
        private FileStream TemlateFileStream;

        #region 属性

        /// <summary>
        /// Excel操作实体
        /// </summary>
        public IWorkbook ExcelObj { get; set; }

        /// <summary>
        /// 标签替换字典内容 格式:tagname,tagvalue
        /// </summary>
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// 导出的数据源
        /// </summary>
        public DataSet DataSource { get; set; }

        /// <summary>
        /// 是否使用模版进行导出
        /// </summary>
        public bool ExportByTemplate { get; set; }

        /// <summary>
        /// Excel模版路径
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// 模版数据行起始标志位(默认{RowData})
        /// </summary>
        public string TemplateDataRowTag { get; set; } = "{RowData}";

        #endregion

        #region 构造函数

        public ExcelExportHelper() { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="source">数据源</param>
        public ExcelExportHelper(DataSet source) : this(source, null, false, string.Empty)
        { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="source">数据源</param>
        public ExcelExportHelper(DataTable source) : this(source, null, false, string.Empty)
        { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="source">导出的数据源</param>
        /// <param name="tags">模版标签替换数据</param>
        /// <param name="exportByTemplate">是否使用模版进行导出</param>
        /// <param name="templatePath">Excel模版路径</param>
        public ExcelExportHelper(DataTable source, Dictionary<string, string> tags, bool exportByTemplate, string templatePath)
        {
            DataSource = new DataSet();
            DataSource.Tables.Add(source);
            Tags = tags;
            ExportByTemplate = exportByTemplate;
            TemplatePath = templatePath;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="source">导出的数据源</param>
        /// <param name="tags">模版标签替换数据</param>
        /// <param name="exportByTemplate">是否使用模版进行导出</param>
        /// <param name="templatePath">Excel模版路径</param>
        public ExcelExportHelper(DataSet source, Dictionary<string, string> tags, bool exportByTemplate, string templatePath)
        {
            DataSource = source;
            Tags = tags;
            ExportByTemplate = exportByTemplate;
            TemplatePath = templatePath;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <returns>返回Stream</returns>
        public MemoryStream Export()
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                //验证数据
                ValidData();
                //初始化Excel
                InitExcelObj();

                //数据处理
                for (var i = 0; i < DataSource.Tables.Count; i++)
                {
                    if (ExportByTemplate)
                    {
                        //替换标签
                        ReplaceTag(i);
                        //输出数据
                        OutRowData(i);
                    }
                    else
                    {
                        //表头初始化
                        InitHeader(i);
                        //输出数据
                        OutDataRowTwo(i);
                    }
                }
                ExcelObj.Write(stream);
                stream.Seek(0, SeekOrigin.Begin);
            }
            finally
            {
                if (this.TemlateFileStream != null)
                {
                    this.TemlateFileStream.Close();
                }
            }
            return stream;
        }

        /// <summary>
        ///  Excel导出
        /// </summary>
        /// <param name="outFilePath">保存的Excel地址</param>
        public void Export(string outFilePath)
        {
            FileInfo fileInfo = new FileInfo(outFilePath);

            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            using (Stream fileStream = new FileStream(outFilePath, FileMode.Create))
            {
                using (MemoryStream ms = Export())
                {
                    byte[] buff = ms.ToArray();

                    fileStream.Write(buff, 0, buff.Length);
                }
            }
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 数据验证
        /// </summary>
        private void ValidData()
        {
            if (ExportByTemplate && string.IsNullOrEmpty(this.TemplatePath))
            {
                throw new ArgumentNullException(nameof(this.TemplatePath));
            }
            if (DataSource == null || DataSource.Tables.Count == 0)
            {
                throw new ArgumentNullException(nameof(this.DataSource));
            }
        }

        /// <summary>
        /// 初始ExcelObj
        /// </summary>
        private void InitExcelObj()
        {
            if (this.ExportByTemplate)
            {
                //excel模板读取
                TemlateFileStream = new FileStream(this.TemplatePath, FileMode.Open, FileAccess.Read);
                string fileExt = Path.GetExtension(TemplatePath).ToLower();
                if (fileExt == ".xlsx")
                {
                    ExcelObj = new XSSFWorkbook(TemlateFileStream);
                }
                else if (fileExt == ".xls")
                {
                    ExcelObj = new HSSFWorkbook(TemlateFileStream);
                }
                else
                {
                    throw new NotImplementedException(fileExt);
                }
            }
            else
            {
                ExcelObj = new HSSFWorkbook();
            }
        }

        #region 模版
        /// <summary>
        /// 标签替换
        /// </summary>
        /// <param name="sheetIndex"></param>
        private void ReplaceTag(int sheetIndex)
        {
            var sheet = ExcelObj.GetSheetAt(sheetIndex);
            if (Tags != null && sheet != null)
            {
                var rows = sheet.GetRowEnumerator();
                if (rows == null)
                {
                    return;
                }
                //遍历行
                while (rows.MoveNext())
                {
                    //替换指定文本标签 格式:[Text]
                    var item = rows.Current;
                    IRow row = null;
                    if (item is HSSFRow hRow)
                    {
                        row = hRow;
                    }
                    if (item is XSSFRow xRow)
                    {
                        row = xRow;
                    }
                    if (row == null)
                    {
                        continue;
                    }
                    //遍历单元格
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        try
                        {
                            var cell = row.GetCell(i);
                            if (cell == null)
                            {
                                continue;
                            }
                            string cellValue = cell.GetCellValue();

                            if (string.IsNullOrEmpty(cellValue))
                            {
                                continue;
                            }
                            foreach (var key in Tags.Keys)
                            {
                                string replacestr = string.Format("[{0}]", key);
                                if (cellValue.Contains(replacestr))
                                {
                                    cellValue = cellValue.Replace(replacestr, Tags[key]);
                                    cell.SetCellValue(cellValue);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 工作表初始化
        /// </summary>
        private bool InitDataRow(int sheetIndex, out int rowIndex, out IRow row)
        {
            //输出数据行 开始位置标识:{RowData}
            var sheet = ExcelObj.GetSheetAt(sheetIndex);

            //获取数据起始行
            rowIndex = 0;
            row = null;
            var rows = sheet.GetRowEnumerator();

            //是否启用数据行
            bool isDataRow = false;
            while (rows.MoveNext())
            {
                //当前行
                var item = rows.Current;
                if (item is HSSFRow hRow)
                {
                    row = hRow;
                }
                if (item is XSSFRow xRow)
                {
                    row = xRow;
                }
                if (row == null)
                {
                    continue;
                }
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    try
                    {
                        var cell = row.GetCell(i);
                        if (cell == null)
                        {
                            continue;
                        }
                        string cellValue = cell.GetCellValue();

                        if (string.IsNullOrEmpty(cellValue))
                        {
                            continue;
                        }
                        //检查数据行标签位置
                        if (cellValue.Contains(TemplateDataRowTag))
                        {
                            isDataRow = true;
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                if (!isDataRow)
                {
                    rowIndex++;
                }
                else
                {
                    break;
                }
            }
            return isDataRow;
        }

        /// <summary>
        /// 数据输出
        /// </summary>
        /// <param name="sheetIndex"></param>
        private void OutRowData(int sheetIndex)
        {
            if (DataSource != null && ExcelObj.GetSheetAt(sheetIndex) != null)
            {
                //输出数据行 开始位置标识:{RowData}
                var sheet = ExcelObj.GetSheetAt(sheetIndex);
                DataTable dt = DataSource.Tables[sheetIndex];
                int rowIndex;
                IRow row;
                bool isDataRow = InitDataRow(sheetIndex, out rowIndex, out row);

                if (isDataRow)
                {
                    //绘制数据行
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow newRow = row;
                        if (i != 0)
                        {
                            row.CopyRowTo(rowIndex + i);
                            newRow = sheet.GetRow(rowIndex + i);
                        }

                        var dtrow = dt.Rows[i];

                        for (var j = 0; j < dtrow.ItemArray.Length; j++)
                        {
                            var cell = newRow.GetCell(j);
                            var cellValue = dtrow.ItemArray[j].ToString();
                            cell.SetCellValue(cellValue);
                        }
                    }
                }
            }
        }

        #endregion

        #region 非模版
        /// <summary>
        /// 表头初始化
        /// </summary>
        private void InitHeader(int index)
        {
            DataTable dt = DataSource.Tables[index];
            var sheet = string.IsNullOrEmpty(dt.TableName) ? ExcelObj.CreateSheet("Sheet1") : ExcelObj.CreateSheet(dt.TableName);

            ICellStyle headStyle = ExcelObj.CreateCellStyle();
            // 左右居中
            headStyle.Alignment = HorizontalAlignment.Center;
            // 上下居中     
            headStyle.VerticalAlignment = VerticalAlignment.Center;
            //背景颜色
            headStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
            headStyle.FillPattern = FillPattern.SolidForeground;

            //边框
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.TopBorderColor = HSSFColor.Black.Index;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.RightBorderColor = HSSFColor.Black.Index;
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BottomBorderColor = HSSFColor.Black.Index;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.LeftBorderColor = HSSFColor.Black.Index;

            //定义font
            IFont font = ExcelObj.CreateFont();
            font.FontHeightInPoints = 15;
            font.Boldweight = 700;
            font.FontName = "新宋体";
            headStyle.SetFont(font);

            IRow row = sheet.CreateRow(0);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.CellStyle = headStyle;
                cell.SetCellValue(dt.Columns[i].ColumnName);

            }
        }

        /// <summary>
        /// 数据输出
        /// </summary>
        private void OutDataRowTwo(int sheetIndex)
        {
            DataTable dt = DataSource.Tables[sheetIndex];
            var sheet = ExcelObj.GetSheetAt(sheetIndex);

            ICellStyle dataStyle = ExcelObj.CreateCellStyle();
            // 左右居中  
            dataStyle.Alignment = HorizontalAlignment.Center;
            // 上下居中 
            dataStyle.VerticalAlignment = VerticalAlignment.Center;

            //边框
            dataStyle.BorderTop = BorderStyle.Thin;
            dataStyle.TopBorderColor = HSSFColor.Black.Index;
            dataStyle.BorderRight = BorderStyle.Thin;
            dataStyle.RightBorderColor = HSSFColor.Black.Index;
            dataStyle.BorderBottom = BorderStyle.Thin;
            dataStyle.BottomBorderColor = HSSFColor.Black.Index;
            dataStyle.BorderLeft = BorderStyle.Thin;
            dataStyle.LeftBorderColor = HSSFColor.Black.Index;

            //定义font
            IFont font = ExcelObj.CreateFont();
            font.FontHeightInPoints = 15;
            font.FontName = "新宋体";

            dataStyle.SetFont(font);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);

                    cell.CellStyle = dataStyle;
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

        }

        #endregion

        #endregion
    }
}
