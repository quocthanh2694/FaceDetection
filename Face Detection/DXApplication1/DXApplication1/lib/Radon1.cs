using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DXApplication1
{
    class Radon1
    {
        const int SIZE = 128;
        public static int[] Orient_x0 = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] Orient_y0 = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7 };

        public static int[] Orient_x30 = new int[15] { 0, 0, -1, -1, -2, -2, -3, -3, 0, 1, 1, 2, 2, 3, 3 };
        public static int[] Orient_y30 = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7 };

        public static int[] Orient_x45 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };
        public static int[] Orient_y45 = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7 };

        public static int[] Orient_x60 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };
        public static int[] Orient_y60 = new int[15] { 0, 0, 1, 1, 2, 2, 3, 3, 0, -1, -1, -2, -2, -3, -3 };

        public static int[] Orient_x90 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };
        public static int[] Orient_y90 = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static int[] Orient_x120 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };
        public static int[] Orient_y120 = new int[15] { 0, 0, -1, -1, -2, -2, -3, -3, 0, 1, 1, 2, 2, 3, 3 };

        public static int[] Orient_x135 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };
        public static int[] Orient_y135 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };

        public static int[] Orient_x175 = new int[15] { 0, 0, -1, -1, -2, -2, -3, -3, 0, 1, 1, 2, 2, 3, 3 };
        public static int[] Orient_y175 = new int[15] { 0, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };

        public static Matrix1 ApdungRadon(Matrix1 original)//,Matrix1 en,  Matrix1 direction)

        {
            Matrix1 direction= new Matrix1(original.NoRows,original.NoCols);
            int i, j;
            //Duyet qua toan bo anh
            double e_min;
            for (i = 0; i < original.NoRows; i++)
                for (j = 0; j < original.NoCols; j++)
                {
                    int dong = i;
                    int cot = j;
                    double e0 = CaCulate_Energy(dong, cot, Orient_x0, Orient_y0, original);
                    // double e15 = CaCulate_Energy(dong, cot, Orient_x15, Orient_y15, original);
                    double e30 = CaCulate_Energy(dong, cot, Orient_x30, Orient_y30, original);
                    //double e45 = CaCulate_Energy(dong, cot, Orient_x45, Orient_y45, original);
                    double e60 = CaCulate_Energy(dong, cot, Orient_x60, Orient_y60, original);
                    // double e75 = CaCulate_Energy(dong, cot, Orient_x75, Orient_y75, original);
                    double e90 = CaCulate_Energy(dong, cot, Orient_x90, Orient_y90, original);
                    //double e105 = CaCulate_Energy(dong, cot, Orient_x105, Orient_y105, original);
                    double e120 = CaCulate_Energy(dong, cot, Orient_x120, Orient_y120, original);
                    //double e135 = CaCulate_Energy(dong, cot, Orient_x135, Orient_y135, original);
                    // double e150 = CaCulate_Energy(dong, cot, Orient_x150, Orient_y150, original);
                    double e175 = CaCulate_Energy(dong, cot, Orient_x175, Orient_y175, original);

                    int _direction = 0;
                    e_min = e0;
                    //if (e_min > e15)
                    //{
                    //    e_min = e15;
                    //    _direction = 15;
                    //}
                    if (e_min > e30)
                    {
                        e_min = e30;
                        _direction = 30;
                    }

                    //if (e_min > e45)
                    //{
                    //    e_min = e45;
                    //    _direction = 45;
                    //}

                    if (e_min > e60)
                    {
                        e_min = e60;
                        _direction = 60;
                    }
                    //if (e_min > e75)
                    //{
                    //    e_min = e75;
                    //    _direction = 75;
                    //}
                    if (e_min > e90)
                    {
                        e_min = e90;
                        _direction = 90;
                    }
                    //if (e_min > e105)
                    //{
                    //    e_min = e105;
                    //    _direction = 105;
                    //}
                    if (e_min > e120)
                    {
                        e_min = e120;
                        _direction = -60;// 120;
                    }

                    //if (e_min > e135)
                    //{
                    //    e_min = e135;
                    //    _direction = -45;// 135;
                    //}

                    //if (e_min > e150)
                    //{
                    //    e_min = e150;
                    //    _direction = 150;
                    //}
                    if (e_min > e175)
                    {
                        e_min = e175;
                        _direction = -30;// 175;
                    }
                  //   en[i, j] = Math.Round(e_min, 3);
                    direction[i, j] = _direction;

                }
            return direction;
        }

        public static Matrix1 RLOC(Matrix1 original, Matrix1 direction)
        {

            int i, j;
            //Duyet qua toan bo anh
            double e_min;
            for (i = 0; i < original.NoRows; i++)
                for (j = 0; j < original.NoCols; j++)
                {
                    int dong = i;
                    int cot = j;
                    double e0 = CaCulate_Energy(dong, cot, Orient_x0, Orient_y0, original);
                    // double e15 = CaCulate_Energy(dong, cot, Orient_x15, Orient_y15, original);
                //    double e30 = CaCulate_Energy(dong, cot, Orient_x30, Orient_y30, original);
                    double e45 = CaCulate_Energy(dong, cot, Orient_x45, Orient_y45, original);
                  //  double e60 = CaCulate_Energy(dong, cot, Orient_x60, Orient_y60, original);
                    // double e75 = CaCulate_Energy(dong, cot, Orient_x75, Orient_y75, original);
                    double e90 = CaCulate_Energy(dong, cot, Orient_x90, Orient_y90, original);

                  //  double e120 = CaCulate_Energy(dong, cot, Orient_x120, Orient_y120, original);
                    double e135 = CaCulate_Energy(dong, cot, Orient_x135, Orient_y135, original);
                    // double e150 = CaCulate_Energy(dong, cot, Orient_x150, Orient_y150, original);
                   // double e175 = CaCulate_Energy(dong, cot, Orient_x175, Orient_y175, original);

                    int _direction = 0;
                    e_min = e0;
                    //if (e_min > e30)
                    //{
                    //    e_min = e30;
                    //    _direction = 30;
                    //}

                    if (e_min > e45)
                    {
                        e_min = e45;
                        _direction = 45;
                    }

                    //if (e_min > e60)
                    //{
                    //    e_min = e60;
                    //    _direction = 60;
                    //}
                    
                    if (e_min > e90)
                    {
                        e_min = e90;
                        _direction = 90;
                    }
                    //if (e_min > e120)
                    //{
                    //    e_min = e120;
                    //    _direction = -60;// 120;
                    //}

                    if (e_min > e135)
                    {
                        e_min = e135;
                        _direction = -45;// 135;
                    }

                   
                    //if (e_min > e175)
                    //{
                    //    e_min = e175;
                    //    _direction = -30;// 175;
                    //}
                   // energy[i, j] = Math.Round(e_min, 3);
                    direction[i, j] = _direction;

                }
            return direction;//energy;
        }



        //Cho truoc diem anh. Duyet qua cac diem anh theo huong do
        public static double CaCulate_Energy(int dong, int cot, int[] Orient_x0, int[] Orient_y0, Matrix1 original)
        {
            int num_of_point = 0;
            double value = 0.0;
            for (int i = 0; i < Orient_x0.Length; i++) // Duyet qua tat ca cac diem trong mat na ma tran p
            {
                int new_dong = dong + Orient_x0[i];
                int new_cot = cot + Orient_y0[i];
                //Neu con nam trong anh
                if (new_dong >= 0 && new_dong < original.NoRows && new_cot >= 0 && new_cot < original.NoCols)
                {

                    value += original[new_dong, new_cot];
                    num_of_point++;
                }
            }
            return value / num_of_point;
        }
    }
}
