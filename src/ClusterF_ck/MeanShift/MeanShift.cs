// Adam Dernis © 2022

using ClusterF_ck.Kernels;
using ClusterF_ck.Spaces.Properties;
using CommunityToolkit.HighPerformance;
using System.Collections.Generic;
using System;

namespace ClusterF_ck.MeanShift
{
    public static partial class MeanShift
    {
        #region ReadOnlySpan

        /// <summary>
        /// Clusters a set of points using MeanShift over a field.
        /// </summary>
        /// <remarks>
        /// It is usually wise to use <see cref="WeightedMeanShift.Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead unless all points are unique.
        /// Weighted MeanShift greatly reduces computation time for duplicate points.
        /// </remarks>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The points to shift until convergence.</param>
        /// <param name="field">The field of points to converge over.</param>
        /// <param name="kernel">The kernel to use for clustering.</param>
        /// <param name="shape">The shape to use on the points to cluster.</param>
        /// <returns>A list of clusters weighted by the contributing points.</returns>
        public static List<MeanShiftCluster<T, TShape>> Cluster<T, TShape, TKernel>(
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

        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static List<MeanShiftCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

        #endregion

        #region Array

        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static List<MeanShiftCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            T[] points,
            T[] field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), field.AsSpan(), kernel, shape);

        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static List<MeanShiftCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            T[] points,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), kernel, shape);

        #endregion

        #region List

#if NET6_0_OR_GREATER

        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static List<MeanShiftCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            List<T> points,
            List<T> field,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), field.AsSpan(), kernel, shape);

        /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
        public static List<MeanShiftCluster<T, TShape>> Cluster<T, TShape, TKernel>(
            List<T> points,
            TKernel kernel,
            TShape shape = default)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
            where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), kernel, shape);

#endif
        #endregion
    }
}
