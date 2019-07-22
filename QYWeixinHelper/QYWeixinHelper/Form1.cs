using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QYWeixinHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            //Console.WriteLine("回车终止：");
            //do
            //{
            try
            {
                string touser = "IT";
                QYWeixinHelper.SendText(touser, "消息推送测试_</br>当前时间是" + DateTime.Now);
            }
            catch (Exception ex)
            {

                throw;
            }

            //}
            //while (Console.ReadKey().Key != ConsoleKey.Enter);
        }
    }
}
