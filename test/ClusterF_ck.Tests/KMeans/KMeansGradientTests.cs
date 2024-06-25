// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using ClusterF_ck.Tests.Data.DataSet;
using ClusterF_ck.Tests.KMeans.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace ClusterF_ck.Tests.KMeans
{
    [TestClass]
    public class KMeansGradientTests : KMeansTestBase
    {
        [TestMethod("1D Linear Gradient 101")]
        public void DoubleTest1()
        {
            var data = GradientSets.Linear1D_101;
            int k = 3;

            Run<double, DoubleShape>(data, k);
        }

        [TestMethod("2D Linear Gradient 11x11")]
        public void Vector2Test1()
        {
            var data = GradientSets.Linear2D_11x11;
            int k = 4;

            Run<Vector2, Vector2Shape>(data, k);
        }
    }
}
