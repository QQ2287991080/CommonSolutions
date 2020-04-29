using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.文档处理
{
    /// <summary>
    /// EXCEl  使用Syncfusion
    /// https://help.syncfusion.com/file-formats/xlsio/create-read-edit-excel-files-in-asp-net-mvc-c-sharp
    /// </summary>
    public class ExcelDocs
    {
        #region Web导出Excel
        public HttpResponseMessage Export()
        {
            var list = new List<TestExcle>();
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;
            //创建一个工作簿
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet sheet = workbook.Worksheets[0];
            //导出得表头
            string[] title = new string[] { "序号", "时间", "产品编号", "产品名称", "品牌类型" };

            //设置标题单元格样式
            var cellStyle = sheet.Range[1, 1, 1, title.Length].CellStyle;
            cellStyle.Font.Bold = true;//字体加粗
            cellStyle.Font.Size = 12;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Color = Color.FromArgb(42, 118, 189);
            sheet.Range[1, 1, 1, title.Length].RowHeight = 35;

            #region 黑色边框
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Black;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeRight].Color = ExcelKnownColors.Black;
            sheet.Range[1, 1, 1, title.Length].CellStyle.Borders[ExcelBordersIndex.EdgeLeft].Color = ExcelKnownColors.Black;
            #endregion
            #region 设置整个Excel单元格样式
            //设置整个单元格样式
            var sheetStyle = sheet.Range[1, 1, list.Count + 1, title.Length].CellStyle;
            sheetStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;//单元格居中
            sheetStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;//单元格居中
            #endregion
            //设置列宽
            sheet.Range[1, 2].ColumnWidth = 20;//时间
            sheet.Range[1, 3].ColumnWidth = 25;//产品编号
            sheet.Range[1, 4].ColumnWidth = 20;//产品名称
            //填充值
            for (int i = 1; i <= title.Length; i++)
            {
                sheet.Range[1, i].Value = title[i - 1];
            }
            //将数据放入单元格
            int index = 2;
            foreach (var item in list)
            {
                sheet.Range[index, 1].Value2 = index - 1;//序号
                sheet.Range[index, 2].DateTime = item.LastTime;//时间
                sheet.Range[index, 2].NumberFormat = "yyyy-MM-dd HH:mm:ss";
                sheet.Range[index, 3].Value2 = item.ProductCode;//产品编号
                sheet.Range[index, 4].Value2 = item.ProductName;//产品名称
                sheet.Range[index, 5].Value2 = "";//产品类型
                index++;
            }

            //excel文件名
            string excelName = "WeChatUserDetails" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Seek(0, SeekOrigin.Begin);
            response.Content = new StreamContent(ms);//http返回内容
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");//设置http的Content-Type
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = excelName
            };
            excelEngine.Dispose();
            return response;
        }
        public struct TestExcle
        {
            public DateTime LastTime { get; set; }
            public string ProductName { get; set; }
            public string ProductCode { get; set; }
        }
        #endregion

        #region Web导入Excel

        #endregion
    }
}
