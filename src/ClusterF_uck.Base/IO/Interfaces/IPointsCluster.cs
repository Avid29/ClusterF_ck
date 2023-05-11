// Adam Dernis © 2022

using System.Collections.Generic;

namespace ClusterF_ck.IO.Interfaces;

/// <summary>
/// An interface for Clusters that output with a collection points.
/// </summary>
/// <typeparam name="T">The type of data in the cluster.</typeparam>
public interface IPointsCluster<out T>
    where T : unmanaged
{
    /// <summary>
    /// Gets an <see cref="IReadOnlyCollection{T}"/> of points in the cluster.
    /// </summary>
    IReadOnlyCollection<T> Points { get; }
}