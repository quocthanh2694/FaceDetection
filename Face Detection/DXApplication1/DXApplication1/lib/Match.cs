using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace DXApplication1
{
    class Match
    {
        public static double sum(Matrix1 img)
        {
            int nd, nc; nd = img.NoRows; nc = img.NoCols;
            double tt = 0;
            for (int i = 0; i < nd; i++)
                for (int j = 0; j < nc; j++)
                {
                    tt += img[i, j];
                }            
            return tt;
        }
        public static double average(Matrix1 img)
        {
            int nd, nc; nd = img.NoRows; nc = img.NoCols;
            double tt = 0;
            for (int i = 0; i < nd; i++)
                for (int j = 0; j < nc; j++)
                {
                    tt += img[i, j];
                }
            return tt/nd/nc;
        }

        public static Matrix1 correlate(Matrix1 imgq,Matrix1 imgt)
        {
            int nd, nc; nd = imgq.NoRows; nc = imgq.NoCols;
            Matrix1 imgm = new Matrix1(nd,nc);
            double tt, qt;
            int n = 2; int yi, xj; int nt=25;
            for(int i=0;i<nd;i++)
                for (int j = 0; j < nc; j++)
                {
                    tt = 0; qt = 0;
                    for (int ii = -n; ii <= n; ii++)
                        for (int jj = -n; jj <= n; jj++)
                        {
                            yi = i + ii; xj = j + jj;
                            if (yi < 0 || yi >= nd || xj < 0 || xj >= nc) continue;
                            tt += imgt[yi, xj]; qt += imgq[yi, xj];                            
                        }
                    tt = tt / nt; qt = qt /nt;

                    imgm[i,j]=correlate1p(i, j, imgq, imgt,qt,tt,n);
                }
            return imgm;
        }

        public static double correlate1p(int y,int x,Matrix1 imgq, Matrix1 imgt,double qt,double tt,int n)
        {
            double c = 0;double tc,mc1,mc2; tc=0;mc1=0;mc2=0;
            int i,j;
            for (int ii = -n; ii <= n; ii++)
            {
                i = ii + y;
                if (i < 0 || i >= imgq.NoRows) continue; 
                for (int jj = -n; jj <= n; jj++)
                {
                    j = jj + x;
                    if (j < 0 || j >= imgq.NoCols) continue;
                    tc += (imgt[i, j] - tt) * (imgq[i, j] - qt);
                    mc1 += (imgt[i, j] - tt) * (imgt[i, j] - tt);
                    mc2 += (imgq[i, j] - qt) * (imgq[i, j] - qt);
                }
            }
            if (mc1 == 0 || mc2 == 0) return -1;
            c = tc / Math.Sqrt(mc1 * mc2);
            return c;
        }
    

        public static double biMatch(Matrix1 imgt, Matrix1 imgq)
        {
            int nd, nc; nd = imgq.NoRows; nc = imgq.NoCols;
            int nd1, nc1; nd1 = imgq.NoRows-1; nc1 = imgq.NoCols-1;
            double gt, gq; int dt, dq;
            gt = gq = 0;  double d=0;
            dt = dq = 0;
            double kch = 0;
            for (int i = 1; i < nd1; i++)
                for (int j = 1; j < nc1; j++)
                {
                   
                    d = 0;
                    if (imgt[i, j] == 1)
                    {
                        d += imgq[i, j] +imgq[i - 1, j] + imgq[i + 1, j] + imgq[i, j - 1] + imgq[i, j + 1];
                        d += imgq[i - 1, j-1] + imgq[i + 1, j+1] + imgq[i+1, j - 1] + imgq[i-1, j + 1];
                        if (d > 0) gt++;
                        dt++;
                    }
                    d = 0;
                    if (imgq[i, j] == 1)
                    {
                        d += imgt[i, j] +imgt[i - 1, j] + imgt[i + 1, j] + imgt[i, j - 1] + imgt[i, j + 1];
                        d += imgt[i - 1, j - 1] + imgt[i + 1, j + 1] + imgt[i + 1, j - 1] + imgt[i - 1, j + 1];
                        if (d > 0) gq++;
                        dq++;
                    }
                    //if (imgt[i, j] != imgq[i, j]) kch++;
                    //if (imgt[i, j] == 1 || imgq[i, j] == 1) d++;

                }
           
            gt = gt / dt; gq = gq / dq;
         
            return (gt+gq)/2;
            
         
        }
        public static double biMatch1(Matrix1 imgt, Matrix1 imgq)
        {
            int nd, nc; nd = imgq.NoRows; nc = imgq.NoCols;
            int nd1, nc1; nd1 = imgq.NoRows - 1; nc1 = imgq.NoCols - 1;
            double gt, gq; int dt, dq;
            gt = gq = 0; double d = 0; double d1 = 0;
            dt = dq = 0;
            double kch = 0;
            for (int i = 1; i < nd1; i++)
                for (int j = 1; j < nc1; j++)
                {

                    d = 0;
                    d1 = 0;
                    if (imgt[i, j] == 1)
                    {
                        d += imgq[i, j] + imgq[i - 1, j] + imgq[i + 1, j] + imgq[i, j - 1] + imgq[i, j + 1];
                        d += imgq[i - 1, j - 1] + imgq[i + 1, j + 1] + imgq[i + 1, j - 1] + imgq[i - 1, j + 1];

                        if (d == 0) continue;
                        d1 += imgt[i, j] + imgt[i - 1, j] + imgt[i + 1, j] + imgt[i, j - 1] + imgt[i, j + 1];
                        d1 += imgt[i - 1, j - 1] + imgt[i + 1, j + 1] + imgt[i + 1, j - 1] + imgt[i - 1, j + 1];

                       // if (d1/d > 0) gt++;
                        //dt++;
                        gt += (d1 - d)/9;
                        dt++;
                    }
                    //if (imgt[i, j] != imgq[i, j]) kch++;
                    //if (imgt[i, j] == 1 || imgq[i, j] == 1) d++;

                }

            gt = gt / dt;

            return 1 - gt;


        }
    

        public static double biMatchO(Matrix1 imgt, Matrix1 imgq,Matrix1 dirt,Matrix1 dirq)
        {
            int nd, nc; nd = imgq.NoRows; nc = imgq.NoCols;
            int nd1, nc1; nd1 = imgq.NoRows - 1; nc1 = imgq.NoCols - 1;
            double gt, gq; int dt, dq;
            gt = gq = 0; double d = 0;
            dt = dq = 0;
           // double kch = 0;
            for (int i = 1; i < nd1; i++)
                for (int j = 1; j < nc1; j++)
                {
                    if (imgt[i, j] == 255 || imgq[i, j] == 255) continue;                    
                    d = 0;                    
                    if (imgt[i, j] == 1)                    
                    {
                        dt++;
                        if (dirq[i, j] * dirt[i, j] < 0) continue;
                        //if (Math.Abs(dirt[i, j] - dirq[i, j]) > 45) continue;
                        
                            d += imgq[i, j] + imgq[i - 1, j] + imgq[i + 1, j] + imgq[i, j - 1] + imgq[i, j + 1];
                        if (d > 0) gt++;
                      
                    }
                    d = 0;
                    if (imgq[i, j] == 1)
                    {
                        dq++;
                        if (dirq[i, j] * dirt[i, j] < 0) continue;
                        //if (Math.Abs(dirt[i, j] - dirq[i, j]) > 45) continue;

                            d += imgt[i, j] + imgt[i - 1, j] + imgt[i + 1, j] + imgt[i, j - 1] + imgt[i, j + 1];
                        if (d > 0) gq++;
                        
                    }
                    //if (imgt[i, j] != imgq[i, j]) kch++;
                    //if (imgt[i, j] == 1 || imgq[i, j] == 1) d++;

                }
            //gt = gt / dt; gq = gq / dq;
            gt = gt / dq; gq = gq / dt;
            //if (gt < gq)
            //    return gt;
            //return gq;
            return (gt + gq) / 2;
            //return 1 - kch / d;// nd1 / nc1;
        }
        
       
   

        public static double MatchO(Matrix1 imgt, int x, int y, Matrix1 imgq, int u, int v)
        {
            double gt;
            gt = 0;
            double gq;
            gq = 0;
            int w = 10;
            for (int i = -w; i < w; i += 2)
                for (int j = -w; j < w; j += 2)
                {
                    //tinh khop tu anh t
                    if (imgq[u + i, v + j] == imgt[x + i, y + j]
                        ||
                        imgq[u + i - 1, v + j] == imgt[x + i, y + j] ||
                        imgq[u + i + 1, v + j] == imgt[x + i, y + j] ||
                        imgq[u + i, v + j - 1] == imgt[x + i, y + j] ||
                        imgq[u + i, v + j + 1] == imgt[x + i, y + j]
                        )
                    {

                        gt++;
                    }
                    // if (imgt[u+i, v+j] == imgt[x+i, y+j]        ||
                    //    imgt[u + i - 1, v + j] == imgq[x + i, y + j] ||
                    //    imgt[u + i + 1, v + j] == imgq[x + i, y + j] ||
                    //    imgt[u + i, v + j - 1] == imgq[x + i, y + j] ||
                    //    imgt[u + i, v + j + 1] == imgq[x + i, y + j])
                    //{
                    //    gq++;
                    //}


                }
            //gt = Math.Max(gt,gq) / w/w;
            gt = gt / w / w;

            return gt;
        }

        public static double MatchO(Matrix1 imgt, Matrix1 imgq)
        {
            int w = 1;// 2;
            int nd, nc; nd = imgq.NoRows; nc = imgq.NoCols;
            int nd1, nc1; nd1 = imgq.NoRows - w - 1; nc1 = imgq.NoCols - w - 1;
            double gt, gq; double dt, dq;
            gt = gq = 0; double d = 0;
            dt = dq = 0;
            double kch = 0; double gtq = 0;
            int u, v; u = 0; v = 0;
            //so khop 8 anh trung tam
            double m0, m1, m2, m3, m4, m5, m6, m7, m8;

            int xc, yc; xc = nd / 2 + w; yc = nc / 2 + w;

            m0 = MatchO(imgt, xc, yc, imgq, xc, yc);
            m1 = MatchO(imgt, xc, yc, imgq, xc - w, yc);
            m2 = MatchO(imgt, xc, yc, imgq, xc + w, yc);
            m3 = MatchO(imgt, xc, yc, imgq, xc - w, yc - w);
            m4 = MatchO(imgt, xc, yc, imgq, xc, yc - w);
            m5 = MatchO(imgt, xc, yc, imgq, xc + w, yc - w);
            m6 = MatchO(imgt, xc, yc, imgq, xc - w, yc + w);
            m7 = MatchO(imgt, xc, yc, imgq, xc, yc + w);
            m8 = MatchO(imgt, xc, yc, imgq, xc + w, yc + w);
            double mm;
            mm = m0;
            int h = 0;
            if (m1 > mm) { mm = m1; u = -w; v = 0; }
            if (m2 > mm) { mm = m2; u = w; v = 0; }
            if (m3 > mm) { mm = m3; u = -w; v = -w; }
            if (m4 > mm) { mm = m4; u = 0; v = -w; }
            if (m5 > mm) { mm = m5; u = w; v = -w; }
            if (m6 > mm) { mm = m6; u = -w; v = w; }
            if (m7 > mm) { mm = m7; u = 0; v = w; }
            if (m8 > mm) { mm = m8; u = w; v = w; }


            w = Math.Max(Math.Abs(u), Math.Abs(v));
            nd1 = nd - w - 1; nc1 = nc1 - w - 1;
            for (int i = w + 1; i < nd1; i++)
                for (int j = w + 1; j < nc1; j++)
                {
                    //tinh khop tu anh t

                    if (imgq[i + u, j + v] == imgt[i, j]
                        ||
                        imgq[i - 1 + u, j + v] == imgt[i, j] ||
                        imgq[i + 1 + u, j + v] == imgt[i, j] ||
                        imgq[i + u, j - 1 + v] == imgt[i, j] ||
                        imgq[i + u, j + 1 + v] == imgt[i, j]
                        )
                    {
                        gt++;
                    }
                    if (imgt[i + u, j + v] == imgq[i, j] ||
                                    imgt[i - 1 + u, j + v] == imgq[i, j] ||
                                    imgt[i + 1 + u, j + v] == imgq[i, j] ||
                                    imgt[i + u, j - 1 + v] == imgq[i, j] ||
                                    imgt[i + u, j + 1 + v] == imgq[i, j])
                    {
                        gq++;
                    }

                    dt++;
                }
            gt = Math.Max(gt, gq) / dt;
            return gt;
        }



        


    }
}
