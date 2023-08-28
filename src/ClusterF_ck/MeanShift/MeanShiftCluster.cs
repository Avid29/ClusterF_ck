// Adam Dernis © 2022

using ClusterF_ck.IO;
using ClusterF_ck.IO.Interfaces;

namespace ClusterF_ck.MeanShift;

/// <summary>
/// A <see cref="Cluster{T}"/> implementation for the MeanShift algorithm.
/// </summary>
/// <typeparam name="T">The type of points in the cluster.</typeparam>
public class MeanShiftCluster<T> : Cluster<T>, ICentroidCluster<T>, IWeightedCluster
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MeanShiftCluster{T}"/> class.
    /// </summary>
    /// <param name="centroid">The centroid of the <see cref="MeanShiftCluster{T}"/>.</param>
    /// <param name="weight">The total weight of, or summed point count for the cluster.</param>
    internal MeanShiftCluster(T centroid, double weight)
    {
        Centroid = centroid;
        Weight = weight;
    }

    /// <inheritdoc/>
    public T Centroid { get; }

    /// <inheritdoc/>
    public double Weight { get; }
}