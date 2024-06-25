// Adam Dernis © 2022

using ClusterF_ck.Kernels;
using ClusterF_ck.MeanShift;
using ClusterF_ck.Spaces.Properties;
using ClusterF_ck.Tests.Data.DataSet.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MS = ClusterF_ck.MeanShift.MeanShift;
using WMS = ClusterF_ck.MeanShift.WeightedMeanShift;

namespace ClusterF_ck.Tests.MeanShift.Abstract
{
    public abstract class MSTestBase
    {
        protected static List<MeanShiftCluster<T>>? Run<T, TShape, TKernel>(DataSet<T> data, TKernel kernel, TShape shape = default, bool expectFail = false)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            List<MeanShiftCluster<T>>? results = null;

            bool failed = false;
            try
            {
                results = MS.Cluster(data.Data, kernel, shape);
            }
            catch
            {
                failed = true;
            }

            Assert.AreEqual(expectFail, failed);
            return results;
        }

        protected void WeightedComparedRun<T, TShape, TKernel>(DataSet<T> data, TKernel kernel, TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            var uwResult = Run(data, kernel, shape);
            var wResult = WMS.Cluster(data.Data, kernel, shape);

            // TODO: Compare results
        }
    }
}
