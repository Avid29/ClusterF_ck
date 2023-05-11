// Adam Dernis © 2022

using ClusterF_ck.IO;
using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Properties;

namespace ClusterF_ck.MeanShift;

/// <summary>
/// A <see cref="Cluster{T, TShape}"/> implementation for the MeanShift algorithm.
/// </summary>
/// <typeparam name="T">The type of points in the cluster.</typeparam>
/// <typeparam name="TShape">A shape to describe to provide comparison methods for <typeparamref name="T"/>.</typeparam>
public class MeanShiftCluster<T, TShape> : Cluster<T, TShape>, ICentroidCluster<T>, IWeightedCluster
    where T : unmanaged
    where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MeanShiftCluster{T,TShape}"/> class.
    /// </summary>
    /// <param name="centroid">The centroid of the <see cref="MeanShiftCluster{T,TShape}"/>.</param>
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