using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.文档处理
{
    public class CSVDosc
    {
        #region 导出
        public static bool Export(string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    string header = "";//定义表头和表体
                    header += $"1,2,3,\"" + DateTime.Now + "\"\r\n";
                    header += $"1,2,3,4,\"" + DateTime.Now + "\"," + "" + "\r\n";
                    sw.Write(header);
                    sw.Close();
                    fs.Close();
                    Console.WriteLine("成功");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region 导入
        public static DataTable Import(string filepath)
        {
            DataTable dt = new DataTable();
            //Encoding encoding = CommonHelper.GetType(filepath);//ASCII
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                CreateColumn(dt);
                StreamReader sr = new StreamReader(filepath, Encoding.Default);
                //表头
                string[] header = null;
                //表体
                string[] body = null;
                //读取每一行
                string strLine = "";
                int count = 0;//列数
                bool IsFirst = true;//标识是否读取第一行
                try
                {
                    //逐行读取数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        strLine = strLine.Replace("#", "").Replace("\t", ",").Replace("\"", "");
                        //去除多余字符
                        if (IsFirst == false)
                        {

                            header = strLine.Split(',');//列头
                            IsFirst = false;
                            count = header.Length;//列数
                                                  //创建列
                            for (int i = 0; i < count; i++)
                            {
                                DataColumn column = new DataColumn(header[i]);
                                dt.Columns.Add(column);
                            }
                        }
                        else
                        {
                            body = strLine.Split(',');//行数据
                            DataRow row = dt.NewRow();
                            for (int j = 0; j < 8; j++)
                            {
                                row[j] = body[j];
                            }
                            dt.Rows.Add(row);
                        }
                    }
                    sr.Close();
                    fs.Close();
                    Console.WriteLine("返回dt");
                    return dt;
                }
                catch (Exception)
                {
                    return dt;
                }
            }
        }
        /// <summary>
        /// 构建datatable表头
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DataTable CreateColumn(DataTable dt)
        {
            DataColumn column1 = new DataColumn("cPOID");
            dt.Columns.Add(column1);
            DataColumn column2 = new DataColumn("OrderLine");
            dt.Columns.Add(column2);
            DataColumn column3 = new DataColumn("OutTime");
            dt.Columns.Add(column3);
            DataColumn column4 = new DataColumn("Num");
            dt.Columns.Add(column4);
            DataColumn column5 = new DataColumn("Number");
            dt.Columns.Add(column5);
            DataColumn column6 = new DataColumn("cUnitID");
            dt.Columns.Add(column6);
            DataColumn column7 = new DataColumn("Price");
            dt.Columns.Add(column7);
            DataColumn column8 = new DataColumn("OPrice");
            dt.Columns.Add(column8);
            return dt;
        }
        #endregion
    }
}
