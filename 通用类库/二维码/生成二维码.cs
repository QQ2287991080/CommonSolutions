using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;

namespace 通用类库.二维码
{
    public class 生成二维码
    {
        #region 生成方法
        public HttpResponseMessage GenerateHousingEstateQRCode(string Code)
        {

            try
            {
                string qrCodeContent = Code;
                //string name = "";
                string folderName = "用户二维码";
                int qrCodeBitSize = 520; // 二维码图片尺寸
                int qrCodeBitMargin = 12; // 二维码图片边界尺寸

                //int textBitWidth = 600; // 文字图片宽度
                //int textBitHeight = 80; // 文字图片高度

                int bitWidth = 600; // 新画布宽度
                int bitHeight = 600; // 新画布长度
                int qrCodeBitMarginTopInBit = 30; // 二维码图片在新画布中的顶部边界尺寸
                                                  //int textBitMarginTopInBit = 50; // 文字图片在新画布中的顶部边界尺寸
                                                  // 二维码图片
                var qrCodeBit = CreateQRCode(qrCodeContent, qrCodeBitSize, qrCodeBitMargin);

                //// 文字图片
                //var textBit = QRCode.CreateText(textBitWidth, textBitHeight, name, "黑体", false, 24);
                // 创建新画布
                Bitmap newBit = new Bitmap(bitWidth, bitHeight, PixelFormat.Format32bppRgb);
                // 设置画布背景为白色
                for (int y = 0; y < bitHeight; y++)
                {
                    for (int x = 0; x < bitWidth; x++)
                    {
                        newBit.SetPixel(x, y, Color.White);
                    }
                }

                // 在画布中绘制二维码
                Graphics qrCodeGraphics = Graphics.FromImage(newBit);
                var qrCodeGraphicsSize = qrCodeBitSize + (2 * qrCodeBitMargin); // 二维码图片真实尺寸 = 二维码图片尺寸 + 2个边界尺寸
                var qrCodeGraphicsMarginX = (bitWidth - qrCodeGraphicsSize) / 2; // 横向二维码边界尺寸 = (新画布宽度 - 二维码图片真实尺寸) / 2
                qrCodeGraphics.DrawImage(qrCodeBit, qrCodeGraphicsMarginX, qrCodeBitMarginTopInBit, qrCodeGraphicsSize, qrCodeGraphicsSize);
                qrCodeGraphics.Dispose();

                // 在画布中绘制文字
                //Graphics textGraphics = Graphics.FromImage(newBit);
                //var textGraphicsMarginX = (bitWidth - textBitWidth) / 2; // 横向文字边界尺寸 = (新画布宽度 - 文字图片宽度) / 2
                //textGraphics.DrawImage(textBit, textGraphicsMarginX, textBitMarginTopInBit, textBitWidth, textBitHeight);
                //textGraphics.Dispose();

                // 文件存储至本地
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string file_path = AppDomain.CurrentDomain.BaseDirectory + "/二维码/" + folderName + "/";
                string codeUrl = file_path + filename + ".png";

                //根据文件名称，自动建立对应目录
                if (!System.IO.Directory.Exists(file_path))
                {
                    System.IO.Directory.CreateDirectory(file_path);
                }
                MemoryStream ms = new MemoryStream();
                newBit.Save(ms, ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);
                //newBit.Save(codeUrl);//保存图片
                //FileStream fs = new FileStream(codeUrl, FileMode.Open, FileAccess.Read);
                //byte[] length = new byte[fs.Length];
                //fs.Read(length, 0, (int)fs.Length);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "" + filename + ".png"
                };
                return response;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        #endregion
        #region 生成二维码图片
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="size">尺寸（二维码是正方形）</param>
        /// <param name="margin">边界尺寸</param>
        /// <returns></returns>
        public static Bitmap CreateQRCode(string content, int size, int margin = 0)
        {
            // 生成二维码对象
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder(); // 二维码对象
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; // 模式
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H; // 误差校正水平
            qrCodeEncoder.QRCodeScale = 10; // 精细程度
            qrCodeEncoder.QRCodeVersion = 0; // 版本



            // 二维码对象存储为Image
            Image image = qrCodeEncoder.Encode(content);

            // 生成新的画布
            int resWidth = size + 2 * margin;
            int resHeight = size + 2 * margin;
            Bitmap newBit = new Bitmap(resWidth, resHeight, PixelFormat.Format32bppRgb);

            // 设置画布背景为白色
            for (int y = 0; y < resHeight; y++)
            {
                for (int x = 0; x < resWidth; x++)
                {
                    newBit.SetPixel(x, y, Color.White);
                }
            }

            // 在画布中指定区域绘制图形
            Graphics graphics = Graphics.FromImage(newBit);
            graphics.DrawImage(image, margin, margin, size, size);
            graphics.Dispose();

            return newBit;
        }
        #endregion
        #region 生成文字图片
        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="hight">高度</param>
        /// <param name="text">文字内容</param>
        /// <param name="fontFamliy">字体</param>
        /// <param name="isBold">文字是否加粗</param>
        /// <param name="fontSize">文字尺寸</param>
        /// <returns></returns>
        public static Bitmap CreateText(int width, int hight, string text, string fontFamliy, bool isBold, int fontSize)
        {
            Font font = new Font(fontFamliy, fontSize, (isBold ? FontStyle.Bold : FontStyle.Regular));

            // 配置文字规则
            StringFormat format = new StringFormat(StringFormatFlags.NoClip); // 设置文字内容超出矩形宽度后自动换行
            format.Alignment = StringAlignment.Center; // 设置左右居中
            format.LineAlignment = StringAlignment.Center; // 设置上下居中

            // 生成新的画布
            Bitmap newBit = new Bitmap(width, hight);

            // 生成存储文字的矩形区域
            RectangleF rect = new RectangleF(0, 0, width, hight);
            // 设置文字颜色
            SolidBrush brush = new SolidBrush(Color.Black);

            // 在画布中指定区域绘制文字
            Graphics graphics = Graphics.FromImage(newBit);
            graphics.DrawString(text, font, brush, rect, format);
            graphics.Dispose();

            return newBit;
        }
        #endregion
    }
}
