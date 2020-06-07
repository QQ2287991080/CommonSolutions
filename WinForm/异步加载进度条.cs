using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class 异步加载进度条 : Form
    {
        const int taskCount = 10000;
        public 异步加载进度条()
        {
            InitializeComponent();
            label1.Text = "0/10000";
        }
        /// <summary>
        /// 创建一个非创建控件的线程更新控件
        /// </summary>
        /// <param name="step"></param>
        delegate void AsyncUpdateUI(int step);
        private void button1_Click(object sender, EventArgs e)
        {
            int taskCount = 10000;//任务量为100
            this.progressBar1.Maximum = taskCount;
            this.progressBar1.Value = 0;
            DataWrite dw = new DataWrite();
            dw.UpdateUIDelegate += UpdateUiStatus;
            dw.TaskRollBack += Finsh;

            var task = new Thread(new ParameterizedThreadStart(dw.Write));
            task.IsBackground = true;
            task.Start(taskCount);

        }
        void UpdateUiStatus(int step)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsyncUpdateUI(delegate (int s)
                {
                    PropressUpdate(s);
                }
                ), step);
            }
            else
            {
                PropressUpdate(step);
            }
        }

        void PropressUpdate(int s)
         {
            this.progressBar1.Value += s;
            this.label1.Text = this.progressBar1.Value.ToString() + "/" + this.progressBar1.Maximum.ToString();
            if (this.progressBar1.Value / taskCount > 0.2)
            {
                this.progressBar1.BackColor = Color.Black;
            }
        }
        void Finsh()
        {
            MessageBox.Show("导入成功");
        }
    }
    /// <summary>
    /// 数据读取类
    /// </summary>
    public class DataWrite
    {
        /// <summary>
        /// 声明一个更新ui的委托
        /// </summary>
        /// <param name="step"></param>
        public delegate void UpdateUI(int step);

        public  UpdateUI UpdateUIDelegate;
        /// <summary>
        /// 声明一个通知主线程的委托
        /// </summary>
        public delegate void SignalRTask();

        public  SignalRTask TaskRollBack;


        /// <summary>
        /// 定义错误
        /// </summary>
        public delegate bool ErrTask();
        public ErrTask Error;
        public void Write(object lineCount)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            using (StreamWriter ws = new StreamWriter(path, false, Encoding.UTF8))
            {
                
                for (int i = 0; i < (int)lineCount; i++)
                {
                    ws.Write(i + "测试");
                    //每写入一条数据
                    UpdateUIDelegate(1);
                }
                
            }
            //任务完成通知主线程
            TaskRollBack();
        }
    }

}
