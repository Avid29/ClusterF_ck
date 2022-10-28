// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;

namespace ClusterF_ck.Spaces
{
    /// <summary>
    /// An <see cref="ISpace{T}"/> for geometric points in a cluster.
    /// </summary>
    /// <remarks>
    /// Geometric points are defined by their absolute positions in space.
    /// </remarks>
    /// <typeparam name="T">The type being wrapped by the implementation.</typeparam>
    public interface IGeometricSpace<T> : ISpace<T>, IMetricSpace<T>, IAverageSpace<T>, IWeightedAverageSpace<T>
    {
    }
}
