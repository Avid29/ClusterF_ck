// Adam Dernis © 2022

using ClusterF_ck.Kernels;
using ClusterF_ck.MeanShift;
using ClusterF_ck.Spaces.Properties;
using ClusterF_ck.Tests.Data.DataSet.Abstract;
using MS = ClusterF_ck.MeanShift.MeanShift;
using WMS = ClusterF_ck.MeanShift.WeightedMeanShift;

namespace ClusterF_ck.Tests.MeanShift.Abstract
{
    public abstract class MSTestBase
    {
        protected List<MSCluster<T, TShape>> Run<T, TShape, TKernel>(DataSet<T> data, TKernel kernel, TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => MS.Cluster<T, TShape, TKernel>(data.Data, kernel, shape);

        protected void WeightedComparedRun<T, TShape, TKernel>(DataSet<T> data, TKernel kernel, TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            var uwResult = Run(data, kernel, shape);
            var wResult = WMS.Cluster<T, TShape, TKernel>(data.Data, kernel, shape);

            // TODO: Compare results
        }
    }
}
