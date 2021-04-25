using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TrainOne.Lib.Common
{
    public class NPOIExcel
    {
        /*
         * 作用：将DataTable数据表转Excel的方法，数据存在内存中，返回内存流
         * 1、创建workbook
         * 2、用workbook创建sheet
         * 3、用sheet创建row
         * 4、用row创建cell
         * 5、给这个cell设置值
         * 6、将整个workbook写入流         
         * 
         * ***/
        public System.IO.MemoryStream DataTableToExcel(DataTable dt)
        {
            int rows = dt.Rows.Count; //数据表格的总行数
            int cols = dt.Columns.Count; //数据表格的总列数

            //1、创建book对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //2、创建sheet对象
            var sheet = book.CreateSheet("sheet名称");

            //3、创建row（行）对象
            //sheet.CreateRow(0) 
            for (int r = 0; r < rows; r++)
            {
                var row = sheet.CreateRow(r);

                for (int c = 0; c < cols; c++)
                {
                    //4、创建col（列）对象
                    var col = row.CreateCell(c);
                    //5、为这行这列填充数据
                    col.SetCellValue(Convert.ToString(dt.Rows[r][c]));
                }
            }

            //6、将整个book写入流
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);

            return ms;
        }

        /*
         * 作用：将Excel文件的数据存放在DataTable  （注：仅限该excel中只有一个sheet）
         * 1、读取excel文件（用流的形式），创建workbook
         * 2、读取该workbook中的sheet
         *      2.1)获取该sheet中的所有行数
         * 3、读取该sheet中的row
         *      3.1)获取该行中的所有列
         *      3.2)每个row中创建一个datatable行
         * 4、读取该row中的cell
         * 
         * ***/
        public DataTable ExcelToDataTable(string filename, bool haveHead)
        {
            DataTable dt = new DataTable(); //默认导出只有一个sheet才能这样，要不然就得用dataset
            IWorkbook book;
            var exten = System.IO.Path.GetExtension(filename);
            if (!exten.ToLower().Equals(".xls") && !exten.ToLower().Equals(".xlsx"))
            {
                //不是excel文件，返回提示
                return null;
            }

            using (System.IO.Stream rem = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                //1、读取excel  (版本不一样)
                if (exten.ToLower().Equals(".xls"))
                {
                    book = new NPOI.HSSF.UserModel.HSSFWorkbook(rem);
                }
                else
                {
                    book = new NPOI.XSSF.UserModel.XSSFWorkbook(rem);
                }

                //2、读取sheet
                int sheetNumber = book.NumberOfSheets; //几个sheet
                for (int s = 0; s < sheetNumber; s++)
                {
                    //获取具体的sheet
                    var sheet = book.GetSheetAt(s);

                    var _hrow = sheet.GetRow(0);
                    int _hcols = _hrow.Cells.Count;
                    //表头信息  (创建一个datatable)
                    for (int i = 0; i < _hcols; i++)
                    {
                        DataColumn dc = new DataColumn();
                        if (haveHead)
                        {
                            dc.ColumnName = _hrow.GetCell(i).StringCellValue;
                        }
                        else
                        {
                            dc.ColumnName = i.ToString();
                        }
                        dt.Columns.Add(dc);
                    }

                    //3、读取row
                    int rows = sheet.LastRowNum;//这个sheet的总行数  (#########  获取数据的时候，要注意是否有表头，才决定从0行开始  ###########)

                    //数据行
                    for (int r = 0; r < rows; r++)
                    {
                        var row = sheet.GetRow(haveHead ? r + 1 : r); //获取具体的row

                        int cols = row.Cells.Count; //该行的总列数

                        //创建Datatable的行
                        DataRow dr = dt.NewRow();

                        for (int c = 0; c < cols; c++)
                        {
                            //4、获取该行中的列
                            var cell = row.GetCell(c);

                            string v = "";
                            //excel列中的类型（最好在excel中设置的值都为字符串类型）
                            switch(cell.CellType)
                            {
                                case CellType.Blank:
                                    break;
                                case CellType.Boolean:
                                    v = cell.BooleanCellValue.ToString();
                                    break;
                                case CellType.String:
                                    v = cell.StringCellValue;
                                    break;
                                case CellType.Unknown:
                                    break;
                                default:
                                    break;
                            }

                            v = cell.StringCellValue; //获取列的值
                            dr[c] = v;
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;

        }
    }
}
