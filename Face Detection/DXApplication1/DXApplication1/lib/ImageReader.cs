using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace _DPCA
{
    public class ImageReader
    {
        //ham so hoa anh dau vao
        public static int[,] SoHoaAnh(Bitmap b)
        {
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            int[,] matrananhgoc = new int[b.Width, b.Height];
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                for (int y = 0; y < b.Height; y++)
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        // MessageBox.Show(p[0].ToString()+"  abc "+p[1].ToString());
                        matrananhgoc[x, y] = (int)p[0];
                        p += 3;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return matrananhgoc;
        }

        

    }
}
