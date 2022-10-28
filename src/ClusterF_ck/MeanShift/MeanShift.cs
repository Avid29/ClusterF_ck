// Adam Dernis © 2022

using ClusterF_ck.DBSCAN;
using ClusterF_ck.Kernels;
using ClusterF_ck.Shapes.Wrapper;
using ClusterF_ck.Spaces.Properties;
using Microsoft.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DBS = ClusterF_ck.DBSCAN.DBSCAN;

namespace ClusterF_ck.MeanShift
{
    /// MeanShift finds clusters by moving clusters towards a convergence point.
    /// This initial position of a cluster is a clone of a coresponding point. If clusters share a position they can be merged into one.
    ///
    /// Mathematically, the convergence point can be found by graphing the distribution from each point.
    /// After summing these distributions, the nearest local maxima to a cluster's initial
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
    /// The clusters would be 1 and 4.5, because those are all the local maximas.
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
    public static class MeanShift
    {
        // This is the margin of error allowed to merge convergence points.
        private const double ACCEPTED_ERROR = 0.000005;


        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static List<MSCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

        /// <summary>
        /// Clusters a set of points using MeanShift over a field.
        /// </summary>
        /// <remarks>
        /// It is usually wise to use <see cref="WeightedMeanShift.Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead unless all points are unique.
        /// Weighted MeanShift greatly reduces computation time when dealing with duplicate points.
        /// </remarks>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The points to shift until convergence.</param>
        /// <param name="field">The field of points to converge over.</param>
        /// <param name="kernel">The kernel to use for clustering.</param>
        /// <param name="shape">The shape to use on the points to cluster.</param>
        /// <returns>A list of clusters weighted by the contributing points.</returns>
        public static List<MSCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            ReadOnlySpan<T> field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            // Take the regular raw cluster.
            (T, int)[] raw = ClusterRaw(points, field, kernel, shape);

            return Wrap<T, TShape>(raw);
        }

        /// <inheritdoc cref="ClusterRaw{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static unsafe (T, int)[] ClusterRaw<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            return ClusterRaw(points, points, kernel, shape);
        }

        /// <remarks>
        /// It is usually wise to use <see cref="WeightedMeanShift.ClusterRaw{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead unless all points are unique.
        /// Weighted MeanShift greatly reduces computation time when dealing with duplicate points.
        /// </remarks>
        /// <returns>An array of clusters weighted by the contributing points.</returns>
        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static unsafe (T, int)[] ClusterRaw<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            ReadOnlySpan<T> field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            // Points will bed cloned into a modifiable list of clusters
            T[] clusters = new T[points.Length];

            // This array will be reused on every iteration
            // However we allocate it here once to save on allocation time and space
            (T, double)[] fieldWeights = new (T, double)[field.Length];

            fixed (T* f = field)
            {
                // Shift each cluster to its convergence point.
                for (int i = 0; i < clusters.Length; i++)
                {
                    T point = points[i];
                    T cluster = MeanShiftPoint(point, f, points.Length, shape, kernel, fieldWeights);
                    clusters[i] = cluster;

                    // TODO: Track points in the cluster
                }
            }

            return PostProcess(clusters, kernel, shape);
        }

        /// <summary>
        /// Takes an array of points and tuples and converts them to <see cref="MSCluster{T, TShape}"/>s.
        /// </summary>
        private static List<MSCluster<T, TShape>> Wrap<T, TShape>((T, int)[] raw)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        {
            List<MSCluster<T, TShape>> clusters = new();
            foreach (var cluster in raw)
            {
                clusters.Add(new MSCluster<T, TShape>(cluster.Item1, cluster.Item2));
            }

            return clusters;
        }

        private static unsafe T MeanShiftPoint<T, TShape, TKernel>(
            T cluster,
            T* field,
            int fieldSize,
            TShape shape,
            TKernel kernel,
            (T, double)[] fieldWeights)
            where T : unmanaged
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            bool changed = true;

            Unsafe.SkipInit(out T newCluster);

            // Shift point until it converges
            while (changed)
            {
                // Determine weight of all field points from the cluster's current position.
                for (int i = 0; i < fieldSize; i++)
                {
                    T point = field[i];
                    double distSqrd = shape.FindDistanceSquared(cluster, point);
                    double weight = kernel.WeightDistance(distSqrd);
                    fieldWeights[i] = (point, weight);
                }

                newCluster = shape.WeightedAverage(fieldWeights);
                changed = !shape.AreEqual(newCluster, cluster, ACCEPTED_ERROR);
                cluster = newCluster;
            }

            return cluster;
        }

        private static (T, int)[] PostProcess<T, TShape, TKernel>(
            T[] clusters,
            TKernel kernel,
            TShape shape)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            // Remove explict duplicate values.
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

            // Connected componenents merge using DBSCAN with a minPoints of 0.
            // Because convergence may be imperfect, a minimum difference can be used to merge similar clusters.
            // A wrapping shape must be used inorder to cluster the weighted points.
            DBSConfig<(T, int), WrappingPairShape<T, int, TShape>> config = new(kernel.WindowSize, 0);
            WrappingPairShape<T, int, TShape> wrappingShape = new(shape);
            var results = DBS.Cluster(mergedCentroids, config, wrappingShape);

            // No components to merge
            if (mergedCentroids.Length == results.Count) return mergedCentroids;

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
}
