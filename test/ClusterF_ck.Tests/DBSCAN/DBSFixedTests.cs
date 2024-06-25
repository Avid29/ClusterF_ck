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
    public class DBSFixedTests : DBSTestsBase
    {
        [TestMethod("Double Test 1")]
        public void DoubleTest1()
        {
            var data = FixedSets.Double_Test1;
            var config = new DBSConfig(2, 1, true);

            Run<double, DoubleShape>(data, config);
        }

        [TestMethod("Vector2 Test 1")]
        public void Vector2Test1()
        {
            var data = FixedSets.Vector2_Test1;
            var config = new DBSConfig(2, 1, true);

            Run<Vector2, Vector2Shape>(data, config);
        }
    }
}
