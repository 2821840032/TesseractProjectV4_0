using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace TesseractProjectV4_0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       


        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateUIFunc = UpdateUI;
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Multiselect = true;
                //fileDialog.FileName = "D:软件";
                fileDialog.Title = "请选择w图片文件";
                fileDialog.Filter = "所有图片(*.jpg;*.png)|*.jpg;*.png";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileDialog.FileNames.ToList().ForEach(d=> {
                        string Path = System.IO.Path.GetFullPath(d);
                        TesseractFor @for = new TesseractFor();
                        @for.ImgComplete += SetImg;
                        @for.RecognitionComplete += SetTxt;
                        @for.Recognition(Path);
                    });
                    
                }
            }



          


        }

        /// <summary>
        /// 处理图片列表
        /// </summary>
        List<Bitmap> ImgList = new List<Bitmap>();

        /// <summary>
        /// 文字列表
        /// </summary>
        List<string> ContentList = new List<string>();

        /// <summary>
        /// 自信率
        /// </summary>
        List<float> ConfidenceList = new List<float>();
        public void SetImg(Bitmap ImgContent, int I)
        {
           
         
            textBox1.Invoke(UpdateUIFunc, "请稍后");
            ImgList.Add(ImgContent);
           
        }
        /// <summary>
        /// 一个异步完成处理函数
        /// </summary>
        /// <param name="Content">正文</param>
        /// <param name="I">索引</param>
        /// <param name="ImgPath">图片路径</param>
        /// <param name="Confidence">自信率</param>
        public void SetTxt(string Content, int I,string ImgPath,float Confidence) {
            textBox1.Invoke(UpdateUIFunc, "完成一项 自信率"+ Confidence);
            ContentList.Add(Content);
            ConfidenceList.Add(Confidence);
            comboBox1.Items.Add(ImgPath);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = ImgList[comboBox1.SelectedIndex];
            textBox1.Text = SetContentNewline(ContentList[comboBox1.SelectedIndex]);
            label3.Text = ConfidenceList[comboBox1.SelectedIndex]+"%";
        }

        public delegate void UpdateUIDelegate(object o);
        public UpdateUIDelegate UpdateUIFunc { get; set; }
        public void UpdateUI(object o)
        {
            textBox1.Text= o as string;
        }


        /// <summary>
        /// 修改多行文本框不能换行的BUG
        /// </summary>
        /// <returns></returns>
        public string SetContentNewline(string Content) {
           return Content.Replace("\n", "\r\n");
        }


    }
}
