using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace TrainOne.Lib.Common
{
    public class NpoiHelper
    {
        //将数据填充到Excel（导出）
        public static MemoryStream DataSetToExcel(DataSet ds)
        {
            if (ds == null || ds.Tables ==null)
                return null;

            IWorkbook workbook = new HSSFWorkbook();//创建workbook
            int tableCount = ds.Tables.Count; //数据集dataset存在的datatable的个数
            for (int t = 0; t < tableCount; t++) //循环datatable
            {
                var sheet = workbook.CreateSheet($"sheet{t}"); //创建sheet,根据索引命名方式来命名sheet

                var dt = ds.Tables[t]; //得到datatable
                int rows = dt.Rows.Count; //得到datatable的行数
                int cols = dt.Columns.Count; //得到datatable的列数
                for (int r = 0; r < rows; r++) //循环datatable的行数（表格行数）
                {
                    var row = sheet.CreateRow(r); //在该sheet里创建每行
                    for (int c = 0; c < cols; c++) //循环datatable的列数
                    {
                        var col = row.CreateCell(c); //在该行中创建每列
                        col.SetCellValue(Convert.ToString(dt.Rows[r][c])); //填充该单元格的值（值是对应datatable里的该行该列）
                    }
                }
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms); //将workbook写入内存流

            return ms;
        }
    }
}
