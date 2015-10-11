using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;


namespace DXApplication1
{
    #region "Exception in the Library"
    class Matrix1LibraryExceptions : ApplicationException
    { public Matrix1LibraryExceptions(string message) : base(message) { } }

    // The Exceptions in this Class
    class Matrix1NullException : ApplicationException
    {
        public Matrix1NullException()
            :
            base("To do this operation, Matrix1 can not be null") { }
    }
    class Matrix1DimensionException : ApplicationException
    {
        public Matrix1DimensionException()
            :
            base("Dimension of the two matrices not suitable for this operation !") { }
    }
    class Matrix1NotSquare : ApplicationException
    {
        public Matrix1NotSquare()
            :
            base("To do this operation, Matrix1 must be a square Matrix1 !") { }
    }
    class Matrix1DeterminentZero : ApplicationException
    {
        public Matrix1DeterminentZero()
            :
            base("Determinent of Matrix1 equals zero, inverse can't be found !") { }
    }
    class VectorDimensionException : ApplicationException
    {
        public VectorDimensionException()
            :
            base("Dimension of Matrix1 must be [3 , 1] to do this operation !") { }
    }
    class Matrix1SingularException : ApplicationException
    {
        public Matrix1SingularException()
            :
            base("Matrix1 is singular this operation cannot continue !") { }
    }
    #endregion
    public class Matrix1
    {
        private double[,] in_Mat;

        #region "Class Constructor and Indexer"
        /// <summary>
        /// Matrix1 object constructor, constructs an empty
        /// Matrix1 with dimensions: rows = noRows and cols = noCols.
        /// </summary>
        /// <param name="noRows"> no. of rows in this Matrix1 </param>
        /// <param name="noCols"> no. of columns in this Matrix1</param>
        public Matrix1(int noRows, int noCols)
        {
            this.in_Mat = new double[noRows, noCols];
        }

        /// <summary>
        /// Matrix1 object constructor, constructs a Matrix1 from an
        /// already defined array object.
        /// </summary>
        /// <param name="Mat">the array the Matrix1 will contain</param>
        public Matrix1(double[,] Mat)
        {
            this.in_Mat = (double[,])Mat.Clone();
        }

        public Matrix1(Matrix1 Mat)
        {
            //this.in_Mat = (double[,])Mat.Clone();
            int m, n;
            m = Mat.NoRows;
            n = Mat.NoCols;
            double[,] tem = new double[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tem[i, j] = Mat[i, j];
            this.in_Mat = (double[,])tem.Clone();
        }
        /// <summary>
        /// Set or get an element from the Matrix1
        /// </summary>
        public double this[int Row, int Col]
        {
            get { return this.in_Mat[Row, Col]; }
            set { this.in_Mat[Row, Col] = value; }
        }
        #endregion

        #region "Public Properties"
        /// <summary>
        /// Set or get the no. of rows in the Matrix1.
        /// Warning: Setting this property will delete all of
        /// the elements of the Matrix1 and set them to zero.
        /// </summary>
        public int NoRows
        {
            get { return this.in_Mat.GetUpperBound(0) + 1; }
            set { this.in_Mat = new double[value, this.in_Mat.GetUpperBound(0)]; }
        }

        /// <summary>
        /// Set or get the no. of columns in the Matrix1.
        /// Warning: Setting this property will delete all of
        /// the elements of the Matrix1 and set them to zero.
        /// </summary>
        public int NoCols
        {
            get { return this.in_Mat.GetUpperBound(1) + 1; }
            set { this.in_Mat = new double[this.in_Mat.GetUpperBound(0), value]; }
        }

        /// <summary>
        /// This property returns the Matrix1 as an array.
        /// </summary>
        public double[,] toArray
        {
            get { return this.in_Mat; }
        }
        #endregion

        #region "Max and Min element"
        public static double Find_Max(double[,] Mat)
        {
            double res = Mat[0,0];
            for (int i = 0; i < Mat.GetLength(0); i++)
                for (int j = 0; j < Mat.GetLength(1); j++)
                    if (res < Mat[i,j])
                        res = Mat[i,j];
            return res;
        }
        public static double Find_Max(Matrix1 Mat1)
        { return (Find_Max(Mat1.in_Mat)); }

        public static double Find_Min(double[,] Mat)
        {
            double res = Mat[0,0];
            for (int i = 0; i < Mat.GetLength(0); i++)
                for (int j = 0; j < Mat.GetLength(1); j++)
                    if (res > Mat[i,j])
                        res = Mat[i,j];
            return res;
        }
        public static double Find_Min(Matrix1 Mat1)
        { return (Find_Min(Mat1.in_Mat)); }

        #endregion


        #region "Find Rows and Columns in a Matrix1"
        private static void Find_R_C(double[] Mat, out int Row)
        {
            Row = Mat.GetUpperBound(0);
        }

        private static void Find_R_C(double[,] Mat, out int Row, out int Col)
        {
            Row = Mat.GetUpperBound(0);
            Col = Mat.GetUpperBound(1);
        }
        #endregion

        #region "Change 1D array ([n]) to/from 2D array [n,1]"

        /// <summary>
        /// Returns the 2D form of a 1D array. i.e. array with
        /// dimension[n] is returned as an array with dimension [n,1]. 
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat"> 
        /// the array to convert, with dimesion [n] 
        /// </param>
        /// <returns> the same array but with [n,1] dimension </returns>
        public static double[,] OneD_2_TwoD(double[] Mat)
        {
            int Rows;
            //Find The dimensions !!
            try { Find_R_C(Mat, out Rows); }
            catch { throw new Matrix1NullException(); }

            double[,] Sol = new double[Rows + 1, 1];

            for (int i = 0; i <= Rows; i++)
            {
                Sol[i, 0] = Mat[i];
            }

            return Sol;
        }

        /// <summary>
        /// Returns the 1D form of a 2D array. i.e. array with
        /// dimension[n,1] is returned as an array with dimension [n]. 
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">
        /// the array to convert, with dimesions [n,1] 
        /// </param>
        /// <returns>the same array but with [n] dimension</returns>
        public static double[] TwoD_2_OneD(double[,] Mat)
        {
            int Rows;
            int Cols;
            //Find The dimensions !!
            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new Matrix1NullException(); }

            if (Cols != 0) throw new Matrix1DimensionException();

            double[] Sol = new double[Rows + 1];

            for (int i = 0; i <= Rows; i++)
            {
                Sol[i] = Mat[i, 0];
            }
            return Sol;
        }
        #endregion

        #region "Identity Matrix1"
        /// <summary>
        /// Returns an Identity Matrix1 with dimensions [n,n] in the from of an array.
        /// </summary>
        /// <param name="n">the no. of rows or no. cols in the Matrix1</param>
        /// <returns>An identity Matrix1 with dimensions [n,n] in the form of an array</returns>
        public static double[,] Identity(int n)
        {
            double[,] temp = new double[n, n];
            for (int i = 0; i < n; i++) temp[i, i] = 1;
            return temp;
        }
        #endregion

        #region "Add Matrices"
        /// <summary>
        /// Returns the summation of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First array in the summation</param>
        /// <param name="Mat2">Second array in the summation</param>
        /// <returns>Sum of Mat1 and Mat2 as an array</returns>
        public static double[,] Add(double[,] Mat1, double[,] Mat2)
        {
            double[,] sol;
            int i, j;
            int Rows1, Cols1;
            int Rows2, Cols2;

            //Find The dimensions !!
            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new Matrix1NullException();
            }

            if (Rows1 != Rows2 || Cols1 != Cols2) throw new Matrix1DimensionException();

            sol = new double[Rows1 + 1, Cols1 + 1];

            for (i = 0; i <= Rows1; i++)
                for (j = 0; j <= Cols1; j++)
                {
                    sol[i, j] = Mat1[i, j] + Mat2[i, j];
                }

            return sol;
        }

        /// <summary>
        /// Returns the summation of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 in the summation</param>
        /// <param name="Mat2">Second Matrix1 in the summation</param>
        /// <returns>Sum of Mat1 and Mat2 as a Matrix1 object</returns>
        public static Matrix1 Add(Matrix1 Mat1, Matrix1 Mat2)
        { return new Matrix1(Add(Mat1.in_Mat, Mat2.in_Mat)); }

