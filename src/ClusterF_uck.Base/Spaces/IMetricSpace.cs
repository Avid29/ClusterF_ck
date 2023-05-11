// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;

namespace ClusterF_ck.Spaces;

/// <summary>
/// An <see cref="ISpace{T}"/> for non-geometric points in a cluster.
/// </summary>
/// <remarks>
/// Metric points are defined by having relative distances from each other, but no absolute position in an space.
/// </remarks>
/// <typeparam name="T">The type being wrapped by the implementation.</typeparam>
public interface IMetricSpace<T> : ISpace<T>, IDistanceSpace<T>, IEquatableSpace<T>
{
}