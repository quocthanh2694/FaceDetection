using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace DXApplication1
{
    public class PCA
    {

        public static double min_sum=0, min_i=0,max_sum=0,max_i=0;

        #region "Properties"
        public static int h, w;//, m;
        
        //khai bao cac bien ma tran
        
        //public static Matrix1 mt_data;
        public static Matrix1 mt_hpsai;
        public static Matrix1 mt_eigen_vec;
        public static Matrix1 mt_eigen_val;
        public static Matrix1 mt_feature; //face space
        private static Matrix1 mt_U_tran;
        public static Matrix1 mt_project;
        public static Matrix1 mt_mean;
        public static FeatureMatrix fm=new FeatureMatrix();
        public static double[,,] distance_diff;
        public static int ns, nd;
        public static double[] min_diff, max_same;
        
        //
        public static string[] str_file_name_eigen;

        /*public Matrix1 MT_DATA
        {
            get { return mt_data; }
            set { mt_data = value; }
        }*/
       

        public Matrix1 MT_HPSAI
        {
            get { return mt_hpsai; }
            set { mt_hpsai = value; }
        }
        

        public Matrix1 MT_EIGEN_VEC
        {
            get { return mt_eigen_vec; }
            set { mt_eigen_vec = value; }
        }
        
        public Matrix1 MT_EIGEN_VAL
        {
            get { return mt_eigen_val; }
            set { mt_eigen_val = value; }
        }
        

        public Matrix1 MT_FEATURE
        {
            get { return mt_feature; }
            set { mt_feature = value; }
        }
        

        public Matrix1 MT_U_TRAN
        {
            get { return mt_U_tran; }
            set { mt_U_tran = value; }
        }
        

        public Matrix1 MT_PROJECT
        {
            get { return mt_project; }
            set { mt_project = value; }
        }

        public Matrix1 MT_MEAN
        {
            get { return mt_mean; }
            set { mt_mean = value; }
        }
        
        #endregion

        #region "image to matrix"
        //in: image name
        //out: 1D vector cot
        public static Matrix1 image_2_matrix(Bitmap bmp1)// String file_name)
        {
            //tao anh bitmap tam
            Bitmap bmp = bmp1;// new Bitmap(file_name);

            //khai bao ma tran anh

            Matrix1 matrananhgoc = new Matrix1(bmp.Height, bmp.Width);
           
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            int nOffset = stride - bmp.Width * 3;
            //nOffset chính là cái rìa của bức ảnh, khi con trỏ xử lý đến pixel cuối cùng của hàng, thì muốn xuống 
            //hàng kế tiếp, ta phải bỏ qua cái rìa này bằng cách cộng thêm địa chỉ con trỏ với nOffset
            unsafe
            {
                byte* p = (byte*)bmData.Scan0;
                //p sẽ trỏ đến địa chỉ đẩu của ảnh
                int x, y;
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                    {
                      
                        matrananhgoc[y, x] = (int)p[0];
                        //Chuyển con trỏ sang pixel kế tiếp
                        p += 3; // 2 pixel kế tiếp cách nhau 3 bytes
                    }//Xử lý xong 1 hàng
                    //Chuyển con trỏ xuông hàng kế tiếp
                    p += nOffset;
                }
            }
            bmp.UnlockBits(bmData);//giải phóng biến BitmapData
            return matrananhgoc;
        }
        //ham chuyen ma tran sang vector cot 1d
        public static double[] matrix2vector(double[,] mat, int m,int n)
        {
            int i, j,dem=0;
            double[] res = new double[m * n];
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    res[dem++] = mat[i, j];
            return res;
        }

        #endregion

        #region "Average Matrix"
        /*ham chuyen cac file anh thanh ma tran va tinh ma tran trung binh
        input: string ten cac file anh
        out: ma tran trung binh cua cac anh dau vao
         ten ban dau: image_2_column_matrix
         */
        //ham Gabor dung cho viec tang cuong duong van 

        public static double gabor(double x, double y, double theta, double f)
        {
            double x0, y0;
            double detaX2, detaY2;

            detaX2 = 0.0625; detaY2 = 0.0625;
            x0 = x * Math.Cos(theta) + y * Math.Sin(theta);
            y0 = -x * Math.Sin(theta) + y * Math.Cos(theta);
            double g = 2 * Math.PI * f * x0;

            return Math.Exp(-0.5 * (x0 * x0 * detaX2 + y0 * y0 * detaY2)) * Math.Cos(g);
        }

        public static Matrix1 apDungGabor(Matrix1 img, double goc, double tanso)
        {
            Matrix1 image = new Matrix1(img);
            Matrix1 image2 = new Matrix1(img);
            
            //luc tinh ham gabor
            int wgs = 5;
            double[,] gV = new double[11, 11];
            //tinh toan ham gabor su dung mat na wg x wg  			
            for (int v = -wgs; v <= wgs; v++)
                for (int u = -wgs; u <= wgs; u++)
                    gV[v + wgs, u + wgs] = gabor(v, u, goc, tanso);

            //luc ap dung cho tung pixel
            double sum;
            int dx, dy, du, dv;
            dx = 0;
            dy = 0;
            du = 0;
            dv = 0;
            int ndong, ncot;
            ndong = image.NoRows; //-wgs;
            ncot = image.NoCols;// -wgs;
            int d, x, y;
            for (x = 0; x < ndong; x++)
            {
                for (y = 0; y < ncot; y++)
                {
                    //xem xet tung pixel(x,y)
                    //su dung mat na wg x wg	
                    sum = 0;
                    d = 0;
                    for (int v = -wgs; v <= wgs; v++)
                    {
                        dv = x + v;
                        if (dv >= ncot || dv < 0)
                        {
                            d = d + 2 * wgs + 1;
                            continue;
                        }
                        for (int u = -wgs; u <= wgs; u++)
                        {
                            du = y + u;
                            if (du >= ndong || du < 0)
                            {
                                d++;
                                continue;
                            }
                            sum += (int)(gV[v + wgs, u + wgs] * image2[dv, du]);
                            d++;
                        }
                    }
                    sum = sum / d;

                    //gan lai muc xam cho anh
                    image[x, y] = sum;
                    //image[x, y] = sum;
                }
            }


            
            //Bitmap res = new Bitmap(PCA.matrix_2_image(image, image.NoRows, image.NoCols));

            return image;
        }

        public static double waveletGabor(double x, double y, double theta, double w0)
        {
            double R; double kf = 2.5;
            R = w0 / 2 / Math.PI / kf * Math.Exp(-w0 * w0 / 8 / kf / kf * (4 * (x * Math.Cos(theta) + y * Math.Sin(theta)) + Math.Pow((-x * Math.Sin(theta) + y * Math.Cos(theta)), 2)));
            double mr;
            mr = w0*(x * Math.Cos(theta) + y * Math.Sin(theta));
            return -R * Math.Exp(-kf * kf / 2) + R * Math.Cos(mr);
           
        }

        public static Matrix1 apDungWaveletGabor(Matrix1 img, double goc, double tanso)
        {
            Matrix1 image = new Matrix1(img);
            Matrix1 image2 = new Matrix1(img);
            //image = Matrix.Transpose(image);
            //image2 = Matrix.Transpose(image2);
            //luc tinh ham gabor
            double[] fre = new double[10] { 0.25, 0.5, 1, 2, 4, 6, 8, 10, 12, 14 }; //{2, 3, 4, 5, 6, 7, 8, 9,10,11,12};
            double[] rad = new double[8] { -4, 2, 4, 1, 3, -3, -6, 6 };

            int sh = 8;
            int wgs = 5;
            double[, ,] gV = new double[17, 11, 11];
            //for (int j = 0; j < 4; j++)

            {
                // tanso = 1.0 / Convert.ToDouble(fre[j]);
                for (int i = 0; i < sh; i++)
                {
                    goc = Math.PI / Convert.ToDouble(rad[i]);
                    //tinh toan ham gabor su dung mat na wg x wg  			
                    for (int v = -wgs; v <= wgs; v++)
                        for (int u = -wgs; u <= wgs; u++)
                            gV[i, v + wgs, u + wgs] = waveletGabor(v, u, goc, tanso);
                    //gV[i, v + wgs, u + wgs] = gabor(v, u, goc, tanso); 
                }
            }

            //luc ap dung cho tung pixel
            double sum;
            int dx, dy, du, dv;
            dx = 0;
            dy = 0;
            du = 0;
            dv = 0;
            int ndong, ncot;
            ndong = image.NoRows; //-wgs;
            ncot = image.NoCols;// -wgs;
            int d, x, y;
            double sm=-1000, im=-1000,sm1=1000,im1=1000;
            int dem = 0;
                                 //
            j = 0;
            for (x = 0; x < ndong; x++)
            {
                for (y = 0; y < ncot; y++)
                {
                    sm = Double.MaxValue;
                    sm1 = Double.MinValue;                      ///
                    //  for (int j = 0; j < 4; j++)

                    for (int i = 0; i < sh; i++)
                    {

                        //xem xet tung pixel(x,y)
                        //su dung mat na wg x wg	
                        sum = 0;
                        d = 0;
                        for (int v = -wgs; v <= wgs; v++)
                        {
                            dv = x + v;
                            if (dv >= ncot || dv < 0)
                            {
                                d = d + 2 * wgs + 1;
                                continue;
                            }
                            for (int u = -wgs; u <= wgs; u++)
                            {
                                du = y + u;
                                if (du >= ndong || du < 0)
                                {
                                    d++;
                                    continue;
                                }
                                sum += (int)(gV[i, v + wgs, u + wgs] * image2[dv, du]);
                                d++;
                            }
                        }
                        //sum = sum / d;
                        if (sum < sm) { sm = sum; im = i; }
                         if (sum > sm1) { sm1 = sum;  im1 = i; }              /////
                         dem++;
                    }
                    //                 Console.WriteLine(sm.ToString()+"__"+ im.ToString());
                    //gan lai muc xam cho anh
                    image[x, y] = sm;// sum;
                    //image[x, y] = (sm + sm1)/2;
                }
            }
            Console.WriteLine(dem);
           // MessageBox.Show("sm+im: " + sm.ToString() + " & " + im.ToString());

            //10 scale : sum min + i min
            min_sum = sm1;
            min_i = im1;

            max_sum = sm;
            max_i = im;

            //Bitmap res = new Bitmap(PCA.matrix_2_image(image, image.NoRows, image.NoCols));

            return image;
        }
        public static void getmax(out double sm1,out double i1)
        {
            sm1 = min_sum;
            i1 = min_i;
        }

        public static void getmin(out double sm1, out double i1)
        {
            sm1 = max_sum;
            i1 = max_i;
        }

  
        #endregion

        #region "ham tao ra anh trung binh (eigenface) tu vector dac trung"
        //va kich thuoc of anh bitmap
        //matrix la vector 1 cot
        public static Bitmap create_image_mean(double[] vector,int h, int w)
        {
            Bitmap bmp = new Bitmap(w,h);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int t = 0;
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        
                        ptr[0] = (byte)vector[t];
                        t++;
                        ptr++;
                    }
                    ptr += data.Stride - data.Width;
                }
            }
            bmp.UnlockBits(data);
            bmp = ImageProcess.to_Gray(bmp);
            return bmp;
        }
         
        public static Bitmap matrix_2_image(Matrix1 vector, int h, int w)
        {
           
            Bitmap bmp = new Bitmap(w, h);
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            int nOffset = stride - bmp.Width * 3;
            //nOffset chính là cái rìa của bức ảnh, khi con trỏ xử lý đến pixel cuối cùng của hàng, thì muốn xuống 
            //hàng kế tiếp, ta phải bỏ qua cái rìa này bằng cách cộng thêm địa chỉ con trỏ với nOffset
            unsafe
            {
                byte* p = (byte*)bmData.Scan0;
                //p sẽ trỏ đến địa chỉ đẩu của ảnh
                int x, y;
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                    {
                        //Xử lý 3 byte của pixel
                        p[0] = (byte)vector[y, x];
                        p[1] = (byte)vector[y, x];
                        p[2] = (byte)vector[y, x];
                        //Chuyển con trỏ sang pixel kế tiếp
                        p += 3; // 2 pixel kế tiếp cách nhau 3 bytes
                    }//Xử lý xong 1 hàng
                    //Chuyển con trỏ xuông hàng kế tiếp
                    p += nOffset;
                }
            }
            bmp.UnlockBits(bmData);//giải phóng biến BitmapData
            return bmp;
        }
        #endregion

        #region "Ham tinh ma tran trung binh cua tap anh dau vao"
        //in: ma tran cot
        //out: vector cot trung binh
        /*public static double[,] mean_matrix(string[] str_file_name)
        {
            double[,] mean = new double[h,w];

            for (int i = 0; i < mat.NoRows; i++)
            {
                for (int j = 0; j < mat.NoCols; j++)
                    mean[i] += mat[i, j];
                mean[i] /= mat.NoCols;
            }
            return mean;
        }*/
       
        #endregion

        #region "Ham tinh ma tran do lech chuan"
        //in: ma tran cot ban dau va vector cot mean
        //out: ma tran cot ma moi cot da tru di vector cot
        public static Matrix1 differ_matrix(Matrix1 mat, double[] mean)
        {
            Matrix1 res = new Matrix1(mat.NoRows, mat.NoCols);
            for (int i = 0; i < mat.NoRows; i++)
                for (int j = 0; j < mat.NoCols; j++)
                    res[i, j] = mat[i, j] - mean[i];
            return res;
        }
        #endregion

        #region "Ham tinh hieu 2 vector"
        //in: 2 vector u,v;
        //out: vector hieu
        public static double[] hieu_vector(double[] u, double[] v)
        {
            double[] res;
            if ((u.Length) != (v.Length))
            {
                MessageBox.Show("Hieu 2 vector khong cung chieu");
                res = new double[u.Length];
                return res;
            }
            int n = u.Length;
            res = new double[n];
            for (int i = 0; i < n; i++)
                res[i] = u[i] - v[i];
            return res;
        }
        #endregion

        #region "Ham tinh do do Euclid giua 2 vector va ma tran"
        public static double Euclid_distance_vector(double[] a, double[] b)
        {
            double sol=0;
            if (a.GetLength(0) != b.GetLength(0))
            {
                MessageBox.Show("2 vector khac so chieu");
                return sol;
            }
            for (int i = 0; i < a.GetLength(0); i++)
                sol += (a[i] - b[i]) * (a[i] - b[i]);
            sol = Math.Sqrt(sol);
            return sol;
        }

        //ham tinh khoang cach Euclid giua 2 ma tran
        public static double Euclid_distance_matrix(Matrix1 a, Matrix1 b)
        {
            double sol = 0;
            if (a.NoCols != b.NoCols || a.NoRows != b.NoRows)
            {
                MessageBox.Show("2 ma tran khac so chieu");
                return sol;
            }
            int i, j;
            for (i = 0; i < a.NoRows; i++)
                for (j = 0; j < a.NoCols; j++)
                    sol += (a[i, j] - b[i, j]) * (a[i, j] - b[i, j]);
            //sol = Math.Sqrt(sol);
            //sol /= a.NoCols * a.NoRows;
            //sol /= 255;
            return sol;
        }
        #endregion

        #region "Ham tim eigenvalue va eigenvector tot nhat"
        //in: k la so luong eigenvalue va eigenvector do user chon
        //lay defaul neu k=-1
        public static void lagest_eigen(int k)
        {
            int i, j;
            //xet truong hop mac dinh
            if (k < 0)
            {
                //xet cot cac eigenvalue
                for (i = 0; i < mt_eigen_val.NoRows; i++)
                {
                    if (mt_eigen_val[i, 0] < 0.0001)
                    { 
                        //xoa dong thu i cua eigenvalue
                        for (j = i; j < mt_eigen_val.NoRows-1; j++)
                            mt_eigen_val[j, 0] = mt_eigen_val[j + 1, 0];
                        mt_eigen_val.NoRows = mt_eigen_val.NoRows - 1;
                        //xoa cot thu i cua eigenvector
                        for (j = i; j < mt_eigen_vec.NoCols-1; j++)
                        {
                            for (k = 0; k < mt_eigen_vec.NoRows; k++)
                                mt_eigen_vec[k, j] = mt_eigen_vec[k, j + 1];
                        }
                        mt_eigen_vec.NoCols = mt_eigen_vec.NoCols - 1;
                        //cap nhat lai danh sach cac anh duoc giu lai
                        for(j=i;j<str_file_name_eigen.Length-1;j++)
                        {
                            str_file_name_eigen[j] = "";
                            str_file_name_eigen[j] = str_file_name_eigen[j + 1].ToString();
                        }
                        str_file_name_eigen[str_file_name_eigen.Length - 1]="";
                        MessageBox.Show("so lon file name cap nhat :" + str_file_name_eigen.Length.ToString());
                    }
                }
            }

        }
        #endregion

   

        public static double hamLoi(double x)
        {
            double loi = 1 - Math.Exp(-x * x) / Math.Sqrt(Math.PI) / x * (1 - 0.5 / x / x + 3 / 4 / Math.Pow(x, 4));
            return loi;
        }
        #region "Ham uoc luong ma tran hiep phuong sai"
        public static Matrix1 cov_computew()
        {
           
            //cac bien local
            int i;
            //khai bao ma tran hiep phuong sai
            Matrix1 cov_matrix = new Matrix1(w,w);
            //
            int x, y;
            Matrix1 tam= new Matrix1(w, w);;
            int k = 0;
            for (i = 0; i < ns; i++)
            {
                
              

                //tru cho ma tran trung binh
                tam = Matrix1.Subtract(A[i], AT[k]);
                //phan thu nghiem weigh 2DLDA
                //for (y = 0; y < w; y++)
                //    for (x = 0; x < w; x++)
                //        tam[y, x] = (1 + 0.5 / (A[i][y, x] - mt_mean[y, x]) / (A[i][y, x] - mt_mean[y, x]) * hamLoi((A[i][y, x] - mt_mean[y, x]) * 0.5 * Math.Sqrt(2))) * (A[i][y, x] - mt_mean[y, x]);


                Matrix1 mat = Matrix1.Multiply(Matrix1.Transpose(tam), tam);
                
                cov_matrix = Matrix1.Add(cov_matrix, mat);
                if ((i + 1) % sn == 0)
                {
                    k++;  //xet anh trung binh cua nguoi tiep theo
                }
            }
            for (y = 0; y < w; y++)
            {
                for (x = 0; x < w; x++)
                {
                    cov_matrix[y, x] /= (ns - 1);
                    //cov_matrix[y, x] *= sn;
                    //Console.Write(cov_matrix[y, x] + "  ");
                }
                //Console.WriteLine("");
            }
            //Console.ReadLine();
            
            return cov_matrix;
        }
        
        public static Matrix1 cov_compute()
        {

            //cac bien local
            int i;
            //khai bao ma tran hiep phuong sai
            Matrix1 cov_matrix = new Matrix1(w, w);
            //
            int x, y;
            Matrix1 tam;

            for (i = 0; i < ns; i++)
            {



                //tru cho ma tran trung binh
                tam = Matrix1.Subtract(A[i], mt_mean);
                //phan thu nghiem weigh 2DLDA
                //for (y = 0; y < w; y++)
                //    for (x = 0; x < w; x++)
                //        tam[y, x] = (1 + 0.5 / (AT[i][y, x] - mt_mean[y, x]) / (AT[i][y, x] - mt_mean[y, x]) * hamLoi((AT[i][y, x] - mt_mean[y, x])*0.5*Math.Sqrt(2))) * (AT[i][y, x] - mt_mean[y, x]);

                Matrix1 mat = Matrix1.Multiply(Matrix1.Transpose(tam), tam);
               

                cov_matrix = Matrix1.Add(cov_matrix, mat);
            }
            for (y = 0; y < w; y++)
                for (x = 0; x < w; x++)
                    cov_matrix[y, x] /= (ns - 1);


            return cov_matrix;
        }

        public static Matrix1 cov_compute2(string[] str_file_name)
        {

            //cac bien local
            int i;
            int n = str_file_name.Length;// so anh huan luyen

            //khai bao ma tran hiep phuong sai
            Matrix1 cov_matrix = new Matrix1(h, w);
            //
            int x, y,k,r;
            Matrix1 tam;
            Matrix1 tam2;
            mt_mean = Matrix1.Transpose(mt_mean);
            double[] col_sum = new double[w];
            for (i = 0; i < ns; i++)
            {
                //chuyen anh thu i thanh ma tran
                Bitmap bmtam = new Bitmap(str_file_name[i].ToString());
                tam = image_2_matrix(bmtam);

                tam = Matrix1.Transpose(tam);


                //tru cho ma tran trung binh
                tam = Matrix1.Subtract(tam, mt_mean);
                tam2=new Matrix1(tam);

                for (k = 0; k < tam2.NoCols; k++)
                {
                    double tong = 0;
                    for ( r = 0; r < tam2.NoRows; r++)
                        tong += tam2[r, k];
                    col_sum[k] = tong;
                }
                for (k = 0; k < tam2.NoCols; k++)
                {
                   // double tong = 0;
                    for (r = 0; r < tam2.NoRows; r++)
                        tam2[r, k] = col_sum[k];
                    //col_sum[k] = tong;
                }
                Matrix1 mat = Matrix1.Multiply(Matrix1.Transpose(tam2), tam);

                cov_matrix = Matrix1.Add(cov_matrix, mat);
            }
            for (y = 0; y < h; y++)
                for (x = 0; x < w; x++)
                    cov_matrix[y, x] /= (sn - 1);


            return cov_matrix;
        }
        #endregion

        #region "Ham tinh toan tong hop"
        //in:chuoi file name
        //tinh toan cac bien can thiet trong PCA
        public static Matrix1[] A;
        public static Matrix1[] AT;
        public static  int n;
        public static int tt = 0;
        public static int TT = 1;//32;      //so lan ap dung gabor
        public static int NH = 4;//32;      //so luong huong
        public static int j = 0, k = 0;
        public static int sn = 1;
        public static void chuyen_huong_tiep_theo_Gabor()
        {
            j = (j + 1);
            if (j > NH-1)
                k++;
            j = j % NH;
        }
    
    
        #endregion


        public static Matrix1 apDungWaveletGabors(Matrix1 img, double goc, double tanso,int s)
        {
            Matrix1 image = new Matrix1(img.NoRows/s,img.NoCols/s);
            Matrix1 image2 = new Matrix1(img);
            //image = Matrix.Transpose(image);
            //image2 = Matrix.Transpose(image2);
            //luc tinh ham gabor
            double[] fre = new double[10] { 0.25, 0.5, 1, 2, 4, 6, 8, 10, 12, 14 }; //{2, 3, 4, 5, 6, 7, 8, 9,10,11,12};
            double[] rad = new double[8] { -4, 2, 4, 1, 3, -3, -6, 6 };

            int sh = 8;
            int wgs = 5;
            double[, ,] gV = new double[17, 11, 11];
            //for (int j = 0; j < 4; j++)

            {
                // tanso = 1.0 / Convert.ToDouble(fre[j]);
                for (int i = 0; i < sh; i++)
                {
                    goc = Math.PI / Convert.ToDouble(rad[i]);
                    //tinh toan ham gabor su dung mat na wg x wg  			
                    for (int v = -wgs; v <= wgs; v++)
                        for (int u = -wgs; u <= wgs; u++)
                            gV[i, v + wgs, u + wgs] = waveletGabor(v, u, goc, tanso);
                    //gV[i, v + wgs, u + wgs] = gabor(v, u, goc, tanso); 
                }
            }

            //luc ap dung cho tung pixel
            double sum;
            int dx, dy, du, dv;
            dx = 0;
            dy = 0;
            du = 0;
            dv = 0;
            int ndong, ncot;
            ndong = image.NoRows; //-wgs;
            ncot = image.NoCols;// -wgs;
            int d, x, y;
            double sm =0, im = 0, sm1 = 0, im1 = 0;
            int dem = 0;
            //
            j = 0;
            for (x = 0; x < ndong; x+=s)
            {
                for (y = 0; y < ncot; y+=s)
                {
                    sm = Double.MaxValue;
                    sm1 = Double.MinValue;                      ///
                    //  for (int j = 0; j < 4; j++)

                    for (int i = 0; i < sh; i++)
                    {

                        //xem xet tung pixel(x,y)
                        //su dung mat na wg x wg	
                        sum = 0;
                        d = 0;
                        for (int v = -wgs; v <= wgs; v++)
                        {
                            dv = x + v;
                            if (dv >= ncot || dv < 0)
                            {
                                d = d + 2 * wgs + 1;
                                continue;
                            }
                            for (int u = -wgs; u <= wgs; u++)
                            {
                                du = y + u;
                                if (du >= ndong || du < 0)
                                {
                                    d++;
                                    continue;
                                }
                                sum += (int)(gV[i, v + wgs, u + wgs] * image2[dv, du]);
                                d++;
                            }
                        }
                        //sum = sum / d;
                        if (sum < sm) { sm = sum; im = i; }
                        if (sum > sm1) { sm1 = sum; im1 = i; }              /////
                        dem++;
                    }
                    //                 Console.WriteLine(sm.ToString()+"__"+ im.ToString());
                    //gan lai muc xam cho anh
                    //gan huong min cho anh
                    image[x / s, y / s] = im;// sm;// sum;
                    //gan sum min cho anh
              //      image[x/s, y/s] = sm;
                    //image[x, y] = (sm + sm1)/2;
                }
            }
         //   Console.WriteLine(dem);
            // MessageBox.Show("sm+im: " + sm.ToString() + " & " + im.ToString());

            //10 scale : sum min + i min
            max_sum = sm1;
            max_i = im1;

            min_sum = sm;
            min_i = im;

           // Bitmap res = new Bitmap(PCA.matrix_2_image(image, image.NoRows, image.NoCols));

            return image;
        }


  




    }
}
