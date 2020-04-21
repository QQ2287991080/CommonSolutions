using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.文档处理
{
    public class PdfDocs
    {
        #region 使用PdfSharp图片转pdf
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfPath">pdf的地址</param>
        /// <param name="pdfImg">图片的地址</param>
        public void CreatePDF(string pdfPath, string pdfImg)
        {
            //测试路径
            System.Drawing.Image image4 = System.Drawing.Image.FromFile(pdfImg);
            using (PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument())
            {
                PdfSharp.Pdf.PdfPage ss = new PdfSharp.Pdf.PdfPage(pdf);
                double w = image4.Width * 0.75;
                double h = image4.Height * 0.75;
                ss.Width = new XUnit(w);
                ss.Height = new XUnit(h);
                pdf.AddPage(ss);
                PdfSharp.Pdf.PdfPage page = pdf.Pages[0];
                XGraphics gfx = XGraphics.FromPdfPage(page);
                // Draw background
                gfx.DrawImage(PdfSharp.Drawing.XImage.FromFile(pdfImg), 0, 0);
                pdf.Save(pdfPath);
                pdf.Close();
            }
            image4.Dispose();
        }
        #endregion
    }
}
