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


namespace qwewqe
{
    public partial class Form1 : Form
    {
        private int value;
        public int value2;
        public bool button1_able;
        private string name = "DongTingChiMake ";
        private string HOUZHUI;

        public int FILELEN { get; private set; }
        public string TOU { get; private set; }
        public string NAMEPATH { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }
        private void Jiake_Click(object sender, EventArgs e)
        {
            label2.Text = "加密";
            this.button3.BackColor = System.Drawing.Color.DarkBlue;
            this.button4.BackColor = System.Drawing.Color.LightSkyBlue;
            label9.Text = "请选择...";
            label7.Text = "请选择...";

        }

        private void Quke_Click(object sender, EventArgs e)
        {
            label2.Text = "解密";
            this.button4.BackColor = System.Drawing.Color.DarkBlue;
            this.button3.BackColor = System.Drawing.Color.LightSkyBlue;
            label9.Text = "请选择...";
            label7.Text = "请选择...";
        }

        private void Menuopen_Click(object sender, EventArgs e)
        {
            if (label2.Text == "加密")
            {
                openFileDialog1.Reset();
                openFileDialog1.Multiselect = true;
                openFileDialog1.CheckPathExists = true;
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.Filter = "所有文件|*.*|MP4文件(*.mp4)|*.mp4|MPK文件(*.mpk)|*.mpk|AVI文件(*.avi)|*.avi|QGLH文件(*.qglh)|*.qglh";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    label9.Text = "已选文件数："+openFileDialog1.SafeFileNames.Length.ToString();
                }
            }
            if (label2.Text == "解密")
            {
                openFileDialog1.Reset();
                openFileDialog1.Multiselect = true;
                openFileDialog1.CheckPathExists = true;
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.Filter = "QGLH文件|*.qglh";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    label9.Text = "已选文件数："+openFileDialog1.SafeFileNames.Length.ToString();          
                }
            }
        }

        private void Menusave_Click(object sender, EventArgs e)
        {
            string save;
            folderBrowserDialog1.Reset();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                save = folderBrowserDialog1.SelectedPath.ToString();
                label7.Text = save;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (label7.Text!= "请选择..."&&label9.Text != "请选择..." && openFileDialog1.FileNames.Length != 0 && folderBrowserDialog1.SelectedPath.Length != 0 && (label2.Text == "加密" || label2.Text == "解密"))
            {
                progressBar1.Maximum = openFileDialog1.FileNames.Length;
                progressBar1.Step = 1;

                progressBar2.Maximum = 100;
                progressBar2.Step = 1;

                Thread start = new Thread(START);
                start.Start();
                Thread.Sleep(100);

                button1_able = false;
            }
            else
            {
                MessageBox.Show("未选择文件或保存路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void START()
        {
            int NUM = openFileDialog1.FileNames.Length;

            data DATA = new data();
            Thread back = new Thread(DATA.www);
            back.IsBackground = true;
            back.Start();
            value = 0;
            for (int i = 0; i < NUM; i++)
            {
                if (label2.Text == "加密")
                {
                    make(i);
                    if (HOUZHUI == ".qglh")
                    {
                        MessageBox.Show("第" + (i + 1).ToString() + "个文件为加密过的文件，已跳过此文件！", "重复加密", MessageBoxButtons.OK);
                        continue;
                    }
                }
                if (label2.Text == "解密") fenjie(i);
                FileStream open = new FileStream(openFileDialog1.FileNames[i], FileMode.Open);
                FileStream save = new FileStream(NAMEPATH, FileMode.Create);
                ///////*******头处理**************/////////////
                if (label2.Text == "加密") save.Write(System.Text.Encoding.Default.GetBytes(TOU), 0, TOU.Length);
                if (label2.Text == "解密") open.Seek(TOU.Length, SeekOrigin.Begin);
                //////////////////////**************///////////////
                DATA.LEN = open.Length;
                DATA.OPEN = open;
                DATA.SAVE = save;
                DATA.KEY = 1;
                Thread.Sleep(1);
                while (DATA.flag == 0) { value2 = DATA.value2; Thread.Sleep(10); }
                DATA.flag = 0;
                save.Close();
                open.Close();
                value++;      
            }
            value2 = 100;
            MessageBox.Show("全部完成！");
            value = 0;
            value2 = 0;
            button1_able = true;

            System.Diagnostics.Process.Start(folderBrowserDialog1.SelectedPath);
        }
        public void fenjie(long i)
        {
            string qwe2 = @"\";
            FileStream fileStream = new FileStream(openFileDialog1.FileNames[i], FileMode.Open);
            fileStream.Seek(name.Length, SeekOrigin.Begin);
            long j;
            int qqqq;
            char[] aaa = new char[10];
            for (j = 0; j < 10; j++)
            {
               qqqq = fileStream.ReadByte();
                if (qqqq != '?') aaa[j] = (char)qqqq;
                else break;
            }
            char []ss = new char[j];
            for (long k = 0; k < j; k++) ss[k] = aaa[k];
            string s = new string(ss);
            HOUZHUI = s;
            TOU = name + HOUZHUI + "?";

/////////////////********************名字****///////////////
            string result = openFileDialog1.SafeFileNames[i];
            char[] result1 = result.ToCharArray();
            char[] hyhy = new char[result.Length];
            long h;
            for (h = 0; h < result.Length; h++)
            {
                char t = result1[h];
                if (t == '.')
                {
                    break;
                }
                else hyhy[h] = t;
            }
            char[] s1 = new char[h];
            for (long k = 0; k < h; k++) s1[k] = hyhy[k];
            string eee = new string(s1);
////////////*********************/////////////////////////

            NAMEPATH = folderBrowserDialog1.SelectedPath.ToString() + qwe2 + eee  + HOUZHUI;
            fileStream.Close();
        }

        public void make(long i)
        {
            string qwe2 = @"\";
            string result = openFileDialog1.SafeFileNames[i];
            char[] result1 = result.ToCharArray();
            char[] hyhy = new char[result.Length];
            char[] hou = new char[10];
            long h;
            for (h = 0; h < result.Length; h++)
            {
                char j = result1[h];
                if (j == '.' ) break;
                else hyhy[h] = j;
            }
            char[] s1 = new char[h];
            for (long k = 0; k < h; k++) s1[k] = hyhy[k];
            string s = new string(s1);

//////////*****后缀*********/////////////////////
            long d = 0;
            for (; h < result1.Length; h++)
            {
                char j = result1[h];
                if (j == '.' || d != 0)
                {
                    hou[d] = j;
                    d++;
                }
            }
            char[] s2 = new char[d];
            for (long k = 0; k < d; k++) s2[k] = hou[k];
            HOUZHUI = new string(s2);
///////////////***************///////////////////
///
            TOU = name + HOUZHUI+"?";
            NAMEPATH = folderBrowserDialog1.SelectedPath.ToString() + qwe2 + s + "." + "qglh";
        }
        
 

        private void About1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本意用于某盘的", "七宫六花丶", MessageBoxButtons.OK);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = value;
            progressBar2.Value = value2;

            label12.Text = (value *100/ progressBar1.Maximum).ToString() + "%";
            label11.Text = value2.ToString() + "%";

            if (button1_able == false) { button1.Enabled = false;button1.Text = "请勿关闭"; toolStripStatusLabel1.Text = "注意：1.正在运行！！关闭会损坏当前转换到的文件！！    2.磁盘大量读写操作！！电脑会变卡！！"; }

            if (button1_able == true) { button1.Enabled = true; button1.Text = "开始"; toolStripStatusLabel1.Text = "注意！！不要出现这种情况！！点我查看"; }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button1_able = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出吗", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                Application.Exit();
            } 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }

        private void QQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("up:七宫六花丶", "2233", MessageBoxButtons.OK);
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"比如加密 <D:\ABC.txt> 保存在目录：<D:\> 下 ，可是<D:\>下已有<ABC.qglh>文件，此时会出错！ 解密同理", "注意", MessageBoxButtons.OK);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }

    public class data
    {
        public FileStream OPEN;
        public FileStream SAVE;
        public long LEN;
        public int KEY = 0;
        public int flag = 0;
        public int value2;
        private int _8M = 1<<23;

        public void www()
        {
            while (true)
            {
                if (KEY == 1)
                {
                    byte []aaa=new byte[_8M];
                    long X = LEN / _8M;
                    long Y = LEN % _8M;
                    if (X != 0) 
                    {
                        for (long i = 0; i < X; i++)
                        {
                            OPEN.Read(aaa, 0, _8M);
                            for (int j = 0; j < _8M; j++) aaa[j] = (byte)(~aaa[j]);
                            SAVE.Write(aaa, 0, _8M);
                            value2 = ((int)(((i + 1) * _8M) >> 10) * 100 / (int)(LEN >> 10));
                        }
                    }

                    OPEN.Read(aaa, 0, (int)Y);
                    for (int i = 0; i < Y; i++) aaa[i] = (byte)~aaa[i];
                    Thread.Sleep(0);
                    SAVE.Write(aaa, 0, (int)Y);
                    value2 = 100;
                    KEY = 0;
                    flag = 1;
                }
                else Thread.Sleep(20);  
            }
        }
    }
}
