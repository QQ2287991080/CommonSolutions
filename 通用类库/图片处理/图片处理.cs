using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.图片处理
{
    public class 图片处理
    {
        #region 图片压缩
        /// <summary>
        /// 原图压缩
        /// </summary>
        /// <param name="image"></param>
        public byte[] Origin(string savePath, string url)
        {
            Image _image = Image.FromFile(savePath);
            OrientationImage(_image);//去除旋转属性
            int originalwidth = _image.Width;//图片宽度
            int originalheight = _image.Height;//图片高度
            Bitmap bmpimage = new Bitmap(originalwidth, originalheight);//初始化System.Drawing的一个新实例。指定的位图类大小。
            Graphics gf = Graphics.FromImage(bmpimage);//创建一个新的System.Drawing。来自指定系统的图形。
            // 获取或设置此系统的呈现质量。
            gf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //获取或设置绘制到此系统的合成图像的呈现质量。
            gf.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            //获取或设置与此系统关联的插值模式。
            gf.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //初始化System.Drawing的一个新实例。指定的矩形类地点和大小。
            Rectangle rect = new Rectangle(0, 0, originalwidth, originalheight);
            //绘制指定系统的指定部分。指定位置的图像位置和指定的大小。
            gf.DrawImage(_image, rect, 0, 0, originalwidth, originalheight, GraphicsUnit.Pixel);
            MemoryStream ms = new MemoryStream();
            bmpimage.Save(ms, ImageFormat.Jpeg);
            bmpimage.Dispose();
            var arr = ms.ToArray();
            return arr;
        }


        /// <summary>
        /// 正方形
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public byte[] LimitImg(string savePath)
        {
            Image _image = Image.FromFile(savePath);
            int sW, sH;
            int dHeight = _image.Width / 3, dWidth = _image.Height / 3;
            //按比例缩放  
            Size tem_size = new Size(_image.Width, _image.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)//如果图片宽度大于指定长度或宽度
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))//如果图片宽度*指定高度>图片宽度*指定宽度
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap bmpimage = new Bitmap(dWidth, dWidth);//初始化System.Drawing的一个新实例。指定的位图类大小。
            Graphics gf = Graphics.FromImage(bmpimage);//创建一个新的System.Drawing。来自指定系统的图形。
            gf.Clear(Color.White);
            // 获取或设置此系统的呈现质量。
            gf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //获取或设置绘制到此系统的合成图像的呈现质量。
            gf.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            //获取或设置与此系统关联的插值模式。
            gf.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //初始化System.Drawing的一个新实例。指定的矩形类地点和大小。
            Rectangle rect = new Rectangle((dWidth - sW) / 2, (dWidth - sH) / 2, sW, sH);
            //绘制指定系统的指定部分。指定位置的图像位置和指定的大小。
            gf.DrawImage(_image, rect, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel);
            gf.Dispose();
            MemoryStream ms = new MemoryStream();
            bmpimage.Save(ms, ImageFormat.Jpeg);
            bmpimage.Dispose();
            //Save(ms, savePath);
            var arr = ms.ToArray();
            return arr;
        }

        #endregion
        #region 去除图片的旋转属性
        /// <summary>
        /// 图片移除有旋转过的属性值
        /// </summary>
        /// <param name="image">Image</param>
        public static void OrientationImage(Image image)
        {
            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
            {
                var orientation = (int)image.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                image.RemovePropertyItem(274);
            }
        }
        #endregion
        #region 合成图片(纵向)
        public void CombineImages(List<string> imgPahts, string pdfImg)
        {
            System.Drawing.Bitmap finalImage = null;
            int width = 0;
            int height = 0;
            //转换图片
            List<Image> list = new List<Image>();
            foreach (var p in imgPahts)
            {
                list.Add(Image.FromFile(p));
            }
            foreach (var x in list)
            {
                //确定画布的长宽//当前是纵向合成图片，横向则相反
                height += x.Height;
                width = x.Width > width ? x.Width : width;
            }
            //创建新画布
            finalImage = new System.Drawing.Bitmap(width, height);
            //绘制图像
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
            {
                //设置背景颜色
                g.Clear(System.Drawing.Color.FromArgb(232, 232, 232));
                //初始长度，叠加确认不同图片的在pdf的位置
                int offset = 0;
                foreach (System.Drawing.Bitmap imageddd in list)
                {
                    //g.DrawImage(imageddd,
                    //  new System.Drawing.Rectangle(offset, 0, imageddd.Width, imageddd.Height));
                    g.DrawImage(imageddd,
                         new System.Drawing.Rectangle(0, offset, imageddd.Width, imageddd.Height));
                    offset += imageddd.Height + 10;
                }
            }
            finalImage.Save(pdfImg, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        #endregion
        #region 图片添加文字水印
        public void 图片文字水印()
        {
            Console.WriteLine("开始导出");
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string image1 = basePath + "ceshi.jpg";
            string image2 = basePath + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
            System.Drawing.Image image = System.Drawing.Image.FromFile(image1);
            var _width = image.Width / 2;
            var _height = image.Height / 2;
            //添加文字水印
            //18款禅静床垫
            string str = "Length长度";//M18CJD12719618S|MP-191011|00014
            var strSize = str.Length * 50; //字符串的长度
            using (FileStream fs = new FileStream(image1, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                MemoryStream ms = new MemoryStream(bytes);
                Bitmap map = new Bitmap(ms);
                Graphics graphics = Graphics.FromImage(map);
                graphics.ResetTransform();//重置图像
                graphics.TranslateTransform(0, image.Height);//设置旋转中心（图片的中心点）
                var cSqrt = Math.Pow(_width, 2) + Math.Pow(_height, 2);//a平方+b平方=c平方
                var c = Math.Sqrt(cSqrt);//对角线的长度
                var cos = (_width / c) * (180 / Math.PI);//获取余弦值
                graphics.RotateTransform(-float.Parse((90 - cos).ToString()));//设置图片旋转角度，顺时针
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                Font font = new Font("Source Han Sans CN Light", 50);
                //水印个数解析
                var x = c / strSize;
                var drawCount = (int)Math.Ceiling(x);//获得需要画水印得个数
                                                     //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(100, 193, 143, 8));
                SolidBrush solidBrush = new SolidBrush(Color.White);
                for (int i = 0; i <= drawCount; i++)
                {
                    graphics.DrawString(str, font, solidBrush, (strSize * 2) * i + 100, 0);
                }
                map.Save(image2, ImageFormat.Jpeg);
            }
            Console.WriteLine("导出成功！");
        }
        #endregion
    }
}
