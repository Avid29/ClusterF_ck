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
    public class MSFixedTests : MSTestBase
    {
        [TestMethod("Double Test 1")]
        public void DoubleTest1()
        {
            var data = FixedSets.Double_Test1;
            var kernel = new GaussianKernel(5);

            Run<double, DoubleShape, GaussianKernel>(data, kernel);
        }

        [TestMethod("Vector2 Test 1")]
        public void Vector2Test1()
        {
            var data = FixedSets.Vector2_Test1;
            var kernel = new GaussianKernel(5);

            Run<Vector2, Vector2Shape, GaussianKernel>(data, kernel);
        }
    }
}
