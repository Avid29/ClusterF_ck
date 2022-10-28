// Adam Dernis © 2022

using ClusterF_ck.Spaces;

namespace ClusterF_ck.IO
{
    /// <summary>
    /// The base class for clusters.
    /// </summary>
    public abstract class Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, ISpace<T>
    {
    }
}
