using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace TesseractProjectV4_0
{
   public class TesseractFor
    {
        /// <summary>
        /// 异步完成后处理代码
        /// </summary>
        public Action<string, int, string,float> RecognitionComplete { get; set; }

        /// <summary>
        /// 图片处理完成后的流
        /// </summary>
        public Action<Bitmap, int> ImgComplete { get; set; }

        /// <summary>
        /// 异步识别
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public async Task Recognition(string Path, int Index = 0)
        {
            string Content = "";
            float ratio = 0f;
            await Task.Run(() =>
            {
                var img = new Bitmap(Path);
                //灰度化
                //ToGrey(img);//灰度化 效果 背景突出

                //二值化
                //Thresholding(img);

                //GrayReverse(img);//灰度翻转 --用于识别背景图案
                //pictureBox1.Image = Image.FromHbitmap(img.GetHbitmap());
                //ConvertTo1Bpp1(img);//二值化 效果就是图片毁掉
                //pictureBox1.Image = Image.FromHbitmap(img.GetHbitmap());
                //ConvertTo1Bpp2(img);//  效果不太好
                //pictureBox1.Image = Image.FromHbitmap(img.GetHbitmap());

               
                Tool.Processing(img,false);
                ImgComplete?.Invoke(img,Index);


                var ocr = new TesseractEngine("./tessdata", "chi_sim", EngineMode.TesseractOnly);
                //Bitmap bitmap = new Bitmap(_emguImage.ToBitmap());
                var page = ocr.Process(img);
               
                Content = page.GetText();//Tool.OReplace(page.GetText());

                ratio = page.GetMeanConfidence();
                //完成
            });
            RecognitionComplete?.Invoke(Content, Index, Path, ratio);
        }
    }
}
