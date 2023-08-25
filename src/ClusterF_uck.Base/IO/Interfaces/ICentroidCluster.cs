// Adam Dernis © 2022

namespace ClusterF_ck.IO.Interfaces;

/// <summary>
/// An interface for Clusters that output with a Centroid.
/// </summary>
/// <typeparam name="T">The type of data in the cluster.</typeparam>
public interface ICentroidCluster<out T>
{
    /// <summary>
    /// Gets the center point of all points in the <see cref="ICentroidCluster{T}"/>.
    /// </summary>
    T? Centroid { get; }
}