        /// <summary>
        /// Returns the summation of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 object in the summation</param>
        /// <param name="Mat2">Second Matrix1 object in the summation</param>
        /// <returns>Sum of Mat1 and Mat2 as a Matrix1 object</returns>
        public static Matrix1 operator +(Matrix1 Mat1, Matrix1 Mat2)
        { return new Matrix1(Add(Mat1.in_Mat, Mat2.in_Mat)); }
        #endregion

        #region "Subtract Matrices"

        /// <summary>
        /// Returns the difference of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First array in the subtraction</param>
        /// <param name="Mat2">Second array in the subtraction</param>
        /// <returns>Difference of Mat1 and Mat2 as an array</returns>
        public static double[,] Subtract(double[,] Mat1, double[,] Mat2)
        {
            double[,] sol;
            int i, j;
            int Rows1, Cols1;
            int Rows2, Cols2;

            //Find The dimensions !!
            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new Matrix1NullException();
            }

            if (Rows1 != Rows2 || Cols1 != Cols2) throw new Matrix1DimensionException();

            sol = new double[Rows1 + 1, Cols1 + 1];

            for (i = 0; i < Rows1; i++)
                for (j = 0; j < Cols1; j++)
                {
                    sol[i, j] = Mat1[i, j] - Mat2[i, j];
                }

