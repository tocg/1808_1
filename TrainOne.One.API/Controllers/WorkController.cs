using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TrainOne.Lib.Common;

namespace TrainOne.One.API.Controllers
{
    /// <summary>
    /// 工作日志文件
    /// </summary>
    [Route("work")]
    [ApiController]
    public class WorkController : ControllerBase
    {

        IWebHostEnvironment _host;
        public WorkController(IWebHostEnvironment host)
        {
            _host = host;
        }

        /// <summary>
        /// 上传工作进度文件
        /// </summary>
        /// <returns></returns>
        [Route("upload"), HttpPost]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files;

            if (file == null)
                return Ok(new { state = false, msg = "未选择文件" });

            var temp = "/temp/";
            var path = _host.WebRootPath;
            path = $"{path}{temp}";

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            using (var sem = System.IO.File.Create($"{path}{file[0].FileName}"))
            {
                await file[0].CopyToAsync(sem);
            }

            var dt = ExcelToDataTable($"{path}{file[0].FileName}");

            return Ok(dt);
        }

        private List<WorkName> ExcelToDataTable(string filename)
        {
            List<WorkName> list = new List<WorkName>();
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


                var sheet = book.GetSheetAt(0);

                int rows = sheet.ActiveCell.Row;

                //数据行
                for (int r = 0; r < rows; r++)
                {
                    WorkName work = new WorkName();
                    var row = sheet.GetRow(r + 2);

                    for (int c = 0; c < 3; c++)
                    {
                        var cell = row.GetCell(c + 1);

                        string v = "";
                        //excel列中的类型（最好在excel中设置的值都为字符串类型）
                        switch (cell.CellType)
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
                            default:
                                v = cell.StringCellValue;
                                break;
                        }
                        

                        if (c == 0)
                            work.Name = v;
                        else if (c == 1)
                            work.Content = v;
                        else if (c == 2)
                            work.Flag = v;
                    }

                    list.Add(work);
                }
            }
            return list;
        }
    }

    class WorkName
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Flag { get; set; }
    }
}
