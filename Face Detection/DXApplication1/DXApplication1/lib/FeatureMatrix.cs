using System;
using System.Collections.Generic;
using System.Text;

namespace DXApplication1
{
    public class FeatureMatrix
    {
        public Matrix1[] mt_eigen_vec;
        int n;
        public FeatureMatrix()
        {
            mt_eigen_vec = new Matrix1[PCA.TT];
            n = 0;
        }
        public void add(Matrix1 mat)
        {
            mt_eigen_vec[n] = new Matrix1(mat);
            n++;
        }

    }
}
