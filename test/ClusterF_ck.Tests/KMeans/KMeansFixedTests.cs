// Adam Dernis © 2022

using ClusterF_ck.Shapes;
using ClusterF_ck.Tests.Data.DataSet;
using ClusterF_ck.Tests.KMeans.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace ClusterF_ck.Tests.KMeans
{
    [TestClass]
    public class KMeansFixedTests : KMeansTestBase
    {

        [TestMethod("Double Test 1")]
        public void DoubleTest1()
        {
            var data = FixedSets.Double_Test1;
            int k = 3;

            Run<double, DoubleShape>(data, k);
        }

        [TestMethod("Vector2 Test 1")]
        public void Vector2Test1()
        {
            var data = FixedSets.Vector2_Test1;
            int k = 2;

            Run<Vector2, Vector2Shape>(data, k);
        }
    }
}
