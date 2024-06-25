// Adam Dernis © 2022

using ClusterF_ck.DBSCAN;
using ClusterF_ck.Spaces.Interfaces.Properties;
using ClusterF_ck.Tests.Data.DataSet.Abstract;
using DBS = ClusterF_ck.DBSCAN.DBSCAN;

namespace ClusterF_ck.Tests.DBSCAN.Abstract
{
    public abstract class DBSTestsBase
    {
        protected void Run<T, TShape>(DataSet<T> data, DBSConfig config, TShape shape = default)
            where T : unmanaged
            where TShape : struct, IDistanceSpace<T>
        {
            // TODO: Verify results
            DBS.Cluster<T, TShape>(data.Data, config, shape);
        }
    }
}
