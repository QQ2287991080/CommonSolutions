using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace 通用类库.文件处理.ffmpeg专区
{
    public class FfmpegHelper
    {
        #region arm转mp3
        /// <summary>
        /// arm网络路径
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string GetMP3(string Url)
        {
            //Arm文件的虚拟路径
            //string arm = "Arm文件的虚拟路径";
            //Arm文件的物理路径
            string arm2 = "Arm文件的物理路径";
            if (!Directory.Exists(arm2)) Directory.CreateDirectory(arm2);
            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //arm创建本地文件写入流
            Stream stream = new FileStream(arm2, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            //MP3文件的虚拟路径
            //string mp3 = "MP3文件的虚拟路径";
            //mp3的物理路径
            var mp3Path = "mp3的物理路径";
            if (!Directory.Exists(mp3Path)) Directory.CreateDirectory(mp3Path);
            //转换mp3
            var fullpath = ConvertToMp3(arm2, mp3Path);
            return fullpath;
        }
        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        private string RunCmd(string c)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo("cmd.exe")
                {
                    RedirectStandardOutput = false,
                    UseShellExecute = false
                };
                Process p = Process.Start(info);
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                p.StandardInput.WriteLine(c);
                p.StandardInput.AutoFlush = true;
                Thread.Sleep(1000);
                p.StandardInput.WriteLine("exit");
                p.WaitForExit();
                string outStr = p.StandardOutput.ReadToEnd();
                p.Close();

                return outStr;
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
        }

        public string ConvertToMp3(string pathBefore, string pathLater)
        {
            string c = HttpContext.Current.Server.MapPath("/ffmpeg/") + @"ffmpeg.exe -i " + pathBefore + " " + pathLater;
            string str = RunCmd(c);
            return str;
        }
        #endregion
    }
}
