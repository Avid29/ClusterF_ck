// Adam Dernis © 2023

using ClusterF_ck.DBSCAN;
using ClusterF_ck.Kernels;
using ClusterF_ck.Shapes.Wrapper;
using ClusterF_ck.Spaces.Properties;
using Microsoft.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DBS = ClusterF_ck.DBSCAN.DBSCAN;

namespace ClusterF_ck.MeanShift;

/// MeanShift finds clusters by moving clusters towards a convergence point.
/// This initial position of a cluster is a clone of a corresponding point. If clusters share a position they can be merged into one.
///
/// Mathematically, the convergence point can be found by graphing the distribution from each point.
/// After summing these distributions, the nearest local maximum to a cluster's initial
/// position is that cluster's convergence point.
///
/// Clusters at 1, 4, 4.5 and 5. Overlayed
///
///           *                             *    *    *
///         *   *                         *   **   **   *
///       *       *                     *    *  * *  *    *
///     *           *                 *    *    * *    *    *
///   *               *             *    *    *     *    *    *
/// *                   *         *    *    *         *    *    *
/// 0 - - - - 1 - - - - 2 - - - - 3 - - - - 4 - - - - 5 - - - - 6
///           ·                             ·    ·    ·
///
/// Clusters at 1, 4, 4.5 and 5. Summed
///
///                                              *
///                                            *   *
///                                          *       *
///                                         *         *
///                                         *         *
///                                        *           *
///                                       *             *
///           *                          *               *
///         *   *                       *                 *
///       *       *                    *                   *
///     *           *                 *                     *
///   *               *             *                         *
/// *                   *         *                             *
/// 0 - - - - 1 - - - - 2 - - - - 3 - - - - 4 - - - - 5 - - - - 6
///           ·                             ·    ·    ·
///
/// The clusters would be 1 and 4.5, because those are all the local maximums.
/// The clusters weighted would be (1, 1) and (4.5, 3) because 1 point went to the local max at 1 and 3 points went to the local max at 3.
///
///
/// Programmatically, these clusters are found by continually shifting each cluster towards their convergence point.
/// Each shift is performed by finding the cluster's distance from each point then weighting its effect on the cluster.
/// These weights are then used to find a weighted average, the result of each is the new cluster position.
/// 
/// Floating point errors prevent the clusters from perfectly converging. As a result Connected Components will be used to merge
/// similar clusters after convergence.


/// <summary>
/// A static class containing Mean Shift methods.
/// </summary>
public static partial class MeanShift
{
    // This is the margin of error allowed to merge convergence points.
    private const double ACCEPTED_ERROR = 0.000005;

    /// <remarks>
    /// It is usually wise to use <see cref="WeightedMeanShift.ClusterRaw{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead unless all points are unique.
    /// Weighted MeanShift greatly reduces computation time when dealing with duplicate points.
    /// </remarks>
    /// <returns>An array of clusters weighted by the contributing points.</returns>
    /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
    internal static (T, int)[] ClusterRaw<T, TShape, TKernel>(
        ReadOnlySpan<T> points,
        ReadOnlySpan<T> field,
        TKernel kernel,
        TShape shape = default)
        where T : IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel
    {
        // Points will bed cloned into a modifiable list of clusters
        T[] clusters = new T[points.Length];

        // This array will be reused on every iteration
        // However we allocate it here once to save on allocation time and space
        (T, double)[] fieldWeights = new (T, double)[field.Length];
        
            // Shift each cluster to its convergence point.
            for (int i = 0; i < clusters.Length; i++)
            {
                T point = points[i];
                T cluster = MeanShiftPoint(point, field, shape, kernel, fieldWeights);
                clusters[i] = cluster;

                // TODO: Track points in the cluster
            }

        return PostProcess<T, TShape, TKernel>(clusters.AsSpan(), kernel, shape);
    }

    /// <summary>
    /// Takes an array of points and tuples and converts them to <see cref="MeanShiftCluster{T,TShape}"/>s.
    /// </summary>
    private static List<MeanShiftCluster<T, TShape>> Wrap<T, TShape>((T, int)[] raw)
        where T : IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
    {
        List<MeanShiftCluster<T, TShape>> clusters = new();
        foreach (var cluster in raw)
        {
            clusters.Add(new MeanShiftCluster<T, TShape>(cluster.Item1, cluster.Item2));
        }

        return clusters;
    }

    private static T MeanShiftPoint<T, TShape, TKernel>(
        T cluster,
        ReadOnlySpan<T> field,
        TShape shape,
        TKernel kernel,
        (T, double)[] fieldWeights)
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel
    {
        bool changed = true;

        Unsafe.SkipInit(out T newCluster);

        // Shift point until it converges
        while (changed)
        {
            // Determine weight of all field points from the cluster's current position.
            for (int i = 0; i < field.Length; i++)
            {
                T point = field[i];
                double distanceSquared = shape.FindDistanceSquared(cluster, point);
                double weight = kernel.WeightDistance(distanceSquared);
                fieldWeights[i] = (point, weight);
            }

            newCluster = shape.WeightedAverage(fieldWeights);
            changed = !shape.AreEqual(newCluster, cluster, ACCEPTED_ERROR);
            cluster = newCluster;
        }

        return cluster;
    }

    private static (T, int)[] PostProcess<T, TShape, TKernel>(
        ReadOnlySpan<T> clusters,
        TKernel kernel,
        TShape shape)
        where T : IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel
    {
        // Remove explicit duplicate values.
        DictionarySlim<T, int> mergeMap = new();
        foreach (var cluster in clusters)
        {
            mergeMap.GetOrAddValueRef(cluster)++;
        }

        // Convert Dictionary to tuple list.
        (T, int)[] mergedCentroids = new (T, int)[mergeMap.Count];
        int i = 0;
        foreach (var value in mergeMap)
        {
            mergedCentroids[i] = (value.Key, value.Value);
            i++;
        }

        // Connected components merge using DBSCAN with a minPoints of 0.
        // Because convergence may be imperfect, a minimum difference can be used to merge similar clusters.
        // A wrapping shape must be used in order to cluster the weighted points.
        DBSConfig config = new(kernel.WindowSize, 0);
        WrappingPairShape<T, int, TShape> wrappingShape = new(shape);
        var results = DBS.Cluster<(T, int), WrappingPairShape<T, int, TShape>>(mergedCentroids, config, wrappingShape);

        // No components to merge
        if (mergedCentroids.Length == results.Count)
            return mergedCentroids;

        // Convert the DBSCAN clusters into centroids.
        mergedCentroids = new (T, int)[results.Count];
        for (i = 0; i < results.Count; i++)
        {
            // Track the weight of each point the DBSCAN cluster
            double weightSum = 0;

            // Pull the points from the DBSCAN cluster in order to take the average.
            (T, double)[] points = new (T, double)[results[i].Points.Count];
            for (int j = 0; j < results[i].Points.Count; j++)
            {
                points[j] = results[i].Points[j];
                weightSum += results[i].Points[j].Item2;
            }

            // Cache the weighted average of the points in the DBSCAN cluster
            // and the sum of their weights
            T point = shape.WeightedAverage(points);
            mergedCentroids[i] = (point, (int)weightSum);
        }

        return mergedCentroids;
    }
}