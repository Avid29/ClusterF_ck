// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;
using ClusterF_ck.Tests.Data.DataSet.Abstract;
using KM = ClusterF_ck.KMeans.KMeans;

namespace ClusterF_ck.Tests.KMeans.Abstract
{
    public abstract class KMeansTestBase
    {
        protected void Run<T, TShape>(DataSet<T> data, int k, TShape shape = default)
            where T : unmanaged
            where TShape : struct, IDistanceSpace<T>, IAverageSpace<T>
        {
            // TODO: Verify results
            KM.Cluster<T, TShape>(data.Data, k, shape);
        }
    }
}
