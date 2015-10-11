using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace DXApplication1
{
    class ImageProcess
    {
        #region "Ham chuyen anh mau ve anh xam"
        public static Bitmap to_Gray(Bitmap bm)
        {
            Bitmap bitmap = new Bitmap(bm);
            int x, y;
            Color c;
            Byte gray;
            for (y = 0; y < bm.Height - 1; y++)
            {
                for (x = 0; x < bm.Width - 1; x++)
                {
                    c = bm.GetPixel(x, y);
                    //gray = (byte)c.ToArgb();
                      gray = Convert.ToByte(c.R * 0.287 + c.G * 0.599 + c.B * 0.114);
                    //gray = Convert.ToByte(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                      //gray += 20;
                    if (gray > 255)
                        gray = 255;
                    if (gray < 0)
                        gray = 0;

                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return bitmap;
        }
        public static Bitmap to_Gray_inver(Bitmap bm)
        {
            Bitmap bitmap = new Bitmap(bm);
            int x, y;
            Color c;
            Byte gray;
            for (y = 0; y < bm.Height - 1; y++)
            {
                for (x = 0; x < bm.Width - 1; x++)
                {
                    c = bm.GetPixel(x, y);
                    //gray = (byte)c.ToArgb();
                    gray = Convert.ToByte(c.R * 0.287 + c.G * 0.599 + c.B * 0.114);
                    //gray = Convert.ToByte(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    //gray += 20;
                    if (gray > 255)
                        gray = 255;
                    if (gray < 0)
                        gray = 0;
                    //gray = Convert.ToByte(255 - gray);
                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return bitmap;
        }
        #endregion

        #region "Can bang histogram"
        public static void equilizeHist(Bitmap bmpimg)
        {
            BitmapData data = bmpimg.LockBits(new System.Drawing.Rectangle(0, 0, bmpimg.Width, bmpimg.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                int remain = data.Stride - data.Width * 3;

                int[] histogram = new int[256];
                for (int i = 0; i < histogram.Length; i++)
                    histogram[i] = 0;

                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        int mean = ptr[0] + ptr[1] + ptr[2];
                        mean /= 3;

                        histogram[mean]++;
                        ptr += 3;
                    }

                    ptr += remain;
                }

                float[] LUT = equilize(histogram, data.Width * data.Height);
                ptr = (byte*)data.Scan0;

                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        int index = ptr[0];
                        byte nValue = (byte)LUT[index];
                        if (LUT[index] > 255)
                            nValue = 255;
                        ptr[0] = ptr[1] = ptr[2] = nValue;
                        ptr += 3;
                    }

                    ptr += remain;
                }

                ptr = (byte*)data.Scan0;
            }

            bmpimg.UnlockBits(data);
        }

        public static float[] equilize(int[] histogram, long numPixel)
        {
            float[] hist = new float[256];

            hist[0] = histogram[0] * histogram.Length / numPixel;
            long prev = histogram[0];
            string str = "";
            str += (int)hist[0] + "\n";

            for (int i = 1; i < hist.Length; i++)
            {
                prev += histogram[i];
                hist[i] = prev * histogram.Length / numPixel;
                str += (int)hist[i] + "   _" + i + "\t";
            }

            //	MessageBox.Show( str );
            return hist;

        }

        #endregion


    }
}
