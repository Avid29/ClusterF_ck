// Adam Dernis © 2022

using ClusterF_ck.DBSCAN;
using ClusterF_ck.Spaces;
using ClusterF_ck.Tests.Data.DataSet;
using ClusterF_ck.Tests.DBSCAN.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace ClusterF_ck.Tests.DBSCAN
{
    [TestClass]
    public class DBSGradientTests : DBSTestsBase
    {
        [TestMethod("1D Linear Gradient 101")]
        public void DoubleTest1()
        {
            var data = GradientSets.Linear1D_101;
            var config = new DBSConfig(.01, 2, true);

            Run<double, DoubleShape>(data, config);
        }

        [TestMethod("2D Linear Gradient 11x11")]
        public void Vector2Test1()
        {
            var data = GradientSets.Linear2D_11x11;
            var config = new DBSConfig(.2, 4, true);

            Run<Vector2, Vector2Shape>(data, config);
        }
    }
}