            return sol;
        }

        /// <summary>
        /// Returns the difference of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 in the subtraction</param>
        /// <param name="Mat2">Second Matrix1 in the subtraction</param>
        /// <returns>Difference of Mat1 and Mat2 as a Matrix1 object</returns>
        public static Matrix1 Subtract(Matrix1 Mat1, Matrix1 Mat2)
        { return new Matrix1(Subtract(Mat1.in_Mat, Mat2.in_Mat)); }

        /// <summary>
        /// Returns the difference of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 object in the subtraction</param>
        /// <param name="Mat2">Second Matrix1 object in the subtraction</param>
        /// <returns>Difference of Mat1 and Mat2 as a Matrix1 object</returns>
        public static Matrix1 operator -(Matrix1 Mat1, Matrix1 Mat2)
        { return new Matrix1(Subtract(Mat1.in_Mat, Mat2.in_Mat)); }
        #endregion

        #region "Multiply Matrices"

        /// <summary>
        /// Returns the multiplication of two matrices with compatible 
        /// dimensions.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat1">First array in multiplication</param>
        /// <param name="Mat2">Second array in multiplication</param>
        /// <returns>Mat1 multiplied by Mat2 as an array</returns>
        public static double[,] Multiply(double[,] Mat1, double[,] Mat2)
        {
            double[,] sol;
            int Rows1, Cols1;
            int Rows2, Cols2;
            double MulAdd = 0;

            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new Matrix1NullException();
            }
            if (Cols1 != Rows2) throw new Matrix1DimensionException();

            sol = new double[Rows1 + 1, Cols2 + 1];

            for (int i = 0; i <= Rows1; i++)
                for (int j = 0; j <= Cols2; j++)
                {
                    for (int l = 0; l <= Cols1; l++)
                    {
                        MulAdd = MulAdd + Mat1[i, l] * Mat2[l, j];
                    }
                    sol[i, j] = MulAdd;
                    MulAdd = 0;
                }
            return sol;
        }

        /// <summary>
        /// Returns the multiplication of two matrices with compatible 
        /// dimensions OR the cross-product of two vectors.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat1">
        /// First Matrix1 or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <param name="Mat2">
        /// Second Matrix1 or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <returns>Mat1 multiplied by Mat2 as a Matrix1 object</returns>
        public static Matrix1 Multiply(Matrix1 Mat1, Matrix1 Mat2)
        {
            if ((Mat1.NoRows == 3) && (Mat2.NoRows == 3) &&
                (Mat1.NoCols == 1) && (Mat1.NoCols == 1))
            { return new Matrix1(CrossProduct(Mat1.in_Mat, Mat2.in_Mat)); }
            else
            { return new Matrix1(Multiply(Mat1.in_Mat, Mat2.in_Mat)); }
        }

        /// <summary>
        /// Returns the multiplication of two matrices with compatible 
        /// dimensions OR the cross-product of two vectors.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat1">
        /// First Matrix1 or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <param name="Mat2">
        /// Second Matrix1 or vector (i.e: dimension [3,1]) object in 
        /// multiplication
        /// </param>
        /// <returns>Mat1 multiplied by Mat2 as a Matrix1 object</returns>
        public static Matrix1 operator *(Matrix1 Mat1, Matrix1 Mat2)
        {
            if ((Mat1.NoRows == 3) && (Mat2.NoRows == 3) &&
                (Mat1.NoCols == 1) && (Mat1.NoCols == 1))
            {
                return new Matrix1(CrossProduct(Mat1.in_Mat, Mat2.in_Mat));
            }
            else
            {
                return new Matrix1(Multiply(Mat1.in_Mat, Mat2.in_Mat));
            }
        }
        #endregion

        #region "Determinant of a Matrix1"
        /// <summary>
        /// Returns the determinant of a Matrix1 with [n,n] dimension.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Array with [n,n] dimension whose determinant is to be found
        /// </param>
        /// <returns>Determinant of the array</returns>
        public static double Det(double[,] Mat)
        {
            int S, k, k1, i, j;
            double[,] DArray;
            double save, ArrayK, tmpDet;
            int Rows, Cols;

            try
            {
                DArray = (double[,])Mat.Clone();
                Find_R_C(Mat, out Rows, out Cols);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows != Cols) throw new Matrix1NotSquare();

            S = Rows;
            tmpDet = 1;

            for (k = 0; k <= S; k++)
            {
                if (DArray[k, k] == 0)
                {
                    j = k;
                    while ((j < S) && (DArray[k, j] == 0)) j = j + 1;
                    if (DArray[k, j] == 0) return 0;
                    else
                    {
                        for (i = k; i <= S; i++)
                        {
                            save = DArray[i, j];
                            DArray[i, j] = DArray[i, k];
                            DArray[i, k] = save;
                        }
                    }
                    tmpDet = -tmpDet;
                }
                ArrayK = DArray[k, k];
                tmpDet = tmpDet * ArrayK;
                if (k < S)
                {
                    k1 = k + 1;
                    for (i = k1; i <= S; i++)
                    {
                        for (j = k1; j <= S; j++)
                            DArray[i, j] = DArray[i, j] - DArray[i, k] * (DArray[k, j] / ArrayK);
                    }
                }
            }
            return tmpDet;
        }

        /// <summary>
        /// Returns the determinant of a Matrix1 with [n,n] dimension.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Matrix1 object with [n,n] dimension whose determinant is to be found
        /// </param>
        /// <returns>Determinant of the Matrix1 object</returns>
        public static double Det(Matrix1 Mat)
        { return Det(Mat.in_Mat); }
        #endregion

        #region "Inverse of a Matrix1"
        /// <summary>
        /// Returns the inverse of a Matrix1 with [n,n] dimension 
        /// and whose determinant is not zero.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Array with [n,n] dimension whose inverse is to be found
        /// </param>
        /// <returns>Inverse of the array as an array</returns>
        public static double[,] Inverse(double[,] Mat)
        {
            double[,] AI, Mat1;
            double AIN, AF;
            int Rows, Cols;
            int LL, LLM, L1, L2, LC, LCA, LCB;

            try
            {
                Find_R_C(Mat, out Rows, out Cols);
                Mat1 = (double[,])Mat.Clone();
            }
            catch { throw new Matrix1NullException(); }

            if (Rows != Cols) throw new Matrix1NotSquare();
            if (Det(Mat) == 0) throw new Matrix1DeterminentZero();

            LL = Rows;
            LLM = Cols;
            AI = new double[LL + 1, LL + 1];

            for (L2 = 0; L2 <= LL; L2++)
            {
                for (L1 = 0; L1 <= LL; L1++) AI[L1, L2] = 0;
                AI[L2, L2] = 1;
            }

            for (LC = 0; LC <= LL; LC++)
            {
                if (Math.Abs(Mat1[LC, LC]) < 0.0000000001)
                {
                    for (LCA = LC + 1; LCA <= LL; LCA++)
                    {
                        if (LCA == LC) continue;
                        if (Math.Abs(Mat1[LC, LCA]) > 0.0000000001)
                        {
                            for (LCB = 0; LCB <= LL; LCB++)
                            {
                                Mat1[LCB, LC] = Mat1[LCB, LC] + Mat1[LCB, LCA];
                                AI[LCB, LC] = AI[LCB, LC] + AI[LCB, LCA];
                            }
                            break;
                        }
                    }
                }
                AIN = 1 / Mat1[LC, LC];
                for (LCA = 0; LCA <= LL; LCA++)
                {
                    Mat1[LCA, LC] = AIN * Mat1[LCA, LC];
                    AI[LCA, LC] = AIN * AI[LCA, LC];
                }

                for (LCA = 0; LCA <= LL; LCA++)
                {
                    if (LCA == LC) continue;
                    AF = Mat1[LC, LCA];
                    for (LCB = 0; LCB <= LL; LCB++)
                    {
                        Mat1[LCB, LCA] = Mat1[LCB, LCA] - AF * Mat1[LCB, LC];
                        AI[LCB, LCA] = AI[LCB, LCA] - AF * AI[LCB, LC];
                    }
                }
            }
            return AI;
        }

        /// <summary>
        /// Returns the inverse of a Matrix1 with [n,n] dimension 
        /// and whose determinant is not zero.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">
        /// Matrix1 object with [n,n] dimension whose inverse is to be found
        /// </param>
        /// <returns>Inverse of the Matrix1 as a Matrix1 object</returns>
        public static Matrix1 Inverse(Matrix1 Mat)
        { return new Matrix1(Inverse(Mat.in_Mat)); }
        #endregion

        #region "Transpose of a Matrix1"
        /// <summary>
        /// Returns the transpose of a Matrix1.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">Array whose transpose is to be found</param>
        /// <returns>Transpose of the array as an array</returns>
        public static double[,] Transpose(double[,] Mat)
        {
            double[,] Tr_Mat;
            int i, j, Rows, Cols;

            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new Matrix1NullException(); }

            Tr_Mat = new double[Cols + 1, Rows + 1];

            for (i = 0; i <= Rows; i++)
                for (j = 0; j <= Cols; j++) 
                    Tr_Mat[j, i] = Mat[i, j];

            return Tr_Mat;
        }



        /// <summary>
        /// Returns the transpose of a Matrix1.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">Matrix1 object whose transpose is to be found</param>
        /// <returns>Transpose of the Matrix1 object as a Matrix1 object</returns>
        public static Matrix1 Transpose(Matrix1 Mat)
        { return new Matrix1(Transpose(Mat.in_Mat)); }
        #endregion

        #region "Singula Value Decomposition of a Matrix1"
        /// <summary>
        /// Evaluates the Singular Value Decomposition of a Matrix1, 
        /// returns the  matrices S, U and V. Such that a given
        /// Matrix1 = U x S x V'.
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'Singular Value Decomposition'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat_">Array whose SVD is to be computed</param>
        /// <param name="S_">An array where the S Matrix1 is returned</param>
        /// <param name="U_">An array where the U Matrix1 is returned</param>
        /// <param name="V_">An array where the V Matrix1 is returned</param>
        public static void SVD(double[,] Mat_, out double[,] S_, out double[,] U_, out double[,] V_)
        {
            int Rows, Cols;
            int m, MP, n, NP;
            double[] w;
            double[,] A, v;

            try { Find_R_C(Mat_, out Rows, out Cols); }
            catch { throw new Matrix1NullException(); }

            m = Rows + 1;
            n = Cols + 1;

            if (m < n)
            {
                m = n;
                MP = NP = n;
            }
            else if (m > n)
            {
                n = m;
                NP = MP = m;
            }
            else
            {
                MP = m;
                NP = n;
            }

            A = new double[m + 1, n + 1];

            for (int row = 1; row <= Rows + 1; row++)
                for (int col = 1; col <= Cols + 1; col++)
                { A[row, col] = Mat_[row - 1, col - 1]; }

            const int NMAX = 100;
            v = new double[NP + 1, NP + 1];
            w = new double[NP + 1];

            int k, l, nm;
            int flag, i, its, j, jj;

            double[,] U_temp, S_temp, V_temp;
            double anorm, c, f, g, h, s, scale, x, y, z;
            double[] rv1 = new double[NMAX];

            l = 0;
            nm = 0;
            g = 0.0;
            scale = 0.0;
            anorm = 0.0;

            for (i = 1; i <= n; i++)
            {
                l = i + 1;
                rv1[i] = scale * g;
                g = s = scale = 0.0;
                if (i <= m)
                {
                    for (k = i; k <= m; k++) scale += Math.Abs(A[k, i]);
                    if (scale != 0)
                    {
                        for (k = i; k <= m; k++)
                        {
                            A[k, i] /= scale;
                            s += A[k, i] * A[k, i];
                        }
                        f = A[i, i];
                        g = -Sign(Math.Sqrt(s), f);
                        h = f * g - s;
                        A[i, i] = f - g;
                        if (i != n)
                        {
                            for (j = l; j <= n; j++)
                            {
                                for (s = 0, k = i; k <= m; k++) s += A[k, i] * A[k, j];
                                f = s / h;
                                for (k = i; k <= m; k++) A[k, j] += f * A[k, i];
                            }
                        }
                        for (k = i; k <= m; k++) A[k, i] *= scale;
                    }
                }
                w[i] = scale * g;
                g = s = scale = 0.0;
                if (i <= m && i != n)
                {
                    for (k = l; k <= n; k++) scale += Math.Abs(A[i, k]);
                    if (scale != 0)
                    {
                        for (k = l; k <= n; k++)
                        {
                            A[i, k] /= scale;
                            s += A[i, k] * A[i, k];
                        }
                        f = A[i, l];
                        g = -Sign(Math.Sqrt(s), f);
                        h = f * g - s;
                        A[i, l] = f - g;
                        for (k = l; k <= n; k++) rv1[k] = A[i, k] / h;
                        if (i != m)
                        {
                            for (j = l; j <= m; j++)
                            {
                                for (s = 0.0, k = l; k <= n; k++) s += A[j, k] * A[i, k];
                                for (k = l; k <= n; k++) A[j, k] += s * rv1[k];
                            }
                        }
                        for (k = l; k <= n; k++) A[i, k] *= scale;
                    }
                }
                anorm = Math.Max(anorm, (Math.Abs(w[i]) + Math.Abs(rv1[i])));
            }
            for (i = n; i >= 1; i--)
            {
                if (i < n)
                {
                    if (g != 0)
                    {
                        for (j = l; j <= n; j++)
                            v[j, i] = (A[i, j] / A[i, l]) / g;
                        for (j = l; j <= n; j++)
                        {
                            for (s = 0.0, k = l; k <= n; k++) s += A[i, k] * v[k, j];
                            for (k = l; k <= n; k++) v[k, j] += s * v[k, i];
                        }
                    }
                    for (j = l; j <= n; j++) v[i, j] = v[j, i] = 0.0;
                }
                v[i, i] = 1.0;
                g = rv1[i];
                l = i;
            }
            for (i = n; i >= 1; i--)
            {
                l = i + 1;
                g = w[i];
                if (i < n)
                    for (j = l; j <= n; j++) A[i, j] = 0.0;
                if (g != 0)
                {
                    g = 1.0 / g;
                    if (i != n)
                    {
                        for (j = l; j <= n; j++)
                        {
                            for (s = 0.0, k = l; k <= m; k++) s += A[k, i] * A[k, j];
                            f = (s / A[i, i]) * g;
                            for (k = i; k <= m; k++) A[k, j] += f * A[k, i];
                        }
                    }
                    for (j = i; j <= m; j++) A[j, i] *= g;
                }
                else
                {
                    for (j = i; j <= m; j++) A[j, i] = 0.0;
                }
                ++A[i, i];
            }
            for (k = n; k >= 1; k--)
            {
                for (its = 1; its <= 30; its++)
                {
                    flag = 1;
                    for (l = k; l >= 1; l--)
                    {
                        nm = l - 1;
                        if (Math.Abs(rv1[l]) + anorm == anorm)
                        {
                            flag = 0;
                            break;
                        }
                        if (Math.Abs(w[nm]) + anorm == anorm) break;
                    }
                    if (flag != 0)
                    {
                        c = 0.0;
                        s = 1.0;
                        for (i = l; i <= k; i++)
                        {
                            f = s * rv1[i];
                            if (Math.Abs(f) + anorm != anorm)
                            {
                                g = w[i];
                                h = PYTHAG(f, g);
                                w[i] = h;
                                h = 1.0 / h;
                                c = g * h;
                                s = (-f * h);
                                for (j = 1; j <= m; j++)
                                {
                                    y = A[j, nm];
                                    z = A[j, i];
                                    A[j, nm] = y * c + z * s;
                                    A[j, i] = z * c - y * s;
                                }
                            }
                        }
                    }
                    z = w[k];
                    if (l == k)
                    {
                        if (z < 0.0)
                        {
                            w[k] = -z;
                            for (j = 1; j <= n; j++) v[j, k] = (-v[j, k]);
                        }
                        break;
                    }
                    if (its == 30) Console.WriteLine("No convergence in 30 SVDCMP iterations");
                    x = w[l];
                    nm = k - 1;
                    y = w[nm];
                    g = rv1[nm];
                    h = rv1[k];
                    f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y);
                    g = PYTHAG(f, 1.0);
                    f = ((x - z) * (x + z) + h * ((y / (f + Sign(g, f))) - h)) / x;
                    c = s = 1.0;
                    for (j = l; j <= nm; j++)
                    {
                        i = j + 1;
                        g = rv1[i];
                        y = w[i];
                        h = s * g;
                        g = c * g;
                        z = PYTHAG(f, h);
                        rv1[j] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = g * c - x * s;
                        h = y * s;
                        y = y * c;
                        for (jj = 1; jj <= n; jj++)
                        {
                            x = v[jj, j];
                            z = v[jj, i];
                            v[jj, j] = x * c + z * s;
                            v[jj, i] = z * c - x * s;
                        }
                        z = PYTHAG(f, h);
                        w[j] = z;
                        if (z != 0)
                        {
                            z = 1.0 / z;
                            c = f * z;
                            s = h * z;
                        }
                        f = (c * g) + (s * y);
                        x = (c * y) - (s * g);
                        for (jj = 1; jj <= m; jj++)
                        {
                            y = A[jj, j];
                            z = A[jj, i];
                            A[jj, j] = y * c + z * s;
                            A[jj, i] = z * c - y * s;
                        }
                    }
                    rv1[l] = 0.0;
                    rv1[k] = f;
                    w[k] = x;
                }
            }

            S_temp = new double[NP, NP];
            V_temp = new double[NP, NP];
            U_temp = new double[MP, NP];

            for (i = 1; i <= NP; i++) S_temp[i - 1, i - 1] = w[i];

            S_ = S_temp;

            for (i = 1; i <= NP; i++)
                for (j = 1; j <= NP; j++) V_temp[i - 1, j - 1] = v[i, j];

            V_ = V_temp;

            for (i = 1; i <= MP; i++)
                for (j = 1; j <= NP; j++) U_temp[i - 1, j - 1] = A[i, j];

            U_ = U_temp;
        }

        private static double SQR(double a)
        {
            return a * a;
        }

        private static double Sign(double a, double b)
        {
            if (b >= 0.0) { return Math.Abs(a); }
            else { return -Math.Abs(a); }
        }

        private static double PYTHAG(double a, double b)
        {
            double absa, absb;

            absa = Math.Abs(a);
            absb = Math.Abs(b);
            if (absa > absb) return absa * Math.Sqrt(1.0 + SQR(absb / absa));
            else return (absb == 0.0 ? 0.0 : absb * Math.Sqrt(1.0 + SQR(absa / absb)));
        }

        /// <summary>
        /// Evaluates the Singular Value Decomposition of a Matrix1, 
        /// returns the  matrices S, U and V. Such that a given
        /// Matrix1 = U x S x V'.
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'Singular Value Decomposition'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="Mat">Matrix1 object whose SVD is to be computed</param>
        /// <param name="S">A Matrix1 object where the S Matrix1 is returned</param>
        /// <param name="U">A Matrix1 object where the U Matrix1 is returned</param>
        /// <param name="V">A Matrix1 object where the V Matrix1 is returned</param>
        public static void SVD(Matrix1 Mat, out Matrix1 S, out Matrix1 U, out Matrix1 V)
        {
            double[,] s, u, v;
            SVD(Mat.in_Mat, out s, out u, out v);
            S = new Matrix1(s);
            U = new Matrix1(u);
            V = new Matrix1(v);
        }
        #endregion

        #region "LU Decomposition of a Matrix1"
        /// <summary>
        /// Returns the LU Decomposition of a Matrix1. 
        /// the output is: lower triangular Matrix1 L, upper
        /// triangular Matrix1 U, and permutation Matrix1 P so that
        ///	P*X = L*U.
        /// In case of an error the error is raised as an exception.
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat">Array which will be LU Decomposed</param>
        /// <param name="L">An array where the lower traingular Matrix1 is returned</param>
        /// <param name="U">An array where the upper traingular Matrix1 is returned</param>
        /// <param name="P">An array where the permutation Matrix1 is returned</param>
        public static void LU(double[,] Mat, out double[,] L, out double[,] U, out double[,] P)
        {
            double[,] A;
            int i, j, k, Rows, Cols;

            try
            {
                A = (double[,])Mat.Clone();
                Find_R_C(Mat, out Rows, out Cols);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows != Cols) throw new Matrix1NotSquare();

            int IMAX = 0, N = Rows;
            double AAMAX, Sum, Dum, TINY = 1E-20;

            int[] INDX = new int[N + 1];
            double[] VV = new double[N * 10];
            double D = 1.0;

            for (i = 0; i <= N; i++)
            {
                AAMAX = 0.0;
                for (j = 0; j <= N; j++)
                    if (Math.Abs(A[i, j]) > AAMAX) AAMAX = Math.Abs(A[i, j]);
                if (AAMAX == 0.0) throw new Matrix1SingularException();
                VV[i] = 1.0 / AAMAX;
            }
            for (j = 0; j <= N; j++)
            {
                if (j > 0)
                {
                    for (i = 0; i <= (j - 1); i++)
                    {
                        Sum = A[i, j];
                        if (i > 0)
                        {
                            for (k = 0; k <= (i - 1); k++)
                                Sum = Sum - A[i, k] * A[k, j];
                            A[i, j] = Sum;
                        }
                    }
                }
                AAMAX = 0.0;
                for (i = j; i <= N; i++)
                {
                    Sum = A[i, j];
                    if (j > 0)
                    {
                        for (k = 0; k <= (j - 1); k++)
                            Sum = Sum - A[i, k] * A[k, j];
                        A[i, j] = Sum;
                    }
                    Dum = VV[i] * Math.Abs(Sum);
                    if (Dum >= AAMAX)
                    {
                        IMAX = i;
                        AAMAX = Dum;
                    }
                }
                if (j != IMAX)
                {
                    for (k = 0; k <= N; k++)
                    {
                        Dum = A[IMAX, k];
                        A[IMAX, k] = A[j, k];
                        A[j, k] = Dum;
                    }
                    D = -D;
                    VV[IMAX] = VV[j];
                }
                INDX[j] = IMAX;
                if (j != N)
                {
                    if (A[j, j] == 0.0) A[j, j] = TINY;
                    Dum = 1.0 / A[j, j];
                    for (i = j + 1; i <= N; i++)
                        A[i, j] = A[i, j] * Dum;

                }
            }

            if (A[N, N] == 0.0) A[N, N] = TINY;

            int count = 0;
            double[,] l = new double[N + 1, N + 1];
            double[,] u = new double[N + 1, N + 1];

            for (i = 0; i <= N; i++)
            {
                for (j = 0; j <= count; j++)
                {
                    if (i != 0) l[i, j] = A[i, j];
                    if (i == j) l[i, j] = 1.0;
                    u[N - i, N - j] = A[N - i, N - j];
                }
                count++;
            }

            L = l;
            U = u;

            P = Identity(N + 1);
            for (i = 0; i <= N; i++)
            {
                SwapRows(P, i, INDX[i]);
            }
        }

        private static void SwapRows(double[,] Mat, int Row, int toRow)
        {
            int N = Mat.GetUpperBound(0);
            double[,] dumArray = new double[1, N + 1];
            for (int i = 0; i <= N; i++)
            {
                dumArray[0, i] = Mat[Row, i];
                Mat[Row, i] = Mat[toRow, i];
                Mat[toRow, i] = dumArray[0, i];
            }
        }

        /// <summary>
        /// Returns the LU Decomposition of a Matrix1. 
        /// the output is: lower triangular Matrix1 L, upper
        /// triangular Matrix1 U, and permutation Matrix1 P so that
        ///	P*X = L*U.
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat">Matrix1 object which will be LU Decomposed</param>
        /// <param name="L">A Matrix1 object where the lower traingular Matrix1 is returned</param>
        /// <param name="U">A Matrix1 object where the upper traingular Matrix1 is returned</param>
        /// <param name="P">A Matrix1 object where the permutation Matrix1 is returned</param>
        public static void LU(Matrix1 Mat, out Matrix1 L, out Matrix1 U, out Matrix1 P)
        {
            double[,] l, u, p;
            LU(Mat.in_Mat, out l, out u, out p);
            L = new Matrix1(l);
            U = new Matrix1(u);
            P = new Matrix1(p);
        }
        #endregion

        #region "Solve system of linear equations"
        /// <summary>
        /// Solves a set of n linear equations A.X = B, and returns
        /// X, where A is [n,n] and B is [n,1]. 
        /// In the same manner if you need to compute: inverse(A).B, it is
        /// better to use this method instead, as it is much faster.  
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="MatA">The array 'A' on the left side of the equations A.X = B</param>
        /// <param name="MatB">The array 'B' on the right side of the equations A.X = B</param>
        /// <returns>Array 'X' in the system of equations A.X = B</returns>
        public static double[,] SolveLinear(double[,] MatA, double[,] MatB)
        {
            double[,] A;
            double[,] B;
            double SUM;
            int i, ii, j, k, ll, Rows, Cols;

            #region "LU Decompose"
            try
            {
                A = (double[,])MatA.Clone();
                B = (double[,])MatB.Clone();
                Find_R_C(A, out Rows, out Cols);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows != Cols) throw new Matrix1NotSquare();
            if ((B.GetUpperBound(0) != Rows) || (B.GetUpperBound(1) != 0))
                throw new Matrix1DimensionException();

            int IMAX = 0, N = Rows;
            double AAMAX, Sum, Dum, TINY = 1E-20;

            int[] INDX = new int[N + 1];
            double[] VV = new double[N * 10];
            double D = 1.0;

            for (i = 0; i <= N; i++)
            {
                AAMAX = 0.0;
                for (j = 0; j <= N; j++)
                    if (Math.Abs(A[i, j]) > AAMAX) AAMAX = Math.Abs(A[i, j]);
                if (AAMAX == 0.0) throw new Matrix1SingularException();
                VV[i] = 1.0 / AAMAX;
            }
            for (j = 0; j <= N; j++)
            {
                if (j > 0)
                {
                    for (i = 0; i <= (j - 1); i++)
                    {
                        Sum = A[i, j];
                        if (i > 0)
                        {
                            for (k = 0; k <= (i - 1); k++)
                                Sum = Sum - A[i, k] * A[k, j];
                            A[i, j] = Sum;
                        }
                    }
                }
                AAMAX = 0.0;
                for (i = j; i <= N; i++)
                {
                    Sum = A[i, j];
                    if (j > 0)
                    {
                        for (k = 0; k <= (j - 1); k++)
                            Sum = Sum - A[i, k] * A[k, j];
                        A[i, j] = Sum;
                    }
                    Dum = VV[i] * Math.Abs(Sum);
                    if (Dum >= AAMAX)
                    {
                        IMAX = i;
                        AAMAX = Dum;
                    }
                }
                if (j != IMAX)
                {
                    for (k = 0; k <= N; k++)
                    {
                        Dum = A[IMAX, k];
                        A[IMAX, k] = A[j, k];
                        A[j, k] = Dum;
                    }
                    D = -D;
                    VV[IMAX] = VV[j];
                }
                INDX[j] = IMAX;
                if (j != N)
                {
                    if (A[j, j] == 0.0) A[j, j] = TINY;
                    Dum = 1.0 / A[j, j];
                    for (i = j + 1; i <= N; i++)
                        A[i, j] = A[i, j] * Dum;

                }
            }
            if (A[N, N] == 0.0) A[N, N] = TINY;
            #endregion

            ii = -1;
            for (i = 0; i <= N; i++)
            {
                ll = INDX[i];
                SUM = B[ll, 0];
                B[ll, 0] = B[i, 0];
                if (ii != -1)
                {
                    for (j = ii; j <= i - 1; j++) SUM = SUM - A[i, j] * B[j, 0];
                }
                else if (SUM != 0) ii = i;
                B[i, 0] = SUM;
            }
            for (i = N; i >= 0; i--)
            {
                SUM = B[i, 0];
                if (i < N)
                {
                    for (j = i + 1; j <= N; j++) SUM = SUM - A[i, j] * B[j, 0];
                }
                B[i, 0] = SUM / A[i, i];
            }
            return B;
        }

        /// <summary>
        /// Solves a set of n linear equations A.X = B, and returns
        /// X, where A is [n,n] and B is [n,1]. 
        /// In the same manner if you need to compute: inverse(A).B, it is
        /// better to use this method instead, as it is much faster.  
        /// In case of an error the error is raised as an exception. 
        /// Note: This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="MatA">Matrix1 object 'A' on the left side of the equations A.X = B</param>
        /// <param name="MatB">Matrix1 object 'B' on the right side of the equations A.X = B</param>
        /// <returns>Matrix1 object 'X' in the system of equations A.X = B</returns>
        public static Matrix1 SolveLinear(Matrix1 MatA, Matrix1 MatB)
        { return new Matrix1(Matrix1.SolveLinear(MatA.in_Mat, MatB.in_Mat)); }
        #endregion

        #region "Rank of a Matrix1"
        /// <summary>
        /// Returns the rank of a Matrix1.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">An array whose rank is to be found</param>
        /// <returns>The rank of the array</returns>
        public static int Rank(double[,] Mat)
        {
            int r = 0;
            double[,] S, U, V;
            try
            {
                int Rows, Cols;
                Find_R_C(Mat, out Rows, out Cols);
            }
            catch { throw new Matrix1NullException(); }
            double EPS = 2.2204E-16;
            SVD(Mat, out S, out U, out V);

            for (int i = 0; i <= S.GetUpperBound(0); i++)
            { if (Math.Abs(S[i, i]) > EPS) r++; }

            return r;
        }

        /// <summary>
        /// Returns the rank of a Matrix1.
        /// In case of an error the error is raised as an exception. 
        /// </summary>
        /// <param name="Mat">a Matrix1 object whose rank is to be found</param>
        /// <returns>The rank of the Matrix1 object</returns>
        public static int Rank(Matrix1 Mat)
        { return Rank(Mat.in_Mat); }
        #endregion

        #region "Pseudoinverse of a Matrix1"
        /// <summary>
        /// Returns the pseudoinverse of a Matrix1, such that
        /// X = PINV(A) produces a Matrix1 'X' of the same dimensions
        /// as A' so that A*X*A = A, X*A*X = X.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">An array whose pseudoinverse is to be found</param>
        /// <returns>The pseudoinverse of the array as an array</returns>
        public static double[,] PINV(double[,] Mat)
        {
            double[,] Matrix1;
            int i, m, n, j, l;
            double[,] S, Part_I, Part_II;
            double EPS, MulAdd, Tol, Largest_Item = 0;

            double[,] svdU, svdS, svdV;

            try
            {
                Matrix1 = (double[,])Mat.Clone();
                Find_R_C(Mat, out m, out n);
            }
            catch { throw new Matrix1NullException(); }

            SVD(Mat, out svdS, out svdU, out svdV);

            EPS = 2.2204E-16;
            m++;
            n++;

            Part_I = new double[m, n];
            Part_II = new double[m, n];
            S = new Double[n, n];

            MulAdd = 0;
            for (i = 0; i <= svdS.GetUpperBound(0); i++)
            {
                if (i == 0) Largest_Item = svdS[0, 0];
                if (Largest_Item < svdS[i, i]) Largest_Item = svdS[i, i];
            }

            if (m > n) Tol = m * Largest_Item * EPS;
            else Tol = n * Largest_Item * EPS;

            for (i = 0; i < n; i++) S[i, i] = svdS[i, i];

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    for (l = 0; l < n; l++)
                    {
                        if (S[l, j] > Tol) MulAdd += svdU[i, l] * (1 / S[l, j]);
                    }
                    Part_I[i, j] = MulAdd;
                    MulAdd = 0;
                }
            }

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    for (l = 0; l < n; l++)
                    {
                        MulAdd += Part_I[i, l] * svdV[j, l];
                    }
                    Part_II[i, j] = MulAdd;
                    MulAdd = 0;
                }
            }
            return Transpose(Part_II);
        }

        /// <summary>
        /// Returns the pseudoinverse of a Matrix1, such that
        /// X = PINV(A) produces a Matrix1 'X' of the same dimensions
        /// as A' so that A*X*A = A, X*A*X = X.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">a Matrix1 object whose pseudoinverse is to be found</param>
        /// <returns>The pseudoinverse of the Matrix1 object as a Matrix1 Object</returns>
        public static Matrix1 PINV(Matrix1 Mat)
        { return new Matrix1(PINV(Mat.in_Mat)); }
        #endregion

        #region "Eigen Values and Vactors of Symmetric Matrix1"
        /// <summary>
        /// Returns the Eigenvalues and Eigenvectors of a real symmetric
        /// Matrix1, which is of dimensions [n,n]. 
        /// In case of an error the error is raised as an exception.
        /// Note: This method is based on the 'Eigenvalues and Eigenvectors of a TridiagonalMatrix1'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="Mat">
        /// The array whose Eigenvalues and Eigenvectors are to be found
        /// </param>
        /// <param name="d">An array where the eigenvalues are returned</param>
        /// <param name="v">An array where the eigenvectors are returned</param>
        public static void Eigen(double[,] Mat, out double[,] d, out double[,] v)
        {

            double[,] a;
            int Rows, Cols;
            try
            {
                Find_R_C(Mat, out Rows, out Cols);
                a = (double[,])Mat.Clone();
            }
            catch { throw new Matrix1NullException(); }

            if (Rows != Cols) throw new Matrix1NotSquare();

            int j, iq, ip, i, n, nrot;
            double tresh, theta, tau, t, sm, s, h, g, c;
            double[] b, z;

            n = Rows;
            d = new double[n + 1, 1];
            v = new double[n + 1, n + 1];

            b = new double[n + 1];
            z = new double[n + 1];
            for (ip = 0; ip <= n; ip++)
            {
                for (iq = 0; iq <= n; iq++) v[ip, iq] = 0.0;
                v[ip, ip] = 1.0;
            }
            for (ip = 0; ip <= n; ip++)
            {
                b[ip] = d[ip, 0] = a[ip, ip];
                z[ip] = 0.0;
            }

            nrot = 0;
            for (i = 0; i <= 50; i++)
            {
                sm = 0.0;
                for (ip = 0; ip <= n - 1; ip++)
                {
                    for (iq = ip + 1; iq <= n; iq++)
                        sm += Math.Abs(a[ip, iq]);
                }
                if (sm == 0.0)
                {
                    return;
                }
                if (i < 4)
                    tresh = 0.2 * sm / (n * n);
                else
                    tresh = 0.0;
                for (ip = 0; ip <= n - 1; ip++)
                {
                    for (iq = ip + 1; iq <= n; iq++)
                    {
                        g = 100.0 * Math.Abs(a[ip, iq]);
                        if (i > 4 && (double)(Math.Abs(d[ip, 0]) + g) == (double)Math.Abs(d[ip, 0])
                            && (double)(Math.Abs(d[iq, 0]) + g) == (double)Math.Abs(d[iq, 0]))
                            a[ip, iq] = 0.0;
                        else if (Math.Abs(a[ip, iq]) > tresh)
                        {
                            h = d[iq, 0] - d[ip, 0];
                            if ((double)(Math.Abs(h) + g) == (double)Math.Abs(h))
                                t = (a[ip, iq]) / h;
                            else
                            {
                                theta = 0.5 * h / (a[ip, iq]);
                                t = 1.0 / (Math.Abs(theta) + Math.Sqrt(1.0 + theta * theta));
                                if (theta < 0.0) t = -t;
                            }
                            c = 1.0 / Math.Sqrt(1 + t * t);
                            s = t * c;
                            tau = s / (1.0 + c);
                            h = t * a[ip, iq];
                            z[ip] -= h;
                            z[iq] += h;
                            d[ip, 0] -= h;
                            d[iq, 0] += h;
                            a[ip, iq] = 0.0;
                            for (j = 0; j <= ip - 1; j++)
                            {
                                ROT(g, h, s, tau, a, j, ip, j, iq);
                            }
                            for (j = ip + 1; j <= iq - 1; j++)
                            {
                                ROT(g, h, s, tau, a, ip, j, j, iq);
                            }
                            for (j = iq + 1; j <= n; j++)
                            {
                                ROT(g, h, s, tau, a, ip, j, iq, j);
                            }
                            for (j = 0; j <= n; j++)
                            {
                                ROT(g, h, s, tau, v, j, ip, j, iq);
                            }
                            ++(nrot);
                        }
                    }
                }
                for (ip = 0; ip <= n; ip++)
                {
                    b[ip] += z[ip];
                    d[ip, 0] = b[ip];
                    z[ip] = 0.0;
                }
            }
            Console.WriteLine("Too many iterations in routine jacobi");
        }

        private static void ROT(double g, double h, double s, double tau,
            double[,] a, int i, int j, int k, int l)
        {
            g = a[i, j]; h = a[k, l];
            a[i, j] = g - s * (h + g * tau);
            a[k, l] = h + s * (g - h * tau);
        }

        /// <summary>
        /// Returns the Eigenvalues and Eigenvectors of a real symmetric
        /// Matrix1, which is of dimensions [n,n]. In case of an error the
        /// error is raised as an exception.
        /// Note: This method is based on the 'Eigenvalues and Eigenvectors of a TridiagonalMatrix1'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.
        /// </summary>
        /// <param name="Mat">
        /// The Matrix1 object whose Eigenvalues and Eigenvectors are to be found
        /// </param>
        /// <param name="d">A Matrix1 object where the eigenvalues are returned</param>
        /// <param name="v">A Matrix1 object where the eigenvectors are returned</param>
        public static void Eigen(Matrix1 Mat, out Matrix1 d, out Matrix1 v)
        {
            double[,] D, V;
            Eigen(Mat.in_Mat, out D, out V);
            d = new Matrix1(D);
            v = new Matrix1(V);
        }
        #endregion

        #region "Multiply a Matrix1 or a vector with a scalar quantity"
        /// <summary>
        /// Returns the multiplication of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to multiply the array</param>
        /// <param name="Mat">Array which is to be multiplied by a scalar</param>
        /// <returns>The multiplication of the scalar and the array as an array</returns>
        public static double[,] ScalarMultiply(double Value, double[,] Mat)
        {
            int i, j, Rows, Cols;
            double[,] sol;

            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new Matrix1NullException(); }
            sol = new double[Rows + 1, Cols + 1];

            for (i = 0; i <= Rows; i++)
                for (j = 0; j <= Cols; j++)
                    sol[i, j] = Mat[i, j] * Value;

            return sol;
        }

        /// <summary>
        /// Returns the multiplication of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to multiply the array</param>
        /// <param name="Mat">Matrix1 which is to be multiplied by a scalar</param>
        /// <returns>The multiplication of the scalar and the array as an array</returns>
        public static Matrix1 ScalarMultiply(double Value, Matrix1 Mat)
        { return new Matrix1(ScalarMultiply(Value, Mat.in_Mat)); }

        /// <summary>
        /// Returns the multiplication of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">Matrix1 object which is to be multiplied by a scalar</param>
        /// <param name="Value">The scalar value to multiply the Matrix1 object</param>
        /// <returns>
        /// The multiplication of the scalar and the Matrix1 object as a 
        /// Matrix1 object
        /// </returns>
        public static Matrix1 operator *(Matrix1 Mat, double Value)
        { return new Matrix1(ScalarMultiply(Value, Mat.in_Mat)); }

        /// <summary>
        /// Returns the multiplication of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) with a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to multiply the Matrix1 object</param>
        /// <param name="Mat">Matrix1 object which is to be multiplied by a scalar</param>
        /// <returns>
        /// The multiplication of the scalar and the Matrix1 object as a 
        /// Matrix1 object
        /// </returns>
        public static Matrix1 operator *(double Value, Matrix1 Mat)
        { return new Matrix1(ScalarMultiply(Value, Mat.in_Mat)); }
        #endregion

        #region "Divide a Matrix1 or a vector with a scalar quantity"
        /// <summary>
        /// Returns the division of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) by a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to divide the array with</param>
        /// <param name="Mat">Array which is to be divided by a scalar</param>
        /// <returns>The division of the array and the scalar as an array</returns>
        public static double[,] ScalarDivide(double Value, double[,] Mat)
        {
            int i, j, Rows, Cols;
            double[,] sol;

            try { Find_R_C(Mat, out Rows, out Cols); }
            catch { throw new Matrix1NullException(); }

            sol = new double[Rows + 1, Cols + 1];

            for (i = 0; i <= Rows; i++)
                for (j = 0; j <= Cols; j++)
                    sol[i, j] = Mat[i, j] / Value;

            return sol;
        }

        /// <summary>
        /// Returns the division of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) by a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to divide the array with</param>
        /// <param name="Mat">Matrix1 which is to be divided by a scalar</param>
        /// <returns>The division of the array and the scalar as an array</returns>
        public static Matrix1 ScalarDivide(double Value, Matrix1 Mat)
        { return new Matrix1(ScalarDivide(Value, Mat.in_Mat)); }

        /// <summary>
        /// Returns the division of a Matrix1 or a vector (i.e 
        /// dimension [3,1]) by a scalar quantity.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Value">The scalar value to divide the Matrix1 object with</param>
        /// <param name="Mat">Matrix1 object which is to be divided by a scalar</param>
        /// <returns>
        /// The division of the Matrix1 object and the scalar as a Matrix1 object
        /// </returns>
        public static Matrix1 operator /(Matrix1 Mat, double Value)
        { return new Matrix1(ScalarDivide(Value, Mat.in_Mat)); }
        #endregion

        #region "Vectors Cross Product"
        /// <summary>
        /// Returns the cross product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First vector array (dimension [3]) in the cross product</param>
        /// <param name="V2">Second vector array (dimension [3]) in the cross product</param>
        /// <returns>Cross product of V1 and V2 as an array (dimension [3])</returns>
        public static double[] CrossProduct(double[] V1, double[] V2)
        {
            double i, j, k;
            double[] sol = new double[2];
            int Rows1;
            int Rows2;

            try
            {
                Find_R_C(V1, out Rows1);
                Find_R_C(V2, out Rows2);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows1 != 2) throw new VectorDimensionException();

            if (Rows2 != 2) throw new VectorDimensionException();

            i = V1[1] * V2[2] - V1[2] * V2[1];
            j = V1[2] * V2[0] - V1[0] * V2[2];
            k = V1[0] * V2[1] - V1[1] * V2[0];

            sol[0] = i; sol[1] = j; sol[2] = k;

            return sol;
        }

        /// <summary>
        /// Returns the cross product of two vectors whose
        /// dimensions should be [3] or [3x1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First vector array (dimensions [3,1]) in the cross product</param>
        /// <param name="V2">Second vector array (dimensions [3,1]) in the cross product</param>
        /// <returns>Cross product of V1 and V2 as an array (dimension [3,1])</returns>
        public static double[,] CrossProduct(double[,] V1, double[,] V2)
        {
            double i, j, k;
            double[,] sol = new double[3, 1];
            int Rows1, Cols1;
            int Rows2, Cols2;

            try
            {
                Find_R_C(V1, out Rows1, out Cols1);
                Find_R_C(V2, out Rows2, out Cols2);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows1 != 2 || Cols1 != 0) throw new VectorDimensionException();

            if (Rows2 != 2 || Cols2 != 0) throw new VectorDimensionException();

            i = V1[1, 0] * V2[2, 0] - V1[2, 0] * V2[1, 0];
            j = V1[2, 0] * V2[0, 0] - V1[0, 0] * V2[2, 0];
            k = V1[0, 0] * V2[1, 0] - V1[1, 0] * V2[0, 0];

            sol[0, 0] = i; sol[1, 0] = j; sol[2, 0] = k;

            return sol;
        }

        /// <summary>
        /// Returns the cross product of two vectors whose
        /// dimensions should be [3] or [3x1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First Matrix1 (dimensions [3,1]) in the cross product</param>
        /// <param name="V2">Second Matrix1 (dimensions [3,1]) in the cross product</param>
        /// <returns>Cross product of V1 and V2 as a Matrix1 (dimension [3,1])</returns>
        public static Matrix1 CrossProduct(Matrix1 V1, Matrix1 V2)
        { return (new Matrix1((CrossProduct(V1.in_Mat, V2.in_Mat)))); }
        #endregion

        #region "Vectors Dot Product"
        /// <summary>
        /// Returns the dot product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First vector array (dimension [3]) in the dot product</param>
        /// <param name="V2">Second vector array (dimension [3]) in the dot product</param>
        /// <returns>Dot product of V1 and V2</returns>
        public static double DotProduct(double[] V1, double[] V2)
        {
            int Rows1;
            int Rows2;

            try
            {
                Find_R_C(V1, out Rows1);
                Find_R_C(V2, out Rows2);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows1 != 2) throw new VectorDimensionException();

            if (Rows2 != 2) throw new VectorDimensionException();

            return (V1[0] * V2[0] + V1[1] * V2[1] + V1[2] * V2[2]);
        }

        /// <summary>
        /// Returns the dot product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First vector array (dimension [3,1]) in the dot product</param>
        /// <param name="V2">Second vector array (dimension [3,1]) in the dot product</param>
        /// <returns>Dot product of V1 and V2</returns>
        public static double DotProduct(double[,] V1, double[,] V2)
        {
            int Rows1, Cols1;
            int Rows2, Cols2;

            try
            {
                Find_R_C(V1, out Rows1, out Cols1);
                Find_R_C(V2, out Rows2, out Cols2);
            }
            catch { throw new Matrix1NullException(); }

            if (Rows1 != 2 || Cols1 != 0) throw new VectorDimensionException();

            if (Rows2 != 2 || Cols2 != 0) throw new VectorDimensionException();

            return (V1[0, 0] * V2[0, 0] + V1[1, 0] * V2[1, 0] + V1[2, 0] * V2[2, 0]);
        }

        /// <summary>
        /// Returns the dot product of two vectors whose
        /// dimensions should be [3] or [3,1].
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V1">First Matrix1 object (dimension [3,1]) in the dot product</param>
        /// <param name="V2">Second Matrix1 object (dimension [3,1]) in the dot product</param>
        /// <returns>Dot product of V1 and V2</returns>
        public static double DotProduct(Matrix1 V1, Matrix1 V2)
        { return (DotProduct(V1.in_Mat, V2.in_Mat)); }
        #endregion

        #region "Magnitude of a Vector"
        /// <summary>
        ///  Returns the magnitude of a vector whose dimension is [3] or [3,1].
        ///  In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V">The vector array (dimension [3]) whose magnitude is to be found</param>
        /// <returns>The magnitude of the vector array</returns>
        public static double VectorMagnitude(double[] V)
        {
            int Rows;

            try { Find_R_C(V, out Rows); }
            catch { throw new Matrix1NullException(); }

            if (Rows != 2) throw new VectorDimensionException();

            return Math.Sqrt(V[0] * V[0] + V[1] * V[1] + V[2] * V[2]);
        }

        /// <summary>
        ///  Returns the magnitude of a vector whose dimension is [3] or [3,1].
        ///  In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V">The vector array (dimension [3,1]) whose magnitude is to be found</param>
        /// <returns>The magnitude of the vector array</returns>
        public static double VectorMagnitude(double[,] V)
        {
            int Rows, Cols;

            try { Find_R_C(V, out Rows, out Cols); }
            catch { throw new Matrix1NullException(); }

            if (Rows != 2 || Cols != 0) throw new VectorDimensionException();

            return Math.Sqrt(V[0, 0] * V[0, 0] + V[1, 0] * V[1, 0] + V[2, 0] * V[2, 0]);
        }

        /// <summary>
        ///  Returns the magnitude of a vector whose dimension is [3] or [3,1].
        ///  In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="V">Matrix1 object (dimension [3,1]) whose magnitude is to be found</param>
        /// <returns>The magnitude of the Matrix1 object</returns>
        public static double VectorMagnitude(Matrix1 V)
        { return (VectorMagnitude(V.in_Mat)); }
        #endregion

        #region "Two Matrices are equal or not"
        /// <summary>
        /// Checks if two Arrays of equal dimensions are equal or not.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First array in equality check</param>
        /// <param name="Mat2">Second array in equality check</param>
        /// <returns>If two matrices are equal or not</returns>
        public static bool IsEqual(double[,] Mat1, double[,] Mat2)
        {
            double eps = 1E-14;
            int Rows1, Cols1;
            int Rows2, Cols2;

            //Find The dimensions !!
            try
            {
                Find_R_C(Mat1, out Rows1, out Cols1);
                Find_R_C(Mat2, out Rows2, out Cols2);
            }
            catch
            {
                throw new Matrix1NullException();
            }
            if (Rows1 != Rows2 || Cols1 != Cols2) throw new Matrix1DimensionException();

            for (int i = 0; i <= Rows1; i++)
            {
                for (int j = 0; j <= Cols1; j++)
                {
                    if (Math.Abs(Mat1[i, j] - Mat2[i, j]) > eps) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if two matrices of equal dimensions are equal or not.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 in equality check</param>
        /// <param name="Mat2">Second Matrix1 in equality check</param>
        /// <returns>If two matrices are equal or not</returns>
        public static bool IsEqual(Matrix1 Mat1, Matrix1 Mat2)
        { return IsEqual(Mat1.in_Mat, Mat2.in_Mat); }

        /// <summary>
        /// Checks if two matrices of equal dimensions are equal or not.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 object in equality check</param>
        /// <param name="Mat2">Second Matrix1 object in equality check</param>
        /// <returns>If two matrices are equal or not</returns>
        public static bool operator ==(Matrix1 Mat1, Matrix1 Mat2)
        { return IsEqual(Mat1.in_Mat, Mat2.in_Mat); }

        /// <summary>
        /// Checks if two matrices of equal dimensions are not equal.
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat1">First Matrix1 object in equality check</param>
        /// <param name="Mat2">Second Matrix1 object in equality check</param>
        /// <returns>If two matrices are not equal</returns>
        public static bool operator !=(Matrix1 Mat1, Matrix1 Mat2)
        { return (!IsEqual(Mat1.in_Mat, Mat2.in_Mat)); }

        /// <summary>
        /// Tests whether the specified object is a Matrix1Library.Matrix1
        /// object and is identical to this Matrix1Library.Matrix1 object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>This method returns true if obj is the specified Matrix1 object identical to this Matrix1 object; otherwise, false.</returns>
        public override bool Equals(Object obj)
        {
            try { return (bool)(this == (Matrix1)obj); }
            catch { return false; }
        }
        #endregion

        #region "Print Matrix1"
        /// <summary>
        /// Returns a Matrix1 as a string, so it can be viewed
        /// in a multi-text textbox or in a richtextBox (preferred).
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">The array to be viewed</param>
        /// <returns>The string view of the array</returns>
        public static string PrintMat(double[,] Mat)
        {
            int N_Rows, N_Columns, k, i, j, m;
            string StrElem;
            int StrLen; int[] Greatest;
            string LarString, OptiString, sol;

            //Find The dimensions !!
            try { Find_R_C(Mat, out N_Rows, out N_Columns); }
            catch
            {
                throw new Matrix1NullException();
            }

            sol = "";
            LarString = "";
            OptiString = "";
            Greatest = new int[N_Columns + 1];

            for (i = 0; i <= N_Rows; i++)
            {
                for (j = 0; j <= N_Columns; j++)
                {
                    if (i == 0)
                    {
                        Greatest[j] = 0;
                        for (m = 0; m <= N_Rows; m++)
                        {
                            StrElem = Mat[m, j].ToString("0.0000");
                            StrLen = StrElem.Length;
                            if (Greatest[j] < StrLen)
                            {
                                Greatest[j] = StrLen;
                                LarString = StrElem;
                            }
                        }
                        if (LarString.StartsWith("-")) Greatest[j] = Greatest[j];
                    }
                    StrElem = Mat[i, j].ToString("0.0000");
                    if (StrElem.StartsWith("-"))
                    {
                        StrLen = StrElem.Length;
                        if (Greatest[j] >= StrLen)
                        {
                            for (k = 1; k <= (Greatest[j] - StrLen); k++) OptiString += "  ";
                            OptiString += " ";
                        }
                    }
                    else
                    {
                        StrLen = StrElem.Length;
                        if (Greatest[j] > StrLen)
                        {
                            for (k = 1; k <= (Greatest[j] - StrLen); k++)
                            {
                                OptiString += "  ";
                            }
                        }
                    }
                    OptiString += "  " + (Mat[i, j].ToString("0.0000"));
                }

                if (i != N_Rows)
                {
                    sol += OptiString + "\n";
                    OptiString = "";
                }
                sol += OptiString;
                OptiString = "";
            }
            return sol;
        }

        /// <summary>
        /// Returns a Matrix1 as a string, so it can be viewed
        /// in a multi-text textbox or in a richtextBox (preferred).
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <param name="Mat">The Matrix1 object to be viewed</param>
        /// <returns>The string view of the Matrix1 object</returns>
        public static string PrintMat(Matrix1 Mat)
        { return (PrintMat(Mat.in_Mat)); }

        /// <summary>
        /// Returns the Matrix1 as a string, so it can be viewed
        /// in a multi-text textbox or in a richtextBox (preferred).
        /// In case of an error the error is raised as an exception.
        /// </summary>
        /// <returns>The string view of the Matrix1 object</returns>
        public override string ToString()
        { return (PrintMat(this.in_Mat)); }

        #endregion


        #region "Ham chuyen dong thanh cot"
        public static double[,] Row_to_col(double[,] mat)
        {
            int m, n;
            m = mat.GetLength(0);
            n = mat.GetLength(1);
            double[,] res = new double[mat.GetLength(0), mat.GetLength(1)];
            int i, j;
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    res[j, i] = mat[i, j];
            return res;
        }
        #endregion

        internal static void Eigen(Matrix1 A, double[,] eig_val, double[,] eig_vec)
        {
            throw new NotImplementedException();
        }

        public static double[,] ConvertMatrix(double[] flat,int m,int n)
        {
           
            if (flat.Length != m * n)
            {
                throw new ArgumentException("Invalid length");
            }
            double[,] ret = new double[m, n];
            // BlockCopy uses byte lengths: a double is 8 bytes
            Buffer.BlockCopy(flat, 0, ret, 0, flat.Length * sizeof(double));
            return ret;
        }

        #region compare2maxtrix
        public static void Compare(List<Matrix1> matrix1s, List<string> labels, Matrix1 mtthem, out double max, out string name,out double avg,out string nameAvg)
        {
            int dem = 0;
            double[] avgs= new double[matrix1s.Count/10];
            double gt = 0,sum=0;
            name = ""; nameAvg = "";
            max = 0; avg = 0;
            for (int i = 0; i < matrix1s.Count; i++)
            {
               
                gt = Match.MatchO(matrix1s[i], mtthem);
                sum += gt; 
                if (gt.CompareTo(max) == 1)
                {
                    
                    max = gt;
                    name = labels[i];
                }
              
                
                if (i % 9 == 0 & i != 0)
                {
                    avgs[dem] = sum / 10;
                    sum = 0;
                    dem++;
                }
                if ((i - 1) % 9 == 0)
                {

                    
                }
            }
            //tim max trong mang cac phan tu trung binh
            double mavg = 0;
            for (int i = 0; i < matrix1s.Count / 10;i++ )
            {
                if (mavg < avgs[i])
                {
                    mavg = avgs[i];
                    nameAvg = labels[i];
                }
            }
            avg = mavg;
            if (max < 0.8) name = "Unknown";
            if (mavg < 0.7) nameAvg = "Unknown";
        }
        #endregion
    }
}
