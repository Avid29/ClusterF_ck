// Adam Dernis © 2022

using ClusterF_ck.Kernels;
using ClusterF_ck.Shapes;
using ClusterF_ck.Tests.Data.DataSet;
using ClusterF_ck.Tests.MeanShift.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace ClusterF_ck.Tests.MeanShift
{
    [TestClass]
    public class MSGradientTests : MSTestBase
    {
        [TestMethod("1D Linear Gradient 101")]
        public void DoubleTest1()
        {
            var data = GradientSets.Linear1D_101;
            var kernel = new GaussianKernel(5);

            Run<double, DoubleShape, GaussianKernel>(data, kernel);
        }

        [TestMethod("2D Linear Gradient 11x11")]
        public void Vector2Test1()
        {
            var data = GradientSets.Linear2D_11x11;
            var kernel = new GaussianKernel(5);

            Run<Vector2, Vector2Shape, GaussianKernel>(data, kernel);
        }
    }
}
