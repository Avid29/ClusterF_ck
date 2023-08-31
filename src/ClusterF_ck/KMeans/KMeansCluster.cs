// Adam Dernis © 2022

using ClusterF_ck.IO;
using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Interfaces.Properties;
using CommunityToolkit.Diagnostics;
using System.Collections.Generic;

namespace ClusterF_ck.KMeans;

/// <summary>
/// A <see cref="Cluster{T}"/> implementation for KMeans.
/// </summary>
/// <typeparam name="T">The type of data in the cluster.</typeparam>
/// <typeparam name="TShape">A shape to describe to provide comparison methods for <typeparamref name="T"/>.</typeparam>
public class KMeansCluster<T, TShape> : Cluster<T>, ICentroidCluster<T>, IPointsCluster<T>, IWeightedCluster
    where TShape : struct, IDistanceSpace<T>, IAverageSpace<T>
{
    private T? _centroid;
    private bool _invalid;

    /// <summary>
    /// Initializes a new instance of the <see cref="KMeansCluster{T, TShape}"/> class.
    /// </summary>
    internal KMeansCluster()
    {
        _centroid = default;
    }

    // Cached with deferred calculation
    /// <inheritdoc/>
    public T? Centroid => _invalid ? _centroid = CalculateCentroid() : _centroid;

    /// <inheritdoc/>
    IReadOnlyCollection<T> IPointsCluster<T>.Points => Points;

    /// <summary>
    /// Gets a list of points in the cluster.
    /// </summary>
    internal List<T> Points { get; } = new();

    /// <inheritdoc/>
    public double Weight => Points.Count;

    /// <summary>
    /// Gets or sets an element at an index in the sub point list.
    /// </summary>
    /// <param name="i">The index of the element to get or set.</param>
    /// <returns>The element at index <paramref name="i"/> in the sub point list.</returns>
    public T this[int i]
    {
        get => Points[i];
        set => Points[i] = value;
    }

    /// <summary>
    /// Adds a point to the <see cref="KMeansCluster{T, TShape}"/>.
    /// </summary>
    /// <param name="item">The item to add.</param>
    internal void Add(T item)
    {
        Points.Add(item);

        // Reset centroid, but defer calculation
        _invalid = true;
    }

    /// <summary>
    /// Removes the point at <paramref name="index"/> from the sub point list.
    /// </summary>
    /// <param name="index">The index of the element to remove.</param>
    /// <returns>The removed item.</returns>
    internal T RemoveAt(int index)
    {
        T removed = Points[index];
        Points.RemoveAt(index);
        _invalid = true;
        return removed;
    }

    /// <summary>
    /// Gets the element in the cluster closest to the Centroid.
    /// </summary>
    /// <returns>The element in the cluster nearest the Centroid.</returns>
    public T GetNearestToCenter()
    {
        TShape shape = default;

        // Track nearest seen value and its index.
        double minimumDistance = double.PositiveInfinity;
        int nearestPointIndex = -1;

        for (int i = 0; i < Points.Count; i++)
        {
            T p = Points[i];

            // This should never happen because the cluster should never be empty.
            Guard.IsNotNull(Centroid);

            double distance = shape.FindDistanceSquared(p, Centroid);

            // Update tracking variables
            if (minimumDistance > distance)
            {
                minimumDistance = distance;
                nearestPointIndex = i;
            }
        }

        // return the value at the index with the largest value found
        return Points[nearestPointIndex];
    }

    /// <summary>
    /// Calculates the Centroid for the values in the cluster.
    /// </summary>
    /// <remarks>
    /// Calculates the centroid entirely from scratch ignoring <see cref="_centroid"/>.
    /// </remarks>
    /// <returns>The center point of all values in the cluster.</returns>
    private T CalculateCentroid()
    {
        TShape shape = default;
        return shape.Average(Points.ToArray());
    }
}