// Adam Dernis © 2022

using ClusterF_ck.DBSCAN;
using Microsoft.Collections.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using ClusterF_ck.Spaces.Properties;
using ClusterF_ck.Kernels;
using ClusterF_ck.Shapes.Wrapper;
using DBS = ClusterF_ck.DBSCAN.DBSCAN;

namespace ClusterF_ck.MeanShift
{/// <summary>
 /// A static class containing Weighted Mean Shift methods.
 /// </summary>
    public static class WeightedMeanShift
    {
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
        /// Clusters a set of points using a weighted version of MeanShift over a field by merging equal points.
        /// </summary>
        /// <remarks>
        /// If all points are unique, it is wise to use <see cref="MeanShift.Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead since there's no duplicates to merge.
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
            where TKernel : struct, IKernel => Wrap<T, TShape>(ClusterRaw(points, field, kernel, shape));

        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{(T, double)}, ReadOnlySpan{(T, double)}, TKernel, TShape)"/>
        public static List<MSCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            ReadOnlySpan<(T, double)> points,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

        /// <summary>
        /// Clusters a set of weighted points using MeanShift over a weighted field.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The weighted points to shift until convergence.</param>
        /// <param name="field">The field of weighted points to converge over.</param>
        /// <param name="kernel">The kernel to use for clustering.</param>
        /// <param name="shape">The shape to use on the points to cluster.</param>
        /// <returns>A list of clusters weighted by the contributing points.</returns>
        public static List<MSCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            ReadOnlySpan<(T, double)> points,
            ReadOnlySpan<(T, double)> field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Wrap<T, TShape>(ClusterRaw(points, field, kernel, shape));

        /// <remarks>
        /// If all points are unique, it is wise to use <see cref="MeanShift.ClusterRaw{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape){T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead since there's no duplicates to merge.
        /// </remarks>
        /// <returns>An array clusters weighted by their contributing points.</returns>
        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static (T, double)[] ClusterRaw<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            ReadOnlySpan<T> field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => ClusterRaw(MakeWeighted(points), MakeWeighted(field), kernel, shape);

        /// <returns>An array clusters weighted by their contributing points.</returns>
        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{(T, double)}, ReadOnlySpan{(T, double)}, TKernel, TShape)"/>
        public static unsafe (T, double)[] ClusterRaw<T, TShape, TKernel>(
            ReadOnlySpan<(T, double)> points,
            ReadOnlySpan<(T, double)> field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            // Points will bed cloned into a modifiable list of clusters
            (T, double)[] clusters = new (T, double)[points.Length];

            // This array will be reused on every iteration
            // However we allocate it here once to save on allocation time and space
            (T, double)[] fieldWeights = new (T, double)[field.Length];

            fixed ((T, double)* p = points)
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    (T, double) point = points[i];
                    T clusterPoint = MeanShiftPoint(point.Item1, p, points.Length, shape, kernel, fieldWeights);
                    (T, double) cluster = (clusterPoint, point.Item2);
                    clusters[i] = cluster;
                }
            }

            return PostProcess(clusters, kernel, shape);
        }

        /// <summary>
        /// Merges a set of points into a list of weighted unique points.
        /// </summary>
        private static ReadOnlySpan<(T, double)> MakeWeighted<T>(ReadOnlySpan<T> points)
            where T : unmanaged, IEquatable<T>
        {
            // Merge equal points
            DictionarySlim<T, int> merged = new();
            foreach (var point in points) merged.GetOrAddValueRef(point)++;

            // Convert back to tuple array
            (T, double)[] weightedPoints = new (T, double)[merged.Count];
            int i = 0;
            foreach (var entry in merged)
            {
                weightedPoints[i] = (entry.Key, entry.Value);
                i++;
            }

            return weightedPoints;
        }

        /// <summary>
        /// Takes an array of points and tuples and converts them to <see cref="MSCluster{T, TShape}"/>s.
        /// </summary>
        private static List<MSCluster<T, TShape>> Wrap<T, TShape>((T, double)[] raw)
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

        /// <summary>
        /// Shifts a single point to its convergence point.
        /// </summary>
        private static unsafe T MeanShiftPoint<T, TShape, TKernel>(
            T cluster,
            (T, double)* field,
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
                    (T, double) point = field[i];
                    double distSqrd = shape.FindDistanceSquared(cluster, point.Item1);
                    double weight = kernel.WeightDistance(distSqrd);
                    fieldWeights[i] = (point.Item1, weight * point.Item2);
                }

                newCluster = shape.WeightedAverage(fieldWeights);
                changed = !shape.AreEqual(newCluster, cluster, ACCEPTED_ERROR);
                cluster = newCluster;
            }

            return cluster;
        }

        /// <summary>
        /// Merges converged points.
        /// </summary>
        private static (T, double)[] PostProcess<T, TShape, TKernel>(
            (T, double)[] clusters,
            TKernel kernel,
            TShape shape)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel
        {
            // Remove explict duplicate values.
            DictionarySlim<T, double> mergeMap = new();

            foreach (var cluster in clusters) mergeMap.GetOrAddValueRef(cluster.Item1) += cluster.Item2;

            // Connected componenents merge.
            // Because convergence may be imperfect, a minimum difference can be used to merge similar clusters.

            // Convert Dictionary to tuple list.
            (T, double)[] mergedCentroids = new (T, double)[mergeMap.Count];
            int i = 0;
            foreach (var value in mergeMap)
            {
                mergedCentroids[i] = (value.Key, value.Value);
                i++;
            }

            // Connected componenents merge using DBSCAN with a minPoints of 0.
            // Because convergence may be imperfect, a minimum difference can be used to merge similar clusters.
            // A wrapping shape must be used in order to cluster the weighted points.
            DBSConfig<(T, double), WrappingPairShape<T, double, TShape>> config = new(kernel.WindowSize, 0);
            WrappingPairShape<T, double, TShape> weightedShape = new(shape);
            var results = DBS.Cluster(mergedCentroids, config, weightedShape);

            // No components to merge
            if (mergedCentroids.Length == results.Count) return mergedCentroids;

            // Convert the DBSCAN clusters into centroids.
            mergedCentroids = new (T, double)[results.Count];
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
